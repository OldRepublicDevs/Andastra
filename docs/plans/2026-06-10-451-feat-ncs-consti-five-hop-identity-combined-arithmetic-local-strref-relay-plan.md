---
title: "feat: NCS CONSTI five-hop identity combined arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-449-feat-ncs-consti-five-hop-identity-second-offset-local-strref-relay-plan.md
branch: feat/plan-451-ncs-consti-five-hop-identity-combined-arithmetic-local-strref-relay
---

# feat: NCS CONSTI five-hop identity combined arithmetic local StrRef relay (plan 451)

## Summary

Plan **445** covers four-hop identity combined arithmetic (`n = CONST + k1; m = n + 0; p = m; q = p`). Plan **449** extends second-offset identity to five hops. This slice merges both: `int n = CONST + k1; int m = n + 0; int p = m; int q = p; int r = q; ActionSpeakStringByStrRef(r)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_FiveHopIdentityCombinedArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_FiveHopIdentityCombinedArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **151** NcsConsti tests pass (149 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FiveHopIdentityCombinedArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc through plan **451** at **151** tests.
- Field-value arc **#81**–**#86**.
- Five-hop identity double arithmetic local StrRef relay.
