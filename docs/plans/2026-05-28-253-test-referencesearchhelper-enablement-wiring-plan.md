---
title: "test: referencesearchhelper menu enablement and wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-252-test-referencesearchhelper-guards-menu-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper menu enablement and wiring (plan 253)

## Summary

Gap-fill after plan 252: menu item enable/disable rules and one override wiring smoke test through `FindAndShowTagReferences`. Adds **4** tests to `ReferenceSearchHelperTests.cs`.

## Requirements

- R1. `AttachTagFindReferencesMenu` disables when tag empty or installation missing; enables when both present.
- R2. `FindAndShowTagReferences` with override hit and `showOptionsDialog: false` completes without exception.
- R3. `FindAndShowTemplateResRefReferences` and `FindAndShowConversationReferences` guard null installation (no throw).
- R4. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchHelper` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchHelper
```
