---
title: "test: NCS CONSTI advanced control-flow coverage"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-330-test-ncs-consti-while-zero-if-live-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI advanced control-flow coverage (plan 331)

## Summary

Extend the control-flow regression suite after plan **330** with do-while break, nested dead-if inside live loop, dead for-body, and nested dead-if patterns. Fix `NcsConstiScanner` only if a probe fails.

## Requirements

- R1. `GetConstiUsageContext_DoWhileBreakLocalStrRef_ReturnsStrRefConsumer` — `do { break; } while(1); ActionSpeakStringByStrRef(n);`
- R2. `GetConstiUsageContext_DeadForBodyLocalStrRef_RemainsStackStored` — `for (i=0; i<0; i++) { ActionSpeakStringByStrRef(n); }`
- R3. `GetConstiUsageContext_NestedDeadIfLocalStrRef_RemainsStackStored` — `if (0) { if (0) return; } ActionSpeakStringByStrRef(n);` dead consumer
- R4. `GetConstiUsageContext_LiveLoopNestedDeadReturnLocalStrRef_ReturnsStrRefConsumer` — `while(1) { if(0) return; } ActionSpeakStringByStrRef(n);`
- R5. All NcsConsti tests pass (baseline 48; expect 52 after R1–R4)

## Implementation Units

| Unit | Files | Notes |
|------|-------|-------|
| U1 | `tests/BioWare.Tests/NcsConstiScannerTests.cs` | Add four tests; characterization-first |
| U2 | `src/BioWare/Tools/NcsConstiScanner.cs` | Only if a probe fails |

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- No full stack simulation (plan 063 backlog).
- No PR body-only fallback unless all four probes pass without scanner changes — then mark plan complete test-only.
