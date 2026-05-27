---
title: "test: kotorcli extract key and mod search content disabled"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-165-test-kotorcli-search-mod-case-sensitive-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract KEY + MOD search content-disabled (plan 166)

## Summary

Baseline KEY extraction test (no filter) and MOD parity for `search-archive` content-disabled behavior when the name pattern does not match.

## Requirements

- R1. `extract --file sample.key` writes named resource under `{bifStem}/` subdirectory.
- R2. `search-archive` with `--content` disabled skips payload-only matches on MOD (exits non-zero).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExtractCommand|ArchiveCommands`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
