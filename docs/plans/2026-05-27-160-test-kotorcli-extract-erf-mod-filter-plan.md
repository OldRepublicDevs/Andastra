---
title: "test: kotorcli extract erf mod filter"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-159-test-kotorcli-extract-bif-key-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract ERF/MOD filter (plan 160)

## Summary

Integration tests for `extract --filter` when unpacking MOD (ERF) archives. Closes the parity gap deferred from plan 159 after RIM and BIF+KEY filter coverage.

## Requirements

- R1. Filter `creature_a*` on a MOD with two resources extracts only `creature_a.utc`.
- R2. Filter with no matches exits zero and writes no output files.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExtractCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
