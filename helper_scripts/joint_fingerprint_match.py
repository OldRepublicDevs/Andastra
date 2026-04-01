#!/usr/bin/env python3
"""
Joint callee+caller fingerprint matching.

Uses BOTH the callee set AND the caller set simultaneously as a combined fingerprint.
This is much more discriminating than either alone — catches functions where neither
the callee set nor the caller set uniquely identifies them, but the COMBINATION does.

Also runs iterative waves: each wave adds freshly-named functions to unlock new matches.
"""
import json
from collections import defaultdict

TSL_CG   = "docs/tsl_call_graph.json"
K1_CG    = "docs/k1_call_graph.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_joint_fingerprint_matches.json"

MIN_COMBINED = 3   # minimum total unique named functions (callee + caller combined)
MAX_WAVES    = 10


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def build_caller_map(funcs):
    caller_map = defaultdict(set)
    for f in funcs:
        for c in f.get("c", []):
            caller_map[c].add(f["a"])
    return caller_map


def run_matching_wave(tsl_funcs, k1_funcs, tsl_addr_name, k1_addr_name,
                      matched_tsl, matched_k1):
    """One wave of joint fingerprint matching. Returns new matches list."""

    tsl_caller_map = build_caller_map(tsl_funcs)
    k1_caller_map  = build_caller_map(k1_funcs)

    # Build K1 joint fingerprints
    k1_fp_to_funcs: dict = {}
    for f in k1_funcs:
        callees = frozenset(
            k1_addr_name.get(c, "") for c in f.get("c", [])
            if k1_addr_name.get(c, "")
        )
        callers = frozenset(
            k1_addr_name.get(c, "") for c in k1_caller_map.get(f["a"], set())
            if k1_addr_name.get(c, "")
        )
        combined = callees | callers
        if len(combined) >= MIN_COMBINED:
            fp = (frozenset(callees), frozenset(callers))
            k1_fp_to_funcs.setdefault(fp, []).append(f)

    unique_k1 = {fp: flist[0] for fp, flist in k1_fp_to_funcs.items() if len(flist) == 1}

    new_matches = []
    seen_k1 = set()

    tsl_fun_xxx = [f for f in tsl_funcs if f["n"].startswith("FUN_") and f["a"] not in matched_tsl]
    for f in tsl_fun_xxx:
        tsl_a = f["a"]
        callees = frozenset(
            tsl_addr_name.get(c, "") for c in f.get("c", [])
            if tsl_addr_name.get(c, "") and not tsl_addr_name.get(c, "").startswith("FUN_")
        )
        callers = frozenset(
            tsl_addr_name.get(c, "") for c in tsl_caller_map.get(tsl_a, set())
            if tsl_addr_name.get(c, "") and not tsl_addr_name.get(c, "").startswith("FUN_")
        )
        combined = callees | callers
        if len(combined) < MIN_COMBINED:
            continue
        fp = (callees, callers)
        if fp not in unique_k1:
            continue
        k1f = unique_k1[fp]
        k1_a = k1f["a"]
        if k1_a in matched_k1 or k1_a in seen_k1:
            continue
        seen_k1.add(k1_a)
        new_matches.append({
            "k1_addr":  hex(k1_a),
            "tsl_addr": hex(tsl_a),
            "name":     (k1f.get("ns", "") + "::" if k1f.get("ns") else "") + k1f["n"],
            "class":    k1f.get("ns", ""),
            "method":   k1f["n"],
            "callees":  sorted(callees)[:6],
            "callers":  sorted(callers)[:6],
            "via":      "joint_callee_caller_fingerprint",
        })

    return new_matches


def main():
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    k1_addr_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_addr_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    all_new_matches = []
    total_before = len(matched_tsl)

    for wave in range(1, MAX_WAVES + 1):
        wave_matches = run_matching_wave(
            tsl_cg["functions"], k1_cg["functions"],
            tsl_addr_name, k1_addr_name,
            matched_tsl, matched_k1,
        )
        if not wave_matches:
            print(f"Wave {wave}: 0 new matches — converged")
            break
        print(f"Wave {wave}: {len(wave_matches)} new matches")
        for m in wave_matches[:5]:
            print(f"  {m['tsl_addr']} -> {m['k1_addr']} ({m['name']}) | c+c={len(m['callees'])+len(m['callers'])}")
        all_new_matches.extend(wave_matches)
        # Apply renames to simulated TSL name map for next wave
        for m in wave_matches:
            ta = parse_hex(m["tsl_addr"])
            matched_tsl.add(ta)
            matched_k1.add(parse_hex(m["k1_addr"]))
            tsl_addr_name[ta] = m["name"]

    print(f"\nTotal new matches across all waves: {len(all_new_matches)}")
    print(f"Matched TSL functions: {total_before} -> {len(matched_tsl)}")

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(all_new_matches, fh, indent=2)
    print(f"Wrote {len(all_new_matches)} matches to {OUT}")


if __name__ == "__main__":
    main()
