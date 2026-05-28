---
title: "test: kotorcli pack and unpack cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-214-test-kotorcli-build-pipeline-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: pack and unpack CLI subprocess (plan 215)

## Summary

Extend build-pipeline CLI coverage with **`unpack`** and **`pack`** subprocess tests (unit tests in `UnpackCommandTests` / `PackCommandTests`).

## Requirements

- R1. `unpack default <mod>` with `--removeDeleted` writes creature JSON under rules path (exit **0**).
- R2. `pack default --noConvert --noCompile` with pre-populated cache writes `test.mod` (exit **0**).
- R3. `pack missing-target` exits **1**.
- R4. Extend `BuildPipelineCommandCliTests.cs`.
- R5. README test count **332**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~BuildPipelineCommandCli"
```
