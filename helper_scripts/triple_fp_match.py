"""
Triple fingerprint: (own_size_bucket, callee_count, named_callee_frozenset)

For cases where named_callee_frozenset alone has 2+ K1 matches, adding the
function's own size as a tiebreaker may produce a unique match.

"size bucket" = size rounded to nearest 16 bytes to allow small MSVC variation.
"""
import json
from collections import defaultdict

K1_CG    = "docs/k1_call_graph.json"
TSL_CG   = "docs/tsl_call_graph.json"
K1_BYTES = "docs/k1_function_bytes.json"
TSL_BYTES= "docs/tsl_function_bytes.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_triple_fp_matches.json"

MIN_NAMED = 2

BUCKET = 32    # size bucket width in bytes

def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

def load_sizes(path):
    data = json.load(open(path, encoding="utf-8"))
    funcs = data.get("functions", data) if isinstance(data, dict) else data
    return {parse_hex(e["a"]): int(e.get("sz", 0))
            for e in funcs if isinstance(e, dict)}

def bucket(sz):
    if sz <= 0:
        return -1
    return (sz // BUCKET) * BUCKET   # round down to nearest BUCKET

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

    GENERIC = {"__imp__", "__imp", "j_", "??"}

    def named_callees_k1(f):
        return frozenset(
            k1_name.get(c, "") for c in f.get("c", [])
            if k1_name.get(c, "") and not k1_name.get(c, "").startswith("FUN_")
            and not any(k1_name.get(c, "").startswith(p) for p in GENERIC)
        )

    def named_callees_tsl(f):
        return frozenset(
            tsl_name.get(c, "") for c in f.get("c", [])
            if tsl_name.get(c, "") and not tsl_name.get(c, "").startswith("FUN_")
            and not any(tsl_name.get(c, "").startswith(p) for p in GENERIC)
        )

    # ---- Build K1 triple fingerprints ----
    k1_fp_map = {}
    for f in k1_cg["functions"]:
        nc = named_callees_k1(f)
        if len(nc) < MIN_NAMED:
            continue
        fp = (bucket(k1_size.get(f["a"], 0)), len(f.get("c", [])), nc)
        k1_fp_map.setdefault(fp, []).append(f)

    unique_k1 = {fp: lst[0] for fp, lst in k1_fp_map.items() if len(lst) == 1}
    print("Unique K1 triple fingerprints: {:,}".format(len(unique_k1)))

    # Also note how many would be unique on named-callees alone for comparison
    k1_name_only = {}
    for f in k1_cg["functions"]:
        nc = named_callees_k1(f)
        if len(nc) < MIN_NAMED:
            continue
        fp2 = nc
        k1_name_only.setdefault(fp2, []).append(f)
    unique_name_only = sum(1 for lst in k1_name_only.values() if len(lst)==1)
    non_unique = sum(1 for lst in k1_name_only.values() if len(lst)>1)
    print("Named-only unique: {:,}  non-unique groups: {:,}".format(unique_name_only, non_unique))

    matches = []
    seen_k1 = set()
    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        nc = named_callees_tsl(f)
        if len(nc) < MIN_NAMED:
            continue
        fp = (bucket(tsl_size.get(f["a"], 0)), len(f.get("c", [])), nc)
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
            "k1_sz":    k1_size.get(k1f["a"],0),
            "tsl_sz":   tsl_size.get(f["a"],0),
            "n_named":  len(nc),
            "via":      "triple_fp_size_callee_count_named",
        })

    matches.sort(key=lambda x: -x["n_named"])
    print("Matches: {}".format(len(matches)))
    for m in matches[:30]:
        print("  {} -> {:12s}  named={:2d}  sz={}/{}  {}".format(
            m["tsl_addr"], m["k1_addr"], m["n_named"],
            m["tsl_sz"], m["k1_sz"], m["name"]))

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} to {}".format(len(matches), OUT))

if __name__ == "__main__":
    main()
