---
title: "test: NCS CONSTI subroutine and edge-case cache probes"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-333-test-ncs-consti-cf-cache-completion-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI subroutine and edge-case cache probes (plan 334)

## Summary

Plans **324**–**333** completed main-scoped control-flow context + cache parity. Probe subroutine-scoped early-return classification and remaining edge-case cache paths (`while (0) { return; }`, subroutine dead-return).

## Requirements

- R1. `GetConstiUsageContext_SubroutineEarlyReturnLocalStrRef_ReturnsStrRefConsumer` — local CONSTI in `sub1()` with `if (0) return;` then consumer
- R2. `StrRefReferenceCache_SubroutineEarlyReturnLocalStrRef_IsIndexed`
- R3. `StrRefReferenceCache_WhileZeroReturnLocalStrRef_IsIndexed` — `while (0) { return; }` then live consumer
- R4. `StrRefReferenceCache_WhileOneDeadIfReturnLocalStrRef_IsNotIndexed` — unreachable post-loop consumer must not index
- R5. `GetConstiUsageContext_WhileOneDeadIfReturnLocalStrRef_RemainsStackStored` — context matches cache (scanner fix: infinite loop without break stops linear scan)
- R6. Scanner: `HasForwardLoopExitJump` / `IsForwardLoopBreakJump` — distinguish `break` from `return` forward jumps inside loop body
- R7. **67** NcsConsti tests pass

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

| Unit | Files | Notes |
|------|-------|-------|
| U1 | `tests/BioWare.Tests/NcsConstiScannerTests.cs` | Four tests + update while-one-dead-if context expectation |
| U2 | `src/BioWare/Tools/NcsConstiScanner.cs` | Infinite-loop linear scan cutoff (R4–R6) |

## Scope Boundaries

- Subroutine + edge-case probes; no full CFG simulation.
