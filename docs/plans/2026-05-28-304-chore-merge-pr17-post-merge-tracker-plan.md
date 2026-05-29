---
title: "chore: merge pr17 and post-merge tracker sync"
type: chore
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-303-feat-ncs-consti-opcode-context-disambiguation-plan.md
branch: docs/post-pr17-tracker-sync
---

# chore: Merge PR #17 and post-merge tracker sync (plan 304)

## Summary

Merge PR **#17** (plan **303** NCS CONSTI opcode-context StrRef disambiguation) to `master` and record outcome in maintenance tracker. Snyk `code/snyk` quota failure is non-blocking.

## Requirements

- R1. Merge PR **#17** via `gh pr merge` (merge commit; Snyk quota excepted).
- R2. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md` — PR #17 outcome section; suggested next slices to 305+.
- R3. Update `docs/plans/README.md` with plan **304** row and PR #17 merge note.
- R4. Sync plan **063** with PR #17 / plan **303** tracker closure note.
- R5. Verify: `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti`.

## Verification

```bash
git checkout master && git pull
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Doc/merge gate only; no feature code.
- Do not commit `.cursor/hooks/`.
