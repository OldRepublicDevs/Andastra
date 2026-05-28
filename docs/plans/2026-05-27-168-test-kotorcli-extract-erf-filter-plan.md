---
title: "test: kotorcli extract erf filter"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-167-test-kotorcli-create-archive-erf-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract ERF filter (plan 168)

## Summary

Integration tests for `extract --filter` when unpacking ERF archives. Closes parity gap after plan 160 (MOD) and plan 167 (create-archive ERF filter).

## Requirements

- R1. Filter `creature_a*` on an ERF with two resources extracts only `creature_a.utc`.
- R2. Filter with no matches exits zero and writes no output files.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExtractCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
