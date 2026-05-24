---
title: "fix: Restore OdyTools compile (C5 drift)"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md
---

# fix: Restore OdyTools compile (C5 drift)

## Summary

Restore a green `dotnet build` for `src/Tools/OdyTools/OdyTools.csproj` by fixing delegate-signature mismatches in `EditorHelpers.BindLostFocus` call sites and one `BindClick` call site, unblocking OdyPatch and closing drift register item C5.

---

## Problem Frame

OdyTools fails to compile on `net9.0` with 222× `CS1503` errors: editor commit handlers use `EventHandler` signatures `(object, EventArgs)` but `EditorHelpers.BindLostFocus` only accepts `System.Action`. OdyPatch depends on OdyTools; CI and local tool builds cannot include the patcher stack until this is fixed. `[REPO]`

---

## Requirements

- R1. `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0` succeeds with zero errors.
- R2. `dotnet build src/Tools/OdyPatch/OdyPatch.csproj --framework net9.0` succeeds (downstream verification).
- R3. No behavioral change to undo/commit semantics — only delegate wiring fixes.
- R4. Update drift register to mark C5 resolved and add remediation item 5.

---

## Scope Boundaries

- Do not refactor editor UI logic beyond delegate wiring.
- Do not add HoloPatcher references (OdyPatch-only policy).
- Do not fix unrelated OdyPatch.UI or game-runtime issues.

### Deferred to Follow-Up Work

- OdyPatch.UI Avalonia smoke tests and full UI regression: separate slice after compile green.

---

## Context & Research

### Relevant Code and Patterns

- `src/Tools/OdyTools/Editors/EditorHelpers.cs` — `BindLostFocus(Control, Action)` wraps `LostFocus` with `(s, e) => handler()`.
- Eight editor files define local `void OnCommit(object s, EventArgs e)` and pass method groups to `BindLostFocus`.
- `OdyToolUTE.axaml.cs` — `AddCreature(string resname = "", ...)` cannot convert to `Action` for `BindClick`.

### Institutional Learnings

- AGENTS.md documents OdyTools/OdyPatch as pre-existing compile failures on this branch; this slice targets the known method-group → Action class.

---

## Key Technical Decisions

- **Add `EventHandler` overload to `BindLostFocus`**: Preserves existing `Action` overload for `CommitEdits`, `CommitAndPush`, etc.; fixes all `OnCommit` call sites with a one-file change instead of 200+ call-site edits.
- **Wrap `AddCreature` in lambda for `BindClick`**: Minimal fix for optional-parameter method group that cannot bind to `Action`.

---

## Implementation Units

- U1. **Add EventHandler overload to BindLostFocus**

**Goal:** Allow `OnCommit(object, EventArgs)` method groups to bind to lost-focus handlers.

**Requirements:** R1, R3

**Dependencies:** None

**Files:**
- Modify: `src/Tools/OdyTools/Editors/EditorHelpers.cs`

**Approach:**
- Add `BindLostFocus(Control control, EventHandler handler)` that attaches handler directly to `control.LostFocus`.
- Keep existing `Action` overload unchanged.

**Test scenarios:**
- Test expectation: none — compile verification only; no test project for OdyTools editors.

**Verification:**
- OdyTools build error count drops from 222 to 2 (UTE BindClick only).

---

- U2. **Fix UTE AddCreature BindClick**

**Goal:** Resolve remaining CS1503 in OdyToolUTE.

**Requirements:** R1, R3

**Dependencies:** U1

**Files:**
- Modify: `src/Tools/OdyTools/Editors/OdyToolUTE.axaml.cs`

**Approach:**
- Replace `EditorHelpers.BindClick(_addCreatureButton, AddCreature)` with `() => AddCreature()` at both call sites.

**Test scenarios:**
- Test expectation: none — compile verification only.

**Verification:**
- OdyTools builds cleanly on net9.0.

---

- U3. **Verify OdyPatch build and update drift register**

**Goal:** Confirm downstream unblock and document C5 resolution.

**Requirements:** R2, R4

**Dependencies:** U1, U2

**Files:**
- Modify: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`

**Approach:**
- Run OdyPatch build after OdyTools green.
- Add C5 resolved entry and remediation item 5 done.

**Test scenarios:**
- Test expectation: none — documentation + build verification.

**Verification:**
- OdyPatch csproj builds; drift register reflects C5 resolved.

---

## System-Wide Impact

- **Interaction graph:** Editor lost-focus commit handlers unchanged in behavior; only delegate type alignment.
- **Unchanged invariants:** BioWare, NSSComp, and existing CI green paths unaffected.

---

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| Dual overload ambiguity | Signatures are distinct (`Action` vs `EventHandler`); no call site matches both |
| OdyPatch has additional errors | Verify with explicit OdyPatch build in U3 |

---

## Sources & References

- **Origin document:** docs/knowledgebase/40-operational-risk/documentation-drift-register.md
- Related code: `src/Tools/OdyTools/Editors/EditorHelpers.cs`
- PR: #2
