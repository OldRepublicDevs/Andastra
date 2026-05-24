---
title: "docs: odypatch e2e manual runbook for real game install"
type: docs
status: completed
date: 2026-05-24
origin: pr-merge-readiness suggested slice 058+; odypatch-installer-ux E2E claims still [OPEN]
---

# docs: OdyPatch E2E manual runbook for real game install

## Summary

Plans 055/057 added `--validate` fixture smoke without a game install. The maintenance tracker still lists full mod install as `[OPEN]`. Add a canonical manual E2E runbook under `50-execution/`, cross-link from product UX and build ladder, and record drift remediation **#49** on PR #4.

---

## Requirements

- R1. Create `docs/knowledgebase/50-execution/odypatch-e2e-runbook.md` with prerequisites, CLI `--install` steps, expected outcomes, and `[OPEN]` boundaries for unverified claims.
- R2. Link runbook from `odypatch-installer-ux.md`, `run-tools-reference.md`, and `build-and-test-ladder.md` Step 6 note.
- R3. Update `pr-merge-readiness.md` — plan 058 row, PR #4 scope note.
- R4. Drift remediation **#49**; plans index **058**.

---

## Scope Boundaries

- Docs-only; no CI job requiring K1/TSL install.
- No `--install` automation against real game in this slice.
- No new test fixtures beyond documenting existing validate fixture.

---

## Implementation Units

- U1. **E2E runbook doc** — new runbook with CLI flags from `Program.cs`, validate vs install distinction, manual verification checklist.
- U2. **Cross-links** — odypatch-installer-ux, run-tools-reference, build-and-test-ladder.
- U3. **Tracker sync** — pr-merge-readiness, drift register #49, plans README 058.

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Runbook doc | Contains `--install`, `--validate`, `--game-dir`, `--tslpatchdata` with repo-relative fixture paths |
| Cross-links | Bidirectional navigation from UX doc and run-tools-reference |

---

## Repo Implications

- Agents with a local K1/TSL install have a single authoritative manual path for E2E verification.
- CI gap remains explicit and documented rather than implied.
