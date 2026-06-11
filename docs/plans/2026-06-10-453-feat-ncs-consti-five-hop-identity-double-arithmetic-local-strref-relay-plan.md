---
title: "feat: NCS CONSTI five-hop identity double arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-451-feat-ncs-consti-five-hop-identity-combined-arithmetic-local-strref-relay-plan.md
branch: feat/plan-453-ncs-consti-five-hop-identity-double-arithmetic-local-strref-relay
---

# feat: NCS CONSTI five-hop identity double arithmetic local StrRef relay (plan 453)

## Summary

Plan **443** covers four-hop identity double arithmetic (`n = CONST + k1; m = n + k2; p = m; q = p`). Plan **451** extends combined-arithmetic identity to five hops. This slice merges double arithmetic with a fifth identity hop: `int n = CONST + k1; int m = n + k2; int p = m; int q = p; int r = q; ActionSpeakStringByStrRef(r)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_FiveHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_FiveHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **153** NcsConsti tests pass (151 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FiveHopIdentityDoubleArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **453** at **153** tests.
- Field-value arc **#81**–**#86**.
