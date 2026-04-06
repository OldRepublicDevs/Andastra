"""
Partial callee Jaccard matching.

For TSL FUN_xxx with >= MIN_CALLEES named callees, compute Jaccard similarity
against K1 unmatched functions. Accept unique best-match pairs with similarity
>= MIN_JACCARD.

Unlike callee_name_match.py (which requires exact frozenset match), this handles
cases where one side has extra callees that are still FUN_xxx, as long as the
named callee overlap is strong enough.

Disambiguation: if multiple K1 candidates have equal best Jaccard score, enforce
uniqueness by using instruction count (size) as a tiebreaker. Only accept if the
size-matched candidate is unique or has a clearly better score.

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_partial_callee_matches.json
"""

from __future__ import annotations

import json

MIN_CALLEES = 2      # minimum named callees needed
MIN_JACCARD = 0.40   # minimum Jaccard similarity to accept a match
SCORE_MARGIN = 0.12  # second-best must be at least this much worse to accept

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_partial_callee_matches.json"


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


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

    # K1: build callee fingerprints for unmatched functions
    k1_candidates = []
    for f in k1cg["functions"]:
        ka = f["a"]
        if ka in matched_k1:
            continue
        k1_name = f["n"]
        if k1_name.startswith("FUN_"):
            continue  # K1 should be fully named — skip if not
        named = frozenset(
            k1_addr_name[c] for c in f.get("c", [])
            if c in k1_addr_name and not k1_addr_name[c].startswith("FUN_")
        )
        if len(named) < MIN_CALLEES:
            continue
        k1_candidates.append((ka, k1_name, named, f.get("s", 0)))

    print("K1 unmatched with >={} named callees: {}".format(MIN_CALLEES, len(k1_candidates)))

    # TSL: find FUN_xxx with enough named callees
    tsl_candidates = []
    for f in tsl_cg["functions"]:
        ta = f["a"]
        if ta in matched_tsl:
            continue
        if not f["n"].startswith("FUN_"):
            continue
        named = frozenset(
            tsl_addr_name[c] for c in f.get("c", [])
            if c in tsl_addr_name and not tsl_addr_name[c].startswith("FUN_")
        )
        if len(named) < MIN_CALLEES:
            continue
        tsl_candidates.append((ta, named, f.get("s", 0)))

    print("TSL FUN_xxx with >={} named callees: {}".format(MIN_CALLEES, len(tsl_candidates)))

    # For each TSL candidate, score against all K1 candidates
    proposed = {}       # tsl_addr -> (k1_addr, score, k1_name, tsl_size, k1_size)
    conflict_tsl = set()
    conflict_k1 = set()
    assigned_k1 = {}    # k1_addr -> (tsl_addr, score)

    for (ta, tsl_named, tsl_size) in tsl_candidates:
        best_score = -1.0
        second_score = -1.0
        best_k1 = None
        best_k1_name = ""
        best_k1_size = 0

        for (ka, k1_name, k1_named, k1_size) in k1_candidates:
            if ka in matched_k1:
                continue
            score = jaccard(tsl_named, k1_named)
            if score >= MIN_JACCARD:
                if score > best_score:
                    second_score = best_score
                    best_score = score
                    best_k1 = ka
                    best_k1_name = k1_name
                    best_k1_size = k1_size
                elif score > second_score:
                    second_score = score

        if best_k1 is None:
            continue
        if best_score - second_score < SCORE_MARGIN:
            continue  # too ambiguous

        # Check if K1 candidate already has a better TSL assignment
        if best_k1 in assigned_k1:
            prev_ta, prev_score = assigned_k1[best_k1]
            if prev_score >= best_score:
                conflict_tsl.add(ta)
                continue
            else:
                conflict_tsl.add(prev_ta)
        assigned_k1[best_k1] = (ta, best_score)
        proposed[ta] = (best_k1, best_score, best_k1_name, tsl_size, best_k1_size)

    new_matches = []
    for ta, (ka, score, k1_name, tsl_size, k1_size) in proposed.items():
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
            "via_k1": "partial_callee_jaccard:{:.2f}".format(score),
        })

    print("Proposed matches: {}  Conflicts discarded: {}".format(
        len(new_matches), len(conflict_tsl)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(new_matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(new_matches), OUT))


if __name__ == "__main__":
    main()
