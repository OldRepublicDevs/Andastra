#!/usr/bin/env python3
"""
Match remaining TSL FUN_xxx to K1 named functions using masked byte-prologue fingerprinting.

Strategy:
  1. Parse each function's hex bytes.
  2. Mask "variable" instruction patterns (CALL rel32, JMP rel32, MOV reg,[abs32]).
  3. Use the first WINDOW bytes + mask as a fingerprint.
  4. Functions with identical masked prologues that appear uniquely in both games = matches.

This catches functions where the code structure is identical but call/jump targets differ.
"""
import json
from pathlib import Path

K1_BYTES  = "docs/k1_function_bytes.json"
TSL_BYTES = "docs/tsl_function_bytes.json"
CASCADE   = "docs/k1_tsl_cascade_matches.json"
OUT       = "docs/tsl_byte_prologue_matches.json"

WINDOW    = 32   # bytes to use from start of function
MIN_WINDOW = 16  # skip functions shorter than this


def parse_hex_bytes(s: str) -> bytes:
    return bytes(int(x, 16) for x in s.split())


def masked_prologue(raw: bytes, window: int) -> tuple:
    """
    Return a (mask, value) fingerprint for the first `window` bytes.
    Bytes that are part of 32-bit relative/absolute addresses are masked to 0.
    The mask has 0x00 at "don't care" positions and 0xFF at "must match" positions.
    """
    b = list(raw[:window]) if len(raw) >= window else list(raw)
    m = [0xFF] * len(b)  # mask (0xFF = significant)

    i = 0
    while i < len(b):
        op = b[i]

        # CALL rel32 | JMP rel32
        if op in (0xE8, 0xE9) and i + 4 < len(b):
            m[i+1] = m[i+2] = m[i+3] = m[i+4] = 0x00
            i += 5
            continue

        # Short JMP / Jcc
        if op in (0xEB,) and i + 1 < len(b):
            m[i+1] = 0x00
            i += 2
            continue

        # JMP/CALL [mem32]  FF 25 xx xx xx xx  or  FF 15 xx xx xx xx
        if op == 0xFF and i + 5 < len(b) and b[i+1] in (0x15, 0x25):
            m[i+2] = m[i+3] = m[i+4] = m[i+5] = 0x00
            i += 6
            continue

        # MOV reg, [abs32]:  8B xx xx xx xx xx  (ModRM=05 = disp32 only)
        if op == 0x8B and i + 5 < len(b) and (b[i+1] & 0xC7) == 0x05:
            m[i+2] = m[i+3] = m[i+4] = m[i+5] = 0x00
            i += 6
            continue

        # MOV [abs32], reg:  89 05 xx xx xx xx  etc.
        if op == 0x89 and i + 5 < len(b) and (b[i+1] & 0xC7) == 0x05:
            m[i+2] = m[i+3] = m[i+4] = m[i+5] = 0x00
            i += 6
            continue

        # PUSH imm32 / MOV reg, imm32  (might be an address or a literal)
        # We keep these because non-address immediates are characteristic.

        i += 1

    masked_val = tuple(v & mk for v, mk in zip(b, m))
    return (tuple(m), masked_val)


def main():
    k1_data  = json.load(open(K1_BYTES,  encoding="utf-8"))
    tsl_data = json.load(open(TSL_BYTES, encoding="utf-8"))
    cas      = json.load(open(CASCADE,   encoding="utf-8"))

    matched_tsl = {m["tsl_addr"].lower() for m in cas["matches"]}
    matched_k1  = {m["k1_addr"].lower()  for m in cas["matches"]}

    # Build K1 fingerprint -> list of (func, raw_bytes)
    k1_fp_to_funcs: dict = {}
    skipped_k1 = 0
    for f in k1_data["functions"]:
        if len(f.get("b", "")) == 0:
            continue
        raw = parse_hex_bytes(f["b"])
        if len(raw) < MIN_WINDOW:
            skipped_k1 += 1
            continue
        fp = masked_prologue(raw, WINDOW)
        k1_fp_to_funcs.setdefault(fp, []).append(f)

    unique_k1 = {fp: flist[0] for fp, flist in k1_fp_to_funcs.items() if len(flist) == 1}
    print(f"K1 unique prologue fingerprints ({WINDOW}-byte): {len(unique_k1)}  (skipped short: {skipped_k1})")

    tsl_fun_xxx = [f for f in tsl_data["functions"]
                   if f["n"].startswith("FUN_") and f["a"].lower() not in matched_tsl]
    print(f"TSL FUN_xxx candidates: {len(tsl_fun_xxx)}")

    new_matches = []
    seen_k1 = set()

    for f in tsl_fun_xxx:
        raw = parse_hex_bytes(f.get("b", ""))
        if len(raw) < MIN_WINDOW:
            continue
        fp = masked_prologue(raw, WINDOW)
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
            "via": "byte_prologue_fingerprint",
        })

    print(f"New matches: {len(new_matches)}")
    for m in new_matches[:20]:
        print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) k1sz={m['k1_size']} tslsz={m['tsl_size']}")

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(new_matches, fh, indent=2)
    print(f"\nWrote {len(new_matches)} matches to {OUT}")


if __name__ == "__main__":
    main()
