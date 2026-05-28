---
title: "test: kotorcli stats and validate on erf"
type: test
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-191-test-kotorcli-stats-validate-2da-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI stats and validate on ERF (plan 192)

## Summary

Extend `stats` / `validate` integration coverage to **ERF** archives (`.erf`), using existing `ERFStatsAnalyzer` / `ValidateERFStructure` in `UtilityCommands.cs`.

## PyKotor / Holocron parity

| Surface | PyKotor / Holocron | C# (Andastra) | Test assertion |
| --- | --- | --- | --- |
| Archive stats | PyKotor `ERF` reader lists resource count/types | `ERFStatsAnalyzer` → resource counts, sizes, ERF type | `ExecuteStats` on `.erf` exits 0 |
| Validation | Holocron module/ERF workflows expect readable ERF headers + resources | `ValidateERFStructure` via `ExecuteValidate` | `ExecuteValidate` on valid `.erf` exits 0 |
| Fixture | Archive tests build minimal ERF with UTC + GFF entries | Mirror `CreateSampleErf` from `ArchiveCommandsTests.cs` | Same two-resource ERF used in list/extract tests |

Upstream: PyKotor `pykotor/resource/formats/erf/`; Holocron treats ERF/MOD as encapsulated resource containers — C# mirrors via `BioWare.Resource.Formats.ERF`.

## Requirements

- R1. `ExecuteStats` on a minimal valid `.erf` exits 0.
- R2. `ExecuteValidate` on the same `.erf` exits 0.
- R3. Helper `WriteSampleErf` in `UtilityCommandsTests.cs` (no cross-test-file dependency).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- README test count **265**.
