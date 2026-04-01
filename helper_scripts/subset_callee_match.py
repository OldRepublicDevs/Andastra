"""
Subset callee fingerprint matching.

TSL often adds new function calls on top of K1's existing logic (new features, TSL additions).
So TSL's named_callees is often a SUPERSET of K1's named_callees for the same method.

Strategy: 
  For each (TSL FUN_xxx A, K1 named function B):
    If K1_named_callees(B) ⊆ TSL_named_callees(A)
    AND |K1_named_callees(B)| >= MIN_SUBSET_SIZE
    AND B is not already matched
    AND no other K1 function C has K1_named_callees(C) ⊆ TSL_named_callees(A)
  Then A → B

Also tries the reverse: K1 callees ⊇ TSL callees (TSL removed some calls).
"""
import json
from collections import defaultdict

K1_CG   = "docs/k1_call_graph.json"
TSL_CG  = "docs/tsl_call_graph.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
OUT     = "docs/tsl_subset_callee_matches.json"

MIN_SUBSET = 3      # minimum named callees in the K1 set
MIN_SPECIFICITY = 2  # minimum callees that are NOT generic

GENERIC = {
    "__imp__", "_free", "_malloc", "_memcpy", "_memset", "_memmove",
    "_strlen", "_strcpy", "_strcat", "_strcmp", "_sprintf", "_printf",
    "__chkstk", "FUN_", "j__", "j___",
}


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

def is_generic(name):
    if not name or name.startswith("FUN_"):
        return True
    for g in GENERIC:
        if name.startswith(g):
            return True
    return False

def main():
    k1_cg = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg= json.load(open(TSL_CG, encoding="utf-8"))
    cas   = json.load(open(CASCADE, encoding="utf-8"))

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # Pre-compute named callee sets
    def named_callees_k1(f):
        return frozenset(
            k1_name.get(c,"") for c in f.get("c",[])
            if k1_name.get(c,"") and not is_generic(k1_name.get(c,""))
        )

    def named_callees_tsl(f):
        return frozenset(
            tsl_name.get(c,"") for c in f.get("c",[])
            if tsl_name.get(c,"") and not is_generic(tsl_name.get(c,""))
        )

    k1_named = {}
    for f in k1_cg["functions"]:
        nc = named_callees_k1(f)
        if len(nc) >= MIN_SUBSET:
            k1_named[f["a"]] = (f, nc)

    print("K1 functions with >= {} non-generic named callees: {:,}".format(
        MIN_SUBSET, len(k1_named)))

    # For each TSL FUN_xxx, find K1 functions whose callees ⊆ TSL callees
    matches = []
    seen_k1 = set()
    checked = 0

    for f in tsl_cg["functions"]:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        tsl_nc = named_callees_tsl(f)
        if len(tsl_nc) < MIN_SUBSET:
            continue
        checked += 1

        # Find all K1 funcs whose callees are subset of this TSL func's callees
        k1_candidates = [
            (addr, k1f, k1nc)
            for addr, (k1f, k1nc) in k1_named.items()
            if addr not in matched_k1 and addr not in seen_k1
            and k1nc <= tsl_nc          # K1 callees ⊆ TSL callees
            and len(k1nc) >= MIN_SUBSET
        ]

        if len(k1_candidates) != 1:
            continue

        addr, k1f, k1nc = k1_candidates[0]

        # Specificity check: ≥ MIN_SPECIFICITY of the matched callees must be very specific
        # (not just 3 common library names)
        specific = [n for n in k1nc if "::" in n or len(n) > 25]
        if len(specific) < MIN_SPECIFICITY:
            continue

        seen_k1.add(addr)
        matches.append({
            "k1_addr":  hex(addr),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "k1_callees": sorted(k1nc)[:6],
            "extra_tsl_callees": sorted(tsl_nc - k1nc)[:4],
            "via":      "subset_callee_fingerprint",
        })

    print("Checked {:,} TSL FUN_xxx; found {} matches".format(checked, len(matches)))
    for m in matches[:20]:
        print("  {} -> {:12s}  {}".format(m["tsl_addr"], m["k1_addr"], m["name"]))
        print("    k1_callees:", m["k1_callees"][:3])
        if m["extra_tsl_callees"]:
            print("    +TSL_extras:", m["extra_tsl_callees"][:2])

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(matches, fh, indent=2)
    print("Wrote {} to {}".format(len(matches), OUT))

if __name__ == "__main__":
    main()
