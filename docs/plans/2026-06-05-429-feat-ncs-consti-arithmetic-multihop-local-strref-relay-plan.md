---
title: "feat: NCS CONSTI arithmetic multihop local StrRef relay"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-428-chore-stack-simulation-arc-tracker-sync-v3-plan.md
branch: feat/plan-429-ncs-consti-arithmetic-multihop-local-strref-relay
---

# feat: NCS CONSTI arithmetic multihop local StrRef relay (plan 429)

## Summary

Plan **427** closed single-assignment local arithmetic. Plan **313** multihop tests cover copy relay (`int n = CONST; int m = n`). This slice adds **arithmetic on the second local hop**: `int n = CONST; int m = n + 0; ActionSpeakStringByStrRef(m)`. Test-only — existing `TryFindNextStackStoreAfterLoad` relay after CPTOPSP should bridge ADD to the next store.

## Requirements

- R1. `GetConstiUsageContext_ArithmeticMultiHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_ArithmeticMultiHopLocalStrRefViaCptopsp_IsIndexed`.
- R3. **129** NcsConsti tests pass (127 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArithmeticMultiHop
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Arithmetic-first multihop (`int n = CONST + k; int m = n; ActionSpeakStringByStrRef(m)`).
- Docs tracker sync: stack-simulation arc **#97**–**#98** after merge.
- Field-value arc **#81**–**#86**.
