---
title: "feat: Holocron port phase K — DLG installation refs + find-refs flags"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-079-feat-holocron-phase-j-kotorcli-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase K (plan 080)

## Summary

Unify OdyToolDLG installation reference search with BioWare `ReferenceFinder` (conversation + script ResRefs) and complete KotorCLI `find-refs` scope/match flags from plan 079.

## Requirements

- R1. `OdyToolDLG.FindDialogReferencesInInstallation` delegates to `ReferenceSearchHelper.FindAndShowConversationReferences` (full installation scope + options dialog + `FileResultsDialog`).
- R2. `OdyToolDLG.FindReferencesToResref` for `script` type delegates to `ReferenceSearchHelper.FindAndShowScriptReferences` instead of override-only UTF-8 scan.
- R3. `FindRefsCommand` adds `--no-override`, `--case-sensitive`, `--partial`; accepts `--installation` alias for `--install-dir`.
- R4. Extend `tests/KotorCLI.Tests/FindRefsCommandTests.cs` with tag hit and `--partial` GFF bytes path via Execute.

## Deferred

- DLG-internal link graph changes, NCS bytecode reference cache, JSON CLI output.

## Verification

- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FindRefs`
