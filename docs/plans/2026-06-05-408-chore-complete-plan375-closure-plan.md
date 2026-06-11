---
title: "chore: complete plan 375 closure and refresh next slices"
type: chore
status: complete
completed: 2026-06-05
date: 2026-06-05
origin: docs/plans/2026-05-24-375-chore-merge-pr63-tracker-sync-plan.md
branch: chore/plan-408-complete-plan375-closure
---

# chore: Complete plan 375 closure and refresh next slices (plan 408)

## Summary

Plan **375** tracker sync for [PR #63](https://github.com/th3w1zard1/Andastra/pull/63) / plan **374** landed inline on `master` (PR #63 outcome row, build-ladder **98** NcsConsti, README index) but plan frontmatter remained `active`. Close plans **374**/**375**, refresh suggested next slices to **408+** with open PR stack context, and note plan **375** completion in plan **063**.

## Requirements

- R1. Set `status: complete` on plans **374** and **375** frontmatter.
- R2. Refresh `pr-merge-readiness.md` suggested next slices to **408+** (open PRs #67–#74, deferred CONSTI stack simulation).
- R3. Note plan **375** closure in plan **063** deferred CONSTI section.
- R4. Index plan **408** in `docs/plans/README.md`.

## Verification

- Grep plans 374/375 for `status: complete`.
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti` — **98** pass.

## Scope Boundaries

- Documentation and plan metadata only.
- Does not merge or duplicate open PRs **#67**–**#74**.
