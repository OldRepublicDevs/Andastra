---
title: "feat: NCS CONSTI combined arithmetic multihop local StrRef relay"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-430-feat-ncs-consti-arithmetic-first-multihop-local-strref-relay-plan.md
branch: feat/plan-432-ncs-consti-combined-arithmetic-multihop-local-strref-relay
---

# feat: NCS CONSTI combined arithmetic multihop local StrRef relay (plan 432)

## Summary

Plans **429** and **430** cover arithmetic on second hop only (`m = n + 0`) and first hop only (`n = CONST + k; m = n`). This slice combines both: `int n = CONST + offset; int m = n + 0; ActionSpeakStringByStrRef(m)`. Test-only — composes plan 422 arithmetic-then-store with plan 429 multihop ADD relay.

## Requirements

- R1. `GetConstiUsageContext_CombinedArithmeticMultiHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_CombinedArithmeticMultiHopLocalStrRefViaCptopsp_IsIndexed`.
- R3. **133** NcsConsti tests pass (131 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CombinedArithmeticMultiHop
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#100**–**#101** at **131**–**133** tests.
- Field-value arc **#81**–**#86**.
- Merge relay arc **#77**–**#88** then rebase stack-simulation tip.
