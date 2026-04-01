import json, re

CASCADE = "docs/k1_tsl_cascade_matches.json"
SIGS    = "docs/kotor_function_signatures.json"
TSL_CG  = "docs/tsl_call_graph.json"
OUT     = "docs/tsl_signature_new_pairs.json"

cas  = json.load(open(CASCADE, encoding="utf-8"))
sigs = json.load(open(SIGS,    encoding="utf-8"))
tslcg= json.load(open(TSL_CG,  encoding="utf-8"))

tsl_name     = {f["a"]: f["n"] for f in tslcg["functions"]}
existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
existing_k1  = {m["k1_addr"]  for m in cas["matches"]}

def norm_addr(a): return hex(int(a, 16))

new_pairs = []
skipped_name_mismatch = 0

for f in sigs["functions"]:
    k1a  = f.get("addresses", {}).get("k1")
    tsla = f.get("addresses", {}).get("tsl")
    if not k1a or not tsla: continue
    k1h = norm_addr(k1a); tslh = norm_addr(tsla)
    if k1h in existing_k1 or tslh in existing_tsl: continue
    tsl_int = int(tsla, 16)
    ghidra_name = tsl_name.get(tsl_int, "")
    sig_name = f["name"]
    # Keep only entries where Ghidra name and sig name are related
    # (one is substring of the other, or names share the method part)
    method_part = sig_name.split("::")[-1] if "::" in sig_name else sig_name
    if (sig_name not in ghidra_name and ghidra_name not in sig_name
            and method_part not in ghidra_name):
        skipped_name_mismatch += 1
        continue
    new_pairs.append({
        "tsl_addr": tslh, "k1_addr": k1h,
        "name":     sig_name,
        "class":    f.get("class",""),
        "method":   f.get("method", sig_name),
        "via":      "signatures_name_match",
        "tsl_ghidra_name": ghidra_name,
    })

print("Total new pairs: {}  skipped name mismatch: {}".format(len(new_pairs), skipped_name_mismatch))
with open(OUT, "w", encoding="utf-8") as fh:
    json.dump(new_pairs, fh, indent=2)
print("Wrote {} to {}".format(len(new_pairs), OUT))
