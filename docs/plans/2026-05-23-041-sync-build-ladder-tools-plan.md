---
title: "docs: Sync build ladder and tools-ecosystem post-035"
type: docs
status: completed
date: 2026-05-23
origin: build-and-test-ladder missing NuGet pack and CI -m:1; tools-ecosystem missing product UX links
---

# docs: Sync build ladder and tools-ecosystem post-035

## Summary

`build-and-test-ladder.md` and `tools-ecosystem.md` predate NuGet pack green path (plan 035) and `30-product-ux/` stub (plan 038). Align execution docs with current toolchain truth.

---

## Requirements

- R1. Add optional NuGet pack step and CI `-m:1` note to `build-and-test-ladder.md`.
- R2. Extend `tools-ecosystem.md` with NuGet/BioWare.TSLPatcher and product UX links.
- R3. Update `pr-merge-readiness.md` plan **041** row.
- R4. Drift register remediation **#32**.

---

## Scope Boundaries

- No code or CI workflow changes.
