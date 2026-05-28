---
title: "feat: kotorcli launch resolves k2_path env"
type: feat
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-195-test-kotorcli-stats-validate-ncs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI launch K2_PATH resolution (plan 196)

## Summary

Extend `LaunchCommand` installation discovery with **`K2_PATH`**, matching Holocron toolset and Andastra `GamePathDetector` env conventions for TSL installs.

## PyKotor / Holocron parity

| Surface | Holocron / vendor | C# (Andastra) | Test assertion |
| --- | --- | --- | --- |
| K1 install env | `KOTOR_PATH`, `K1_PATH` + `chitin.key` | Already in `DetermineInstallationDirectory` | Existing `LaunchCommandTests` |
| TSL install env | `K2_PATH` + `chitin.key` (vendor tests) | Add after `K1_PATH` in env chain | `ResolveGameBinary` → `swkotor2.exe` |
| Launch stub | Full pipeline not in PyKotor CLI quickstart | Fail-fast except `--dry-run` | Unchanged; dry-run exit 0 |

Holocron reference: `vendor/tests/conftest.py`, `HTInstallation(K2_PATH, ...)`; Andastra `GamePathDetector.TryEnvironmentVariable` uses `K2_PATH` for K2.

## Requirements

- R1. `DetermineInstallationDirectory` checks `K2_PATH` when `KOTOR_PATH` / `K1_PATH` unset.
- R2. Requires `chitin.key` under the env directory (same as K1 paths).
- R3. `ResolveGameBinary` returns `swkotor2.exe` when only TSL binary present.
- R4. Integration test: `Execute` dry-run with `K2_PATH` exits 0.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- README test count **273**.
