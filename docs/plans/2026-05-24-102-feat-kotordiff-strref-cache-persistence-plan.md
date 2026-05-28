---
title: "feat: kotordiff strref cache save and restore"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-086-feat-ncs-strref-reference-cache-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorDiff StrRef cache persistence (plan 102)

## Summary

Wire `StrRefReferenceCache` serialization into KotorDiff `DiffCacheIO` save/load (deferred from plan 086).

## Requirements

- R1. `SaveDiffCache` accepts `StrRefReferenceCache` and writes `strref_cache_game` + `strref_cache_data` via `ToDict()`.
- R2. `RestoreStrrefCacheFromCache` returns `StrRefReferenceCache` from `FromDict` when cache data present.
- R3. `StrRefReferenceCache.Game` exposed read-only for save metadata.
- R4. Tests: `ToDict`/`FromDict` round-trip; DiffCacheIO restore from in-memory `DiffCache`.

## Scope Boundaries

- No TwoDAMemoryReferenceCache persistence; no find-strref CLI cache flags.

## Verification

- `dotnet build src/Tools/KotorDiff/KotorDiff.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReferenceCache`
- `dotnet test tests/KotorDiff.Tests/KotorDiff.Tests.csproj --framework net9.0 --filter FullyQualifiedName~DiffCacheIO`
