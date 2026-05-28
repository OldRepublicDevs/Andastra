---
title: "test: kotorcli init config list cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-213-test-kotorcli-merge-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: init, config, and list CLI subprocess (plan 214)

## Summary

Start build-pipeline CLI subprocess coverage with **`init`**, **`config`**, and **`list`** (unit tests exist; CLI untested).

## Requirements

- R1. `init . . --default --vcs none` in empty project dir creates `kotorcli.cfg` (exit **0**); treat `.` file positional as no unpack source.
- R2. `config <key> <value> --local` writes `.kotorcli/user.cfg` (exit **0**).
- R3. `list` in initialized project exits **0**.
- R4. `list` outside a package exits **1**.
- R5. `BuildPipelineCommandCliTests.cs` with cwd-aware `RunKotorCli`.
- R6. README test count **329**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~BuildPipelineCommandCli"
```
