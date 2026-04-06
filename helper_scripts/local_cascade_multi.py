"""
Comprehensive local cascade: iterates until convergence using:
1. Exact callee-set translation (single_callee_cascade logic)
2. Single-caller uniqueness (single_caller_match logic)
3. Callee-set + size tie-breaking

For each wave:
  a. Run callee-set (all-matched callees translated)
  b. Run single-caller
  c. Run callee+size
  d. Update TSL call graph names
  until no new matches in a wave

Also runs joint_fingerprint, triple_fp, caller_name, ordered_seq, string_ref
at the end of each wave to propagate.
"""
from __future__ import annotations

import json
import os
import re
import subprocess
import sys
import time
from collections import defaultdict

REPO    = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
K1_CG   = os.path.join(REPO, "docs", "k1_call_graph.json")
TSL_CG  = os.path.join(REPO, "docs", "tsl_call_graph.json")
CASCADE = os.path.join(REPO, "docs", "k1_tsl_cascade_matches.json")
RENAME  = os.path.join(REPO, "helper_scripts", "ghidra_batch_rename_tsl.py")
K1_BYTES  = os.path.join(REPO, "docs", "k1_function_bytes.json")
TSL_BYTES = os.path.join(REPO, "docs", "tsl_function_bytes.json")

MAX_MAIN_CALLEES = 6
MAX_WAVES = 30
LOG = os.path.join(REPO, "docs", "local_cascade_output.txt")


def log(msg):
    ts = time.strftime("%H:%M:%S")
    line = "[{}] {}".format(ts, msg)
    print(line, flush=True)
    with open(LOG, "a", encoding="utf-8") as f:
        f.write(line + "\n")


def px(s):
    return int(s, 16) if isinstance(s, str) else s


def load_sizes(path):
    data = json.load(open(path, encoding="utf-8"))
    funcs = data.get("functions", data) if isinstance(data, dict) else data
    return {px(e["a"]): int(e.get("sz", 0)) for e in funcs if isinstance(e, dict)}


def build_callee_set_fn(fn_callees_raw):
    result = set()
    for c in fn_callees_raw:
        result.add(c if isinstance(c, int) else c.get("a", c))
    return frozenset(result)


def load_state():
    k1_cg = json.load(open(K1_CG, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    cas = json.load(open(CASCADE, encoding="utf-8"))

    k1_to_tsl = {px(m["k1_addr"]): px(m["tsl_addr"]) for m in cas["matches"]}
    tsl_to_k1 = {px(m["tsl_addr"]): px(m["k1_addr"]) for m in cas["matches"]}
    matched_k1 = set(k1_to_tsl.keys())
    matched_tsl = set(tsl_to_k1.keys())

    return k1_cg, tsl_cg, cas, k1_to_tsl, matched_k1, matched_tsl


def wave_callee_set(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl, max_callees):
    """Exact callee-set translation matching."""
    k1_set_to_funcs = defaultdict(list)
    for f in k1_cg["functions"]:
        if f["n"].startswith("FUN_") or f["a"] in matched_k1:
            continue
        callees = frozenset(f.get("c", []))
        if not callees or len(callees) > max_callees:
            continue
        if not all(c in k1_to_tsl for c in callees):
            continue
        tsl_set = frozenset(k1_to_tsl[c] for c in callees)
        k1_set_to_funcs[tsl_set].append(f)

    unique_k1 = {s: v[0] for s, v in k1_set_to_funcs.items() if len(v) == 1}

    tsl_set_to_funcs = defaultdict(list)
    for fn in tsl_cg["functions"]:
        if not fn["n"].startswith("FUN_") or fn["a"] in matched_tsl:
            continue
        callees = build_callee_set_fn(fn.get("callees", []))
        if not callees or len(callees) > max_callees:
            continue
        tsl_set_to_funcs[callees].append(fn["a"])

    new_matches = []
    seen_k1 = set(); seen_tsl = set()
    for tsl_set, k1f in unique_k1.items():
        tsl_cands = [ta for ta in tsl_set_to_funcs.get(tsl_set, []) if ta not in matched_tsl]
        if len(tsl_cands) != 1:
            continue
        ka, ta = k1f["a"], tsl_cands[0]
        if ka in seen_k1 or ta in seen_tsl:
            continue
        name = k1f["n"]
        ns, meth = (name.rsplit("::", 1) if "::" in name else ("", name))
        new_matches.append({"k1_addr": "0x{:08X}".format(ka), "tsl_addr": "0x{:08X}".format(ta),
                             "name": name, "class": ns, "method": meth,
                             "via_k1": "callee_set|csz={}".format(len(tsl_set))})
        seen_k1.add(ka); seen_tsl.add(ta)
    return new_matches


def wave_single_caller(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl):
    """Single-caller uniqueness matching."""
    k1_name = {f["a"]: f["n"] for f in k1_cg["functions"]}

    k1_caller_of = defaultdict(set)
    for f in k1_cg["functions"]:
        for c in f.get("c", []):
            k1_caller_of[c].add(f["a"])

    tsl_caller_of = defaultdict(set)
    for fn in tsl_cg["functions"]:
        for c in fn.get("callees", []):
            ca = c if isinstance(c, int) else c["a"]
            tsl_caller_of[ca].add(fn["a"])

    k1_to_tsl_rev = {v: k for k, v in k1_to_tsl.items()}
    new_matches = []
    seen_k1 = set(); seen_tsl = set()
    k1_match_count = defaultdict(int); tsl_match_count = defaultdict(int)

    # First pass: collect all candidate matches and count
    candidates = []
    for k1_b, tsl_b in k1_to_tsl.items():
        k1_unc = [ca for ca in k1_caller_of.get(k1_b, set())
                  if ca not in matched_k1 and not k1_name.get(ca, "").startswith("FUN_")]
        tsl_unc = [ca for ca in tsl_caller_of.get(tsl_b, set())
                   if ca not in matched_tsl]
        if len(k1_unc) == 1 and len(tsl_unc) == 1:
            ka, ta = k1_unc[0], tsl_unc[0]
            candidates.append((ka, ta, k1_name.get(ka, "")))
            k1_match_count[ka] += 1
            tsl_match_count[ta] += 1

    for ka, ta, name in candidates:
        if k1_match_count[ka] != 1 or tsl_match_count[ta] != 1:
            continue  # Conflict
        if not name or name.startswith("FUN_"):
            continue
        if ka in seen_k1 or ta in seen_tsl:
            continue
        ns, meth = (name.rsplit("::", 1) if "::" in name else ("", name))
        new_matches.append({"k1_addr": "0x{:08X}".format(ka), "tsl_addr": "0x{:08X}".format(ta),
                             "name": name, "class": ns, "method": meth,
                             "via_k1": "single_caller_match"})
        seen_k1.add(ka); seen_tsl.add(ta)
    return new_matches


def wave_callee_size(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl, k1_sizes, tsl_sizes, max_callees):
    """Callee-set translation + size tie-breaking for non-unique sets."""
    k1_set_to_funcs = defaultdict(list)
    for f in k1_cg["functions"]:
        if f["n"].startswith("FUN_") or f["a"] in matched_k1:
            continue
        callees = frozenset(f.get("c", []))
        if not callees or len(callees) > max_callees:
            continue
        if not all(c in k1_to_tsl for c in callees):
            continue
        tsl_set = frozenset(k1_to_tsl[c] for c in callees)
        k1_set_to_funcs[tsl_set].append((f, k1_sizes.get(f["a"], 0)))

    # Skip unique sets (already handled by wave_callee_set)
    non_unique_k1 = {s: v for s, v in k1_set_to_funcs.items() if len(v) > 1}

    tsl_set_to_funcs = defaultdict(list)
    for fn in tsl_cg["functions"]:
        if not fn["n"].startswith("FUN_") or fn["a"] in matched_tsl:
            continue
        callees = build_callee_set_fn(fn.get("callees", []))
        if not callees or len(callees) > max_callees:
            continue
        tsl_set_to_funcs[callees].append((fn["a"], tsl_sizes.get(fn["a"], 0)))

    new_matches = []
    seen_k1 = set(); seen_tsl = set()

    for tsl_set, k1_group in non_unique_k1.items():
        tsl_group = [(ta, tsz) for ta, tsz in tsl_set_to_funcs.get(tsl_set, [])
                     if ta not in matched_tsl]
        if not tsl_group:
            continue
        k1_size_map = defaultdict(list)
        for kf, ksz in k1_group:
            k1_size_map[ksz].append(kf)
        for ta, tsz in tsl_group:
            if ta in seen_tsl:
                continue
            k1_cands = [kf for kf in k1_size_map.get(tsz, []) if kf["a"] not in seen_k1]
            if len(k1_cands) != 1:
                continue
            tsl_same_size = [x for x in tsl_group if x[1] == tsz and x[0] not in seen_tsl]
            if len(tsl_same_size) != 1:
                continue
            ka = k1_cands[0]["a"]
            if ka in seen_k1:
                continue
            name = k1_cands[0]["n"]
            ns, meth = (name.rsplit("::", 1) if "::" in name else ("", name))
            new_matches.append({"k1_addr": "0x{:08X}".format(ka), "tsl_addr": "0x{:08X}".format(ta),
                                 "name": name, "class": ns, "method": meth,
                                 "via_k1": "callee_set+size|csz={}|sz={}".format(len(tsl_set), tsz)})
            seen_k1.add(ka); seen_tsl.add(ta)
    return new_matches


def merge_new(cas, new_matches):
    existing_tsl = {m["tsl_addr"] for m in cas["matches"]}
    existing_k1  = {m["k1_addr"]  for m in cas["matches"]}
    added = 0
    for m in new_matches:
        if m["tsl_addr"] in existing_tsl or m["k1_addr"] in existing_k1:
            continue
        cas["matches"].append(m)
        existing_tsl.add(m["tsl_addr"])
        existing_k1.add(m["k1_addr"])
        added += 1
    return added


def update_tsl_names_in_memory(tsl_cg, cas, all_new):
    """Update TSL function names in the loaded call graph."""
    updated_names = {}
    for m in cas["matches"]:
        ta = px(m["tsl_addr"])
        cls = m.get("class", "") or ""
        meth = m.get("method", m.get("name", ""))
        full = (cls + "::" + meth) if cls else meth
        updated_names[ta] = full
    updated = 0
    for fn in tsl_cg["functions"]:
        if fn["a"] in updated_names and fn["n"].startswith("FUN_"):
            fn["n"] = updated_names[fn["a"]]
            updated += 1
    return updated


def run_helper_script(script, *extra_args):
    path = os.path.join(REPO, "helper_scripts", script)
    args = [sys.executable, path] + list(extra_args)
    result = subprocess.run(args, cwd=REPO, capture_output=True, text=True, timeout=300)
    return (result.stdout + result.stderr).strip()


def run_standard_algorithms():
    """Run the 'new algorithm' phase of extended algorithms."""
    out = run_helper_script("joint_fingerprint_match.py")
    r = run_helper_script("merge_any.py", "docs/tsl_joint_fingerprint_matches.json")
    log("  joint_fp: " + r[-100:])
    out = run_helper_script("caller_name_match.py")
    r = run_helper_script("merge_any.py", "docs/tsl_caller_name_matches.json")
    log("  caller_name: " + r[-100:])
    out = run_helper_script("ordered_seq_match.py")
    r = run_helper_script("merge_any.py", "docs/tsl_ordered_seq_matches.json")
    log("  ordered_seq: " + r[-100:])
    out = run_helper_script("string_ref_match.py")
    r = run_helper_script("merge_any.py", "docs/tsl_string_ref_matches.json")
    log("  string_ref: " + r[-100:])


def write_rename_script(cas, tsl_cg):
    tsl_name_map = {fn["a"]: fn["n"] for fn in tsl_cg["functions"]}
    pending = [(m, px(m["tsl_addr"])) for m in cas["matches"]
               if tsl_name_map.get(px(m["tsl_addr"]), "").startswith("FUN_")]
    lines = [
        "# Rename ({})".format(len(pending)),
        "from ghidra.program.model.symbol import SourceType",
        "fm = currentProgram.getFunctionManager()",
        "sm = currentProgram.getSymbolTable()",
        "renamed = 0; errors = 0",
        "",
    ]
    for m, ta in pending:
        clean = re.sub(r"[^A-Za-z0-9_]", "_", m["method"])
        ns = m.get("class", "") or ""
        if ns.strip():
            lines += [
                "try:",
                "    fn = fm.getFunctionAt(toAddr(0x{:X}))".format(ta),
                "    if fn:",
                "        fn.setName('{}', SourceType.USER_DEFINED)".format(clean),
                "        nsobj = sm.getNamespace('{}', currentProgram.getGlobalNamespace())".format(ns),
                "        if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '{}', SourceType.USER_DEFINED)".format(ns),
                "        fn.setParentNamespace(nsobj); renamed += 1",
                "    else: errors += 1",
                "except Exception as e: print('ERR:'+str(e)); errors += 1",
                "",
            ]
        else:
            lines += [
                "try:",
                "    fn = fm.getFunctionAt(toAddr(0x{:X}))".format(ta),
                "    if fn:",
                "        fn.setName('{}', SourceType.USER_DEFINED)".format(clean),
                "        renamed += 1",
                "    else: errors += 1",
                "except Exception as e: print('ERR:'+str(e)); errors += 1",
                "",
            ]
    lines.append("print('renamed:{} errors:{}'.format(renamed, errors))")
    with open(RENAME, "w", encoding="utf-8") as fh:
        fh.write("\n".join(lines))
    return len(pending)


def main():
    k1_sizes = load_sizes(K1_BYTES)
    tsl_sizes = load_sizes(TSL_BYTES)

    with open(LOG, "a", encoding="utf-8") as f:
        f.write("\n")
    log("=== local_cascade_multi.py start ===")

    for wave in range(1, MAX_WAVES + 1):
        # Reload fresh state
        k1_cg, tsl_cg, cas, k1_to_tsl, matched_k1, matched_tsl = load_state()
        log("Wave {} — cascade: {}".format(wave, len(cas["matches"])))

        wave_added = 0

        # 1. Callee-set
        new = wave_callee_set(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl, MAX_MAIN_CALLEES)
        added = merge_new(cas, new)
        log("  callee_set: +{}".format(added))
        wave_added += added
        if added:
            k1_to_tsl = {px(m["k1_addr"]): px(m["tsl_addr"]) for m in cas["matches"]}
            matched_k1 = set(k1_to_tsl.keys())
            matched_tsl = {px(m["tsl_addr"]) for m in cas["matches"]}

        # 2. Single caller
        new = wave_single_caller(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl)
        added = merge_new(cas, new)
        log("  single_caller: +{}".format(added))
        wave_added += added
        if added:
            k1_to_tsl = {px(m["k1_addr"]): px(m["tsl_addr"]) for m in cas["matches"]}
            matched_k1 = set(k1_to_tsl.keys())
            matched_tsl = {px(m["tsl_addr"]) for m in cas["matches"]}

        # 3. Callee + size
        new = wave_callee_size(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl,
                               k1_sizes, tsl_sizes, MAX_MAIN_CALLEES)
        added = merge_new(cas, new)
        log("  callee+size: +{}".format(added))
        wave_added += added

        # Save cascade for this wave
        with open(CASCADE, "w", encoding="utf-8") as fh:
            json.dump(cas, fh, indent=2)

        # Update TSL names for next wave
        update_tsl_names_in_memory(tsl_cg, cas, [])
        with open(TSL_CG, "w", encoding="utf-8") as fh:
            json.dump(tsl_cg, fh)

        # Run standard algorithms (they re-read from disk)
        log("  Running standard algorithms...")
        run_standard_algorithms()

        # Re-read cascade after standard algorithms (they may have added more)
        cas = json.load(open(CASCADE, encoding="utf-8"))

        # Also run callee cascade convergence
        out = run_helper_script("single_callee_cascade.py")
        log("  single_callee_cascade: " + out.replace("\n", " | ")[-200:])
        cas = json.load(open(CASCADE, encoding="utf-8"))

        fun_xxx = sum(1 for fn in tsl_cg["functions"] if fn["n"].startswith("FUN_"))
        log("  FUN_xxx remaining (approx): {}".format(fun_xxx))
        log("  Cascade now: {}".format(len(cas["matches"])))

        if wave_added == 0:
            log("Converged at wave {}.".format(wave))
            break

    cas = json.load(open(CASCADE, encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))
    pending = write_rename_script(cas, tsl_cg)
    log("=== FINAL cascade: {} === Pending renames: {}".format(len(cas["matches"]), pending))


if __name__ == "__main__":
    main()
