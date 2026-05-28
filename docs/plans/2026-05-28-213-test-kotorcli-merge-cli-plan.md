---
title: "test: kotorcli merge cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-212-test-kotorcli-grep-diff-cat-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: merge CLI subprocess (plan 213)

## Summary

Add CLI subprocess tests for **`merge`** (GFF overlay). Unit coverage exists in `MergeGffCommandsTests`; CLI entry was untested.

## Requirements

- R1. `merge` CLI with `--output` overlays source onto target and exits **0**.
- R2. `merge` CLI with missing source exits **1**.
- R3. `MergeCommandCliTests.cs` with shared `RunKotorCli`.
- R4. README test count **325**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~MergeCommandCli"
```
