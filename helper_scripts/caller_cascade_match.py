#!/usr/bin/env python3
"""
Caller-based cascade matching.

For each known K1↔TSL pair (k1_a, tsl_a):
- Build inverted call index: function_addr -> set of its callers
- Find K1 functions that CALL k1_a but are not yet matched
- Find TSL functions that CALL tsl_a but are not yet matched
- If there is exactly 1 unmatched K1 caller AND exactly 1 unmatched TSL caller → potential pair
- Score by: how many of the K1 caller's OTHER callees also map to the TSL caller's callees

This propagates UPSTREAM – cascading from leaves/mid up to root callers.

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

MIN_CONFIRMATION = 2  # caller must share at least this many mapped callee links to confirm
STRICT_UNIQUE    = True  # only accept unique (1-to-1) caller matches

print("Loading data...")
k1_data  = json.load(open(K1_CG,  encoding='utf-8'))
tsl_data = json.load(open(TSL_CG, encoding='utf-8'))
sig_data = json.load(open(SIGS,   encoding='utf-8'))
cas_data = json.load(open(CASCADE, encoding='utf-8'))

k1_by_addr  = {f['a']: f for f in k1_data['functions']}
tsl_by_addr = {f['a']: f for f in tsl_data['functions']}

def is_auto(name):
    return name.startswith(('FUN_', 'LAB_', 'DAT_', 'thunk_FUN_', 'SUB_'))

# Build full K1→TSL matched map
k1_to_tsl = {}
for e in sig_data['functions']:
    a = e.get('addresses', {})
    n = e.get('name', '')
    if 'k1' in a and 'tsl' in a and not n.startswith(('FUN_','LAB_','DAT_')):
        k1_to_tsl[int(a['k1'], 16)] = int(a['tsl'], 16)
for p in cas_data.get('matches', []):
    k1_to_tsl[int(p['k1_addr'], 16)] = int(p['tsl_addr'], 16)

tsl_to_k1 = {v: k for k, v in k1_to_tsl.items()}
print("  Existing matched pairs: %d" % len(k1_to_tsl))

# Build INVERTED call indexes (callee -> set of callers)
print("  Building inverted call indexes...")
k1_inv = {}   # k1_callee_addr -> set(k1_caller_addrs)
for f in k1_data['functions']:
    for c in f['c']:
        k1_inv.setdefault(c, set()).add(f['a'])

tsl_inv = {}  # tsl_callee_addr -> set(tsl_caller_addrs)
for f in tsl_data['functions']:
    for c in f['c']:
        tsl_inv.setdefault(c, set()).add(f['a'])

print("  K1 inverted index: %d entries" % len(k1_inv))
print("  TSL inverted index: %d entries" % len(tsl_inv))

# Auto-named function sets for quick lookup
auto_k1  = {f['a'] for f in k1_data['functions']  if is_auto(f['n'])}
auto_tsl = {f['a'] for f in tsl_data['functions'] if is_auto(f['n'])}

matched_k1  = set(k1_to_tsl.keys())
matched_tsl = set(k1_to_tsl.values())

print("  Unmatched K1 auto-named: %d" % len([a for a in auto_k1 if a not in matched_k1]))
print("  Unmatched TSL auto-named: %d" % len([a for a in auto_tsl if a not in matched_tsl]))

new_pairs = []
already_matched = set(k1_to_tsl.keys())
existing_tsl_matched = set(k1_to_tsl.values())

confident     = 0
confirmed     = 0
ambiguous     = 0
no_k1_callers = 0
no_tsl_callers= 0

# Iterative: each round new confirmed pairs expand scope
rounds = 0
MAX_ROUNDS = 5
current_k1_to_tsl = dict(k1_to_tsl)

while rounds < MAX_ROUNDS:
    rounds += 1
    round_new = []
    current_matched_k1  = set(current_k1_to_tsl.keys())
    current_matched_tsl = set(current_k1_to_tsl.values())

    for k1_a, tsl_a in list(current_k1_to_tsl.items()):
        # Find unmatched K1 callers of k1_a
        k1_callers = k1_inv.get(k1_a, set())
        unmatched_k1_callers = [c for c in k1_callers
                                 if c not in current_matched_k1 and is_auto(k1_by_addr.get(c, {}).get('n', 'FUN_'))]

        tsl_callers = tsl_inv.get(tsl_a, set())
        unmatched_tsl_callers = [c for c in tsl_callers
                                  if c not in current_matched_tsl and is_auto(tsl_by_addr.get(c, {}).get('n', 'FUN_'))]

        if not unmatched_k1_callers:
            no_k1_callers += 1
            continue
        if not unmatched_tsl_callers:
            no_tsl_callers += 1
            continue

        # For each unmatched K1 caller, score against TSL callers
        for k1_caller in unmatched_k1_callers:
            kf = k1_by_addr.get(k1_caller)
            if not kf:
                continue
            if kf['n'].startswith(('FUN_', 'LAB_')):
                # k1 caller is still unnamed, skip
                continue

            # K1 caller is named! map its other callees
            k1_caller_callees = set(kf['c'])
            mapped_k1_callees = {c for c in k1_caller_callees if c in current_k1_to_tsl}

            if len(mapped_k1_callees) < MIN_CONFIRMATION:
                continue

            # Expected TSL callees
            expected_tsl_callees = {current_k1_to_tsl[c] for c in mapped_k1_callees}

            # Score each unmatched TSL caller
            best_tsl = None
            best_score = -1
            tie = False

            for tsl_caller in unmatched_tsl_callers:
                tf = tsl_by_addr.get(tsl_caller)
                if not tf:
                    continue
                tsl_caller_callees = set(tf['c'])
                shared = len(expected_tsl_callees & tsl_caller_callees)
                if shared > best_score:
                    best_score = shared
                    best_tsl = tsl_caller
                    tie = False
                elif shared == best_score and best_score > 0:
                    tie = True

            if best_tsl is None or best_score < MIN_CONFIRMATION or tie:
                ambiguous += 1
                continue

            # Record new pair
            k1_name = (kf['ns'] + '::' + kf['n']) if kf['ns'] else kf['n']
            round_new.append({
                'k1_addr':   '0x%08X' % k1_caller,
                'tsl_addr':  '0x%08X' % best_tsl,
                'name':      k1_name,
                'class':     kf['ns'],
                'method':    kf['n'],
                'via':       'caller_cascade_from_0x%08X' % k1_a,
                'shared_callees_confirmation': best_score,
            })
            current_k1_to_tsl[k1_caller] = best_tsl
            current_matched_k1.add(k1_caller)
            current_matched_tsl.add(best_tsl)
            confident += 1

    new_pairs.extend(round_new)
    print("  Round %d: %d new pairs found" % (rounds, len(round_new)))
    if not round_new:
        break

print("\n=== Caller-Cascade Results ===")
print("  Total new pairs: %d" % len(new_pairs))
print("  Ambiguous/skipped: %d" % ambiguous)

top = Counter(p['class'] for p in new_pairs if p['class']).most_common(15)
for cls, cnt in top:
    print("  %-40s %d" % (cls, cnt))

if new_pairs:
    cas_data['matches'].extend(new_pairs)
    n_field = len([p for p in new_pairs])
    cas_data['caller_cascade_pairs'] = n_field
    cas_data['total_pairs'] = len(cas_data['matches'])
    with open(CASCADE, 'w', encoding='utf-8') as f:
        json.dump(cas_data, f, indent=2)
    print("\nTotal in cascade file: %d" % len(cas_data['matches']))
else:
    print("No new caller-cascade pairs found.")
