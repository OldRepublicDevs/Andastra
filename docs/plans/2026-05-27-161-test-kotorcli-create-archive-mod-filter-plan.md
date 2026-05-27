---
title: "test: kotorcli create archive mod filter"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-160-test-kotorcli-extract-erf-mod-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI create-archive MOD filter (plan 161)

## Summary

Integration tests for `create-archive --type mod --filter` wildcard selection when packing a directory into a MOD archive. Closes parity gap after plan 157 (RIM-only filter coverage).

## Requirements

- R1. Filter `merchant*` packs only matching files into the MOD; `vendor.utc` is excluded.
- R2. Filter with no matches produces an empty MOD archive and exits zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CreateArchiveCommand`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
