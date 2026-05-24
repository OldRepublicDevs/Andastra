---
title: "fix: obsolete performance and monogame doc paths"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md Obsolete Paths
---

# fix: obsolete performance and monogame doc paths

## Summary

Remove the broken `CSharpKOTOR.Tests.csproj` reference from the Performance test helper project and update `docs/MONOGAME_RUNNING.md` to use current `Andastra.Game` paths. Resolve drift register entries and caveat C2.

---

## Problem Frame

`tests/BioWare.Tests/Performance/Performance.csproj` references a non-existent `CSharpKOTOR.Tests.csproj`, causing MSB9008 warnings on build. `docs/MONOGAME_RUNNING.md` still documents obsolete `OdysseyRuntime` paths superseded by `src/Andastra/Game/`.

---

## Requirements

- R1. Fix Performance.csproj so it builds without missing project reference warnings
- R2. Update MONOGAME_RUNNING.md to document `Andastra.Game.csproj` and current CLI flags
- R3. Remove obsolete Build-MonoGame.ps1 reference if script does not exist
- R4. Mark Performance.csproj and MONOGAME_RUNNING.md resolved in drift register; resolve C2

---

## Scope Boundaries

- NCSDecomp_*.md path sweep — deferred (multiple files)
- REVERSE_ENGINEERING_PROJECT_SUMMARY.md — deferred

---

## Implementation Units

- U1. **Fix Performance.csproj reference**

**Goal:** Clean build with no missing project reference.

**Requirements:** R1

**Files:**
- Modify: `tests/BioWare.Tests/Performance/Performance.csproj`

**Approach:** Remove unused `ProjectReference` to `CSharpKOTOR.Tests.csproj` — Performance helpers only depend on NUnit.

**Test scenarios:**
- `dotnet build tests/BioWare.Tests/Performance/Performance.csproj --framework net9.0` succeeds with 0 warnings MSB9008

**Verification:** Build clean

- U2. **Update MONOGAME_RUNNING.md and KB registers**

**Goal:** Doc matches `run-game-runtime.md` authority.

**Requirements:** R2–R4

**Dependencies:** None (parallel with U1)

**Files:**
- Modify: `docs/MONOGAME_RUNNING.md`
- Modify: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`
- Modify: `docs/knowledgebase/90-meta/caveat-register.md`
- Modify: `docs/knowledgebase/50-execution/run-game-runtime.md` (remove "obsolete" warning once fixed)

**Verification:** No `OdysseyRuntime` paths in MONOGAME_RUNNING.md; C2 resolved

---

## Sources & References

- `docs/knowledgebase/50-execution/run-game-runtime.md`
- Caveat C2
