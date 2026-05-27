---
title: "chore: kotorcli pr7 archive bif key pr refresh"
type: chore
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-142-test-kotorcli-resolve-sibling-key-path-plan.md
branch: feat/holocron-port-phase-b
---

# chore: KotorCLI PR #7 archive BIF+KEY PR refresh (plan 143)

## Summary

Sync README and PR #7 body after plans 140–142 (list-archive/search-archive BIF+KEY + ResolveSiblingKeyPath tests). Confirm **151** KotorCLI tests locally.

## Requirements

- R1. README Known Issues / Next Steps reflect archive BIF+KEY coverage and **151** test count.
- R2. PR #7 body adds plans 140–142 archive closure section and updated test count.
- R3. Local verification: full `KotorCLI.Tests` pass.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
