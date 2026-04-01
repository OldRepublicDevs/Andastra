#!/usr/bin/env python3
"""Find K1-TSL pairs in kotor_function_signatures that aren't yet in cascade matches."""
import json

SIGS    = "docs/kotor_function_signatures.json"
CASCADE = "docs/k1_tsl_cascade_matches.json"
TSL_CG  = "docs/tsl_call_graph.json"
OUT     = "docs/tsl_sig_new_matches.json"


def main():
    sigs   = json.load(open(SIGS,    encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG,  encoding="utf-8"))

    funcs = sigs["functions"]
    tsl_addr_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    # Normalize to lowercase hex for comparison
    matched_tsl = {m["tsl_addr"].lower() for m in cas["matches"]}
    matched_k1  = {m["k1_addr"].lower()  for m in cas["matches"]}

    both = [f for f in funcs if f["addresses"].get("k1") and f["addresses"].get("tsl")]
    print(f"Sig entries with both K1+TSL: {len(both)}")

    new_from_sigs = []
    for f in both:
        tsl_a = f["addresses"]["tsl"].lower()
        k1_a  = f["addresses"]["k1"].lower()
        if tsl_a in matched_tsl or k1_a in matched_k1:
            continue
        tsl_int = int(tsl_a, 16)
        cur_name = tsl_addr_name.get(tsl_int, "")
        if cur_name.startswith("FUN_") or not cur_name:
            new_from_sigs.append({
                "k1_addr": k1_a,
                "tsl_addr": tsl_a,
                "name": f["name"],
                "class": f["class"],
                "method": f["method"],
                "current_tsl_name": cur_name,
                "via": "kotor_function_signatures",
            })

    print(f"New candidates (TSL still FUN_xxx, not in cascade): {len(new_from_sigs)}")
    for m in new_from_sigs[:20]:
        print(f"  {m['tsl_addr']} ({m['current_tsl_name']}) -> {m['name']}")

    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(new_from_sigs, fh, indent=2)
    print(f"\nWrote {len(new_from_sigs)} entries to {OUT}")


if __name__ == "__main__":
    main()
