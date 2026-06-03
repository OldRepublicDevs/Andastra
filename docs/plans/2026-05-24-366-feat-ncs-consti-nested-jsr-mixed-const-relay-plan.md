---
title: "feat: ncs consti nested jsr mixed const cptopsp relay"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-362-feat-ncs-consti-nested-jsr-multi-arg-relay-plan.md
branch: feat/plan-366-nested-jsr-mixed-const-relay
---

# feat: NCS CONSTI nested JSR mixed CONST/CPTOPSP relay (plan 366)

## Summary

Plan **362** covers symmetric multi-arg forward (`relay(a,s) → speak(a,s)`). Add mixed push runs where the nested callee receives a **CONST literal** for one arg and **CPTOPSP** for the StrRef param (`relay(a,s) → speak(0,s)`).

## Requirements

- R1. Nested relay with CONST + CPTOPSP push run before JSR → `StrRefConsumer` for forwarded StrRef param.
- R2. `StrRefReferenceCache` indexes the StrRef.
- R3. Existing **88** NcsConsti tests pass; **+2** new tests (**90** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Mixed CONST/CPTOPSP nested relay only; no full stack simulation.
