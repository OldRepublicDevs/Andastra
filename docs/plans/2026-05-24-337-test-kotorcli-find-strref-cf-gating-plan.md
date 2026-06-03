---
title: "test: KotorCLI find-strref control-flow gating"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-336-chore-pr36-merge-readiness-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: KotorCLI find-strref control-flow gating (plan 337)

## Summary

Plans **324**–**336** landed NCS CONSTI control-flow classification in BioWare. Extend KotorCLI `find-strref` **cache-path** tests so dead early-return locals are not hits and live early-return paths remain hits — parity with `StrRefReferenceCache` indexing (slow path still matches raw CONSTI by design).

## Requirements

- R1. `Execute_NcsDeadReturnLocalStrRef_CachePath_ExitsNonZero` — cache build + query; dead `if (1) return;` consumer
- R2. `Execute_NcsEarlyReturnLiveLocalStrRef_CachePath_ExitsZero` — cache build + query; live `if (0) return;` consumer
- R3. KotorCLI.Tests green on `net9.0`

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRefCommandTests
```

## Scope Boundaries

- Test-only; reuses existing `FindStrRefCommand` + `StrRefReferenceCache` stack from PR #36.
