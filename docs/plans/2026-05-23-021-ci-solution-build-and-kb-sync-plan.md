---
title: "ci: Add full solution build + sync post-020 KB"
type: chore
status: completed
date: 2026-05-23
origin: docs/plans/2026-05-23-020-fix-standalone-obj-collision-plan.md
---

# ci: Add full solution build + sync post-020 KB

## Summary

Plan 020 verified `dotnet build Andastra.sln --framework net9.0` on Linux. KB and AGENTS.md still claim full-solution failure. Add ubuntu CI smoke job and sync remaining stale docs; verify ConvertKotorGame build status.

---

## Problem Frame

- `definition-of-done.md`, `AGENTS.md`, `solution-topology.md`, `run-tools-reference.md` still cite Stride/full-solution failure as default. `[REPO]`
- No CI job exercises full solution build — regression risk for plan 020 fix. `[REPO]`
- `ConvertKotorGame` listed as build unverified. `[REPO]`

---

## Requirements

- R1. `ci.yml` adds `solution-build` job: restore + build `Andastra.sln --framework net9.0` on ubuntu.
- R2. Sync definition-of-done, AGENTS, solution-topology, run-tools-reference, build-health-matrix broken section.
- R3. Mark ConvertKotorGame green in tools-ecosystem.
- R4. Update ci-release-risks and drift register remediation 12.

---

## Implementation Units

- U1. **ci.yml** — solution-build job
- U2. **KB + AGENTS sync**
- U3. **ConvertKotorGame status**
- U4. **ci-release-risks + drift register**

---

## Scope Boundaries

- Do not add full solution test run (too heavy).
- Do not add game runtime CI.
