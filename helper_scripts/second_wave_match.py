#!/usr/bin/env python3
"""
Simulate applying cascade renames to the TSL call graph, then re-run
callee-name and caller-name fingerprint matching to find second-wave matches.

This predicts what new matches would be available AFTER the current batch
of 126 renames gets applied in Ghidra.
"""
import json
import copy
from collections import defaultdict

TSL_CG   = "docs/tsl_call_graph.json"
K1_CG    = "docs/k1_call_graph.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT_CALLEE  = "docs/tsl_second_wave_callee.json"
OUT_CALLER  = "docs/tsl_second_wave_caller.json"

MIN_CALLEE  = 2
MIN_CALLER  = 2

GENERIC = {
    "CExoString", "operator_new", "operator_delete", "void_return", "operator=",
    "~CExoString", "CResRef", "~CResRef", "operator+", "operator new", "operator delete",
    "operator!=", "operator==", "operator<", "malloc", "free", "memcpy", "memset",
}


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def build_caller_map(funcs):
    caller_map = defaultdict(set)
    for f in funcs:
        for c in f.get("c", []):
            caller_map[c].add(f["a"])
    return caller_map


def main():
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    # Build simulated TSL name map (apply all cascade matches as renames)
    tsl_addr_name_orig = {f["a"]: f["n"] for f in tsl_cg["functions"]}
    tsl_addr_name      = dict(tsl_addr_name_orig)  # mutable copy

    matched_tsl = set()
    matched_k1  = set()
    for m in cas["matches"]:
        ta = parse_hex(m["tsl_addr"])
        ka = parse_hex(m["k1_addr"])
        matched_tsl.add(ta)
        matched_k1.add(ka)
        # Apply the rename to the simulated map
        ns  = m.get("class", "")
        fn  = m.get("method", "") or m.get("name", "")
        if fn:
            tsl_addr_name[ta] = (ns + "::" if ns else "") + fn

    fun_xxx_before = sum(1 for v in tsl_addr_name_orig.values() if v.startswith("FUN_"))
    fun_xxx_after  = sum(1 for v in tsl_addr_name.values()       if v.startswith("FUN_"))
    print(f"TSL FUN_xxx before simulated renames: {fun_xxx_before}")
    print(f"TSL FUN_xxx after  simulated renames: {fun_xxx_after}")
    print(f"Renamed in simulation: {fun_xxx_before - fun_xxx_after}")

    k1_addr_name = {f["a"]: f["n"] for f in k1_cg["functions"]}

    # ---- CALLEE FINGERPRINT PASS ----
    k1_fp_to_funcs: dict = {}
    for f in k1_cg["functions"]:
        callees = f.get("c", [])
        named = frozenset(k1_addr_name.get(c, "") for c in callees)
        named = frozenset(n for n in named if n)
        if len(named) >= MIN_CALLEE:
            k1_fp_to_funcs.setdefault(named, []).append(f)
    unique_k1_callee = {fp: flist[0] for fp, flist in k1_fp_to_funcs.items() if len(flist) == 1}

    callee_matches = []
    seen_k1 = set()
    tsl_fun_xxx = [f for f in tsl_cg["functions"] if tsl_addr_name.get(f["a"], f["n"]).startswith("FUN_")]
    for f in tsl_fun_xxx:
        ta = f["a"]
        if ta in matched_tsl:
            continue
        named = frozenset(
            tsl_addr_name.get(c, "") for c in f.get("c", [])
            if tsl_addr_name.get(c, "") and not tsl_addr_name.get(c, "").startswith("FUN_")
        )
        if len(named) < MIN_CALLEE:
            continue
        if named not in unique_k1_callee:
            continue
        k1f = unique_k1_callee[named]
        if k1f["a"] in matched_k1 or k1f["a"] in seen_k1:
            continue
        seen_k1.add(k1f["a"])
        callee_matches.append({
            "k1_addr": hex(k1f["a"]),
            "tsl_addr": hex(ta),
            "name": (k1f.get("ns", "") + "::" if k1f.get("ns") else "") + k1f["n"],
            "class": k1f.get("ns", ""),
            "method": k1f["n"],
            "named_callees": sorted(named)[:8],
            "via": "second_wave_callee_name_fingerprint",
        })

    print(f"\nSecond-wave callee matches: {len(callee_matches)}")

    # ---- CALLER FINGERPRINT PASS ----
    tsl_caller_map = build_caller_map(tsl_cg["functions"])
    k1_caller_map  = build_caller_map(k1_cg["functions"])

    k1_caller_fp_to_funcs: dict = {}
    for f in k1_cg["functions"]:
        callers = k1_caller_map.get(f["a"], set())
        named = frozenset(k1_addr_name.get(c, "") for c in callers if c in k1_addr_name)
        if len(named) >= MIN_CALLER:
            k1_caller_fp_to_funcs.setdefault(named, []).append(f)
    unique_k1_caller = {fp: flist[0] for fp, flist in k1_caller_fp_to_funcs.items() if len(flist) == 1}

    caller_matches = []
    seen_k1_c = set()
    for f in tsl_fun_xxx:
        ta = f["a"]
        if ta in matched_tsl:
            continue
        callers = tsl_caller_map.get(ta, set())
        named = frozenset(
            tsl_addr_name.get(c, "") for c in callers
            if tsl_addr_name.get(c, "") and not tsl_addr_name.get(c, "").startswith("FUN_")
        )
        if len(named) < MIN_CALLER:
            continue
        if named not in unique_k1_caller:
            continue
        k1f = unique_k1_caller[named]
        if k1f["a"] in matched_k1 or k1f["a"] in seen_k1_c:
            continue
        seen_k1_c.add(k1f["a"])
        caller_matches.append({
            "k1_addr": hex(k1f["a"]),
            "tsl_addr": hex(ta),
            "name": (k1f.get("ns", "") + "::" if k1f.get("ns") else "") + k1f["n"],
            "class": k1f.get("ns", ""),
            "method": k1f["n"],
            "named_callers": sorted(named)[:8],
            "via": "second_wave_caller_name_fingerprint",
        })

    print(f"Second-wave caller matches: {len(caller_matches)}")

    with open(OUT_CALLEE, "w", encoding="utf-8") as fh:
        json.dump(callee_matches, fh, indent=2)
    with open(OUT_CALLER, "w", encoding="utf-8") as fh:
        json.dump(caller_matches, fh, indent=2)

    # Print first few of each
    for m in callee_matches[:10]:
        print(f"  CALLEE {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | {m['named_callees'][:4]}")
    for m in caller_matches[:10]:
        print(f"  CALLER {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | {m['named_callers'][:4]}")


if __name__ == "__main__":
    main()
