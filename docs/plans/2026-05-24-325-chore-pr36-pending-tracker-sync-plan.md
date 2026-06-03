---
title: "chore: PR #36 pending tracker sync"
type: chore
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-324-feat-ncs-consti-jump-fork-strref-scan-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# chore: PR #36 pending tracker sync (plan 325)

## Summary

PR **#36** (plan **324** NCS CONSTI jump-fork forward scan for local StrRef after conditional early-return) is **open** awaiting merge to `master`. Record pending state in maintenance tracker and plan index. After merge, promote PR #36 section from pending to outcome (merge SHA, CI pass note).

## Requirements

- R1. PR **#36** open — document pending scope (`TryResolveJumpTarget` + fork scan at `JMP`/`JZ`/`JNZ`; early-return regression test; **38** NcsConsti tests).
- R2. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md` — PR #36 **pending** section; bump suggested next slices to **326+**.
- R3. Update `docs/plans/README.md` with plan **324** and **325** rows; note PR #36 open.
- R4. Mark plan **325** `status: complete` after doc edits land.

## Verification

- Doc-only; no code changes beyond this plan commit batch.

## Scope Boundaries

- Doc/tracker sync only; post-merge outcome block deferred until PR #36 merges.
- PR #33–#34 tracker promotion remains on [PR #35](https://github.com/th3w1zard1/Andastra/pull/35).
