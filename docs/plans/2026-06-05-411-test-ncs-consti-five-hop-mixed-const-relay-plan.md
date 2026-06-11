---
title: "test: NCS CONSTI five-hop mixed CONST+CPTOPSP relay"
type: test
status: completed
date: 2026-06-05
origin: docs/plans/2026-06-05-410-feat-ncs-consti-five-hop-multi-arg-jsr-relay-plan.md
branch: feat/plan-411-five-hop-mixed-const-relay-rebase
---

# test: NCS CONSTI five-hop mixed CONST+CPTOPSP relay (plan 411)

## Summary

Four-hop mixed CONST+CPTOPSP nested JSR relay is covered on `master`; six-hop mixed landed via stack integration **#135**. This slice fills the **five-hop** gap: outer passes `(99, CONST)` through five JSR hops where the innermost `speak` uses `speak(0, s)` (mixed CONST + CPTOPSP parameter relay).

Test-only — `MaxNestedJsrRelayDepth = 6` from plan **463** integration already satisfies depth.

## Requirements

- R1. `GetConstiUsageContext_FiveHopMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer`.
- R2. `StrRefReferenceCache_FiveHopMixedConstCptopspRelayStrRef_IsIndexed`.
- R3. **167** NcsConsti tests pass (165 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FiveHopMixedConstCptopsp
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Suggested next slices

- Plan **466** post-D1 tracker sync v21.
- Field-value arc **#81**–**#85** (plan 465 Day 3).
