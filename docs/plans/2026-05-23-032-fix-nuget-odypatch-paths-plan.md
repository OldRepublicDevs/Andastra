---
title: "docs: Fix NUGET OdyPatch paths and sync CI build notes"
type: docs
status: completed
date: 2026-05-23
origin: PR #2 post-031 doc audit
---

# docs: Fix NUGET OdyPatch paths and sync CI build notes

## Summary

`docs/NUGET.md` still references `src/OdyPatch/` for pack/push commands. KB build-health and meta README omit plan 031 `-m:1` CI serialization for solution-build.

---

## Requirements

- R1. Update OdyPatch pack/push paths in `docs/NUGET.md` to `src/Tools/OdyPatch/`.
- R2. Note in `build-health-matrix.md` that CI `solution-build` uses `-m:1`; local full solution may build parallel.
- R3. Update `90-meta/README.md` solution-build row with `-m:1` caveat.
- R4. Drift register remediation **#23** (NUGET OdyPatch paths).

---

## Scope Boundaries

- Full NUGET.md rewrite for missing `TSLPatcher.Core` project — defer; only OdyPatch path fix.
