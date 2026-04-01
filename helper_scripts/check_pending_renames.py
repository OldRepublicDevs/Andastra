#!/usr/bin/env python3
"""Cross-reference cascade matches against TSL call graph to find functions still needing rename."""
import json
import sys

CALL_GRAPH = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_pending_renames.json"

def parse_hex(s):
    if isinstance(s, str):
        return int(s, 16)
    return s

def main():
    cg = json.load(open(CALL_GRAPH, encoding="utf-8"))
    cas = json.load(open(CASCADE, encoding="utf-8"))

    addr_to_entry = {f["a"]: f for f in cg["functions"]}

    fun_xxx = []
    already_named = []
    missing = []

    for m in cas["matches"]:
        tsl_addr = parse_hex(m["tsl_addr"])
        entry = addr_to_entry.get(tsl_addr)
        if entry is None:
            missing.append(m)
        elif entry["n"].startswith("FUN_") or entry.get("auto", False):
            fun_xxx.append({
                "tsl_addr": m["tsl_addr"],
                "new_name": m["name"],
                "class": m.get("class", ""),
                "method": m.get("method", ""),
                "k1_addr": m["k1_addr"],
                "current_name": entry["n"],
            })
        else:
            already_named.append((m, entry["n"]))

    print(f"Still FUN_xxx / auto: {len(fun_xxx)}")
    print(f"Already named:        {len(already_named)}")
    print(f"Not in call graph:    {len(missing)}")
    print()
    print("First 10 needing rename:")
    for item in fun_xxx[:10]:
        print(f"  {item['tsl_addr']} ({item['current_name']}) -> {item['new_name']}")

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(fun_xxx, f, indent=2)
    print(f"\nWrote {len(fun_xxx)} entries to {OUT}")

if __name__ == "__main__":
    main()
