---
title: "feat: Holocron port phase H — xaml ref menus, uts script refs, cli polish"
type: feat
status: complete
completed: 2026-05-24
date: 2026-05-24
origin: docs/plans/2026-05-24-076-feat-holocron-phase-g-script-refs-archive-tests-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase H (plan 077)

## Summary

Extend reference-finder and script-ref wiring to remaining Holocron editors (XAML-loaded paths), add UTS script find-references, and polish KotorCLI archive error messages.

## Requirements

- R1. `AttachReferenceSearchMenus()` for XAML-init path on UTC, UTD, UTP, UTT, UTI, ARE, IFO (mirror UTW/UTM pattern).
- R2. `OdyToolUTS` script combo context menu includes Find References via `ScriptReferenceHelper`.
- R3. KotorCLI `launch` command documents fail-fast behavior; optional `--dry-run` lists resolved game path without spawn.
- R4. BioWare KEY `BytesKey` round-trip regression test in BioWare.Tests or KotorCLI.Tests.

## Deferred

- Full launch + game spawn, Module Designer 3D, Lip Syncer, PLT parser, OdyPatch E2E on real installs.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1`
