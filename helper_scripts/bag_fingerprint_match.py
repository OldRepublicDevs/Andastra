"""
Bag fingerprint matching for n=2..6 bags where size ordering is ambiguous.

For each matched (K1_parent, TSL_parent) pair where:
  - Both have the same number of unmatched callees (bag size n=2..MAX_N)
  - Size-ordering is ambiguous (equal callee counts)

Use callee-name fingerprints to assign the best alignment:
  - For each K1 callee Ci: fingerprint = frozenset of named callees of Ci
  - For each TSL callee Ti: fingerprint = frozenset of K1-translated names of Ti's callees
  - Score pairs by |fp(Ci) ∩ fp(Ti)| / max(|fp(Ci)|, |fp(Ti)|) (Jaccard-like)
  - Accept pair only if:
    * The best assignment gives every pair a score > 0
    * The assignment is unique (greedy: best scored pair wins, no conflicts)

Minimum FP overlap score: MIN_OVERLAP (fraction of shared names).
Require at least MIN_FP_SIZE named callees in at least one side of each pair.

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_bag_fingerprint_matches.json
"""

from __future__ import annotations

import json
from collections import defaultdict

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_bag_fingerprint_matches.json"

MAX_N = 6          # max bag size to attempt
MIN_FP_SIZE = 1    # minimum named callees in one side for a pair to be trustworthy
MIN_OVERLAP = 0.1  # minimum Jaccard score (at least 1 shared callee name)


def main():
    k1cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    matched_k1 = {int(m["k1_addr"], 16) for m in cascade["matches"]}
    matched_tsl = {int(m["tsl_addr"], 16) for m in cascade["matches"]}
    tsl_to_k1 = {int(m["tsl_addr"], 16): int(m["k1_addr"], 16) for m in cascade["matches"]}
    k1_to_tsl = {int(m["k1_addr"], 16): int(m["tsl_addr"], 16) for m in cascade["matches"]}
    k1_to_match = {int(m["k1_addr"], 16): m for m in cascade["matches"]}

    k1_callees = {f["a"]: list(f.get("c", [])) for f in k1cg["functions"]}
    tsl_callees = {f["a"]: list(f.get("c", [])) for f in tsl_cg["functions"]}
    k1_addr_to_fn = {f["a"]: f for f in k1cg["functions"]}
    tsl_addr_to_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}
    k1_addr_to_name = {f["a"]: f["n"] for f in k1cg["functions"]}

    # Build fingerprints for K1 functions: frozenset of named callees
    def k1_fp(ka):
        names = set()
        for c in k1_callees.get(ka, []):
            n = k1_addr_to_name.get(c, "")
            if n and not n.startswith("FUN_"):
                names.add(n)
        return frozenset(names)

    # Build fingerprints for TSL functions: translate callees via cascade then get K1 names
    def tsl_fp(ta):
        names = set()
        for c in tsl_callees.get(ta, []):
            k1c = tsl_to_k1.get(c)
            if k1c is not None:
                n = k1_addr_to_name.get(k1c, "")
                if n and not n.startswith("FUN_"):
                    names.add(n)
            else:
                # TSL callee is unmatched; fall back to TSL name if not FUN_xxx
                n = tsl_addr_to_name.get(c, "")
                if n and not n.startswith("FUN_"):
                    names.add(n)
        return frozenset(names)

    def jaccard(a, b):
        if not a and not b:
            return 0.0
        inter = len(a & b)
        union = len(a | b)
        return inter / union if union > 0 else 0.0

    # Find all matched parent pairs
    matched_pairs = []
    for m in cascade["matches"]:
        ka = int(m["k1_addr"], 16)
        ta = int(m["tsl_addr"], 16)
        matched_pairs.append((ka, ta, m))

    tsl_fun_addrs = {f["a"] for f in tsl_cg["functions"] if f["n"].startswith("FUN_")}

    stats = {"parents_checked": 0, "size_ok": 0, "fp_assigned": 0, "conflicts": 0}
    proposed = {}     # tsl_addr -> match entry
    conflict_tsl = set()
    conflict_k1 = set()

    for (parent_k1, parent_tsl, parent_m) in matched_pairs:
        k1_unmatched = [c for c in k1_callees.get(parent_k1, [])
                        if c not in matched_k1]
        tsl_unmatched = [c for c in tsl_callees.get(parent_tsl, [])
                         if c not in matched_tsl and c in tsl_fun_addrs]

        n = len(k1_unmatched)
        if n != len(tsl_unmatched) or n < 2 or n > MAX_N:
            continue

        stats["parents_checked"] += 1

        # Check if parent_callee_elimination would already handle this (dist sizes)
        k1_sizes = [len(k1_callees.get(c, [])) for c in k1_unmatched]
        tsl_sizes = [len(tsl_callees.get(c, [])) for c in tsl_unmatched]
        if len(set(k1_sizes)) == n and len(set(tsl_sizes)) == n:
            # All distinct sizes — parent_callee_elimination handles this
            continue

        stats["size_ok"] += 1

        # Build fingerprint matrix
        k1_fps = [(c, k1_fp(c)) for c in k1_unmatched]
        tsl_fps = [(c, tsl_fp(c)) for c in tsl_unmatched]

        # Check that we have useful fps
        any_useful = any(len(fp) >= MIN_FP_SIZE for _, fp in k1_fps + tsl_fps)
        if not any_useful:
            continue

        # Score all pairings
        score_matrix = []
        for ki, (ka, kfp) in enumerate(k1_fps):
            for ti, (ta, tfp) in enumerate(tsl_fps):
                score = jaccard(kfp, tfp)
                score_matrix.append((score, ki, ti, ka, ta))

        score_matrix.sort(reverse=True)

        # Greedy assignment
        assigned_k = set()
        assigned_t = set()
        assignments = []
        for score, ki, ti, ka, ta in score_matrix:
            if ki in assigned_k or ti in assigned_t:
                continue
            if score < MIN_OVERLAP:
                break  # remaining scores are all lower
            assigned_k.add(ki); assigned_t.add(ti)
            assignments.append((ka, ta, score))
            if len(assignments) == n:
                break

        if len(assignments) != n:
            continue  # couldn't assign all bags

        # All scores must be > 0
        if any(s < MIN_OVERLAP for _, _, s in assignments):
            continue

        stats["fp_assigned"] += 1

        # Propose all n pairs
        for ka, ta, score in assignments:
            k1_fn = k1_addr_to_fn.get(ka, {})
            k1_name = k1_fn.get("n", "")
            if not k1_name or k1_name.startswith("FUN_"):
                continue  # K1 unnamed

            if ka in conflict_k1 or ta in conflict_tsl:
                stats["conflicts"] += 1
                continue

            # Check for existing conflicts
            existing_ka_for_ta = int(proposed[ta]["k1_addr"], 16) if ta in proposed else None
            if existing_ka_for_ta is not None and existing_ka_for_ta != ka:
                conflict_tsl.add(ta); del proposed[ta]
                stats["conflicts"] += 1; continue

            existing_ta_for_ka = next((t for t, e in proposed.items() if int(e["k1_addr"], 16) == ka), None)
            if existing_ta_for_ka is not None and existing_ta_for_ka != ta:
                conflict_k1.add(ka)
                del proposed[existing_ta_for_ka]
                stats["conflicts"] += 1; continue

            k1_ns = k1_fn.get("ns", "")
            if "::" in k1_name:
                parts = k1_name.rsplit("::", 1)
                class_f = parts[0]; method_f = parts[1]
            elif k1_ns:
                class_f = k1_ns; method_f = k1_name
            else:
                class_f = ""; method_f = k1_name

            proposed[ta] = {
                "k1_addr": "0x{:08X}".format(ka),
                "tsl_addr": "0x{:08X}".format(ta),
                "name": k1_name,
                "class": class_f,
                "method": method_f,
                "via_k1": "bag_fp:score={:.2f}:parent_{}".format(score, parent_m.get("name", "?")),
            }

    matches = list(proposed.values())
    print("Parents checked (same bag size): {}".format(stats["parents_checked"]))
    print("Size-ambiguous bags attempted: {}".format(stats["size_ok"]))
    print("Bags fully assigned by fp: {}".format(stats["fp_assigned"]))
    print("Conflicts skipped: {}".format(stats["conflicts"]))
    print("New matches found: {}".format(len(matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
