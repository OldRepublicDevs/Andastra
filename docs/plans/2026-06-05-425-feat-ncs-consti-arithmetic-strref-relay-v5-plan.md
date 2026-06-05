---
title: "feat: NCS CONSTI arithmetic StrRef relay v5"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-424-feat-ncs-consti-arithmetic-strref-relay-v4-plan.md
branch: feat/plan-425-ncs-consti-arithmetic-strref-relay-v5
---

# feat: NCS CONSTI arithmetic StrRef relay v5 (plan 425)

## Summary

Plan **424** closed MUL/MOD/chained-ADD cache gaps and local SUB. This slice completes the binary-int arithmetic operator matrix: **DIV** in direct ACTION runs, plus **local MUL/MOD** via CPTOPSP (`int n = CONST op k; ActionSpeakStringByStrRef(n);`). Test-only — existing `IsBinaryIntArithmeticOpcode` and plan 422 stack-store tracing should cover all cases.

## Requirements

- R1. `GetConstiUsageContext_ArithmeticDivStrRefLiteral_ReturnsStrRefConsumer` — `ActionSpeakStringByStrRef(CONST / 1)`.
- R2. `StrRefReferenceCache_ArithmeticDivStrRefLiteral_IsIndexed`.
- R3. `GetConstiUsageContext_ArithmeticLocalMulStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R4. `StrRefReferenceCache_ArithmeticLocalMulStrRefViaCptopsp_IsIndexed`.
- R5. `GetConstiUsageContext_ArithmeticLocalModStrRefViaCptopsp_ReturnsStrRefConsumer`.
- R6. `StrRefReferenceCache_ArithmeticLocalModStrRefViaCptopsp_IsIndexed`.
- R7. **123** NcsConsti tests pass (117 + 6).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Arithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Docs tracker sync: stack-simulation arc **#89–#93**, Step 3b count **123**.
- Local chained arithmetic or DIV local assignment if modder patterns warrant.
- Field-value arc merge stack (#81–#86).
