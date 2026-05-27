---
title: "feat: Holocron port phase L — reference finder installation coverage"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-080-feat-holocron-phase-k-dlg-refs-cli-flags-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase L (plan 081)

## Summary

Close reference-finder gaps with installation-level NCS/template/conversation tests, populate `MatchedValue` on search hits, and document KotorCLI `find-refs`.

## Requirements

- R1. `ReferenceFinder.SearchInstallation` sets `MatchedValue` on each `ReferenceSearchResult`.
- R2. Installation integration tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs` for NCS script hits and conversation override UTC.
- R3. `tests/KotorCLI.Tests/FindRefsCommandTests.cs` covers `--type template` and NCS script override hits.
- R4. Document `find-refs` in `src/Tools/KotorCLI/README.md`.

## Deferred

- StrRef/2DA reference cache CLI, NCS CONST opcode parsing, Module Designer.

## Verification

- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FindRefs`
