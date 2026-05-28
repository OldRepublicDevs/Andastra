---
title: "test: referencesearchhelper guards and context menus"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-251-test-fileresultsdialog-reference-search-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper guards and context menus (plan 252)

## Summary

Continue OdyTools reference-search UI layer after plan 251. Add **4** unit tests for `ReferenceSearchHelper` guard clauses and context-menu attachment.

## Requirements

- R1. `FindAndShowTagReferences` no-ops on null installation or whitespace needle (no throw).
- R2. `FindAndShowScriptReferences` no-ops on null installation or whitespace needle (no throw).
- R3. `AttachTagFindReferencesMenu` wires a context menu with "Find Tag References".
- R4. `AttachTemplateResRefFindReferencesMenu` wires a context menu with "Find Template ResRef References".
- R5. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchHelper` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchHelper
```
