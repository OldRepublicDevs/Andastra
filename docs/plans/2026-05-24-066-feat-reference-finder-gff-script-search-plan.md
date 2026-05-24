---
title: "feat: reference finder phase 1 — gff script resref search"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U6 phase 1)
branch: feat/holocron-fac-kotorcli
---

# feat: Reference finder phase 1 — GFF script ResRef search

## Summary

Deliver holocron **U6 phase 1** on `feat/holocron-fac-kotorcli`: add BioWare `ReferenceFinder.FindScriptResRefInGffBytes` to locate script ResRef field paths inside GFF payloads, plus characterization tests. Defers installation-wide search, UTC context menu wiring, and `FileResultsDialog` field-path UI to a follow-up slice.

## Requirements

- R1. `ReferenceFinder.FindScriptResRefInGffBytes(byte[] data, string resRef)` returns matching GFF field paths (e.g. `ScriptHeartbeat`).
- R2. Empty or whitespace needle returns empty list without throwing.
- R3. No match returns empty list.
- R4. Tests live in `OdyTools.Tests` (net9.0 build path avoids Andastra/Stride dependency on Linux CI).

## Scope Boundaries

- **In:** BioWare API + tests for in-memory GFF bytes.
- **Out:** Installation scan, NCS bytecode search, `ReferenceSearchOptionsDialog`, UTC editor menu wiring (U6 remainder).

## Implementation Units

### U6a — BioWare ReferenceFinder API

**Files:** `src/BioWare/Tools/ReferenceFinder.cs`

**Approach:** Mirror `ReferenceCacheHelpers` GFF recursion; match `GFFFieldType.ResRef` values to needle (case-insensitive).

### U6b — Tests

**Files:** `tests/OdyTools.Tests/ReferenceFinderTests.cs`

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| UTC ScriptHeartbeat match | Returns path `ScriptHeartbeat` |
| Empty needle | Empty list |
| No match | Empty list |

## Verification

- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder`
