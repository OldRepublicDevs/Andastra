---
title: "feat: NCS CONSTI six-hop identity double arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-453-feat-ncs-consti-five-hop-identity-double-arithmetic-local-strref-relay-plan.md
branch: feat/plan-455-ncs-consti-six-hop-identity-double-arithmetic-local-strref-relay
---

# feat: NCS CONSTI six-hop identity double arithmetic local StrRef relay (plan 455)

## Summary

Plan **453** covers five-hop identity double arithmetic (`n = CONST + k1; m = n + k2; p = m; q = p; r = q`). This slice adds a sixth identity hop: `int s = r; ActionSpeakStringByStrRef(s)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_SixHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_SixHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **155** NcsConsti tests pass (153 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SixHopIdentityDoubleArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **455** at **155** tests.
- Field-value arc **#81**–**#86**.
