---
title: "chore: merge pr34 and post-merge tracker sync"
type: chore
status: complete
date: 2026-05-30
completed: 2026-05-30
applied_on_master: 2026-06-03
origin: docs/plans/2026-05-29-322-feat-ncs-consti-bp-fullfile-relay-plan.md
branch: docs/post-pr34-tracker-sync
superseded_by: docs/plans/2026-05-24-345-chore-reconcile-plan323-tracker-sync-plan.md
---

# chore: Merge PR #34 and post-merge tracker sync (plan 323)

## Summary

PR **#34** (plans **321**–**322** NCS CONSTI BP multi-hop + full-file cross-sub relay) and PR **#33** (plan **319** instruction-size hardening) are merged to `master`. Record outcomes in maintenance tracker.

**Applied on `master` via plan **345** (2026-06-03)** after [PR #35](https://github.com/th3w1zard1/Andastra/pull/35) conflicted post–PR #36 merge.

## Requirements

- R1. PR **#34** merged @ `4514f2b05` — document outcome (37 NcsConsti tests at merge time).
- R2. PR **#33** merged @ `8bdf07844` — promote pending → outcome (plan **319** scoped `GetInstructionStepSizeAt`).
- R3. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md`.
- R4. Update `docs/plans/README.md` with plan **323** row.

## Scope Boundaries

- Doc/merge gate only; no feature code.
