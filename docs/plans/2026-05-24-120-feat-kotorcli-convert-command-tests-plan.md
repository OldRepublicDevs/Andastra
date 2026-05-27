---
title: "feat: kotorcli convert command test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-119-feat-kotorcli-launch-tsl-pack-error-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI convert command test closure (plan 120)

## Summary

Add build-pipeline tests for wired `convert`: error paths and a minimal JSON→GFF conversion happy path.

## Requirements

- R1. Expose `ConvertCommand.Execute` for direct tests.
- R2. Test: no `kotorcli.cfg` exits non-zero.
- R3. Test: unknown target exits non-zero.
- R4. Test: converts a `src/*.json` GFF fixture to binary alongside the JSON source (exit 0).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ConvertCommand`
