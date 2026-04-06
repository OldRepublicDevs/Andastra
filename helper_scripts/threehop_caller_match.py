"""
Three-hop caller fingerprint matching.

Caller-direction counterpart to threehop_callee_match.py.

For each unmatched TSL FUN_xxx A:
  Find matched callers B_tsl of A (B_tsl → A)
  For each B, find matched callers C_tsl of B
  For each C, find matched callers D_tsl of C
  Build triple (B_k1_name, C_k1_name, D_k1_name)

Build the same triple set for each unmatched K1 function.
If a triple set is unique to ONE unmatched K1 function → match.

Requires MIN_TRIPLES distinct triples to be trustworthy.

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_threehop_caller_matches.json
"""

from __future__ import annotations

import json
from collections import defaultdict

K1_CG = "docs/k1_call_graph.json"
TSL_CG = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT = "docs/tsl_threehop_caller_matches.json"

MIN_TRIPLES = 2
MAX_CALLERS = 25

GENERIC = frozenset({
    "", "return_zero", "return_minus_one", "void_return", "return_true",
    "return_false", "exit_error_25",
})


def is_generic(name):
    return not name or name.startswith("FUN_") or name in GENERIC


def main():
    k1cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cascade = json.load(open(CASCADE, encoding="utf-8"))

    matched_k1 = {int(m["k1_addr"], 16) for m in cascade["matches"]}
    matched_tsl = {int(m["tsl_addr"], 16) for m in cascade["matches"]}
    tsl_to_k1 = {int(m["tsl_addr"], 16): int(m["k1_addr"], 16) for m in cascade["matches"]}

    k1_addr_to_fn = {f["a"]: f for f in k1cg["functions"]}
    k1_addr_to_name = {f["a"]: f["n"] for f in k1cg["functions"]}
    tsl_addr_to_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # Build caller maps
    k1_callers_map = defaultdict(set)
    for f in k1cg["functions"]:
        for c in f.get("c", []):
            k1_callers_map[c].add(f["a"])

    tsl_callers_map = defaultdict(set)
    for f in tsl_cg["functions"]:
        for c in f.get("c", []):
            tsl_callers_map[c].add(f["a"])

    # Build K1 fingerprints: for each unmatched K1 function, compute 3-hop caller triples
    def k1_triples(ka):
        triples = set()
        b_callers = [c for c in k1_callers_map.get(ka, [])
                     if c in matched_k1 and not is_generic(k1_addr_to_name.get(c, ""))]
        if len(b_callers) > MAX_CALLERS:
            return frozenset()
        for b in b_callers:
            b_name = k1_addr_to_name.get(b, "")
            c_callers = [c for c in k1_callers_map.get(b, [])
                         if c in matched_k1 and not is_generic(k1_addr_to_name.get(c, ""))]
            if len(c_callers) > MAX_CALLERS:
                continue
            for c in c_callers:
                c_name = k1_addr_to_name.get(c, "")
                d_callers = [d for d in k1_callers_map.get(c, [])
                             if d in matched_k1 and not is_generic(k1_addr_to_name.get(d, ""))]
                if len(d_callers) > MAX_CALLERS:
                    continue
                for d in d_callers:
                    d_name = k1_addr_to_name.get(d, "")
                    triples.add((b_name, c_name, d_name))
        return frozenset(triples)

    # Build K1 fingerprint -> [ka] mapping for unmatched K1 functions
    k1_fp_to_funcs = {}
    print("Building K1 3-hop caller fingerprints...")
    for f in k1cg["functions"]:
        ka = f["a"]
        if ka in matched_k1:
            continue
        k1_name = f.get("n", "")
        if not k1_name or k1_name.startswith("FUN_"):
            continue  # only match named K1 functions
        fp = k1_triples(ka)
        if len(fp) < MIN_TRIPLES:
            continue
        k1_fp_to_funcs.setdefault(fp, []).append((ka, f))

    unique_k1_fps = {fp: funcs[0] for fp, funcs in k1_fp_to_funcs.items() if len(funcs) == 1}
    print("K1 unique 3-hop caller fingerprints (>={} triples): {}".format(MIN_TRIPLES, len(unique_k1_fps)))

    # Compute TSL fingerprints for unmatched FUN_xxx
    def tsl_triples(ta):
        triples = set()
        b_callers = [c for c in tsl_callers_map.get(ta, [])
                     if c in tsl_to_k1 and not is_generic(tsl_addr_to_name.get(c, ""))]
        if len(b_callers) > MAX_CALLERS:
            return frozenset()
        for b_tsl in b_callers:
            b_k1 = tsl_to_k1[b_tsl]
            b_name = k1_addr_to_name.get(b_k1, "")
            if is_generic(b_name):
                continue
            c_callers = [c for c in tsl_callers_map.get(b_tsl, [])
                         if c in tsl_to_k1 and not is_generic(tsl_addr_to_name.get(c, ""))]
            if len(c_callers) > MAX_CALLERS:
                continue
            for c_tsl in c_callers:
                c_k1 = tsl_to_k1[c_tsl]
                c_name = k1_addr_to_name.get(c_k1, "")
                if is_generic(c_name):
                    continue
                d_callers = [d for d in tsl_callers_map.get(c_tsl, [])
                             if d in tsl_to_k1 and not is_generic(tsl_addr_to_name.get(d, ""))]
                if len(d_callers) > MAX_CALLERS:
                    continue
                for d_tsl in d_callers:
                    d_k1 = tsl_to_k1[d_tsl]
                    d_name = k1_addr_to_name.get(d_k1, "")
                    if is_generic(d_name):
                        continue
                    triples.add((b_name, c_name, d_name))
        return frozenset(triples)

    tsl_fun_xxx = [f for f in tsl_cg["functions"]
                   if f["n"].startswith("FUN_") and f["a"] not in matched_tsl]
    print("TSL FUN_xxx to check: {}".format(len(tsl_fun_xxx)))

    matches = []
    seen_k1 = set()

    for f in tsl_fun_xxx:
        ta = f["a"]
        fp = tsl_triples(ta)
        if len(fp) < MIN_TRIPLES:
            continue
        if fp not in unique_k1_fps:
            continue
        ka, k1_fn = unique_k1_fps[fp]
        if ka in matched_k1 or ka in seen_k1:
            continue
        k1_name = k1_fn.get("n", "")
        if not k1_name or k1_name.startswith("FUN_"):
            continue
        seen_k1.add(ka)

        k1_ns = k1_fn.get("ns", "")
        if "::" in k1_name:
            parts = k1_name.rsplit("::", 1)
            class_f = parts[0]; method_f = parts[1]
        elif k1_ns:
            class_f = k1_ns; method_f = k1_name
        else:
            class_f = ""; method_f = k1_name

        matches.append({
            "k1_addr": "0x{:08X}".format(ka),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": k1_name,
            "class": class_f,
            "method": method_f,
            "via_k1": "threehop_caller:triples={}".format(len(fp)),
        })

    print("New matches found: {}".format(len(matches)))
    with open(OUT, "w", encoding="utf-8") as f:
        json.dump(matches, f, indent=2)
    print("Wrote {} matches to {}".format(len(matches), OUT))


if __name__ == "__main__":
    main()
