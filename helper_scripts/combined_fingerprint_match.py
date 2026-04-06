"""
Combined caller+callee fingerprint matching.

For each K1 unmatched and TSL FUN_xxx, build a combined fingerprint from:
  - Named functions that call it (caller names)
  - Named functions it calls (callee names)

Uses Jaccard similarity on the union set of both directions.
This gives more discriminating fingerprints than either direction alone.

Min requirements: at least MIN_COMBINED entries in the combined set.
Only accepts unique best-match pairs with clear score margin.

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_combined_fingerprint_matches.json
"""

from __future__ import annotations

import json

MIN_COMBINED = 3     # minimum entries in combined (callers + callees)
MIN_JACCARD = 0.40   # minimum Jaccard to accept
SCORE_MARGIN = 0.15  # clear margin over second-best

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_combined_fingerprint_matches.json"


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def build_caller_map(cg_data):
    caller_map = {}
    for f in cg_data["functions"]:
        caller = f["a"]
        for callee in f.get("c", []):
            caller_map.setdefault(callee, []).append(caller)
    return caller_map


def jaccard(a, b):
    if not a and not b:
        return 0.0
    inter = len(a & b)
    union = len(a | b)
    return inter / union if union > 0 else 0.0


def main():
    k1cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    k1_addr_name = {f["a"]: ((f.get("ns","")+"::"+f["n"]) if f.get("ns") else f["n"]) for f in k1cg["functions"]}
    tsl_addr_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    matched_k1 = {parse_hex(m["k1_addr"]) for m in cascade["matches"]}
    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cascade["matches"]}

    k1_callers = build_caller_map(k1cg)
    tsl_callers = build_caller_map(tsl_cg)

    def build_fp(addr, addr_name_map, callers_map, cg_func):
        """Combined caller + callee named fingerprint."""
        # Callees
        callees = set(
            addr_name_map[c] for c in cg_func.get("c", [])
            if c in addr_name_map and not addr_name_map[c].startswith("FUN_")
        )
        # Callers (use name prefix "~" to distinguish from callees)
        callers = set(
            "~" + addr_name_map[c] for c in callers_map.get(addr, [])
            if c in addr_name_map and not addr_name_map[c].startswith("FUN_")
        )
        return callees | callers

    # K1 candidates
    k1_idx = {f["a"]: f for f in k1cg["functions"]}
    k1_candidates = []
    for f in k1cg["functions"]:
        ka = f["a"]
        if ka in matched_k1:
            continue
        k1_name = f["n"]
        if k1_name.startswith("FUN_"):
            continue
        fp = build_fp(ka, k1_addr_name, k1_callers, f)
        if len(fp) < MIN_COMBINED:
            continue
        k1_candidates.append((ka, k1_name, fp, f.get("s", 0)))

    print("K1 unmatched with >={} combined entries: {}".format(MIN_COMBINED, len(k1_candidates)))

    # TSL FUN_xxx candidates
    tsl_candidates = []
    for f in tsl_cg["functions"]:
        ta = f["a"]
        if ta in matched_tsl:
            continue
        if not f["n"].startswith("FUN_"):
            continue
        fp = build_fp(ta, tsl_addr_name, tsl_callers, f)
        if len(fp) < MIN_COMBINED:
            continue
        tsl_candidates.append((ta, fp, f.get("s", 0)))

    print("TSL FUN_xxx with >={} combined entries: {}".format(MIN_COMBINED, len(tsl_candidates)))

    # Greedy best-match with score margin
    proposed = {}       # tsl_addr -> (k1_addr, score, k1_name)
    conflict_tsl = set()
    assigned_k1 = {}    # k1_addr -> (tsl_addr, score)

    for (ta, tsl_fp, tsl_size) in tsl_candidates:
        best_score = -1.0
        second_score = -1.0
        best_k1 = None
        best_k1_name = ""

        for (ka, k1_name, k1_fp, k1_size) in k1_candidates:
            if ka in matched_k1:
                continue
            score = jaccard(tsl_fp, k1_fp)
            if score >= MIN_JACCARD:
                if score > best_score:
                    second_score = best_score
                    best_score = score
                    best_k1 = ka
                    best_k1_name = k1_name
                elif score > second_score:
                    second_score = score

        if best_k1 is None:
            continue
        if best_score - second_score < SCORE_MARGIN:
            continue

        if best_k1 in assigned_k1:
            prev_ta, prev_score = assigned_k1[best_k1]
            if prev_score >= best_score:
                conflict_tsl.add(ta)
                continue
            else:
                conflict_tsl.add(prev_ta)
        assigned_k1[best_k1] = (ta, best_score)
        proposed[ta] = (best_k1, best_score, best_k1_name)

    new_matches = []
    for ta, (ka, score, k1_name) in proposed.items():
        if ta in conflict_tsl:
            continue

        if "::" in k1_name:
            parts = k1_name.rsplit("::", 1)
            class_field = parts[0]
            method_field = parts[1]
        else:
            class_field = ""
            method_field = k1_name

        new_matches.append({
            "k1_addr": "0x{:08X}".format(ka),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": k1_name,
            "class": class_field,
            "method": method_field,
            "via_k1": "combined_fp_jaccard:{:.2f}".format(score),
        })

    print("Proposed matches: {}  Conflicts: {}".format(len(new_matches), len(conflict_tsl)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(new_matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(new_matches), OUT))


if __name__ == "__main__":
    main()
