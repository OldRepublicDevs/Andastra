---
title: "test: kotorcli stats and validate on ncs"
type: test
status: completed
date: 2026-05-27
origin: docs/plans/2026-05-27-194-test-kotorcli-stats-validate-tlk-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI stats and validate on NCS (plan 195)

## Summary

Complete the `stats` / `validate` format coverage sweep with **NCS** compiled scripts (`.ncs`), using `NCSFileStats` / `ValidateNCSStructure` in `UtilityCommands.cs`.

## PyKotor / Holocron parity

| Surface | PyKotor / Holocron | C# (Andastra) | Test assertion |
| --- | --- | --- | --- |
| NCS stats | PyKotor `NCS` decompiler/stats; Holocron script panels | `NCSStatsAnalyzer` (`NCS V1.0` magic) | `ExecuteStats` on `.ncs` exits 0 |
| Validation | Instruction sanity in toolset script tools | `ValidateNCSStructure` via `ExecuteValidate` | `ExecuteValidate` on valid `.ncs` exits 0 |
| Fixture | KotorCLI / BioWare tests compile minimal NSS | `NCSAuto.CompileNss` + `WriteNcs` (K1) | Mirrors `ScriptToolCommandsTests` |

Upstream: PyKotor `pykotor/resource/formats/ncs/`; Holocron NSS/NCS workflow — C# mirrors via `BioWare.Resource.Formats.NCS`.

## Requirements

- R1. `ExecuteStats` on a minimal valid `.ncs` exits 0.
- R2. `ExecuteValidate` on the same `.ncs` exits 0.
- R3. `WriteSampleNcs` helper compiles `void main() { int n = 42; }` for K1.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- README test count **271**; note stats/validate closure for wired analyzers.
