---
title: "test: referencesearchhelper showoptionsdialog cancel paths"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-266-test-referencesearchhelper-prompt-accept-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper showOptionsDialog cancel paths (plan 267)

## Summary

Add **3** tests mirroring plan 266 tag cancel smoke for template, script, and conversation FindAndShow paths when `showOptionsDialog: true` and headless dialog is not accepted.

## Requirements

- R1. `FindAndShowTemplateResRefReferences` with options dialog cancel completes without throw.
- R2. `FindAndShowScriptReferences` with options dialog cancel completes without throw.
- R3. `FindAndShowConversationReferences` with options dialog cancel completes without throw.
- R4. `ReferenceSearchHelperTests` filter passes (**34** tests).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests
```
