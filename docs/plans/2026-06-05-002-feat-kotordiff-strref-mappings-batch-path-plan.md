---
title: "feat: kotordiff strref mappings batch generation path"
type: feat
status: complete
completed: 2026-06-05
date: 2026-06-05
origin: docs/plans/2026-06-05-001-feat-kotordiff-installation-reference-search-plan.md
branch: feat/plan-384-kotordiff-strref-mappings-batch
---

# feat: KotorDiff StrRef mappings batch generation path (plan 384)

## Summary

Wire TLK `strref_mappings` into the batch `GenerateTSLPatcherData` path so `AnalyzeTlkStrrefReferences` runs with real old-StrRef → token mappings. The incremental writer already stores mappings via `SetTlkMetadata`; batch generation currently passes an empty dictionary and analysis exits immediately.

## Problem Frame

`DiffApplicationHelpers.GenerateTSLPatcherData` contains `// TODO: Here we do not have strref_mappings directly, so pass empty mapping for now`. `ReferenceAnalyzers.AnalyzeTlkStrrefReferences` returns early when mappings are empty, so batch TSLPatcher generation never creates StrRef linking patches.

## Requirements

- R1. Derive `Dictionary<int,int>` from each `ModificationsTLK.Modifiers` entry (`ModIndex` → `TokenId`).
- R2. Batch path passes derived mappings into `AnalyzeTlkStrrefReferences`.
- R3. Shared helper is testable without full diff orchestration.
- R4. Remove the TODO comment once wired.

## Key Technical Decisions

- Place `BuildStrrefMappingsFromTlkMod` on `ReferenceAnalyzers` beside existing installation helpers.
- Mirror `DiffAnalyzerFactory` / incremental writer semantics: key = TLK index (old StrRef), value = STRREF memory token.

## Scope Boundaries

- **In:** Helper extraction, batch path wiring, KotorDiff.Tests coverage.
- **Out:** Threading `StrRefReferenceCache` from incremental writer (separate follow-up).

## Implementation Units

### U1. StrRef mapping helper

**Goal:** R1, R3

**Files:**
- `src/Tools/KotorDiff/Diff/ReferenceAnalyzers.cs`

**Approach:** Add `BuildStrrefMappingsFromTlkMod(ModificationsTLK tlkMod)` returning empty dict for null/empty modifiers; otherwise `modifiers[i].ModIndex → modifiers[i].TokenId`.

**Test scenarios:**
- Two modifiers produce two entries with correct keys/values.
- Null mod or empty modifiers returns empty dict.

**Verification:** Unit tests pass for helper.

### U2. Batch path wiring

**Goal:** R2, R4

**Files:**
- `src/Tools/KotorDiff/App/DiffApplicationHelpers.cs`

**Approach:** Replace empty `strrefMappings` with `ReferenceAnalyzers.BuildStrrefMappingsFromTlkMod(tlkMod)`.

**Verification:** `dotnet build src/Tools/KotorDiff/KotorDiff.csproj --framework net9.0`

### U3. Tests

**Goal:** R3

**Files:**
- `tests/KotorDiff.Tests/ReferenceAnalyzersStrrefMappingsTests.cs`

**Test scenarios:**
- `BuildStrrefMappingsFromTlkMod_TwoModifiers_MapsModIndexToTokenId`
- `BuildStrrefMappingsFromTlkMod_EmptyModifiers_ReturnsEmpty`

**Verification:**

```bash
dotnet test tests/KotorDiff.Tests/KotorDiff.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrrefMappings
```
