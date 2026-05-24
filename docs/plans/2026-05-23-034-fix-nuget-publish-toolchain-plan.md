---
title: "fix: Align NuGet publish scripts and manual push docs"
type: fix
status: completed
date: 2026-05-23
origin: plan 033 deferred MANUAL_PUSH + helper_scripts CSharpKOTOR drift
---

# fix: Align NuGet publish scripts and manual push docs

## Summary

`docs/MANUAL_PUSH_INSTRUCTIONS.md`, `docs/NUGET_SETUP.md`, and `helper_scripts/build-nuget.{ps1,sh}` still reference non-existent `CSharpKOTOR` and `src/OdyPatch/` paths. Align with plan 033 reality: **OdyPatch only** at `src/Tools/OdyPatch/`.

---

## Requirements

- R1. Rewrite `MANUAL_PUSH_INSTRUCTIONS.md` for OdyPatch package paths and nuget.org ID.
- R2. Update `NUGET_SETUP.md` to reference `helper_scripts/build-nuget.{ps1,sh}` and `helper_scripts/setup-nuget-key.ps1`.
- R3. Fix `helper_scripts/build-nuget.ps1` and `build-nuget.sh` — OdyPatch-only pack/push; correct csproj path; Linux uses `--framework net9.0`.
- R4. Cross-link from `docs/NUGET.md` to manual push doc.
- R5. Drift register remediation **#25**.

---

## Scope Boundaries

- Do not enable BioWare.NET.TSLPatcher packaging.
- No CI workflow changes.

---

## Test Scenarios

- TS1. `bash helper_scripts/build-nuget.sh` produces `.nupkg` under `src/Tools/OdyPatch/bin/Release/net9.0/` (or Release without TFM subfolder — verify after run).
- TS2. Grep docs for `CSharpKOTOR` in NuGet publish paths returns zero in updated files.
