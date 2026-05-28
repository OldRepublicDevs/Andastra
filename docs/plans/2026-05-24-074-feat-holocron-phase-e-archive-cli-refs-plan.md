---
title: "feat: Holocron port phase E — list/search archive CLI + ARE/IFO tag refs"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-073-feat-holocron-phase-d-cli-validation-ncs-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase E (plan 074)

## Summary

Wire remaining high-value KotorCLI archive STUBs and extend Holocron reference-finder UX to area/module editors.

## Requirements

- R1. `list-archive` lists ERF/RIM/MOD/SAV/HAK/BIF/KEY resources with optional filter and verbose size output; failures exit non-zero.
- R2. `search-archive` matches resource names (and optional content) by wildcard; no match exits non-zero.
- R3. `launch` STUB exits non-zero (no false success).
- R4. `OdyToolARE` and `OdyToolIFO` tag fields get find-references context menus.
- R5. Unit tests for archive list/search helpers.

## Deferred

- Full `launch` install+game spawn workflow.
- Module Designer 3D, Lip Syncer, PLT parser.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
