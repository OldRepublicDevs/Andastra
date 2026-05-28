---
title: "feat: kotordiff twoda memory cache save and restore"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-102-feat-kotordiff-strref-cache-persistence-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorDiff 2DA memory cache persistence (plan 103)

## Summary

Wire `TwoDAMemoryReferenceCache` serialization into KotorDiff `DiffCacheIO` save/load (deferred from plan 102).

## Requirements

- R1. `SaveDiffCache` accepts optional `TwoDAMemoryReferenceCache` and writes `twoda_cache_game` + `twoda_cache_data`.
- R2. `RestoreTwodaCacheFromCache` returns cache from `FromDict` when data present.
- R3. `TwoDAMemoryReferenceCache.Game` exposed read-only for save metadata.
- R4. `DiffCache.FromDict` normalizes `twoda_cache_data` like StrRef.
- R5. Tests: ToDict/FromDict round-trip; DiffCacheIO YAML save/load.

## Scope Boundaries

- No multi-installation nested twoda cache map persistence yet.

## Verification

- `dotnet build src/Tools/KotorDiff/KotorDiff.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDAMemoryReferenceCache`
- `dotnet test tests/KotorDiff.Tests/KotorDiff.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwodaCache`
