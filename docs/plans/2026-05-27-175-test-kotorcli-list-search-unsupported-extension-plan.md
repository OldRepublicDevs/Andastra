---
title: "test: kotorcli list search archive unsupported extension"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-174-test-kotorcli-extract-rim-baseline-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list/search archive unsupported extension (plan 175)

## Summary

Add integration tests for `list-archive` and `search-archive` when given a non-archive file extension, mirroring `ExecuteExtract_UnsupportedExtension_ExitsNonZero` in `ExtractCommandTests.cs`. Archive happy paths are closed through plan 174; this covers fail-fast error paths for list/search helpers.

## Requirements

- R1. `ListArchiveCommand.Execute` with a `.txt` file exits non-zero.
- R2. `SearchArchiveCommand.Execute` with a `.txt` file and a pattern exits non-zero.
- R3. Use the same temp-file setup pattern as extract unsupported-extension test.

## Implementation

- Add tests to `ArchiveCommandsTests.cs` near existing list error-path tests (`ExecuteListArchive_MissingFile_ExitsNonZero`).
- Write a plain text file, call list/search execute methods, assert exit code `1`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update `src/Tools/KotorCLI/README.md` test count to match passing total.
