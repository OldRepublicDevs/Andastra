---
title: "test: referencefinder installation template conversation script case sensitivity"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-068-feat-reference-finder-installation-utc-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder installation-level case sensitivity (plan 241)

## Summary

Add three installation-scoped tests that prove `ReferenceSearchOptions.CaseSensitive` flows through `SearchInstallation` for template ResRef, conversation ResRef, and script ResRef searches—mirroring the existing `FindTagReferences_CaseSensitive_OverrideUtc` pattern. Brings ReferenceFinder test count from **65 → 68**.

---

## Problem Frame

Byte-level GFF scanners already have case-sensitivity coverage (`FindTemplateResRefInGffBytes_CaseSensitive_RequiresExactCase`, etc.), and tag search has installation-level case coverage (`FindTagReferences_CaseSensitive_OverrideUtc`). Template, conversation, and script installation entry points lack the same end-to-end override fixture tests, leaving a wiring gap between options and `Find*References` results.

---

## Requirements

- R1. `FindTemplateResRefReferences_CaseSensitive_OverrideUtc` — insensitive finds mixed-case template; sensitive rejects wrong case, accepts exact case.
- R2. `FindConversationResRefReferences_CaseSensitive_OverrideUtc` — same for `Conversation` field.
- R3. `FindScriptReferences_CaseSensitive_OverrideUtc` — same for `ScriptHeartbeat` (or another populated script field).
- R4. ReferenceFinder filter count is **68** tests, all passing on net9.0.

---

## Scope Boundaries

- **In:** Three new tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs` using temp K1 stub install + override UTC (same pattern as plan 240 tag case test).
- **Out:** Production changes to `ReferenceFinder.cs` (behavior already implemented).
- **Out:** Chitin-only scope tests — no minimal KEY/BIF UTC fixture exists in-repo; defer to a future plan that adds a shared chitin test harness (see Deferred).

### Deferred to Follow-Up Work

- **Chitin-only ReferenceFinder scope:** Requires constructing `chitin.key` + BIF with embedded GFF UTC; track as plan 242+ once a reusable BioWare test helper exists (similar to `WriteModuleWithUtc` for modules).

---

## Context & Research

### Relevant Code and Patterns

- `tests/OdyTools.Tests/ReferenceFinderTests.cs` — `FindTagReferences_CaseSensitive_OverrideUtc` (lines ~1548–1600): temp install, `utc.Tag = "TestTag"`, insensitive vs sensitive `ReferenceSearchOptions`.
- `src/BioWare/Tools/ReferenceFinder.cs` — `FindTemplateResRefReferences`, `FindConversationResRefReferences`, `FindScriptReferences` all delegate to `SearchInstallation` with respective GFF scanners.
- Plan 240 completed partial-match and tag installation case; template/conversation partial at installation scope already landed in 240.

### Institutional Learnings

- Holocron port tests favor override-only scope (`SearchChitin = false`, `SearchModules = false`) to avoid real game installs.

---

## Key Technical Decisions

- **Mirror tag case test structure:** Reuse identical install bootstrap (`SWKOTOR.EXE`, override dir, `UTCHelpers.DismantleUtc`) for consistency and maintainability.
- **Script field choice:** Use `OnHeartbeat` / `ScriptHeartbeat` — already used across script reference tests.
- **Template field:** Set `utc.ResRef` so GFF exposes `TemplateResRef` (same as partial-match template test).
- **Conversation field:** Set `utc.Conversation` with mixed-case ResRef (e.g. `Test_Dlg`).

---

## Open Questions

### Resolved During Planning

- **Chitin in this slice?** No — defer; empty `chitin.key` does not populate `ChitinResources()` with scannable GFF.

### Deferred to Implementation

- None — tests are straightforward copies of existing pattern.

---

## Implementation Units

- U1. **Add template installation case-sensitivity test**

**Goal:** Cover R1.

**Requirements:** R1, R4

**Dependencies:** None

**Files:**
- Modify: `tests/OdyTools.Tests/ReferenceFinderTests.cs`

**Approach:**
- After `FindTagReferences_CaseSensitive_OverrideUtc`, add `FindTemplateResRefReferences_CaseSensitive_OverrideUtc`.
- `utc.ResRef = new ResRef("p_Creature_Tpl")`; search `p_creature_tpl` insensitive (hit), sensitive wrong case (empty), sensitive exact `p_Creature_Tpl` (hit); assert `TemplateResRef` field path.

**Patterns to follow:**
- `FindTagReferences_CaseSensitive_OverrideUtc`

**Test scenarios:**
- Happy path: `CaseSensitive = false` finds template with case-mismatched needle.
- Edge case: `CaseSensitive = true` with wrong-case needle returns empty.
- Happy path: `CaseSensitive = true` with exact-case needle returns `TemplateResRef`.

**Verification:**
- New test passes; total ReferenceFinder tests = 68.

---

- U2. **Add conversation installation case-sensitivity test**

**Goal:** Cover R2.

**Requirements:** R2, R4

**Dependencies:** None

**Files:**
- Modify: `tests/OdyTools.Tests/ReferenceFinderTests.cs`

**Approach:**
- Add `FindConversationResRefReferences_CaseSensitive_OverrideUtc` with `utc.Conversation = new ResRef("Test_Dlg")`.

**Test scenarios:**
- Same three assertions as U1 but for `Conversation` field path.

**Verification:**
- New test passes.

---

- U3. **Add script installation case-sensitivity test**

**Goal:** Cover R3.

**Requirements:** R3, R4

**Dependencies:** None

**Files:**
- Modify: `tests/OdyTools.Tests/ReferenceFinderTests.cs`

**Approach:**
- Add `FindScriptReferences_CaseSensitive_OverrideUtc` with `utc.OnHeartbeat = new ResRef("k_Test_Hb")`.

**Test scenarios:**
- Insensitive finds `k_test_hb`; sensitive rejects `k_test_hb`; sensitive accepts `k_Test_Hb`; assert `ScriptHeartbeat`.

**Verification:**
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder` reports 68 passed.

---

## System-Wide Impact

- **Interaction graph:** Test-only; no runtime or UI changes.
- **Unchanged invariants:** `ReferenceFinder` search semantics unchanged; tests document existing behavior.

---

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| ResRef normalization alters case in GFF bytes | Use same mixed-case values as byte-level case tests (`p_Carth`, `Test_Dlg`, `k_Test_Hb` patterns) |

---

## Sources & References

- **Origin document:** `docs/plans/2026-05-24-068-feat-reference-finder-installation-utc-plan.md`
- Prior slice: `docs/plans/2026-05-28-240-test-referencefinder-partial-case-completion-plan.md`
- Implementation: `src/BioWare/Tools/ReferenceFinder.cs`
- Tests: `tests/OdyTools.Tests/ReferenceFinderTests.cs`
