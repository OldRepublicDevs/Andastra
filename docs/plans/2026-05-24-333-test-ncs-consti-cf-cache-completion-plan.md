---
title: "test: NCS CONSTI control-flow cache completion"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-332-test-ncs-consti-advanced-cf-cache-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI control-flow cache completion (plan 333)

## Summary

Plans **324**–**332** added context-classification and cache tests for dead branches, loops, and for-bodies. Close remaining gaps: live-path cache indexing for early-return, while-break, else-branch, if-one-live, and variable-condition probes; dead-path cache exclusion for variable `if (x) return`.

## Requirements

- R1. `StrRefReferenceCache_EarlyReturnLocalStrRef_IsIndexed` — `if (0) return; ActionSpeakStringByStrRef(n);`
- R2. `StrRefReferenceCache_WhileBreakLocalStrRef_IsIndexed`
- R3. `StrRefReferenceCache_ElseBranchLocalStrRef_IsIndexed`
- R4. `StrRefReferenceCache_IfOneLiveBranchLocalStrRef_IsIndexed`
- R5. `StrRefReferenceCache_VariableConditionZeroReturnLocalStrRef_IsIndexed`
- R6. `StrRefReferenceCache_VariableConditionOneReturnLocalStrRef_IsNotIndexed`
- R7. **63** NcsConsti tests pass; no scanner changes

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Test-only; completes cache parity for plans **324**–**330** live/dead context probes.
