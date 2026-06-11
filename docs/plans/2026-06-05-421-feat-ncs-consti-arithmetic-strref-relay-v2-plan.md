---
title: "feat: NCS CONSTI arithmetic StrRef relay v2"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-409-feat-ncs-consti-arithmetic-strref-relay-plan.md
branch: feat/plan-421-ncs-consti-arithmetic-strref-relay-v2
---

# feat: NCS CONSTI arithmetic StrRef relay v2 (plan 421)

## Summary

Plan **409** / PR **#77** lands binary int arithmetic (ADD/SUB) in the action-argument run. This slice stacks on the open NCS relay tip (**#88**, **107** NcsConsti tests) and extends stack-simulation coverage to **MUL**, **MOD**, and **chained ADD** StrRef ACTION patterns with **+3** characterization tests.

## Requirements

- R1. `ActionSpeakStringByStrRef(424242 * 1)`, `ActionSpeakStringByStrRef(424242 % 1000000)`, and chained `424242 + 100 + 0` classify target CONSTI as `StrRefConsumer`.
- R2. Three `GetConstiUsageContext` probes for MUL, MOD, and chained ADD.
- R3. **110** NcsConsti tests pass (107 + 3); no scanner change unless probes fail.

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Arithmetic
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
