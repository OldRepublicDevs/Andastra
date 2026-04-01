#!/usr/bin/env python3
"""
Generate a Ghidra Jython batch-rename script for all TSL FUN_xxx functions
that have matches in the cascade JSON.

Usage:
    python helper_scripts/gen_batch_rename_script.py [--file MATCH_JSON] [--out SCRIPT_OUT]

The generated script can be passed directly to mcp_agdec-mcp_execute-script
with program_path=/TSL_GOG_swkotor2 (or whatever TSL program is open in Ghidra).
"""
import json
import argparse
from pathlib import Path

CASCADE  = "docs/k1_tsl_cascade_matches.json"
TSL_CG   = "docs/tsl_call_graph.json"
OUT_FILE = "helper_scripts/ghidra_batch_rename_tsl.py"

PREAMBLE = """\
# AUTO-GENERATED Ghidra batch-rename script for TSL FUN_xxx functions.
# Run via mcp_agdec-mcp_execute-script with program_path=/TSL_GOG_swkotor2
from ghidra.app.util import NamespaceUtils
from ghidra.program.model.symbol import SourceType

RENAMES = [
"""

POSTAMBLE = """\
]

skipped = 0
renamed = 0
for entry in RENAMES:
    addr_int = entry[0]
    ns_name  = entry[1]
    fn_name  = entry[2]
    addr = toAddr(addr_int)
    fn = getFunctionAt(addr)
    if fn is None:
        skipped += 1
        continue
    cur = fn.getName()
    if not cur.startswith("FUN_"):
        skipped += 1
        continue
    try:
        if ns_name:
            ns = NamespaceUtils.createNamespaceHierarchy(
                ns_name, None, currentProgram, SourceType.USER_DEFINED)
            fn.setParentNamespace(ns)
        fn.setName(fn_name, SourceType.USER_DEFINED)
        renamed += 1
        print("OK  %08x  %s::%s" % (addr_int, ns_name, fn_name))
    except Exception as ex:
        skipped += 1
        print("ERR %08x  %s::%s  (%s)" % (addr_int, ns_name, fn_name, ex))

print("Done: renamed=%d  skipped=%d" % (renamed, skipped))
"""


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--cascade", default=CASCADE)
    parser.add_argument("--tsl-cg",  default=TSL_CG)
    parser.add_argument("--out",     default=OUT_FILE)
    args = parser.parse_args()

    cas    = json.load(open(args.cascade, encoding="utf-8"))
    tsl_cg = json.load(open(args.tsl_cg,  encoding="utf-8"))

    tsl_addr_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    entries = []
    for m in cas["matches"]:
        tsl_a = int(m["tsl_addr"], 16)
        cur = tsl_addr_name.get(tsl_a, "")
        if cur.startswith("FUN_") or not cur:
            ns  = m.get("class", "")
            fn  = m.get("method", "") or m.get("name", "")
            if fn:
                entries.append((tsl_a, ns, fn))

    entries.sort(key=lambda e: e[0])
    print(f"Entries to rename: {len(entries)}")

    lines = [PREAMBLE]
    for a, ns, fn in entries:
        # Escape any backslashes/quotes in names (shouldn't happen but be safe)
        ns_safe = ns.replace("\\", "\\\\").replace('"', '\\"')
        fn_safe = fn.replace("\\", "\\\\").replace('"', '\\"')
        lines.append(f'    ({a:#010x}, "{ns_safe}", "{fn_safe}"),\n')
    lines.append(POSTAMBLE)

    Path(args.out).write_text("".join(lines), encoding="utf-8")
    print(f"Wrote Ghidra script to {args.out}")
    print("To apply: use mcp_agdec-mcp_execute-script with program_path=/TSL_GOG_swkotor2")


if __name__ == "__main__":
    main()
