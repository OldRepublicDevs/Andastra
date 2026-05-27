---
title: "feat: find-strref --no-ncs flag"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-093-feat-ncs-consti-strref-disambiguation-plan.md
branch: feat/holocron-port-phase-b
---

# feat: find-strref --no-ncs (plan 094)

## Summary

Add `--no-ncs` to KotorCLI `find-strref` to skip NCS CONSTI bytecode scanning (GFF/2DA/SSF only).

## Requirements

- R1. `ReferenceSearchOptions.IncludeNcsStrRefScan` (default true).
- R2. `FindStrRefReferences` slow path skips NCS when false; cache path filters `offset_` locations.
- R3. `FindStrRefCommand` exposes `--no-ncs` and passes option through.
- R4. Tests: NCS hit without flag; no NCS hit with `--no-ncs` on SSF-only fixture.

## Scope Boundaries

- No disk cache persistence; no configurable `--ncs-strref-min` yet.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRef`
