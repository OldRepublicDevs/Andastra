---
title: "test: kotorcli list search key archive"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-175-test-kotorcli-list-search-unsupported-extension-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list/search archive on KEY file (plan 176)

## Summary

Add integration tests for `list-archive` and `search-archive` when the archive path is a standalone `.key` file (not only BIF+KEY sibling paths). `ArchiveCommandHelpers.ReadArchiveResources` already supports `.key`; BIF+KEY list/search exists but KEY-only paths were untested.

## Requirements

- R1. `ListArchiveCommand.Execute` on a sample `.key` with sibling `.bif` exits zero and matches a known resref filter.
- R2. `SearchArchiveCommand.Execute` on the same `.key` exits zero for a wildcard matching the named entry.
- R3. Reuse `WriteSampleBifKeyPair` helper in `ArchiveCommandsTests.cs`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update `src/Tools/KotorCLI/README.md` test count.
