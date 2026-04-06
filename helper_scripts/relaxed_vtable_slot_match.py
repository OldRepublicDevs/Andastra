"""
Relaxed vtable slot matching (±1 slot difference).

For classes where K1 and TSL vtables differ by exactly 1 slot, use already-matched
functions (anchor points) to determine the correct alignment offset, then match
unmatched TSL FUN_xxx slots to named K1 functions at the aligned position.

Algorithm:
  1. For each class: |k1_slots| - |tsl_slots| == ±1
  2. Collect anchor pairs: (k1_slot_idx, tsl_slot_idx) where both addrs are in cascade
  3. Compute offsets (k1_idx - tsl_idx) for all anchors
  4. If all anchors agree on a single offset (or no anchors → try both and pick the one
     consistent with any partial anchors), use that offset
  5. Match unmatched TSL FUN_xxx to named K1 functions at (tsl_idx + offset)

Input:
  docs/k1_vtables.json             — {vtables: [{vtable_addr, class_name, slots:[{offset,fn_addr,fn_name}]}]}
  docs/tsl_vtables.json            — same format
  docs/k1_tsl_cascade_matches.json — existing cascade pairs

Output:
  docs/tsl_relaxed_vtable_matches.json — [{k1_addr, tsl_addr, name, class, method, via_k1}]
"""

from __future__ import annotations

import json
from collections import Counter

K1_VTABLES = "docs/k1_vtables.json"
TSL_VTABLES = "docs/tsl_vtables.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_relaxed_vtable_matches.json"

MAX_SLOT_DIFF = 1  # only match classes differing by exactly this many slots

GENERIC_STUB_NAMES = frozenset({
    "return_zero", "return_minus_one", "return_true", "return_false",
    "void_return", "exit_error_25",
})


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else int(s)


def main():
    k1_data = json.load(open(K1_VTABLES, encoding="utf-8"))
    tsl_data = json.load(open(TSL_VTABLES, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    # Build cascade lookup sets
    matched_k1 = {parse_hex(m["k1_addr"]) for m in cascade["matches"]}
    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cascade["matches"]}
    tsl_to_k1 = {parse_hex(m["tsl_addr"]): parse_hex(m["k1_addr"]) for m in cascade["matches"]}

    k1_by_class = {v["class_name"]: v for v in k1_data["vtables"]}
    tsl_by_class = {v["class_name"]: v for v in tsl_data["vtables"]}

    common_classes = set(k1_by_class) & set(tsl_by_class)

    stats = {
        "classes_checked": 0,
        "no_anchors_tried_both": 0,
        "aligned_by_anchors": 0,
        "ambiguous_offset_skipped": 0,
        "new_matches": 0,
    }

    matches = []
    seen_tsl = set()  # dedup by TSL addr

    for cls in sorted(common_classes):
        k1v = k1_by_class[cls]
        tsl_v = tsl_by_class[cls]
        k1_slots = k1v["slots"]
        tsl_slots = tsl_v["slots"]

        diff = len(k1_slots) - len(tsl_slots)
        if abs(diff) != MAX_SLOT_DIFF:
            continue  # only handle exact ±1 difference

        stats["classes_checked"] += 1

        # Build a reverse index: fn_addr -> slot index for each vtable
        k1_addr_to_idx = {s["fn_addr"]: i for i, s in enumerate(k1_slots)}
        tsl_addr_to_idx = {s["fn_addr"]: i for i, s in enumerate(tsl_slots)}

        # Collect anchor offsets
        offset_counter = Counter()
        for m in cascade["matches"]:
            ka = parse_hex(m["k1_addr"])
            ta = parse_hex(m["tsl_addr"])
            if ka in k1_addr_to_idx and ta in tsl_addr_to_idx:
                ki = k1_addr_to_idx[ka]
                ti = tsl_addr_to_idx[ta]
                offset_counter[ki - ti] += 1

        if len(offset_counter) == 0:
            # No anchors — try both possible offsets: 0 and ±1
            # Only apply if there's exactly 1 possible miss-slot (i.e., the extra slot is
            # either at front or back). For safety, skip ambiguous no-anchor classes.
            stats["no_anchors_tried_both"] += 1
            # Skip: too risky without anchors
            continue

        if len(offset_counter) > 1:
            # Multiple conflicting offsets — skip this class
            stats["ambiguous_offset_skipped"] += 1
            continue

        # Single consistent offset
        offset = next(iter(offset_counter))
        stats["aligned_by_anchors"] += 1

        # Match using this offset: for each tsl slot i, k1 slot index = i + offset
        for ti, tsl_slot in enumerate(tsl_slots):
            ki = ti + offset
            if ki < 0 or ki >= len(k1_slots):
                continue

            k1_slot = k1_slots[ki]
            k1_name = k1_slot["fn_name"]
            if not k1_name or k1_name.startswith("FUN_"):
                continue
            if k1_name in GENERIC_STUB_NAMES:
                continue

            tsl_addr = tsl_slot["fn_addr"]
            k1_addr = k1_slot["fn_addr"]

            # TSL slot must be unmatched
            tsl_name = tsl_slot["fn_name"]
            if tsl_name and not tsl_name.startswith("FUN_"):
                continue
            if tsl_addr in tsl_to_k1:
                continue
            if tsl_addr in seen_tsl:
                continue

            seen_tsl.add(tsl_addr)
            stats["new_matches"] += 1

            if "::" in k1_name:
                parts = k1_name.rsplit("::", 1)
                class_field = parts[0]
                method_field = parts[1]
            else:
                class_field = cls
                method_field = k1_name

            matches.append({
                "k1_addr": "0x{:08X}".format(k1_addr),
                "tsl_addr": "0x{:08X}".format(tsl_addr),
                "name": k1_name,
                "class": class_field,
                "method": method_field,
                "via_k1": "vtable_relaxed:{:s}:slot{:d}(off{:+d})".format(cls, ti, offset),
            })

    print("Classes with ±1 slot diff: {}".format(stats["classes_checked"]))
    print("  Aligned by anchors: {}  Ambiguous/skipped: {}  No-anchor/skipped: {}".format(
        stats["aligned_by_anchors"], stats["ambiguous_offset_skipped"], stats["no_anchors_tried_both"]))
    print("New matches found: {}".format(len(matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
