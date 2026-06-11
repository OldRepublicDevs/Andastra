---
title: "feat: NCS CONSTI four-hop identity second-offset local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-445-feat-ncs-consti-four-hop-identity-combined-arithmetic-local-strref-relay-plan.md
branch: feat/plan-447-ncs-consti-four-hop-identity-second-offset-local-strref-relay
---

# feat: NCS CONSTI four-hop identity second-offset local StrRef relay (plan 447)

## Summary

Plan **443** covers `n = CONST + k1; m = n + k2; p = m; q = p` with both offsets non-zero. Plan **445** covers combined arithmetic with first offset only (`m = n + 0`). This slice covers the variant where only the **second** offset is non-zero (first hop is identity): `int n = CONST; int m = n + k2; int p = m; int q = p; ActionSpeakStringByStrRef(q)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_FourHopIdentitySecondOffsetLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_FourHopIdentitySecondOffsetLocalStrRefViaCptopsp_IsIndexed`.
- R3. **147** NcsConsti tests pass (145 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FourHopIdentitySecondOffset
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **447** at **147** tests.
- Field-value arc **#81**–**#86**.
- Four-hop identity variants with additional hop permutations.
