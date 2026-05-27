---
title: "test: kotorcli list-archive verbose and error paths"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-147-chore-kotorcli-pr7-sync-159-tests-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list-archive verbose and error paths (plan 148)

## Summary

Cover `list-archive --verbose` happy path and missing archive file error exit.

## Requirements

- R1. `ListArchiveCommand.Execute(..., verbose: true, ...)` exits 0 on sample RIM.
- R2. Missing archive path exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ListArchive`
