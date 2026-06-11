---
title: "chore: stack simulation arc tracker sync"
type: chore
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-420-chore-ncs-consti-arc-tracker-sync-plan.md
branch: feat/plan-423-stack-simulation-arc-tracker-sync
---

# chore: Stack simulation arc tracker sync (plan 423)

## Summary

Plans **421**–**422** / open PRs **#90**–**#91** extend CONSTI stack simulation (arithmetic ACTION runs + local `CPDOWNSP`→`CPTOPSP` reload). Plan **420** / **#89** documents the relay stack. This docs-only slice stacks on **#89** and adds stack-simulation tracking plus **112** NcsConsti baseline.

## Requirements

- R1. `pr-merge-readiness.md` adds stack-simulation open PR table (**#90**–**#91**), updates Step **3b** count (**112** pending **#91**).
- R2. `build-and-test-ladder.md` Step **3b** documents **112** open-stack tip.
- R3. Suggested next slices refreshed; index plan **423** in `docs/plans/README.md`.

## Verification

```bash
grep -E '#90|#91|112|421|422' docs/knowledgebase/90-meta/pr-merge-readiness.md
grep '112' docs/knowledgebase/50-execution/build-and-test-ladder.md
grep '423' docs/plans/README.md
```
