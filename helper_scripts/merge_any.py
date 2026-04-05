"""Generic merge: reads a matches JSON file and merges into cascade."""

from __future__ import annotations

import json
import re
import sys
from typing import Any, cast

CASCADE = "docs/k1_tsl_cascade_matches.json"
TSL_CG = "docs/tsl_call_graph.json"
RENAME = "helper_scripts/ghidra_batch_rename_tsl.py"
SRC = sys.argv[1] if len(sys.argv) > 1 else "docs/tsl_collision_resolved_matches.json"

cas = json.load(open(CASCADE, encoding="utf-8"))
new_ = json.load(open(SRC, encoding="utf-8"))

existing_tsl: set[str] = {m["tsl_addr"] for m in cas["matches"]}
existing_k1: set[str] = {m["k1_addr"] for m in cas["matches"]}
added = skipped = 0
for m in new_:
    if m["tsl_addr"] in existing_tsl or m["k1_addr"] in existing_k1:
        skipped += 1
        continue
    cast("list[Any]", cas["matches"]).append(m)
    existing_tsl.add(m["tsl_addr"])
    existing_k1.add(m["k1_addr"])
    added += 1

print("Added {}  Skipped(dup): {}  Total now: {}".format(added, skipped, len(cas["matches"])))
with open(CASCADE, "w", encoding="utf-8") as fh:
    json.dump(cas, fh, indent=2)


def parse_hex(s: str | int) -> int:
    return int(s, 16) if isinstance(s, str) else s


tsl_name: dict[int, str] = {f["a"]: f["n"] for f in json.load(open(TSL_CG, encoding="utf-8"))["functions"]}
pending: list[tuple[Any, int]] = [(m, parse_hex(m["tsl_addr"])) for m in cas["matches"] if tsl_name.get(parse_hex(m["tsl_addr"]), "").startswith("FUN_")]
print("Pending renames: {}".format(len(pending)))

lines: list[str] = [
    "# Ghidra TSL rename script  ({} entries)".format(len(pending)),
    "from ghidra.program.model.symbol import SourceType",
    "fm = currentProgram.getFunctionManager()",
    "sm = currentProgram.getSymbolTable()",
    "renamed = 0; errors = 0",
    "",
]
for m, ta in pending:
    clean: str = re.sub(r"[^A-Za-z0-9_]", "_", m["method"])
    ns: str = cast("dict[str, Any]", m).get("class", "")
    if ns and ns.strip():
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
            "    print('ERR 0x{:X}: '+str(e)); errors += 1".format(ta),
            "",
        ]
    else:
        lines += [
            "try:",
            "    fn = fm.getFunctionAt(toAddr(0x{:X}))".format(ta),
            "    if fn:",
            "        fn.setName('{}', SourceType.USER_DEFINED)".format(clean),
            "        renamed += 1",
            "    else: errors += 1",
            "except Exception as e:",
            "    print('ERR 0x{:X}: '+str(e)); errors += 1".format(ta),
            "",
        ]
lines += ["print('renamed={} errors={}'.format(renamed, errors))"]
with open(RENAME, "w", encoding="utf-8") as fh:
    fh.write("\n".join(lines))
print("Wrote {} to {}".format(len(pending), RENAME))
