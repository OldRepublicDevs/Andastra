---
title: "refactor: dedupe NCS CONSTI scanner into BioWare"
type: refactor
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-086-feat-ncs-strref-reference-cache-plan.md
branch: feat/holocron-port-phase-b
---

# refactor: KotorDiff NCS CONSTI dedupe (plan 087)

## Summary

Route KotorDiff `ExtractNcsConstiOffsets` through BioWare `NcsConstiScanner` and document NCS coverage in KotorCLI README.

## Requirements

- R1. `ReferenceAnalyzers.ExtractNcsConstiOffsets` delegates to `NcsConstiScanner.ExtractConstiOffsetsForValue` (remove duplicated bytecode walk).
- R2. KotorCLI README `find-strref` row mentions NCS CONSTI scanning.

## Scope Boundaries

- No behavior change to diff analyzer outputs beyond shared scanner parity.

## Verification

- `dotnet build src/Tools/KotorDiff/KotorDiff.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConstiScanner`
