---
title: "test: NCS CONSTI advanced control-flow cache integration"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-331-fix-ncs-consti-backward-jmp-scan-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI advanced control-flow cache integration (plan 332)

## Summary

Plans **328**–**331** added context-classification and backward-`JMP` guards for loops, for, and nested dead branches. Extend `StrRefReferenceCache` end-to-end indexing coverage for those patterns (mirrors plan **328** dead-return cache test).

## Requirements

- R1. `StrRefReferenceCache_DeadForBodyLocalStrRef_IsNotIndexed`
- R2. `StrRefReferenceCache_DeadWhileBodyLocalStrRef_IsNotIndexed`
- R3. `StrRefReferenceCache_DoWhileBreakLocalStrRef_IsIndexed`
- R4. `StrRefReferenceCache_NestedDeadIfReturnLocalStrRef_IsIndexed`
- R5. `StrRefReferenceCache_DeadIfBranchLocalStrRef_IsNotIndexed` (plan **328** context-only gap)
- R6. **57** NcsConsti tests pass; no scanner changes

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Test-only; scanner unchanged from plan **331**.
