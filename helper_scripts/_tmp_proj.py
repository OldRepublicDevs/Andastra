import json

m = json.load(open("docs/tsl_projected_intersection_matches.json"))
# Check for duplicates and quality
tsl_seen = set()
dups = 0
for x in m:
    if x["tsl_addr"] in tsl_seen:
        dups += 1
    tsl_seen.add(x["tsl_addr"])

by_via: dict[str, list[dict]] = {}
for x in m:
    by_via.setdefault(x["via"], []).append(x)

print("Total:", len(m), "Duplicates:", dups)
for v, lst in by_via.items():
    print("  {}: {}".format(v, len(lst)))

print("\nTop by anchor count:")
for x in sorted(m, key=lambda x: -x["n_anchors"])[:10]:
    print("  {} -> {:12s}  a={}  {}".format(x["tsl_addr"], x["k1_addr"], x["n_anchors"], x["name"]))

print("\nSuspicious (anchors=2):")
for x in m:
    if x["n_anchors"] == 2:
        print("  {} -> {:12s}  {}".format(x["tsl_addr"], x["k1_addr"], x["name"]))
