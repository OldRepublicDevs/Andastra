---
title: "docs: sync agents.md and build health with odypatch validate"
type: docs
status: completed
date: 2026-05-24
origin: plans 057-060 synced KB docs but AGENTS.md and build-health-matrix still omit validate fixture
---

# docs: sync AGENTS.md and build health with OdyPatch validate

## Summary

Cursor Cloud agents read `AGENTS.md` and `build-health-matrix.md` before KB deep dives. Plans 057–060 wired the OdyPatch `--validate` fixture and E2E runbook through KB execution docs, but these high-traffic entry points still describe OdyPatch as GUI-only. Sync them on PR #4.

---

## Requirements

- R1. Add OdyPatch `--help` / `--validate` fixture to `AGENTS.md` Running tools section; link KB runbook.
- R2. Update `build-health-matrix.md` OdyPatch rows with validate fixture and runbook link.
- R3. Add validate smoke note to `docs/QUICKSTART.md` OdyPatch section.
- R4. Drift remediation **#52**; plans index **061**; extend PR #4 tracker.

---

## Scope Boundaries

- Docs-only.
- No E2E execution.

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| AGENTS.md | Contains validate fixture command matching build ladder |
| build-health-matrix | OdyPatch host row mentions CI validate smoke |

---

## Repo Implications

- Cloud agents see headless OdyPatch validation in the first doc they read.
