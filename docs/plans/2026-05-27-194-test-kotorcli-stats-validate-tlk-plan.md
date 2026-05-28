---
title: "test: kotorcli stats and validate on tlk"
type: test
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-193-test-kotorcli-stats-validate-bif-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI stats and validate on TLK (plan 194)

## Summary

Extend `stats` / `validate` integration coverage to **TLK** talk tables (`.tlk`), using `TLKFileStats` / `ValidateTLKStructure` in `UtilityCommands.cs`.

## PyKotor / Holocron parity

| Surface | PyKotor / Holocron | C# (Andastra) | Test assertion |
| --- | --- | --- | --- |
| TLK stats | PyKotor `TLK` reader; Holocron dialog/strref tooling | `TLKStatsAnalyzer` (`TLK V3.0` magic) | `ExecuteStats` on `.tlk` exits 0 |
| Validation | Entry counts / usage in toolset TLK views | `ValidateTLKStructure` via `ExecuteValidate` | `ExecuteValidate` on valid `.tlk` exits 0 |
| Fixture | Format-convert integration tests build English TLK | `WriteSampleTlk` mirrors `FormatConvertIntegrationTests` | One non-empty text entry |

Upstream: PyKotor `pykotor/resource/formats/tlk/`; Holocron TLK editor — C# mirrors via `BioWare.Resource.Formats.TLK`.

## Requirements

- R1. `ExecuteStats` on a minimal valid `.tlk` exits 0.
- R2. `ExecuteValidate` on the same `.tlk` exits 0.
- R3. `WriteSampleTlk` helper in `UtilityCommandsTests.cs`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- README test count **269**.
