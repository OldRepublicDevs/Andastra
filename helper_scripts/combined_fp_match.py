"""
Combined caller+callee fingerprint matching.

Build fingerprints using BOTH direct callers AND direct callees simultaneously.
More discriminating than either alone — especially for functions whose callee
set alone or caller set alone is not unique.

Fingerprint: (frozenset(named_callees), frozenset(named_callers))
Both sets must be non-empty; combined they must uniquely identify a K1 function.
"""

from __future__ import annotations

import json
from collections import defaultdict

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_combined_fp_matches.json"

MIN_CALLEES = 1
MIN_CALLERS = 1
MAX_DEGREE  = 30   # skip very high-degree nodes

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


def build_callers(cg_funcs: list) -> dict[int, list[int]]:
    callers: dict[int, list[int]] = defaultdict(list)
    for f in cg_funcs:
        for c in f.get("c", []):
            callers[c].append(f["a"])
    return callers


def make_fp(named_callees: frozenset, named_callers: frozenset):
    return (named_callees, named_callers)


def main():
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    tsl_vname = dict(tsl_name)
    for m in cas["matches"]:
        k1a = parse_hex(m["k1_addr"])
        ta  = parse_hex(m["tsl_addr"])
        k1n = k1_name.get(k1a, "")
        if k1n and not is_generic(k1n):
            tsl_vname[ta] = k1n

    k1_callers  = build_callers(k1_cg["functions"])
    tsl_callers = build_callers(tsl_cg["functions"])

    # ---- K1 fingerprints ----
    k1_fp_map: dict = {}
    for f in k1_cg["functions"]:
        a = f["a"]
        callees  = f.get("c", [])
        callers  = k1_callers.get(a, [])
        if len(callees) > MAX_DEGREE or len(callers) > MAX_DEGREE:
            continue
        nc = frozenset(k1_name.get(c,"") for c in callees if not is_generic(k1_name.get(c,"")))
        nr = frozenset(k1_name.get(c,"") for c in callers if not is_generic(k1_name.get(c,"")))
        if len(nc) < MIN_CALLEES or len(nr) < MIN_CALLERS:
            continue
        fp = make_fp(nc, nr)
        k1_fp_map.setdefault(fp, []).append(f)

    unique_k1 = {fp: flist[0] for fp, flist in k1_fp_map.items() if len(flist) == 1}
    print("K1 unique combined fingerprints: {:,}".format(len(unique_k1)))

    # ---- Match TSL FUN_xxx ----
    matches = []
    seen_k1: set = set()

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        a = f["a"]
        callees = f.get("c", [])
        callers = tsl_callers.get(a, [])
        if len(callees) > MAX_DEGREE or len(callers) > MAX_DEGREE:
            continue
        nc = frozenset(tsl_vname.get(c,"") for c in callees if not is_generic(tsl_vname.get(c,"")))
        nr = frozenset(tsl_vname.get(c,"") for c in callers if not is_generic(tsl_vname.get(c,"")))
        if len(nc) < MIN_CALLEES or len(nr) < MIN_CALLERS:
            continue
        fp = make_fp(nc, nr)
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
            "via_k1":   "combined_caller_callee_fp",
        })

    print("New matches: {}".format(len(matches)))
    for m in matches[:15]:
        print("  {} -> {:12s}  {}".format(m["tsl_addr"], m["k1_addr"], m["name"]))

    with open(OUT, "w") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
