"""
Two targeted passes:

Pass 1 - Single named callee with size tiebreak:
  TSL FUN_xxx has exactly 1 specific named callee, matches exactly 1 K1 function,
  AND function sizes are within SINGLE_SIZE_PCT of each other.

Pass 2 - K1-common subset fingerprint (from D_no_k1_match):
  Strip TSL-unique callee names, match remaining K1-common names against K1.
  Require at least 2 K1-common named callees. Size tiebreak applied.
"""
import json
from collections import defaultdict

K1_CG    = "docs/k1_call_graph.json"
TSL_CG   = "docs/tsl_call_graph.json"
K1_BYTES = "docs/k1_function_bytes.json"
TSL_BYTES= "docs/tsl_function_bytes.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_targeted_passes.json"

SINGLE_SIZE_PCT  = 0.40   # for pass 1: max size ratio diff
SUBSET_MIN       = 2      # for pass 2: min K1-shared named callees
SUBSET_SIZE_PCT  = 0.60   # for pass 2: max size ratio diff

GENERIC = {"_free","_malloc","_memcpy","_memset","_memmove","_strlen","_strcpy",
           "_strcat","_strcmp","_sprintf","_printf","__chkstk","FUN_"}

def parse_hex(s):
    return int(s,16) if isinstance(s,str) else s

def is_generic(n):
    return not n or n.startswith("FUN_") or any(n.startswith(g) for g in GENERIC)

def load_sizes(path):
    data = json.load(open(path, encoding="utf-8"))
    funcs = data.get("functions", data) if isinstance(data, dict) else data
    return {parse_hex(e["a"]): int(e.get("sz",0)) for e in funcs if isinstance(e,dict)}

def size_ok(a, b, pct):
    if a <= 0 or b <= 0: return True  # no size data, don't exclude
    return abs(a-b)/max(a,b) <= pct

def main():
    k1_cg  = json.load(open(K1_CG,    encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG,   encoding="utf-8"))
    cas    = json.load(open(CASCADE,   encoding="utf-8"))
    k1_sz  = load_sizes(K1_BYTES)
    tsl_sz = load_sizes(TSL_BYTES)

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}
    k1_name_set = set(k1_name.values())   # for TSL-unique detection

    # K1 named-callee index
    k1_fp = defaultdict(list)
    for f in k1_cg["functions"]:
        nc = frozenset(k1_name.get(c,"") for c in f.get("c",[]) if not is_generic(k1_name.get(c,"")))
        if nc:
            k1_fp[nc].append(f["a"])

    all_matches = []
    seen_k1 = set()

    # ---- Pass 1: single named callee + size ----
    p1_checked = p1_found = 0
    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl: continue
        nc = [tsl_name.get(c,"") for c in f.get("c",[]) if not is_generic(tsl_name.get(c,""))]
        nc = list(dict.fromkeys(nc))   # deduplicate preserving order
        if len(nc) != 1: continue
        fp = frozenset(nc)
        k1_cands = k1_fp.get(fp, [])
        if len(k1_cands) != 1: continue
        k1a = k1_cands[0]
        if k1a in matched_k1 or k1a in seen_k1: continue
        # size check
        if not size_ok(tsl_sz.get(f["a"],0), k1_sz.get(k1a,0), SINGLE_SIZE_PCT): continue
        p1_checked += 1
        k1f = next(x for x in k1_cg["functions"] if x["a"]==k1a)
        seen_k1.add(k1a)
        all_matches.append({
            "k1_addr":  hex(k1a),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "k1_sz":    k1_sz.get(k1a,0),
            "tsl_sz":   tsl_sz.get(f["a"],0),
            "named_callee": nc[0],
            "pass":     1,
            "via":      "single_callee_size_fingerprint",
        })
        p1_found += 1

    print("Pass 1 (single callee+size): checked {} unique matches, found {}".format(
        p1_checked, p1_found))

    # ---- Pass 2: K1-common subset, ≥2 callees, size check ----
    p2_found = 0
    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl: continue
        tsl_nc_all = frozenset(tsl_name.get(c,"") for c in f.get("c",[]) if not is_generic(tsl_name.get(c,"")))
        if not tsl_nc_all: continue
        # Keep only names that also exist in K1's named functions
        k1_common = frozenset(n for n in tsl_nc_all if n in k1_name_set)
        if len(k1_common) < SUBSET_MIN: continue
        if k1_common not in k1_fp: continue
        k1_cands = k1_fp[k1_common]
        if len(k1_cands) != 1: continue
        k1a = k1_cands[0]
        if k1a in matched_k1 or k1a in seen_k1: continue
        if not size_ok(tsl_sz.get(f["a"],0), k1_sz.get(k1a,0), SUBSET_SIZE_PCT): continue
        k1f = next(x for x in k1_cg["functions"] if x["a"]==k1a)
        seen_k1.add(k1a)
        all_matches.append({
            "k1_addr":  hex(k1a),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "k1_sz":    k1_sz.get(k1a,0),
            "tsl_sz":   tsl_sz.get(f["a"],0),
            "shared_callees": sorted(k1_common)[:4],
            "pass":     2,
            "via":      "k1_common_subset_size_fingerprint",
        })
        p2_found += 1

    print("Pass 2 (K1-common subset+size): found {}".format(p2_found))
    print("Total: {}".format(len(all_matches)))

    for m in all_matches[:30]:
        if m["pass"]==1:
            print("  P1 {} -> {:12s}  sz={}/{}  {}  callee={}".format(
                m["tsl_addr"], m["k1_addr"], m["tsl_sz"], m["k1_sz"],
                m["name"], m["named_callee"][:40]))
        else:
            print("  P2 {} -> {:12s}  sz={}/{}  {}".format(
                m["tsl_addr"], m["k1_addr"], m["tsl_sz"], m["k1_sz"], m["name"]))
            print("      shared:", m["shared_callees"][:3])

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(all_matches, fh, indent=2)
    print("Wrote {} to {}".format(len(all_matches), OUT))

if __name__ == "__main__":
    main()
