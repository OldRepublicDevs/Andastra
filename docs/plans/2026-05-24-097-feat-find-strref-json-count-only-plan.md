---
title: "feat: find-strref --json and --count-only"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-096-feat-find-refs-json-count-only-plan.md
branch: feat/holocron-port-phase-b
---

# feat: find-strref JSON and count-only output (plan 097)

## Summary

Extend machine-readable output modes to KotorCLI `find-strref`, reusing `ReferenceSearchOutputFormatter` from plan 096.

## Requirements

- R1. `--json` emits JSON with `needle` (StrRef id string), `type` (`strref`), `count`, and `references[]` via converted `ReferenceSearchResult` rows.
- R2. `--count-only` prints only the hit count.
- R3. Empty results: JSON `count: 0`; count-only `0`; exit 1 unchanged.
- R4. Human text output unchanged when neither flag is set.
- R5. Tests cover JSON hit, JSON miss, count-only hit/miss.

## Scope Boundaries

- No changes to `find-2da-ref` / `find-field-value` yet.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRef`
