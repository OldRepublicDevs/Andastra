---
title: "feat: NCS CONSTI five-hop identity second-offset local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-447-feat-ncs-consti-four-hop-identity-second-offset-local-strref-relay-plan.md
branch: feat/plan-449-ncs-consti-five-hop-identity-second-offset-local-strref-relay
---

# feat: NCS CONSTI five-hop identity second-offset local StrRef relay (plan 449)

## Summary

Plan **447** covers `n = CONST; m = n + k2; p = m; q = p` with second offset non-zero. This slice extends the identity chain by one hop: `int n = CONST; int m = n + k2; int p = m; int q = p; int r = q; ActionSpeakStringByStrRef(r)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_FiveHopIdentitySecondOffsetLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_FiveHopIdentitySecondOffsetLocalStrRefViaCptopsp_IsIndexed`.
- R3. **149** NcsConsti tests pass (147 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FiveHopIdentitySecondOffset
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **449** at **149** tests.
- Field-value arc **#81**–**#86**.
- Five-hop identity variants with additional hop permutations.
