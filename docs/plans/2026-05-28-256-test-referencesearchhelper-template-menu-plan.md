---
title: "test: referencesearchhelper template menu enablement"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-253-test-referencesearchhelper-enablement-wiring-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper template menu enablement (plan 256)

## Summary

Mirror plan 253 tag enablement tests for template ResRef context menus, plus template override wiring smoke. Adds **4** tests to `ReferenceSearchHelperTests.cs`.

## Requirements

- R1. `AttachTemplateResRefFindReferencesMenu` disables when ResRef empty or installation missing; enables when both present.
- R2. `FindAndShowTemplateResRefReferences` with override hit and `showOptionsDialog: false` completes without exception.
- R3. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchHelper` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceSearchHelper
```
