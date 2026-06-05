---
title: "feat: kotordiff installation reference search"
type: feat
status: complete
completed: 2026-06-05
date: 2026-06-05
origin: src/Tools/KotorDiff/Diff/ReferenceAnalyzers.cs (TODO STUB at installation branch)
branch: feat/plan-383-kotordiff-installation-ref-search
---

# feat: KotorDiff installation-based reference search (plan 383)

## Summary

Replace KotorDiff `ReferenceAnalyzers` installation stubs that fall back to naive folder walks. When the diff base path resolves to a BioWare `Installation`, StrRef analysis must use `ReferenceCacheHelpers.FindStrRefReferences`, and 2DA-memory GFF analysis must enumerate installation capsules via `Resolution.BuildResourceIndex` instead of `Directory.GetFiles`.

## Problem Frame

`AnalyzeTlkStrrefReferences` logs `TODO: STUB - Installation-based search not yet implemented` and sets `isInstallation = false`, missing override/chitin/module resources not present as loose files. `Analyze2daMemoryReferences` silently does the same for GFF enumeration. BioWare already ships installation-aware reference search used by OdyTools and KotorCLI.

## Requirements

- R1. StrRef reference discovery uses `ReferenceCacheHelpers.FindStrRefReferences` when `Installation` is available.
- R2. 2DA-memory GFF discovery collects GFF `FileResource` entries from `Resolution.BuildResourceIndex(installation)`.
- R3. Folder-based fallback remains unchanged when path is not an installation.
- R4. Remove `TODO: STUB` log for installation StrRef search.
- R5. Add KotorDiff unit tests for the new public helper methods.

## Key Technical Decisions

- Reuse BioWare `ReferenceCacheHelpers` and `Resolution.BuildResourceIndex` rather than duplicating capsule traversal in KotorDiff.
- Extract small public static helpers (`CollectInstallationStrRefResources`, `CollectInstallationGffResources`) so tests avoid full TLK patch orchestration.
- Default `ReferenceSearchOptions` (all scopes enabled) matches Holocron/KotorDiff folder-search breadth.

## Scope Boundaries

- **In:** `ReferenceAnalyzers.cs` installation branches, focused helpers, KotorDiff.Tests coverage.
- **Out:** Wiring `strrefMappings` in `DiffApplicationHelpers` (separate TODO), StrRef cache injection from incremental writer, multi-installation nested twoda cache maps.

### Deferred to Follow-Up Work

- Pass populated `strrefMappings` from TLK diff into `AnalyzeTlkStrrefReferences`.
- Thread `StrRefReferenceCache` from incremental writer into analysis for cache-fast path.

## Implementation Units

### U1. StrRef installation resource collection

**Goal:** Find override/chitin/module resources referencing a StrRef via BioWare APIs.

**Requirements:** R1, R3, R4

**Files:**
- `src/Tools/KotorDiff/Diff/ReferenceAnalyzers.cs`

**Approach:** Add `CollectInstallationStrRefResources(Installation, int strref, StrRefReferenceCache cache, Action<string> logFunc)` returning `HashSet<FileResource>`. Call from `AnalyzeTlkStrrefReferences` installation branch; delete stub log and `isInstallation = false` hack.

**Patterns to follow:** `tests/BioWare.Tests/ReferenceCacheStrRefTests.cs` override SSF fixture layout.

**Test scenarios:**
- Happy path: override SSF with known StrRef returns non-empty set containing that resource.
- Edge case: installation with no matching StrRef returns empty set (no throw).

**Verification:** Helper returns expected `FileResource` count for temp K1 stub install.

### U2. 2DA-memory GFF installation enumeration

**Goal:** Collect GFF resources from installation capsules for 2DA row reference patching.

**Requirements:** R2, R3

**Files:**
- `src/Tools/KotorDiff/Diff/ReferenceAnalyzers.cs`

**Approach:** Add `CollectInstallationGffResources(Installation installation)` using `Resolution.BuildResourceIndex`, filter `ResType.IsGff()`. Replace silent `isInstallation = false` block in `Analyze2daMemoryReferences`.

**Test scenarios:**
- Happy path: override UTC GFF appears in collected list.
- Edge case: installation with no GFF files returns empty list.

**Verification:** Helper includes override `.utc` resource from stub install.

### U3. KotorDiff.Tests characterization

**Goal:** Lock installation helper behavior.

**Requirements:** R5

**Files:**
- `tests/KotorDiff.Tests/ReferenceAnalyzersInstallationTests.cs`

**Approach:** Mirror BioWare test fixture pattern (temp dir + `chitin.key` + `SWKOTOR.EXE` + Override resources).

**Test scenarios:**
- `CollectInstallationStrRefResources_FindsOverrideSsf`
- `CollectInstallationStrRefResources_NoMatch_ReturnsEmpty`
- `CollectInstallationGffResources_IncludesOverrideUtc`
- `CollectInstallationGffResources_NoGff_ReturnsEmpty`

**Verification:**

```bash
dotnet build src/Tools/KotorDiff/KotorDiff.csproj --framework net9.0
dotnet test tests/KotorDiff.Tests/KotorDiff.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceAnalyzersInstallation
```

## Risks & Dependencies

- `Installation` constructor requires minimal game markers (`chitin.key`, exe) — tests must create them.
- Duplicate `FileResource` entries possible from `BuildResourceIndex`; use `HashSet` for StrRef collection (already used).

## Sources & Research

- [REPO] `src/Tools/KotorDiff/Diff/ReferenceAnalyzers.cs` lines 205–212, 776–782
- [REPO] `src/BioWare/Tools/ReferenceCache.cs` — `FindStrRefReferences`
- [REPO] `src/BioWare/TSLPatcher/Diff/Resolution.cs` — `BuildResourceIndex`
- [REPO] `tests/BioWare.Tests/ReferenceCacheStrRefTests.cs` — installation fixture pattern
