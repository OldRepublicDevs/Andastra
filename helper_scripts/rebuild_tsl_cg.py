"""
Rebuild TSL call graph with cascade-derived names substituted for FUN_xxx entries.

Loads tsl_call_graph.json + cascade, writes tsl_call_graph.json with names updated.
Also updates ns field from cascade class.

This lets fingerprint algorithms (caller_name, joint_fingerprint, etc.) use
cascade-matched function names without needing a fresh Ghidra CG export.
"""
from __future__ import annotations
import json
import re

CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"

cg = json.load(open(CG, encoding="utf-8"))
cas = json.load(open(CASCADE, encoding="utf-8"))

# Build cascade lookup: tsl_addr -> (name, class)
cascade_names: dict[int, tuple[str, str]] = {}
for m in cas["matches"]:
    ta = int(m["tsl_addr"], 16)
    method = m.get("method", "")
    cls = m.get("class", "")
    if method and not method.startswith("FUN_"):
        clean = re.sub(r"[^A-Za-z0-9_]", "_", method)
        # Full name = class::method if class present
        if cls:
            full_name = cls + "::" + clean
        else:
            full_name = clean
        cascade_names[ta] = (full_name, cls, clean)

before_fun = sum(1 for f in cg["functions"] if f["n"].startswith("FUN_"))
updated = 0

for fn in cg["functions"]:
    addr = fn["a"]
    if fn["n"].startswith("FUN_") and addr in cascade_names:
        full_name, cls, method = cascade_names[addr]
        fn["n"] = full_name
        if cls:
            fn["ns"] = cls
        fn["auto"] = False
        updated += 1

after_fun = sum(1 for f in cg["functions"] if f["n"].startswith("FUN_"))

print("Before FUN_xxx: {}".format(before_fun))
print("Updated:        {}".format(updated))
print("After FUN_xxx:  {}".format(after_fun))

with open(CG, "w", encoding="utf-8") as f:
    json.dump(cg, f)

print("Saved to", CG)
