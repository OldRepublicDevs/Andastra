#!/usr/bin/env python3
"""
Shared-callee intersection matching.

For each K1 named function not yet matched:
- Find which of its callees are already in our K1->TSL matched map
- If >=2 of those callees are known, find the TSL function that calls ALL
  of the corresponding TSL equivalents
- Unique intersection = new match (no strict call-count requirement)

Input:  docs/k1_call_graph.json
        docs/tsl_call_graph.json
        docs/k1_tsl_cascade_matches.json   (existing matches)
        docs/kotor_function_signatures.json (seeds)
Output: docs/k1_tsl_cascade_matches.json   (appended with new matches)
"""
import json
import os
from collections import Counter

REPO      = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
K1_CG     = os.path.join(REPO, 'docs', 'k1_call_graph.json')
TSL_CG    = os.path.join(REPO, 'docs', 'tsl_call_graph.json')
CASCADE   = os.path.join(REPO, 'docs', 'k1_tsl_cascade_matches.json')
SIGS      = os.path.join(REPO, 'docs', 'kotor_function_signatures.json')
OUT       = os.path.join(REPO, 'docs', 'k1_tsl_cascade_matches.json')

MIN_SHARED = 3   # minimum shared mapped callees to attempt a match

print("Loading data...")
k1_data  = json.load(open(K1_CG,  encoding='utf-8'))
tsl_data = json.load(open(TSL_CG, encoding='utf-8'))
sig_data = json.load(open(SIGS,   encoding='utf-8'))

existing_data = {}
if os.path.exists(CASCADE):
    existing_data = json.load(open(CASCADE, encoding='utf-8'))

k1_by_addr  = {f['a']: f for f in k1_data['functions']}
tsl_by_addr = {f['a']: f for f in tsl_data['functions']}

def is_auto(name):
    return name.startswith(('FUN_', 'LAB_', 'DAT_', 'thunk_FUN_', 'SUB_'))

# Build full matched map (seeds + cascade)
k1_to_tsl = {}
for e in sig_data['functions']:
    a = e.get('addresses', {})
    n = e.get('name', '')
    if 'k1' in a and 'tsl' in a and not n.startswith(('FUN_','LAB_','DAT_')):
        k1_to_tsl[int(a['k1'], 16)] = int(a['tsl'], 16)

for p in existing_data.get('matches', []):
    k1_to_tsl[int(p['k1_addr'], 16)] = int(p['tsl_addr'], 16)

print("  Existing matched pairs: %d" % len(k1_to_tsl))

# Build TSL inverted index: tsl_callee_addr -> set(tsl_parent_addrs)
print("  Building TSL inverted call index...")
tsl_inv = {}
for f in tsl_data['functions']:
    for c in f['c']:
        tsl_inv.setdefault(c, set()).add(f['a'])

tsl_to_k1 = {v: k for k, v in k1_to_tsl.items()}  # reverse map

# Auto-named TSL function set
auto_tsl = {f['a'] for f in tsl_data['functions'] if is_auto(f['n'])}

print("  TSL auto-named functions: %d" % len(auto_tsl))
print("  TSL inverted index entries: %d" % len(tsl_inv))

# Shared-callee matching
new_pairs = []
already_matched = set(k1_to_tsl.keys())
existing_tsl_matched = set(k1_to_tsl.values())

confident  = 0
ambiguous  = 0
no_candidates = 0
too_few    = 0

for f in k1_data['functions']:
    if is_auto(f['n']):
        continue
    if f['a'] in already_matched:
        continue

    # Unique callees from this K1 function
    k1_callees_unique = list(dict.fromkeys(f['c']))  # deduped, order preserved

    # Map each callee to TSL equivalent (only those already matched)
    tsl_mapped_callees = []
    for k1_c in k1_callees_unique:
        if k1_c in k1_to_tsl:
            tsl_c = k1_to_tsl[k1_c]
            if tsl_c not in tsl_mapped_callees:
                tsl_mapped_callees.append(tsl_c)

    if len(tsl_mapped_callees) < MIN_SHARED:
        too_few += 1
        continue

    # Find TSL functions that call ALL of these mapped callees
    candidate_set = None
    for tc in tsl_mapped_callees:
        callers = tsl_inv.get(tc, set())
        if candidate_set is None:
            candidate_set = set(callers)
        else:
            candidate_set &= callers
        if not candidate_set:
            break

    if not candidate_set:
        no_candidates += 1
        continue

    # Filter to auto-named TSL functions not already matched
    auto_candidates = [a for a in candidate_set
                       if a in auto_tsl and a not in existing_tsl_matched]

    if len(auto_candidates) == 1:
        confident += 1
        tsl_match = auto_candidates[0]

        k1_ns   = f['ns']
        k1_meth = f['n']
        qname   = (k1_ns + '::' + k1_meth) if k1_ns else k1_meth

        k1_to_tsl[f['a']] = tsl_match
        already_matched.add(f['a'])
        existing_tsl_matched.add(tsl_match)
        new_pairs.append({
            'k1_addr':  '0x%08X' % f['a'],
            'tsl_addr': '0x%08X' % tsl_match,
            'name':     qname,
            'class':    k1_ns,
            'method':   k1_meth,
            'via':      'shared_callee_intersection[%d]' % len(tsl_mapped_callees),
            'shared_callee_count': len(tsl_mapped_callees),
        })
    elif len(auto_candidates) > 1:
        ambiguous += 1
        # Try size-based disambiguation: pick closest call count
        best = min(auto_candidates,
                   key=lambda x: abs(len(tsl_by_addr[x]['c']) - len(f['c'])))
        # Only accept if size diff is small
        k1_count = len(f['c'])
        best_count = len(tsl_by_addr[best]['c'])
        if abs(k1_count - best_count) <= 3:
            k1_ns   = f['ns']
            k1_meth = f['n']
            qname   = (k1_ns + '::' + k1_meth) if k1_ns else k1_meth
            k1_to_tsl[f['a']] = best
            already_matched.add(f['a'])
            existing_tsl_matched.add(best)
            new_pairs.append({
                'k1_addr':  '0x%08X' % f['a'],
                'tsl_addr': '0x%08X' % best,
                'name':     qname,
                'class':    k1_ns,
                'method':   k1_meth,
                'via':      'shared_callee_intersection[%d]+size_tiebreak[%d]' % (len(tsl_mapped_callees), len(auto_candidates)),
                'shared_callee_count': len(tsl_mapped_callees),
            })

print("\n=== Shared-Callee Results ===")
print("  Confident (1 candidate):     %d" % confident)
print("  Ambiguous (tied, resolved):  %d" % (len(new_pairs) - confident))
print("  Total new matches:           %d" % len(new_pairs))
print("  Ambiguous unresolved:        %d" % (ambiguous - (len(new_pairs) - confident)))
print("  No candidates:               %d" % no_candidates)
print("  Too few shared callees:      %d" % too_few)

cls_counts = Counter(p['class'] for p in new_pairs if p['class'])
print("\n  Top 25 classes in new matches:")
for cls, cnt in cls_counts.most_common(25):
    print("    %-40s %d" % (cls, cnt))

# Merge with existing cascade matches
all_new = existing_data.get('matches', []) + new_pairs

output = {
    'seed_pairs':        len([e for e in sig_data['functions']
                               if 'k1' in e.get('addresses', {}) and 'tsl' in e.get('addresses', {})]),
    'cascade_pairs':     len(existing_data.get('matches', [])),
    'shared_callee_pairs': len(new_pairs),
    'total_pairs':       len(k1_to_tsl),
    'matches':           all_new,
}
with open(OUT, 'w', encoding='utf-8') as f:
    json.dump(output, f, indent=2)

print("\n  Total K1->TSL mappings: %d" % len(k1_to_tsl))
print("  Written to:", OUT)
