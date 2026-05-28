---
title: "feat: TLK StrRef reference search options dialog"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-089-feat-combobox2da-reference-options-dialog-plan.md
branch: feat/holocron-port-phase-b
---

# feat: TLK StrRef reference options dialog (plan 091)

## Summary

Wire `ReferenceSearchOptionsDialog` into TLK editor StrRef **Find References**, mirroring ComboBox2DA (plan 089).

## Requirements

- R1. `StrRefReferenceHelper.FindAndShowStrRefReferences` accepts `showOptionsDialog`; prompts via `ReferenceSearchHelper.PromptSearchOptions` when true.
- R2. `CollectStrRefReferences` passes `ReferenceSearchOptions` to `ReferenceCacheHelpers.FindStrRefReferences`.
- R3. `OdyToolTLK.FindLocalizedStringReferences` invokes search with `showOptionsDialog: true`.
- R4. Unit tests: override-only finds SSF hit; no-override returns empty.

## Scope Boundaries

- No reference cache persistence or NCS CONSTI disambiguation.

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReference`
