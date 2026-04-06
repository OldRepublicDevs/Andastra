"""Diagnose projection candidate bucket distribution."""
import json
from collections import defaultdict

def load_cg(path):
    data = json.load(open(path, encoding='utf-8'))
    return {f['a']: f for f in data.get('functions', data)}

k1cg  = load_cg('docs/k1_call_graph.json')
tslcg = load_cg('docs/tsl_call_graph.json')
cas   = json.load(open('docs/k1_tsl_cascade_matches.json', encoding='utf-8'))

def addr_int(a): return int(a, 16) if isinstance(a, str) else a
tsl_to_k1 = {addr_int(m['tsl_addr']): addr_int(m['k1_addr']) for m in cas['matches']}
k1_taken   = set(tsl_to_k1.values())

k1_callee_to_callers = defaultdict(set)
for addr, f in k1cg.items():
    for c in f.get('c', []):
        if c not in k1_taken:
            k1_callee_to_callers[c].add(addr)

tsl_name = {f['a']: f['n'] for f in tslcg.values()}
bucket: dict[int, int] = defaultdict(int)
examples: dict[int, list[tuple[int, int, set[int]]]] = defaultdict(list)

for addr, f in tslcg.items():
    name = tsl_name.get(addr, '')
    if not name.startswith('FUN_'):
        continue
    if addr in tsl_to_k1:
        continue
    mapped = [tsl_to_k1[c] for c in f.get('c', []) if c in tsl_to_k1]
    if not mapped:
        continue
    cs = [k1_callee_to_callers.get(k, set()) for k in mapped]
    cands = cs[0].copy()
    for s in cs[1:]:
        cands &= s
    cands -= k1_taken
    n = len(cands)
    if n >= 1:
        bucket[n] += 1
        if len(examples[n]) < 3:
            examples[n].append((addr, len(mapped), cands))

print("Candidate count : # TSL functions")
for k, v in sorted(bucket.items())[:25]:
    print("  {:3d} candidates : {:4d} functions".format(k, v))
