---
title: "feat: NCS CONSTI seven-hop identity double arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-455-feat-ncs-consti-six-hop-identity-double-arithmetic-local-strref-relay-plan.md
branch: feat/plan-461-ncs-consti-seven-hop-identity-double-arithmetic-local-strref-relay
---

# feat: NCS CONSTI seven-hop identity double arithmetic local StrRef relay (plan 461)

## Summary

Plan **455** covers six-hop identity double arithmetic (`n = CONST + k1; m = n + k2; p = m; q = p; r = q; s = r`). This slice adds a seventh identity hop: `int t = s; ActionSpeakStringByStrRef(t)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_SevenHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_SevenHopIdentityDoubleArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **161** NcsConsti tests pass (159 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SevenHopIdentityDoubleArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **461** at **161** tests.
- Field-value arc **#81**–**#86**.
