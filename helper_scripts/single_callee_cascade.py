"""
Single/small-callee wrapper cascade matching.

Match K1 named functions to TSL FUN_xxx based on having the EXACT same
translated callee set (via cascade), where all callees are already matched.

Supports callee sets of size 1..MAX_CALLEES and runs iteratively until
convergence (each wave may unlock new matches via cascade propagation).

Input:
  docs/k1_call_graph.json
  docs/tsl_call_graph.json
  docs/k1_tsl_cascade_matches.json

Output:
  docs/tsl_single_callee_matches.json  (cumulative)
  Appends to docs/k1_tsl_cascade_matches.json
"""
from __future__ import annotations

import json
import os
import re
import sys
import time
from collections import defaultdict

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
K1_CG   = os.path.join(REPO, "docs", "k1_call_graph.json")
TSL_CG  = os.path.join(REPO, "docs", "tsl_call_graph.json")
CASCADE = os.path.join(REPO, "docs", "k1_tsl_cascade_matches.json")
OUT     = os.path.join(REPO, "docs", "tsl_single_callee_matches.json")
RENAME  = os.path.join(REPO, "helper_scripts", "ghidra_batch_rename_tsl.py")
TSL_NAME_OUT = os.path.join(REPO, "docs", "tsl_call_graph.json")  # updated in place

MAX_CALLEES = 4  # match functions with up to 4 callees (all must be cascade-matched)
MAX_WAVES   = 20


def parse_hex(s):
    return int(s, 16) if isinstance(s, str) else s


def log(msg):
    ts = time.strftime("%H:%M:%S")
    print("[{}] {}".format(ts, msg), flush=True)


def build_callee_set(fn_callees_raw):
    """Return frozenset of callee addresses from various list formats."""
    result = set()
    for c in fn_callees_raw:
        result.add(c if isinstance(c, int) else c.get("a", c))
    return frozenset(result)


def run_wave(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl, max_callees):
    """One matching wave. Returns list of new matches."""
    k1_name = {f["a"]: f["n"] for f in k1_cg["functions"]}

    # K1: build exact-callee-set → list of unmatched named functions
    k1_set_to_funcs = defaultdict(list)
    for f in k1_cg["functions"]:
        if f["n"].startswith("FUN_") or f["a"] in matched_k1:
            continue
        callees_raw = f.get("c", [])
        callees = frozenset(callees_raw)
        if not callees or len(callees) > max_callees:
            continue
        # All callees must be in cascade
        if not all(c in k1_to_tsl for c in callees):
            continue
        # Translate to TSL callee set
        tsl_set = frozenset(k1_to_tsl[c] for c in callees)
        k1_set_to_funcs[tsl_set].append(f)

    # Only keep unique K1 → TSL-set mappings (1 K1 function per translated set)
    unique_k1 = {tsl_set: flist[0] for tsl_set, flist in k1_set_to_funcs.items()
                 if len(flist) == 1}

    # TSL: build exact-callee-set → list of unmatched FUN_xxx functions
    tsl_set_to_funcs = defaultdict(list)
    for fn in tsl_cg["functions"]:
        if not fn["n"].startswith("FUN_") or fn["a"] in matched_tsl:
            continue
        callees_raw = fn.get("c", [])
        callees = build_callee_set(callees_raw)
        if not callees or len(callees) > max_callees:
            continue
        tsl_set_to_funcs[callees].append(fn["a"])

    # Match: unique K1 + unique TSL for same TSL callee set
    new_matches = []
    seen_k1 = set()
    seen_tsl = set()

    for tsl_set, k1f in unique_k1.items():
        tsl_candidates = [ta for ta in tsl_set_to_funcs.get(tsl_set, [])
                          if ta not in matched_tsl]
        if len(tsl_candidates) != 1:
            continue
        ka = k1f["a"]
        ta = tsl_candidates[0]
        if ka in seen_k1 or ta in seen_tsl:
            continue
        name = k1f["n"]
        ns, meth = (name.rsplit("::", 1) if "::" in name else ("", name))
        new_matches.append({
            "k1_addr": "0x{:08X}".format(ka),
            "tsl_addr": "0x{:08X}".format(ta),
            "name": name,
            "class": ns,
            "method": meth,
            "via_k1": "callee_set_wrap|size={}".format(len(tsl_set)),
        })
        seen_k1.add(ka)
        seen_tsl.add(ta)

    return new_matches


def main():
    k1_cg  = json.load(open(K1_CG,  encoding="utf-8"))
    tsl_cg = json.load(open(TSL_CG, encoding="utf-8"))

    all_new = []
    total_added = 0

    for wave in range(1, MAX_WAVES + 1):
        cascade = json.load(open(CASCADE, encoding="utf-8"))
        k1_to_tsl = {parse_hex(m["k1_addr"]): parse_hex(m["tsl_addr"]) for m in cascade["matches"]}
        tsl_to_k1 = {parse_hex(m["tsl_addr"]): parse_hex(m["k1_addr"]) for m in cascade["matches"]}
        matched_k1  = set(k1_to_tsl.keys())
        matched_tsl = set(tsl_to_k1.keys())

        new_matches = run_wave(k1_cg, tsl_cg, k1_to_tsl, matched_k1, matched_tsl, MAX_CALLEES)
        log("Wave {}: {} new matches".format(wave, len(new_matches)))

        if not new_matches:
            log("Converged.")
            break

        # Merge into cascade
        existing_tsl = {m["tsl_addr"] for m in cascade["matches"]}
        existing_k1  = {m["k1_addr"]  for m in cascade["matches"]}
        added = 0
        for m in new_matches:
            if m["tsl_addr"] in existing_tsl or m["k1_addr"] in existing_k1:
                continue
            cascade["matches"].append(m)
            existing_tsl.add(m["tsl_addr"])
            existing_k1.add(m["k1_addr"])
            added += 1
            all_new.append(m)
        total_added += added

        with open(CASCADE, "w", encoding="utf-8") as fh:
            json.dump(cascade, fh, indent=2)

        # Update TSL call graph names so next wave benefits from new names
        updated_names = {parse_hex(m["tsl_addr"]): m["name"] for m in all_new}
        for fn in tsl_cg["functions"]:
            if fn["a"] in updated_names and fn["n"].startswith("FUN_"):
                # Update in-memory so next wave sees updated callers of renamed functions
                # The "name" stored is the K1 name — use as TSL name too
                fn["n"] = updated_names[fn["a"]]

        if added == 0:
            log("No new unique pairs after dedup — converged.")
            break

    log("Total added: {} (cascade now {})".format(total_added, len(cascade["matches"])))

    # Save all matches
    with open(OUT, "w", encoding="utf-8") as fh:
        json.dump(all_new, fh, indent=2)

    # Regenerate rename script
    cascade_final = json.load(open(CASCADE, encoding="utf-8"))
    tsl_name_map = {fn["a"]: fn["n"] for fn in tsl_cg["functions"]}
    pending = [
        (m, parse_hex(m["tsl_addr"]))
        for m in cascade_final["matches"]
        if tsl_name_map.get(parse_hex(m["tsl_addr"]), "").startswith("FUN_")
    ]
    log("Pending renames: {}".format(len(pending)))

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
    log("Wrote rename script with {} entries".format(len(pending)))


if __name__ == "__main__":
    main()
