---
title: "test: scriptreferencehelper combo wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-253-test-referencesearchhelper-enablement-wiring-plan.md
branch: feat/holocron-port-phase-b
---

# test: ScriptReferenceHelper combo wiring (plan 255)

## Summary

Add **5** unit tests for `ScriptReferenceHelper`, the editor script-combo wrapper that delegates to `ReferenceSearchHelper.FindAndShowScriptReferences`.

## Requirements

- R1. `FindAndShowScriptReferences` returns early when `ComboBox` is null (no throw).
- R2. Empty text and no selection delegates safely (no throw with null installation).
- R3. `SelectedItem` is used when `Text` is empty.
- R4. `ComboBox.Text` is trimmed and passed when non-empty.
- R5. `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ScriptReferenceHelper` passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ScriptReferenceHelper
```
