"""
Voting callee projection: for each TSL FUN_xxx, find K1 functions that call
the most of its cascade-mapped callee equivalents. Accept if the winner has
enough votes AND its size is close to TSL.

This is softer than strict intersection — it doesn't require ALL K1 callees
to share a common K1 caller, just a majority.
"""
import json
from collections import defaultdict, Counter

CASCADE   = "docs/k1_tsl_cascade_matches.json"
K1_CG     = "docs/k1_call_graph.json"
TSL_CG    = "docs/tsl_call_graph.json"
K1_BYTES  = "docs/k1_function_bytes.json"
TSL_BYTES = "docs/tsl_function_bytes.json"
OUT       = "docs/tsl_vote_projection_matches.json"

MIN_VOTES   = 3     # minimum number of agreeing callee anchors
SIZE_TOL    = 0.45  # TSL/K1 size ratio must be within this
LEAD_FACTOR = 2.0   # winner must have at least LEAD_FACTOR × votes of 2nd place

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

# TSL cascade lookup: TSL addr int -> K1 addr int
tsl_to_k1 = {addr_int(m["tsl_addr"]): addr_int(m["k1_addr"]) for m in cas["matches"]}
k1_taken  = set(tsl_to_k1.values())

# Build K1 callee -> callers index (excluding taken K1 callers)
k1_callee_to_callers = defaultdict(set)
for addr, f in k1cg.items():
    if addr in k1_taken:
        continue
    for c in f.get("c", []):
        k1_callee_to_callers[c].add(addr)

results  = []
seen_k1  = set()

for tsl_addr, f in tslcg.items():
    tsl_n = tsl_name.get(tsl_addr, "")
    if not tsl_n.startswith("FUN_"):
        continue
    if tsl_addr in tsl_to_k1:
        continue

    # Map TSL callees through cascade
    cascade_mapped_k1 = [tsl_to_k1[c] for c in f.get("c", []) if c in tsl_to_k1]
    if len(cascade_mapped_k1) < MIN_VOTES:
        continue

    # Vote: how many mapped callees does each K1 function (caller) share?
    votes = Counter()
    for k1c in cascade_mapped_k1:
        for k1_caller in k1_callee_to_callers.get(k1c, set()):
            votes[k1_caller] += 1

    if not votes:
        continue

    top2 = votes.most_common(2)
    winner, win_votes = top2[0]

    if win_votes < MIN_VOTES:
        continue

    # Check lead factor
    if len(top2) >= 2:
        second_votes = top2[1][1]
        if second_votes > 0 and win_votes / second_votes < LEAD_FACTOR:
            continue  # too close a tie

    # Skip if winner already taken
    k1_hex  = hex(winner)
    tsl_hex = hex(tsl_addr)
    if k1_hex in existing_k1 or k1_hex in seen_k1:
        continue

    # Size check
    tsl_size = tslsz.get(tsl_addr, 0)
    k1_size  = k1sz.get(winner, 0)
    if tsl_size and k1_size:
        ratio = tsl_size / k1_size
        if not ((1 - SIZE_TOL) <= ratio <= (1 + SIZE_TOL)):
            continue

    full_name = k1_name.get(winner, "")
    if "::" in full_name:
        cls, method = full_name.split("::", 1)
    else:
        cls, method = "", full_name

    seen_k1.add(k1_hex)
    results.append({
        "tsl_addr": tsl_hex, "k1_addr": k1_hex,
        "name":   full_name, "class": cls, "method": method,
        "via":    "vote_callee_projection",
        "votes":  win_votes,
        "total_anchors": len(cascade_mapped_k1),
        "tsl_sz": tsl_size, "k1_sz": k1_size,
    })

# Remove within-results K1 collisions
k1_use = Counter(r["k1_addr"] for r in results)
results = [r for r in results if k1_use[r["k1_addr"]] == 1]

results.sort(key=lambda x: -x["votes"])
print("Vote projection results: {}".format(len(results)))
for r in results[:15]:
    print("  {tsl_addr} -> {k1_addr}  votes={votes}/{total_anchors}  {name}".format(**r))

with open(OUT, "w", encoding="utf-8") as fh:
    json.dump(results, fh, indent=2)
print("Wrote {} to {}".format(len(results), OUT))
