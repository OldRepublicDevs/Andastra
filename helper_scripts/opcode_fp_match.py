"""
Opcode fingerprint matching.

Matches K1 named functions to TSL FUN_xxx by comparing MD5 hashes of
their instruction mnemonic sequences (ignoring operands). Structurally
identical functions in near-identical binaries will have the same hash.

Input:
  docs/k1_opcode_fps.json    — {addr: {hash, len, name}}
  docs/tsl_opcode_fps.json   — {addr: {hash, len}} for FUN_xxx only
  docs/k1_tsl_cascade_matches.json
  docs/k1_call_graph.json
  docs/tsl_call_graph.json

Output:
  docs/tsl_opcode_fp_matches.json
"""
from __future__ import annotations
import json
from collections import defaultdict

K1_FPS   = "docs/k1_opcode_fps.json"
TSL_FPS  = "docs/tsl_opcode_fps.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
K1_CG    = "docs/k1_call_graph.json"
TSL_CG   = "docs/tsl_call_graph.json"
OUT      = "docs/tsl_opcode_fp_matches.json"


def main():
    k1_fps  = json.load(open(K1_FPS,  encoding="utf-8"))
    tsl_fps = json.load(open(TSL_FPS,  encoding="utf-8"))
    cascade = json.load(open(CASCADE,  encoding="utf-8"))

    matched_tsl = {int(m["tsl_addr"], 16) for m in cascade["matches"]}
    matched_k1  = {int(m["k1_addr"],  16) for m in cascade["matches"]}

    # Build hash → K1 unmatched named func
    k1_hash: dict = defaultdict(list)
    for addr_str, fp in k1_fps.items():
        addr = int(addr_str)
        if addr in matched_k1:
            continue
        k1_hash[fp["hash"]].append({"addr": addr, "name": fp["name"], "len": fp["len"]})

    unique_k1 = {h: v[0] for h, v in k1_hash.items() if len(v) == 1}
    print(f"Unique K1 opcode hashes (unmatched): {len(unique_k1)}")

    matches = []
    seen_k1 = set()
    for addr_str, fp in tsl_fps.items():
        ta = int(addr_str)
        if ta in matched_tsl:
            continue
        h = fp["hash"]
        if h not in unique_k1:
            continue
        k1f = unique_k1[h]
        ka = k1f["addr"]
        if ka in matched_k1 or ka in seen_k1:
            continue
        name = k1f["name"]
        ns, meth = (name.rsplit("::", 1) if "::" in name else ("", name))
        matches.append({
            "k1_addr": "0x{:08X}".format(ka),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": name,
            "class": ns,
            "method": meth,
            "via_k1": "opcode_fp:hash={}".format(h[:8]),
        })
        seen_k1.add(ka)

    print(f"Opcode fingerprint matches: {len(matches)}")
    if matches:
        for m in matches[:5]:
            print(f"  {m['name']}: K1={m['k1_addr']} TSL={m['tsl_addr']}")

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print(f"Wrote {len(matches)} matches to {OUT}")


if __name__ == "__main__":
    main()
