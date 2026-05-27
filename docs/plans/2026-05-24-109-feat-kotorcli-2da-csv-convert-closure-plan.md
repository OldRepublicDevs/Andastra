---
title: "feat: kotorcli 2da csv convert closure"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-070-feat-pykotor-port-residuals-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI 2DA CSV convert closure (plan 109)

## Summary

Add integration tests for wired `2da2csv` / `csv22da` commands and mark them implemented in KotorCLI README (follow-on to plan 106 GFF JSON closure).

## Requirements

- R1. Integration test: `2da2csv` writes non-empty CSV from a minimal 2DA fixture.
- R2. Integration test: `csv22da` after `2da2csv` round-trips row label or cell value.
- R3. README marks `2da2csv`/`csv22da` as wired (not stub).

## Scope Boundaries

- No changes to `Conversions.cs` unless tests expose a bug.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvertIntegration`
