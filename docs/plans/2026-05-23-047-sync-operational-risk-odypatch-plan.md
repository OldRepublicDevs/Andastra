---
title: "docs: Sync operational-risk and tslpatcher-domain for OdyPatch host"
type: docs
status: completed
date: 2026-05-23
origin: build-health-matrix and tslpatcher-domain still blur OdyPatch exe vs OdyPatch.UI library
---

# docs: Sync operational-risk and tslpatcher-domain for OdyPatch host

## Summary

Plans 044–046 fixed run commands and agent onboarding. `build-health-matrix.md`, `ci-release-risks.md`, `tslpatcher-domain.md`, and `QUICKSTART.md` still merge OdyPatch/OdyPatch.UI roles or omit the host-run model.

---

## Requirements

- R1. Clarify OdyPatch (exe host) vs OdyPatch.UI (library) in `build-health-matrix.md` and `tslpatcher-domain.md`.
- R2. Add CI HEAD re-check guidance to `ci-release-risks.md`.
- R3. Add OdyPatch host note to `QUICKSTART.md`; extend `docs/plans/README.md` with plan 047.
- R4. Drift register remediation **#38**.

---

## Scope Boundaries

- No CI workflow changes; CI poll deferred (runs queued on HEAD).
