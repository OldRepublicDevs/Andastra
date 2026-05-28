---
title: "test: kotorcli stats and validate on bif"
type: test
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-192-test-kotorcli-stats-validate-erf-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI stats and validate on BIF (plan 193)

## Summary

Extend `stats` / `validate` integration coverage to **BIF** archives (`.bif`), using `BIFFileStats` / `ValidateBIFStructure` in `UtilityCommands.cs`.

## PyKotor / Holocron parity

| Surface | PyKotor / Holocron | C# (Andastra) | Test assertion |
| --- | --- | --- | --- |
| BIF stats | PyKotor `BIF` reader (`BIFFV1  ` header); Holocron chitin inspection | `BIFStatsAnalyzer` (magic aligned with `BIFBinaryReader`) | `ExecuteStats` on `.bif` exits 0 |
| Validation | Holocron chitin/BIF inspection workflows | `ValidateBIFStructure` via `ExecuteValidate` | `ExecuteValidate` on valid `.bif` exits 0 |
| Fixture | Key-pack / archive tests use named ResRef in BIF | `WriteSampleBif` mirrors `KeyPackCommandTests` | Single UTC resource with embedded ResRef |

Upstream: PyKotor `pykotor/resource/formats/bif/`; Holocron lists BIF variable resources — C# mirrors via `BioWare.Resource.Formats.BIF`.

## Requirements

- R1. `ExecuteStats` on a minimal valid `.bif` exits 0.
- R2. `ExecuteValidate` on the same `.bif` exits 0.
- R3. `WriteSampleBif` helper in `UtilityCommandsTests.cs`.
- R4. `BIFStatsAnalyzer.CanAnalyze` accepts `BIFFV1  ` magic (fix drift from `BIFFV1.0`).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- README test count **267**.
