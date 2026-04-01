#!/usr/bin/env python3
"""
Match remaining TSL FUN_xxx against K1 named functions using named-callee fingerprints.

Strategy: If K1 function F calls {A, B, C} (all named) and TSL function G also calls
functions with the same {A, B, C} names, they're likely the same function.

Only accepts unique matches (fingerprint appears in exactly ONE K1 function).
Requires at least MIN_CALLEES named callees for a fingerprint.
"""
import json
import sys
from pathlib import Path

MIN_CALLEES = 2

TSL_CG   = "docs/tsl_call_graph.json"
K1_CG    = "docs/k1_call_graph.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_callee_name_matches.json"


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def main():
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    k1_addr_name = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_addr_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # TSL calls -> translated to names (skip remaining FUN_xxx and thunks)
    def tsl_named_callees(f):
        result = []
        for c in f.get("c", []):
            name = tsl_addr_name.get(c, "")
            if name and not name.startswith("FUN_"):
                result.append(name)
        return result

    # Build K1 fingerprint -> [func] (only unique fingerprints used for matching)
    k1_fp_to_funcs: dict = {}
    for f in k1_cg["functions"]:
        callees = f.get("c", [])
        named = [k1_addr_name.get(c, "") for c in callees]
        named = [n for n in named if n]
        fp = frozenset(named)
        if len(fp) >= MIN_CALLEES:
            k1_fp_to_funcs.setdefault(fp, []).append(f)

    unique_k1_fps = {fp: funcs[0] for fp, funcs in k1_fp_to_funcs.items() if len(funcs) == 1}
    print(f"K1 unique fingerprints (>={MIN_CALLEES} named callees): {len(unique_k1_fps)}")

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    tsl_fun_xxx = [f for f in tsl_cg["functions"] if f["n"].startswith("FUN_")]
    print(f"TSL FUN_xxx to check: {len(tsl_fun_xxx)}")

    new_matches = []
    seen_k1 = set()

    for f in tsl_fun_xxx:
        tsl_a = f["a"]
        if tsl_a in matched_tsl:
            continue
        named_callees = tsl_named_callees(f)
        fp = frozenset(named_callees)
        if len(fp) < MIN_CALLEES:
            continue
        if fp not in unique_k1_fps:
            continue
        k1f = unique_k1_fps[fp]
        k1_a = k1f["a"]
        if k1_a in matched_k1:
            continue
        # Avoid double-assigning the same K1 function
        if k1_a in seen_k1:
            continue
        seen_k1.add(k1_a)
        new_matches.append({
            "k1_addr": hex(k1_a),
            "tsl_addr": hex(tsl_a),
            "name": (k1f.get("ns", "") + "::" if k1f.get("ns") else "") + k1f["n"],
            "class": k1f.get("ns", ""),
            "method": k1f["n"],
            "named_callee_count": len(fp),
            "named_callees": sorted(named_callees)[:10],
            "via": "callee_name_fingerprint",
        })

    print(f"New matches found: {len(new_matches)}")
    for m in new_matches[:20]:
        print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | callees: {m['named_callees'][:4]}")

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(new_matches, fh, indent=2)
    print(f"\nWrote {len(new_matches)} matches to {OUT}")


if __name__ == "__main__":
    main()
