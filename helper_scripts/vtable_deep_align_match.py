"""
Deep vtable alignment matching for vtables with >= 2 slot size differences.

Extends relaxed_vtable_slot_match.py to handle larger slot count differences by
using more anchor pairs to determine the alignment offset(s).

Algorithm:
  1. For each class where |k1_slots| - |tsl_slots| == DIFF (2..MAX_SLOT_DIFF):
  2. Collect anchor pairs: (k1_slot_idx, tsl_slot_idx) where both addrs are in cascade
  3. Build a histogram of (k1_idx - tsl_idx) offsets
  4. Accept if the dominant offset has >= MIN_ANCHOR_VOTES votes AND represents
     >= MIN_ANCHOR_FRACTION of all votes (strong majority)
  5. Match unmatched TSL FUN_xxx slots to named K1 functions at (tsl_idx + offset)

Input:
  docs/k1_vtables.json             — {vtables: [{vtable_addr, class_name, slots:[{offset,fn_addr,fn_name}]}]}
  docs/tsl_vtables.json            — same format
  docs/k1_tsl_cascade_matches.json — existing cascade pairs

Output:
  docs/tsl_deep_vtable_matches.json — [{k1_addr, tsl_addr, name, class, method, via_k1}]
"""

from __future__ import annotations

import json
from collections import Counter

K1_VTABLES = "docs/k1_vtables.json"
TSL_VTABLES = "docs/tsl_vtables.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_deep_vtable_matches.json"

MIN_SLOT_DIFF = 2   # handle diffs >= this value (skip ±1 already handled by relaxed)
MAX_SLOT_DIFF = 10  # ignore extreme size mismatches (likely different classes)

MIN_ANCHOR_VOTES = 2         # minimum anchor pairs agreeing on the offset
# Safety: require anchors ≥ abs(diff)/3 to avoid single-anchor alignment
# on heavily diverged vtables (e.g. diff=9 with 2 anchors is too risky)
MIN_ANCHORS_PER_DIFF = 0.40  # need at least 40% of slot-diff as anchors
MIN_ANCHOR_FRACTION = 0.65   # dominant offset must represent >= 65% of anchors

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

    matched_k1 = {parse_hex(m["k1_addr"]) for m in cascade["matches"]}
    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cascade["matches"]}
    tsl_to_k1 = {parse_hex(m["tsl_addr"]): parse_hex(m["k1_addr"]) for m in cascade["matches"]}

    k1_by_class = {v["class_name"]: v for v in k1_data["vtables"]}
    tsl_by_class = {v["class_name"]: v for v in tsl_data["vtables"]}

    common_classes = set(k1_by_class) & set(tsl_by_class)

    stats = {
        "classes_checked": 0,
        "too_few_anchors": 0,
        "ambiguous_offset": 0,
        "matched_classes": 0,
        "new_matches": 0,
    }

    matches = []
    seen_tsl = set()

    for cls in sorted(common_classes):
        k1v = k1_by_class[cls]
        tsl_v = tsl_by_class[cls]
        k1_slots = k1v["slots"]
        tsl_slots = tsl_v["slots"]

        diff = len(k1_slots) - len(tsl_slots)
        if abs(diff) < MIN_SLOT_DIFF or abs(diff) > MAX_SLOT_DIFF:
            continue

        stats["classes_checked"] += 1

        # Build reverse indices: fn_addr -> slot index
        k1_addr_to_idx = {s["fn_addr"]: i for i, s in enumerate(k1_slots)}
        tsl_addr_to_idx = {s["fn_addr"]: i for i, s in enumerate(tsl_slots)}

        # Collect offsets from cascade anchor pairs
        offset_counter = Counter()
        for m in cascade["matches"]:
            ka = parse_hex(m["k1_addr"])
            ta = parse_hex(m["tsl_addr"])
            if ka in k1_addr_to_idx and ta in tsl_addr_to_idx:
                ki = k1_addr_to_idx[ka]
                ti = tsl_addr_to_idx[ta]
                offset_counter[ki - ti] += 1

        total_anchors = sum(offset_counter.values())
        if total_anchors == 0:
            stats["too_few_anchors"] += 1
            continue

        dominant_offset, dominant_count = offset_counter.most_common(1)[0]

        # Safety check: need enough anchors relative to the slot count difference
        slot_diff = abs(diff)
        if dominant_count < MIN_ANCHOR_VOTES:
            stats["too_few_anchors"] += 1
            continue
        if total_anchors < slot_diff * MIN_ANCHORS_PER_DIFF:
            stats["too_few_anchors"] += 1
            continue
        if dominant_count / total_anchors < MIN_ANCHOR_FRACTION:
            stats["ambiguous_offset"] += 1
            continue

        stats["matched_classes"] += 1
        offset = dominant_offset

        # Match unmatched TSL FUN_xxx to named K1 slots using the dominant offset
        class_new = 0
        for ti, tsl_slot in enumerate(tsl_slots):
            tsl_addr = tsl_slot["fn_addr"]

            # TSL slot must be FUN_xxx and unmatched
            tsl_name = tsl_slot["fn_name"]
            if tsl_name and not tsl_name.startswith("FUN_"):
                continue
            if tsl_addr in tsl_to_k1:
                continue
            if tsl_addr in seen_tsl:
                continue

            ki = ti + offset
            if ki < 0 or ki >= len(k1_slots):
                continue

            k1_slot = k1_slots[ki]
            k1_addr = k1_slot["fn_addr"]
            k1_name = k1_slot["fn_name"]

            if not k1_name or k1_name.startswith("FUN_"):
                continue
            if k1_name in GENERIC_STUB_NAMES:
                continue

            seen_tsl.add(tsl_addr)
            stats["new_matches"] += 1
            class_new += 1

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
                "via_k1": "vtable_deep:{:s}:slot{:d}(off{:+d},anchors{:d})".format(
                    cls, ti, offset, dominant_count),
            })

    print("Classes with |diff|={}-{}: {}".format(MIN_SLOT_DIFF, MAX_SLOT_DIFF, stats["classes_checked"]))
    print("  Matched (strong anchor): {}  Too few anchors: {}  Ambiguous offset: {}".format(
        stats["matched_classes"], stats["too_few_anchors"], stats["ambiguous_offset"]))
    print("New matches found: {}".format(len(matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
