#!/usr/bin/env python3
"""
Cascade call-graph matching: start from 707 known K1<->TSL seed pairs,
propagate through call graphs to find TSL equivalents of K1-named functions.

Input:  docs/k1_call_graph.json   (from K1_GOG_full execute-script)
        docs/tsl_call_graph.json  (from TSL_GOG_swkotor2 execute-script)
        docs/kotor_function_signatures.json  (seed pairs)
Output: docs/k1_tsl_cascade_matches.json
"""
import json
import os
from collections import deque, Counter

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
K1_CG  = os.path.join(REPO, 'docs', 'k1_call_graph.json')
TSL_CG = os.path.join(REPO, 'docs', 'tsl_call_graph.json')
SIGS   = os.path.join(REPO, 'docs', 'kotor_function_signatures.json')
OUT    = os.path.join(REPO, 'docs', 'k1_tsl_cascade_matches.json')

print("Loading call graphs...")
k1_data  = json.load(open(K1_CG,  encoding='utf-8'))
tsl_data = json.load(open(TSL_CG, encoding='utf-8'))
sig_data = json.load(open(SIGS,   encoding='utf-8'))

print("  K1:  %d functions" % k1_data['count'])
print("  TSL: %d functions (%d auto-named)" % (tsl_data['count'], tsl_data.get('auto_named', '?')))

# Index by address
k1_by_addr  = {f['a']: f for f in k1_data['functions']}
tsl_by_addr = {f['a']: f for f in tsl_data['functions']}

# Build seeds
seeds = {}
for e in sig_data['functions']:
    a = e.get('addresses', {})
    n = e.get('name', '')
    if 'k1' not in a or 'tsl' not in a:
        continue
    if n.startswith(('FUN_', 'LAB_', 'DAT_')):
        continue
    k1_a  = int(a['k1'], 16)
    tsl_a = int(a['tsl'], 16)
    if k1_a in k1_by_addr and tsl_a in tsl_by_addr:
        seeds[k1_a] = tsl_a

print("  Seeds: %d" % len(seeds))

# Helper
def is_auto(name):
    return name.startswith(('FUN_', 'LAB_', 'DAT_', 'thunk_FUN_', 'SUB_'))

# BFS cascade
matched    = dict(seeds)
visited_k1 = set(seeds.keys())
queue      = deque(seeds.keys())
new_pairs  = []
count_miss = 0
both_named = 0

while queue:
    k1_addr  = queue.popleft()
    tsl_addr = matched[k1_addr]

    k1_func  = k1_by_addr.get(k1_addr)
    tsl_func = tsl_by_addr.get(tsl_addr)
    if k1_func is None or tsl_func is None:
        continue

    k1_calls  = k1_func['c']
    tsl_calls = tsl_func['c']

    if len(k1_calls) != len(tsl_calls) or not k1_calls:
        count_miss += 1
        continue

    for k1_tgt, tsl_tgt in zip(k1_calls, tsl_calls):
        if k1_tgt in visited_k1:
            continue
        visited_k1.add(k1_tgt)

        k1_callee  = k1_by_addr.get(k1_tgt)
        tsl_callee = tsl_by_addr.get(tsl_tgt)
        if k1_callee is None or tsl_callee is None:
            continue
        if is_auto(k1_callee['n']):
            continue
        if not is_auto(tsl_callee['n']):
            both_named += 1
            continue

        # New discovery: K1 named -> TSL auto-named
        k1_ns    = k1_callee['ns']
        k1_meth  = k1_callee['n']
        qname    = (k1_ns + '::' + k1_meth) if k1_ns else k1_meth

        matched[k1_tgt] = tsl_tgt
        new_pairs.append({
            'k1_addr':  '0x%08X' % k1_tgt,
            'tsl_addr': '0x%08X' % tsl_tgt,
            'name':     qname,
            'class':    k1_ns,
            'method':   k1_meth,
            'via_k1':   '0x%08X' % k1_addr,
        })
        queue.append(k1_tgt)

print("\n=== Cascade Results ===")
print("  New K1->TSL matches:  %d" % len(new_pairs))
print("  Total mapped now:     %d" % len(matched))
print("  Count-mismatch skips: %d" % count_miss)
print("  Both already named:   %d" % both_named)

cls_counts = Counter(p['class'] for p in new_pairs if p['class'])
print("\n  Top 25 classes in new matches:")
for cls, cnt in cls_counts.most_common(25):
    print("    %-40s %d" % (cls, cnt))

output = {
    'seed_pairs':  len(seeds),
    'new_pairs':   len(new_pairs),
    'total_pairs': len(matched),
    'stats': {
        'count_mismatch_skips': count_miss,
        'both_already_named':   both_named,
    },
    'matches': new_pairs,
}
with open(OUT, 'w', encoding='utf-8') as f:
    json.dump(output, f, indent=2)

print("\n  Written to:", OUT)
