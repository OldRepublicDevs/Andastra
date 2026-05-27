---
title: "test: kotorcli create-archive empty mod erf"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-177-test-kotorcli-create-archive-empty-and-search-empty-path-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI create-archive empty MOD and ERF directories (plan 178)

## Summary

Extend plan 177's empty-input-directory coverage to MOD and ERF archives, mirroring `Execute_CreateRimFromEmptyDirectory_ProducesEmptyArchive`.

## Requirements

- R1. `CreateArchiveCommand.Execute` on empty input directory creates readable empty MOD and exits zero.
- R2. Same for ERF archive type.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **233**.
