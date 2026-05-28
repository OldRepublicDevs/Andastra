---
title: "test: referencesearchhelper no-match and prompt wiring"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-264-docs-strref-twoda-reference-search-closure-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceSearchHelper no-match and PromptSearchOptions (plan 265)

## Summary

Add **3** tests for uncovered `ReferenceSearchHelper` paths: empty-result FindAndShow smoke (tag, template) and `PromptSearchOptions` cancel/null-parent behavior in headless Avalonia.

## Requirements

- R1. `FindAndShowTagReferences` with non-matching tag completes without exception (`showOptionsDialog: false`).
- R2. `FindAndShowTemplateResRefReferences` with non-matching ResRef completes without exception.
- R3. `PromptSearchOptions(null, defaults)` returns null when dialog is not accepted (headless cancel path).
- R4. Filtered OdyTools tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchHelperTests
```
