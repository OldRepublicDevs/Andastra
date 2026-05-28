---
title: "chore: kotorcli pr7 sync 159 tests plan 146"
type: chore
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-146-test-kotorcli-archive-helpers-filter-plan.md
branch: feat/holocron-port-phase-b
---

# chore: KotorCLI PR #7 sync to 159 tests (plan 147)

## Summary

After plan 146 (`ArchiveCommandHelpers` filter/content unit tests), sync README and PR #7 to **159** passing tests.

## Requirements

- R1. README test count → **159**.
- R2. PR #7 body: plan 146 bullet; test count **159**.
- R3. Full `KotorCLI.Tests` suite passes locally.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
