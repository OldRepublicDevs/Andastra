---
title: "test: kotorcli list search archive erf"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-168-test-kotorcli-extract-erf-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list/search archive ERF (plan 169)

## Summary

Integration tests for `list-archive` and `search-archive` against ERF archives. Closes parity gap after plan 162 (MOD) and plan 168 (extract ERF filter).

## Requirements

- R1. `list-archive --file sample.erf` exits zero for an ERF with resources.
- R2. `list-archive --filter sample_*` exits zero when a matching resource exists.
- R3. `search-archive` wildcard name match exits zero on ERF.
- R4. `search-archive --content` matches payload strings inside ERF resources.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommands`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
