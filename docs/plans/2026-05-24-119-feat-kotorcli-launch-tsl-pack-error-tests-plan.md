---
title: "feat: kotorcli launch tsl and pack error path tests"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-118-feat-kotorcli-create-archive-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI launch TSL resolution and pack error-path tests (plan 119)

## Summary

Extend build-pipeline test coverage: TSL executable resolution for launch dry-run, and pack command failure paths before cache exists.

## Requirements

- R1. Launch dry-run resolves `swkotor2.exe` when K2 install dir has no `swkotor.exe`.
- R2. Expose `PackCommand.Execute` for direct tests.
- R3. Pack tests: no config, unknown target, and missing cache (with `--noConvert` / `--noCompile`) exit non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~LaunchCommand|FullyQualifiedName~PackCommand"`
