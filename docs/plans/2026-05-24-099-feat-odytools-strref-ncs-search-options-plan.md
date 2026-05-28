---
title: "feat: odyTools strref ncs search options in dialog"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-095-feat-find-strref-ncs-strref-min-plan.md
branch: feat/holocron-port-phase-b
---

# feat: OdyTools StrRef NCS options in reference dialog (plan 099)

## Summary

Wire `IncludeNcsStrRefScan` and optional `NcsStrRefCandidateMinimum` into OdyTools StrRef reference search when the options dialog is shown (deferred from plan 095).

## Requirements

- R1. `ReferenceSearchOptionsDialog` optional StrRef NCS section: scan NCS toggle + minimum CONSTI text field.
- R2. `ReferenceSearchHelper.PromptSearchOptions` accepts `showStrRefNcsOptions`; seeds defaults via `SetDefaults`.
- R3. `StrRefReferenceHelper` passes `showStrRefNcsOptions: true` when prompting.
- R4. `ToSearchOptions` maps NCS fields; empty min field leaves `NcsStrRefCandidateMinimum` null.
- R5. Tests: dialog maps NCS options; `CollectStrRefReferences` honors `IncludeNcsStrRefScan = false`.

## Scope Boundaries

- No changes to tag/template/2DA reference dialogs beyond shared helper signature default.

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~StrRef|FullyQualifiedName~ReferenceSearchOptions"
