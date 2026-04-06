"""
Parent callee elimination matching.

For each matched (K1_parent, TSL_parent) pair:
  - Find K1 FUN_xxx callees (unmatched K1 functions called by K1_parent)
  - Find TSL FUN_xxx callees (unmatched TSL FUN_xxx called by TSL_parent)
  - When |k1_orphans| == |tsl_funs| == 1: direct 1:1 match (high confidence)
  - When |k1_orphans| == |tsl_funs| >= 2: use function size ordering as tiebreak

The positional (index) ordering is only used for bags of size >= 2 where sizes
within the bag are all distinct (unambiguous).  Identical-size functions in a
bag are skipped unless there's only one bag of that size.
"""
from __future__ import annotations

import json
from collections import defaultdict
from typing import Any

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_parent_callee_matches.json"

# For size-ordered bags: max bag size to attempt
MAX_BAG_SIZE = 8


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def get_func_size(cg_funcs: list[dict[str, Any]]) -> dict[int, int]:
    """Approximate function size from call graph: use callee list length as proxy.
    For better accuracy we'd need instruction counts but we only have CG data.
    Use the number of callees as a structural proxy."""
    return {f["a"]: len(f.get("c", [])) for f in cg_funcs}


def main() -> None:
    k1_cg: dict[str, Any]  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg: dict[str, Any] = json.load(open(TSL_CG, encoding="utf-8"))
    cas: dict[str, Any]    = json.load(open(CASCADE, encoding="utf-8"))

    matched_k1:  set[int] = {parse_hex(m["k1_addr"])  for m in cas["matches"]}
    matched_tsl: set[int] = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}

    k1_name: dict[int, str]  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name: dict[int, str] = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # Build a translation table: K1 addr -> TSL addr (and reverse)
    k1_to_tsl: dict[int, int] = {parse_hex(m["k1_addr"]): parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    tsl_to_k1: dict[int, int] = {v: k for k, v in k1_to_tsl.items()}

    # Build K1/TSL callee indexes: addr -> [callees]
    k1_callees: dict[int, list[int]] = {f["a"]: f.get("c", []) for f in k1_cg["functions"]}
    tsl_callees: dict[int, list[int]] = {f["a"]: f.get("c", []) for f in tsl_cg["functions"]}

    # Proxy sizes from callee counts
    k1_size_proxy: dict[int, int] = get_func_size(k1_cg["functions"])
    tsl_size_proxy: dict[int, int] = get_func_size(tsl_cg["functions"])

    matches: list[dict[str, str]] = []
    stats = {"direct_1to1": 0, "size_ordered": 0, "skipped_ambiguous": 0, "skipped_size_conflict": 0}

    # Iterate over all matched parent pairs
    for m in cas["matches"]:
        k1_parent = parse_hex(m["k1_addr"])
        tsl_parent = parse_hex(m["tsl_addr"])

        k1_orphans = [c for c in k1_callees.get(k1_parent, [])
                      if c not in matched_k1 and not k1_name.get(c, "").startswith("FUN_") is False
                      and k1_name.get(c, "").startswith("FUN_") is False  # K1 has no FUN_xxx - all named
                      and c not in matched_k1]
        # Actually K1 funcs are all named - orphan means "not yet matched to TSL"
        k1_orphans = [c for c in k1_callees.get(k1_parent, [])
                      if c not in matched_k1]

        tsl_funxxx = [c for c in tsl_callees.get(tsl_parent, [])
                      if c not in matched_tsl and tsl_name.get(c, "").startswith("FUN_")]

        if not k1_orphans or not tsl_funxxx:
            continue

        n_k1 = len(k1_orphans)
        n_tsl = len(tsl_funxxx)

        if n_k1 != n_tsl:
            continue  # Bag sizes don't match

        if n_k1 > MAX_BAG_SIZE:
            stats["skipped_ambiguous"] += 1
            continue

        # Build a deduplicated set of unique orphan/funtsl addresses
        # (avoid duplicates from multiple callsite edges)
        k1_set = list(dict.fromkeys(k1_orphans))
        tsl_set = list(dict.fromkeys(tsl_funxxx))
        if len(k1_set) != len(tsl_set):
            stats["skipped_ambiguous"] += 1
            continue

        if len(k1_set) == 1:
            # Direct 1:1 match - highest confidence
            k1_a = k1_set[0]
            tsl_a = tsl_set[0]
            if k1_a in matched_k1 or tsl_a in matched_tsl:
                continue
            kn = k1_name.get(k1_a, "")
            if not kn:
                continue
            ns = m.get("class", "")
            meth = kn.rsplit("::", 1)[-1] if "::" in kn else kn
            cls = kn.rsplit("::", 1)[0] if "::" in kn else ns
            matches.append({
                "k1_addr":  "0x{:08X}".format(k1_a),
                "tsl_addr": "0x{:08X}".format(tsl_a),
                "name":     kn,
                "class":    cls,
                "method":   meth,
                "via_k1":   "parent_elim:1to1:parent={}".format(m.get("method", "?")),
            })
            stats["direct_1to1"] += 1
        else:
            # Multi-bag: sort by callee-count proxy and pair positionally
            k1_sorted = sorted(k1_set, key=lambda a: k1_size_proxy.get(a, 0))
            tsl_sorted = sorted(tsl_set, key=lambda a: tsl_size_proxy.get(a, 0))

            # Only proceed if the size ordering is unambiguous (all distinct sizes)
            k1_sizes = [k1_size_proxy.get(a, 0) for a in k1_sorted]
            tsl_sizes = [tsl_size_proxy.get(a, 0) for a in tsl_sorted]
            if len(set(k1_sizes)) < len(k1_set) or len(set(tsl_sizes)) < len(tsl_set):
                stats["skipped_size_conflict"] += 1
                continue

            for k1_a, tsl_a in zip(k1_sorted, tsl_sorted):
                if k1_a in matched_k1 or tsl_a in matched_tsl:
                    continue
                kn = k1_name.get(k1_a, "")
                if not kn:
                    continue
                meth = kn.rsplit("::", 1)[-1] if "::" in kn else kn
                cls = kn.rsplit("::", 1)[0] if "::" in kn else ""
                matches.append({
                    "k1_addr":  "0x{:08X}".format(k1_a),
                    "tsl_addr": "0x{:08X}".format(tsl_a),
                    "name":     kn,
                    "class":    cls,
                    "method":   meth,
                    "via_k1":   "parent_elim:size_ordered_n{}:parent={}".format(n_k1, m.get("method", "?")),
                })
            stats["size_ordered"] += len(k1_set)

    # Deduplicate: if same k1_addr or tsl_addr appears twice, keep only the first
    seen_k1: set[str] = set()
    seen_tsl: set[str] = set()
    deduped: list[dict[str, str]] = []
    for match in matches:
        if match["k1_addr"] in seen_k1 or match["tsl_addr"] in seen_tsl:
            continue
        seen_k1.add(match["k1_addr"])
        seen_tsl.add(match["tsl_addr"])
        deduped.append(match)

    print("Stats: direct_1to1={direct_1to1}, size_ordered={size_ordered}, "
          "skipped_ambiguous={skipped_ambiguous}, skipped_size_conflict={skipped_size_conflict}".format(**stats))
    print("Matches before dedup: {}  after: {}".format(len(matches), len(deduped)))
    print("Sample matches:")
    for mm in deduped[:15]:
        print("  {} -> {}  {}".format(mm["tsl_addr"], mm["k1_addr"], mm["name"]))

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(deduped, fh, indent=2)
    print("Wrote {} matches to {}".format(len(deduped), OUT))


if __name__ == "__main__":
    main()
