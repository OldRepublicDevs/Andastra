---
title: "feat: kotorcli install command test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-121-feat-kotorcli-compile-command-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI install command test closure (plan 122)

## Summary

Add build-pipeline tests for wired `install`: config/target error paths and invalid install directory detection.

## Requirements

- R1. Expose `InstallCommand.Execute` for direct tests.
- R2. Test: no `kotorcli.cfg` exits non-zero.
- R3. Test: unknown target exits non-zero.
- R4. Test: `--installDir` pointing at a directory without `chitin.key` exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~InstallCommand`
