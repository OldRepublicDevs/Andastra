---
title: "feat: kotorcli compile command test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-120-feat-kotorcli-convert-command-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI compile command test closure (plan 121)

## Summary

Add build-pipeline tests for wired `compile`: error paths and a no-NSS-sources graceful exit.

## Requirements

- R1. Expose `CompileCommand.Execute` for direct tests.
- R2. Test: no `kotorcli.cfg` exits non-zero.
- R3. Test: unknown target exits non-zero.
- R4. Test: config with no matching NSS sources exits zero (warning path).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CompileCommand`
