"""
Projected callee intersection via cascade mapping.

Insight: If we know TSL FUN_xxx T, and we know T is called by TSL functions
that are in our cascade (mapped to K1 functions K1A, K1B, ...), then T's K1
counterpart must be a function that is called by ALL of K1A, K1B, ...
(i.e., it's in the intersection of their callee sets).

If that intersection contains exactly one unmatched K1 function, we have a match.

This is the "reverse cascade lookup" — propagating identity through the call graph
using already-established pairs as anchors.

Also: the dual approach — for each TSL FUN_xxx, find its K1 counterpart via
its CALLEES that are already cascade-mapped (project callees through cascade
and look for K1 functions that call all of them).
"""
import json
from collections import defaultdict

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_projected_intersection_matches.json"

MIN_ANCHORS = 2   # minimum cascade anchors for a reliable projection

def parse_hex(s):
    return int(s,16) if isinstance(s,str) else s

def main():
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    # Cascade: TSL addr -> K1 addr (for already-matched pairs)
    tsl_to_k1 = {parse_hex(m["tsl_addr"]): parse_hex(m["k1_addr"]) for m in cas["matches"]}
    k1_to_tsl = {parse_hex(m["k1_addr"]): parse_hex(m["tsl_addr"]) for m in cas["matches"]}

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    k1_ns    = {f["a"]: f.get("ns","") for f in k1_cg["functions"]}

    # Build caller maps
    tsl_caller_map = defaultdict(set)   # callee -> {caller addrs}
    k1_caller_map  = defaultdict(set)
    for f in tsl_cg["functions"]:
        for c in f.get("c",[]): tsl_caller_map[c].add(f["a"])
    for f in k1_cg["functions"]:
        for c in f.get("c",[]): k1_caller_map[c].add(f["a"])

    # K1 callee map: K1 addr -> set of callee addrs
    k1_callees = {f["a"]: set(f.get("c",[])) for f in k1_cg["functions"]}

    # ------------------------------------------------------------------
    # APPROACH 1: caller-projection
    # For TSL FUN_xxx T, look at its callers. Map them to K1 via cascade.
    # Intersect K1 callers' callee sets -> find unique unmatched K1 function.
    # ------------------------------------------------------------------
    matches1 = []
    seen_k1 = set()

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue

        # TSL callers of this FUN_xxx that are in the cascade
        anchored_tsl_callers = [
            c for c in tsl_caller_map.get(f["a"], set())
            if c in tsl_to_k1
        ]
        if len(anchored_tsl_callers) < MIN_ANCHORS:
            continue

        # Map TSL callers to K1 callers
        k1_callers = [tsl_to_k1[c] for c in anchored_tsl_callers]

        # Intersect callees of all K1 callers
        callee_sets = [k1_callees.get(ka, set()) for ka in k1_callers]
        if not callee_sets:
            continue
        intersection = callee_sets[0].intersection(*callee_sets[1:])

        # Remove already matched K1 functions from intersection
        candidates = [c for c in intersection
                      if c not in matched_k1 and c not in seen_k1]
        if len(candidates) != 1:
            continue

        k1a = candidates[0]
        seen_k1.add(k1a)

        k1f_matches = [x for x in k1_cg["functions"] if x["a"] == k1a]
        if not k1f_matches:
            continue
        k1f = k1f_matches[0]

        matches1.append({
            "k1_addr":  hex(k1a),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "n_anchors": len(anchored_tsl_callers),
            "via":      "projected_caller_intersection",
        })

    print("Approach 1 (caller projection): {} matches".format(len(matches1)))
    for m in sorted(matches1, key=lambda x: -x["n_anchors"])[:15]:
        print("  {} -> {:12s}  anchors={}  {}".format(
            m["tsl_addr"], m["k1_addr"], m["n_anchors"], m["name"]))

    # ------------------------------------------------------------------
    # APPROACH 2: callee-projection
    # For TSL FUN_xxx T, look at its callees. Map them to K1 via cascade.
    # Find K1 functions that call ALL of those K1 callees.
    # If unique unmatched K1 function -> match.
    # ------------------------------------------------------------------
    # Build K1 caller-of-callee index: k1_callee -> {k1_funcs that call it}
    k1_callee_callers = defaultdict(set)
    for f in k1_cg["functions"]:
        for c in f.get("c",[]): k1_callee_callers[c].add(f["a"])

    matches2 = []
    seen_k1_2 = set(seen_k1)

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue

        # TSL callees of this FUN_xxx that are in the cascade
        anchored_tsl_callees = [
            c for c in f.get("c",[])
            if c in tsl_to_k1
        ]
        if len(anchored_tsl_callees) < MIN_ANCHORS:
            continue

        # Map TSL callees to K1 callees
        k1_callee_addrs = [tsl_to_k1[c] for c in anchored_tsl_callees]

        # Find K1 functions that call ALL of these K1 callees
        k1_sets = [k1_callee_callers.get(kc, set()) for kc in k1_callee_addrs]
        if not k1_sets:
            continue
        caller_intersection = k1_sets[0].intersection(*k1_sets[1:])

        candidates = [c for c in caller_intersection
                      if c not in matched_k1 and c not in seen_k1_2]
        if len(candidates) != 1:
            continue

        k1a = candidates[0]
        seen_k1_2.add(k1a)

        k1f_matches = [x for x in k1_cg["functions"] if x["a"] == k1a]
        if not k1f_matches:
            continue
        k1f = k1f_matches[0]

        matches2.append({
            "k1_addr":  hex(k1a),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "n_anchors": len(anchored_tsl_callees),
            "via":      "projected_callee_intersection",
        })

    print("\nApproach 2 (callee projection): {} matches".format(len(matches2)))
    for m in sorted(matches2, key=lambda x: -x["n_anchors"])[:15]:
        print("  {} -> {:12s}  anchors={}  {}".format(
            m["tsl_addr"], m["k1_addr"], m["n_anchors"], m["name"]))

    all_matches = matches1 + matches2
    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(all_matches, fh, indent=2)
    print("\nTotal: {}  Wrote to {}".format(len(all_matches), OUT))


if __name__ == "__main__":
    main()
