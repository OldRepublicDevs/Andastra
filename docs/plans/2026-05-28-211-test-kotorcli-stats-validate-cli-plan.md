---
title: "test: kotorcli stats and validate cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-210-test-kotorcli-check-txi-2da-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: stats and validate CLI subprocess (plan 211)

## Summary

Add CLI subprocess tests for **`stats`** and **`validate`** (plans 189–195 added unit tests only). Fix `ExecuteStats` to exit non-zero when `FileStats.IsValid` is false (parity with `validate`).

## Requirements

- R1. `stats` CLI on sample UTC exits **0**.
- R2. `stats` CLI on missing file exits **1**.
- R3. `validate` CLI on sample UTC exits **0**.
- R4. `validate` CLI on missing file exits **1**.
- R5. `StatsValidateCommandCliTests.cs` with shared `RunKotorCli`.
- R6. README test count **317**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
