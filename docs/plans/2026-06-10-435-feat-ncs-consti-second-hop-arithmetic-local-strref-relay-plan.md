---
title: "feat: NCS CONSTI second-hop arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-05-432-feat-ncs-consti-combined-arithmetic-multihop-local-strref-relay-plan.md
branch: feat/plan-435-ncs-consti-second-hop-arithmetic-local-strref-relay
---

# feat: NCS CONSTI second-hop arithmetic local StrRef relay (plan 435)

## Summary

Plans **429** (`m = n + 0`) and **432** (`n = CONST + k; m = n + 0`) cover zero-offset second-hop ADD. This slice adds non-zero second-hop local ADD after plain CONST store: `int n = CONST; int m = n + offset; ActionSpeakStringByStrRef(m)`. Test-only — existing plan 422 arithmetic-then-store and multihop relay paths cover the bytecode shape.

## Requirements

- R1. `GetConstiUsageContext_SecondHopArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_SecondHopArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **135** NcsConsti tests pass (133 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SecondHopArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#101**–**#103** at **133**–**135** tests.
- Double-hop arithmetic: `int n = CONST + k1; int m = n + k2; ActionSpeakStringByStrRef(m)`.
- Field-value arc **#81**–**#86**.
