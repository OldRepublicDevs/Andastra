---
title: "test: NCS CONSTI while-zero and if-one-live coverage"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-329-fix-ncs-consti-while-break-scan-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: NCS CONSTI while-zero and if-one-live coverage (plan 330)

## Summary

Extend control-flow regression suite after plan **329** with never-entered `while (0)` bodies and live `if (1) { consumer }` branches. No scanner changes — probes confirm existing heuristics handle both.

## Requirements

- R1. `GetConstiUsageContext_DeadWhileBodyLocalStrRef_RemainsStackStored`
- R2. `GetConstiUsageContext_IfOneLiveBranchLocalStrRef_ReturnsStrRefConsumer`
- R3. **48** NcsConsti tests pass

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Test-only; no `NcsConstiScanner` changes.
