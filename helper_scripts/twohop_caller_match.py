"""
Two-hop caller fingerprint matching.

Mirror of twohop_callee_match but going UP the call tree:
  Build (caller_name(B), caller_of_caller_name(C)) pairs where C→B→A
  (A is the function we're trying to match, B calls A, C calls B)

This catches functions that are deeply embedded in a call hierarchy
where the callers are well-named but the callees are not.
"""

from __future__ import annotations

import json
from collections import defaultdict
from typing import Any

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_twohop_caller_matches.json"

MIN_PAIRS  = 2
MAX_CALLERS = 25

GENERIC = {
    "", "return_zero", "return_minus_one", "void_return", "return_true",
    "return_false", "exit_error_25",
}


def is_generic(name: str) -> bool:
    if not name:
        return True
    if name.startswith("FUN_") or name.startswith("j__"):
        return True
    return name in GENERIC


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def build_callers(cg: list[dict[str, Any]]) -> dict[int, list[int]]:
    """Build reverse call graph: addr -> list of callers."""
    callers: dict[int, list[int]] = defaultdict(list)
    for f in cg:
        for c in f.get("c", []):
            callers[c].append(f["a"])
    return callers


def main() -> None:
    k1_cg: dict[str, Any]  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg: dict[str, Any] = json.load(open(TSL_CG, encoding="utf-8"))
    cas: dict[str, Any]    = json.load(open(CASCADE, encoding="utf-8"))

    matched_tsl: set[int] = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1: set[int]  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    k1_name: dict[int, str]  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name: dict[int, str] = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    tsl_vname: dict[int, str] = dict(tsl_name)
    k1_to_tsl: dict[int, int] = {}
    for m in cas["matches"]:
        k1a = parse_hex(m["k1_addr"])
        ta  = parse_hex(m["tsl_addr"])
        k1_to_tsl[k1a] = ta
        k1n = k1_name.get(k1a, "")
        if k1n and not is_generic(k1n):
            tsl_vname[ta] = k1n

    # Build reverse call graphs
    k1_callers: dict[int, list[int]] = build_callers(k1_cg["functions"])
    tsl_callers: dict[int, list[int]] = build_callers(tsl_cg["functions"])

    # ---- K1 2-hop caller fingerprints ----
    k1_fp_map: dict[frozenset[tuple[str, str]], list[dict[str, str]]] = {}
    for f in k1_cg["functions"]:
        a = f["a"]
        my_callers = k1_callers.get(a, [])
        if not my_callers or len(my_callers) > MAX_CALLERS:
            continue
        pairs: set[tuple[str, str]] = set()
        for b_addr in my_callers:
            b_name = k1_name.get(b_addr, "")
            if is_generic(b_name):
                continue
            for c_addr in k1_callers.get(b_addr, []):
                c_name = k1_name.get(c_addr, "")
                if is_generic(c_name):
                    continue
                pairs.add((b_name, c_name))
        if len(pairs) < MIN_PAIRS:
            continue
        fp = frozenset(pairs)
        k1_fp_map.setdefault(fp, []).append(f)

    unique_k1 = {fp: flist[0] for fp, flist in k1_fp_map.items() if len(flist) == 1}
    print("K1 unique 2-hop caller fingerprints (>={} pairs): {:,}".format(MIN_PAIRS, len(unique_k1)))

    # ---- Match TSL FUN_xxx ----
    matches: list[dict[str, str]] = []
    seen_k1: set[str] = set()

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        a = f["a"]
        my_callers = tsl_callers.get(a, [])
        if not my_callers or len(my_callers) > MAX_CALLERS:
            continue
        pairs = set()
        for b_addr in my_callers:
            b_name = tsl_vname.get(b_addr, "")
            if is_generic(b_name):
                continue
            for c_addr in tsl_callers.get(b_addr, []):
                c_name = tsl_vname.get(c_addr, "")
                if is_generic(c_name):
                    continue
                pairs.add((b_name, c_name))
        if len(pairs) < MIN_PAIRS:
            continue
        fp = frozenset(pairs)
        if fp not in unique_k1:
            continue
        k1f = unique_k1[fp]
        k1_a = k1f["a"]
        if k1_a in matched_k1 or k1_a in seen_k1:
            continue
        seen_k1.add(k1_a)
        ns     = k1f.get("ns", "")
        method = k1f["n"]
        name   = (ns + "::" + method) if ns else method
        matches.append({
            "k1_addr":  "0x{:08X}".format(k1_a),
            "tsl_addr": "0x{:08X}".format(f["a"]),
            "name":     name,
            "class":    ns,
            "method":   method,
            "via_k1":   "twohop_caller_fingerprint",
        })

    print("New matches: {}".format(len(matches)))
    for m in matches[:15]:
        print("  {} -> {:12s}  {}".format(m["tsl_addr"], m["k1_addr"], m["name"]))

    with open(OUT, "w") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
