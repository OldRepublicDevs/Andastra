---
title: "chore: resolve pr 3 merge conflicts with master"
type: chore
status: completed
date: 2026-05-24
origin: gh pr view mergeable CONFLICTING; master squash f48cdaad1 vs branch plans 049-053
---

# chore: resolve PR #3 merge conflicts with master

## Summary

PR #3 is **CONFLICTING** with `master` after PR #2 squash merge (`f48cdaad1`). Merge `origin/master` and resolve add/add KB conflicts by keeping branch content (strict superset: plans 049–053 atop 001–048).

---

## Requirements

- R1. Merge `origin/master` into `docs/feat-knowledgebase-initial`.
- R2. Resolve ~20 add/add conflicts — prefer branch (`--ours`) for KB docs and plans index.
- R3. Preserve `nuget-pack-smoke` + CLI help CI in `.github/workflows/ci.yml` (plans 051/053).
- R4. Preserve `Andastra.Utility.csproj` path in topology (plan 049).
- R5. Update `pr-merge-readiness.md` with conflict-resolution note and merge-ready status.
- R6. Drift remediation **#45**; plans index **054**.

---

## Scope Boundaries

- No new features; conflict resolution only.
- OdyPatch E2E install validation remains deferred.

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| `git merge origin/master` | Completes with resolved conflicts |
| `gh pr view --json mergeable` | `MERGEABLE` after push |

---

## Repo Implications

- Unblocks PR #3 merge to master.
- Branch history retains individual commits for plans 049–053.
