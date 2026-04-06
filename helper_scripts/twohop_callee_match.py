"""
Two-hop callee fingerprint matching.

Instead of just direct callees, build fingerprints from (callee, callee-of-callee) pairs.
This extends the effective fingerprint radius — functions with few named direct callees
may still have distinctive 2-hop chains.

Strategy:
  K1 side: build (k1_name(B), k1_name(C)) pairs where A→B→C, B and C are named
  TSL side: build (vname(B'), vname(C')) pairs where A'→B'→C', B' and C' are
            cascade-translated or directly named in TSL CG

  Match if 2-hop set is unique on both sides and has >= MIN_PAIRS pairs.
"""

from __future__ import annotations

import json

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_twohop_callee_matches.json"

MIN_PAIRS = 2  # minimum 2-hop pairs required for a match
MAX_CALLEES = 25  # skip functions with too many direct callees (to avoid noise)

GENERIC = {
    "",
    "FUN_",
    "__imp__",
    "_free",
    "_malloc",
    "_memcpy",
    "_memset",
    "_memmove",
    "_strlen",
    "_strcpy",
    "_strcat",
    "_strcmp",
    "_sprintf",
    "_printf",
    "__chkstk",
}


def is_generic(name: str) -> bool:
    if not name:
        return True
    if name.startswith("FUN_"):
        return True
    if name.startswith("j__"):
        return True
    for g in GENERIC:
        if g and name.startswith(g):
            return True
    return False


def parse_hex(s: str | int) -> int:
    return int(s, 16) if isinstance(s, str) else s


def main() -> None:
    k1_cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas = json.load(open(CASCADE, encoding="utf-8"))

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1 = {parse_hex(m["k1_addr"]) for m in cas["matches"]}

    # Build name lookups
    k1_name = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # Build cascade-translated virtual name for TSL (addr -> K1 name)
    tsl_vname = dict(tsl_name)  # start with actual TSL names
    tsl_to_k1 = {}
    k1_to_tsl = {}
    for m in cas["matches"]:
        k1a = parse_hex(m["k1_addr"])
        ta = parse_hex(m["tsl_addr"])
        tsl_to_k1[ta] = k1a
        k1_to_tsl[k1a] = ta
        k1n = k1_name.get(k1a, "")
        if k1n and not is_generic(k1n):
            tsl_vname[ta] = k1n  # overwrite with K1 name for cascade-matched funcs

    # Build call graph dicts
    k1_cg_dict = {f["a"]: f for f in k1_cg["functions"]}
    tsl_cg_dict = {f["a"]: f for f in tsl_cg["functions"]}

    # ---- Build K1 2-hop fingerprints ----
    k1_fp_map: dict = {}
    for f in k1_cg["functions"]:
        callees = f.get("c", [])
        if not callees or len(callees) > MAX_CALLEES:
            continue
        pairs: set = set()
        for b_addr in callees:
            b_name = k1_name.get(b_addr, "")
            if is_generic(b_name):
                continue
            b_callees = k1_cg_dict.get(b_addr, {}).get("c", [])
            for c_addr in b_callees:
                c_name = k1_name.get(c_addr, "")
                if is_generic(c_name):
                    continue
                pairs.add((b_name, c_name))
        if len(pairs) < MIN_PAIRS:
            continue
        fp = frozenset(pairs)
        k1_fp_map.setdefault(fp, []).append(f)

    unique_k1 = {fp: flist[0] for fp, flist in k1_fp_map.items() if len(flist) == 1}
    print("K1 unique 2-hop fingerprints (>={} pairs): {:,}".format(MIN_PAIRS, len(unique_k1)))

    # ---- Match TSL FUN_xxx ----
    matches = []
    seen_k1: set = set()

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        callees = f.get("c", [])
        if not callees or len(callees) > MAX_CALLEES:
            continue
        pairs = set()
        for b_addr in callees:
            b_name = tsl_vname.get(b_addr, "")
            if is_generic(b_name):
                continue
            b_callees = tsl_cg_dict.get(b_addr, {}).get("c", [])
            for c_addr in b_callees:
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
        full_name = (k1f.get("ns", "") + "::" + k1f["n"]) if k1f.get("ns") else k1f["n"]
        matches.append(
            {
                "k1_addr": hex(k1_a),
                "tsl_addr": hex(f["a"]),
                "name": full_name,
                "k1_name": full_name,
                "class": k1f.get("ns", ""),
                "method": k1f["n"],
                "pairs": sorted((a + "::" + b if a != b else a) for a, b in list(fp)[:5]),
                "via": "twohop_callee_fingerprint",
            }
        )

    print("Found {} matches".format(len(matches)))
    for m in matches[:20]:
        print("  {} -> {:12s}  {}".format(m["tsl_addr"], m["k1_addr"], m["name"]))
        if m["pairs"]:
            print("    pairs:", m["pairs"][:3])

    # write flat list so merge_any.py can consume directly
    flat = []
    for m in matches:
        flat.append({
            "k1_addr": "0x{:08X}".format(parse_hex(m["k1_addr"])),
            "tsl_addr": "0x{:08X}".format(parse_hex(m["tsl_addr"])),
            "name": m["name"],
            "class": m.get("class", ""),
            "method": m.get("method", m["name"]),
            "via_k1": m.get("via", "twohop_callee_fingerprint"),
        })
    with open(OUT, "w") as fh:
        json.dump(flat, fh, indent=2)
    print("Wrote {} to {}".format(len(flat), OUT))


if __name__ == "__main__":
    main()
