---
title: "feat: reference finder phase 2 — installation search and utc wiring"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U6 phase 2)
branch: feat/holocron-fac-kotorcli
follow_up_landed: 2026-05-28
---

# feat: Reference finder phase 2 — installation search and UTC wiring

## Summary

Deliver holocron **U6 phase 2** on `feat/holocron-fac-kotorcli`: extend BioWare `ReferenceFinder` with installation-wide script ResRef search returning structured results with GFF field paths; show field paths in `FileResultsDialog`; wire **Find References** on UTC script combo context menus.

**Follow-up landed (2026-05-28, plans 224–260 on `feat/holocron-port-phase-b`):** tag, template ResRef, conversation, and field-value installation search; `ReferenceSearchOptionsDialog`; `ReferenceSearchHelper` / `ScriptReferenceHelper` editor wiring; **95** `ReferenceFinderTests` + **42** OdyTools UI/options/wiring tests.

**StrRef/2DA helper follow-up landed (2026-05-28, plans 261–264 on `feat/holocron-port-phase-b`):** `StrRefReferenceHelper` and `TwoDAMemoryReferenceHelper` FindAndShow wiring + empty-result collect tests; **148** reference-search tests total (**95** BioWare `ReferenceFinder` + **53** OdyTools helper/wiring, including **8** StrRef + **10** TwoDA helper tests). See plans `082`, `084`, `261`–`263`.

**ReferenceSearchHelper prompt/cancel follow-up landed (2026-05-28, plans 265–268 on `feat/holocron-port-phase-b`):** `BuildPromptResult` accept round-trip, no-match FindAndShow smoke, and `showOptionsDialog: true` cancel coverage for tag/template/script/conversation; **169** reference-search tests total (**95** BioWare `ReferenceFinder` + **74** OdyTools helper/UI, including **34** `ReferenceSearchHelperTests`). See plans `265`–`267`.

## Requirements

- R1. `ReferenceFinder.FindScriptReferences(Installation, string scriptResRef, ReferenceSearchOptions options)` returns `List<ReferenceSearchResult>` with `FileResource` + `FieldPath`.
- R2. `ReferenceSearchOptions` supports override/modules/chitin scope toggles (Holocron defaults: all true).
- R3. Empty/whitespace needle returns empty list; null installation throws `ArgumentNullException`.
- R4. `FileResultsDialog` displays field path suffix when results include `ReferenceSearchResult`.
- R5. UTC script combo context menu adds **Find References** calling search and opening results dialog.
- R6. Tests in `OdyTools.Tests` using temp override fixture (no real game install required).

## Scope Boundaries

- **In:** BioWare installation scan for GFF script ResRefs, FileResultsDialog field paths, UTC script field wiring.
- **Originally out (now landed):** `ReferenceSearchOptionsDialog` UI, tag/template/conversation/field-value search, chitin/module scope matrix, `ReferenceSearchHelper` / `ScriptReferenceHelper` wiring tests.
- **Still out:** NCS bytecode reference scan via `ReferenceCache` (StrRef/NCS scan partially covered in `StrRefReferenceHelper`).
- **Follow-up (landed in same branch):** UTD/UTP/UTT script combo **Find References** via `ScriptReferenceHelper`.

## Implementation Units

### U6c — BioWare FindScriptReferences

**Files:** `src/BioWare/Tools/ReferenceFinder.cs`

**Approach:** Enumerate resources per scope options (mirror `ReferenceCacheHelpers.GetAllResources` pattern); scan GFF resources via existing `FindScriptResRefInGffBytes`.

### U6d — FileResultsDialog field paths

**Files:** `src/Tools/OdyTools/Dialogs/FileResultsDialog.axaml.cs`

**Approach:** Add constructor overload for `IEnumerable<ReferenceSearchResult>`; display `parent/file.ext :: FieldPath`.

### U6e — UTC context menu

**Files:** `src/Tools/OdyTools/Editors/OdyToolUTC.axaml.cs`

**Approach:** Add Find References menu item to `SetupScriptComboBoxContextMenu`; require active installation.

### U6f — Tests

**Files:** `tests/OdyTools.Tests/ReferenceFinderTests.cs`

**Approach:** Temp K1 stub install with override UTC; assert FindScriptReferences finds ScriptHeartbeat path.

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Override UTC with matching script | Result with field path `ScriptHeartbeat` |
| Empty needle | Empty list |
| Override-only scope | Skips when override disabled |

## Verification

- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder` (**95** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FileResultsDialogReferenceSearchTests` (**8** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests` (**34** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ScriptReferenceHelperTests` (**5** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchOptionsDialogTests` (**9** tests)

See plans `docs/plans/2026-05-28-224-*` through `docs/plans/2026-05-28-268-*` for slice history.

**StrRef/2DA helper verification (OdyTools):**

- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReferenceHelperTests` (**8** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDAMemoryReferenceHelperTests` (**10** tests)
