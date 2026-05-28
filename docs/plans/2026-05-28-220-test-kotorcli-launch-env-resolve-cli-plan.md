---
title: "test: kotorcli launch dry-run env path resolution cli"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-219-test-kotorcli-launch-resolve-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: launch dry-run env-based path resolution CLI (plan 220)

## Summary

Complete `ResolveGameBinary` CLI subprocess coverage for environment-variable install roots and K1-over-TSL preference (unit tests exist in `LaunchCommandTests`).

## Requirements

- R1. `KOTOR_PATH` + `chitin.key` + `swkotor.exe` → dry-run exit **0**, output contains resolved path.
- R2. `K1_PATH` fallback when `KOTOR_PATH` unset → same.
- R3. `K2_PATH` + `swkotor2.exe` → dry-run resolves TSL binary.
- R4. `--installDir` with both `swkotor.exe` and `swkotor2.exe` prefers K1.
- R5. README **351** tests.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~LaunchCommandCli"
```
