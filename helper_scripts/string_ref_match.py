"""
String reference fingerprint matching.

Functions that reference the same set of string constants are very likely
the same function — debug strings, field names, error messages, etc. are
highly discriminating identifiers.

Input:
  docs/k1_string_refs.json   — {func_strings: {addr_int: [str, ...]}}
  docs/tsl_string_refs.json  — same format
  docs/k1_call_graph.json    — for K1 addr -> name mapping
  docs/tsl_call_graph.json   — for TSL addr -> name (FUN_xxx = unmatched)
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_string_ref_matches.json  — [{k1_addr, tsl_addr, name, class, method, via, strings}]
"""
from __future__ import annotations

import json
from collections import defaultdict

K1_STRINGS = "docs/k1_string_refs.json"
TSL_STRINGS = "docs/tsl_string_refs.json"
K1_CG       = "docs/k1_call_graph.json"
TSL_CG      = "docs/tsl_call_graph.json"
CASCADE     = "docs/k1_tsl_cascade_matches.json"
OUT         = "docs/tsl_string_ref_matches.json"

MIN_STRINGS = 2      # minimum number of matching strings to count as a fingerprint
MIN_RATIO   = 0.75   # fraction of K1 strings that must appear in TSL match

# Very generic strings present in many functions — exclude as discriminators
GENERIC_STRINGS = {
    "", " ", "  ", "\n", "\r\n",
    "%s", "%d", "%f", "%i", "%u", "%x",
    "TRUE", "FALSE", "true", "false",
    "0", "1",
}


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else int(s)


def fingerprint_strings(strs):
    """Return a frozenset of non-generic strings."""
    return frozenset(s for s in strs if s not in GENERIC_STRINGS and len(s) >= 2)


def main():
    k1_sr  = json.load(open(K1_STRINGS, encoding="utf-8"))
    tsl_sr = json.load(open(TSL_STRINGS, encoding="utf-8"))
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    # K1 addr -> name/class/method
    k1_funcs = {f["a"]: f for f in k1_cg["functions"]}
    tsl_funcs = {f["a"]: f for f in tsl_cg["functions"]}

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    # Build K1 string fingerprint -> [addr]
    k1_fp = {}   # frozenset(strings) -> [k1_addr]
    k1_str_data = {}
    for addr_str, strs in k1_sr["func_strings"].items():
        addr = int(addr_str)
        fp = fingerprint_strings(strs)
        if len(fp) >= MIN_STRINGS:
            k1_fp.setdefault(fp, []).append(addr)
            k1_str_data[addr] = fp

    # Only keep unique K1 fingerprints
    unique_k1 = {fp: addrs[0] for fp, addrs in k1_fp.items() if len(addrs) == 1}
    print("K1 unique string fingerprints (>={} strings): {}".format(MIN_STRINGS, len(unique_k1)))

    # Build TSL string fingerprint -> [addr]  (only unmatched functions)
    tsl_str_data = {}
    for addr_str, strs in tsl_sr["func_strings"].items():
        addr = int(addr_str)
        if addr in matched_tsl:
            continue
        fp = fingerprint_strings(strs)
        if len(fp) >= MIN_STRINGS:
            tsl_str_data[addr] = fp

    print("TSL unmatched functions with string fingerprints: {}".format(len(tsl_str_data)))

    # Match: TSL fp must be a superset of a unique K1 fp (or the same)
    matches = []
    used_tsl = set()
    used_k1  = set()

    for tsl_addr, tsl_fp in tsl_str_data.items():
        best_k1_addr = None
        best_overlap = 0
        best_k1_fp = None

        # Direct lookup first
        if tsl_fp in unique_k1:
            k1_addr = unique_k1[tsl_fp]
            if k1_addr not in matched_k1 and k1_addr not in used_k1:
                best_k1_addr = k1_addr
                best_overlap = len(tsl_fp)
                best_k1_fp = tsl_fp
        else:
            # Partial match: find K1 fp that is a subset of TSL fp with high coverage
            for fp, k1_addr in unique_k1.items():
                if k1_addr in matched_k1 or k1_addr in used_k1:
                    continue
                overlap = len(fp & tsl_fp)
                if overlap >= MIN_STRINGS and overlap / len(fp) >= MIN_RATIO:
                    if overlap > best_overlap:
                        best_overlap = overlap
                        best_k1_addr = k1_addr
                        best_k1_fp = fp

        if best_k1_addr is None:
            continue

        # Verify uniqueness of this K1-addr match for this TSL query
        # (only one TSL function should match this K1 fingerprint)
        competitor_count = sum(
            1 for ta2, tf2 in tsl_str_data.items()
            if ta2 != tsl_addr and len(best_k1_fp & tf2) >= MIN_STRINGS
            and len(best_k1_fp & tf2) / len(best_k1_fp) >= MIN_RATIO
        )
        if competitor_count > 0:
            continue  # ambiguous — multiple TSL functions match

        k1_fn = k1_funcs.get(best_k1_addr, {})
        ns    = k1_fn.get("ns", "")
        name  = k1_fn.get("n", "FUN_{:08X}".format(best_k1_addr))
        method = name

        # Build cascade-compatible hex addresses
        k1_hex  = "0x{:08X}".format(best_k1_addr)
        tsl_hex = "0x{:08X}".format(tsl_addr)

        matches.append({
            "k1_addr":  k1_hex,
            "tsl_addr": tsl_hex,
            "name":     name,
            "class":    ns,
            "method":   method,
            "via":      "string_ref_fingerprint[{}]".format(best_overlap),
            "strings":  sorted(best_k1_fp),
        })
        used_tsl.add(tsl_addr)
        used_k1.add(best_k1_addr)

    print("Matches: {}".format(len(matches)))
    for m in matches[:15]:
        print("  {} -> {}  ({}) | {}".format(
            m["tsl_addr"], m["k1_addr"], m["name"],
            [s[:30] for s in m["strings"][:3]]))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
