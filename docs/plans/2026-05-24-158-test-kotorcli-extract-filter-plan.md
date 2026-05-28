---
title: "test: kotorcli extract filter"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-157-test-kotorcli-create-archive-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract filter (plan 158)

## Summary

Integration tests for `extract --filter` wildcard selection when unpacking RIM archives.

## Requirements

- R1. Filter `creature_a*` extracts only the matching resource file.
- R2. Filter with no matches exits zero and writes no output files.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExtractCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
