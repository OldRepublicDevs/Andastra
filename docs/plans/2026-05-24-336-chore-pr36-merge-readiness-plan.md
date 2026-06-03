---
title: "chore: PR #36 merge-readiness tracker sync"
type: chore
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-335-test-ncs-consti-subroutine-dead-path-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# chore: PR #36 merge-readiness tracker sync (plan 336)

## Summary

Plans **324**–**335** complete the NCS CONSTI control-flow forward-scan arc on [PR #36](https://github.com/th3w1zard1/Andastra/pull/36): **71** NcsConsti tests with main + subroutine live/dead context and cache parity. Record merge-ready state in tracker and bump suggested next slices to **336+**.

## Requirements

- R1. PR **#36** pending section notes merge-ready status (plans **324**–**335**, **71** tests, scanner fixes through plan **334**).
- R2. Suggested next slices table bumped to **336+**; lead item: promote PR #36 outcome after merge.
- R3. `docs/plans/README.md` row for plan **336**.
- R4. PR **#36** body updated with merge-ready note.

## Verification

- Doc-only; no code changes beyond this plan commit batch.

## Scope Boundaries

- Post-merge outcome promotion deferred until PR #36 lands on `master`.
- No scanner or test changes.
