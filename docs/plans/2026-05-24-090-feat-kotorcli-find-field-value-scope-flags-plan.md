---
title: "feat: KotorCLI find-field-value scope flags"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-088-feat-kotorcli-strref-2da-ref-scope-flags-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI find-field-value scope flags (plan 090)

## Summary

Add installation scope flags to `find-field-value`, completing parity with `find-refs` / `find-strref` / `find-2da-ref`.

## Requirements

- R1. `FindFieldValueCommand` exposes `--override-only`, `--no-override`, `--no-chitin`, `--no-modules`.
- R2. Scope flags combine with existing `--partial` and `--case-sensitive` via `FindRefsCommand.BuildSearchOptions`.
- R3. Tests: override hit with `--override-only`; `--no-override` skips override-only fixture.
- R4. KotorCLI README documents scope flags for `find-field-value`.

## Scope Boundaries

- No BioWare API changes (`ReferenceFinder.FindFieldValueReferences` already accepts options).
- Reference cache persistence deferred.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindFieldValue`
