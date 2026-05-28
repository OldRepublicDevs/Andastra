---
title: "test: kotorcli stats and validate on 2da"
type: test
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-190-test-kotorcli-check-2da-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI stats and validate on 2DA (plan 191)

## Summary

Extend `stats` / `validate` integration test coverage from GFF (plan 189) to **2DA** files, using existing `TwoDAStatsAnalyzer` / `ValidateTwoDAStructure` in `UtilityCommands.cs`.

## PyKotor / Holocron parity

| Surface | PyKotor / Holocron | C# (Andastra) | Test assertion |
| --- | --- | --- | --- |
| Format stats | Holocron resource inspectors show row/column counts for 2DA tables | `TwoDAStatsAnalyzer` → row/column/cell metrics | `ExecuteStats` on `.2da` exits 0 |
| Validation | Table structure sanity (dimensions, fill) in toolset workflows | `ValidateTwoDAStructure` via `ExecuteValidate` | `ExecuteValidate` on valid `.2da` exits 0 |
| CLI wiring | PyKotor library-level; KotorCLI adds `stats`/`validate` commands | `UtilityCommands.ExecuteStats` / `ExecuteValidate` (public, plan 189) | Same exit-code pattern as GFF tests |

Upstream: PyKotor `TwoDA` readers in `pykotor/resource/formats/twoda/`; Holocron 2DA editor uses row/column metadata — C# mirrors via `BioWare.Resource.Formats.TwoDA`.

## Requirements

- R1. `ExecuteStats` on a minimal valid `.2da` exits 0.
- R2. `ExecuteValidate` on the same `.2da` exits 0.
- R3. Reuse `WriteSampleTwoDA` helper pattern from format-convert tests.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- README test count **263** (261 after plan 190 structure test + 2 new tests).
