---
title: "test: kotorcli bif verbose and case sensitive search"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-182-test-kotorcli-bif-list-search-edge-cases-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI BIF+KEY verbose list and case-sensitive search (plan 183)

## Summary

Complete BIF+KEY list/search parity with KEY-only paths: verbose list mode and case-sensitive name search match/reject.

## Requirements

- R1. `ListArchiveCommand.Execute` on BIF with sibling KEY and `--verbose` exits zero.
- R2. Case-mismatched search pattern with `--case-sensitive` exits non-zero.
- R3. Exact-case search pattern with `--case-sensitive` exits zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **245**.
