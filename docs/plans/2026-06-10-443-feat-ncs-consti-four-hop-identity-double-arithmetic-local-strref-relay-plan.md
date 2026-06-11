---
title: "feat: NCS CONSTI four-hop identity double arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-441-feat-ncs-consti-triple-multihop-combined-arithmetic-local-strref-relay-plan.md
branch: feat/plan-443-ncs-consti-four-hop-identity-double-arithmetic-local-strref-relay
---

# feat: NCS CONSTI four-hop identity double arithmetic local StrRef relay (plan 443)

## Summary

Plan **439** covers triple multihop double arithmetic (`n = CONST + k1; m = n + k2; p = m`). This slice adds a fourth identity hop: `int n = CONST + k1; int m = n + k2; int p = m; int q = p; ActionSpeakStringByStrRef(q)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_FourHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_FourHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **143** NcsConsti tests pass (141 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FourHopIdentityDoubleArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#110**–**#111** at **141**–**143** tests (plan **444**).
- Four-hop identity after triple multihop combined arithmetic.
- Field-value arc **#81**–**#86**.
