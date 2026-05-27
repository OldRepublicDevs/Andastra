---
title: "test: kotorcli list-archive filter and empty path"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-155-test-kotorcli-search-archive-case-sensitive-happy-path-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list-archive filter and empty path (plan 156)

## Summary

Complete list-archive coverage with empty path validation and wildcard filter happy path.

## Requirements

- R1. Empty archive file path exits non-zero.
- R2. Wildcard filter matching a RIM resource exits zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ListArchive`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
