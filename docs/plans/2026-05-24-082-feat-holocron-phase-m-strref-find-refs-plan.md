---
title: "feat: Holocron port phase M — StrRef find-references parity"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-081-feat-holocron-phase-l-ref-finder-coverage-plan.md
branch: feat/holocron-port-phase-b
follow_up_landed: 2026-05-28
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

- NCS StrRef bytecode scan enablement, reference cache persistence.

## Follow-up landed (2026-05-28, plans 261–263 on `feat/holocron-port-phase-b`)

- **261:** `StrRefReferenceHelper.FindAndShowStrRefReferences` guard clauses and override wiring smoke (**4** tests).
- **263:** `CollectStrRefReferences` empty-result precondition when StrRef has no override hits (**1** test in StrRef suite).
- **OdyTools:** **8** tests in `tests/OdyTools.Tests/StrRefReferenceHelperTests.cs`.

## Verification

- `dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter StrRef`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FindStrRef`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReferenceHelperTests` (**8** tests)

See plans `docs/plans/2026-05-28-261-*`, `263-*`, and `264-*` for wiring and closure slices.
