---
title: "test: kotorcli validate-installation no-essential and cli"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-27-197-feat-kotorcli-validate-installation-plan.md
branch: feat/holocron-port-phase-b
---

# test: validate-installation --no-essential + CLI (plan 198)

## Summary

Close gaps in plan 197 by testing **`--no-essential`** (PyKotor `check_essential_files=False`) and a **subprocess CLI** invocation of `validate-installation`.

## PyKotor / Holocron parity

| Flag | PyKotor | C# / CLI |
| --- | --- | --- |
| Skip essentials | `validate_installation(..., check_essential_files=False)` | `--no-essential` → `checkEssentialFiles=false` |
| With essentials | Default True | Default (no flag) |

When essentials are skipped, a valid install path with no `.2da` files should still pass.

## Requirements

- R1. Unit test: `ExecuteValidateInstallation(..., checkEssentialFiles: false)` on minimal dir (no 2DAs) → exit 0.
- R2. Unit test: same install with `checkEssentialFiles: true` → exit 1 (regression guard).
- R3. CLI subprocess test via `RunKotorCli`: `validate-installation --installation <dir> --no-essential` → exit 0.
- R4. README test count **278** (276 + 2).

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
```
