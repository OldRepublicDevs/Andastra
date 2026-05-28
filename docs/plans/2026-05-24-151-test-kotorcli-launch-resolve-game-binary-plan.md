---
title: "test: kotorcli launch resolve game binary"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-150-test-kotorcli-search-archive-error-paths-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI LaunchCommand ResolveGameBinary unit tests (plan 151)

## Summary

Direct unit tests for `LaunchCommand.ResolveGameBinary` (internal, exposed via `InternalsVisibleTo`) covering game path resolution priority, env-var fallback, and null/error paths not asserted by existing `Execute` dry-run tests.

## Requirements

- R1. **Resolution priority**: explicit `--gameBin` (with `Path.GetFullPath` normalization) wins over `--installDir`; install dir prefers `swkotor.exe` over `swkotor2.exe` when both exist; `KOTOR_PATH` then `K1_PATH` env vars used when install dir unset and `chitin.key` present.
- R2. **Null/error paths**: missing game binary, nonexistent install dir, install dir without exe files, env var without `chitin.key` all return null; invalid `--gameBin` falls back to install dir when provided.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~LaunchCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
