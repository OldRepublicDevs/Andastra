"""Merge tsl_joint_fingerprint_matches.json into the cascade and regenerate ghidra_batch_rename_tsl.py"""
import json, re

CASCADE = "docs/k1_tsl_cascade_matches.json"
NEW     = "docs/tsl_joint_fingerprint_matches.json"
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

print(f"Added {added} new matches  (total now {len(cas['matches'])})")

with open(CASCADE, "w", encoding="utf-8") as fh:
    json.dump(cas, fh, indent=2)

# Re-generate Ghidra batch rename script
tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
tsl_addr_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}
tsl_fun_addr_to_func = {f["a"]: f for f in tsl_cg["functions"]}

def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

pending = []
for m in cas["matches"]:
    tsl_a = parse_hex(m["tsl_addr"])
    # Only rename if the function is still FUN_xxx in the call graph
    if tsl_a in tsl_addr_name and tsl_addr_name[tsl_a].startswith("FUN_"):
        pending.append(m)

print(f"Pending renames in Ghidra: {len(pending)}")

# Build Jython script
lines = [
    "# Auto-generated Ghidra rename script for TSL FUN_xxx -> named functions",
    "# Run via AgentDecompile execute-script on /TSL_GOG_swkotor2",
    "from ghidra.program.model.symbol import SourceType",
    "from ghidra.app.cmd.label import RenameLabelCmd",
    "fm = currentProgram.getFunctionManager()",
    "sm = currentProgram.getSymbolTable()",
    "renamed = 0",
    "errors  = 0",
    "",
]

for m in pending:
    tsl_a = parse_hex(m["tsl_addr"])
    clean_name = re.sub(r"[^A-Za-z0-9_]", "_", m["method"])
    ns = m.get("class", "")
    lines += [
        "try:",
        "    addr = toAddr(0x{:X})".format(tsl_a),
        "    fn = fm.getFunctionAt(addr)",
        "    if fn is not None:",
        "        fn.setName('{}', SourceType.USER_DEFINED)".format(clean_name),
        "        if fn.getParentNamespace().getName() != '{}':".format(ns),
        "            ns_obj = sm.getNamespace('{}', currentProgram.getGlobalNamespace())".format(ns),
        "            if ns_obj is None:",
        "                ns_obj = currentProgram.getSymbolTable().createNameSpace(",
        "                    currentProgram.getGlobalNamespace(), '{}', SourceType.USER_DEFINED)".format(ns),
        "            fn.setParentNamespace(ns_obj)",
        "        renamed += 1",
        "    else:",
        "        print('WARN: no func at 0x{:X}')".format(tsl_a),
        "        errors += 1",
        "except Exception as e:",
        "    print('ERROR 0x{:X}: ' + str(e))".format(tsl_a),
        "    errors += 1",
        "",
    ]

lines += [
    "print('Done: renamed={} errors={}'.format(renamed, errors))",
]

script = "\n".join(lines)
with open(RENAME, "w", encoding="utf-8") as fh:
    fh.write(script)
print(f"Wrote {len(pending)}-rename script to {RENAME}")
