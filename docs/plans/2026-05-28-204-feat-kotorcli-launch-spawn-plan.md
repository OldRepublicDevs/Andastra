---
title: "feat: kotorcli launch install and spawn game"
type: feat
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-202-feat-kotorcli-launch-install-only-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI launch install + spawn (plan 204)

## Summary

Complete the Holocron `launch` workflow: run **`install`**, then start the resolved game executable with the installation directory as the working directory. Optional **`--wait`** returns the game process exit code for tests and automation.

## Requirements

- R1. Full launch (no `--dry-run`, no `--install-only`) calls `InstallCommand.Execute` first; abort if install fails.
- R2. `TryStartGameProcess` starts `Process` with `UseShellExecute = false` and working directory = resolved install root (or executable directory fallback).
- R3. Default: fire-and-forget (exit 0 after successful start). With `--wait`, wait and propagate process exit code.
- R4. `--install-only` and `--dry-run` behavior unchanged.
- R5. Unit tests: `TryStartGameProcess` with executable shell stub; full launch installs mod and runs stub with `--wait`.
- R6. README: launch no longer spawn-stub; document `--wait`.

## Out of scope

- Command-line module load arguments to the game (KOTOR loads from `modules/` after install).
- Real swkotor.exe integration tests.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~LaunchCommand"
```
