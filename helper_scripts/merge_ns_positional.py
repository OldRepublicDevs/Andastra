"""Merge ns_cluster and positional matches into cascade; regenerate rename script."""
import json, re

CASCADE = "docs/k1_tsl_cascade_matches.json"
FILES   = ["docs/tsl_ns_cluster_matches.json", "docs/tsl_positional_matches.json"]
RENAME  = "helper_scripts/ghidra_batch_rename_tsl.py"
TSL_CG  = "docs/tsl_call_graph.json"

cas = json.load(open(CASCADE, encoding="utf-8"))
existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
existing_k1  = {m["k1_addr"]  for m in cas["matches"]}
added = skipped = 0
for path in FILES:
    for m in json.load(open(path, encoding="utf-8")):
        if m["tsl_addr"] in existing_tsl or m["k1_addr"] in existing_k1:
            skipped += 1
            continue
        cas["matches"].append(m)
        existing_tsl.add(m["tsl_addr"])
        existing_k1.add(m["k1_addr"])
        added += 1
print("Added {}  Skipped(dup): {}  (total now {})".format(added, skipped, len(cas["matches"])))
with open(CASCADE, "w", encoding="utf-8") as fh:
    json.dump(cas, fh, indent=2)

tsl_name = {f["a"]: f["n"] for f in json.load(open(TSL_CG, encoding="utf-8"))["functions"]}
def parse_hex(s): return int(s,16) if isinstance(s,str) else s

pending = [(m, parse_hex(m["tsl_addr"]))
           for m in cas["matches"]
           if tsl_name.get(parse_hex(m["tsl_addr"]),"").startswith("FUN_")]
print("Pending renames: {}".format(len(pending)))

lines = [
    "# Ghidra TSL rename script  ({} entries)".format(len(pending)),
    "from ghidra.program.model.symbol import SourceType",
    "fm = currentProgram.getFunctionManager()",
    "sm = currentProgram.getSymbolTable()",
    "renamed = 0; errors = 0", "",
]
for m, ta in pending:
    clean = re.sub(r"[^A-Za-z0-9_]","_", m["method"])
    ns    = m.get("class","")
    lines += [
        "try:",
        "    fn = fm.getFunctionAt(toAddr(0x{:X}))".format(ta),
        "    if fn:",
        "        fn.setName('{}', SourceType.USER_DEFINED)".format(clean),
        "        nsobj = sm.getNamespace('{}', currentProgram.getGlobalNamespace())".format(ns),
        "        if nsobj is None:",
        "            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),",
        "                '{}', SourceType.USER_DEFINED)".format(ns),
        "        fn.setParentNamespace(nsobj); renamed += 1",
        "    else: errors += 1",
        "except Exception as e:",
        "    print('ERR 0x{:X}: '+str(e)); errors += 1".format(ta), "",
    ]
lines += ["print('renamed={} errors={}'.format(renamed, errors))"]
with open(RENAME, "w", encoding="utf-8") as fh:
    fh.write("\n".join(lines))
print("Wrote {} to {}".format(len(pending), RENAME))
