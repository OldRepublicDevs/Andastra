---
title: "test: kotorcli list search archive mod"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-161-test-kotorcli-create-archive-mod-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list/search archive MOD (plan 162)

## Summary

Integration tests for `list-archive` and `search-archive` against MOD (ERF) archives. Closes parity gap noted in PR #7 residuals after RIM/BIF coverage.

## Requirements

- R1. `list-archive --file sample.mod` exits zero for a MOD with resources.
- R2. `list-archive --filter sample_*` exits zero when a matching resource exists.
- R3. `search-archive` wildcard name match exits zero on MOD.
- R4. `search-archive --content` matches payload strings inside MOD resources.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommands`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
