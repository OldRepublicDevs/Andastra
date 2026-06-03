---
title: "test: KotorCLI CLI find-strref cache-path control-flow"
type: test
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-337-test-kotorcli-find-strref-cf-gating-plan.md
branch: feat/plan-324-ncs-consti-conditional-strref
---

# test: KotorCLI CLI find-strref cache-path control-flow (plan 338)

## Summary

Plan **337** added in-process `FindStrRefCommand` cache-path control-flow tests. Extend with subprocess CLI coverage via `InstallationRefSearchCommandCliTests` so `find-strref --cache-file --rebuild-cache` gating is verified end-to-end.

## Requirements

- R1. `Cli_FindStrRef_NcsDeadReturn_CachePath_ExitsNonZero`
- R2. `Cli_FindStrRef_NcsEarlyReturnLive_CachePath_ExitsZero`
- R3. KotorCLI.Tests green on `net9.0`

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~InstallationRefSearchCommandCliTests
```

## Scope Boundaries

- Test-only; reuses plan **337** NSS fixtures via shared helper pattern in CLI test file.
