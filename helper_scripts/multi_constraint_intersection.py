"""
Multi-constraint intersection matching.

For TSL FUN_xxx with MULTIPLE named callers:
  For each named caller B of TSL FUN_xxx A:
    - Look up b_k1 = TSL->K1 of B
    - Collect the set of unmatched K1 callees of b_k1 as "candidates"
  
  Intersect all candidate sets across all named callers.
  If intersection has exactly 1 element -> match.

This is high-confidence: A must be the common callee of all named parents.
Works best for functions called from 2+ named parents.
"""
from __future__ import annotations

import json
from collections import defaultdict
from typing import Any

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_multi_constraint_matches.json"

MIN_NAMED_CALLERS = 2


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def main() -> None:
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    matched_tsl: set[int] = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1:  set[int] = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    k1_name:  dict[int, str] = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name: dict[int, str] = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # K1 and TSL callee maps
    k1_callees: dict[int, set[int]] = {f["a"]: set(f.get("c", [])) for f in k1_cg["functions"]}
    tsl_callees: dict[int, set[int]] = {f["a"]: set(f.get("c", [])) for f in tsl_cg["functions"]}

    # Reverse: TSL caller map
    tsl_callers: dict[int, list[int]] = defaultdict(list)
    for f in tsl_cg["functions"]:
        for c in f.get("c", []):
            tsl_callers[c].append(f["a"])

    # TSL->K1 lookup
    tsl_to_k1: dict[int, int] = {parse_hex(m["tsl_addr"]): parse_hex(m["k1_addr"]) for m in cas["matches"]}

    matches: list[dict[str, str]] = []
    seen_k1: set[int] = set()
    seen_tsl: set[int] = set()
    stats = {"checked": 0, "single": 0, "empty": 0, "multi": 0}

    for f in tsl_cg["functions"]:
        ta = f["a"]
        if not f["n"].startswith("FUN_") or ta in matched_tsl:
            continue

        # Get named callers
        named_callers = [ca for ca in tsl_callers.get(ta, [])
                         if not tsl_name.get(ca, "").startswith("FUN_")
                         and tsl_name.get(ca, "") != ""]
        if len(named_callers) < MIN_NAMED_CALLERS:
            continue

        stats["checked"] += 1

        # For each named caller, find its K1 counterpart and gather K1 orphan callees
        candidate_pools: list[set[int]] = []
        for tsl_parent in named_callers:
            k1_parent = tsl_to_k1.get(tsl_parent)
            if k1_parent is None:
                continue
            # Unmatched K1 callees of this K1_parent
            orphan_callees = {c for c in k1_callees.get(k1_parent, set())
                              if c not in matched_k1}
            if orphan_callees:
                candidate_pools.append(orphan_callees)

        if len(candidate_pools) < MIN_NAMED_CALLERS:
            continue

        # Intersect all pools
        intersection = candidate_pools[0]
        for pool in candidate_pools[1:]:
            intersection = intersection & pool

        if len(intersection) == 0:
            stats["empty"] += 1
            continue
        elif len(intersection) > 1:
            stats["multi"] += 1
            continue

        # Exactly 1 candidate
        stats["single"] += 1
        k1_a = next(iter(intersection))
        if k1_a in matched_k1 or k1_a in seen_k1 or ta in seen_tsl:
            continue

        kn = k1_name.get(k1_a, "")
        if not kn:
            continue
        ns     = k1_a  # will fill from namespace below
        ns_str = ""
        meth   = kn
        if "::" in kn:
            parts  = kn.rsplit("::", 1)
            ns_str = parts[0]
            meth   = parts[1]
        else:
            # look up namespace from K1 CG
            for func in k1_cg["functions"]:
                if func["a"] == k1_a:
                    ns_str = func.get("ns", "")
                    break

        seen_k1.add(k1_a)
        seen_tsl.add(ta)
        matches.append({
            "k1_addr":  "0x{:08X}".format(k1_a),
            "tsl_addr": "0x{:08X}".format(ta),
            "name":     kn,
            "class":    ns_str,
            "method":   meth,
            "via_k1":   "multi_constraint_intersection",
        })

    print("Stats: checked={checked}, single={single}, empty={empty}, multi={multi}".format(**stats))
    print("New matches: {}".format(len(matches)))
    for m in matches[:15]:
        print("  {} -> {}  {}".format(m["tsl_addr"], m["k1_addr"], m["name"]))

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
