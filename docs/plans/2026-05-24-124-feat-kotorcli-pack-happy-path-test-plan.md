---
title: "feat: kotorcli pack command happy path test"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-123-fix-kotorcli-convert-glob-pattern-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI pack command happy path test (plan 124)

## Summary

Add a happy-path test for `pack` when `.kotorcli/cache` is populated, using `--noConvert` and `--noCompile` to isolate the archive write step.

## Requirements

- R1. Test: populated cache directory produces `test.mod` with exit code 0.
- R2. Test: packed MOD contains the cached UTC resource (resref round-trip).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~PackCommand`
