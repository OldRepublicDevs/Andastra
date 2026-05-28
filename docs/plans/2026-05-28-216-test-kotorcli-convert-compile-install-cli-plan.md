---
title: "test: kotorcli convert compile install cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-215-test-kotorcli-pack-unpack-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: convert, compile, and install CLI subprocess (plan 216)

## Summary

Complete build-pipeline CLI subprocess coverage for **`convert`**, **`compile`**, and **`install`** (unit tests exist; CLI untested).

## Requirements

- R1. `convert default` on `*.json` source writes binary GFF alongside JSON (exit **0**).
- R2. `convert missing-target` exits **1**.
- R3. `compile default` with no NSS sources exits **0**.
- R4. `install default --installDir <path> --noPack` copies existing `test.mod` to `modules/` when `chitin.key` present.
- R5. `install` to directory without `chitin.key` exits **1**.
- R6. Extend `BuildPipelineCommandCliTests.cs`.
- R7. README test count **337**; build-pipeline CLI subprocess suite complete.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~BuildPipelineCommandCli"
```
