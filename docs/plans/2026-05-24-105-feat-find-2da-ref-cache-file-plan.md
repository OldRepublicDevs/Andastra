---
title: "feat: find-2da-ref cache file load and save"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-104-feat-find-strref-cache-file-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI find-2da-ref cache file (plan 105)

## Summary

Mirror plan 104 for `find-2da-ref`: persist and reuse `TwoDAMemoryReferenceCache` via `--cache-file`.

## Requirements

- R1. `TwoDAMemoryReferenceCacheIO` saves/loads JSON with `game` + `ToDict()` payload.
- R2. `ReferenceCacheHelpers.BuildTwoDAMemoryReferenceCache` extracts installation scan loop.
- R3. `find-2da-ref --cache-file PATH` loads or builds+saves cache; `--rebuild-cache` forces rescan.
- R4. `TwoDAMemoryReferenceCache.FromDict` accepts `locations` as `List<string>` or JSON arrays.
- R5. README documents new flags.
- R6. Tests: BioWare IO round-trip; KotorCLI cache reuse without rescan.

## Scope Boundaries

- Plan 070 grep/format tests already landed; no FAC work in this slice.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDAMemoryReferenceCacheIO`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CacheFile`
