---
title: "test: referencefinder nooverride scope for tag template conversation"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-226-test-referencefinder-scope-partial-field-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder NoOverride scope — tag, template, conversation (plan 227)

## Summary

Complete OdyTools installation-scope parity for reference search types that already have override-positive tests and a script `SearchOverride = false` guard. Add three symmetric **NoOverride** tests so tag, template ResRef, and conversation ResRef searches skip override UTC when `SearchOverride` is false, matching plan 068 scenario “Override-only scope | Skips when override disabled” and KotorCLI `no-override` CLI behavior.

## Problem Frame

Plan 226 landed `FindScriptReferences_NoOverride_SkipsOverrideUtc` plus partial-match and field-value coverage. Tag, template, and conversation installation searches still lack the negative scope test, leaving a small parity gap vs Holocron/PyKotor expectations and `FindRefsCommandTests` / `FindFieldValueCommandTests` patterns.

## Requirements

- R1. `FindTagReferences_NoOverride_SkipsOverrideUtc` — override UTC with Tag not returned when `SearchOverride = false` (chitin/modules off).
- R2. `FindTemplateResRefReferences_NoOverride_SkipsOverrideUtc` — same for `TemplateResRef`.
- R3. `FindConversationResRefReferences_NoOverride_SkipsOverride` — same for `Conversation`.
- R4. OdyTools ReferenceFinder filter **25** tests pass (22 existing + 3 new).

## Scope Boundaries

- **In:** `tests/OdyTools.Tests/ReferenceFinderTests.cs` only.
- **Out:** BioWare `ReferenceFinder` implementation changes, UI wiring, KotorCLI CLI, AgentDecompile (test-only slice).

### Deferred to Follow-Up Work

- `FindFieldValueReferences_NoOverride` installation test (CLI covered; lower priority than the three primary search APIs).
- Module/chitin-only scope matrix tests.

## Context & Research

### Relevant Code and Patterns

- Existing positive override tests: `FindTagReferences_OverrideUtc_ReturnsFieldPath`, `FindTemplateResRefReferences_OverrideUtc_ReturnsFieldPath`, `FindConversationResRefReferences_OverrideUtc_ReturnsFieldPath`.
- Negative script pattern: `FindScriptReferences_NoOverride_SkipsOverrideUtc` (plan 226).
- KotorCLI: `FindRefsCommandTests.Execute_NoOverride_SkipsOverrideHit`, `FindFieldValueCommandTests.Execute_NoOverride_SkipsOverrideTag`.

## Key Technical Decisions

- **Mirror script NoOverride fixture:** Temp install root, `Override/test_npc.utc`, `SearchChitin = false`, `SearchModules = false`, `SearchOverride = false`, assert empty results.
- **No production code:** `ReferenceFinder` already respects `ReferenceSearchOptions.SearchOverride`; this slice is test-only characterization.

## Implementation Units

- U1. **NoOverride scope tests for tag, template, conversation**

**Goal:** Lock installation search scope for three reference types when override is disabled.

**Requirements:** R1, R2, R3, R4

**Dependencies:** None

**Files:**
- Modify: `tests/OdyTools.Tests/ReferenceFinderTests.cs`

**Approach:**
- Copy structure from `FindScriptReferences_NoOverride_SkipsOverrideUtc`.
- Use distinct temp path prefixes and unique needle values per test to avoid cross-test pollution.

**Patterns to follow:**
- `FindScriptReferences_NoOverride_SkipsOverrideUtc` in the same file.

**Test scenarios:**
- **Edge case:** Tag search with override UTC present but `SearchOverride = false` → empty list.
- **Edge case:** Template ResRef search with override UTC present but `SearchOverride = false` → empty list.
- **Edge case:** Conversation ResRef search with override UTC present but `SearchOverride = false` → empty list.

**Verification:**
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder` reports 25 passed.

## System-Wide Impact

- **Unchanged invariants:** BioWare `ReferenceFinder` API and OdyTools UI behavior unchanged.

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| Flaky temp-dir cleanup | try/finally with best-effort `Directory.Delete` (existing pattern) |

## Sources & References

- Origin: `docs/plans/2026-05-28-226-test-referencefinder-scope-partial-field-plan.md`
- Plan 068: `docs/plans/2026-05-24-068-feat-reference-finder-installation-utc-plan.md`
- PR: #11
