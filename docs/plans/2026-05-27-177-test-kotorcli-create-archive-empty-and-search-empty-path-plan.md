---
title: "test: kotorcli create-archive empty dir and search empty path"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-176-test-kotorcli-list-search-key-archive-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI create-archive empty directory and search-archive empty path (plan 177)

## Summary

Close two small archive error-path gaps: packing an empty input directory into a RIM (distinct from filter-no-match with files present), and `search-archive` with an empty file path (mirroring `ExecuteListArchive_EmptyFilePath_ExitsNonZero`).

## Requirements

- R1. `CreateArchiveCommand.Execute` on an empty input directory creates a readable empty RIM and exits zero.
- R2. `SearchArchiveCommand.Execute` with empty file path exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count.
