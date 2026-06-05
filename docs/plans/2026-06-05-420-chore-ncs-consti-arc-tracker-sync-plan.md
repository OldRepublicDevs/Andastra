---
title: "chore: NCS CONSTI arc closure tracker sync"
type: chore
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-419-test-ncs-consti-six-hop-mixed-const-relay-plan.md
branch: feat/plan-420-ncs-consti-arc-tracker-sync
---

# chore: NCS CONSTI arc closure tracker sync (plan 420)

## Summary

Plans **409**–**419** / open PRs **#70**, **#77**, **#79**–**#80**, **#87**–**#88** extend the bounded nested JSR relay arc to six hops (`MaxNestedJsrRelayDepth = 6`, **107** NcsConsti tests on the open stack tip). This docs-only slice updates merge-readiness tracking and the build ladder so agents and reviewers have one authority path before the relay stack merges.

## Requirements

- R1. `pr-merge-readiness.md` documents the **#70**, **#77**, **#79**–**#80**, **#87**–**#88** open stack, merge order, and depth/test-count baselines.
- R2. Local validation Step **3b** refreshed: **98** on `master`; **107** NcsConsti pending **#88** merge.
- R3. Suggested next slices refreshed (field-value **#81**–**#86** / plan **417**, full stack simulation per plan **063**, KotorDiff **#71**, CodeQL **#74**).
- R4. Index plan **420** in `docs/plans/README.md`.

## Verification

```bash
grep -E '#70|#77|#79|#80|#87|#88|107' docs/knowledgebase/90-meta/pr-merge-readiness.md
grep '107' docs/knowledgebase/50-execution/build-and-test-ladder.md
grep '420' docs/plans/README.md
```
