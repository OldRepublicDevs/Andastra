---
title: "feat: Holocron port phase D — check-txi, cat, RIM pack, NCS script refs"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-072-feat-holocron-phase-c-resource-merge-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase D (plan 073)

## Summary

Continue PyKotor/Holocron CLI and reference-finder parity on PR #7 branch: wire remaining KotorCLI STUBs (`check-txi`, `cat`, RIM `pack`) and extend script reference search to NCS bytecode.

## Requirements

- R1. KotorCLI `check-txi` delegates to `BioWare.Tools.Validation.CheckTxiFiles`; missing TXI exits non-zero.
- R2. KotorCLI `cat` reads ERF/RIM/MOD resources and writes bytes to stdout; missing resource exits non-zero.
- R3. KotorCLI `pack` supports `.rim` output using same cache-file loop as ERF.
- R4. `ReferenceFinder.FindScriptReferences` includes NCS resources via byte-offset scan (`FindScriptResRefInNcsBytes`).
- R5. Unit tests for check-txi, cat, RIM pack, and NCS script reference scan.

## Deferred

- Full NCS CONST instruction parsing (deferred beyond byte scan).
- Module Designer 3D, Lip Syncer, PLT parser.

## Verification

- `dotnet build Andastra.sln --framework net9.0 -m:1`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder`
