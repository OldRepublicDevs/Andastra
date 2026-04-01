#!/usr/bin/env python3
"""
Apply K1 FLIRT-style patterns from kotor_function_signatures.json against TSL function bytes
to find TSL counterparts of K1 functions that don't yet have a TSL address.

Uses a static-prefix index for fast matching without numpy:
  1. Find the first 4 static (non-wildcard) bytes in each K1 pattern.
  2. Index K1 patterns by (offset0,b0, offset1,b1, offset2,b2, offset3,b3).
  3. For each TSL function, compute its 4-byte probe keys and look up candidates.
  4. For candidates, verify the full pattern match.
"""
import json
from collections import defaultdict

SIGS      = "docs/kotor_function_signatures.json"
TSL_BYTES = "docs/tsl_function_bytes.json"
CASCADE   = "docs/k1_tsl_cascade_matches.json"
OUT       = "docs/tsl_pattern_matches.json"

BYTE_LEN   = 64   # bytes stored per function
MIN_STATIC = 8    # minimum static bytes in pattern to trust it
INDEX_LEN  = 4    # number of static bytes used for the index key


def parse_hex_bytes(s: str) -> bytes:
    return bytes(int(x, 16) for x in s.split())


def parse_pattern(pattern_str: str):
    """Return (mask, value) as bytearray pairs."""
    tokens = pattern_str.strip().split()
    mask  = bytearray()
    value = bytearray()
    for t in tokens:
        if t == "??":
            mask.append(0x00)
            value.append(0x00)
        else:
            mask.append(0xFF)
            value.append(int(t, 16))
    return bytes(mask), bytes(value)


def pattern_matches(tsl_bytes: bytes, mask: bytes, value: bytes) -> bool:
    n = len(mask)
    if len(tsl_bytes) < n:
        return False
    for i in range(n):
        if (tsl_bytes[i] & mask[i]) != (value[i] & mask[i]):
            return False
    return True


def get_static_positions(mask: bytes, count: int):
    """Return the first `count` indices where mask[i] == 0xFF."""
    positions = []
    for i, m in enumerate(mask):
        if m == 0xFF:
            positions.append(i)
            if len(positions) >= count:
                break
    return positions


def build_index_key(tsl_bytes: bytes, positions):
    """Build an index key tuple from specific byte positions."""
    return tuple((p, tsl_bytes[p]) for p in positions if p < len(tsl_bytes))


def main():
    sigs    = json.load(open(SIGS,      encoding="utf-8"))
    tsl_b   = json.load(open(TSL_BYTES, encoding="utf-8"))
    cas     = json.load(open(CASCADE,   encoding="utf-8"))

    matched_tsl = {m["tsl_addr"].lower() for m in cas["matches"]}
    matched_k1  = {m["k1_addr"].lower()  for m in cas["matches"]}

    # Build K1 pattern list
    k1_patterns = []
    for f in sigs["functions"]:
        if not f.get("sig"):
            continue
        if not f["addresses"].get("k1"):
            continue
        if f["addresses"].get("tsl"):
            continue
        sig = f["sig"]
        if sig.get("static_bytes", 0) < MIN_STATIC:
            continue
        tokens = sig["pattern"].split()
        if len(tokens) > BYTE_LEN:
            continue
        mask, value = parse_pattern(sig["pattern"])
        static_pos = get_static_positions(mask, INDEX_LEN)
        if len(static_pos) < INDEX_LEN:
            continue  # not enough static bytes to build a reliable index key
        k1_patterns.append({
            "func": f,
            "mask": mask,
            "value": value,
            "static_pos": static_pos,
        })

    print(f"K1 indexable patterns: {len(k1_patterns)}")

    # Build TSL candidate list
    tsl_candidates = []
    for f in tsl_b["functions"]:
        if not f["n"].startswith("FUN_"):
            continue
        if f["a"].lower() in matched_tsl:
            continue
        if not f.get("b"):
            continue
        raw = parse_hex_bytes(f["b"])
        tsl_candidates.append((f, raw))

    print(f"TSL FUN_xxx candidates: {len(tsl_candidates)}")

    # Build inverted index: TSL function -> index keys for the static positions
    # For each K1 pattern, we'll compute its index key and look up TSL functions
    # Strategy: group K1 patterns by their static positions, then batch-check TSL

    # Group K1 patterns by their static_pos tuple (most patterns share position sets)
    pos_to_patterns = defaultdict(list)
    for kp in k1_patterns:
        pos_to_patterns[tuple(kp["static_pos"])].append(kp)

    print(f"Unique static position sets: {len(pos_to_patterns)}")

    # For each position set, build TSL index
    new_matches = []
    seen_k1 = set()
    assigned_tsl = set()
    checked = 0

    for positions, patterns in pos_to_patterns.items():
        # Build index: (pos, byte) tuple -> [tsl func indices]
        tsl_index = defaultdict(list)
        for i, (f, raw) in enumerate(tsl_candidates):
            key = build_index_key(raw, positions)
            tsl_index[key].append(i)

        for kp in patterns:
            # Index key for this K1 pattern
            k1_key = tuple((p, kp["value"][p]) for p in positions)
            candidates = tsl_index.get(k1_key, [])
            if not candidates:
                continue

            # Full pattern match against candidates
            matches = []
            for idx in candidates:
                f, raw = tsl_candidates[idx]
                if pattern_matches(raw, kp["mask"], kp["value"]):
                    matches.append((idx, f))

            checked += len(candidates)
            if len(matches) != 1:
                continue  # 0 = no match, >1 = ambiguous

            idx, tsl_f = matches[0]
            tsl_a = tsl_f["a"].lower()
            k1_a  = kp["func"]["addresses"]["k1"].lower()

            if k1_a in matched_k1 or k1_a in seen_k1:
                continue
            if tsl_a in assigned_tsl:
                continue

            seen_k1.add(k1_a)
            assigned_tsl.add(tsl_a)
            name = kp["func"]["name"]
            new_matches.append({
                "k1_addr":      k1_a,
                "tsl_addr":     tsl_a,
                "name":         name,
                "class":        kp["func"].get("class", ""),
                "method":       kp["func"].get("method", "") or name,
                "static_bytes": kp["func"]["sig"]["static_bytes"],
                "via":          "flirt_pattern_match",
            })

    print(f"Full-match checks performed: {checked}")
    print(f"New matches: {len(new_matches)}")
    for m in sorted(new_matches, key=lambda x: x["static_bytes"], reverse=True)[:30]:
        print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) static={m['static_bytes']}")

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(new_matches, fh, indent=2)
    print(f"\nWrote {len(new_matches)} matches to {OUT}")


if __name__ == "__main__":
    main()
