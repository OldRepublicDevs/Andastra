---
title: "feat: KotorCLI find-strref / find-2da-ref scope flags"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-080-feat-holocron-phase-k-dlg-refs-cli-flags-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI StrRef and 2DA ref scope flags (plan 088)

## Summary

Add installation scope flags to `find-strref` and `find-2da-ref`, mirroring `find-refs` (`--override-only`, `--no-override`, `--no-chitin`, `--no-modules`).

## Requirements

- R1. `FindStrRefCommand` and `Find2DARefCommand` expose the four scope flags and `--installation` alias (already present).
- R2. Commands build `ReferenceSearchOptions` via `FindRefsCommand.BuildSearchOptions` and pass to BioWare cache helpers.
- R3. `ReferenceCacheHelpers.FindStrRefReferences` and `Find2DAMemoryReferences` honor optional `ReferenceSearchOptions` when enumerating resources.
- R4. Tests: override hit with `--override-only`; `--no-override` skips override-only fixture.

## Scope Boundaries

- No `--case-sensitive` / `--partial` on numeric StrRef / 2DA row searches.
- OdyTools UI options dialog deferred.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet build src/BioWare/BioWare.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FindStrRef`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~Find2DARef`
