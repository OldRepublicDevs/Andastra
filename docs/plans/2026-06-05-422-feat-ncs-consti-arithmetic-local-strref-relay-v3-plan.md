---
title: "feat: NCS CONSTI arithmetic local StrRef relay v3"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-421-feat-ncs-consti-arithmetic-strref-relay-v2-plan.md
branch: feat/plan-422-ncs-consti-arithmetic-local-strref-relay-v3
---

# feat: NCS CONSTI arithmetic local StrRef relay v3 (plan 422)

## Summary

Plans **409**/**421** classify arithmetic in direct ACTION argument runs. This slice extends stack simulation to **local assignment** — `int n = 424242 + 100; ActionSpeakStringByStrRef(n);` — by tracing CONSTI through ADD/SUB then `CPDOWNSP` into the existing CPTOPSP reload path.

## Requirements

- R1. `GetConstiUsageContext_ArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer` and matching `StrRefReferenceCache` test.
- R2. Scanner: forward arithmetic run ending in `CPDOWNSP`/`CPDOWNBP` links tracked CONSTI to stack reload consumer detection.
- R3. **112** NcsConsti tests pass (110 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArithmeticLocal
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
