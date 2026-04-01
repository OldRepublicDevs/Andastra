"""
Caller-namespace clustering + positional disambiguation.

Approach A — Namespace restriction:
  For each TSL FUN_xxx with named callers:
    1. Identify the dominant namespace of those callers
    2. Look for K1 functions in that same namespace with the same callee fingerprint
    3. If unique within that namespace, match them

Approach B — Positional ordering within namespace:
  For C_collision cases where 2+ K1 functions have the same named-callee fingerprint:
    1. Among the K1 candidates, rank by address
    2. Among TSL neighbors (sorted by TSL addr), pick the positionally corresponding K1 func
    3. Require that the K1 rank == TSL rank among still-unmatched functions in that NS

Both approaches combined: namespace-restricted + positional tiebreak.
"""
import json
from collections import defaultdict, Counter

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT_A   = "docs/tsl_ns_cluster_matches.json"
OUT_B   = "docs/tsl_positional_matches.json"

MIN_NS_CALLERS = 2      # minimum callers from the dominant namespace
MIN_NAMED_FP   = 1      # minimum named callees for namespace-restricted match
POSITION_WINDOW = 3     # max positional rank difference allowed (for approach B)

GENERIC = frozenset({"_free","_malloc","_memcpy","_memset","_memmove","_strlen","_strcpy",
           "_strcat","_strcmp","_sprintf","_printf","__chkstk","operator_new",
           "operator_delete","FUN_","j__"})

def parse_hex(s):
    return int(s,16) if isinstance(s,str) else s

def is_generic(n):
    return (not n or n.startswith("FUN_") or
            any(n.startswith(g) for g in GENERIC))

def main():
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    k1_ns    = {f["a"]: f.get("ns","") for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}
    tsl_ns   = {f["a"]: f.get("ns","") for f in tsl_cg["functions"]}

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    # Cascade virtual names for TSL
    tsl_vname = dict(tsl_name)
    tsl_vns   = dict(tsl_ns)   # virtual namespace (filled from cascade)
    for m in cas["matches"]:
        ta = parse_hex(m["tsl_addr"])
        if tsl_vname.get(ta,"").startswith("FUN_"):
            tsl_vname[ta] = m["name"]
            tsl_vns[ta]   = m.get("class","")

    tsl_caller_map = defaultdict(set)
    k1_caller_map  = defaultdict(set)
    for f in tsl_cg["functions"]:
        for c in f.get("c",[]): tsl_caller_map[c].add(f["a"])
    for f in k1_cg["functions"]:
        for c in f.get("c",[]): k1_caller_map[c].add(f["a"])

    # K1 index: (namespace, named_callee_frozenset) -> [k1_addr]
    k1_ns_fp = defaultdict(list)
    for f in k1_cg["functions"]:
        if not f.get("ns"): continue
        cs = frozenset(k1_name.get(c,"") for c in f.get("c",[])
                       if not is_generic(k1_name.get(c,"")))
        k1_ns_fp[(f["ns"], cs)].append(f["a"])

    # ------------------------------------------------------------------
    # APPROACH A: namespace-restricted fingerprint
    # ------------------------------------------------------------------
    matches_a = []
    seen_k1_a = set()

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl: continue

        # Gather named callers' namespaces
        caller_ns_list = [
            tsl_vns.get(c,"") for c in tsl_caller_map.get(f["a"],set())
            if tsl_vns.get(c,"") and not tsl_vname.get(c,"").startswith("FUN_")
        ]
        if not caller_ns_list: continue
        ns_counter = Counter(caller_ns_list)
        dominant_ns, count = ns_counter.most_common(1)[0]
        if count < MIN_NS_CALLERS or not dominant_ns: continue

        # Named callees of this TSL function (virtual)
        cs = frozenset(tsl_vname.get(c,"") for c in f.get("c",[])
                       if not is_generic(tsl_vname.get(c,"")))

        # Look in K1 same namespace
        candidates = k1_ns_fp.get((dominant_ns, cs), [])
        if len(candidates) != 1: continue
        k1a = candidates[0]
        if k1a in matched_k1 or k1a in seen_k1_a: continue
        seen_k1_a.add(k1a)

        k1f = next(x for x in k1_cg["functions"] if x["a"]==k1a)
        matches_a.append({
            "k1_addr":  hex(k1a),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "dominant_ns": dominant_ns,
            "ns_caller_count": count,
            "via":      "ns_cluster_fingerprint",
        })

    print("Approach A (namespace clustering): {} matches".format(len(matches_a)))
    for m in matches_a[:15]:
        print("  {} -> {:12s}  ns={}  {}".format(
            m["tsl_addr"], m["k1_addr"], m["dominant_ns"][:20], m["name"]))

    # ------------------------------------------------------------------
    # APPROACH B: positional ordering within namespace
    # ------------------------------------------------------------------
    # For each namespace with >= 5 matched pairs, build rank maps
    # k1 rank = position of k1_addr in sorted matched k1 addrs for that namespace
    # tsl rank = position of tsl_addr in sorted matched tsl addrs for that namespace

    ns_k1_pairs  = defaultdict(list)   # ns -> [(k1_a, tsl_a)]
    for m in cas["matches"]:
        ns = m.get("class","")
        if ns:
            ns_k1_pairs[ns].append((parse_hex(m["k1_addr"]), parse_hex(m["tsl_addr"])))

    matches_b = []
    seen_k1_b = set()

    for ns, pairs in ns_k1_pairs.items():
        if len(pairs) < 5: continue
        # Sort both lists
        pairs_sorted_k1  = sorted(pairs, key=lambda p: p[0])
        pairs_sorted_tsl = sorted(pairs, key=lambda p: p[1])

        # Already matched addrs
        matched_k1_ns  = {p[0] for p in pairs}
        matched_tsl_ns = {p[1] for p in pairs}

        # Unmatched K1 in this namespace (sorted)
        unmatched_k1_ns = sorted(
            f["a"] for f in k1_cg["functions"]
            if f.get("ns") == ns and f["a"] not in matched_k1 and f["a"] not in seen_k1_b
        )
        # Unmatched TSL FUN_xxx in this namespace (by dominant caller ns proximity)
        # Estimate: TSL FUN_xxx whose callers are predominantly in this namespace
        unmatched_tsl_ns = sorted(
            f["a"] for f in tsl_cg["functions"]
            if f["n"].startswith("FUN_") and f["a"] not in matched_tsl
            and tsl_caller_map.get(f["a"],set())
            and Counter(
                tsl_vns.get(c,"") for c in tsl_caller_map.get(f["a"],set())
                if tsl_vns.get(c,"")
            ).most_common(1) and
            Counter(
                tsl_vns.get(c,"") for c in tsl_caller_map.get(f["a"],set())
                if tsl_vns.get(c,"")
            ).most_common(1)[0][0] == ns
        )

        if not unmatched_k1_ns or not unmatched_tsl_ns: continue
        if len(unmatched_k1_ns) != len(unmatched_tsl_ns): continue  # only if exact count
        if len(unmatched_k1_ns) > 6: continue  # too risky for larger gaps

        # Check positional ordering is consistent for the matched pairs
        # (matched tsl order should mirror matched k1 order)
        k1_order  = {p[0]: i for i,p in enumerate(pairs_sorted_k1)}
        tsl_order = {p[1]: i for i,p in enumerate(pairs_sorted_tsl)}
        consistent = sum(1 for p in pairs if abs(k1_order[p[0]] - tsl_order[p[1]]) <= 2)
        if consistent < 0.65 * len(pairs): continue

        # Match unmatched by position
        for k1a, tsl_a in zip(unmatched_k1_ns, unmatched_tsl_ns):
            if k1a in matched_k1 or k1a in seen_k1_b: continue
            seen_k1_b.add(k1a)
            k1f = next((x for x in k1_cg["functions"] if x["a"]==k1a), None)
            if k1f is None: continue
            matches_b.append({
                "k1_addr":  hex(k1a),
                "tsl_addr": hex(tsl_a),
                "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
                "class":    k1f.get("ns",""),
                "method":   k1f["n"],
                "ns":       ns,
                "ns_pairs": len(pairs),
                "via":      "positional_ordering",
            })

    print("\nApproach B (positional ordering): {} matches".format(len(matches_b)))
    for m in matches_b[:15]:
        print("  {} -> {:12s}  ns={}  pairs={}  {}".format(
            m["tsl_addr"], m["k1_addr"], m["ns"][:20], m["ns_pairs"], m["name"]))

    with open(OUT_A, "w", encoding="utf-8") as fh:
        json.dump(matches_a, fh, indent=2)
    with open(OUT_B, "w", encoding="utf-8") as fh:
        json.dump(matches_b, fh, indent=2)
    print("\nWrote {} to {}".format(len(matches_a), OUT_A))
    print("Wrote {} to {}".format(len(matches_b), OUT_B))

if __name__ == "__main__":
    main()
