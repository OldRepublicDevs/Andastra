---
title: "chore: kotorcli pr7 sync 161 tests plan 148"
type: chore
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-148-test-kotorcli-list-archive-verbose-plan.md
branch: feat/holocron-port-phase-b
---

# chore: KotorCLI PR #7 sync to 161 tests (plan 149)

## Summary

After plan 148 (`list-archive` verbose + missing-file tests), sync README and PR #7 to **161** passing tests.

## Requirements

- R1. README test count → **161**.
- R2. PR #7: plan 148 bullet; test count **161**.
- R3. Full suite passes locally.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
