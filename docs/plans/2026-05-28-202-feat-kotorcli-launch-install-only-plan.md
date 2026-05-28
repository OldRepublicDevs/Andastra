---
title: "feat: kotorcli launch --install-only"
type: feat
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI launch --install-only (plan 202)

## Summary

Expose the install step of the Holocron `launch` workflow on KotorCLI without resolving or spawning the game executable. `launch`, `serve`, `play`, and `test` accept `--install-only` and delegate to `InstallCommand.Execute`.

## PyKotor / Holocron parity

Holocron launch runs convert/compile/pack/install before starting the game. This slice wires step 1 only; full spawn remains deferred.

## Requirements

- R1. `--install-only` on all launch aliases (`launch`, `serve`, `play`, `test`).
- R2. When `--install-only` is set, call `InstallCommand.Execute(targets, installDir, noPack: false, clean: false)` and return its exit code.
- R3. Do not require `ResolveGameBinary` when `--install-only` is set.
- R4. When both `--install-only` and `--dry-run` are set, **install-only wins** (documented).
- R5. Without `--install-only`, existing dry-run and fail-fast stub behavior unchanged.
- R6. README documents `--install-only` and updates launch stub wording.

## Implementation units

- U1. `LaunchCommand.cs` — option + `Execute(..., installOnly)` overload.
- U2. `LaunchCommandTests.cs` — happy path install-only, no-config failure, install-only without game binary.
- U3. `README.md` — launch flags and known-gaps line.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~LaunchCommand"
```
