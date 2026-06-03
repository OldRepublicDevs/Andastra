---
title: "test: NCS CONSTI subroutine dead-path probes"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-334-test-ncs-consti-subroutine-cf-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI subroutine dead-path probes (plan 335)

## Summary

Plan **334** added live-path subroutine probes and infinite-loop cutoff in `main`. Mirror dead-path coverage for subroutine-scoped locals: dead early return and unreachable post-infinite-loop consumers inside `sub1()`.

## Requirements

- R1. `GetConstiUsageContext_SubroutineDeadReturnLocalStrRef_RemainsStackStored` — `if (1) return;` in sub
- R2. `StrRefReferenceCache_SubroutineDeadReturnLocalStrRef_IsNotIndexed`
- R3. `GetConstiUsageContext_SubroutineInfiniteLoopLocalStrRef_RemainsStackStored` — `while (1) { if (0) return; }` then dead consumer in sub
- R4. `StrRefReferenceCache_SubroutineInfiniteLoopLocalStrRef_IsNotIndexed`
- R5. **71** NcsConsti tests pass; no scanner changes expected

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Test-only; subroutine scope parity for dead paths already handled in main.
