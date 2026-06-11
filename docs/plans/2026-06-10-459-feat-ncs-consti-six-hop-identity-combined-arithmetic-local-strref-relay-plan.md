---
title: "feat: NCS CONSTI six-hop identity combined arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-451-feat-ncs-consti-five-hop-identity-combined-arithmetic-local-strref-relay-plan.md
branch: feat/plan-459-ncs-consti-six-hop-identity-combined-arithmetic-local-strref-relay
---

# feat: NCS CONSTI six-hop identity combined arithmetic local StrRef relay (plan 459)

## Summary

Plan **451** covers five-hop identity combined arithmetic (`n = CONST + k1; m = n + 0; p = m; q = p; r = q`). This slice adds a sixth identity hop: `int s = r; ActionSpeakStringByStrRef(s)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_SixHopIdentityCombinedArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_SixHopIdentityCombinedArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **159** NcsConsti tests pass (157 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SixHopIdentityCombinedArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **459** at **159** tests.
- Field-value arc **#81**–**#86**.
