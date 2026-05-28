---
title: "test: kotorcli bif list search edge cases"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-179-test-kotorcli-list-search-key-no-match-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI BIF+KEY list/search edge cases (plan 182)

## Summary

Extend BIF+KEY archive list/search coverage to match KEY-only paths from plans 176–181: list without filter, filter no-match, and search no-match.

## Requirements

- R1. `ListArchiveCommand.Execute` on BIF with sibling KEY and no filter exits zero.
- R2. BIF list with non-matching filter exits non-zero.
- R3. BIF search with non-matching pattern exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **242**.
