---
title: "feat: Holocron port phase G — extract integration, script refs, create-archive tests"
type: feat
status: complete
completed: 2026-05-24
date: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/holocron-port-phase-b
pr: 7
---

# feat: Holocron port phase G (plan 076)

## Summary

Close Phase F gaps: KEY-aware BIF list/extract via `ArchiveHelpers.MergeKeyDataIntoBif`, OdyTools reference/script menus on UTW/UTM/UTE, and KotorCLI integration tests for extract/create-archive.

---

## Problem Frame

Phase F routed BIF extract through BioWare `ArchiveHelpers`, but synthetic BIF+KEY round-trips still expose blank ResRefs on disk until KEY merge runs. OdyTools UTW XAML path and UTM/UTE script combos need reference-menu parity. KotorCLI needs testable `CreateArchiveCommand.Execute` and BIF+KEY coverage.

---

## Requirements

- R1. `MergeKeyDataIntoBif` resolves BIF filename in KEY (basename or full path) and maps `ResnameKeyIndex` / resource index to `KeyEntry.ResRef`.
- R2. `ArchiveHelpers.ListBif` + `ExtractBif` apply KEY names in integration tests (synthetic BIF+KEY on disk).
- R3. `OdyToolUTW` attaches reference menus after XAML controls exist (resref/tag non-null).
- R4. `OdyToolUTM` / `OdyToolUTE` script combo context menus include Find References; standalone csprojs include `ScriptReferenceHelper.cs`.
- R5. `CreateArchiveCommand.Execute` is public; RIM round-trip test in `CreateArchiveCommandTests.cs`.
- R6. C# 7.3, OdyPatch-only, per-file git commits on branch `feat/holocron-port-phase-b` (PR #7).

---

## Scope Boundaries

- No HoloPatcher, Module Designer 3D, Lip Syncer, PLT parser.
- No `list-archive` KEY wiring for BIF in this slice (helpers + tests only unless trivial).
- AgentDecompile skipped (tooling-only).

### Deferred to Follow-Up Work

- `ArchiveCommandHelpers` BIF list using sibling KEY/chitin.key.
- Full launch workflow and remaining KotorCLI utility STUBs.

---

## Key Technical Decisions

- **Single merge implementation:** Keep KEY→BIF name merge in `ArchiveHelpers.MergeKeyDataIntoBif`; KotorCLI delegates to `ArchiveHelpers` (no duplicate lookup in `ExtractCommand`).
- **Lookup keys:** Index by `ResIndex`, full `ResourceId`, and sequential BIF resource index fallback for synthetic archives.
- **Test surface:** Direct `ArchiveHelpers.ListBif` / `ExtractCommand.Execute` tests with temp BIF+KEY files.

---

## Implementation Units

- U1. **KEY merge fix** — `src/BioWare/Tools/Archives.cs`
  - **Test scenarios:** `ListBif_WithKey_AppliesKeyResourceNames`; `ExecuteExtractBif_WithKey_WritesNamedOutputFile`
  - **Verification:** KotorCLI.Tests green on net9.0

- U2. **KotorCLI extract delegation** — `src/Tools/KotorCLI/Commands/ExtractCommand.cs`
  - **Dependencies:** U1
  - **Verification:** Extract tests pass; no duplicate KEY merge logic

- U3. **Create-archive tests** — `CreateArchiveCommand.cs` (public `Execute`), `tests/KotorCLI.Tests/CreateArchiveCommandTests.cs`
  - **Test scenarios:** RIM pack from directory; readable via `LazyCapsule`

- U4. **OdyTools wiring** — `OdyToolUTW.axaml.cs`, `OdyToolUTM.axaml.cs`, `OdyToolUTE.axaml.cs`, standalone csprojs
  - **Verification:** `dotnet build` affected standalone editors net9.0 `-m:1`

---

## System-Wide Impact

- **Unchanged:** Game runtime, OdyPatch install path, ERF/RIM formats.
- **Integration:** `ArchiveHelpers` is shared by KotorCLI and any future callers of BIF extract/list.

---

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| KEY ResourceId vs ResnameKeyIndex mismatch | Test synthetic round-trip; lookup by ResIndex and masked id |
| Duplicate menu attach on UTW | Attach only after controls resolved (XAML or programmatic path once) |

---

## Sources & References

- **Origin:** `docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md`
- **Prior phase:** `docs/plans/2026-05-24-075-feat-holocron-phase-f-bif-extract-refs-plan.md`
- **Patterns:** `src/BioWare/Tools/Archives.cs`, `src/Tools/OdyTools/Editors/OdyToolUTC.axaml.cs`
