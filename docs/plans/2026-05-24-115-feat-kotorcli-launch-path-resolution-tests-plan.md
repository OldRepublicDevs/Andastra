---
title: "feat: kotorcli launch path resolution test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-114-feat-kotorcli-format-convert-missing-input-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI launch path resolution test closure (plan 115)

## Summary

Extend launch stub coverage with integration-style unit tests for `LaunchCommand.ResolveGameBinary` / dry-run path resolution (README Known Issue #1: launch remains stub; path resolution must stay reliable).

## Requirements

- R1. Dry-run with `--installDir` containing `swkotor.exe` exits 0.
- R2. Dry-run with invalid `--gameBin` falls back to `--installDir` when the install dir contains `swkotor.exe`.
- R3. Dry-run with no resolvable binary (no gameBin, installDir, or env) exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~LaunchCommand`
