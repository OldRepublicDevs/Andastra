---
title: "feat: ncs consti three-hop mixed const cptopsp jsr relay"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-368-feat-ncs-consti-three-hop-multi-arg-jsr-relay-plan.md
branch: feat/plan-370-three-hop-mixed-const-relay
---

# feat: NCS CONSTI three-hop mixed CONST/CPTOPSP nested JSR relay (plan 370)

## Summary

Plan **366** covers single-hop mixed CONST+CPTOPSP relay (`relay → speak(0,s)`). Plan **368** covers three-hop symmetric multi-arg relay. Combine: **three-hop chain with mixed CONST push at the leaf callee** (`inner` calls `speak(0,s)`).

## Requirements

- R1. Three-hop relay where innermost nested call uses CONST for first arg and CPTOPSP for StrRef param → `StrRefConsumer`.
- R2. `StrRefReferenceCache` indexes the StrRef.
- R3. Existing **92** NcsConsti tests pass; **+2** new tests (**94** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Characterization tests only; scanner from plan **362** handles three-hop mixed push runs without code changes.
