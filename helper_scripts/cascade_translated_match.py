"""
Cascade-translated callee fingerprinting.

For each TSL FUN_xxx, look at ALL its callees (named and unnamed).
For unnamed callees that are already in the K1-TSL cascade, substitute their K1 name.
This makes previously "zero named callee" functions suddenly have named callees.

Multiple iterative waves: each new match adds more names for the next wave.

This targets the A_no_callees and B_no_callers categories (28% + 13% = 41% of FUN_xxx).
"""
import json
from collections import defaultdict

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_cascade_translated_matches.json"

MIN_TRANSLATED = 2    # minimum translated named callees
MAX_WAVES = 15

GENERIC = frozenset({"_free","_malloc","_memcpy","_memset","_memmove","_strlen","_strcpy",
           "_strcat","_strcmp","_sprintf","_printf","__chkstk","operator_new","operator_delete",
           "FUN_","j__","__imp"})

def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

def is_generic(n):
    if not n or n.startswith("FUN_"): return True
    for g in GENERIC:
        if n.startswith(g): return True
    return False

def build_caller_map(funcs):
    cm = defaultdict(set)
    for f in funcs:
        for c in f.get("c", []):
            cm[c].add(f["a"])
    return cm

def run_wave(tsl_funcs, k1_funcs, tsl_vname, k1_name,
             matched_tsl, matched_k1):
    """
    tsl_vname: addr -> "virtual name" (may be K1 name translated via cascade,
               or actual TSL name if not FUN_xxx)
    """
    tsl_cm = build_caller_map(tsl_funcs)
    k1_cm  = build_caller_map(k1_funcs)

    # Build K1 joint fingerprints (callee + caller sets, both translated)
    k1_fp = {}
    for f in k1_funcs:
        cs = frozenset(k1_name.get(c,"") for c in f.get("c",[])
                       if not is_generic(k1_name.get(c,"")))
        ps = frozenset(k1_name.get(c,"") for c in k1_cm.get(f["a"],set())
                       if not is_generic(k1_name.get(c,"")))
        combined = cs | ps
        if len(combined) < MIN_TRANSLATED:
            continue
        fp = (cs, ps)
        k1_fp.setdefault(fp, []).append(f)
    unique_k1 = {fp: lst[0] for fp, lst in k1_fp.items() if len(lst)==1}

    new_matches = []
    seen_k1 = set()
    for f in tsl_funcs:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        # Use tsl_vname (which may have cascade-translated names)
        cs = frozenset(tsl_vname.get(c,"") for c in f.get("c",[])
                       if not is_generic(tsl_vname.get(c,"")))
        ps = frozenset(tsl_vname.get(c,"") for c in tsl_cm.get(f["a"],set())
                       if not is_generic(tsl_vname.get(c,"")))
        combined = cs | ps
        if len(combined) < MIN_TRANSLATED:
            continue
        fp = (cs, ps)
        if fp not in unique_k1:
            continue
        k1f = unique_k1[fp]
        if k1f["a"] in matched_k1 or k1f["a"] in seen_k1:
            continue
        seen_k1.add(k1f["a"])
        new_matches.append({
            "k1_addr":  hex(k1f["a"]),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "via":      "cascade_translated_callee_fingerprint",
        })
    return new_matches


def main():
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    # Build the "virtual name" map for TSL:
    # Start with actual TSL names, then overlay cascade-translated names.
    # For any TSL addr that is in the cascade, use the K1 name as a stand-in.
    tsl_vname = dict(tsl_name)
    cascade_translated = 0
    for m in cas["matches"]:
        ta = parse_hex(m["tsl_addr"])
        if tsl_vname.get(ta, "").startswith("FUN_"):
            # Translate this FUN_xxx to its K1 name
            tsl_vname[ta] = m["name"]
            cascade_translated += 1

    print("Cascade translations applied to tsl_vname: {:,}".format(cascade_translated))
    print("Total TSL FUN_xxx remaining: {:,}".format(
        sum(1 for f in tsl_cg["functions"]
            if f["n"].startswith("FUN_") and f["a"] not in matched_tsl)))

    all_new = []
    total_before = len(matched_tsl)

    for wave in range(1, MAX_WAVES + 1):
        wm = run_wave(tsl_cg["functions"], k1_cg["functions"],
                      tsl_vname, k1_name,
                      matched_tsl, matched_k1)
        if not wm:
            print("Wave {}: 0 new matches — converged".format(wave))
            break
        print("Wave {}: {} new matches".format(wave, len(wm)))
        for m in wm[:5]:
            print("  {} -> {}  {}".format(m["tsl_addr"], m["k1_addr"], m["name"]))
        all_new.extend(wm)
        for m in wm:
            ta = parse_hex(m["tsl_addr"])
            matched_tsl.add(ta)
            matched_k1.add(parse_hex(m["k1_addr"]))
            # Feed back virtual name for next wave
            tsl_vname[ta] = m["name"]

    print("\nTotal new matches: {}  ({} -> {})".format(
        len(all_new), total_before, len(matched_tsl)))

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(all_new, fh, indent=2)
    print("Wrote {} to {}".format(len(all_new), OUT))


if __name__ == "__main__":
    main()
