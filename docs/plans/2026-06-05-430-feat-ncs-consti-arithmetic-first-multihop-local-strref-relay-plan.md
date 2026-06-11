---
title: "feat: NCS CONSTI arithmetic-first multihop local StrRef relay"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-429-feat-ncs-consti-arithmetic-multihop-local-strref-relay-plan.md
branch: feat/plan-430-ncs-consti-arithmetic-first-multihop-local-strref-relay
---

# feat: NCS CONSTI arithmetic-first multihop local StrRef relay (plan 430)

## Summary

Plan **429** added arithmetic on the second local hop (`m = n + 0`). This slice adds **arithmetic on the first hop then copy relay**: `int n = CONST + offset; int m = n; ActionSpeakStringByStrRef(m)`. Test-only — plan 422 arithmetic-then-store plus plan 313 multihop stack reload should compose.

## Requirements

- R1. `GetConstiUsageContext_ArithmeticFirstMultiHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_ArithmeticFirstMultiHopLocalStrRefViaCptopsp_IsIndexed`.
- R3. **131** NcsConsti tests pass (129 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArithmeticFirstMultiHop
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#97**–**#98** at **129**–**131** tests.
- Field-value arc **#81**–**#86**.
- Merge stack-simulation arc after relay **#88** lands.
