---
title: "test: kotorcli extract bif key filter"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-24-158-test-kotorcli-extract-filter-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract BIF+KEY filter (plan 159)

## Summary

Integration tests for `extract --filter` when unpacking BIF archives with a sibling KEY file so resource names come from KEY entries rather than numeric BIF indices. Closes the parity gap left after plan 158 (RIM-only filter coverage).

---

## Problem Frame

Plan 158 verified wildcard filtering for RIM extraction. BIF extraction already supports `--filter` and `--key-file` in `ExtractCommand`, and `ArchiveHelpers.ExtractBif` applies the filter against KEY-resolved resrefs, but no integration test asserts filtered output filenames or exclusion behavior for BIF+KEY pairs.

---

## Requirements

- R1. `extract --file sample.bif --key-file sample.key --filter creature_a*` writes only `creature_a.utc` when the KEY maps two resources (`creature_a`, `creature_b`).
- R2. Filter with no KEY-name matches exits zero and writes no output files (empty output directory).

---

## Scope Boundaries

- No changes to `ExtractCommand` or `ArchiveHelpers` behavior unless tests reveal a bug.
- No ERF/MOD filter tests (deferred follow-up).
- No `launch` implementation.
- No game engine / AgentDecompile work.

### Deferred to Follow-Up Work

- ERF/MOD `extract --filter` parity tests: separate plan after BIF+KEY closure.

---

## Context & Research

### Relevant Code and Patterns

- `src/Tools/KotorCLI/Commands/ExtractCommand.cs` — `ExtractBif` passes filter to `ArchiveHelpers.ExtractBif`.
- `src/BioWare/Tools/Archives.cs` — `ExtractBif` merges KEY names then applies `MatchesFilter`.
- `tests/KotorCLI.Tests/ExtractCommandTests.cs` — RIM filter tests (plan 158), BIF+KEY happy path without filter.
- `WriteSampleBifKeyPair` helper pattern in same test file and `ArchiveCommandsTests.cs`.

---

## Key Technical Decisions

- **Reuse existing test helpers:** Extend `ExtractCommandTests` with a two-entry BIF+KEY fixture rather than new test class.
- **Mirror plan 158 scenarios:** Same wildcard match and no-match cases, different archive type.

---

## Implementation Units

- U1. **BIF+KEY extract filter integration tests**

**Goal:** Assert filtered extraction uses KEY-resolved names.

**Requirements:** R1, R2

**Dependencies:** None

**Files:**
- Modify: `tests/KotorCLI.Tests/ExtractCommandTests.cs`
- Modify: `src/Tools/KotorCLI/README.md` (test count if changed)

**Approach:**
- Add helper to write BIF with two KEY-mapped resources at distinct indices.
- Add `ExecuteExtractBif_WithKeyAndFilter_ExtractsMatchingResourceOnly` — filter `creature_a*`, assert `creature_a.utc` exists, `creature_b.utc` absent.
- Add `ExecuteExtractBif_WithKeyAndFilterNoMatch_WritesNoFiles` — filter `missing_*`, exit 0, zero output files.

**Patterns to follow:**
- Plan 158 RIM filter tests in `ExtractCommandTests.cs`
- `WriteSampleBifKeyPair` for KEY naming setup

**Test scenarios:**
- Happy path: two KEY-named resources, wildcard filter selects one, named output file on disk.
- Edge case: filter matches no KEY resrefs, exit 0, empty output directory.

**Verification:**
- ExtractCommand filter tests pass.
- Full KotorCLI.Tests suite passes on net9.0.

---

## System-Wide Impact

- **Unchanged invariants:** RIM/ERF extract paths, create-archive filter, list/search archive commands.

---

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| BIF multi-resource fixture incorrect | Follow existing `SetData(..., resIndex)` pattern from `WriteSampleBifKeyPair` |

---

## Sources & References

- **Origin document:** docs/plans/2026-05-24-158-test-kotorcli-extract-filter-plan.md
- Related code: `ExtractCommandTests.cs`, `Archives.cs` (`ExtractBif`)
