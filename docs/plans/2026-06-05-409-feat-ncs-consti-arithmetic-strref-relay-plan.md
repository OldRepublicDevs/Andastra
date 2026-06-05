---
title: "feat: ncs consti arithmetic strref relay v1"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/plan-409-ncs-consti-arithmetic-strref-relay
---

# feat: NCS CONSTI arithmetic StrRef relay v1 (plan 409)

## Summary

First stack-simulation slice from plan **063**: CONSTI literals combined via binary int arithmetic (ADD/SUB/MUL/DIV/MOD) feeding a StrRef ACTION classify as `StrRefConsumer`.

## Requirements

- R1. Simulate binary int arithmetic in `TryGetActionArgumentRun`.
- R2. Evaluate action-argument run before immediate `GenericInteger` in `GetConstiUsageContext`.
- R3. **+3** NcsConsti tests (**101** total).

## Verification

`dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti`
