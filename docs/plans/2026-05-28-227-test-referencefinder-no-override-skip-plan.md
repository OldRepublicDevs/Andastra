---
title: "test: referencefinder no-override skip for tag, template, conversation"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-226-test-referencefinder-scope-partial-field-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder no-override skip — tag, template, conversation (plan 227)

## Summary

Extend OdyTools `ReferenceFinderTests` with installation-level tests that verify override UTC resources are skipped when `SearchOverride = false`, mirroring `FindScriptReferences_NoOverride_SkipsOverrideUtc` from plan 226.

## Requirements

- R1. `FindTagReferences_NoOverride_SkipsOverrideUtc` — override UTC with matching Tag is not searched when `SearchOverride = false`.
- R2. `FindTemplateResRefReferences_NoOverride_SkipsOverrideUtc` — override UTC with matching TemplateResRef is not searched when `SearchOverride = false`.
- R3. `FindConversationResRefReferences_NoOverride_SkipsOverrideUtc` — override UTC with matching Conversation ResRef is not searched when `SearchOverride = false`.
- R4. ReferenceFinder filter **25** tests pass (22 existing + 3 new).

## Implementation

**File:** `tests/OdyTools.Tests/ReferenceFinderTests.cs`

For each test, follow the existing pattern in `FindScriptReferences_NoOverride_SkipsOverrideUtc`:

1. Temp install root under `Path.GetTempPath()` with unique suffix.
2. `Override/` directory + `SWKOTOR.EXE` stub.
3. UTC in override with the needle field set.
4. `ReferenceSearchOptions`: `SearchChitin = false`, `SearchModules = false`, `SearchOverride = false`.
5. Assert results list is empty.
6. `finally` block deletes install root (best-effort).

**APIs (no production changes):**

- `ReferenceFinder.FindTagReferences`
- `ReferenceFinder.FindTemplateResRefReferences`
- `ReferenceFinder.FindConversationResRefReferences`

## Test scenarios

| Test | UTC field | Needle | API |
|------|-----------|--------|-----|
| Tag no-override | `Tag` | unique tag string | `FindTagReferences` |
| Template no-override | `ResRef` (TemplateResRef) | unique template resref | `FindTemplateResRefReferences` |
| Conversation no-override | `Conversation` | unique dlg resref | `FindConversationResRefReferences` |

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```

## Scope boundaries

- Test-only slice; no `ReferenceFinder.cs` changes.
- AgentDecompile skipped (test-only).
- C# 7.3 compatible (no new language features).
