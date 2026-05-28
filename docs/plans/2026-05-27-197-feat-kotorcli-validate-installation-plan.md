---
title: "feat: kotorcli validate-installation command"
type: feat
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-196-feat-kotorcli-launch-k2-path-env-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI validate-installation (plan 197)

## Summary

Wire a **`validate-installation`** CLI command that calls `BioWare.Tools.Validation.ValidateInstallation`, matching PyKotor `validation.validate_installation` for Holocron parity.

## PyKotor / Holocron parity

| Surface | PyKotor (`validation.py`) | C# (Andastra) | CLI |
| --- | --- | --- | --- |
| Entry | `validate_installation(installation, *, check_essential_files=True)` | `Validation.ValidateInstallation(installation, checkEssentialFiles=True)` | `validate-installation` |
| Essential 2DAs | `appearance`, `baseitems`, `classes`, `genericdoors` | Same list in `Validation.cs` | Default on; `--no-essential` skips |
| Result shape | `ValidationResult`: `valid`, `missing_files`, `errors` | `ValidationResult`: `Valid`, `MissingFiles`, `Errors` | Log errors + missing; exit code |
| Path missing | Adds error when install path absent | Same in `ValidateInstallation` | Non-zero |

PyKotor uses keyword `check_essential_files`; CLI flag **`--no-essential`** inverts the default (essential check enabled).

## Requirements

- R1. Command `validate-installation` with required `--installation`.
- R2. Optional `--no-essential` — skip essential 2DA checks (default: check essential files).
- R3. Public `ExecuteValidateInstallation(installPath, checkEssentialFiles, logger)` for tests.
- R4. Exit **0** when valid; **1** on invalid path, missing essentials, or errors.
- R5. Log each `Errors` entry and each `MissingFiles` entry.
- R6. Three tests: valid minimal install, nonexistent path, missing essential 2DA.
- R7. Update README command list and test count (**276**).

## Verification

```bash
dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
