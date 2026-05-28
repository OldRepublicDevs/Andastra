---
title: "feat: find-strref cache file load and save"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-102-feat-kotordiff-strref-cache-persistence-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI find-strref cache file (plan 104)

## Summary

Add `--cache-file` (and optional `--rebuild-cache`) to KotorCLI `find-strref` so installation StrRef scans can be persisted and reused (deferred from plan 102).

## Requirements

- R1. `StrRefReferenceCacheIO` in BioWare saves/loads JSON cache files with `game` + serialized `ToDict()` payload.
- R2. `find-strref --cache-file PATH` loads cache when file exists; otherwise builds full cache via `FindAllStrRefReferences`, saves to PATH, then queries.
- R3. `--rebuild-cache` forces rescan even when PATH exists.
- R4. `StrRefReferenceCache.FromDict` accepts `locations` as `List<string>` or deserialized JSON arrays.
- R5. KotorCLI README documents `--cache-file` / `--rebuild-cache`.
- R6. Tests: BioWare IO round-trip; KotorCLI second query hits saved cache without rescan (mtime unchanged).

## Scope Boundaries

- No find-2da-ref cache file; no multi-installation cache maps.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReferenceCacheIO`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CacheFile`
