#!/usr/bin/env python3
"""
KotOR Universal Function Signature Generator (v2)

Consumes function byte dumps extracted from Ghidra via agdec-mcp execute-script.
Matches K1 and TSL functions by namespace::name, generates wildcard byte-pattern
signatures that work against EITHER binary without game-version conditionals.

Input:  docs/k1_function_bytes.json   (from agdec K1_GOG_full)
        docs/tsl_function_bytes.json  (from agdec TSL_GOG_swkotor2)
Output: docs/kotor_function_signatures.json
"""
import argparse
import json
import os
import sys
from datetime import datetime, timezone
from typing import Dict, List, Optional, Tuple


# ---------------------------------------------------------------------------
# Signature building
# ---------------------------------------------------------------------------

def build_wildcard_sig(k1_bytes: List[int], tsl_bytes: List[int]) -> str:
    """Build a wildcard byte pattern where differing bytes become '??'."""
    length = min(len(k1_bytes), len(tsl_bytes))
    parts = []
    for i in range(length):
        if k1_bytes[i] == tsl_bytes[i]:
            parts.append("{:02X}".format(k1_bytes[i]))
        else:
            parts.append("??")
    # Strip trailing wildcards
    while parts and parts[-1] == "??":
        parts.pop()
    return " ".join(parts)


def build_exact_sig(raw_bytes: List[int]) -> str:
    """Build an exact byte pattern (no wildcards)."""
    parts = ["{:02X}".format(b) for b in raw_bytes]
    # Strip trailing 00s (padding)
    while parts and parts[-1] == "00":
        parts.pop()
    return " ".join(parts)


def parse_hex_bytes(hex_str: str) -> List[int]:
    """Parse '8A 54 24 04 ...' into list of ints."""
    if not hex_str:
        return []
    return [int(b, 16) for b in hex_str.split()]


def count_static_bytes(sig: str) -> int:
    """Count non-wildcard bytes in a sig pattern."""
    return sum(1 for part in sig.split() if part != "??")


def sig_scan(sig_parts: List[str], data_funcs: List[dict]) -> int:
    """Count how many functions in the dump match the sig pattern at their entry point."""
    matches = 0
    sig_len = len(sig_parts)
    for f in data_funcs:
        f_bytes = parse_hex_bytes(f["b"])
        if len(f_bytes) < sig_len:
            continue
        match = True
        for i, part in enumerate(sig_parts):
            if part == "??":
                continue
            if f_bytes[i] != int(part, 16):
                match = False
                break
        if match:
            matches += 1
    return matches


# ---------------------------------------------------------------------------
# Main logic
# ---------------------------------------------------------------------------

def main():
    repo_root = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

    parser = argparse.ArgumentParser(description="KotOR universal function sig generator v2 (agdec-based)")
    parser.add_argument("--k1",      default=os.path.join(repo_root, "docs", "k1_function_bytes.json"))
    parser.add_argument("--tsl",     default=os.path.join(repo_root, "docs", "tsl_function_bytes.json"))
    parser.add_argument("--cascade", default=os.path.join(repo_root, "docs", "k1_tsl_cascade_matches.json"),
                        help="Cascade match file to supplement name-based matching")
    parser.add_argument("--out", default=os.path.join(repo_root, "docs", "kotor_function_signatures.json"))
    parser.add_argument("--min-static", type=int, default=6,  help="min non-wildcard bytes for valid sig")
    parser.add_argument("--sig-len",    type=int, default=64, help="max bytes to use from each function")
    parser.add_argument("--check-uniqueness", action="store_true", help="verify each sig is unique (slow)")
    parser.add_argument("--verbose",    action="store_true")
    args = parser.parse_args()

    for path in [args.k1, args.tsl]:
        if not os.path.exists(path):
            print("ERROR: Not found: {}".format(path), file=sys.stderr)
            sys.exit(1)

    print("Loading K1 function bytes: {}".format(args.k1))
    k1_data = json.load(open(args.k1, encoding="utf-8-sig"))
    print("  {} functions from {}".format(k1_data["count"], k1_data["program"]))

    print("Loading TSL function bytes: {}".format(args.tsl))
    tsl_data = json.load(open(args.tsl, encoding="utf-8-sig"))
    print("  {} functions from {}".format(tsl_data["count"], tsl_data["program"]))

    # Build lookup by qualified name (namespace::name)
    def build_lookup(funcs):
        by_qname = {}
        for f in funcs:
            if f["ns"]:
                qname = f["ns"] + "::" + f["n"]
            else:
                qname = f["n"]
            # Handle duplicates: keep the one with more bytes
            if qname not in by_qname or len(f["b"]) > len(by_qname[qname]["b"]):
                by_qname[qname] = f
        return by_qname

    k1_lookup = build_lookup(k1_data["functions"])
    tsl_lookup = build_lookup(tsl_data["functions"])

    # Also build address-based lookups for cascade supplementation
    k1_by_addr  = {f["a"].upper(): f for f in k1_data["functions"]}
    tsl_by_addr = {f["a"].upper(): f for f in tsl_data["functions"]}

    # Load cascade matches and add them to the lookup
    cascade_pairs = []
    if args.cascade and os.path.exists(args.cascade):
        print("Loading cascade matches: {}".format(args.cascade))
        cascade_data = json.load(open(args.cascade, encoding="utf-8"))
        cascade_pairs = cascade_data.get("matches", [])
        print("  {} cascade pairs".format(len(cascade_pairs)))
        # For each cascade pair: if the names are not already in both lookups, add them
        for cp in cascade_pairs:
            k1_addr = cp["k1_addr"].upper()
            tsl_addr = cp["tsl_addr"].upper()
            name  = cp.get("name", "")
            cls   = cp.get("class", "")
            meth  = cp.get("method", name)
            if not name or name.startswith(("FUN_", "LAB_", "DAT_")):
                continue
            # If this qname is already matched by name, skip
            if name in k1_lookup and name in tsl_lookup:
                continue
            # Add to lookups using cascade address data
            k1_f  = k1_by_addr.get(k1_addr)
            tsl_f = tsl_by_addr.get(tsl_addr)
            if k1_f is None or tsl_f is None:
                continue
            # Inject into lookups with the K1 name
            if name not in k1_lookup:
                k1_lookup[name] = k1_f
            if name not in tsl_lookup:
                # Create a virtual TSL entry using tsl address data
                tsl_lookup[name] = dict(tsl_f)  # shallow copy
                tsl_lookup[name]["n"] = meth
                tsl_lookup[name]["ns"] = cls

    # Find all unique qualified names
    all_qnames = sorted(set(k1_lookup.keys()) | set(tsl_lookup.keys()))

    print("\nK1 unique qnames: {}".format(len(k1_lookup)))
    print("TSL unique qnames: {}".format(len(tsl_lookup)))

    # Find matches (present in both)
    both = sorted(set(k1_lookup.keys()) & set(tsl_lookup.keys()))
    k1_only = sorted(set(k1_lookup.keys()) - set(tsl_lookup.keys()))
    tsl_only = sorted(set(tsl_lookup.keys()) - set(k1_lookup.keys()))
    print("Matched (both games): {}".format(len(both)))
    print("K1-only: {}".format(len(k1_only)))
    print("TSL-only: {}".format(len(tsl_only)))

    # Generate signatures
    entries = []
    sig_ok = 0
    wildcard_ok = 0

    print("\n--- Generating signatures ---")

    # 1. Joint K1+TSL entries (wildcard signatures)
    for qname in both:
        k1f = k1_lookup[qname]
        tslf = tsl_lookup[qname]
        k1b = parse_hex_bytes(k1f["b"])[:args.sig_len]
        tslb = parse_hex_bytes(tslf["b"])[:args.sig_len]

        ns = k1f["ns"]
        name = k1f["n"]

        sig = build_wildcard_sig(k1b, tslb)
        static = count_static_bytes(sig)

        entry = {
            "id": qname.replace("::", "_").replace("~", "dtor_").replace(" ", "_"),
            "name": qname,
            "class": ns,
            "method": name,
            "addresses": {
                "k1": k1f["a"],
                "tsl": tslf["a"],
            },
            "size": {
                "k1": k1f["sz"],
                "tsl": tslf["sz"],
            },
            "sig": None,
        }

        if sig and static >= args.min_static:
            sig_entry = {
                "pattern": sig,
                "static_bytes": static,
                "total_bytes": len(sig.split()),
                "confidence": "verified_both",
                "method": "binary_diff_k1_tsl",
            }

            if args.check_uniqueness:
                sig_parts = sig.split()
                k1_hits = sig_scan(sig_parts, k1_data["functions"])
                tsl_hits = sig_scan(sig_parts, tsl_data["functions"])
                sig_entry["k1_matches"] = k1_hits
                sig_entry["tsl_matches"] = tsl_hits
                sig_entry["unique_k1"] = (k1_hits == 1)
                sig_entry["unique_tsl"] = (tsl_hits == 1)

            entry["sig"] = sig_entry
            sig_ok += 1
            wildcard_ok += 1
        elif args.verbose:
            print("  SKIP_SIG: {} (static={})".format(qname, static))

        entries.append(entry)

    # 2. K1-only entries (exact byte sigs)
    for qname in k1_only:
        k1f = k1_lookup[qname]
        k1b = parse_hex_bytes(k1f["b"])[:args.sig_len]
        ns = k1f["ns"]
        name = k1f["n"]
        sig = build_exact_sig(k1b)
        static = count_static_bytes(sig)

        entry = {
            "id": qname.replace("::", "_").replace("~", "dtor_").replace(" ", "_"),
            "name": qname,
            "class": ns,
            "method": name,
            "addresses": {"k1": k1f["a"]},
            "size": {"k1": k1f["sz"]},
            "sig": None,
        }
        if sig and static >= args.min_static:
            sig_entry = {
                "pattern": sig,
                "static_bytes": static,
                "total_bytes": len(sig.split()),
                "confidence": "k1_only",
                "method": "k1_exact",
            }
            if args.check_uniqueness:
                sig_parts = sig.split()
                k1_hits = sig_scan(sig_parts, k1_data["functions"])
                sig_entry["k1_matches"] = k1_hits
                sig_entry["unique_k1"] = (k1_hits == 1)
            entry["sig"] = sig_entry
            sig_ok += 1
        entries.append(entry)

    # 3. TSL-only entries (exact byte sigs)
    for qname in tsl_only:
        tslf = tsl_lookup[qname]
        tslb = parse_hex_bytes(tslf["b"])[:args.sig_len]
        ns = tslf["ns"]
        name = tslf["n"]
        sig = build_exact_sig(tslb)
        static = count_static_bytes(sig)

        entry = {
            "id": qname.replace("::", "_").replace("~", "dtor_").replace(" ", "_"),
            "name": qname,
            "class": ns,
            "method": name,
            "addresses": {"tsl": tslf["a"]},
            "size": {"tsl": tslf["sz"]},
            "sig": None,
        }
        if sig and static >= args.min_static:
            sig_entry = {
                "pattern": sig,
                "static_bytes": static,
                "total_bytes": len(sig.split()),
                "confidence": "tsl_only",
                "method": "tsl_exact",
            }
            if args.check_uniqueness:
                sig_parts = sig.split()
                tsl_hits = sig_scan(sig_parts, tsl_data["functions"])
                sig_entry["tsl_matches"] = tsl_hits
                sig_entry["unique_tsl"] = (tsl_hits == 1)
            entry["sig"] = sig_entry
            sig_ok += 1
        entries.append(entry)

    # Build per-class index
    class_map = {}
    for e in entries:
        cls = e["class"]
        if cls:
            class_map.setdefault(cls, []).append(e["name"])

    # Stats
    total = len(entries)
    both_count = len(both)
    namespaced = sum(1 for e in entries if e["class"])

    output = {
        "meta": {
            "version": "2.0.0",
            "generated_at": datetime.now(timezone.utc).isoformat(),
            "description": (
                "Universal KotOR I / TSL function signature database. "
                "Generated from Ghidra-analyzed binaries via agdec-mcp execute-script. "
                "All 'sig.pattern' values with confidence='verified_both' are wildcard byte patterns "
                "('??' = bytes that differ between games) that locate the function in EITHER binary "
                "without any game-version conditional. "
                "Entries with confidence='k1_only' or 'tsl_only' have exact byte patterns for one game."
            ),
            "sources": {
                "k1": {"program": k1_data["program"], "function_count": k1_data["count"]},
                "tsl": {"program": tsl_data["program"], "function_count": tsl_data["count"]},
            },
            "stats": {
                "total_entries": total,
                "with_sig": sig_ok,
                "verified_both": wildcard_ok,
                "k1_only": sum(1 for e in entries if e["sig"] and e["sig"]["confidence"] == "k1_only"),
                "tsl_only": sum(1 for e in entries if e["sig"] and e["sig"]["confidence"] == "tsl_only"),
                "no_sig": total - sig_ok,
                "namespaced_entries": namespaced,
                "classes": len(class_map),
                "name_matched_both_games": both_count,
            },
        },
        "classes": {k: sorted(v) for k, v in sorted(class_map.items())},
        "functions": entries,
    }

    os.makedirs(os.path.dirname(args.out), exist_ok=True)
    with open(args.out, "w", encoding="utf-8") as fh:
        json.dump(output, fh, indent=2, ensure_ascii=False)

    print("\n--- Summary ---")
    print("  Total entries    : {}".format(total))
    print("  With signatures  : {}".format(sig_ok))
    print("  Verified (both)  : {}".format(wildcard_ok))
    print("  K1-only sigs     : {}".format(output["meta"]["stats"]["k1_only"]))
    print("  TSL-only sigs    : {}".format(output["meta"]["stats"]["tsl_only"]))
    print("  No sig           : {}".format(total - sig_ok))
    print("  Classes          : {}".format(len(class_map)))
    print("  Output           : {}".format(args.out))


if __name__ == "__main__":
    main()
