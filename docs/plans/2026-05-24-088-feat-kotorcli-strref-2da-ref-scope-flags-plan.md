---
title: "feat: KotorCLI find-strref and find-2da-ref scope flags"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-080-feat-holocron-phase-k-dlg-refs-cli-flags-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI find-strref and find-2da-ref scope flags (plan 088)

## Summary

Add installation scope flags to KotorCLI `find-strref` and `find-2da-ref`, mirroring `find-refs` (`--override-only`, `--no-override`, `--no-chitin`, `--no-modules`). Thread `ReferenceSearchOptions` into BioWare cache scanners so scope is enforced during resource enumeration, not post-filtered.

---

## Problem Frame

Plans 082/083 wired StrRef and 2DA memory reference search into KotorCLI with install-dir only. Plan 080 added scope flags to `find-refs`; README documents shared scope flags but `find-strref` and `find-2da-ref` lack them, breaking CLI parity with Holocron reference search options.

---

## Requirements

- R1. `find-strref` accepts `--override-only`, `--no-override`, `--no-chitin`, `--no-modules`; passes scoped `ReferenceSearchOptions` to `ReferenceCacheHelpers.FindStrRefReferences`.
- R2. `find-2da-ref` accepts the same scope flags; passes scoped options to `ReferenceCacheHelpers.Find2DAMemoryReferences`.
- R3. BioWare scanners honor optional `ReferenceSearchOptions` (default null = full installation scan, preserving existing callers).
- R4. KotorCLI tests cover `--override-only` hit and `--no-override` skip for both commands; README documents scope flags for both commands.

---

## Scope Boundaries

- No `--case-sensitive` / `--partial` (not applicable to numeric StrRef / 2DA row search).
- No OdyTools `ReferenceSearchOptionsDialog` wiring (deferred in plan 084).
- No reference cache persistence changes.

---

## Key Technical Decisions

- Reuse `FindRefsCommand.BuildSearchOptions` for consistent scope semantics across CLI reference commands.
- Extend `ReferenceCacheHelpers.GetAllResources` with optional `ReferenceSearchOptions`; null preserves current full-scan behavior for OdyTools callers.
- Add optional trailing `ReferenceSearchOptions` parameter to cache finder methods (backward compatible).

---

## Implementation Units

- U1. **BioWare scoped resource enumeration**

**Goal:** `ReferenceCacheHelpers` respects `ReferenceSearchOptions` when enumerating installation resources.

**Requirements:** R3

**Dependencies:** None

**Files:**
- Modify: `src/BioWare/Tools/ReferenceCache.cs`
- Test: `tests/BioWare.Tests/ReferenceCacheStrRefTests.cs`

**Approach:**
- Add optional `ReferenceSearchOptions options = null` to private `GetAllResources`.
- When options non-null, mirror `ReferenceFinder.EnumerateResources` scope gates (chitin/core, override, modules).
- Add optional trailing `options` parameter to `FindStrRefReferences` and `Find2DAMemoryReferences`; pass through to `GetAllResources`.

**Test scenarios:**
- Happy path: `FindStrRefReferences` with override-only options finds SSF in override fixture (existing test install pattern).
- Edge case: `FindStrRefReferences` with `SearchOverride = false` returns empty for override-only fixture.

**Verification:**
- BioWare StrRef tests pass with scoped enumeration.

---

- U2. **FindStrRefCommand scope flags**

**Goal:** CLI exposes and applies scope flags for StrRef search.

**Requirements:** R1

**Dependencies:** U1

**Files:**
- Modify: `src/Tools/KotorCLI/Commands/FindStrRefCommand.cs`
- Test: `tests/KotorCLI.Tests/FindStrRefCommandTests.cs`

**Approach:**
- Register scope CLI options matching `FindRefsCommand`.
- Extend `Execute` with scope parameters; build options via `FindRefsCommand.BuildSearchOptions`.
- Keep existing 3-arg `Execute(int, string, ILogger)` overload delegating with defaults.

**Test scenarios:**
- Happy path: override SSF hit with `overrideOnly: true` exits 0.
- Edge case: `--no-override` skips override-only fixture, exits 1.

**Verification:**
- `FindStrRef` KotorCLI tests pass.

---

- U3. **Find2DARefCommand scope flags**

**Goal:** CLI exposes and applies scope flags for 2DA memory search.

**Requirements:** R2

**Dependencies:** U1

**Files:**
- Modify: `src/Tools/KotorCLI/Commands/Find2DARefCommand.cs`
- Test: `tests/KotorCLI.Tests/Find2DARefCommandTests.cs`

**Approach:** Same pattern as U2 for `find-2da-ref`.

**Test scenarios:**
- Happy path: override UTC appearance row hit with `overrideOnly: true` exits 0.
- Edge case: `--no-override` skips override-only fixture, exits 1.

**Verification:**
- `Find2DARef` KotorCLI tests pass.

---

- U4. **README scope flag documentation**

**Goal:** README lists scope flags for `find-strref` and `find-2da-ref`.

**Requirements:** R4

**Dependencies:** U2, U3

**Files:**
- Modify: `src/Tools/KotorCLI/README.md`

**Test expectation:** none — documentation only.

**Verification:**
- Flag names match command option registration.

---

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRef`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Find2DARef`

---

## Sources & References

- Origin: `docs/plans/2026-05-24-080-feat-holocron-phase-k-dlg-refs-cli-flags-plan.md`
- Pattern: `src/Tools/KotorCLI/Commands/FindRefsCommand.cs`
- BioWare: `src/BioWare/Tools/ReferenceCache.cs`, `src/BioWare/Tools/ReferenceFinder.cs`
