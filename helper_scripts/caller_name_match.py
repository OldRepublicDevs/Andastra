#!/usr/bin/env python3
"""
Match remaining TSL FUN_xxx against K1 named functions using named-caller fingerprints.

Strategy: Build a reverse caller map. If K1 function F is called only by {X, Y}
(named), and TSL function G is also called only by {X, Y} (same names), they're
likely the same function.

Requires at least MIN_CALLERS unique named callers.
"""
import json
from collections import defaultdict
from pathlib import Path

MIN_CALLERS = 2

TSL_CG   = "docs/tsl_call_graph.json"
K1_CG    = "docs/k1_call_graph.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_caller_name_matches.json"


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def build_caller_map(funcs):
    """Returns addr -> set of callers' addresses."""
    caller_map = defaultdict(set)
    for f in funcs:
        for c in f.get("c", []):
            caller_map[c].add(f["a"])
    return caller_map


def main():
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    k1_addr_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_addr_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    tsl_caller_map = build_caller_map(tsl_cg["functions"])  # callee_addr -> {caller_addrs}
    k1_caller_map  = build_caller_map(k1_cg["functions"])

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    # Build K1 caller-fingerprint -> [func]
    def k1_caller_fp(addr):
        callers = k1_caller_map.get(addr, set())
        names = frozenset(k1_addr_name[c] for c in callers if c in k1_addr_name)
        return names

    k1_fp_to_funcs: dict = {}
    for f in k1_cg["functions"]:
        fp = k1_caller_fp(f["a"])
        if len(fp) >= MIN_CALLERS:
            k1_fp_to_funcs.setdefault(fp, []).append(f)

    unique_k1_fps = {fp: flist[0] for fp, flist in k1_fp_to_funcs.items() if len(flist) == 1}
    print(f"K1 unique caller fingerprints (>={MIN_CALLERS} named callers): {len(unique_k1_fps)}")

    tsl_fun_xxx = [f for f in tsl_cg["functions"] if f["n"].startswith("FUN_")]
    print(f"TSL FUN_xxx to check: {len(tsl_fun_xxx) - len(matched_tsl & {f['a'] for f in tsl_fun_xxx})}")

    new_matches = []
    seen_k1 = set()

    for f in tsl_fun_xxx:
        tsl_a = f["a"]
        if tsl_a in matched_tsl:
            continue
        callers = tsl_caller_map.get(tsl_a, set())
        named_callers = frozenset(
            tsl_addr_name[c] for c in callers
            if c in tsl_addr_name and not tsl_addr_name[c].startswith("FUN_")
        )
        if len(named_callers) < MIN_CALLERS:
            continue
        if named_callers not in unique_k1_fps:
            continue
        k1f = unique_k1_fps[named_callers]
        k1_a = k1f["a"]
        if k1_a in matched_k1 or k1_a in seen_k1:
            continue
        seen_k1.add(k1_a)
        new_matches.append({
            "k1_addr": hex(k1_a),
            "tsl_addr": hex(tsl_a),
            "name": (k1f.get("ns", "") + "::" if k1f.get("ns") else "") + k1f["n"],
            "class": k1f.get("ns", ""),
            "method": k1f["n"],
            "named_caller_count": len(named_callers),
            "named_callers": sorted(named_callers)[:8],
            "via": "caller_name_fingerprint",
        })

    print(f"New matches found: {len(new_matches)}")
    for m in new_matches[:20]:
        print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | callers: {m['named_callers'][:4]}")

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(new_matches, fh, indent=2)
    print(f"\nWrote {len(new_matches)} matches to {OUT}")


if __name__ == "__main__":
    main()
