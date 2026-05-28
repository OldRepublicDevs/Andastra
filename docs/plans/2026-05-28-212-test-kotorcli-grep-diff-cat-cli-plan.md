---
title: "test: kotorcli grep diff cat cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-211-test-kotorcli-stats-validate-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: grep, diff, and cat CLI subprocess (plan 212)

## Summary

Add CLI subprocess tests for file utilities **`grep`**, **`diff`**, and **`cat`** (unit tests exist in `UtilityCommandsTests` and `CatCommandTests` only).

## Requirements

- R1. `grep` CLI: match exits **0**, no match exits **1**, missing file exits **1**.
- R2. `diff` CLI: identical files exit **0**, different files exit **1**.
- R3. Register `CatCommand` on root CLI (`Program.cs` — was missing).
- R4. `cat` CLI: read UTC from RIM exits **0**.
- R5. New `GrepDiffCatCommandCliTests.cs`.
- R6. README test count **323**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~GrepDiffCatCommandCli"
```
