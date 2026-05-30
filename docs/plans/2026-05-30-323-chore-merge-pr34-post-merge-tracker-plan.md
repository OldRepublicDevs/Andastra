---
title: "chore: merge pr34 and post-merge tracker sync"
type: chore
status: complete
date: 2026-05-30
completed: 2026-05-30
origin: docs/plans/2026-05-29-322-feat-ncs-consti-bp-fullfile-relay-plan.md
branch: docs/post-pr34-tracker-sync
---

# chore: Merge PR #34 and post-merge tracker sync (plan 323)

## Summary

PR **#34** (plans **321**–**322** NCS CONSTI BP multi-hop + full-file cross-sub relay) and PR **#33** (plan **319** instruction-size hardening) are merged to `master`. Record outcomes in maintenance tracker. Snyk `code/snyk` quota failure is non-blocking.

## Requirements

- R1. PR **#34** merged @ `4514f2b05` — document outcome (37 NcsConsti tests, CPTOPBP relay + BP full-file cross-sub recursion).
- R2. PR **#33** merged @ `8bdf07844` — promote pending → outcome (plan **319** scoped `GetInstructionStepSizeAt`).
- R3. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md` — PR #34 and PR #33 outcome sections; suggested next **324+**.
- R4. Update `docs/plans/README.md` with plan **323** row and PR #34/#33 merge notes.
- R5. Sync plan **063** merge arc with PR #34 / PR #33 / plans **319**–**322**.
- R6. Verify: `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti` (expect **37** passed).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Doc/merge gate only; no feature code.
- Do not commit `.cursor/hooks/`.

## Follow-on (plan 324+)

| Option | Topic | Notes |
|--------|-------|-------|
| 324+ | Full CONSTI stack simulation | Plan **063** deferred backlog |
| 324+ | Module Designer, 2DA UX, OdyPatch E2E install runbook |
| 324+ | ReferenceFinder / OdyTools vertical slices |
