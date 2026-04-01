"""Merge tsl_triple_fp_matches.json into cascade and regenerate batch rename script."""
import json, re

CASCADE = "docs/k1_tsl_cascade_matches.json"
NEW     = "docs/tsl_triple_fp_matches.json"
RENAME  = "helper_scripts/ghidra_batch_rename_tsl.py"
TSL_CG  = "docs/tsl_call_graph.json"

cas  = json.load(open(CASCADE, encoding="utf-8"))
new_ = json.load(open(NEW,     encoding="utf-8"))

existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
added = 0
for m in new_:
    if m["tsl_addr"] not in existing_tsl:
        cas["matches"].append(m)
        existing_tsl.add(m["tsl_addr"])
        added += 1

print("Added {} new matches  (total now {})".format(added, len(cas["matches"])))
with open(CASCADE, "w", encoding="utf-8") as fh:
    json.dump(cas, fh, indent=2)

# Regenerate Ghidra batch rename script
tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

pending = [(m, parse_hex(m["tsl_addr"]))
           for m in cas["matches"]
           if tsl_name.get(parse_hex(m["tsl_addr"]), "").startswith("FUN_")]

print("Pending renames: {}".format(len(pending)))

lines = [
    "# Auto-generated Ghidra rename script for TSL",
    "# {} entries".format(len(pending)),
    "from ghidra.program.model.symbol import SourceType",
    "fm = currentProgram.getFunctionManager()",
    "sm = currentProgram.getSymbolTable()",
    "renamed = 0; errors = 0",
    "",
]

for m, tsl_a in pending:
    clean = re.sub(r"[^A-Za-z0-9_]", "_", m["method"])
    ns = m.get("class","")
    lines += [
        "try:",
        "    fn = fm.getFunctionAt(toAddr(0x{:X}))".format(tsl_a),
        "    if fn:",
        "        fn.setName('{}', SourceType.USER_DEFINED)".format(clean),
        "        nsobj = sm.getNamespace('{}', currentProgram.getGlobalNamespace())".format(ns),
        "        if nsobj is None:",
        "            nsobj = currentProgram.getSymbolTable().createNameSpace(",
        "                currentProgram.getGlobalNamespace(), '{}', SourceType.USER_DEFINED)".format(ns),
        "        fn.setParentNamespace(nsobj)",
        "        renamed += 1",
        "    else: errors += 1",
        "except Exception as e:",
        "    print('ERR 0x{:X}: '+str(e)); errors += 1".format(tsl_a),
        "",
    ]

lines += ["print('renamed={} errors={}'.format(renamed, errors))"]
with open(RENAME, "w", encoding="utf-8") as fh:
    fh.write("\n".join(lines))
print("Wrote {} to {}".format(len(pending), RENAME))
