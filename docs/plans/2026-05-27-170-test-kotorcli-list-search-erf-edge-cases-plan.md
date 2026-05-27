---
title: "test: kotorcli list search erf edge cases"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-169-test-kotorcli-list-search-archive-erf-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list/search ERF edge cases (plan 170)

## Summary

Negative-path and verbose parity tests for `list-archive` and `search-archive` on ERF archives, mirroring MOD coverage from plans 164–166 after plan 169 added ERF happy paths.

## Requirements

- R1. `list-archive --verbose` exits zero on ERF.
- R2. `list-archive --filter` with no matches exits non-zero on ERF.
- R3. `search-archive` with no wildcard match exits non-zero on ERF.
- R4. `search-archive --case-sensitive` rejects case mismatch on ERF resource names.
- R5. `search-archive --case-sensitive` name match with exact case exits zero on ERF.
- R6. `search-archive --case-sensitive --content` rejects payload case mismatch on ERF.
- R7. `search-archive --case-sensitive --content` matches exact-case payload strings on ERF.
- R8. `search-archive` with content disabled skips payload-only matches on ERF (exits non-zero).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommands`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
