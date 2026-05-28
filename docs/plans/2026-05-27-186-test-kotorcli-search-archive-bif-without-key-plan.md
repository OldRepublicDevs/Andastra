---
title: "test: kotorcli search-archive bif without sibling key"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-185-test-kotorcli-bif-list-without-key-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive BIF without sibling KEY (plan 186)

## Summary

Add integration tests for `search-archive` on a standalone `.bif` with no sibling KEY. Without KEY merge, name search uses blank ResRefs and extension-only display names (e.g. `.utc`), matching `list-archive` plan 185 behavior.

## Requirements

- R1. `SearchArchiveCommand.Execute` with extension wildcard (e.g. `*.utc`) exits 0 on BIF without KEY.
- R2. Resref-only pattern (e.g. `from_key*`) exits non-zero without KEY.
- R3. Non-matching wildcard pattern exits non-zero without KEY.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **252**.
