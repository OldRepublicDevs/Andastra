---
title: "docs: Sync OdyPatch host vs UI library architecture docs"
type: docs
status: completed
date: 2026-05-23
origin: plan 044 fixed run commands; NUGET, bioware-boundary, tools-ecosystem still say host library
---

# docs: Sync OdyPatch host vs UI library architecture docs

## Summary

Plan 044 corrected runnable entry points. Remaining docs still describe OdyPatch as a "host library" or "installer core" and omit the OdyPatch.UI README. Align architecture, NuGet, product UX, engine roadmap, and build ladder with the exe-host + UI-library split.

---

## Requirements

- R1. Fix `NUGET.md`, `bioware-library-boundary.md`, `tools-ecosystem.md` role descriptions.
- R2. Link `OdyPatch.UI/README.md` from `30-product-ux/README.md`.
- R3. Refresh `engine_roadmap.md` and `build-and-test-ladder.md` with host-run note.
- R4. Drift register remediation **#36**.

---

## Scope Boundaries

- No csproj or code changes.
