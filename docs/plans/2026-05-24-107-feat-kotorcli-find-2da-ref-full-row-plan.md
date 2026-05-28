---
title: "feat: kotorcli find-2da-ref --full-row parity"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-28
branch: feat/holocron-port-phase-b
closure: docs/plans/2026-05-28-276-docs-kotorcli-find-2da-ref-full-row-closure-plan.md
---

# feat: KotorCLI find-2da-ref --full-row (plan 107)

## Completion (2026-05-28)

All requirements R1–R5 landed before this closure slice. Authority: plan **276**.

| Req | Status | Evidence |
|-----|--------|----------|
| R1 | **Landed** | `ReferenceCacheHelpers.CollectTwoDARowReferences` in `src/BioWare/Tools/ReferenceCache.cs` |
| R2 | **Landed** | `TwoDAMemoryReferenceHelper.CollectTwoDARowReferences` delegates to BioWare |
| R3 | **Landed** | `Find2DARefCommand` `--full-row` loads 2DA when available |
| R4 | **Landed** | `ReferenceCacheHelpersTwoDARowReferencesTests` (**2**); `Find2DARefCommandTests` (**13** total filter) |
| R5 | **Landed** | `src/Tools/KotorCLI/README.md` documents `--full-row` |

**Verification (2026-05-28):**

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Find2DARef
# Passed: 13

dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDARow
# Passed: 2
```

## Summary

Expose Holocron/OdyTools full 2DA row reference sweep on KotorCLI by moving `CollectTwoDARowReferences` into BioWare and adding `--full-row` to `find-2da-ref`, which loads the target 2DA from the installation when available and includes label field-value and StrRef column matches in addition to GFF 2DA memory references.

## Problem Frame

OdyTools already sweeps an entire 2DA row (memory refs, row label as GFF field value, positive StrRef cells) via `TwoDAMemoryReferenceHelper.CollectTwoDARowReferences`, but KotorCLI `find-2da-ref` only calls `ReferenceCacheHelpers.Find2DAMemoryReferences`. Mod authors using the CLI lack parity with the GUI tool.

## Requirements

- R1. `ReferenceCacheHelpers.CollectTwoDARowReferences` in BioWare mirrors OdyTools row-sweep behavior (memory, label `FindFieldValueReferences`, StrRef column `FindStrRefReferences`).
- R2. `TwoDAMemoryReferenceHelper.CollectTwoDARowReferences` delegates to BioWare (thin wrapper over `OdyInstallation.Installation`).
- R3. KotorCLI `find-2da-ref --full-row` loads 2DA from installation when present and uses `CollectTwoDARowReferences`; without the flag, behavior unchanged.
- R4. BioWare unit tests for row sweep; KotorCLI integration test with override 2DA + referencing GFF/SSF.
- R5. Minimal README update documenting `--full-row`.

## Scope Boundaries

- No new cache format; existing `--cache-file` applies to the memory-ref portion only.
- No AgentDecompile (tooling-only).
- No changes to `find-field-value` / `find-strref` commands beyond shared BioWare helper.

## Key Technical Decisions

- **Shared helper location:** `ReferenceCacheHelpers` in `src/BioWare/Tools/ReferenceCache.cs` — same module as `Find2DAMemoryReferences` and `FindStrRefReferences`.
- **2DA load:** `installation.Resource(resname, ResourceType.TwoDA)` then `TwoDA.FromBytes`; if missing, full-row mode still runs memory refs only (same as OdyTools with `twoDA == null`).
- **CLI flag:** `--full-row` boolean on `Find2DARefCommand`.

## Implementation Units

- U1. **BioWare CollectTwoDARowReferences**

**Goal:** Centralize row-sweep logic in BioWare.

**Files:**
- Modify: `src/BioWare/Tools/ReferenceCache.cs`
- Test: `tests/BioWare.Tests/ReferenceCacheTwoDARowSweepTests.cs`

**Test scenarios:**
- Happy path: override 2DA row label referenced in UTC Tag field; `CollectTwoDARowReferences` with loaded TwoDA returns field-value hit.
- Happy path: 2DA cell StrRef referenced via SSF in override; full sweep includes strref hit.
- Edge case: `twoDA == null` — only memory refs (no label/strref sweep).

**Verification:** `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~TwoDARow`

- U2. **OdyTools thin delegate**

**Goal:** OdyTools calls BioWare; no duplicate logic.

**Files:**
- Modify: `src/Tools/OdyTools/Utils/TwoDAMemoryReferenceHelper.cs`

**Test scenarios:** Existing `tests/OdyTools.Tests/TwoDAMemoryReferenceHelperTests.cs` still pass.

**Verification:** OdyTools tests unchanged behavior.

- U3. **KotorCLI --full-row + README**

**Goal:** CLI parity and docs.

**Files:**
- Modify: `src/Tools/KotorCLI/Commands/Find2DARefCommand.cs`
- Modify: `src/Tools/KotorCLI/README.md`
- Test: `tests/KotorCLI.Tests/Find2DARefCommandTests.cs`

**Test scenarios:**
- Integration: `--full-row` on fixture with override 2DA + UTC Tag matching row label exits 0.
- Regression: without `--full-row`, same fixture (no memory ref) exits 1.

**Verification:** `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Find2DARef`

## Risks & Dependencies

| Risk | Mitigation |
|------|------------|
| Duplicate results when memory ref and field value overlap | Accept union (OdyTools behavior); formatter dedupes by display if needed later |
| Large 2DA StrRef sweep slow | Document; same as OdyTools |

## Documentation / Operational Notes

- README: one line under `find-2da-ref` flags for `--full-row`.

## Sources & References

- `src/Tools/OdyTools/Utils/TwoDAMemoryReferenceHelper.cs` — existing row sweep
- `docs/plans/2026-05-24-089-feat-combobox2da-reference-options-dialog-plan.md` — prior OdyTools row refs
