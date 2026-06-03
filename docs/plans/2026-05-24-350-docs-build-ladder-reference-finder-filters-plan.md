---
title: "docs: build-and-test-ladder reference-finder filters"
type: docs
status: complete
date: 2026-05-24
completed: 2026-06-03
origin: docs/plans/2026-05-24-349-chore-merge-pr38-tracker-sync-plan.md
branch: master
---

# docs: build-and-test-ladder ReferenceFinder filters (plan 350)

## Summary

Plan **348** added Step 3b for NCS CONSTI / StrRef ref-search. Add **Step 3c** for GFF/script ReferenceFinder and OdyTools helper wiring — the other half of the holocron reference-search vertical slice (plan **068** arc).

## Requirements

- R1. Add **Step 3c** with ReferenceFinder, ReferenceSearchHelper, ScriptReferenceHelper, and KotorCLI find-refs filter commands.
- R2. Document expected pass counts on `master` (**97** + **36** + **8** + **21**).
- R3. Fix stale **10 NcsConsti** wording in `odytools-editor-ux.md` plan **286** row (superseded by **74** NcsConsti arc).
- R4. Index plan **350**; mark complete after landing.

## Verification

Run filter commands once; doc-only otherwise.

## Scope Boundaries

- Documentation only; no production code changes.
