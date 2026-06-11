---
title: "feat: odyTools FieldValueReferenceHelper and GFF wiring"
type: feat
status: active
date: 2026-06-05
origin: docs/knowledgebase/90-meta/pr-merge-readiness.md
branch: feat/plan-406-odytools-fieldvalue-reference-helper
---

# feat: OdyTools FieldValueReferenceHelper and GFF wiring (plan 406)

## Summary

BioWare `ReferenceFinder.FindFieldValueReferences` and KotorCLI `find-field-value` are complete, but OdyTools has no UI helper. Add `FieldValueReferenceHelper` mirroring `StrRefReferenceHelper` / `ReferenceSearchHelper`, wire **Find Field Value References** on `OdyToolGFF` String and ResRef value editors, and add OdyTools.Tests coverage.

## Problem Frame

Plan **063** reference-finder arc landed installation search and specialized helpers (tag, template ResRef, script, conversation, StrRef, 2DA row). Generic GFF string/ResRef field-value search remains CLI-only; mod authors using `OdyToolGFF` cannot find references to arbitrary field values from the editor.

## Requirements

- R1. `FieldValueReferenceHelper.CollectFieldValueReferences` delegates to `ReferenceFinder.FindFieldValueReferences` with optional field-name filter.
- R2. `FindAndShowFieldValueReferences` opens `FileResultsDialog` or shows empty-result info dialog (Holocron parity).
- R3. `AttachFieldValueFindReferencesMenu` adds context menu to a `TextBox`, optionally scoping search to a GFF field label.
- R4. `OdyToolGFF` wires menus on `_textEdit` (String) and `_lineEdit` (ResRef) using the selected tree node's label as field filter.
- R5. Unit tests cover collect, guards, and headless FindAndShow smoke without real game install.

## Key Technical Decisions

- New helper class in `src/Tools/OdyTools/Utils/` rather than expanding `ReferenceSearchHelper` — keeps tag/template/script helpers separate from generic field-value API.
- Field filter uses the selected GFF node label (case-insensitive match via BioWare `NormalizeFieldNameFilter`); when no node selected, search all string/ResRef fields (null filter).
- Reuse `ReferenceSearchHelper.PromptSearchOptions` for scope/case/partial options.

## Scope Boundaries

- **In:** Helper, GFF String/ResRef wiring, tests, standalone props compile include.
- **Out:** LocalizedString substring search, generic GFF tree context menu, KotorCLI changes, KB tracker sync (follow-up chore).

### Deferred to Follow-Up Work

- Wire `AttachFieldValueFindReferencesMenu` to other template editors beyond generic GFF.
- KB / build-ladder filter row for FieldValueReferenceHelper tests.

## Implementation Units

### U1. FieldValueReferenceHelper

**Goal:** R1, R2, R3

**Files:**
- `src/Tools/OdyTools/Utils/FieldValueReferenceHelper.cs`

**Approach:** Mirror `StrRefReferenceHelper` structure — collect method, FindAndShow with options prompt, empty-result dialog, Attach menu with `Func<string> getFieldNameFilter`.

**Test scenarios:**
- Null/whitespace value returns empty without throw.
- Null installation returns empty / no-op on FindAndShow.
- Override UTC Tag field hit returns result when field filter is `Tag`.

**Verification:** New unit tests pass.

### U2. OdyToolGFF wiring

**Goal:** R4

**Files:**
- `src/Tools/OdyTools/Editors/OdyToolGFF.axaml.cs`

**Approach:** After property panel setup, call `AttachFieldValueFindReferencesMenu` on `_textEdit` and `_lineEdit` with lambda reading `_selectedNode?.Label`.

**Verification:** `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0`

### U3. Tests

**Goal:** R5

**Files:**
- `tests/OdyTools.Tests/FieldValueReferenceHelperTests.cs`

**Test scenarios:**
- `CollectFieldValueReferences_EmptyValue_ReturnsEmpty`
- `FindAndShowFieldValueReferences_NullInstallation_DoesNotThrow`
- `FindAndShowFieldValueReferences_OverrideHit_CompletesWithoutException` (headless)
- `AttachFieldValueFindReferencesMenu_WiresMenuItem`

**Verification:**

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```

### U4. Standalone compile includes

**Goal:** Standalone GFF editor builds with helper.

**Files:**
- `src/Tools/OdyTools/Editors/Standalone/OdyTools.Standalone.Editor.props`

**Approach:** Add `FieldValueReferenceHelper.cs` alongside existing helper includes.

**Test expectation:** none — compile-only.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FieldValueReferenceHelper
```
