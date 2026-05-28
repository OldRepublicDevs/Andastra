---
title: "feat: kotorcli create-archive command test closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-117-feat-kotorcli-list-command-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI create-archive command test closure (plan 118)

## Summary

Expand archive pipeline test coverage for wired `create-archive` beyond the existing RIM happy path.

## Requirements

- R1. Test: missing input directory exits non-zero.
- R2. Test: packing a directory to MOD produces a readable archive with expected resource.
- R3. Test: unsupported output archive type exits non-zero.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CreateArchiveCommand`
