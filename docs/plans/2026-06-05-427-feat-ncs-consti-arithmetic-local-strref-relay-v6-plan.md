---
title: "feat: NCS CONSTI arithmetic local StrRef relay v6"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-425-feat-ncs-consti-arithmetic-strref-relay-v5-plan.md
branch: feat/plan-427-ncs-consti-arithmetic-local-strref-relay-v6
---

# feat: NCS CONSTI arithmetic local StrRef relay v6 (plan 427)

## Summary

Plan **425** completed direct DIV and local MUL/MOD. This slice closes remaining local assignment gaps: **local DIV** (`int n = CONST / 1`) and **local chained ADD** (`int n = CONST + offset + 0`) before StrRef ACTION. Test-only — plan 422 `TryFindStrRefConsumerViaArithmeticThenStackStore` should handle multi-op runs ending in `CPDOWNSP`.

## Requirements

- R1. `GetConstiUsageContext_ArithmeticLocalDivStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_ArithmeticLocalDivStrRefViaCptopsp_IsIndexed`.
- R3. `GetConstiUsageContext_ArithmeticLocalChainedAddStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R4. `StrRefReferenceCache_ArithmeticLocalChainedAddStrRefViaCptopsp_IsIndexed`.
- R5. **127** NcsConsti tests pass (123 + 4).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArithmeticLocal
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Two-hop local relay (`int n = CONST; int m = n + 0; ActionSpeakStringByStrRef(m)`) if distinct from single-assignment chained ADD.
- Docs tracker sync: stack-simulation arc **#95**–**#96** after merge.
- Field-value arc **#81**–**#86**.
