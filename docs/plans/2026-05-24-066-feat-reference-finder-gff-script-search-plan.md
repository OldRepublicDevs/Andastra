---
title: "feat: reference finder phase 1 — gff script resref search"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U6 phase 1)
branch: feat/holocron-fac-kotorcli
superseded_by: docs/plans/2026-05-24-068-feat-reference-finder-installation-utc-plan.md
closure: docs/plans/2026-05-28-278-docs-close-plan-066-sync-277-plan.md
---

# feat: Reference finder phase 1 — GFF script ResRef search

## Completion (2026-05-28)

Plan **066** scope landed on `feat/holocron-fac-kotorcli` and was extended by plan **068** (installation search + UTC wiring) and follow-ups **224–277**. Closed doc-only via plan **278**.

| Req | Status | Evidence |
|-----|--------|----------|
| R1 | **Landed** | `ReferenceFinder.FindScriptResRefInGffBytes` in `src/BioWare/Tools/ReferenceFinder.cs` |
| R2 | **Landed** | Empty/whitespace needle returns empty list |
| R3 | **Landed** | No match returns empty list |
| R4 | **Landed** | **5** `FindScriptResRefInGffBytes` tests in `ReferenceFinderTests.cs` |

**Verification (2026-05-28):**

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~FindScriptResRefInGffBytes"
# Passed: 5
```

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
