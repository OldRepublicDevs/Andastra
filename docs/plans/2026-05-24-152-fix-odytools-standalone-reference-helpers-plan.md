---
title: "fix: odyTools standalone reference helper wiring"
type: fix
status: completed
date: 2026-05-24
origin: ci-investigation-pr7-solution-build
branch: feat/holocron-port-phase-b
---

# fix: OdyTools standalone reference helper wiring (plan 152)

## Summary

Fix CI **Solution Build (net9.0)** failures: standalone editor csprojs compile `ComboBox2DA` via shared props but omit new reference-search helper sources; `OdyToolUTS` references nonexistent `ReferenceSearchOptionsDialog.axaml`.

## Requirements

- R1. Shared `OdyTools.Standalone.Editor.props` includes `TwoDAMemoryReferenceHelper`, `ReferenceSearchHelper`, and dialog code-behinds used by ComboBox2DA reference menus.
- R2. Per-editor csprojs add missing `ScriptReferenceHelper` (ARE, IFO) and `StrRefReferenceHelper` (SSF, TLK); remove duplicate shared includes and bogus UTS axaml reference.
- R3. `dotnet build Andastra.sln --framework net9.0 -m:1` succeeds locally.

## Verification

- `dotnet build Andastra.sln --framework net9.0 -m:1`
