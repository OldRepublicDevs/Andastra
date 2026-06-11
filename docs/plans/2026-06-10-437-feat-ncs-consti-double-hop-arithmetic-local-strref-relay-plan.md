---
title: "feat: NCS CONSTI double-hop arithmetic local StrRef relay"
type: feat
status: completed
date: 2026-06-10
origin: docs/plans/2026-06-10-435-feat-ncs-consti-second-hop-arithmetic-local-strref-relay-plan.md
branch: feat/plan-437-ncs-consti-double-hop-arithmetic-local-strref-relay
---

# feat: NCS CONSTI double-hop arithmetic local StrRef relay (plan 437)

## Summary

Plans **430**–**435** cover single-hop arithmetic (first or second hop only). This slice combines non-zero ADD on both hops: `int n = CONST + k1; int m = n + k2; ActionSpeakStringByStrRef(m)`. Test-only — existing plan 422 arithmetic-then-store and multihop relay paths cover the bytecode shape.

## Requirements

- R1. `GetConstiUsageContext_DoubleHopArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_DoubleHopArithmeticLocalStrRefViaCptopsp_IsIndexed`.
- R3. **137** NcsConsti tests pass (135 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~DoubleHopArithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#104**–**#105** at **135**–**137** tests (plan **438**).
- Triple multihop with double arithmetic: `int n = CONST + k1; int m = n + k2; int p = m; ActionSpeakStringByStrRef(p)`.
- Field-value arc **#81**–**#86**.
