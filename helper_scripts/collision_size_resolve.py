"""
Collision resolution: for TSL FUN_xxx with callee fingerprints that ambiguously
match 2+ K1 candidates (C_collision category), use function body SIZE to
eliminate all but one candidate.

Algorithm:
1. Find all C_collision FUN_xxx TSL functions
2. For each, get the list of K1 candidates with matching callee fingerprints
3. Filter candidates by size: TSL size must be within SIZE_TOLERANCE of K1 size
4. If exactly one K1 candidate survives → accept match
5. Minimum callee count guard: only attempt if the fingerprint has >= MIN_NAMED_CALLEES
"""
import json
from collections import defaultdict

CASCADE    = "docs/k1_tsl_cascade_matches.json"
K1_CG      = "docs/k1_call_graph.json"
TSL_CG     = "docs/tsl_call_graph.json"
K1_BYTES   = "docs/k1_function_bytes.json"
TSL_BYTES  = "docs/tsl_function_bytes.json"
OUT        = "docs/tsl_collision_resolved_matches.json"

SIZE_TOLERANCE  = 0.40   # TSL size must be within 40% of K1 size
MIN_NAMED_CALLEES = 2    # minimum named callees for the fingerprint to be useful

def load_cg(path):
    data = json.load(open(path, encoding="utf-8"))
    funcs = data.get("functions", data)
    return {f["a"]: f for f in funcs}

def load_sizes(path):
    data = json.load(open(path, encoding="utf-8"))
    funcs = data.get("functions", data)
    return {int(f["a"], 16) if isinstance(f["a"], str) else f["a"]: f.get("sz", 0)
            for f in funcs}

k1cg = load_cg(K1_CG)
tslcg = load_cg(TSL_CG)
k1sz = load_sizes(K1_BYTES)
tslsz = load_sizes(TSL_BYTES)

cas = json.load(open(CASCADE, encoding="utf-8"))
existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
existing_k1  = {m["k1_addr"]  for m in cas["matches"]}

# Build name maps
def addr_int(a):
    if isinstance(a, str):
        return int(a, 16)
    return a

k1_name = {f["a"]: f["n"] for f in k1cg.values()}
tsl_name = {f["a"]: f["n"] for f in tslcg.values()}

# Build TSL virtual name map (cascade substitution)
tsl_vname = dict(tsl_name)
for m in cas["matches"]:
    ta = addr_int(m["tsl_addr"])
    tsl_vname[ta] = m["name"]

# Build K1 callee-fingerprint index: frozenset(named_callees) -> [k1_addrs]
k1_fp_index = defaultdict(list)
k1_taken_addrs = {addr_int(m["k1_addr"]) for m in cas["matches"]}

for addr, f in k1cg.items():
    if addr in k1_taken_addrs:
        continue
    named = frozenset(k1_name[c] for c in f.get("c", []) if c in k1_name
                      and not k1_name[c].startswith("FUN_"))
    if len(named) >= MIN_NAMED_CALLEES:
        k1_fp_index[named].append(addr)

print("K1 fingerprint index: {} unique fingerprints".format(len(k1_fp_index)))

# Find collision candidates for TSL FUN_xxx
results = []
examined = 0
collision_found = 0

for addr, f in tslcg.items():
    name = tsl_name.get(addr, "")
    if not name.startswith("FUN_"):
        continue
    if addr in {addr_int(m["tsl_addr"]) for m in cas["matches"]}:
        continue

    # Build virtual named callee fingerprint
    named = frozenset(tsl_vname.get(c, tsl_name.get(c, ""))
                      for c in f.get("c", [])
                      if tsl_vname.get(c, tsl_name.get(c, ""))
                      and not tsl_vname.get(c, tsl_name.get(c, "")).startswith("FUN_"))

    if len(named) < MIN_NAMED_CALLEES:
        continue

    k1_candidates = k1_fp_index.get(frozenset(named), [])
    if len(k1_candidates) < 2:
        continue  # not a collision case

    examined += 1
    collision_found += 1

    # Apply size filter
    tsl_size = tslsz.get(addr, 0)
    if tsl_size == 0:
        continue

    size_ok = []
    for k1a in k1_candidates:
        k1_size = k1sz.get(k1a, 0)
        if k1_size == 0:
            continue
        ratio = tsl_size / k1_size
        if (1 - SIZE_TOLERANCE) <= ratio <= (1 + SIZE_TOLERANCE):
            size_ok.append(k1a)

    if len(size_ok) == 1:
        k1a = size_ok[0]
        tsl_hex = hex(addr)
        k1_hex  = hex(k1a)
        if k1_hex in existing_k1 or tsl_hex in existing_tsl:
            continue
        # Get the K1 name for the match
        k1_full_name = k1_name.get(k1a, "")
        # Derive class/method from K1 name
        if "::" in k1_full_name:
            cls, method = k1_full_name.split("::", 1)
        else:
            cls, method = "", k1_full_name

        tsl_size_out = tsl_size
        k1_size_out  = k1sz.get(k1a, 0)
        results.append({
            "tsl_addr": tsl_hex,
            "k1_addr":  k1_hex,
            "name":     k1_full_name,
            "class":    cls,
            "method":   method,
            "via":      "collision_size_resolution",
            "callee_count": len(named),
            "tsl_sz":   tsl_size_out,
            "k1_sz":    k1_size_out,
            "collisions_total": len(k1_candidates),
        })

results.sort(key=lambda x: -x["callee_count"])

# Remove within-results K1 collisions (multiple TSL -> same K1 = still ambiguous)
from collections import Counter
k1_use = Counter(r["k1_addr"] for r in results)
results = [r for r in results if k1_use[r["k1_addr"]] == 1]

print("Examined {} collision functions".format(collision_found))
print("Resolved {} via size filter (tolerance={})".format(len(results), SIZE_TOLERANCE))
for r in results[:15]:
    size_ratio = r["tsl_sz"] / r["k1_sz"] if r["k1_sz"] else 0
    print("  {tsl_addr} -> {k1_addr}  callees={callee_count}  sz_ratio={:.2f}  {name}".format(
        size_ratio, **r))

with open(OUT, "w", encoding="utf-8") as fh:
    json.dump(results, fh, indent=2)
print("Wrote {} to {}".format(len(results), OUT))
