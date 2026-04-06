#!/usr/bin/env python3
"""
Second-pass cascade BFS: uses ALL existing 2460 pairs as seeds.
The 1314 shared-callee matches open new BFS branches not reachable before.
"""
import json, os
from collections import deque, Counter

REPO   = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
K1_CG  = os.path.join(REPO, 'docs', 'k1_call_graph.json')
TSL_CG = os.path.join(REPO, 'docs', 'tsl_call_graph.json')
CAS    = os.path.join(REPO, 'docs', 'k1_tsl_cascade_matches.json')
SIGS   = os.path.join(REPO, 'docs', 'kotor_function_signatures.json')

k1_data  = json.load(open(K1_CG,  encoding='utf-8'))
tsl_data = json.load(open(TSL_CG, encoding='utf-8'))
sig_data = json.load(open(SIGS,   encoding='utf-8'))
cas_data = json.load(open(CAS,    encoding='utf-8'))

k1_by_addr  = {f['a']: f for f in k1_data['functions']}
tsl_by_addr = {f['a']: f for f in tsl_data['functions']}

def is_auto(n):
    return n.startswith(('FUN_', 'LAB_', 'DAT_', 'thunk_FUN_', 'SUB_'))

matched = {}
for e in sig_data['functions']:
    a, n = e.get('addresses', {}), e.get('name', '')
    if 'k1' in a and 'tsl' in a and not n.startswith(('FUN_','LAB_','DAT_')):
        matched[int(a['k1'], 16)] = int(a['tsl'], 16)
for p in cas_data.get('matches', []):
    matched[int(p['k1_addr'], 16)] = int(p['tsl_addr'], 16)

print("Loaded %d existing pairs as seeds" % len(matched))

visited_k1 = set(matched.keys())
queue      = deque(matched.keys())
new_pairs  = []

while queue:
    k1_a  = queue.popleft()
    tsl_a = matched.get(k1_a)
    if tsl_a is None:
        continue
    k1_f  = k1_by_addr.get(k1_a)
    tsl_f = tsl_by_addr.get(tsl_a)
    if not k1_f or not tsl_f:
        continue
    k1_c  = k1_f.get('c', [])
    tsl_c = tsl_f.get('c', [])
    if len(k1_c) != len(tsl_c) or not k1_c:
        continue
    for k1_t, tsl_t in zip(k1_c, tsl_c):
        if k1_t in visited_k1:
            continue
        visited_k1.add(k1_t)
        kf = k1_by_addr.get(k1_t)
        tf = tsl_by_addr.get(tsl_t)
        if not kf or not tf:
            continue
        if is_auto(kf['n']) or not is_auto(tf['n']):
            continue
        qname = (kf.get('ns','') + '::' + kf['n']) if kf.get('ns') else kf['n']
        matched[k1_t] = tsl_t
        new_pairs.append({
            'k1_addr':  '0x%08X' % k1_t,
            'tsl_addr': '0x%08X' % tsl_t,
            'name':     qname,
            'class':    kf.get('ns',''),
            'method':   kf['n'],
            'via':      'cascade2_from_0x%08X' % k1_a,
        })
        queue.append(k1_t)

print("New cascade-2 pairs: %d" % len(new_pairs))
top = Counter(p['class'] for p in new_pairs if p['class']).most_common(15)
for cls, cnt in top:
    print("  %-40s %d" % (cls, cnt))

if new_pairs:
    cas_data['matches'].extend(new_pairs)
    cas_data['cascade2_pairs'] = len(new_pairs)
    cas_data['total_pairs'] = len(cas_data['matches'])
    with open(CAS, 'w', encoding='utf-8') as f:
        json.dump(cas_data, f, indent=2)
    print("Total in file: %d" % len(cas_data['matches']))
else:
    print("No new cascade-2 pairs (BFS fully exhausted).")
