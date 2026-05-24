---
title: "docs: Plans index and agent onboarding sync"
type: docs
status: completed
date: 2026-05-23
origin: 45 plan files without index; agent-workflow missing OdyPatch host tooling; 90-meta CI table overstates green
---

# docs: Plans index and agent onboarding sync

## Summary

Add a scannable `docs/plans/README.md` index for plans 001–045. Sync agent onboarding docs with OdyPatch host-vs-UI-library model and make CI status in `90-meta/README.md` evidence-honest (re-check HEAD, not assumed green).

---

## Requirements

- R1. Create `docs/plans/README.md` with numbered plan table and links.
- R2. Extend `agent-workflow.md` and `dev-environment-setup.md` with OdyPatch host run guidance.
- R3. Update `90-meta/README.md` CI table and link plans index; add `docs/plans/` to `authority-map.md`.
- R4. Drift register remediation **#37**.

---

## Scope Boundaries

- No CI workflow changes; poll only if convenient.
