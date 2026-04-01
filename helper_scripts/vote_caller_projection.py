"""
Voting caller projection: for each TSL FUN_xxx T, find T's callers that map
to K1 via cascade (caller anchors). Then vote: which K1 function is called
by the most K1 equivalents of T's callers?

Symmetric to vote_callee_projection but uses the caller graph instead.
"""
import json
from collections import defaultdict, Counter

CASCADE   = "docs/k1_tsl_cascade_matches.json"
K1_CG     = "docs/k1_call_graph.json"
TSL_CG    = "docs/tsl_call_graph.json"
K1_BYTES  = "docs/k1_function_bytes.json"
TSL_BYTES = "docs/tsl_function_bytes.json"
OUT       = "docs/tsl_vote_caller_matches.json"

MIN_VOTES   = 3
SIZE_TOL    = 0.45
LEAD_FACTOR = 2.0

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

tsl_to_k1 = {addr_int(m["tsl_addr"]): addr_int(m["k1_addr"]) for m in cas["matches"]}
k1_taken  = set(tsl_to_k1.values())

# Build TSL callers index: TSL_addr -> [TSL callers]
tsl_callers = defaultdict(set)
for addr, f in tslcg.items():
    for c in f.get("c", []):
        tsl_callers[c].add(addr)

# For each K1 function (not taken), record what it calls
k1_callees = {addr: set(f.get("c", [])) for addr, f in k1cg.items()
              if addr not in k1_taken}

results = []
seen_k1  = set()

for tsl_addr, f in tslcg.items():
    tsl_n = tsl_name.get(tsl_addr, "")
    if not tsl_n.startswith("FUN_"):
        continue
    if tsl_addr in tsl_to_k1:
        continue

    # Find TSL callers that are in cascade
    caller_anchors = [tsl_to_k1[c] for c in tsl_callers.get(tsl_addr, set())
                      if c in tsl_to_k1]
    if len(caller_anchors) < MIN_VOTES:
        continue

    # Vote: for each K1 function kf, how many of caller_anchors call kf?
    # Each unique K1 caller contributes at most 1 vote per callee candidate.
    votes = Counter()
    for k1_caller in set(caller_anchors):  # deduplicate caller_anchors
        # k1_caller is the K1 equiv of a TSL function that calls tsl_addr
        # k1_caller should call the K1 equiv of tsl_addr
        for k1_callee in set(k1cg.get(k1_caller, {}).get("c", [])):
            if k1_callee not in k1_taken and k1_callee >= 0x400000:  # valid PE addr
                votes[k1_callee] += 1

    if not votes:
        continue

    top2 = votes.most_common(2)
    winner, win_votes = top2[0]

    if win_votes < MIN_VOTES:
        continue
    # Correct total_anchors to count unique callers
    total_unique_callers = len(set(caller_anchors))
    if len(top2) >= 2:
        second_votes = top2[1][1]
        if second_votes > 0 and win_votes / second_votes < LEAD_FACTOR:
            continue

    k1_hex  = hex(winner)
    tsl_hex = hex(tsl_addr)
    if k1_hex in existing_k1 or k1_hex in seen_k1:
        continue

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
        "name": full_name, "class": cls, "method": method,
        "via": "vote_caller_projection",
        "votes": win_votes,
        "total_anchors": total_unique_callers,
        "tsl_sz": tsl_size, "k1_sz": k1_size,
    })

# Remove within-results K1 collisions
k1_use = Counter(r["k1_addr"] for r in results)
results = [r for r in results if k1_use[r["k1_addr"]] == 1]

results.sort(key=lambda x: -x["votes"])
print("Vote caller projection results: {}".format(len(results)))
for r in results[:15]:
    print("  {tsl_addr} -> {k1_addr}  votes={votes}/{total_anchors}  {name}".format(**r))

with open(OUT, "w", encoding="utf-8") as fh:
    json.dump(results, fh, indent=2)
print("Wrote {} to {}".format(len(results), OUT))
