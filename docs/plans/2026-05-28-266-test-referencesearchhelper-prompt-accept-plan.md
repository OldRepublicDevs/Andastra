---
title: "test: referencesearchhelper prompt accept round-trip"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-265-test-referencesearchhelper-nomatch-prompt-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper prompt accept round-trip (plan 266)

## Summary

Complete `PromptSearchOptions` coverage: wire `BuildPromptResult` internal helper, add accept-path round-trip tests, `showOptionsDialog: true` cancel smoke, and no-match tests for conversation/script FindAndShow paths.

## Requirements

- R1. `PromptSearchOptions` delegates mapping to `BuildPromptResult` after dialog acceptance.
- R2. `BuildPromptResult` returns null when not accepted; round-trips scope and StrRef/NCS options when accepted.
- R3. `FindAndShowTagReferences` with `showOptionsDialog: true` completes without throw on headless cancel.
- R4. `FindAndShowConversationReferences` and `FindAndShowScriptReferences` no-match smoke tests pass.
- R5. `ReferenceSearchHelperTests` filter passes.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
```
