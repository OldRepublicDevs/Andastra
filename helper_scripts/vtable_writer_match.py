"""
Vtable-writer matching.

For each class present in both K1 and TSL, find which functions write the
vtable pointer into [this] (constructors / destructors). Match unmatched
TSL writers to named K1 writers using the cascade + process-of-elimination.

When a class has exactly 1 unmatched K1 named writer and exactly 1 unmatched
TSL writer, they are the same function.

When multiple writers exist, use cascade to eliminate already-matched pairs,
then apply 1-to-1 matching on the remainder.

Input:
  docs/k1_vtable_writers.json  — {class: {k1_addr_str: fn_name}}
  docs/tsl_vtable_writers.json — {class: {tsl_addr_str: fn_name}}  (names may be FUN_xxx)
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_vtable_writer_matches.json
"""

from __future__ import annotations

import json

K1_WRITERS = "docs/k1_vtable_writers.json"
TSL_WRITERS = "docs/tsl_vtable_writers.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_vtable_writer_matches.json"


def parse_hex(s: str | int) -> int:
    return int(s, 16) if isinstance(s, str) else int(s)


def main() -> None:
    k1_writers: dict[str, dict[str, str]] = json.load(open(K1_WRITERS, encoding="utf-8"))
    tsl_writers: dict[str, dict[str, str]] = json.load(open(TSL_WRITERS, encoding="utf-8"))
    cascade: dict[str, list[dict[str, str]]] = json.load(open(CASCADE, encoding="utf-8"))

    cas_k1_to_tsl: dict[int, int] = {parse_hex(m["k1_addr"]): parse_hex(m["tsl_addr"]) for m in cascade["matches"]}
    cas_tsl_to_k1: dict[int, int] = {parse_hex(m["tsl_addr"]): parse_hex(m["k1_addr"]) for m in cascade["matches"]}
    matched_k1: set[int] = set(cas_k1_to_tsl.keys())
    matched_tsl: set[int] = set(cas_tsl_to_k1.keys())

    # Build name-indexed K1 writers: class -> {fn_name: k1_addr}
    # Only keep named (non-FUN_xxx) K1 writers
    k1_by_name: dict[str, dict[str, int]] = {}
    for cls, writers in k1_writers.items():
        name_to_addr: dict[str, int] = {}
        for fa_str, nm in writers.items():
            fa = int(fa_str)
            if not nm.startswith("FUN_"):
                name_to_addr[nm] = fa
        k1_by_name[cls] = name_to_addr

    matches: list[dict[str, str]] = []
    stats: dict[str, int] = {"unique_matched": 0, "via_cascade_confirm": 0, "skipped_ambiguous": 0}
    common_classes = set(k1_writers) & set(tsl_writers)

    for cls in sorted(common_classes):
        k1_class_writers: dict[str, str] = k1_writers[cls]  # {addr_str: name}
        tsl_class_writers_raw: dict[str, str] = tsl_writers[cls]  # {addr_str: name}

        # Build unmatched TSL writers for this class
        unmatched_tsl: dict[int, str] = {}  # addr_int -> fn_name
        for fa_str, nm in tsl_class_writers_raw.items():
            fa = int(fa_str)
            if fa not in matched_tsl:
                unmatched_tsl[fa] = nm

        if not unmatched_tsl:
            continue

        # Build unmatched K1 named writers
        unmatched_k1_named: dict[int, str] = {}  # addr_int -> fn_name
        for fa_str, nm in k1_class_writers.items():
            fa = int(fa_str)
            if fa not in matched_k1 and not nm.startswith("FUN_"):
                unmatched_k1_named[fa] = nm

        if not unmatched_k1_named:
            continue

        # Check if cascade already maps any K1 writer to a specific TSL writer
        # This confirms that the cascade contains cross-refs for this class
        # and helps us identify which TSL writer corresponds to each K1 writer

        # Strategy 1: via cascade — if K1 writer is already in cascade (matched),
        # its TSL pair tells us which TSL function is that K1 writer's counterpart.
        # If the TSL pair is NOT in tsl_class_writers, the class assignment may shift.
        # Strategy 2: unique pair elimination — if only 1 unmatched K1 named writer
        # and 1 unmatched TSL writer, match them directly.

        if len(unmatched_k1_named) == 1 and len(unmatched_tsl) == 1:
            k1_addr, k1_nm = list(unmatched_k1_named.items())[0]
            tsl_addr, _ = list(unmatched_tsl.items())[0]
            if "::" in k1_nm:
                cf, mf = k1_nm.rsplit("::", 1)
            else:
                cf, mf = cls, k1_nm
            matches.append(
                {
                    "k1_addr": "0x{:08X}".format(k1_addr),
                    "tsl_addr": "0x{:08X}".format(tsl_addr),
                    "name": k1_nm,
                    "class": cf,
                    "method": mf,
                    "via_k1": "vtable_writer:{:s}:unique".format(cls),
                }
            )
            stats["unique_matched"] += 1

        else:
            # Try to use cascade to pair remaining K1/TSL writers
            # Already-matched K1 writers map to specific TSL addrs via cascade.
            # Eliminate those TSL addrs from the unmatched TSL pool, then try again.
            tsl_claimed_by_cascade: set[int] = set()
            for fa_str, nm in k1_class_writers.items():
                fa = int(fa_str)
                if fa in cas_k1_to_tsl:
                    tsl_claimed_by_cascade.add(cas_k1_to_tsl[fa])

            reduced_tsl: dict[int, str] = {ta: tn for ta, tn in unmatched_tsl.items() if ta not in tsl_claimed_by_cascade}

            if len(unmatched_k1_named) == 1 and len(reduced_tsl) == 1:
                k1_addr, k1_nm = list(unmatched_k1_named.items())[0]
                tsl_addr, _ = list(reduced_tsl.items())[0]
                if "::" in k1_nm:
                    cf, mf = k1_nm.rsplit("::", 1)
                else:
                    cf, mf = cls, k1_nm
                matches.append(
                    {
                        "k1_addr": "0x{:08X}".format(k1_addr),
                        "tsl_addr": "0x{:08X}".format(tsl_addr),
                        "name": k1_nm,
                        "class": cf,
                        "method": mf,
                        "via_k1": "vtable_writer:{:s}:reduced".format(cls),
                    }
                )
                stats["via_cascade_confirm"] += 1
            else:
                stats["skipped_ambiguous"] += 1

    print("Unique-pair matches:          {:4d}".format(stats["unique_matched"]))
    print("Cascade-reduced pair matches: {:4d}".format(stats["via_cascade_confirm"]))
    print("Ambiguous (skipped):          {:4d}".format(stats["skipped_ambiguous"]))
    print("Total new matches:            {:4d}".format(len(matches)))

    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
