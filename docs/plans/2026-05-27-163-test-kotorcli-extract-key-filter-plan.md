---
title: "test: kotorcli extract key filter"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-162-test-kotorcli-list-search-archive-mod-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract KEY filter (plan 163)

## Summary

Integration tests for `extract --file sample.key --filter` when unpacking resources via the KEY/BIF extraction path.

## Requirements

- R1. Filter `creature_a*` on a KEY with two mapped BIF resources extracts only `creature_a.utc`.
- R2. Filter with no matches exits zero and writes no output files.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExtractCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
