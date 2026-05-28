---
title: "test: kotorcli list search mod edge cases"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-162-test-kotorcli-list-search-archive-mod-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list/search MOD edge cases (plan 164)

## Summary

Negative-path and verbose parity tests for `list-archive` and `search-archive` on MOD archives, mirroring existing RIM coverage from plans 150–155.

## Requirements

- R1. `list-archive --verbose` exits zero on MOD.
- R2. `list-archive --filter` with no matches exits non-zero on MOD.
- R3. `search-archive` with no wildcard match exits non-zero on MOD.
- R4. `search-archive --case-sensitive` rejects case mismatch on MOD resource names.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommands`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
