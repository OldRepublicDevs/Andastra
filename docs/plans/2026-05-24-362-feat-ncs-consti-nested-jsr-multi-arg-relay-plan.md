---
title: "feat: ncs consti nested jsr multi-arg relay"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-360-feat-ncs-consti-nested-jsr-relay-plan.md
branch: feat/plan-362-nested-jsr-multi-arg-relay
---

# feat: NCS CONSTI nested JSR multi-arg relay (plan 362)

## Summary

Plan **360** follows single-push nested JSR relays (`relay(s) → speak(s)`). Extend nested relay collection to walk back through contiguous CPTOPSP/CONST pushes so multi-arg forwards like `relay(a, s) → speak(a, s)` preserve slot alignment.

## Requirements

- R1. `TryFollowNestedJsrRelay` finds full contiguous push run before nested JSR, not only from the matched CPTOPSP opcode.
- R2. Multi-arg nested relay with StrRef on second param → `StrRefConsumer`.
- R3. Two-hop nested relay (`outer → mid → speak`) → `StrRefConsumer`.
- R4. Existing **83** NcsConsti tests pass; **+3** new tests (**86** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Nested JSR push-run backwalk + characterization tests only; no full stack simulation.
