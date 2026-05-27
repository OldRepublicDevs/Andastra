---
title: "chore: kotorcli pr7 merge readiness gate"
type: chore
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-138-feat-kotorcli-pipeline-config-rename-plan.md
branch: feat/holocron-port-phase-b
---

# chore: KotorCLI PR #7 merge readiness gate (plan 139)

## Summary

Refresh KotorCLI README and PR #7 description to reflect build-pipeline test closure (146 tests), then verify locally before CI completes.

## Requirements

- R1. Update `src/Tools/KotorCLI/README.md` Known Issues / Next Steps — build-pipeline commands now have automated coverage; note remaining gaps (`launch`, BIF listing).
- R2. Refresh PR #7 body with plans 114–138 slice summary and current test command (`146` KotorCLI tests).
- R3. Local gate: `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -m:1` and full `KotorCLI.Tests`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
