---
title: "chore: kotorcli pr7 sync 153 tests plan 144"
type: chore
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-144-test-kotorcli-search-archive-content-plan.md
branch: feat/holocron-port-phase-b
---

# chore: KotorCLI PR #7 sync to 153 tests (plan 145)

## Summary

After plan 144 (`search-archive --content` tests), sync README and PR #7 body to **153** passing `KotorCLI.Tests`.

## Requirements

- R1. README Known Issues test count → **153**; note content-search coverage.
- R2. PR #7 body: add plan 144 bullet; update test count to **153**.
- R3. Local full suite passes.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
