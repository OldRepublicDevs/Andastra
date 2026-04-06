"""Analyze signature-based pairs: confidence distribution and sample entries."""
import json
from collections import Counter

CASCADE = "docs/k1_tsl_cascade_matches.json"
SIGS    = "docs/kotor_function_signatures.json"
TSL_CG  = "docs/tsl_call_graph.json"

cas  = json.load(open(CASCADE, encoding="utf-8"))
sigs = json.load(open(SIGS,    encoding="utf-8"))
tslcg= json.load(open(TSL_CG,  encoding="utf-8"))

tsl_name = {f["a"]: f["n"] for f in tslcg["functions"]}
existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
existing_k1  = {m["k1_addr"]  for m in cas["matches"]}

def norm(a): return hex(int(a, 16))

new_pairs = []
for f in sigs["functions"]:
    k1a  = f.get("addresses", {}).get("k1")
    tsla = f.get("addresses", {}).get("tsl")
    if not k1a or not tsla: continue
    k1h = norm(k1a); tslh = norm(tsla)
    if k1h in existing_k1 or tslh in existing_tsl: continue
    tsl_int = int(tsla, 16)
    n = tsl_name.get(tsl_int, "")
    if not n.startswith("FUN_"):
        new_pairs.append({
            "tsl_addr": tslh,
            "k1_addr":  k1h,
            "name":     f["name"],
            "class":    f.get("class",""),
            "method":   f.get("method", f["name"]),
            "tsl_name": n,
            "sig":      f.get("sig"),
            "confidence": f.get("confidence",""),
            "size_k1":  f.get("size",{}).get("k1"),
            "size_tsl": f.get("size",{}).get("tsl"),
            "via":      "signatures_name_match",
        })

conf_dist = Counter(p["confidence"] for p in new_pairs)
print("Confidence distribution of 3699 new pairs:")
for k,v in conf_dist.most_common():
    print("  {:30s}: {}".format(k or "(none)", v))

# Check: how many tsl_name matches the sig name exactly?
exact_name_match = sum(1 for p in new_pairs if p["tsl_name"] == p["name"])
partial_match    = sum(1 for p in new_pairs if p["name"] in p["tsl_name"] or p["tsl_name"] in p["name"])
print("\nTSL Ghidra name == sig name:", exact_name_match, "/", len(new_pairs))
print("TSL Ghidra name sub/superstring of sig name:", partial_match)

# Sample mismatches
mismatches = [(p["tsl_name"], p["name"]) for p in new_pairs if p["tsl_name"] != p["name"]]
print("\nSample name mismatches (first 10):")
for tsl_n, sig_n in mismatches[:10]:
    print("  TSL_Ghidra='{}' vs Sig='{}'".format(tsl_n[:50], sig_n[:50]))
