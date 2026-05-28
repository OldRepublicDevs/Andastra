---
title: "feat: find-strref --ncs-strref-min flag"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-094-feat-find-strref-no-ncs-flag-plan.md
branch: feat/holocron-port-phase-b
---

# feat: find-strref --ncs-strref-min (plan 095)

## Summary

Expose configurable NCS CONSTI StrRef candidate minimum via KotorCLI `find-strref --ncs-strref-min` and `ReferenceSearchOptions`, wired into StrRef cache indexing (see plan 093).

## Requirements

- R1. `ReferenceSearchOptions.NcsStrRefCandidateMinimum` (`int?`, null = default 100).
- R2. `NcsConstiScanner.IsPlausibleStrRefCandidate(value, minimum)` overload; existing no-arg uses `StrRefCandidateMinimum`.
- R3. `StrRefReferenceCache` constructor accepts optional minimum; `ScanNCS` uses it when indexing.
- R4. `FindAllStrRefReferences` passes options minimum when building cache.
- R5. `FindStrRefCommand` exposes `--ncs-strref-min` (>= 0); sets options (cache-oriented; slow path still exact-matches any CONSTI).
- R6. Tests: cache respects custom min; slow path finds small StrRef even with high `--ncs-strref-min`.

## Scope Boundaries

- No disk cache persistence; no OdyTools UI wiring yet.

## Verification

- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti|ReferenceCacheStrRef`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRef`
