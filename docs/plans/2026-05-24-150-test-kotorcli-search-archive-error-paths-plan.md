---
title: "test: kotorcli search-archive error paths"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-148-test-kotorcli-list-archive-verbose-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive error paths (plan 150)

## Summary

Mirror plan 148 list-archive coverage: test `search-archive` missing archive file and empty search pattern exits.

## Requirements

- R1. Missing archive path exits non-zero.
- R2. Empty search pattern exits non-zero (existing RIM fixture).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SearchArchive`
