---
title: "feat: NCS CONSTI seven-hop identity second-offset local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-457-feat-ncs-consti-six-hop-identity-second-offset-local-strref-relay-plan.md
branch: feat/plan-463-ncs-consti-seven-hop-identity-second-offset-local-strref-relay
---

# feat: NCS CONSTI seven-hop identity second-offset local StrRef relay (plan 463)

## Summary

Plan **457** covers six-hop identity second-offset (`n = CONST; m = n + k2; p = m; q = p; r = q; s = r; ActionSpeakStringByStrRef(s)`). This slice adds a seventh identity hop: `int t = s; ActionSpeakStringByStrRef(t)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_SevenHopIdentitySecondOffsetLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_SevenHopIdentitySecondOffsetLocalStrRefViaCptopsp_IsIndexed`.
- R3. **163** NcsConsti tests pass (161 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SevenHopIdentitySecondOffset
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **463** at **163** tests.
- Field-value arc **#81**–**#86**.
