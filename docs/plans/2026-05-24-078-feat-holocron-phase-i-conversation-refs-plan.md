---
title: "feat: Holocron port phase I — conversation find-references"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-077-feat-holocron-phase-h-xaml-refs-uts-cli-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase I (plan 078)

## Summary

Add BioWare conversation (DLG) ResRef reference search and wire UTC/UTD/UTP conversation combo context menus.

## Requirements

- R1. `ReferenceFinder.FindConversationResRefReferences` matches GFF `Conversation` ResRef fields.
- R2. `ConversationReferenceHelper` + editor combo context menu **Find References** on UTC, UTD, UTP.
- R3. Unit tests in `tests/OdyTools.Tests/ReferenceFinderTests.cs`.

## Deferred

- DLG-internal cross-reference graph search, Module Designer 3D, full launch spawn.

## Verification

- `dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder`
- `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0 -m:1`
