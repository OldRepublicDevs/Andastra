---
title: "test: kotorcli search-archive case-sensitive"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-152-fix-odytools-standalone-reference-helpers-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive case-sensitive mode (plan 153)

## Summary

Integration tests for `search-archive --case-sensitive` on resource names and `--content` payload matching.

## Requirements

- R1. Case-sensitive name filter rejects case-mismatched wildcard matches that would match when insensitive.
- R2. Case-sensitive content search rejects payload patterns that differ only by case.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SearchArchive`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
