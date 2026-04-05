"""Generate compact batch rename scripts for Ghidra execute_script."""
import json, re

CASCADE = "docs/k1_tsl_cascade_matches.json"
TSL_CG  = "docs/tsl_call_graph.json"

cas  = json.load(open(CASCADE))
tsl_name = {f["a"]: f["n"] for f in json.load(open(TSL_CG))["functions"]}

def parse_hex(s): return int(s,16) if isinstance(s,str) else s

pending = [(m, parse_hex(m["tsl_addr"])) for m in cas["matches"]
           if tsl_name.get(parse_hex(m["tsl_addr"]),"").startswith("FUN_")]
print("Pending renames:", len(pending))

BATCH = 250
for i in range(0, len(pending), BATCH):
    batch = pending[i:i+BATCH]
    lines = [
        "from ghidra.program.model.symbol import SourceType",
        "fm = currentProgram.getFunctionManager()",
        "sm = currentProgram.getSymbolTable()",
        "renamed = 0; errors = 0",
    ]
    for m, ta in batch:
        clean = re.sub(r"[^A-Za-z0-9_]", "_", m["method"])
        ns    = m.get("class", "")
        # One try/except block per function rename
        lines += [
            "try:",
            "  fn = fm.getFunctionAt(toAddr(0x{:X}))".format(ta),
            "  if fn:",
            "    fn.setName({!r}, SourceType.USER_DEFINED)".format(clean),
            "    nsobj = sm.getNamespace({!r}, currentProgram.getGlobalNamespace())".format(ns),
            "    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), {!r}, SourceType.USER_DEFINED)".format(ns),
            "    fn.setParentNamespace(nsobj); renamed += 1",
            "  else: errors += 1",
            "except Exception as e: print('ERR 0x{:X}: '+str(e)); errors += 1".format(ta),
        ]
    bn = i // BATCH + 1
    lines.append("print('Batch {} done: renamed='+str(renamed)+' errors='+str(errors))".format(bn))
    script = "\n".join(lines)
    fname  = "helper_scripts/ghidra_rename_tsl_batch{}.py".format(bn)
    with open(fname, "w", encoding="utf-8") as fh:
        fh.write(script)
    print("Wrote batch {} ({} renames, {:,} bytes)".format(bn, len(batch), len(script.encode())))
