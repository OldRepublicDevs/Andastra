---
title: "feat: Holocron port phase F — BIF extract naming + UTM/UTW/UTS/UTE tag refs"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-074-feat-holocron-phase-e-archive-cli-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase F (plan 075)

## Summary

Fix BIF single-file extraction to resolve resource names via KEY BIF index (matching ExtractKey logic), and wire Holocron reference-finder menus on remaining UT* editors.

## Requirements

- R1. `ExtractCommand.ExtractBif` resolves BIF index from KEY `BifEntries` filename and filters `KeyEntry` lookup by that index (remove STUB).
- R2. `OdyToolUTM`, `OdyToolUTW`, `OdyToolUTS`, `OdyToolUTE` get tag + template ResRef find-references context menus.
- R3. Standalone csproj files for those editors include ReferenceSearchHelper + dialog dependencies.
- R4. Unit tests for BIF index lookup helper and/or extract naming behavior.

## Deferred

- Full launch/install-to-game workflow
- Module Designer 3D, Lip Syncer, PLT parser

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
