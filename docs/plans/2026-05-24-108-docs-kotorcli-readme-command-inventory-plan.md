---
title: "docs: kotorcli readme command inventory accuracy"
type: docs
status: complete
date: 2026-05-24
branch: feat/holocron-port-phase-b
origin: docs/plans/2026-05-24-106-feat-kotorcli-format-convert-closure-plan.md
---

# docs: KotorCLI README command inventory (plan 108)

## Summary

Replace stale KotorCLI README command tables that still label most commands as stubs with an accurate wired / partial / stub inventory aligned to `Program.cs` and command implementations on `feat/holocron-port-phase-b`.

## Problem Frame

Plans 100–107 and Holocron port work wired reference search, format conversion, archive tools, script tools, and much of the build pipeline. The README `Status`, core/archive command bullets, and `Known Issues` still describe an early stub-only tree, which misleads mod authors and agents.

OdyTools `TwoDAMemoryReferenceHelper` row-sweep tests (label + StrRef with supplied `twoDA`) landed in plan 107 follow-up commits; no further test work in this slice.

## Requirements

- R1. `Status` and command inventory sections distinguish **wired**, **partial**, and **stub** without claiming all commands are unimplemented.
- R2. Reference-search documentation (find-* commands) remains accurate and unchanged in substance.
- R3. `Known Issues` / `Next Steps` reflect real gaps (`launch` fail-fast stub, `unpack --removeDeleted` placeholder, limited integration tests) — not blanket “all stubs”.
- R4. No production C# changes unless review autofix requires trivial doc-adjacent fixes.

## Scope Boundaries

- No new CLI commands or behavior changes.
- No AgentDecompile (tooling/docs only).
- No duplicate plan 106 integration tests (already present).

## Key Technical Decisions

- **Inventory source of truth:** `src/Tools/KotorCLI/Program.cs` registration plus spot-check of `Execute` bodies; only `launch` is explicitly fail-fast stub.
- **Label scheme:** `wired` = functional CLI with BioWare/Conversions implementation; `partial` = works with known gaps; `stub` = fail-fast or unimplemented pipeline step.

## Implementation Units

- U1. **README command inventory rewrite**

**Goal:** Accurate command status tables in `src/Tools/KotorCLI/README.md`.

**Files:**
- Modify: `src/Tools/KotorCLI/README.md`

**Approach:**
- Update `Status` paragraph.
- Replace per-section stub lists with wired/partial/stub markers for core build, archive, format convert, script, resource, utility, validation.
- Refresh `Known Issues` and `Next Steps`.

**Test scenarios:**
- Test expectation: none — documentation-only; manual spot-check against `Program.cs`.

**Verification:**
- README lists `launch` as stub and `find-*` / `gff2json` as wired.

- U2. **Mark plan complete**

**Goal:** Close plan 108 after README lands.

**Files:**
- Modify: `docs/plans/2026-05-24-108-docs-kotorcli-readme-command-inventory-plan.md` (`status: complete`)

**Verification:** Plan status flipped after commit.

## System-Wide Impact

- **API surface parity:** None — docs only.
- **Unchanged invariants:** All CLI behavior unchanged.

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| Drift recurs as commands land | Next Steps call for README updates alongside new command PRs |

## Sources & References

- `src/Tools/KotorCLI/README.md`
- `src/Tools/KotorCLI/Program.cs`
- `docs/plans/2026-05-24-106-feat-kotorcli-format-convert-closure-plan.md`
