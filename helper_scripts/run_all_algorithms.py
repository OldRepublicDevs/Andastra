"""Master runner: cascade-translated → projected → vote → ns-cluster → merge (3 rounds).
Writes progress to docs/algo_run_output.txt as it goes.
"""
import sys, os, json, subprocess, time

REPO = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
LOG  = os.path.join(REPO, 'docs', 'algo_run_output.txt')

def log(msg):
    ts = time.strftime('%H:%M:%S')
    line = '[{}] {}'.format(ts, msg)
    print(line, flush=True)
    with open(LOG, 'a') as f:
        f.write(line + '\n')

def run(script, *extra_args):
    path = os.path.join(REPO, 'helper_scripts', script)
    args = [sys.executable, path] + list(extra_args)
    result = subprocess.run(args, cwd=REPO, capture_output=True, text=True, timeout=300)
    out = (result.stdout + result.stderr).strip()
    log('  ' + script + (' ' + ' '.join(extra_args) if extra_args else '') + ' => ' + out[-400:] if len(out) > 400 else out)
    return out

def cascade_count():
    p = os.path.join(REPO, 'docs', 'k1_tsl_cascade_matches.json')
    with open(p) as f:
        d = json.load(f)
    return len(d['matches'])

# Clear old log
open(LOG, 'w').close()
log('=== run_all_algorithms.py start ===')
log('Initial cascade: {}'.format(cascade_count()))

for rnd in range(1, 4):
    log('--- Round {} ---'.format(rnd))
    before = cascade_count()

    run('cascade_translated_match.py')
    run('projected_intersection_match.py')
    run('vote_callee_projection.py')
    run('vote_caller_projection.py')
    run('ns_cluster_positional_match.py')

    run('merge_ct_matches.py')
    run('merge_projected.py')

    # merge vote results if they exist
    vp = os.path.join(REPO, 'docs', 'tsl_vote_projection_matches.json')
    vc = os.path.join(REPO, 'docs', 'tsl_vote_caller_matches.json')
    if os.path.exists(vp):
        run('merge_any.py', 'docs/tsl_vote_projection_matches.json')
    if os.path.exists(vc):
        run('merge_any.py', 'docs/tsl_vote_caller_matches.json')

    run('merge_ns_positional.py')

    after = cascade_count()
    added = after - before
    log('Round {} done: {} -> {} (+{})'.format(rnd, before, after, added))
    if added == 0:
        log('Converged, stopping early.')
        break

log('=== FINAL cascade count: {} ==='.format(cascade_count()))
log('DONE')
