---
title: "test: BioWare FindStrRefReferences slow vs cache control-flow"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-338-test-kotorcli-cli-find-strref-cf-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: BioWare FindStrRefReferences slow vs cache control-flow (plan 339)

## Summary

Plans **337**–**338** validated KotorCLI cache-path gating. Add BioWare `ReferenceCacheHelpers.FindStrRefReferences` tests documenting slow-path (raw CONSTI) vs cache-path (context-gated) behavior for dead/live early-return patterns.

## Requirements

- R1. `FindStrRefReferences_DeadReturnSlowPath_StillFindsLiteral` — no cache; slow path matches raw CONSTI
- R2. `FindStrRefReferences_DeadReturnCachePath_IsEmpty` — built cache excludes dead consumer
- R3. `FindStrRefReferences_EarlyReturnLiveCachePath_FindsConsumer` — built cache indexes live consumer
- R4. **74** NcsConsti tests pass

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Test-only; documents intentional slow-path vs cache-path split (no scanner change).
