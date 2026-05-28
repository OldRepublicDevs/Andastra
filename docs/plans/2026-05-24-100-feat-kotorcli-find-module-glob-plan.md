---
title: "feat: kotorcli find commands module-glob filter"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-079-feat-holocron-phase-j-kotorcli-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: find command module-glob filter (plan 100)

## Summary

Add repeatable `--module-glob` to KotorCLI find commands, filtering module capsule scans by filename pattern (deferred from plan 079).

## Requirements

- R1. `ReferenceSearchOptions.ModuleGlobFilters` (`List<string>`, empty/null = all modules).
- R2. `ModuleGlobMatcher.MatchesAnyModuleGlob` supports `*` and `?` (case-insensitive filename match).
- R3. `ReferenceFinder.EnumerateResources` and `ReferenceCacheHelpers` module enumeration honor filters.
- R4. All four find commands expose `--module-glob`; forward to search options.
- R5. Tests: matcher unit tests; `find-refs` integration with two modules.

## Scope Boundaries

- No override/chitin path filtering; no disk cache persistence.

## Verification

- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ModuleGlob`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindRefs`
