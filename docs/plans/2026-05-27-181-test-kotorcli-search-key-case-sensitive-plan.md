---
title: "test: kotorcli search key case sensitive"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-180-test-kotorcli-extract-empty-input-key-list-verbose-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive KEY case-sensitive name (plan 181)

## Summary

Add case-sensitive name search integration tests for standalone `.key` archives, mirroring MOD `ExecuteSearchArchive_ModCaseSensitiveName_*` tests. KEY list/search happy and no-match paths exist from plans 176–180; content search is N/A (KEY entries have no payload in list/read path).

## Requirements

- R1. Case-mismatched pattern with `--case-sensitive` exits non-zero on KEY file.
- R2. Exact-case pattern with `--case-sensitive` exits zero on KEY file.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **239**.
