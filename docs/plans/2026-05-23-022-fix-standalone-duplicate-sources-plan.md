---
title: "fix: Remove duplicate standalone compile and NAudio refs"
type: fix
status: completed
date: 2026-05-23
origin: Andastra.sln build CS2002/NU1504 warnings
---

# fix: Remove duplicate standalone compile and NAudio refs

## Summary

`OdyTools.Standalone.Editor.props` already includes `IMediaPlayer.cs`, `NAudioMediaPlayer.cs`, and `NAudio` package. WAV/DLG/SSF standalones duplicate these, causing NU1504 and CS2002 during full solution builds.

---

## Requirements

- R1. Remove duplicate `Compile` and `PackageReference` entries from WAV, DLG, SSF standalones.
- R2. `dotnet build Andastra.sln --framework net9.0` — no CS2002/NU1504 from these projects.
- R3. Drift register remediation 13.

---

## Implementation Units

- U1. Edit `OdyToolWAV.Standalone.csproj`, `OdyToolSSF.Standalone.csproj`, `DLG/OdyToolDLG.Standalone.csproj`
- U2. Verify solution build
- U3. Update documentation-drift-register

---

## Scope Boundaries

- Do not refactor shared props to conditional audio includes in this slice.
