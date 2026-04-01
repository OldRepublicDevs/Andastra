"""
Callee-size fingerprint matching.

Key insight: even without named callees, the SIZES of callee functions can be a
distinctive fingerprint.  Two functions that call the same callees (in the same
binary-size order) are likely the same method — independent of names.

Strategy:
  - For every K1 function (named or unnamed): build fingerprint =
        (callee_count, frozenset of callee sizes)
  - For every TSL FUN_xxx: build the same fingerprint
  - Match when fingerprint is unique on both sides AND callee_count >= MIN_CALLEES

Also tries a secondary match using caller-count fingerprint to disambiguate.
"""
import json
from collections import defaultdict

K1_CG    = "docs/k1_call_graph.json"
TSL_CG   = "docs/tsl_call_graph.json"
K1_BYTES = "docs/k1_function_bytes.json"
TSL_BYTES= "docs/tsl_function_bytes.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_callee_size_matches.json"

MIN_CALLEES = 8   # minimum callee count for reliable fingerprint
MAX_SIZE_DELTA_PCT = 0.80  # optional: filter if function sizes differ by >80%

def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

def main():
    k1_cg   = json.load(open(K1_CG,    encoding="utf-8"))
    tsl_cg  = json.load(open(TSL_CG,   encoding="utf-8"))
    k1_bytes= json.load(open(K1_BYTES, encoding="utf-8"))
    tsl_bytes=json.load(open(TSL_BYTES,encoding="utf-8"))
    cas     = json.load(open(CASCADE,   encoding="utf-8"))

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    # Build size lookup from bytes files  (int addr -> int size)
    k1_size  = {}
    tsl_size = {}
    for e in k1_bytes.get("functions", k1_bytes) if isinstance(k1_bytes, dict) else k1_bytes:
        try:
            k1_size[parse_hex(e["a"])] = int(e.get("sz", 0))
        except Exception:
            pass
    for e in tsl_bytes.get("functions", tsl_bytes) if isinstance(tsl_bytes, dict) else tsl_bytes:
        try:
            tsl_size[parse_hex(e["a"])] = int(e.get("sz", 0))
        except Exception:
            pass

    k1_funcs  = {f["a"]: f for f in k1_cg["functions"]}
    tsl_funcs = {f["a"]: f for f in tsl_cg["functions"]}

    k1_caller_map   = defaultdict(set)
    tsl_caller_map  = defaultdict(set)
    for f in k1_cg["functions"]:
        for c in f.get("c", []):
            k1_caller_map[c].add(f["a"])
    for f in tsl_cg["functions"]:
        for c in f.get("c", []):
            tsl_caller_map[c].add(f["a"])

    # ---- Build K1 fingerprints ----
    # Use (n_callees, frozenset-of-callee-sizes) as primary key
    k1_fp_map = {}
    for f in k1_cg["functions"]:
        callees = f.get("c", [])
        if len(callees) < MIN_CALLEES:
            continue
        callee_sizes = frozenset(k1_size.get(c, 0) for c in callees)
        caller_count = len(k1_caller_map.get(f["a"], set()))
        fp = (len(callees), callee_sizes, caller_count)
        k1_fp_map.setdefault(fp, []).append(f)

    unique_k1 = {fp: flist[0] for fp, flist in k1_fp_map.items() if len(flist) == 1}
    print("Unique K1 fingerprints: {:,}".format(len(unique_k1)))

    # ---- Match TSL FUN_xxx ----
    matches = []
    seen_k1 = set()
    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        callees = f.get("c", [])
        if len(callees) < MIN_CALLEES:
            continue
        callee_sizes = frozenset(tsl_size.get(c, 0) for c in callees)
        caller_count = len(tsl_caller_map.get(f["a"], set()))
        fp = (len(callees), callee_sizes, caller_count)
        if fp not in unique_k1:
            continue
        k1f = unique_k1[fp]
        if k1f["a"] in matched_k1 or k1f["a"] in seen_k1:
            continue

        # Optional sanity: K1 function size and TSL function size should be roughly compatible
        k1_sz  = k1_size.get(k1f["a"], 0)
        tsl_sz = tsl_size.get(f["a"], 0)
        if k1_sz > 0 and tsl_sz > 0:
            ratio = abs(k1_sz - tsl_sz) / max(k1_sz, tsl_sz)
            if ratio > MAX_SIZE_DELTA_PCT:
                continue   # sizes diverge too much

        seen_k1.add(k1f["a"])
        matches.append({
            "k1_addr":  hex(k1f["a"]),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "k1_sz":    k1_sz,
            "tsl_sz":   tsl_sz,
            "n_callees":len(callees),
            "via":      "callee_size_fingerprint",
        })

    matches.sort(key=lambda x: -x["n_callees"])
    print("Matches found: {}".format(len(matches)))
    print("\nTop 20:")
    for m in matches[:20]:
        print("  {} -> {:12s}  n_callees={:2d}  sz={}/{}  {}".format(
            m["tsl_addr"], m["k1_addr"], m["n_callees"],
            m["tsl_sz"], m["k1_sz"], m["name"]))

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))

if __name__ == "__main__":
    main()
