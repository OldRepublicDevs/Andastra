---
title: "feat: NCS CONSTI six-hop identity second-offset local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-449-feat-ncs-consti-five-hop-identity-second-offset-local-strref-relay-plan.md
branch: feat/plan-457-ncs-consti-six-hop-identity-second-offset-local-strref-relay
---

# feat: NCS CONSTI six-hop identity second-offset local StrRef relay (plan 457)

## Summary

Plan **449** covers five-hop identity second-offset (`n = CONST; m = n + k2; p = m; q = p; r = q`). Plan **455** extended double arithmetic to six hops. This slice adds a sixth identity hop to the second-offset pattern: `int s = r; ActionSpeakStringByStrRef(s)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_SixHopIdentitySecondOffsetLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_SixHopIdentitySecondOffsetLocalStrRefViaCptopsp_IsIndexed`.
- R3. **157** NcsConsti tests pass (155 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SixHopIdentitySecondOffset
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **457** at **157** tests.
- Field-value arc **#81**–**#86**.
