---
title: "feat: NCS CONSTI arithmetic StrRef relay v4"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-422-feat-ncs-consti-arithmetic-local-strref-relay-v3-plan.md
branch: feat/plan-424-ncs-consti-arithmetic-strref-relay-v4
---

# feat: NCS CONSTI arithmetic StrRef relay v4 (plan 424)

## Summary

Plan **421** added direct-action MUL/MOD/chained-ADD `GetConstiUsageContext` probes; plan **422** added local ADD via CPTOPSP. This slice closes **StrRefReferenceCache** parity for the 421 operators and extends local stack simulation to **SUB** (`int n = CONST - 1; ActionSpeakStringByStrRef(n);`). Test-only — no scanner changes expected.

## Requirements

- R1. `StrRefReferenceCache_ArithmeticMulStrRefLiteral_IsIndexed` mirrors Add cache probe.
- R2. `StrRefReferenceCache_ArithmeticModStrRefLiteral_IsIndexed` mirrors Add cache probe.
- R3. `StrRefReferenceCache_ChainedArithmeticAddStrRefLiteral_IsIndexed` mirrors Add cache probe.
- R4. `GetConstiUsageContext_ArithmeticLocalSubStrRefViaCptopsp_ReturnsStrRefConsumer` for local SUB assignment.
- R5. `StrRefReferenceCache_ArithmeticLocalSubStrRefViaCptopsp_IsIndexed` for local SUB cache path.
- R6. **117** NcsConsti tests pass (112 + 5).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Arithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Stack simulation v5: DIV direct-action + local MUL/MOD cache probes.
- Docs tracker sync for stack-simulation arc after #91 merge.
- Field-value arc merge stack (#81–#86).
