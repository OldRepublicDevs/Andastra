"""
Caller-set fingerprint matching.

For each TSL FUN_xxx, build the frozenset of NAMED functions that call it
(i.e., its callers' names). Match to K1 unmatched functions with the same
unique caller-name frozenset.

This complements callee_name_match.py by looking at who CALLS the function
rather than who the function calls. Especially useful for:
  - Frequently-called utilities whose callers are distinctive  
  - Leaf functions (no callees) that are called from named contexts
  - Functions whose callee sets contain FUN_xxx but callers are all named

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_caller_set_matches.json
"""

from __future__ import annotations

import json

MIN_CALLERS = 2          # minimum named callers needed to trust the fingerprint
MAX_CALLERS = 20         # skip extremely popular functions (generic stubs)

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_caller_set_matches.json"


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def build_caller_map(cg_data):
    """Returns {callee_addr: [caller_addr, ...]}."""
    caller_map = {}
    for f in cg_data["functions"]:
        caller = f["a"]
        for callee in f.get("c", []):
            caller_map.setdefault(callee, []).append(caller)
    return caller_map


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

    # Build K1 fingerprint: frozenset of named caller names -> [func info]
    k1_fp_to_funcs = {}
    for f in k1cg["functions"]:
        ka = f["a"]
        if ka in matched_k1:
            continue
        callers = k1_callers.get(ka, [])
        named = sorted(set(
            k1_addr_name[c] for c in callers
            if c in k1_addr_name and not k1_addr_name[c].startswith("FUN_")
        ))
        if len(named) < MIN_CALLERS or len(named) > MAX_CALLERS:
            continue
        fp = frozenset(named)
        k1_fp_to_funcs.setdefault(fp, []).append(f)

    unique_k1_fps = {fp: funcs[0] for fp, funcs in k1_fp_to_funcs.items() if len(funcs) == 1}
    print("K1 unique caller-set fingerprints ({}-{} named callers): {}".format(
        MIN_CALLERS, MAX_CALLERS, len(unique_k1_fps)))

    # Try to match TSL FUN_xxx against these unique fingerprints
    tsl_fun_xxx = [f for f in tsl_cg["functions"] if f["n"].startswith("FUN_")]
    print("TSL FUN_xxx to check: {}".format(len(tsl_fun_xxx)))

    new_matches = []
    seen_k1 = set()
    seen_tsl = set()

    for f in tsl_fun_xxx:
        ta = f["a"]
        if ta in matched_tsl:
            continue
        callers = tsl_callers.get(ta, [])
        named = sorted(set(
            tsl_addr_name[c] for c in callers
            if c in tsl_addr_name and not tsl_addr_name[c].startswith("FUN_")
        ))
        if len(named) < MIN_CALLERS or len(named) > MAX_CALLERS:
            continue
        fp = frozenset(named)
        if fp not in unique_k1_fps:
            continue
        k1f = unique_k1_fps[fp]
        ka = k1f["a"]
        if ka in matched_k1 or ka in seen_k1:
            continue
        if ta in seen_tsl:
            continue

        seen_k1.add(ka)
        seen_tsl.add(ta)

        k1_name = k1f["n"]
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
            "via_k1": "caller_set_fp:{:d}callers".format(len(named)),
        })

    print("New matches found: {}".format(len(new_matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(new_matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(new_matches), OUT))


if __name__ == "__main__":
    main()
