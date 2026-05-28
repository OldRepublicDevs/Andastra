---
title: "test: referencesearchhelper conversation and script wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-256-test-referencesearchhelper-template-menu-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper conversation and script wiring (plan 257)

## Summary

Complete `ReferenceSearchHelper` `FindAndShow*` coverage with whitespace guards and override wiring smoke for conversation and script search. Adds **4** tests.

## Requirements

- R1. `FindAndShowConversationReferences` and `FindAndShowTemplateResRefReferences` no-op on whitespace needle (no throw).
- R2. `FindAndShowConversationReferences` override wiring completes without exception (`showOptionsDialog: false`).
- R3. `FindAndShowScriptReferences` override wiring completes without exception (`showOptionsDialog: false`).
- R4. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests` passes (**20** total).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests
```
