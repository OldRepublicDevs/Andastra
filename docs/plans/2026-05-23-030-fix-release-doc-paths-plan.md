---
title: "docs: Fix release doc paths and README run commands"
type: docs
status: completed
date: 2026-05-23
origin: PR #2 post-029 workflow doc audit
---

# docs: Fix release doc paths and README run commands

## Summary

`docs/WORKFLOWS.md` and `docs/GITHUB_ACTIONS_SETUP.md` cite pre-move `src/OdyPatch/` paths. README Running section omits `--framework net9.0`. `solution-topology.md` understates full-solution green status.

---

## Requirements

- R1. Update OdyPatch/OdyPatch.UI paths in `WORKFLOWS.md` and `GITHUB_ACTIONS_SETUP.md` to `src/Tools/...`.
- R2. Add `--framework net9.0` to README game and OdyPatch.UI run examples.
- R3. Update `solution-topology.md` green baseline (ConvertKotorGame) and full-solution note.
- R4. Drift register remediation **#21**.

---

## Scope Boundaries

- No workflow or code changes.
