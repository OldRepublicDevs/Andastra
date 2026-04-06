"""
Single-caller singleton matching.

High-confidence case: A TSL FUN_xxx has exactly 1 named+matched TSL caller B_tsl,
and B_tsl's K1 counterpart B_k1 has exactly 1 unmatched K1 callee C_k1.
Then TSL FUN_xxx must be C_k1's TSL counterpart.

Also checks the symmetric K1-side direction:
  An unmatched K1 function C_k1 is called by exactly 1 matched K1 caller B_k1,
  and B_k1's TSL counterpart B_tsl has exactly 1 unmatched TSL callee → match.

Conflict resolution: if two unmatched TSL FUN_xxx compete for the same C_k1, both are skipped.

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_single_caller_singleton_matches.json
"""

from __future__ import annotations

import json
from collections import defaultdict

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_single_caller_singleton_matches.json"


def main():
    k1cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    matched_k1 = {int(m["k1_addr"], 16) for m in cascade["matches"]}
    matched_tsl = {int(m["tsl_addr"], 16) for m in cascade["matches"]}
    tsl_to_k1 = {int(m["tsl_addr"], 16): int(m["k1_addr"], 16) for m in cascade["matches"]}
    k1_to_tsl = {int(m["k1_addr"], 16): int(m["tsl_addr"], 16) for m in cascade["matches"]}
    k1_to_match = {int(m["k1_addr"], 16): m for m in cascade["matches"]}

    # Build callee maps
    k1_callees = {f["a"]: set(f.get("c", [])) for f in k1cg["functions"]}
    tsl_callees = {f["a"]: set(f.get("c", [])) for f in tsl_cg["functions"]}

    # Build caller maps (who calls each function)
    tsl_callers_map = defaultdict(set)
    for f in tsl_cg["functions"]:
        for c in f.get("c", []):
            tsl_callers_map[c].add(f["a"])

    k1_callers_map = defaultdict(set)
    for f in k1cg["functions"]:
        for c in f.get("c", []):
            k1_callers_map[c].add(f["a"])

    # Build name lookups
    tsl_addr_to_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}
    k1_addr_to_fn = {f["a"]: f for f in k1cg["functions"]}

    # ---- Direction 1: TSL-driven ----
    # TSL FUN_xxx with 1 named+matched caller, K1 side has 1 unmatched callee
    proposed = {}  # tsl_addr -> (k1_addr, match_obj)
    conflicts = set()

    tsl_fun_addrs = {f["a"] for f in tsl_cg["functions"] if f["n"].startswith("FUN_")}

    for ta in tsl_fun_addrs:
        if ta in matched_tsl:
            continue
        callers = tsl_callers_map.get(ta, set())
        named_matched_callers = [c for c in callers
                                  if c in tsl_to_k1 and not tsl_addr_to_name.get(c, "FUN_").startswith("FUN_")]
        if len(named_matched_callers) != 1:
            continue

        b_tsl = named_matched_callers[0]
        b_k1 = tsl_to_k1[b_tsl]
        k1_unmatched_callees = [c for c in k1_callees.get(b_k1, []) if c not in matched_k1]

        if len(k1_unmatched_callees) != 1:
            continue

        c_k1 = k1_unmatched_callees[0]
        if c_k1 in conflicts:
            continue
        if c_k1 in proposed:
            # Conflict — two TSL FUN_xxx map to same K1 addr
            conflicts.add(c_k1)
            del proposed[c_k1]
            continue

        b_match = k1_to_match[b_k1]
        k1_fn = k1_addr_to_fn.get(c_k1, {})
        k1_name = k1_fn.get("n", "")
        k1_ns = k1_fn.get("ns", "")
        if not k1_name or k1_name.startswith("FUN_"):
            continue  # K1 side also unnamed — low value

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

        proposed[c_k1] = {
            "k1_addr": "0x{:08X}".format(c_k1),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": k1_name,
            "class": class_f,
            "method": method_f,
            "via_k1": "single_caller_singleton_tsl:parent_{}".format(b_match.get("name", "?")),
        }

    dir1 = list(proposed.values())

    # ---- Direction 2: K1-driven ----
    # Unmatched K1 function with 1 matched K1 caller, TSL side has 1 unmatched TSL callee
    proposed2 = {}
    conflicts2 = set()

    for f in k1cg["functions"]:
        ka = f["a"]
        if ka in matched_k1:
            continue
        k1_name = f.get("n", "")
        if not k1_name or k1_name.startswith("FUN_"):
            continue  # K1 side unnamed — low value

        callers = k1_callers_map.get(ka, set())
        matched_callers = [c for c in callers if c in k1_to_tsl]
        if len(matched_callers) != 1:
            continue

        b_k1 = matched_callers[0]
        b_tsl = k1_to_tsl[b_k1]
        tsl_unmatched_callees = [c for c in tsl_callees.get(b_tsl, [])
                                  if c not in matched_tsl and c in tsl_fun_addrs]

        if len(tsl_unmatched_callees) != 1:
            continue

        ta = tsl_unmatched_callees[0]
        key = (ka, ta)
        if ta in conflicts2:
            continue
        if ta in proposed2:
            conflicts2.add(ta)
            del proposed2[ta]
            continue

        k1_ns = f.get("ns", "")
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

        proposed2[ta] = {
            "k1_addr": "0x{:08X}".format(ka),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": k1_name,
            "class": class_f,
            "method": method_f,
            "via_k1": "single_caller_singleton_k1:{}".format(k1_name),
        }

    dir2 = list(proposed2.values())

    # Merge both directions (TSL addr dedup)
    seen_tsl = set()
    seen_k1 = set()
    all_matches = []
    for m in dir1 + dir2:
        ta = int(m["tsl_addr"], 16)
        ka = int(m["k1_addr"], 16)
        if ta in seen_tsl or ka in seen_k1:
            continue
        seen_tsl.add(ta)
        seen_k1.add(ka)
        all_matches.append(m)

    print("Direction 1 (TSL-driven): {} candidates, {} conflicts skipped".format(len(dir1), len(conflicts)))
    print("Direction 2 (K1-driven):  {} candidates, {} conflicts skipped".format(len(dir2), len(conflicts2)))
    print("After dedup: {} total".format(len(all_matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(all_matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(all_matches), OUT))


if __name__ == "__main__":
    main()
