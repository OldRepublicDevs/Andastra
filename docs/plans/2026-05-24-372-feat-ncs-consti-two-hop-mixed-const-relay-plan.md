---
title: "feat: ncs consti two-hop mixed const cptopsp jsr relay"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-366-feat-ncs-consti-nested-jsr-mixed-const-relay-plan.md
branch: feat/plan-372-two-hop-mixed-const-relay
---

# feat: NCS CONSTI two-hop mixed CONST/CPTOPSP nested JSR relay (plan 372)

## Summary

Plan **366** covers single-hop mixed CONST+CPTOPSP relay (`relay → speak(0,s)`). Plan **370** covers three-hop mixed relay (`relay → mid → inner → speak(0,s)`). Fill the gap with **two-hop mixed** (`relay → mid → speak(0,s)`).

## Requirements

- R1. Two-hop relay where the leaf nested call uses CONST for first arg and CPTOPSP for StrRef param → `StrRefConsumer`.
- R2. `StrRefReferenceCache` indexes the StrRef.
- R3. Existing **94** NcsConsti tests pass; **+2** new tests (**96** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Characterization tests only; scanner from plan **362** handles two-hop mixed push runs without code changes.
