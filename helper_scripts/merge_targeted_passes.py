"""
Filter and merge targeted_passes.json into cascade.
Quality filters:
  Pass 1: reject if callee name is Ghidra auto-generated junk (void_return, self_return, return_true_*)
           reject if size diff > 65%
  Pass 2: reject if ALL shared callees are generic CRT/operator names (no '::' and len<12)
           reject if size diff > 70%
"""
import json, re

CASCADE = "docs/k1_tsl_cascade_matches.json"
NEW     = "docs/tsl_targeted_passes.json"
RENAME  = "helper_scripts/ghidra_batch_rename_tsl.py"
TSL_CG  = "docs/tsl_call_graph.json"

JUNK_P1 = {"return_true_4","return_true_3","return_true_2","return_true_1",
           "void_return","self_return","return_false","return_null","nullsub"}

def size_ratio(a, b):
    if a <= 0 or b <= 0: return 0.0
    return abs(a-b)/max(a,b)

def is_all_generic(callees):
    for n in callees:
        if "::" in n or len(n) > 12:
            return False
    return True

cas  = json.load(open(CASCADE, encoding="utf-8"))
new_ = json.load(open(NEW,     encoding="utf-8"))

existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
accepted = rejected = 0

for m in new_:
    if m["tsl_addr"] in existing_tsl:
        continue
    if m["pass"] == 1:
        callee = m.get("named_callee","")
        if any(callee.startswith(j) for j in JUNK_P1):
            rejected += 1; continue
        if size_ratio(m["tsl_sz"], m["k1_sz"]) > 0.65:
            rejected += 1; continue
    else:  # pass 2
        shared = m.get("shared_callees", [])
        if is_all_generic(shared):
            rejected += 1; continue
        if size_ratio(m["tsl_sz"], m["k1_sz"]) > 0.70:
            rejected += 1; continue
    cas["matches"].append(m)
    existing_tsl.add(m["tsl_addr"])
    accepted += 1

print("Accepted: {}  Rejected: {}  (total now {})".format(accepted, rejected, len(cas["matches"])))
with open(CASCADE, "w", encoding="utf-8") as fh:
    json.dump(cas, fh, indent=2)

# Regenerate batch rename script
tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
tsl_name = {f["a"]: f["n"] for f in tsl_cg["functions"]}

def parse_hex(s):
    return int(s,16) if isinstance(s,str) else s

pending = [(m, parse_hex(m["tsl_addr"]))
           for m in cas["matches"]
           if tsl_name.get(parse_hex(m["tsl_addr"]), "").startswith("FUN_")]

print("Pending renames: {}".format(len(pending)))

lines = [
    "# Auto-generated Ghidra TSL rename script  ({} entries)".format(len(pending)),
    "from ghidra.program.model.symbol import SourceType",
    "fm = currentProgram.getFunctionManager()",
    "sm = currentProgram.getSymbolTable()",
    "renamed = 0; errors = 0",
    "",
]

for m, tsl_a in pending:
    clean = re.sub(r"[^A-Za-z0-9_]", "_", m["method"])
    ns = m.get("class","")
    lines += [
        "try:",
        "    fn = fm.getFunctionAt(toAddr(0x{:X}))".format(tsl_a),
        "    if fn:",
        "        fn.setName('{}', SourceType.USER_DEFINED)".format(clean),
        "        nsobj = sm.getNamespace('{}', currentProgram.getGlobalNamespace())".format(ns),
        "        if nsobj is None:",
        "            nsobj = currentProgram.getSymbolTable().createNameSpace(",
        "                currentProgram.getGlobalNamespace(), '{}', SourceType.USER_DEFINED)".format(ns),
        "        fn.setParentNamespace(nsobj)",
        "        renamed += 1",
        "    else: errors += 1",
        "except Exception as e:",
        "    print('ERR 0x{:X}: '+str(e)); errors += 1".format(tsl_a),
        "",
    ]
lines += ["print('renamed={} errors={}'.format(renamed, errors))"]
with open(RENAME, "w", encoding="utf-8") as fh:
    fh.write("\n".join(lines))
print("Wrote {} to {}".format(len(pending), RENAME))
