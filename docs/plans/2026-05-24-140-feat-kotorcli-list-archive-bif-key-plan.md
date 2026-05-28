---
title: "feat: kotorcli list-archive bif key name resolution"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-139-chore-kotorcli-pr7-merge-readiness-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI list-archive BIF+KEY name resolution (plan 140)

## Summary

Fix `ArchiveCommandHelpers.ReadArchiveResources` for `.bif` to resolve sibling KEY files (`chitin.key` or `{stem}.key`) and delegate to `ArchiveHelpers.ListBif`, matching extract behavior.

## Requirements

- R1. BIF listing uses KEY when `chitin.key` or `{bif-stem}.key` exists beside the BIF.
- R2. Fallback to numeric names when no KEY found (existing behavior preserved).
- R3. Integration test: `ListArchiveCommand.Execute` on BIF+KEY pair lists `from_key*` filter successfully.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ArchiveCommands`
