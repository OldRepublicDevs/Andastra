"""
Targeted callee-intersection matching for the 144 K1 VM commands not in cascade data.
Finds likely TSL counterparts by shared callee sets.
"""
import json
import sys
import os

script_dir = os.path.dirname(os.path.abspath(__file__))
repo_root = os.path.dirname(script_dir)

def load(name):
    return json.load(open(os.path.join(repo_root, "docs", name)))

k1_cg = load("k1_call_graph.json")
tsl_cg = load("tsl_call_graph.json")
sigs = load("kotor_function_signatures.json")
cascade = load("k1_tsl_cascade_matches.json")
unmatched = load("k1_unmatched_vm_commands.json")

# Build K1->TSL mapping
k1_to_tsl = {}
for f in sigs["functions"]:
    a = f.get("addresses", {})
    k, t = a.get("k1"), a.get("tsl")
    if k and t:
        k1_to_tsl[int(k, 16)] = int(t, 16)
for m in cascade["matches"]:
    ka, ta = m.get("k1_addr"), m.get("tsl_addr")
    if ka and ta:
        ki = int(ka, 16) if isinstance(ka, str) else ka
        ti = int(ta, 16) if isinstance(ta, str) else ta
        k1_to_tsl[ki] = ti

# TSL call graph indexed by TSL addr
tsl_by_addr = {f["a"]: f for f in tsl_cg["functions"]}
k1_by_addr = {f["a"]: f for f in k1_cg["functions"]}

# Inverted TSL index: tsl_callee_addr -> list of tsl callers
tsl_callee_to_callers = {}
for f in tsl_cg["functions"]:
    for c in f.get("c", []):
        if c not in tsl_callee_to_callers:
            tsl_callee_to_callers[c] = []
        tsl_callee_to_callers[c].append(f["a"])

# All already-matched TSL addresses
matched_tsl = set(k1_to_tsl.values())

# Also exclude TSL-exclusive function addresses (NWScript commands only in TSL)
# These must NOT be assigned to K1 functions as their "equivalents"
TSL_EXCLUSIVE_ADDRS = {
    0x0078EDF0,  # ExecuteCommandForceHeartbeat
    0x0079C250,  # ExecuteCommandAdjustCreatureAttributes
    0x0079C4B0,  # ExecuteCommandAdjustCreatureSkills
    0x0079C5C0,  # ExecuteCommandModifyBaseSavingThrow
    0x0079C800,  # ExecuteCommandInfluence
    0x0079DB00,  # ExecuteCommandGrantAbility
    0x007A1340,  # ExecuteCommandIsStealthed
    0x007A8330,  # ExecuteCommandShowPartySelection
    0x007AF2F0,  # ExecuteCommandGetScriptParameter
    0x007AF710,  # ExecuteCommandSetBonusForcePoints
    0x007B06B0,  # ExecuteCommandIsRunning
}
matched_tsl.update(TSL_EXCLUSIVE_ADDRS)

MIN_SHARED = 2
# Quality gate: reject matches where too many candidates (ambiguous)
MAX_CANDIDATES_FOR_HIGH_CONFIDENCE = 200

new_matches = []
no_candidates = []

for cmd in unmatched["commands"]:
    k1a = int(cmd["addr"], 16)
    k1f = k1_by_addr.get(k1a)
    if not k1f:
        no_candidates.append((cmd["name"], "no K1 graph entry"))
        continue

    k1_callees = k1f.get("c", [])
    tsl_callees = [k1_to_tsl[c] for c in k1_callees if c in k1_to_tsl]
    if len(tsl_callees) < MIN_SHARED:
        no_candidates.append((cmd["name"], "too few mapped callees ({})".format(len(tsl_callees))))
        continue

    tsl_callee_set = set(tsl_callees)

    # Find TSL functions that call at least MIN_SHARED of these TSL callees
    candidate_counts = {}
    for tc in tsl_callee_set:
        for caller in tsl_callee_to_callers.get(tc, []):
            candidate_counts[caller] = candidate_counts.get(caller, 0) + 1

    candidates = [
        (addr, cnt)
        for addr, cnt in candidate_counts.items()
        if cnt >= MIN_SHARED and addr not in matched_tsl
        and addr not in TSL_EXCLUSIVE_ADDRS
    ]

    if not candidates:
        no_candidates.append((cmd["name"], "no unmatched TSL candidate with >= {} shared callees".format(MIN_SHARED)))
        continue

    k1_total = len(k1_callees)
    candidates.sort(key=lambda x: (-x[1], abs(len(tsl_by_addr.get(x[0], {}).get("c", [])) - k1_total)))
    best_tsl = candidates[0][0]
    best_shared = candidates[0][1]

    tsl_f = tsl_by_addr.get(best_tsl, {})
    tsl_name = tsl_f.get("n", "FUN_???")
    tsl_auto = tsl_f.get("auto", True)

    new_matches.append({
        "k1_addr": cmd["addr"],
        "k1_name": cmd["name"],
        "k1_class": "CSWVirtualMachineCommands",
        "k1_method": cmd["name"],
        "tsl_addr": "0x{:08X}".format(best_tsl),
        "tsl_name": tsl_name,
        "tsl_is_auto": tsl_auto,
        "shared_callees": best_shared,
        "k1_total_callees": k1_total,
        "candidate_count": len(candidates),
        "confidence": "high" if len(candidates) <= 5 else ("medium" if len(candidates) <= 30 else "low"),
    })
    matched_tsl.add(best_tsl)

print("New matches found: {}".format(len(new_matches)))
print("No candidates: {}".format(len(no_candidates)))
print()
print("=== MATCHES ===")
for m in new_matches:
    auto = " (FUN_xxx)" if m["tsl_is_auto"] else " [NAMED]"
    print("  {} @ {}".format(m["k1_name"], m["k1_addr"]))
    print("    -> {}{}  @ {}  (shared={}/{} callees, {} candidates)".format(
        m["tsl_name"], auto, m["tsl_addr"],
        m["shared_callees"], m["k1_total_callees"], m["candidate_count"]))
print()
print("=== NO CANDIDATES ===")
for name, reason in no_candidates:
    print("  {}: {}".format(name, reason))

# Save
out = {
    "total": len(new_matches),
    "matches": new_matches,
    "no_candidates": [{"name": n, "reason": r} for n, r in no_candidates]
}
out_path = os.path.join(repo_root, "docs", "k1_vm_final_matches.json")
with open(out_path, "w") as f:
    json.dump(out, f, indent=2)
print()
print("Written:", out_path)
