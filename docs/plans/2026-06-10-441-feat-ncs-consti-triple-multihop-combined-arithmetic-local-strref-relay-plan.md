---
title: "feat: NCS CONSTI triple multihop combined arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-439-feat-ncs-consti-triple-multihop-double-arithmetic-local-strref-relay-plan.md
branch: feat/plan-441-ncs-consti-triple-multihop-combined-arithmetic-local-strref-relay
---

# feat: NCS CONSTI triple multihop combined arithmetic local StrRef relay (plan 441)

## Summary

Plans **432** (`n = CONST + k; m = n + 0`) and **439** (`n = CONST + k1; m = n + k2; p = m`) cover two-hop combined and triple multihop double arithmetic separately. This slice combines both: `int n = CONST + k1; int m = n + 0; int p = m; ActionSpeakStringByStrRef(p)`. Test-only.

## Requirements

- R1. `GetConstiUsageContext_TripleMultihopCombinedArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_TripleMultihopCombinedArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **141** NcsConsti tests pass (139 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TripleMultihopCombinedArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#108**–**#109** at **139**–**141** tests (plan **442**).
- Triple multihop + full combined double-hop: `n = CONST + k1; m = n + k2; p = m; ActionSpeakStringByStrRef(p)` (already **439**); next: four-hop identity relay after combined arithmetic.
- Field-value arc **#81**–**#86**.
