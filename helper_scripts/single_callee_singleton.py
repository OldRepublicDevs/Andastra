"""
Single-callee singleton matching.

Callee-direction counterpart to single_caller_singleton.py.

Case A (TSL-driven):
  A TSL FUN_xxx calls exactly 1 named+matched TSL callee D_tsl,
  and D_tsl's K1 counterpart D_k1 has exactly 1 unmatched K1 caller.
  → TSL FUN_xxx must be that K1 caller's TSL counterpart.

Case B (K1-driven):
  An unmatched K1 function C_k1 is called by exactly 1 unmatched K1 callee D,
  ... actually this is covered by Case A.

Alternate case (K1-driven):
  An unmatched K1 function calls exactly 1 matched named K1 callee D_k1,
  and D_k1's TSL counterpart D_tsl has exactly 1 unmatched TSL caller.
  → That TSL caller matches C_k1.

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_single_callee_singleton_matches.json
"""

from __future__ import annotations

import json
from collections import defaultdict

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_single_callee_singleton_matches.json"


def main():
    k1cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    matched_k1 = {int(m["k1_addr"], 16) for m in cascade["matches"]}
    matched_tsl = {int(m["tsl_addr"], 16) for m in cascade["matches"]}
    tsl_to_k1 = {int(m["tsl_addr"], 16): int(m["k1_addr"], 16) for m in cascade["matches"]}
    k1_to_tsl = {int(m["k1_addr"], 16): int(m["tsl_addr"], 16) for m in cascade["matches"]}
    k1_to_match = {int(m["k1_addr"], 16): m for m in cascade["matches"]}

    k1_callees = {f["a"]: set(f.get("c", [])) for f in k1cg["functions"]}
    tsl_callees = {f["a"]: set(f.get("c", [])) for f in tsl_cg["functions"]}
    k1_addr_to_fn = {f["a"]: f for f in k1cg["functions"]}
    tsl_addr_to_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # Build K1 caller map
    k1_callers_map = defaultdict(set)
    for f in k1cg["functions"]:
        for c in f.get("c", []):
            k1_callers_map[c].add(f["a"])

    # Build TSL caller map
    tsl_callers_map = defaultdict(set)
    for f in tsl_cg["functions"]:
        for c in f.get("c", []):
            tsl_callers_map[c].add(f["a"])

    tsl_fun_addrs = {f["a"] for f in tsl_cg["functions"] if f["n"].startswith("FUN_")}

    # ---- Case A: TSL-driven ----
    proposed_a = {}
    conflicts_a_tsl = set()
    conflicts_a_k1 = set()

    for f in tsl_cg["functions"]:
        ta = f["a"]
        if ta not in tsl_fun_addrs or ta in matched_tsl:
            continue

        # Find named+matched callees of this TSL FUN_xxx
        named_matched_callees = [c for c in tsl_callees.get(ta, [])
                                  if c in tsl_to_k1 and not tsl_addr_to_name.get(c, "FUN_").startswith("FUN_")]
        if len(named_matched_callees) != 1:
            continue

        d_tsl = named_matched_callees[0]
        d_k1 = tsl_to_k1[d_tsl]

        # D_k1 must have exactly 1 unmatched K1 caller
        k1_unmatched_callers = [c for c in k1_callers_map.get(d_k1, []) if c not in matched_k1]
        if len(k1_unmatched_callers) != 1:
            continue

        c_k1 = k1_unmatched_callers[0]
        k1_fn = k1_addr_to_fn.get(c_k1, {})
        k1_name = k1_fn.get("n", "")
        if not k1_name or k1_name.startswith("FUN_"):
            continue  # K1 side unnamed

        if c_k1 in conflicts_a_k1 or ta in conflicts_a_tsl:
            continue
        if c_k1 in {int(e["k1_addr"], 16) for e in proposed_a.values()}:
            conflicts_a_k1.add(c_k1)
            # Remove the conflicting entry
            for tt in list(proposed_a.keys()):
                if int(proposed_a[tt]["k1_addr"], 16) == c_k1:
                    del proposed_a[tt]
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

        proposed_a[ta] = {
            "k1_addr": "0x{:08X}".format(c_k1),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": k1_name,
            "class": class_f,
            "method": method_f,
            "via_k1": "single_callee_singleton_tsl:callee_{}".format(tsl_addr_to_name.get(d_tsl, "?")),
        }

    dir_a = list(proposed_a.values())

    # ---- Case B: K1-driven ----
    proposed_b = {}
    conflicts_b_tsl = set()
    conflicts_b_k1 = set()

    for f in k1cg["functions"]:
        ka = f["a"]
        if ka in matched_k1:
            continue
        k1_name = f.get("n", "")
        if not k1_name or k1_name.startswith("FUN_"):
            continue

        # Find exactly 1 named matched K1 callee
        named_matched_k1_callees = [c for c in k1_callees.get(ka, [])
                                     if c in k1_to_tsl]
        if len(named_matched_k1_callees) != 1:
            continue

        d_k1 = named_matched_k1_callees[0]
        d_tsl = k1_to_tsl[d_k1]

        # D_tsl must have exactly 1 unmatched TSL caller
        tsl_unmatched_callers = [c for c in tsl_callers_map.get(d_tsl, [])
                                   if c not in matched_tsl and c in tsl_fun_addrs]
        if len(tsl_unmatched_callers) != 1:
            continue

        ta = tsl_unmatched_callers[0]

        if ka in conflicts_b_k1 or ta in conflicts_b_tsl:
            continue
        existing_k1_for_ta = {int(e["k1_addr"], 16) for tt, e in proposed_b.items() if tt == ta}
        if existing_k1_for_ta:
            conflicts_b_tsl.add(ta)
            if ta in proposed_b:
                del proposed_b[ta]
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

        proposed_b[ta] = {
            "k1_addr": "0x{:08X}".format(ka),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": k1_name,
            "class": class_f,
            "method": method_f,
            "via_k1": "single_callee_singleton_k1:{}".format(k1_name),
        }

    dir_b = list(proposed_b.values())

    # Merge both directions (dedup by TSL+K1 addr)
    seen_tsl = set()
    seen_k1 = set()
    all_matches = []
    for m in dir_a + dir_b:
        ta = int(m["tsl_addr"], 16)
        ka = int(m["k1_addr"], 16)
        if ta in seen_tsl or ka in seen_k1:
            continue
        seen_tsl.add(ta)
        seen_k1.add(ka)
        all_matches.append(m)

    print("Case A (TSL-driven): {} candidates".format(len(dir_a)))
    print("Case B (K1-driven):  {} candidates".format(len(dir_b)))
    print("After dedup: {} total".format(len(all_matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(all_matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(all_matches), OUT))


if __name__ == "__main__":
    main()
