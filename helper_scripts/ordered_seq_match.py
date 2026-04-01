"""
Ordered callee sequence fingerprinting.

Uses the ORDERED sequence of callee sizes (not a frozenset) as a fingerprint.
More discriminating than the frozenset approach, and can also combine
  (ordered_callee_sizes, named_callee_frozenset) as a compound key.

Also tries a hybrid: named callees + sizes of unnamed callees combined.
"""
import json
from collections import defaultdict

K1_CG    = "docs/k1_call_graph.json"
TSL_CG   = "docs/tsl_call_graph.json"
K1_BYTES = "docs/k1_function_bytes.json"
TSL_BYTES= "docs/tsl_function_bytes.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_ordered_seq_matches.json"

MIN_SCORE = 4   # minimum "score" = named_callees*2 + unnamed_with_size

def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

def load_sizes(path):
    data = json.load(open(path, encoding="utf-8"))
    funcs = data.get("functions", data) if isinstance(data, dict) else data
    return {parse_hex(e["a"]): int(e.get("sz", 0))
            for e in funcs if isinstance(e, dict)}

def main():
    k1_cg   = json.load(open(K1_CG,   encoding="utf-8"))
    tsl_cg  = json.load(open(TSL_CG,  encoding="utf-8"))
    cas     = json.load(open(CASCADE,  encoding="utf-8"))
    k1_size  = load_sizes(K1_BYTES)
    tsl_size = load_sizes(TSL_BYTES)

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # ---- Build fingerprint for each function ----
    # fp = (frozenset(named_callees), tuple(sorted(sizes_of_all_callees)))
    def make_fp_k1(f):
        callees = f.get("c", [])
        named = frozenset(k1_name.get(c,"") for c in callees if k1_name.get(c,""))
        sizes = tuple(sorted(k1_size.get(c, 0) for c in callees))
        return (named, sizes)

    def make_fp_tsl(f):
        callees = f.get("c", [])
        named = frozenset(
            tsl_name.get(c,"") for c in callees
            if tsl_name.get(c,"") and not tsl_name.get(c,"").startswith("FUN_")
        )
        sizes = tuple(sorted(tsl_size.get(c, 0) for c in callees))
        return (named, sizes)

    def score_fp(fp):
        named, sizes = fp
        n_named = len(named)
        n_sized = sum(1 for s in sizes if s > 0)
        return n_named * 2 + n_sized

    k1_fp_map = {}
    for f in k1_cg["functions"]:
        if not f.get("c"):
            continue
        fp = make_fp_k1(f)
        if score_fp(fp) < MIN_SCORE:
            continue
        k1_fp_map.setdefault(fp, []).append(f)
    unique_k1 = {fp: lst[0] for fp, lst in k1_fp_map.items() if len(lst) == 1}
    print("Unique K1 fingerprints: {:,}".format(len(unique_k1)))

    matches = []
    seen_k1 = set()
    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        if not f.get("c"):
            continue
        fp = make_fp_tsl(f)
        if score_fp(fp) < MIN_SCORE:
            continue
        if fp not in unique_k1:
            continue
        k1f = unique_k1[fp]
        if k1f["a"] in matched_k1 or k1f["a"] in seen_k1:
            continue
        seen_k1.add(k1f["a"])
        matches.append({
            "k1_addr":  hex(k1f["a"]),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "score":    score_fp(fp),
            "via":      "ordered_callee_seq_fingerprint",
        })

    matches.sort(key=lambda x: -x["score"])
    print("Matches: {}".format(len(matches)))
    for m in matches[:30]:
        print("  {} -> {:12s}  score={:2d}  {}".format(
            m["tsl_addr"], m["k1_addr"], m["score"], m["name"]))

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))

if __name__ == "__main__":
    main()
