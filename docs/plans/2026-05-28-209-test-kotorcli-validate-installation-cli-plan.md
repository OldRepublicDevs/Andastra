---
title: "test: kotorcli validate-installation cli subprocess"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-27-197-feat-kotorcli-validate-installation-plan.md
branch: feat/holocron-port-phase-b
---

# test: validate-installation CLI subprocess (plan 209)

## Summary

Expand **`validate-installation`** CLI subprocess coverage in `ValidationAndCatCommandsTests.cs`. Plan 197 added unit tests and one CLI test (`--no-essential`); this slice mirrors the remaining unit scenarios through `RunKotorCli`.

## Requirements

- R1. CLI subprocess: minimal install with essential 2DAs exits **0**.
- R2. CLI subprocess: nonexistent installation path exits **1**.
- R3. CLI subprocess: install missing essential 2DAs (default essential check) exits **1**.
- R4. Reuse `WriteEssentialTwoDAFiles` and temp install layout from existing validate tests.
- R5. README Known Issues test count **309**; note validate-installation CLI subprocess complete.
- R6. Close plan 152 (`status: completed`) — solution build verified on branch.

## Verification

```bash
dotnet build tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~ValidateInstallation"
```
