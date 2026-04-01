"""
Projected intersection with size tiebreak:
When callee-projection gives 2-N K1 candidates (instead of 1), apply size
matching to pick the best one.  Only accept if the closest-size candidate is
significantly better than the rest (ratio gap threshold).
"""
import json
from collections import defaultdict

CASCADE   = "docs/k1_tsl_cascade_matches.json"
K1_CG     = "docs/k1_call_graph.json"
TSL_CG    = "docs/tsl_call_graph.json"
K1_BYTES  = "docs/k1_function_bytes.json"
TSL_BYTES = "docs/tsl_function_bytes.json"
OUT       = "docs/tsl_proj_size_tiebreak_matches.json"

# Only consider projections with >= MIN_ANCHORS callee anchors
MIN_ANCHORS = 3
# Accept match if closest-size K1 is within SIZE_TOL *and* next-closest is at
# least GAP_FACTOR worse in size-distance
SIZE_TOL   = 0.35
GAP_FACTOR = 2.0   # best size ratio must be at least 2x better than 2nd best
MAX_CANDIDATES = 8  # skip if there are too many K1 candidates

def load_cg(path):
    data = json.load(open(path, encoding="utf-8"))
    return {f["a"]: f for f in data.get("functions", data)}

def load_sizes(path):
    data = json.load(open(path, encoding="utf-8"))
    return {int(f["a"], 16) if isinstance(f["a"], str) else f["a"]: f.get("sz", 0)
            for f in data.get("functions", data)}

k1cg  = load_cg(K1_CG)
tslcg = load_cg(TSL_CG)
k1sz  = load_sizes(K1_BYTES)
tslsz = load_sizes(TSL_BYTES)

cas = json.load(open(CASCADE, encoding="utf-8"))
existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
existing_k1  = {m["k1_addr"]  for m in cas["matches"]}

def addr_int(a): return int(a, 16) if isinstance(a, str) else a

k1_name  = {f["a"]: f["n"] for f in k1cg.values()}
tsl_name = {f["a"]: f["n"] for f in tslcg.values()}

# Cascade lookup: TSL addr -> K1 addr (int)
tsl_to_k1 = {addr_int(m["tsl_addr"]): addr_int(m["k1_addr"]) for m in cas["matches"]}
k1_taken  = set(tsl_to_k1.values())

# Build K1 callee->callers reverse index
k1_callee_to_callers = defaultdict(set)
for addr, f in k1cg.items():
    for c in f.get("c", []):
        if c not in k1_taken:
            k1_callee_to_callers[c].add(addr)

results = []
seen_k1  = set()

for tsl_addr, f in tslcg.items():
    name = tsl_name.get(tsl_addr, "")
    if not name.startswith("FUN_"):
        continue
    if tsl_addr in {addr_int(m["tsl_addr"]) for m in cas["matches"]}:
        continue

    # Collect cascade-mapped callees of TSL function
    cascade_mapped = []
    for c in f.get("c", []):
        if c in tsl_to_k1:
            cascade_mapped.append(tsl_to_k1[c])

    if len(cascade_mapped) < MIN_ANCHORS:
        continue

    # Intersect K1 callers of those mapped callees
    candidate_sets = [k1_callee_to_callers.get(k, set()) for k in cascade_mapped]
    k1_candidates = candidate_sets[0].copy()
    for s in candidate_sets[1:]:
        k1_candidates &= s

    # Remove already-matched K1 functions (those in k1_taken set)
    k1_candidates -= k1_taken

    if len(k1_candidates) < 2 or len(k1_candidates) > MAX_CANDIDATES:
        continue

    # Size tiebreak
    tsl_size = tslsz.get(tsl_addr, 0)
    if tsl_size == 0:
        continue

    def size_dist(k1a):
        sz = k1sz.get(k1a, 0)
        if sz == 0:
            return 999.0
        return abs(tsl_size / sz - 1.0)

    sorted_by_size = sorted(k1_candidates, key=size_dist)
    best  = sorted_by_size[0]
    dist0 = size_dist(best)

    if dist0 > SIZE_TOL:
        continue  # best candidate is too far in size

    if len(sorted_by_size) >= 2:
        dist1 = size_dist(sorted_by_size[1])
        if dist1 == 0 or dist0 / max(dist1, 0.001) > (1 / GAP_FACTOR):
            continue  # 2nd best too close to best

    # Accept
    k1a     = best
    k1_hex  = hex(k1a)
    tsl_hex = hex(tsl_addr)
    if k1_hex in existing_k1 or k1_hex in seen_k1:
        continue

    full_name = k1_name.get(k1a, "")
    if "::" in full_name:
        cls, method = full_name.split("::", 1)
    else:
        cls, method = "", full_name

    seen_k1.add(k1_hex)
    results.append({
        "tsl_addr": tsl_hex, "k1_addr": k1_hex,
        "name": full_name, "class": cls, "method": method,
        "via": "projected_callee_size_tiebreak",
        "anchors": len(cascade_mapped),
        "candidates": len(k1_candidates),
        "tsl_sz": tsl_size, "k1_sz": k1sz.get(k1a, 0),
        "size_dist0": round(dist0, 3),
        "size_dist1": round(size_dist(sorted_by_size[1]), 3) if len(sorted_by_size) >= 2 else None,
    })

# Remove within-results K1 collisions
from collections import Counter
k1_use = Counter(r["k1_addr"] for r in results)
results = [r for r in results if k1_use[r["k1_addr"]] == 1]

results.sort(key=lambda x: (-x["anchors"], x.get("size_dist0", 1)))
print("Resolved {} via callee-projection + size tiebreak".format(len(results)))
for r in results[:15]:
    print("  {tsl_addr} -> {k1_addr}  anchors={anchors}  cands={candidates}  "
          "sz_d={size_dist0:.3f}/{size_dist1}  {name}".format(**r))

with open(OUT, "w", encoding="utf-8") as fh:
    json.dump(results, fh, indent=2)
print("Wrote {} to {}".format(len(results), OUT))
