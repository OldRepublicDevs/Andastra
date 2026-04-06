"""Patch in positional Journal HandleInputEvent match that was blocked by ns_cluster entry."""
import json
CASCADE = "docs/k1_tsl_cascade_matches.json"
cas = json.load(open(CASCADE, encoding="utf-8"))
existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
existing_k1  = {m["k1_addr"]  for m in cas["matches"]}

patch = {
    "tsl_addr": "0x592200", "k1_addr": "0x6456e0",
    "name": "CSWGuiInGameJournal::HandleInputEvent",
    "class": "CSWGuiInGameJournal", "method": "HandleInputEvent",
    "via": "positional_ordering"
}

tsl_taken = patch["tsl_addr"] in existing_tsl
k1_taken  = patch["k1_addr"]  in existing_k1
if not tsl_taken and not k1_taken:
    cas["matches"].append(patch)
    print("Patched in HandleInputEvent  Total:", len(cas["matches"]))
    with open(CASCADE, "w", encoding="utf-8") as fh:
        json.dump(cas, fh, indent=2)
else:
    print("SKIP: tsl_taken={} k1_taken={}".format(tsl_taken, k1_taken))
    # Show what's claiming those addresses
    for m in cas["matches"]:
        if m["tsl_addr"] == patch["tsl_addr"] or m["k1_addr"] == patch["k1_addr"]:
            print("  CONFLICT:", m["tsl_addr"], "->", m["k1_addr"], m.get("name",""), "via="+m.get("via",""))
