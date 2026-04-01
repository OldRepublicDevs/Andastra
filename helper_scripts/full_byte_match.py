#!/usr/bin/env python3
"""
Full-function masked byte matching for K1-TSL function pairs.

For functions where:
  - Same byte count (within SIZE_TOLERANCE %)
  - Same masked byte sequence (call/jump targets blanked out)
  - Only 1 K1 function has this fingerprint (unique)

This is much more reliable than prologue-only matching.
"""
import json

K1_BYTES  = "docs/k1_function_bytes.json"
TSL_BYTES = "docs/tsl_function_bytes.json"
CASCADE   = "docs/k1_tsl_cascade_matches.json"
OUT       = "docs/tsl_full_byte_matches.json"

# Only try matching on functions large enough to have meaningful fingerprints
MIN_SIZE = 16
# Maximum bytes to use (cap large functions to avoid memory issues)
MAX_BYTES = 256
# Size tolerance: TSL function size must be within X% of K1 size
SIZE_TOLERANCE = 0.20


def parse_hex_bytes(s: str) -> bytes:
    return bytes(int(x, 16) for x in s.split())


def masked_bytes(raw: bytes, limit: int) -> tuple:
    """Mask call/jump relative target bytes and return as tuple for hashing."""
    b = list(raw[:limit])
    m = [0xFF] * len(b)

    i = 0
    while i < len(b):
        op = b[i]

        if op in (0xE8, 0xE9) and i + 4 < len(b):
            m[i+1] = m[i+2] = m[i+3] = m[i+4] = 0x00
            i += 5
            continue

        if op == 0xFF and i + 5 < len(b) and b[i+1] in (0x15, 0x25):
            m[i+2] = m[i+3] = m[i+4] = m[i+5] = 0x00
            i += 6
            continue

        if op in (0x8B, 0x89) and i + 5 < len(b) and (b[i+1] & 0xC7) == 0x05:
            m[i+2] = m[i+3] = m[i+4] = m[i+5] = 0x00
            i += 6
            continue

        # PUSH imm32 with address-like values (heuristic: > 0x00400000)
        if op == 0x68 and i + 4 < len(b):
            val = int.from_bytes(bytes(b[i+1:i+5]), 'little')
            if val >= 0x00400000:
                m[i+1] = m[i+2] = m[i+3] = m[i+4] = 0x00
            i += 5
            continue

        # MOV reg, imm32 where imm is an address  (B8..BF)
        if 0xB8 <= op <= 0xBF and i + 4 < len(b):
            val = int.from_bytes(bytes(b[i+1:i+5]), 'little')
            if val >= 0x00400000:
                m[i+1] = m[i+2] = m[i+3] = m[i+4] = 0x00
            i += 5
            continue

        i += 1

    return tuple(v & mk for v, mk in zip(b, m))


def main():
    k1_data  = json.load(open(K1_BYTES,  encoding="utf-8"))
    tsl_data = json.load(open(TSL_BYTES, encoding="utf-8"))
    cas      = json.load(open(CASCADE,   encoding="utf-8"))

    matched_tsl = {m["tsl_addr"].lower() for m in cas["matches"]}
    matched_k1  = {m["k1_addr"].lower()  for m in cas["matches"]}

    print("Building K1 fingerprints...")
    # Key: (size_bucket, masked_bytes_tuple) -> [func]
    # Use size + masked bytes together for tighter matching
    k1_fp: dict = {}
    for f in k1_data["functions"]:
        if f.get("sz", 0) < MIN_SIZE or not f.get("b"):
            continue
        raw = parse_hex_bytes(f["b"])
        limit = min(len(raw), MAX_BYTES)
        fp = (f["sz"], masked_bytes(raw, limit))
        k1_fp.setdefault(fp, []).append(f)

    unique_k1 = {fp: flist[0] for fp, flist in k1_fp.items() if len(flist) == 1}
    print(f"K1 unique fingerprints: {len(unique_k1)}")

    tsl_fun_xxx = [f for f in tsl_data["functions"]
                   if f["n"].startswith("FUN_") and f["a"].lower() not in matched_tsl
                   and f.get("sz", 0) >= MIN_SIZE and f.get("b")]
    print(f"TSL FUN_xxx candidates: {len(tsl_fun_xxx)}")

    new_matches = []
    seen_k1 = set()

    for f in tsl_fun_xxx:
        raw = parse_hex_bytes(f["b"])
        limit = min(len(raw), MAX_BYTES)
        fp = (f["sz"], masked_bytes(raw, limit))
        if fp not in unique_k1:
            continue
        k1f = unique_k1[fp]
        k1_a = k1f["a"].lower()
        if k1_a in matched_k1 or k1_a in seen_k1:
            continue
        seen_k1.add(k1_a)
        name = (k1f.get("ns", "") + "::" if k1f.get("ns") else "") + k1f["n"]
        new_matches.append({
            "k1_addr": k1_a,
            "tsl_addr": f["a"].lower(),
            "name": name,
            "class": k1f.get("ns", ""),
            "method": k1f["n"],
            "k1_size": k1f["sz"],
            "tsl_size": f["sz"],
            "via": "full_byte_masked_fingerprint",
        })

    print(f"New matches: {len(new_matches)}")
    for m in new_matches[:30]:
        sz_ratio = m["tsl_size"] / m["k1_size"] if m["k1_size"] else 0
        print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) "
              f"k1sz={m['k1_size']} tslsz={m['tsl_size']} ratio={sz_ratio:.2f}")

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(new_matches, fh, indent=2)
    print(f"\nWrote {len(new_matches)} matches to {OUT}")


if __name__ == "__main__":
    main()
