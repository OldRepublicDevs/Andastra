---
title: "docs: Sync onboarding docs after C5 recovery"
type: docs
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md
---

# docs: Sync onboarding docs after C5 recovery

## Summary

Update `docs/QUICKSTART.md` and `docs/engine_roadmap.md` — the primary onboarding entry points linked from README — to remove stale OdyTools/OdyPatch failure claims and reflect post-plan-013–016 build status.

---

## Problem Frame

KB runbooks and build-health docs were synced in plan 016, but README-linked onboarding files still tell new contributors that OdyTools blocks OdyPatch and full solution builds. `[REPO]`

---

## Requirements

- R1. `QUICKSTART.md` reflects OdyTools/OdyPatch green compile path and correct tool run commands.
- R2. `engine_roadmap.md` tools table matches `tools-ecosystem.md`.
- R3. Remove resolved README architecture drift note (fixed plan 007).
- R4. Drift register remediation item 8.
- R5. Refresh PR #2 body to summarize plans 001–016 (step 7).

---

## Implementation Units

- U1. **Update QUICKSTART.md**

**Files:** `docs/QUICKSTART.md`

**Verification:** No "blocked by OdyTools" language remains.

---

- U2. **Update engine_roadmap.md**

**Files:** `docs/engine_roadmap.md`

**Verification:** Tools table shows OdyTools/OdyPatch green; README diagram gap removed.

---

- U3. **Drift register + PR body**

**Files:** `documentation-drift-register.md`; PR #2 via `gh pr edit`

---

## Scope Boundaries

- Do not expand engine_roadmap subsystem detail beyond tool status fixes.
