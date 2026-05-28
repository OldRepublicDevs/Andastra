---
title: "test: kotorcli search-archive bif key parity"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-140-feat-kotorcli-list-archive-bif-key-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive BIF+KEY parity (plan 141)

## Summary

Add test coverage proving `search-archive` finds KEY-named BIF resources after plan 140's shared `ReadArchiveResources` fix.

## Requirements

- R1. Test `SearchArchiveCommand.Execute` on BIF+`{stem}.key` pair matches `from_key*` pattern (exit 0).
- R2. No production changes unless test exposes a bug.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommands`
