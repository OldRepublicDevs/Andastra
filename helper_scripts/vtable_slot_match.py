"""
Vtable slot position matching.

For each class present in both K1 and TSL with matching vtable slot counts,
match unmatched TSL FUN_xxx slots to the named K1 function at the same position.

Input:
  docs/k1_vtables.json             — {vtables: [{vtable_addr, class_name, slots:[{offset,fn_addr,fn_name}]}]}
  docs/tsl_vtables.json            — same format
  docs/k1_tsl_cascade_matches.json — existing cascade pairs

Output:
  docs/tsl_vtable_slot_matches.json — [{k1_addr, tsl_addr, name, class, method, via_k1}]
"""

from __future__ import annotations

import json

K1_VTABLES = "docs/k1_vtables.json"
TSL_VTABLES = "docs/tsl_vtables.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_vtable_slot_matches.json"

# Maximum allowed difference in vtable slot count to still attempt position matching.
MAX_SLOT_DIFF = 0  # strict: must be exact same size


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else int(s)


def main():
    k1_data = json.load(open(K1_VTABLES, encoding="utf-8"))
    tsl_data = json.load(open(TSL_VTABLES, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    matched_k1 = {parse_hex(m["k1_addr"]) for m in cascade["matches"]}
    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cascade["matches"]}

    k1_by_class = {v["class_name"]: v for v in k1_data["vtables"]}
    tsl_by_class = {v["class_name"]: v for v in tsl_data["vtables"]}

    common_classes = set(k1_by_class) & set(tsl_by_class)
    print("K1 vtables: {}  TSL vtables: {}  Common classes: {}".format(len(k1_by_class), len(tsl_by_class), len(common_classes)))

    matches = []
    stats = {"classes_compared": 0, "size_mismatches": 0, "already_matched": 0}

    # Also build a reverse lookup: TSL fn_addr -> already present match
    tsl_to_k1 = {parse_hex(m["tsl_addr"]): parse_hex(m["k1_addr"]) for m in cascade["matches"]}

    for cls in sorted(common_classes):
        k1v = k1_by_class[cls]
        tsl_v = tsl_by_class[cls]
        k1_slots = k1v["slots"]
        tsl_slots = tsl_v["slots"]

        diff = abs(len(k1_slots) - len(tsl_slots))
        if diff > MAX_SLOT_DIFF:
            stats["size_mismatches"] += 1
            continue

        stats["classes_compared"] += 1
        n_slots = min(len(k1_slots), len(tsl_slots))

        for i in range(n_slots):
            k1_slot = k1_slots[i]
            tsl_slot = tsl_slots[i]

            # K1 slot must have a real name (not FUN_xxx, not None)
            k1_name = k1_slot["fn_name"]
            if not k1_name or k1_name.startswith("FUN_"):
                continue
            if k1_name in ("return_zero", "return_minus_one", "return_true", "return_false", "void_return", "exit_error_25"):
                # Generic stub functions are not discriminating
                continue

            # TSL slot must be unmatched (FUN_xxx or None)
            tsl_name = tsl_slot["fn_name"]
            if tsl_name and not tsl_name.startswith("FUN_"):
                stats["already_matched"] += 1
                continue

            k1_addr = k1_slot["fn_addr"]
            tsl_addr = tsl_slot["fn_addr"]

            # Skip if either is already in cascade
            if k1_addr in matched_k1 and tsl_addr in matched_tsl:
                stats["already_matched"] += 1
                continue

            # If K1/TSL addr are already mapped to each other via cascade, skip
            if tsl_addr in tsl_to_k1:
                stats["already_matched"] += 1
                continue

            # Split k1_name into class::method
            if "::" in k1_name:
                parts = k1_name.rsplit("::", 1)
                class_field = parts[0]
                method_field = parts[1]
            else:
                class_field = cls
                method_field = k1_name

            matches.append(
                {
                    "k1_addr": "0x{:08X}".format(k1_addr),
                    "tsl_addr": "0x{:08X}".format(tsl_addr),
                    "name": k1_name,
                    "class": class_field,
                    "method": method_field,
                    "via_k1": "vtable:{:s}:slot{:d}".format(cls, i),
                }
            )

    print(
        "Classes compared (same size): {}  Size mismatches skipped: {}  Already matched slots: {}".format(
            stats["classes_compared"], stats["size_mismatches"], stats["already_matched"]
        )
    )
    print("New matches found: {}".format(len(matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
