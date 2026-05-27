---
title: "feat: NCS CONSTI plausible StrRef candidate filter"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-086-feat-ncs-strref-reference-cache-plan.md
branch: feat/holocron-port-phase-b
---

# feat: NCS CONSTI StrRef disambiguation (plan 093)

## Summary

Reduce NCS false positives in `StrRefReferenceCache` by indexing only CONSTI values at or above a plausible StrRef minimum. Exact StrRef searches without cache still match any CONSTI literal.

## Requirements

- R1. `NcsConstiScanner.IsPlausibleStrRefCandidate(int value)` with documented minimum threshold.
- R2. `StrRefReferenceCache.ScanNCS` skips non-plausible CONSTI when building the cache.
- R3. Slow-path `ScanNCSForStrRef` / `ExtractConstiOffsetsForValue` unchanged (exact match for queried StrRef).
- R4. Tests: cache excludes small CONSTI; slow-path still finds small StrRef literal.

## Scope Boundaries

- No disk cache persistence; no bytecode context analysis.

## Verification

- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConstiScanner`
