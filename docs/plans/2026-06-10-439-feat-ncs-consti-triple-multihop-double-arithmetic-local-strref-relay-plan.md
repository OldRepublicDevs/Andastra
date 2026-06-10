---
title: "feat: NCS CONSTI triple multihop double arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-437-feat-ncs-consti-double-hop-arithmetic-local-strref-relay-plan.md
branch: feat/plan-439-ncs-consti-triple-multihop-double-arithmetic-local-strref-relay
---

# feat: NCS CONSTI triple multihop double arithmetic local StrRef relay (plan 439)

## Summary

Plan **437** covers double-hop non-zero ADD directly to consumer. This slice adds a third identity hop: `int n = CONST + k1; int m = n + k2; int p = m; ActionSpeakStringByStrRef(p)`. Test-only — composes plan 437 double arithmetic with plan 429/430 multihop relay.

## Requirements

- R1. `GetConstiUsageContext_TripleMultihopDoubleArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_TripleMultihopDoubleArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **139** NcsConsti tests pass (137 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TripleMultihopDoubleArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#106**–**#107** at **137**–**139** tests (plan **440**).
- Triple multihop + combined single/zero second hop variants.
- Field-value arc **#81**–**#86**.
