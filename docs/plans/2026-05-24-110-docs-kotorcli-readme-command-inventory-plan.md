---
title: "docs: kotorcli readme command inventory accuracy"
type: docs
status: complete
date: 2026-05-24
branch: feat/holocron-port-phase-b
origin: docs/plans/2026-05-24-106-feat-kotorcli-format-convert-closure-plan.md
---

# docs: KotorCLI README command inventory (plan 110)

## Summary

Replace stale KotorCLI README command tables with an accurate **wired** / **partial** / **stub** inventory aligned to `Program.cs` and command implementations.

## Requirements

- R1. Status and command sections distinguish wired/partial/stub without claiming all commands are unimplemented.
- R2. Reference-search documentation remains accurate.
- R3. Known Issues / Next Steps reflect real gaps (`launch` stub, `unpack --removeDeleted` placeholder).
- R4. Documentation-only slice — no production C# changes.

## Verification

- Manual spot-check against `src/Tools/KotorCLI/Program.cs` and `LaunchCommand.cs`.
