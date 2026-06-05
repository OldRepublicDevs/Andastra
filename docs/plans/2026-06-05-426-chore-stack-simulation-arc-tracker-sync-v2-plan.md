---
title: "chore: stack simulation arc tracker sync v2"
type: chore
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-425-feat-ncs-consti-arithmetic-strref-relay-v5-plan.md
branch: feat/plan-426-stack-simulation-arc-tracker-sync-v2
---

# chore: stack simulation arc tracker sync v2 (plan 426)

## Summary

Plan **423** indexed stack-simulation open PR stack **#89–#91** at **112** NcsConsti tests. Plans **424**–**425** landed **#93**–**#94** (cache probes, local SUB, DIV, local MUL/MOD) reaching **123** tests. This docs-only slice refreshes KB tracker rows and Step 3b ladder counts.

## Requirements

- R1. `pr-merge-readiness.md`: extend stack-simulation arc table with **#92**–**#94**; Step 3b row **123** pending **#94**; plans **348**–**425**.
- R2. `build-and-test-ladder.md`: Step 3b open-tip **123** NcsConsti at **#94**; intermediate **#93** at **117**.
- R3. Refresh suggested next slices for **426+** (post-merge sync, local chained/DIV, field-value arc).

## Verification

Docs-only — no `dotnet test` required. Spot-check PR links **#92**–**#94** resolve on GitHub.
