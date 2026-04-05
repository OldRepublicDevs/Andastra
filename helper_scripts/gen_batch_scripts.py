"""Generate compact batch rename scripts for Ghidra execute_script."""

from __future__ import annotations

import json
import re
from typing import Any

CASCADE = "docs/k1_tsl_cascade_matches.json"
TSL_CG = "docs/tsl_call_graph.json"

cas: dict[str, Any] = json.load(open(CASCADE, encoding="utf-8"))
tsl_info: dict[int, tuple[str, bool]] = {f["a"]: (f["n"], f.get("auto", True)) for f in json.load(open(TSL_CG, encoding="utf-8"))["functions"]}


def parse_hex(s: str | int) -> int:
    return int(s, 16) if isinstance(s, str) else s


pending = [(m, parse_hex(m["tsl_addr"])) for m in cas["matches"] if tsl_info.get(parse_hex(m["tsl_addr"]), ("", True))[0].startswith("FUN_")]
print("Pending renames:", len(pending))

BATCH = 1000
for i in range(0, len(pending), BATCH):
    batch = pending[i : i + BATCH]
    bn = i // BATCH + 1
    lines = [
        "from ghidra.program.model.symbol import SourceType",
        "fm = currentProgram.getFunctionManager()",
        "sm = currentProgram.getSymbolTable()",
        "renamed = 0; errors = 0",
        "tid = currentProgram.startTransaction('batch_rename_{bn}')".format(bn=bn),
    ]
    for m, ta in batch:
        clean = re.sub(r"[^A-Za-z0-9_]", "_", m["method"])
        ns = m.get("class", "")
        # One try/except block per function rename
        # Skip namespace assignment when ns is empty (avoids InvalidInputException)
        ns_lines = []
        if ns:
            ns_lines = [
                "    nsobj = sm.getNamespace({!r}, currentProgram.getGlobalNamespace())".format(ns),
                "    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), {!r}, SourceType.USER_DEFINED)".format(ns),
                "    fn.setParentNamespace(nsobj)",
            ]
        lines += (
            [
                "try:",
                "  fn = fm.getFunctionAt(toAddr(0x{:X}))".format(ta),
                "  if fn:",
                "    fn.setName({!r}, SourceType.USER_DEFINED)".format(clean),
            ]
            + ns_lines
            + [
                "    renamed += 1",
                "  else: errors += 1",
                "except Exception as e: print('ERR 0x{:X}: '+str(e)); errors += 1".format(ta),
            ]
        )
    lines.append("currentProgram.endTransaction(tid, True)")
    lines.append("print('Batch {bn} done: renamed='+str(renamed)+' errors='+str(errors))".format(bn=bn))
    script = "\n".join(lines)
    fname = "helper_scripts/ghidra_rename_tsl_batch{}.py".format(bn)
    with open(fname, "w", encoding="utf-8") as fh:
        fh.write(script)
    print("Wrote batch {} ({} renames, {:,} bytes)".format(bn, len(batch), len(script.encode())))
