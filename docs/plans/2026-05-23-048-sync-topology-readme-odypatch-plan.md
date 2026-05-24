---
title: "docs: Sync solution-topology and README OdyPatch roles"
type: docs
status: completed
date: 2026-05-23
origin: README lists OdyPatch.UI as mod tool; solution-topology merges host and library
---

# docs: Sync solution-topology and README OdyPatch roles

## Summary

Root `README.md` still describes `OdyPatch.UI` as the mod installation tool. `solution-topology.md`, `contributing-paths.md`, and `evidence-contract.md` need the exe-host + UI-library split from plans 044–047.

---

## Requirements

- R1. Fix root `README.md` tool list (OdyPatch = runnable host).
- R2. Update `solution-topology.md`, `contributing-paths.md`, `project-mission.md`, `evidence-contract.md`.
- R3. Add OdyPatch run to `build-and-test-ladder.md` step 5 when tools relevant.
- R4. Drift register remediation **#39**; extend `docs/plans/README.md`.

---

## Scope Boundaries

- No code or CI workflow changes.
