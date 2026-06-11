---
title: "feat: NCS CONSTI four-hop identity combined arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-443-feat-ncs-consti-four-hop-identity-double-arithmetic-local-strref-relay-plan.md
branch: feat/plan-445-ncs-consti-four-hop-identity-combined-arithmetic-local-strref-relay
---

# feat: NCS CONSTI four-hop identity combined arithmetic local StrRef relay (plan 445)

## Summary

Plan **441** covers triple multihop combined arithmetic (`n = CONST + k1; m = n + 0; p = m`). Plan **443** adds a fourth identity hop after double arithmetic. This slice merges both patterns: `int n = CONST + k1; int m = n + 0; int p = m; int q = p; ActionSpeakStringByStrRef(q)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_FourHopIdentityCombinedArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_FourHopIdentityCombinedArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **145** NcsConsti tests pass (143 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FourHopIdentityCombinedArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#110**–**#111** at **143**–**145** tests (plan **446**).
- Four-hop identity after triple multihop double arithmetic with non-zero second offset variants.
- Field-value arc **#81**–**#86**.
