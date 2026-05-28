---
title: "test: referencesearchhelper prompt search options"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper PromptSearchOptions (plan 265)

## Summary

Headless-safe coverage for `PromptSearchOptions` cancel/null-defaults and `BuildPromptResult` accept mapping. See also `2026-05-28-266-test-referencesearchhelper-prompt-accept-plan.md` for the same landed slice.

## Requirements

- R1. `PromptSearchOptions` returns null when dialog not accepted (headless).
- R2. `BuildPromptResult` round-trips scope and StrRef/NCS options when accepted.
- R3. `FindAndShowTagReferences` with `showOptionsDialog: true` cancel does not throw.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
```
