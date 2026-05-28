---
title: "test: kotorcli launch alias cli install-only"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-202-feat-kotorcli-launch-install-only-plan.md
branch: feat/holocron-port-phase-b
---

# test: launch alias CLI install-only (plan 203)

## Summary

Add subprocess CLI tests proving **`launch`**, **`serve`**, **`play`**, and **`test`** honor **`--install-only`** through the real `dotnet exec KotorCLI.dll` entry point (plan 202 unit tests used `LaunchCommand.Execute` directly).

## PyKotor / Holocron parity

Same four aliases as plan 199; install-only path must work from parsed CLI options, not only direct `Execute` calls.

## Requirements

- R1. Parameterized CLI test: `{alias} default --install-only --installDir <fake>` from a temp kotorcli project → exit 0.
- R2. Assert `modules/<target>.mod` exists under fake install dir after run.
- R3. `RunKotorCli` accepts optional working directory for project-scoped commands.
- R4. README test count note **289** (285 + 4 alias cases).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~LaunchCommandCli"
```

## Out of scope

- Spawning the game executable (plan 204+).
