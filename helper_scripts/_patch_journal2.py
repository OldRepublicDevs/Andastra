"""Remove the low-confidence projected match (TSL 0x592280 -> K1 0x6456e0, 2 anchors)
and add back the 5-pair positional match (TSL 0x592200 -> K1 0x6456e0)."""
import json
CASCADE = "docs/k1_tsl_cascade_matches.json"
cas = json.load(open(CASCADE, encoding="utf-8"))

# Remove the 2-anchor projected match
before = len(cas["matches"])
cas["matches"] = [m for m in cas["matches"]
                  if not (m["tsl_addr"] == "0x592280" and m["k1_addr"] == "0x6456e0")]
removed = before - len(cas["matches"])
print("Removed projected match (tsl=0x592280 k1=0x6456e0):", removed)

# Add the positional match
patch = {
    "tsl_addr": "0x592200", "k1_addr": "0x6456e0",
    "name": "CSWGuiInGameJournal::HandleInputEvent",
    "class": "CSWGuiInGameJournal", "method": "HandleInputEvent",
    "via": "positional_ordering"
}
existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
existing_k1  = {m["k1_addr"]  for m in cas["matches"]}
if patch["tsl_addr"] not in existing_tsl and patch["k1_addr"] not in existing_k1:
    cas["matches"].append(patch)
    print("Added positional HandleInputEvent  Total:", len(cas["matches"]))
else:
    print("ERROR: still blocked after removal")

with open(CASCADE, "w", encoding="utf-8") as fh:
    json.dump(cas, fh, indent=2)
