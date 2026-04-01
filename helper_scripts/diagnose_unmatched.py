"""
Diagnostic: explain WHY 7685 TSL FUN_xxx remain unmatched.
Categorize by why fingerprinting fails:
  A) Zero named callees
  B) Zero named callers
  C) 1+ named but fingerprint non-unique (collision in K1)
  D) Named but K1 counterpart not found (K1 may also be unnamed)
  E) Named, unique K1 match but K1 already paired to another TSL
"""
import json
from collections import defaultdict

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"

GENERIC = {"_free","_malloc","_memcpy","_memset","_memmove","_strlen","_strcpy",
           "_strcat","_strcmp","_sprintf","_printf","__chkstk"}

def parse_hex(s):
    return int(s,16) if isinstance(s,str) else s

def is_generic(n):
    return not n or n.startswith("FUN_") or any(n.startswith(g) for g in GENERIC)

def main():
    k1_cg = json.load(open(K1_CG))
    tsl_cg= json.load(open(TSL_CG))
    cas   = json.load(open(CASCADE))

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}
    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    tsl_caller_map = defaultdict(set)
    for f in tsl_cg["functions"]:
        for c in f.get("c",[]): tsl_caller_map[c].add(f["a"])

    # K1 callee fingerprint index
    k1_fp = defaultdict(list)
    for f in k1_cg["functions"]:
        nc = frozenset(k1_name.get(c,"") for c in f.get("c",[]) if not is_generic(k1_name.get(c,"")))
        if nc: k1_fp[nc].append(f["a"])

    cats = {"A_no_callees":0,"B_no_callers":0,"C_collision":0,"D_no_k1_match":0,
            "E_k1_taken":0,"ok":0}

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl: continue
        nc = frozenset(tsl_name.get(c,"") for c in f.get("c",[]) if not is_generic(tsl_name.get(c,"")))
        cr = frozenset(tsl_name.get(c,"") for c in tsl_caller_map.get(f["a"],set()) if not is_generic(tsl_name.get(c,"")))
        if not nc:
            if not cr: cats["A_no_callees"] += 1
            else: cats["B_no_callers"] += 1
            continue
        if nc not in k1_fp:
            cats["D_no_k1_match"] += 1
            continue
        k1_matches = k1_fp[nc]
        if len(k1_matches) > 1:
            cats["C_collision"] += 1
            continue
        k1a = k1_matches[0]
        if k1a in matched_k1:
            cats["E_k1_taken"] += 1
            continue
        cats["ok"] += 1

    total_fun = sum(cats.values())
    print("TSL FUN_xxx analysis (total={:,})".format(total_fun))
    print()
    for k,v in sorted(cats.items(), key=lambda x:-x[1]):
        pct = 100*v/total_fun if total_fun else 0
        print("  {:20s}  {:5,}  ({:.1f}%)".format(k, v, pct))
    print()
    # Deeper look at D: K1 FUN_xxx count in D category
    d_k1_anon = 0
    d_k1_named_diff = 0
    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl: continue
        nc = frozenset(tsl_name.get(c,"") for c in f.get("c",[]) if not is_generic(tsl_name.get(c,"")))
        if nc and nc not in k1_fp:
            # Is it TSL-only (calls functions not in K1 at all)?
            tsl_unique = [n for n in nc if n not in k1_name.values()]
            if len(tsl_unique) == len(nc):
                d_k1_anon += 1
            else:
                d_k1_named_diff += 1
    print("  D subcategory breakdown:")
    print("    All callees are TSL-only names (no K1 equiv): {:,}".format(d_k1_anon))
    print("    Some callees in K1 but fp not found:         {:,}".format(d_k1_named_diff))

if __name__ == "__main__":
    main()
