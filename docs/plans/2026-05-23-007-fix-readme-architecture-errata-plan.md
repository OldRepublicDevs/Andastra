---
title: "fix: README architecture errata"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md remediation #3
---

# fix: README architecture errata

## Summary

Correct README architecture diagram, project list, directory tree, and contributor paths to match actual `src/Andastra/` layout and csproj names, using KB `10-architecture-runtime/` as source of truth. Close drift register item #3 and caveat C9.

---

## Problem Frame

README still lists fictional projects (`Andastra.Runtime.Games.Odyssey`, `Andastra.Runtime.Scripting`, `BioWare.NET`) and a tree with `Runtime/Scripting/` and `src/Andastra/Tests/`. KB and `src/` are authoritative; onboarding agents copy stale paths from README.

---

## Requirements

- R1. Replace architecture diagram with folder/csproj-accurate layering from `runtime-layering.md`
- R2. Replace project bullets with actual csproj names and engine folder paths under `Game/Games/`
- R3. Fix Project Structure tree (remove `Runtime/Scripting`, `BioWare.NET`, wrong test path; add `Core/`, `Graphics/`, `UI/`, `tests/`)
- R4. Update "Adding New Features" and Contributing paths to `Game/Games/{Engine}/` not `Runtime.Games.{Engine}`
- R5. Mark Infinity engine as planned/future (no `Game/Games/Infinity/` yet)
- R6. Resolve C9 in caveat register and remediation #3 in documentation drift register

---

## Scope Boundaries

- Full wiki Home.md rebrand (drift item #4) — deferred
- `.cursorrules` NcsTool path (C11) — deferred
- Refactoring code to match old README names — out of scope
- OdyTools/OdyPatch build recovery — out of scope

---

## Context & Research

### Relevant Code and Patterns

- `docs/knowledgebase/10-architecture-runtime/game-vs-runtime-split.md` — README→actual mapping
- `docs/knowledgebase/10-architecture-runtime/runtime-layering.md` — corrected diagram
- `docs/knowledgebase/50-execution/contributing-paths.md` — contributor targets

### Institutional Learnings

- Plans 001–003 deferred full README rewrite; incremental errata is accepted
- Tool paths (NSSComp, NCSDecomp.CLI) already fixed in README 2026-05-23

---

## Key Technical Decisions

- **Inline rewrite over errata-only callout**: Diagram and project list are wrong throughout; a targeted rewrite of affected sections is clearer than a disclaimer block
- **Folder paths + csproj names**: Use `Andastra.Runtime`, `Andastra.Game`, `Game/Games/Odyssey/` rather than resurrecting fictional dotted project names
- **Infinity labeled future**: Avoid implying implemented engine family

---

## Implementation Units

- U1. **Update README architecture sections**

**Goal:** Align README diagram, project organization, structure tree, and contributor guidance with repo layout.

**Requirements:** R1–R5

**Dependencies:** None

**Files:**
- Modify: `README.md`

**Approach:**
- Replace ASCII diagram per `runtime-layering.md` (Game layer, Runtime folders inside `Andastra.Runtime.csproj`, BioWare base)
- Project list: `Andastra.Runtime`, `Andastra.Game`, `Andastra.Core`, `Andastra.Graphics`, `Andastra.UI`, `BioWare`; engine folders under `Game/Games/`
- Fix tree: `tests/Andastra.Tests`, `src/BioWare/`, `Game/Scripting/`, remove `Runtime/Scripting`
- Fix supporting tools: `OdyTools` not `OdyTools.NET`; `NCSDecomp.CLI`

**Test scenarios:**
- Test expectation: none — documentation-only change

**Verification:**
- No remaining `Runtime.Games.` or `BioWare.NET` in README architecture/contributing sections
- Diagram references `Game/Scripting/` not `Runtime.Scripting`

- U2. **Close KB drift registers**

**Goal:** Mark remediation complete in KB meta docs.

**Requirements:** R6

**Dependencies:** U1

**Files:**
- Modify: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`
- Modify: `docs/knowledgebase/90-meta/caveat-register.md`
- Modify: `docs/knowledgebase/90-meta/README.md` (remove stale README note if present)

**Test scenarios:**
- Test expectation: none — documentation-only

**Verification:**
- C9 resolved; drift register item #3 marked done

---

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| Over-scoping into wiki refresh | Explicit deferral in scope |
| Reintroducing HoloPatcher naming | Use OdyPatch only per repo policy |

---

## Sources & References

- Origin: `docs/knowledgebase/40-operational-risk/documentation-drift-register.md`
- Related: PR #2, plans 001–006
