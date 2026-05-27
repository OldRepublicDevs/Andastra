---
title: "test: kotorcli create archive erf filter"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-161-test-kotorcli-create-archive-mod-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI create-archive ERF filter (plan 167)

## Summary

Integration tests for `create-archive --type erf --filter` wildcard selection when packing a directory into an ERF archive. Closes parity gap after plans 157 (RIM) and 161 (MOD).

## Requirements

- R1. Filter `merchant*` packs only matching files into the ERF; `vendor.utc` is excluded.
- R2. Filter with no matches produces an empty ERF archive and exits zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CreateArchiveCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
