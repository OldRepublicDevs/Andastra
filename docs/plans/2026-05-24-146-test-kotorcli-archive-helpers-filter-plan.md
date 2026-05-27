---
title: "test: kotorcli archive command helpers filter tests"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-144-test-kotorcli-search-archive-content-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI ArchiveCommandHelpers filter/content tests (plan 146)

## Summary

Unit-test `ArchiveCommandHelpers.MatchesFilter` and `ContentMatches` used by list/search-archive.

## Requirements

- R1. `MatchesFilter`: empty pattern matches; wildcard case-insensitive; case-sensitive substring mode.
- R2. `ContentMatches`: UTF-8 substring find; null/empty data returns false; case-insensitive default path.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommandHelpers`
