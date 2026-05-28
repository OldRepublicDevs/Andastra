---
title: "test: script combo precedence and strref ncs prompt"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-271-docs-u6-reference-finder-milestone-plan.md
branch: feat/holocron-port-phase-b
---

# test: Script combo precedence and StrRef/NCS prompt (plan 272)

## Summary

Post-milestone polish: **2** tests covering `ScriptReferenceHelper` combo text precedence over `SelectedItem`, and `PromptSearchOptions(..., showStrRefNcsOptions: true)` headless cancel.

## Requirements

- R1. Combo `Text` is used when non-empty even if `SelectedItem` differs.
- R2. `PromptSearchOptions` StrRef/NCS overload returns null when dialog not accepted.
- R3. Filtered tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~ComboTextPreferredOverSelectedItem|FullyQualifiedName~StrRefNcsOptions_NotAccepted"
```
