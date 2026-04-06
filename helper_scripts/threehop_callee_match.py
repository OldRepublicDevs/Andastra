"""
Three-hop callee fingerprint matching.

Extends two-hop callee matching to 3 levels:
  fp = frozenset{ (direct_callee_name, callee_of_callee_name, callee_of_callee_of_callee_name) }

This catches functions deeply embedded where 2-hop wasn't discriminating enough.
"""
from __future__ import annotations

import json
from typing import Any

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_threehop_callee_matches.json"

MIN_TRIPLES = 2
MAX_CALLEES  = 30

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


def build_callee_index(funcs: list[dict[str, Any]]) -> dict[int, list[int]]:
    return {f["a"]: f.get("c", []) for f in funcs}


def main() -> None:
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    matched_tsl: set[int] = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1:  set[int] = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    k1_name:    dict[int, str] = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name:   dict[int, str] = {f["a"]: f["n"] for f in tsl_cg["functions"]}
    k1_callees: dict[int, list[int]] = build_callee_index(k1_cg["functions"])
    tsl_callees: dict[int, list[int]] = build_callee_index(tsl_cg["functions"])

    # Virtual TSL names: propagate K1 names via cascade
    tsl_vname: dict[int, str] = dict(tsl_name)
    k1_to_tsl: dict[int, int] = {}
    for m in cas["matches"]:
        k1a = parse_hex(m["k1_addr"])
        ta  = parse_hex(m["tsl_addr"])
        k1_to_tsl[k1a] = ta
        kn = k1_name.get(k1a, "")
        if kn and not is_generic(kn):
            tsl_vname[ta] = kn

    def build_3hop_fp(addr: int, callees_idx: dict, name_map: dict) -> frozenset | None:
        direct = callees_idx.get(addr, [])
        if not direct or len(direct) > MAX_CALLEES:
            return None
        triples: set = set()
        for b in direct:
            bn = name_map.get(b, "")
            if is_generic(bn):
                continue
            for c in callees_idx.get(b, []):
                cn = name_map.get(c, "")
                if is_generic(cn):
                    continue
                for d in callees_idx.get(c, []):
                    dn = name_map.get(d, "")
                    if is_generic(dn):
                        continue
                    triples.add((bn, cn, dn))
        if len(triples) < MIN_TRIPLES:
            return None
        return frozenset(triples)

    # Build K1 fingerprint map
    k1_fp_map: dict = {}
    for f in k1_cg["functions"]:
        a = f["a"]
        if a in matched_k1:
            continue
        fp = build_3hop_fp(a, k1_callees, k1_name)
        if fp is None:
            continue
        k1_fp_map.setdefault(fp, []).append(f)

    unique_k1 = {fp: lst[0] for fp, lst in k1_fp_map.items() if len(lst) == 1}
    print("K1 unique 3-hop callee fingerprints (>={} triples): {:,}".format(MIN_TRIPLES, len(unique_k1)))

    # Match TSL FUN_xxx
    matches: list[dict[str, str]] = []
    seen_k1: set[int] = set()

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        fp = build_3hop_fp(f["a"], tsl_callees, tsl_vname)
        if fp is None or fp not in unique_k1:
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
            "via_k1":   "threehop_callee_fingerprint",
        })

    print("New matches: {}".format(len(matches)))
    for m in matches[:15]:
        print("  {} -> {}  {}".format(m["tsl_addr"], m["k1_addr"], m["name"]))

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
