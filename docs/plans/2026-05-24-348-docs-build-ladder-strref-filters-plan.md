---
title: "docs: build-and-test-ladder strref ref-search filters"
type: docs
status: complete
date: 2026-05-24
completed: 2026-06-03
origin: docs/plans/2026-05-24-347-chore-merge-pr37-tracker-sync-plan.md
branch: master
---

# docs: build-and-test-ladder StrRef ref-search filters (plan 348)

## Summary

Plans **324**–**347** landed NCS CONSTI control-flow gating across BioWare, KotorCLI, and OdyTools. Document the narrow ref-search test ladder in `build-and-test-ladder.md` so agents validate the vertical slice without full solution test runs.

## Requirements

- R1. Add **Step 3b** (or equivalent) with NcsConsti, FindStrRefCommand, InstallationRefSearch CLI, and StrRefReferenceHelper filter commands.
- R2. Note expected pass counts (**74** + **18** + **12** + **10** on master post PR **#36**/**#37**).
- R2. Plan index row **348**; mark plan **347** complete if not already.

## Verification

Doc-only; run filter commands once to confirm counts.

## Scope Boundaries

- Documentation only; no code changes.
