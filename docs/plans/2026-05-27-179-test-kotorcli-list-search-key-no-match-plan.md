---
title: "test: kotorcli list search key no-match"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-176-test-kotorcli-list-search-key-archive-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list/search KEY file no-match paths (plan 179)

## Summary

Add negative-path integration tests for standalone `.key` list/search, mirroring MOD/ERF filter-no-match and search-no-match coverage. Plan 176 added KEY happy paths only.

## Requirements

- R1. `ListArchiveCommand.Execute` on KEY with non-matching filter exits non-zero.
- R2. `SearchArchiveCommand.Execute` on KEY with non-matching pattern exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **235**.
