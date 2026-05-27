---
title: "test: kotorcli search mod case sensitive"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-164-test-kotorcli-list-search-mod-edge-cases-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive MOD case-sensitive (plan 165)

## Summary

Case-sensitive happy-path and content-mismatch tests for `search-archive` on MOD archives, completing parity with RIM coverage from plan 155 after plan 164 added MOD name case rejection.

## Requirements

- R1. `--case-sensitive` name match with exact case exits zero on MOD.
- R2. `--case-sensitive --content` rejects payload case mismatch on MOD.
- R3. `--case-sensitive --content` matches exact-case payload strings on MOD.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommands`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
