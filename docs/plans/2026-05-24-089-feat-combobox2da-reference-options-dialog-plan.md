---
title: "feat: ComboBox2DA reference search options dialog"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-088-feat-kotorcli-strref-2da-ref-scope-flags-plan.md
branch: feat/holocron-port-phase-b
---

# feat: ComboBox2DA ReferenceSearchOptionsDialog (plan 089)

## Summary

Wire Holocron-style `ReferenceSearchOptionsDialog` into `ComboBox2DA` **Find References** so 2DA row searches honor override/modules/chitin scope (and field-value partial/case flags for label search).

## Requirements

- R1. `TwoDAMemoryReferenceHelper.FindAndShowTwoDARowReferences` accepts `showOptionsDialog`; when true, prompts via `ReferenceSearchHelper.PromptSearchOptions` before search.
- R2. `CollectTwoDARowReferences` passes `ReferenceSearchOptions` to `Find2DAMemoryReferences`, `FindFieldValueReferences`, and `FindStrRefReferences`.
- R3. `ComboBox2DA` context menu invokes search with `showOptionsDialog: true`.
- R4. Unit test: override-only fixture found with scope; `--no-override` equivalent options returns empty.

## Scope Boundaries

- No changes to KotorCLI (088 landed scope flags).
- Reference cache persistence deferred.

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDAMemoryReference`
