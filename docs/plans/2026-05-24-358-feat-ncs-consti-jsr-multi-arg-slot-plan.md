---
title: "feat: ncs consti jsr multi-arg slot alignment"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-356-feat-ncs-consti-jsr-call-literal-strref-plan.md
branch: feat/plan-358-jsr-multi-arg-slot
---

# feat: NCS CONSTI JSR multi-arg slot alignment (plan 358)

## Summary

Plan **356** classifies `speak(424242)` JSR call-literal patterns as `StrRefConsumer` but uses a loose callee heuristic (any CPTOPSP + StrRef ACTION). Tighten to match the caller push slot index with the callee StrRef ACTION parameter slot — fixing multi-arg false positives.

## Requirements

- R1. `TryFindStrRefConsumerViaJsrCall` collects full CONST push run before JSR (walk back from target CONSTI).
- R2. Callee scan verifies loaded stack param index aligns with caller slot index for StrRef ACTION.
- R3. Tests: multi-arg correct slot → `StrRefConsumer`; wrong slot → `Unknown`; cache indexed only for correct slot.
- R4. Existing **77** NcsConsti tests pass; **+3** new tests (**80** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- JSR call-literal multi-arg slot alignment only; no nested JSR chains or full stack simulation.
