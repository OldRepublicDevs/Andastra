#!/usr/bin/env python3
"""
Reverse-callee cascade matching.

For TSL FUN_xxx functions that call ONLY named TSL functions (all with K1 mappings):
- Map TSL callees → K1 equivalents
- Find K1 function that calls ALL those K1 equivalents in the same order
- Unique exact match → new K1↔TSL pair

This is the reverse of cascade_match_pass2.py: starts from TSL side,
looks UP to K1 by callee sequence matching.

Input:  docs/k1_call_graph.json
        docs/tsl_call_graph.json
        docs/k1_tsl_cascade_matches.json
        docs/kotor_function_signatures.json
Output: docs/k1_tsl_cascade_matches.json (appended)
"""
import json
import os
from collections import Counter

REPO    = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
K1_CG   = os.path.join(REPO, 'docs', 'k1_call_graph.json')
TSL_CG  = os.path.join(REPO, 'docs', 'tsl_call_graph.json')
CASCADE = os.path.join(REPO, 'docs', 'k1_tsl_cascade_matches.json')
SIGS    = os.path.join(REPO, 'docs', 'kotor_function_signatures.json')

MIN_CALLEES  = 2   # require ≥2 mapped callees for a match attempt
STRICT_ORDER = True  # require same call ORDER (not just same set)

print("Loading data...")
k1_data  = json.load(open(K1_CG,  encoding='utf-8'))
tsl_data = json.load(open(TSL_CG, encoding='utf-8'))
sig_data = json.load(open(SIGS,   encoding='utf-8'))
cas_data = json.load(open(CASCADE, encoding='utf-8'))

k1_by_addr  = {f['a']: f for f in k1_data['functions']}
tsl_by_addr = {f['a']: f for f in tsl_data['functions']}

def is_auto(name):
    return name.startswith(('FUN_', 'LAB_', 'DAT_', 'thunk_FUN_', 'SUB_'))

# Build full TSL→K1 map
tsl_to_k1 = {}
for e in sig_data['functions']:
    addrs = e.get('addresses', {})
    if 'k1' in addrs and 'tsl' in addrs:
        tsl_to_k1[int(addrs['tsl'], 16)] = int(addrs['k1'], 16)
for p in cas_data.get('matches', []):
    k1a = int(p['k1_addr'], 16)
    tsa = int(p['tsl_addr'], 16)
    tsl_to_k1[tsa] = k1a

# Also build K1→TSL
k1_to_tsl = {v: k for k, v in tsl_to_k1.items()}

print("  Mapped TSL→K1 pairs: %d" % len(tsl_to_k1))
print("  Already matched TSL: %d" % len(tsl_to_k1))

# Build K1 indexed callee list (for exact order matching)
# k1_callee_sig: sorted frozenset of callees → set of k1 addrs
k1_callee_frozenset_idx = {}   # frozenset(k1_callees) → list of k1_addrs
k1_callee_ordered_idx  = {}   # tuple(k1_callees) → list of k1_addrs

print("  Building K1 callee indexes...")
for f in k1_data['functions']:
    if not f['c']:
        continue
    fs = frozenset(f['c'])
    k1_callee_frozenset_idx.setdefault(fs, []).append(f['a'])
    ct = tuple(f['c'])
    k1_callee_ordered_idx.setdefault(ct, []).append(f['a'])

print("  K1 frozenset callee index: %d unique callee sets" % len(k1_callee_frozenset_idx))

# Find TSL FUN_xxx candidate functions (call ONLY mapped named functions)
candidates = []
already_matched_tsl = set(tsl_to_k1.keys())
already_matched_k1  = set(k1_to_tsl.keys())

for f in tsl_data['functions']:
    addr = f['a']
    if not is_auto(f['n']):
        continue
    if addr in already_matched_tsl:
        continue
    callees = f['c']
    if len(callees) < MIN_CALLEES:
        continue
    # All callees must be: (a) named in TSL AND (b) have K1 mapping
    k1_callees = []
    valid = True
    for c in callees:
        cf = tsl_by_addr.get(c)
        if cf is None or is_auto(cf['n']):
            valid = False
            break
        k1_c = tsl_to_k1.get(c)
        if k1_c is None:
            valid = False
            break
        k1_callees.append(k1_c)
    if valid:
        candidates.append({
            'tsl_addr': addr,
            'tsl_callees': callees,
            'k1_expected_callees': k1_callees
        })

print("\n  TSL FUN_xxx candidates (all-named-mapped callees, ≥%d): %d" % (MIN_CALLEES, len(candidates)))

# Match each candidate against K1
new_pairs = []
found_exact = 0
found_set   = 0
ambiguous   = 0
no_match    = 0

for cand in candidates:
    tsl_addr    = cand['tsl_addr']
    k1_expected = cand['k1_expected_callees']
    k1_ordered  = tuple(k1_expected)
    k1_frozenset = frozenset(k1_expected)

    # Try exact order match first
    k1_matches_ordered = [a for a in k1_callee_ordered_idx.get(k1_ordered, [])
                          if a not in already_matched_k1]
    if len(k1_matches_ordered) == 1:
        k1_addr = k1_matches_ordered[0]
        kf = k1_by_addr[k1_addr]
        if is_auto(kf['n']):
            no_match += 1
            continue
        k1_name = (kf['ns'] + '::' + kf['n']) if kf['ns'] else kf['n']
        new_pairs.append({
            'k1_addr':   '0x%08X' % k1_addr,
            'tsl_addr':  '0x%08X' % tsl_addr,
            'name':      k1_name,
            'class':     kf['ns'],
            'method':    kf['n'],
            'via':       'reverse_callee_exact',
            'callee_count': len(k1_expected),
        })
        already_matched_k1.add(k1_addr)
        already_matched_tsl.add(tsl_addr)
        found_exact += 1
        continue

    # Fall back to frozenset match
    k1_matches_set = [a for a in k1_callee_frozenset_idx.get(k1_frozenset, [])
                      if a not in already_matched_k1]
    if len(k1_matches_set) == 1:
        k1_addr = k1_matches_set[0]
        kf = k1_by_addr[k1_addr]
        if is_auto(kf['n']):
            no_match += 1
            continue
        k1_name = (kf['ns'] + '::' + kf['n']) if kf['ns'] else kf['n']
        new_pairs.append({
            'k1_addr':   '0x%08X' % k1_addr,
            'tsl_addr':  '0x%08X' % tsl_addr,
            'name':      k1_name,
            'class':     kf['ns'],
            'method':    kf['n'],
            'via':       'reverse_callee_set',
            'callee_count': len(k1_expected),
        })
        already_matched_k1.add(k1_addr)
        already_matched_tsl.add(tsl_addr)
        found_set += 1
        continue

    if k1_matches_ordered or k1_matches_set:
        ambiguous += 1
    else:
        no_match += 1

print("\n=== Reverse-Callee Results ===")
print("  Exact order matches:  %d" % found_exact)
print("  Set-equivalent:       %d" % found_set)
print("  Total new pairs:      %d" % len(new_pairs))
print("  Ambiguous:            %d" % ambiguous)
print("  No K1 match:          %d" % no_match)

top = Counter(p['class'] for p in new_pairs if p['class']).most_common(20)
for cls, cnt in top:
    print("  %-40s %d" % (cls, cnt))

if new_pairs:
    cas_data['matches'].extend(new_pairs)
    cas_data['reverse_callee_pairs'] = len(new_pairs)
    cas_data['total_pairs'] = len(cas_data['matches'])
    with open(CASCADE, 'w', encoding='utf-8') as f:
        json.dump(cas_data, f, indent=2)
    print("\nTotal in cascade file: %d" % cas_data['total_pairs'])
else:
    print("No new reverse-callee pairs found.")
