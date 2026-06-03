---
title: "fix: NCS CONSTI while-break linear scan continuation"
type: fix
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-328-test-ncs-consti-control-flow-cache-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# fix: NCS CONSTI while-break linear scan continuation (plan 329)

## Summary

Plan **327** stopped linear scan after every `JZ`/`JNZ`. That correctly suppressed dead consumers after `if (1) return` (fall-through `MOVSP -4`) but also blocked `while (1) { break; } ActionSpeakStringByStrRef(n);` where fall-through uses `MOVSP 0` and `JMP` to the live consumer.

## Requirements

- R1. After `JZ` with known non-zero condition, continue linear scan unless fall-through begins with negative `MOVSP` (return/exit cleanup).
- R2. Symmetric rule for `JNZ` with known zero condition.
- R3. Tests: `WhileBreakLocalStrRef` pass, `DeadReturnLocalStrRef` and `ElseBranchLocalStrRef` unchanged; **46** NcsConsti tests pass.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Heuristic `MOVSP` sign discrimination only; full loop CFG deferred.
