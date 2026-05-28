---
title: "test: scriptreferencehelper nomatch and fallback wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-268-docs-referencesearchhelper-closure-plan.md
branch: feat/holocron-port-phase-b
---

# test: ScriptReferenceHelper no-match and fallback wiring (plan 269)

## Summary

Add **2** tests for `ScriptReferenceHelper.FindAndShowScriptReferences` combo wiring with a real installation: non-matching script ResRef and selected-item fallback when combo text is empty. Both paths use `showOptionsDialog: true` (headless cancel smoke).

## Requirements

- R1. Non-matching script ResRef from combo text completes without throw.
- R2. Selected-item fallback with installation completes without throw when combo text is empty.
- R3. `ScriptReferenceHelperTests` filter passes (**7** tests).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ScriptReferenceHelperTests
```
