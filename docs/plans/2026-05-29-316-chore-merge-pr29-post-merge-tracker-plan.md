---
title: "chore: merge pr29 and post-merge tracker sync"
type: chore
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-29-315-feat-ncs-consti-cptopbp-aligned-scan-plan.md
branch: docs/post-pr29-tracker-sync
---

# chore: Merge PR #29 and post-merge tracker sync (plan 316)

## Summary

PR **#29** (plan **315** NCS CONSTI instruction-aligned CPTOPBP scan) is merged to `master`. Record outcome in maintenance tracker. Snyk `code/snyk` quota failure is non-blocking.

## Requirements

- R1. PR **#29** merged @ `58a2697fe` — document outcome (31 NcsConsti tests, instruction-aligned BP reload walk).
- R2. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md` — PR #29 outcome section; suggested next slices to **317+**.
- R3. Update `docs/plans/README.md` with plan **316** row and PR #29 merge note.
- R4. Sync plan **063** with PR #29 / plan **315** tracker closure note (line 107 merge arc).
- R5. Verify: `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti` (expect **31** passed).

## Verification

```bash
git checkout master && git merge --ff-only origin/master
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Doc/merge gate only; no feature code.
- Do not commit `.cursor/hooks/`.

## Follow-on (plan 317+)

| Option | Topic | Notes |
|--------|-------|-------|
| **317+** | Full CONSTI stack simulation | Plan **063** deferred backlog |
| 317+ | Module Designer, 2DA UX, OdyPatch E2E install runbook |
| 317+ | ReferenceFinder / OdyTools vertical slices per plan **063** |
