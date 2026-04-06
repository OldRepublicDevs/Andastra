"""
Remove duplicate TSL and K1 addresses from cascade, keeping highest-confidence entry.
Priority order (via method): exact_byte_match > known_function > callee_exact >
  reverse_callee_exact > joint_callee_caller_fingerprint > callee_size_structural >
  positional_ordering > projected_callee_intersection (anchors>=3) >
  projected_caller_intersection (anchors>=3) > projected_callee_intersection (anchors=2) >
  projected_caller_intersection (anchors=2) > ns_cluster_fingerprint >
  triple_fingerprint > single_callee_size > cascade2_from_* > (anything else)
"""
import json, re

CASCADE = "docs/k1_tsl_cascade_matches.json"

VIA_PRIORITY = {
    "exact_byte_match": 100,
    "known_function": 90,
    "callee_exact": 80,
    "reverse_callee_exact": 80,
    "joint_callee_caller_fingerprint": 70,
    "callee_size_structural": 65,
    "positional_ordering": 60,
    "ns_cluster_fingerprint": 30,
    "triple_fingerprint": 25,
    "single_callee_size": 20,
    "k1_common_subset_size": 20,
}

def via_score(m):
    via = m.get("via", "")
    if via.startswith("cascade2_from_"):
        return 5
    # projected: adjust by anchors
    if "projected" in via:
        anchors = m.get("anchors", 2)
        if "callee" in via:
            base = 55
        else:
            base = 50
        return base + min(anchors - 2, 5) * 2  # anchors=2 -> base, anchors=7+ -> base+10
    for key, score in VIA_PRIORITY.items():
        if key in via:
            return score
    return 10

cas = json.load(open(CASCADE, encoding="utf-8"))
matches = cas["matches"]

print("Starting cascade size:", len(matches))

# Pass 1: resolve TSL duplicates (keep highest via_score)
tsl_best: dict[str, dict] = {}
for m in matches:
    tsl = m["tsl_addr"]
    score = via_score(m)
    if tsl not in tsl_best or score > via_score(tsl_best[tsl]):
        tsl_best[tsl] = m

tsl_removed = len(matches) - len(tsl_best)
if tsl_removed:
    print(f"TSL dupes removed: {tsl_removed}")
    for m in matches:
        if tsl_best.get(m["tsl_addr"]) is not m and tsl_best.get(m["tsl_addr"]) != m:
            pass  # debug; we won't print all
    # Show removed entries
    kept_set = set(id(v) for v in tsl_best.values())
    for m in matches:
        if id(m) not in kept_set and m["tsl_addr"] in tsl_best and id(tsl_best[m["tsl_addr"]]) != id(m):
            print(f"  TSL REMOVED {m['tsl_addr']} K1={m['k1_addr']} via={m.get('via','')} name={m.get('name','')}")

deduped_tsl = list(tsl_best.values())

# Pass 2: resolve K1 duplicates (keep highest via_score)
k1_best: dict[str, dict] = {}
for m in deduped_tsl:
    k1 = m["k1_addr"]
    score = via_score(m)
    if k1 not in k1_best or score > via_score(k1_best[k1]):
        k1_best[k1] = m

k1_removed = len(deduped_tsl) - len(k1_best)
if k1_removed:
    print(f"K1 dupes removed: {k1_removed}")
    kept_set2 = set(id(v) for v in k1_best.values())
    for m in deduped_tsl:
        if id(m) not in kept_set2:
            print(f"  K1 REMOVED TSL={m['tsl_addr']} K1={m['k1_addr']} via={m.get('via','')} name={m.get('name','')}")

final = list(k1_best.values())
print("Final cascade size:", len(final))

cas["matches"] = final
with open(CASCADE, "w", encoding="utf-8") as fh:
    json.dump(cas, fh, indent=2)
print("Saved.")
