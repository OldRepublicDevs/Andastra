---
title: "chore: PR #33 pending tracker sync"
type: chore
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-29-319-refactor-ncs-instruction-size-hardening-plan.md
branch: feat/plan-319-ncs-instruction-size-hardening
---

# chore: PR #33 pending tracker sync (plan 320)

## Summary

PR **#33** (plan **319** NCS CONSTI `GetInstructionSizeAt` walk hardening) is **open** awaiting merge to `master`. Record pending state in maintenance tracker and plan index. After merge, promote PR #33 section from pending to outcome (merge SHA, CI pass note). Snyk `code/snyk` quota failure is non-blocking.

## Requirements

- R1. PR **#33** open @ [feat/plan-319-ncs-instruction-size-hardening](https://github.com/th3w1zard1/Andastra/pull/33) — document pending scope (`GetInstructionStepSizeAt` for BP walks; `GetInstructionSizeAt` returns 0 for unknown opcodes; **33** NcsConsti tests).
- R2. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md` — PR #33 **pending** section; bump suggested next slices to **321+** (post-merge closure for plan 320).
- R3. Update `docs/plans/README.md` with plan **320** row and PR #33 pending note.
- R4. Sync plan **063** line 107 merge arc with PR #33 pending + plan **320** tracker note.
- R5. Mark plan **320** `status: complete` after doc edits land; post-merge promotion to outcome is follow-on when PR #33 merges.

## Verification

- Doc-only; no code changes.
- Optional: `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti` on PR branch (expect **33** passed) — already validated on PR #33.

## Scope Boundaries

- Doc/tracker sync only; no feature code.
- Do not commit `.cursor/hooks/`.
- Full post-merge outcome block (merge SHA) deferred until PR #33 merges.

## Follow-on (plan 321+)

| Option | Topic | Notes |
|--------|-------|-------|
| **321+** | Promote PR #33 pending → outcome in tracker | After merge |
| 321+ | Full CONSTI stack simulation | Plan **063** deferred backlog |
| 321+ | Module Designer, 2DA UX, OdyPatch E2E install runbook |
