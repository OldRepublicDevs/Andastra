---
title: "feat: SSF editor installation StrRef find references"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-091-feat-tlk-strref-reference-options-dialog-plan.md
branch: feat/holocron-port-phase-b
---

# feat: SSF editor StrRef installation find references (plan 092)

## Summary

Add **Find StrRef References in Installation** to the SSF editor for the selected sound slot, using `StrRefReferenceHelper` with options dialog (Holocron parity with TLK plan 091).

## Requirements

- R1. Edit menu entry **Find StrRef References in Installation...** (distinct from in-file Find Strref navigation).
- R2. Uses selected row's StrRef value; disabled when installation missing or StrRef &lt; 0.
- R3. Delegates to `StrRefReferenceHelper.FindAndShowStrRefReferences(..., showOptionsDialog: true)`.
- R4. Manual verification: build OdyTools; existing `StrRefReferenceHelperTests` cover scoped search logic.

## Scope Boundaries

- No NCS CONSTI disambiguation or cache persistence.

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReference`
