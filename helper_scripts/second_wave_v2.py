"""
Second-wave simulation v2: simulate applying ALL 143 pending renames (callee_name,
caller_name, and joint_callee_caller results), then re-run joint fingerprint matching
to find any newly unlocked pairs.
"""
import json, re
from collections import defaultdict

TSL_CG   = "docs/tsl_call_graph.json"
K1_CG    = "docs/k1_call_graph.json"
CASCADE  = "docs/k1_tsl_cascade_matches.json"
OUT      = "docs/tsl_second_wave_v2.json"

MIN_COMBINED = 3
MAX_WAVES    = 10

def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s

def build_caller_map(funcs):
    cm = defaultdict(set)
    for f in funcs:
        for c in f.get("c", []):
            cm[c].add(f["a"])
    return cm

def run_wave(tsl_funcs, k1_funcs, tsl_name, k1_name, matched_tsl, matched_k1):
    tsl_cm = build_caller_map(tsl_funcs)
    k1_cm  = build_caller_map(k1_funcs)

    k1_fp = {}
    for f in k1_funcs:
        cs = frozenset(k1_name.get(c,"") for c in f.get("c",[]) if k1_name.get(c,""))
        ps = frozenset(k1_name.get(c,"") for c in k1_cm.get(f["a"],set()) if k1_name.get(c,""))
        if len(cs|ps) >= MIN_COMBINED:
            fp = (cs, ps)
            k1_fp.setdefault(fp, []).append(f)
    unique_k1 = {fp: lst[0] for fp, lst in k1_fp.items() if len(lst)==1}

    new_matches = []
    seen_k1 = set()
    for f in tsl_funcs:
        if not f["n"].startswith("FUN_") or f["a"] in matched_tsl:
            continue
        cs = frozenset(tsl_name.get(c,"") for c in f.get("c",[])
                       if tsl_name.get(c,"") and not tsl_name.get(c,"").startswith("FUN_"))
        ps = frozenset(tsl_name.get(c,"") for c in tsl_cm.get(f["a"],set())
                       if tsl_name.get(c,"") and not tsl_name.get(c,"").startswith("FUN_"))
        if len(cs|ps) < MIN_COMBINED:
            continue
        fp = (cs, ps)
        if fp not in unique_k1:
            continue
        k1f = unique_k1[fp]
        if k1f["a"] in matched_k1 or k1f["a"] in seen_k1:
            continue
        seen_k1.add(k1f["a"])
        new_matches.append({
            "k1_addr":  hex(k1f["a"]),
            "tsl_addr": hex(f["a"]),
            "name":     (k1f.get("ns","")+"::"+k1f["n"] if k1f.get("ns") else k1f["n"]),
            "class":    k1f.get("ns",""),
            "method":   k1f["n"],
            "via":      "second_wave_v2_joint",
        })
    return new_matches

def main():
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    cas    = json.load(open(CASCADE, encoding="utf-8"))

    k1_name  = {f["a"]: f["n"] for f in k1_cg["functions"]}
    tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

    matched_tsl = {parse_hex(m["tsl_addr"]) for m in cas["matches"]}
    matched_k1  = {parse_hex(m["k1_addr"])  for m in cas["matches"]}

    # Simulate: apply all cascade entries where TSL is still FUN_xxx
    simulated_renames = 0
    for m in cas["matches"]:
        ta = parse_hex(m["tsl_addr"])
        if tsl_name.get(ta,"").startswith("FUN_"):
            tsl_name[ta] = m["name"]
            simulated_renames += 1
    print("Simulated {} renames in TSL name map".format(simulated_renames))

    all_new = []
    total_before = len(matched_tsl)
    for wave in range(1, MAX_WAVES+1):
        wm = run_wave(tsl_cg["functions"], k1_cg["functions"],
                      tsl_name, k1_name, matched_tsl, matched_k1)
        if not wm:
            print("Wave {}: 0 matches".format(wave))
            break
        print("Wave {}: {} new matches".format(wave, len(wm)))
        for x in wm[:5]:
            print("  {} -> {}  {}".format(x["tsl_addr"], x["k1_addr"], x["name"]))
        all_new.extend(wm)
        for m in wm:
            ta = parse_hex(m["tsl_addr"])
            matched_tsl.add(ta)
            matched_k1.add(parse_hex(m["k1_addr"]))
            tsl_name[ta] = m["name"]

    print("Total after simulation: {} -> {}".format(total_before, len(matched_tsl)))
    print("New matches: {}".format(len(all_new)))
    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(all_new, fh, indent=2)
    print("Wrote to " + OUT)

if __name__ == "__main__":
    main()
