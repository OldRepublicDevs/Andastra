---
title: "test: kotorcli config and launch resolve edge cli"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-220-test-kotorcli-launch-env-resolve-cli-plan.md
branch: feat/holocron-port-phase-b
---

# test: config CLI unset/list and launch resolve edge cases (plan 221)

## Summary

Close remaining unit-test gaps without CLI mirrors: `config --unset`/`--list`, launch invalid `--gameBin` fallback, TSL-only install dir, and env path without `chitin.key`.

## Requirements

- R1. `CliConfig_LocalUnset_RemovesKey` — set then `--unset`.
- R2. `CliConfig_LocalListEmpty_ExitsZero` — dummy key/value + `--list`.
- R3. `CliLaunch_DryRun_InvalidGameBin_FallsBackToInstallDir`.
- R4. `CliLaunch_DryRun_InstallDirOnlyTsl_ResolvesSwkotor2`.
- R5. `CliLaunch_DryRun_KotorPathWithoutChitin_ExitsNonZero`.
- R6. README **356** tests.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
