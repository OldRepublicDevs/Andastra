---
title: "chore: merge pr25 and post-merge tracker sync"
type: chore
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-29-311-feat-ncs-consti-cptopbp-global-strref-plan.md
branch: docs/post-pr25-tracker-sync
---

# chore: Merge PR #25 and post-merge tracker sync (plan 312)

## Summary

PR **#25** (plan **311** NCS CONSTI global StrRef `CPDOWNBP`→`CPTOPBP` cross-subroutine trace) is merged to `master`. Record outcome in maintenance tracker. Snyk `code/snyk` quota failure is non-blocking.

## Requirements

- R1. PR **#25** merged @ `08bd4a3a3` — document outcome (29 NcsConsti tests, global BP reload trace).
- R2. Update `docs/knowledgebase/90-meta/pr-merge-readiness.md` — PR #25 outcome section; suggested next slices to **313+**.
- R3. Update `docs/plans/README.md` with plan **312** row and PR #25 merge note.
- R4. Sync plan **063** with PR #25 / plan **311** tracker closure note (line 107 merge arc).
- R5. Verify: `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti` (expect **29** passed).

## Verification

```bash
git checkout master && git merge --ff-only origin/master
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Doc/merge gate only; no feature code.
- Do not commit `.cursor/hooks/`.

## Follow-on (plan 313+)

| Option | Topic | Notes |
|--------|-------|-------|
| **313 (preferred)** | Multi-hop local StrRef trace (`n→m→ACTION`) | Extends plan 309 forward scan with chained `CPDOWNSP`/`CPTOPSP` hops |
| 313+ | Instruction-aligned `CPTOPBP` scan | Optimize plan 311 byte walk; lower false-positive risk |
| 314+ | Full CONSTI stack simulation | Plan **063** deferred backlog; exotic control-flow |
