---
title: "chore: multi-day open PR merge and Holocron integration"
type: chore
status: active
date: 2026-06-11
origin: user request — investigate open PRs, merge stack, continue PyKotor/Holocron port
branch: work/day1-stack-simulation-land
---

# chore: multi-day open PR merge and Holocron integration (plan 465)

## Executive summary

**28 open PRs** remain. The NCS stack-simulation arc (**#99–#134**) was merged *within its own branch stack* but **never landed on `master`**. `master` was at **#86** with **103** NcsConsti tests; stack tip **#464** targets **163** tests.

Duplicate master-targeting PRs (**#104–#122**) conflict because they repeat slices already in the landed stack.

Holocron plan **063** core units (U1–U7) are **complete** on `master`. Remaining Holocron work is deferred: Module Designer 3D, Lip Syncer, PLT parser, field-value editor wiring (**#78–#85**), KotorDiff installation search stub.

---

## Day 1 — Land stack simulation on `master` (2026-06-11)

| Step | Action | Done |
|------|--------|------|
| D1.1 | Branch `work/day1-stack-simulation-land` from `origin/master` | ✅ |
| D1.2 | Merge stack-simulation arc (plans **420**–**464**) onto `master` | ✅ `3b060001a` |
| D1.3 | Resolve conflicts; keep master's field-value/KotorDiff/LIP tests | ✅ |
| D1.4 | Verify **165** NcsConsti tests pass (163 plan-463 + 2 master four-hop) | ✅ local |
| D1.5 | Open PR → `master`; merge when CI green | 🔄 [PR #135](https://github.com/th3w1zard1/Andastra/pull/135) CI pending |
| D1.6 | Close superseded open PRs **#104–#122**, **#89–#91** as duplicate | ⏳ after #135 merge |

### Day 1 landed (2026-06-11)

- Integration merge commit `3b060001a` — `TryFindStrRefConsumerViaArithmeticThenStackStore`, `MaxNestedJsrRelayDepth = 6`, **163** NcsConsti tests.
- Preserved master's post-#86 tests (arithmetic StrRef relay, four-hop mixed CONST relay, field-value arc tests).
- [PR #135](https://github.com/th3w1zard1/Andastra/pull/135) opened to `master`.

### Day 1 next steps

1. Merge **#135** when all required CI checks green.
2. Close **#104–#122**, **#89–#91** with comment referencing **#135**.
3. Tracker sync v21 on post-D1 `master`.

---

## Day 2 — Relay arc + stack base cleanup

| Step | Action |
|------|--------|
| D2.1 | Rebase **#87–#88** (six-hop relay) onto post-D1 `master`; merge if not already included |
| D2.2 | Rebase **#80** (five-hop mixed relay) if not in D1 merge |
| D2.3 | Close **#76** tracker prep or rebase onto `master` |
| D2.4 | Tracker sync v21 on `master` through landed stack |

**Blockers for Day 2:** D1 PR **#135** must merge first; verify whether six-hop relay tests (**#87**/**#88**) are already covered by the **163**-test integration (likely partial — `MaxNestedJsrRelayDepth = 6` landed but dedicated relay PRs may add more).

---

## Day 3 — Field-value arc (**#78–#85**)

| Step | Action |
|------|--------|
| D3.1 | Rebase **#78** UTC wiring onto `master` (FieldValueReferenceHelper exists via **#72**) |
| D3.2 | Stack **#81–#85** sequentially; merge each after tests |
| D3.3 | Update Step 3d ladder + `pr-merge-readiness.md` |

---

## Day 4–5 — Holocron continuation (post-merge)

Per plan **063** deferred items, priority order:

1. **Field-value find-refs** — complete D3 wiring (Holocron `setup_file_context_menu` parity)
2. **KotorDiff installation ref search** — replace `TODO: STUB` in `ReferenceAnalyzers.cs` (**#71** landed partially)
3. **2DA spreadsheet UX** — `docs/twoda_editor_ux_and_feature_completion.md`
4. **Module Designer 3D / Lip Syncer / PLT** — separate plans; out of scope for week 1

---

## Open PR inventory (2026-06-11)

### Superseded by stack tip (close after D1)

#104–#122, #89–#91 — duplicate master-based stack slices

### Active after D1

#76, #78, #80–#85, #87–#88

### Integration PR (Day 1)

[#135](https://github.com/th3w1zard1/Andastra/pull/135) — land stack-simulation arc onto `master`

### Already merged to `master` (2026-06-11)

#67–#75, #77, #86, #99–#134 (in-stack only for #99–#134 until D1 completes)
