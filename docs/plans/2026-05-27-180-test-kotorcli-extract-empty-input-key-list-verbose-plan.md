---
title: "test: kotorcli extract empty input and key list verbose"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-179-test-kotorcli-list-search-key-no-match-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract empty input and KEY list verbose (plan 180)

## Summary

Close two archive helper gaps: `extract` with empty `--file` path (mirroring list/search empty-path tests), and `list-archive --verbose` on standalone KEY file (mirroring MOD verbose coverage).

## Requirements

- R1. `ExtractCommand.Execute` with empty input file path exits non-zero.
- R2. `ListArchiveCommand.Execute` on KEY with verbose flag exits zero for known entry.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **237**.
