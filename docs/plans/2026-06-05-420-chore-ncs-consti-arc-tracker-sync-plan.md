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

Open PRs **#70**, **#77**, **#79**, **#80**, **#87**, and **#88** (plans **383**, **409**–**411**, **418**, **419**) complete the bounded six-hop nested JSR relay arc. Plans **418** and **419** are implemented in **#87** and **#88** — this slice does **not** redo that code. It updates merge-readiness tracking and the build ladder so agents and reviewers have one authority path before the stack merges.

## Problem Frame

`pr-merge-readiness.md` on `master` still cites **98** NcsConsti (post PR **#63**) and lists stale suggested next slices. Open relay PRs through **#88** raise the ceiling to **107** NcsConsti at `MaxNestedJsrRelayDepth = 6`. Without a docs-only sync, LFG agents duplicate tracker work or miss merge order for **#87**/**#88**.

## Requirements

- R1. `pr-merge-readiness.md` documents the NCS relay open stack (**#70**, **#77**, **#79**, **#80**, **#87**, **#88**), merge order, and **107** NcsConsti pending **#88**.
- R2. `build-and-test-ladder.md` Step **3b** notes **107** NcsConsti at open stack tip (plan **419** / PR **#88**).
- R3. Suggested next slices refreshed: field-value arc (**#86**), post-merge relay sync, full CONSTI stack simulation, KotorDiff **#71**, CodeQL **#74**.
- R4. Index plan **420** in `docs/plans/README.md`.

## Scope Boundaries

### In scope

- Docs-only tracker and build-ladder sync.
- Plan **420** file and README row.

### Out of scope

- Re-implementing plans **418** or **419** (PRs **#87**, **#88**).
- Cherry-picking feature plan files **409**–**419** from open PR branches (land with their PRs).
- Scanner or test code changes.

## Verification

```bash
grep -E '#87|#88|418|419|107' docs/knowledgebase/90-meta/pr-merge-readiness.md
grep '107' docs/knowledgebase/50-execution/build-and-test-ladder.md
grep '420' docs/plans/README.md
```

Docs-only — no `dotnet build` required.
