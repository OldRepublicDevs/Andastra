---
title: "feat: odyTools module-glob in reference search dialog"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-100-feat-kotorcli-find-module-glob-plan.md
branch: feat/holocron-port-phase-b
---

# feat: odyTools module-glob in reference search dialog (plan 101)

## Summary

Wire `ReferenceSearchOptions.ModuleGlobFilters` into OdyTools `ReferenceSearchOptionsDialog` for GUI parity with KotorCLI `--module-glob` (deferred natural follow-up after plan 100).

## Requirements

- R1. Dialog adds optional module glob text field (when modules enabled): comma or newline separated patterns (`*`/`?`, case-insensitive filenames).
- R2. `SetDefaults` populates field from `ModuleGlobFilters`; empty/null clears field.
- R3. `ToSearchOptions` parses non-empty patterns into `List<string>` on options; blank field = null/empty filters (scan all modules).
- R4. Shared dialog used by `ReferenceSearchHelper`, `StrRefReferenceHelper`, `TwoDAMemoryReferenceHelper` — no per-caller changes beyond existing dialog usage.
- R5. Tests in `tests/OdyTools.Tests/ReferenceSearchOptionsDialogTests.cs`: round-trip defaults; parse multiple patterns; blank leaves filters empty.

## Scope Boundaries

- No new BioWare APIs (`ModuleGlobMatcher` already exists). No disk cache persistence.

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceSearchOptions`
