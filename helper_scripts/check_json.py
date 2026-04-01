from __future__ import annotations

import json
import pprint

d = json.load(open("docs/kotor_function_signatures.json"))

print("=== META ===")
pprint.pprint(d["meta"])

print("\n=== SAMPLE: CSWCMapPin deleting thunk ===")
for f in d["functions"]:
    if "CSWCMapPin" in f["name"] and "scalar" in f["name"]:
        pprint.pprint(f)
        break

print("\n=== SKIP ENTRIES (no sig) ===")
for f in d["functions"]:
    if f["sig"] is None:
        k1 = f["addresses"].get("k1", "None")
        tsl = f["addresses"].get("tsl", "None")
        print(f"  {f['name']:<50}  k1={k1}  tsl={tsl}")
