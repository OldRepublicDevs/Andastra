---
title: "feat: Holocron port phase M — StrRef find-references parity"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-081-feat-holocron-phase-l-ref-finder-coverage-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase M (plan 082)

## Summary

Wire BioWare `ReferenceCacheHelpers.FindStrRefReferences` into OdyToolTLK and KotorCLI with field-path display matching Holocron TLK editor behavior.

## Requirements

- R1. `ReferenceCacheHelpers.FormatStrRefLocation` + `ConvertToReferenceSearchResults` for Holocron-style field paths.
- R2. `StrRefReferenceHelper.FindAndShowStrRefReferences` in OdyTools; OdyToolTLK uses it instead of raw `FileResource` list.
- R3. KotorCLI `find-strref <strref>` with `--install-dir` / `--installation` alias.
- R4. Tests: `tests/BioWare.Tests/ReferenceCacheStrRefTests.cs`, `tests/KotorCLI.Tests/FindStrRefCommandTests.cs`.

## Deferred

- NCS StrRef bytecode scan enablement, 2DA memory reference CLI, reference cache persistence.

## Verification

- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter StrRef`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FindStrRef`
