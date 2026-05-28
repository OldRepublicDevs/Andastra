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

**ScriptReferenceHelper combo follow-up landed (2026-05-28, plans 269–270 on `feat/holocron-port-phase-b`):** editor combo no-match and selected-item fallback with installation; **171** reference-search tests total (**95** BioWare `ReferenceFinder` + **76** OdyTools helper/UI, including **7** `ScriptReferenceHelperTests`). See plan `269`.

**Post-milestone polish landed (2026-05-28, plan 272 on `feat/holocron-port-phase-b`):** combo text precedence over `SelectedItem`, StrRef/NCS `PromptSearchOptions` cancel; **173** reference-search tests total (**95** BioWare `ReferenceFinder` + **78** OdyTools helper/UI, including **35** `ReferenceSearchHelperTests` and **8** `ScriptReferenceHelperTests`). See plan `272`.

**NCS override FindAndShow smoke landed (2026-05-28, plan 274 on `feat/holocron-port-phase-b`):** `ReferenceSearchHelper.FindAndShowScriptReferences` override `.ncs` byte-scan path; **174** reference-search tests total (**95** BioWare `ReferenceFinder` + **79** OdyTools helper/UI, including **36** `ReferenceSearchHelperTests`). See plan `274`.

**KotorCLI find-2da-ref `--full-row` parity closed (2026-05-28, plans 107 / 276 on `feat/holocron-port-phase-b`):** BioWare `CollectTwoDARowReferences` shared with OdyTools `TwoDAMemoryReferenceHelper`; KotorCLI `find-2da-ref --full-row` loads 2DA from installation and sweeps label field-value + StrRef columns. **13** `Find2DARefCommandTests` + **2** `ReferenceCacheHelpersTwoDARowReferencesTests`. See plans `107`, `276`.

**OdyTools 2DA row sweep + README closed (2026-05-28, plans 108 / 277 on `feat/holocron-port-phase-b`):** `CollectTwoDARowReferences_WithTwoDA_FindsLabelFieldValueRef` and `_FindsRowStrRefColumnRef` in `TwoDAMemoryReferenceHelperTests`; KotorCLI README wired/partial/stub inventory. **10** `TwoDAMemoryReferenceHelperTests`. See plans `108`, `277`.

**UTD/UTP/UTT script ref finder closed (2026-05-28, plan 069 / 279 on `feat/holocron-port-phase-b`):** `ScriptReferenceHelper` + template editor **Find References** wiring; **8** `ScriptReferenceHelperTests`. See plans `069`, `279`.

**NCS StrRef cache gating closed (2026-05-28, plan 286):** `IncludeNcsStrRefScan` gates NCS indexing in `BuildStrRefReferenceCache` and batch lookup; **10** `NcsConstiScannerTests` + **2** KotorCLI CLI tests. See plan `286`.

**NCS CONSTS script ResRef scanner closed (2026-05-28, plan 287):** `NcsConstStringScanner` + CONSTS-first `FindScriptResRefInNcsBytes` with `(NCS bytecode) offset_<n>` paths; **3** `NcsConstStringScannerTests` + **5** `FindScriptResRefInNcsBytes` tests. See plan `287`.

**Compiled NCS installation test closed (2026-05-28, plan 289):** `FindScriptReferences_OverrideCompiledNcs_ReturnsNcsBytecodePath` in `ReferenceFinderTests`. See plan `289`.

**KotorCLI find-refs compiled NCS CLI test closed (2026-05-28, plan 290):** `Cli_FindRefs_Script_CompiledNcsInOverride_ExitsZero` in `FindRefsCommandCliTests`. See plan `290`.

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
- **Landed (2026-05-28, plan 286):** NCS StrRef indexing via `StrRefReferenceCache` / `ReferenceCacheHelpers` with `IncludeNcsStrRefScan` gating in cache build and batch lookup.
- **Landed (2026-05-28, plan 287):** NCS CONSTS script ResRef scan via `NcsConstStringScanner` and CONSTS-first `ReferenceFinder.FindScriptResRefInNcsBytes` with `(NCS bytecode) offset_<n>` paths.
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
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests` (**36** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ScriptReferenceHelperTests` (**8** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchOptionsDialogTests` (**9** tests)

See plans `docs/plans/2026-05-28-224-*` through `docs/plans/2026-05-28-290-*` and plans **276**–**285** for slice history (parent-plan closures **107**, **108**, **066**, **069**, **067**, **064**, **065**, **070**, U4 KotorDiff). Plan **288** syncs NCS bytecode verification rows in `docs/knowledgebase/30-product-ux/odytools-editor-ux.md`. Plans **289**–**290** add compiled NCS end-to-end test coverage (BioWare installation + KotorCLI CLI).

**KotorCLI find-2da-ref verification:**

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Find2DARef` (**13** tests)
- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDARow` (**2** tests)

**StrRef/2DA helper verification (OdyTools):**

- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReferenceHelperTests` (**8** tests)
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDAMemoryReferenceHelperTests` (**10** tests)
