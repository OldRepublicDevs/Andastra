---
title: "test: NCS CONSTI control-flow cache integration"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-327-fix-ncs-consti-local-condition-resolve-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI control-flow cache integration (plan 328)

## Summary

Plans **324**–**327** added context-classification tests for early/dead/variable `if` returns. Extend coverage to `StrRefReferenceCache` end-to-end indexing and dead `if (0) { consumer }` branches.

## Requirements

- R1. `StrRefReferenceCache_DeadReturnLocalStrRef_IsNotIndexed` — `if (1) return; ActionSpeakStringByStrRef(n);` not indexed.
- R2. `GetConstiUsageContext_DeadIfBranchLocalStrRef_RemainsStackStored` — consumer only in never-taken `if (0)` block.
- R3. **44** NcsConsti tests pass; no scanner logic changes.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Test-only slice; scanner behavior unchanged from plan **327**.
