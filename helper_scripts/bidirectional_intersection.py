"""
Bidirectional intersection matching.

For each unmatched TSL FUN_xxx A:
  Caller constraint: for each matched TSL caller B of A,
    find B_k1's unmatched K1 callees → candidate set C_callers
  Callee constraint: for each matched TSL callee D of A (where D has K1 match),
    find which K1 functions call D_k1 and are unmatched → candidate set C_callees
  Intersect C_callers ∩ C_callees → if size==1, that's the K1 match.

Also tries:
  - Single-side constraint with min_providers >= 2 (weaker than both sides)
  - if only callers give a single non-empty candidate and callers count >= 3

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_bidirectional_matches.json
"""

from __future__ import annotations

import json
from collections import defaultdict

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_bidirectional_matches.json"

MIN_CALLER_PROVIDERS = 1  # minimum matched callers providing caller constraint
MIN_CALLEE_PROVIDERS = 1  # minimum matched callees providing callee constraint


def main():
    k1cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    matched_k1 = {int(m["k1_addr"], 16) for m in cascade["matches"]}
    matched_tsl = {int(m["tsl_addr"], 16) for m in cascade["matches"]}
    tsl_to_k1 = {int(m["tsl_addr"], 16): int(m["k1_addr"], 16) for m in cascade["matches"]}
    k1_to_match = {int(m["k1_addr"], 16): m for m in cascade["matches"]}

    # Build callee maps
    k1_callees = {f["a"]: set(f.get("c", [])) for f in k1cg["functions"]}
    tsl_callees = {f["a"]: set(f.get("c", [])) for f in tsl_cg["functions"]}
    k1_addr_to_fn = {f["a"]: f for f in k1cg["functions"]}

    # Build K1 caller map (who calls each K1 function)
    k1_callers_map = defaultdict(set)
    for f in k1cg["functions"]:
        for c in f.get("c", []):
            k1_callers_map[c].add(f["a"])

    # Build TSL caller map
    tsl_callers_map = defaultdict(set)
    for f in tsl_cg["functions"]:
        for c in f.get("c", []):
            tsl_callers_map[c].add(f["a"])

    tsl_addr_to_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}
    tsl_fun_addrs = {f["a"] for f in tsl_cg["functions"] if f["n"].startswith("FUN_")}

    stats = {"both_constrained": 0, "callers_3plus": 0, "callees_3plus": 0,
             "conflicts_skipped": 0}

    proposed = {}  # tsl_addr -> (k1_addr, entry)
    conflicts_tsl = set()
    conflicts_k1 = set()

    for f in tsl_cg["functions"]:
        ta = f["a"]
        if ta not in tsl_fun_addrs or ta in matched_tsl:
            continue

        # ---- Caller constraint ----
        callers = tsl_callers_map.get(ta, set())
        matched_callers = [c for c in callers if c in tsl_to_k1]
        caller_candidates = None
        for b_tsl in matched_callers:
            b_k1 = tsl_to_k1[b_tsl]
            k1_unmatched = {c for c in k1_callees.get(b_k1, []) if c not in matched_k1}
            if k1_unmatched:
                caller_candidates = k1_unmatched if caller_candidates is None else caller_candidates & k1_unmatched
                if not caller_candidates:
                    break

        # ---- Callee constraint ----
        callees = tsl_callees.get(ta, set())
        matched_callees = [c for c in callees if c in tsl_to_k1]
        callee_candidates = None
        for d_tsl in matched_callees:
            d_k1 = tsl_to_k1[d_tsl]
            k1_unmatched_callers = {c for c in k1_callers_map.get(d_k1, []) if c not in matched_k1}
            if k1_unmatched_callers:
                callee_candidates = k1_unmatched_callers if callee_candidates is None else callee_candidates & k1_unmatched_callers
                if not callee_candidates:
                    break

        # Determine best candidate set
        final_candidates = None

        if (caller_candidates is not None and len(caller_candidates) > 0 and
                callee_candidates is not None and len(callee_candidates) > 0):
            combined = caller_candidates & callee_candidates
            if len(combined) == 1:
                final_candidates = combined
                stats["both_constrained"] += 1
        elif (caller_candidates is not None and len(caller_candidates) == 1 and
              len(matched_callers) >= 3):
            final_candidates = caller_candidates
            stats["callers_3plus"] += 1
        elif (callee_candidates is not None and len(callee_candidates) == 1 and
              len(matched_callees) >= 3):
            final_candidates = callee_candidates
            stats["callees_3plus"] += 1

        if final_candidates is None or len(final_candidates) != 1:
            continue

        ka = next(iter(final_candidates))
        k1_fn = k1_addr_to_fn.get(ka, {})
        k1_name = k1_fn.get("n", "")
        if not k1_name or k1_name.startswith("FUN_"):
            continue  # K1 side unnamed — low value

        if ta in conflicts_tsl or ka in conflicts_k1:
            stats["conflicts_skipped"] += 1
            continue

        if ta in proposed:
            existing_ka = int(proposed[ta]["k1_addr"], 16)
            if existing_ka != ka:
                conflicts_tsl.add(ta)
                del proposed[ta]
            stats["conflicts_skipped"] += 1
            continue

        # Check for K1 conflict
        existing_with_ka = [(tt, ee) for tt, ee in proposed.items() if int(ee["k1_addr"], 16) == ka]
        if existing_with_ka:
            for tt, ee in existing_with_ka:
                del proposed[tt]
            conflicts_k1.add(ka)
            stats["conflicts_skipped"] += 1
            continue

        k1_ns = k1_fn.get("ns", "")
        if "::" in k1_name:
            parts = k1_name.rsplit("::", 1)
            class_f = parts[0]
            method_f = parts[1]
        elif k1_ns:
            class_f = k1_ns
            method_f = k1_name
        else:
            class_f = ""
            method_f = k1_name

        via = "bidir:callers={},callees={}".format(len(matched_callers), len(matched_callees))
        proposed[ta] = {
            "k1_addr": "0x{:08X}".format(ka),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": k1_name,
            "class": class_f,
            "method": method_f,
            "via_k1": via,
        }

    matches = list(proposed.values())
    print("Both-constrained intersections: {}".format(stats["both_constrained"]))
    print("Callers-only (>=3): {}   Callees-only (>=3): {}".format(stats["callers_3plus"], stats["callees_3plus"]))
    print("Conflicts skipped: {}".format(stats["conflicts_skipped"]))
    print("New matches found: {}".format(len(matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
