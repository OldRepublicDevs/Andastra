"""Extended algorithm runner: adds joint, triple_fp, caller_name, ordered_seq,
then re-runs the standard convergence loop.
Run from repo root: python helper_scripts/run_extended_algorithms.py
"""
from __future__ import annotations

import json
import os
import subprocess
import sys
import time

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
LOG = os.path.join(REPO, "docs", "algo_run_output.txt")


def log(msg: str) -> None:
    ts = time.strftime("%H:%M:%S")
    line = "[{}] {}".format(ts, msg)
    print(line, flush=True)
    with open(LOG, "a") as f:
        f.write(line + "\n")


def run(script: str, *extra_args: str) -> str:
    path = os.path.join(REPO, "helper_scripts", script)
    args = [sys.executable, path] + list(extra_args)
    result = subprocess.run(args, cwd=REPO, capture_output=True, text=True, timeout=600)
    out = (result.stdout + result.stderr).strip()
    short = out[-600:] if len(out) > 600 else out
    log("  {} {} => {}".format(script, " ".join(extra_args), short))
    return out


def cascade_count() -> int:
    p = os.path.join(REPO, "docs", "k1_tsl_cascade_matches.json")
    with open(p) as f:
        d = json.load(f)
    return len(d["matches"])


# Append to existing log
with open(LOG, "a") as f:
    f.write("\n")
log("=== run_extended_algorithms.py start ===")
log("Initial cascade: {}".format(cascade_count()))

# ── Phase 1: New algorithms on fresh call graph ──────────────────────────────
log("--- Phase 1: New fingerprint algorithms ---")

log("Running joint_fingerprint_match...")
run("joint_fingerprint_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_joint_fingerprint_matches.json")
after = cascade_count()
log("joint_fingerprint merged: +{} (now {})".format(after - before, after))

log("Running triple_fp_match...")
run("triple_fp_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_triple_fp_matches.json")
after = cascade_count()
log("triple_fp merged: +{} (now {})".format(after - before, after))

log("Running caller_name_match...")
run("caller_name_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_caller_name_matches.json")
after = cascade_count()
log("caller_name merged: +{} (now {})".format(after - before, after))

log("Running ordered_seq_match...")
run("ordered_seq_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_ordered_seq_matches.json")
after = cascade_count()
log("ordered_seq merged: +{} (now {})".format(after - before, after))

log("Running string_ref_match...")
run("string_ref_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_string_ref_matches.json")
after = cascade_count()
log("string_ref merged: +{} (now {})".format(after - before, after))

log("Running single_callee_cascade...")
before = cascade_count()
run("single_callee_cascade.py")
after = cascade_count()
log("single_callee_cascade merged: +{} (now {})".format(after - before, after))

log("Running local_cascade_multi (single_caller + callee+size)...")
before = cascade_count()
run("local_cascade_multi.py")
after = cascade_count()
log("local_cascade_multi merged: +{} (now {})".format(after - before, after))

log("Phase 1 done. Cascade: {}".format(cascade_count()))

# ── Phase 2: Standard convergence loop ───────────────────────────────────────
log("--- Phase 2: Standard convergence loop ---")
for rnd in range(1, 5):
    log("-- Round {} --".format(rnd))
    before = cascade_count()

    run("cascade_translated_match.py")
    run("projected_intersection_match.py")
    run("vote_callee_projection.py")
    run("vote_caller_projection.py")
    run("ns_cluster_positional_match.py")

    run("merge_ct_matches.py")
    run("merge_projected.py")

    vp = os.path.join(REPO, "docs", "tsl_vote_projection_matches.json")
    vc = os.path.join(REPO, "docs", "tsl_vote_caller_matches.json")
    if os.path.exists(vp):
        run("merge_any.py", "docs/tsl_vote_projection_matches.json")
    if os.path.exists(vc):
        run("merge_any.py", "docs/tsl_vote_caller_matches.json")

    run("merge_ns_positional.py")

    after = cascade_count()
    added = after - before
    log("Round {} done: {} -> {} (+{})".format(rnd, before, after, added))
    if added == 0:
        log("Converged.")
        break

# ── Phase 3: Second pass with new algorithms after convergence ────────────────
log("--- Phase 3: Re-run new algorithms post-convergence ---")
p1_before = cascade_count()

run("joint_fingerprint_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_joint_fingerprint_matches.json")
after = cascade_count()
log("joint_fingerprint (pass2): +{}".format(after - before))

run("triple_fp_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_triple_fp_matches.json")
after = cascade_count()
log("triple_fp (pass2): +{}".format(after - before))

run("caller_name_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_caller_name_matches.json")
after = cascade_count()
log("caller_name (pass2): +{}".format(after - before))

run("ordered_seq_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_ordered_seq_matches.json")
after = cascade_count()
log("ordered_seq (pass2): +{}".format(after - before))

run("string_ref_match.py")
before = cascade_count()
run("merge_any.py", "docs/tsl_string_ref_matches.json")
after = cascade_count()
log("string_ref (pass2): +{}".format(after - before))

p1_after = cascade_count()
log("Phase 3 added: +{}".format(p1_after - p1_before))

if p1_after > p1_before:
    log("New matches found — running final convergence round...")
    for rnd in range(1, 4):
        before = cascade_count()
        run("cascade_translated_match.py")
        run("merge_ct_matches.py")
        run("merge_projected.py")
        run("merge_any.py", "docs/tsl_vote_projection_matches.json")
        run("merge_any.py", "docs/tsl_vote_caller_matches.json")
        after = cascade_count()
        added = after - before
        log("Final round {} +{}".format(rnd, added))
        if added == 0:
            break

log("=== FINAL cascade: {} ===".format(cascade_count()))
log("DONE")
