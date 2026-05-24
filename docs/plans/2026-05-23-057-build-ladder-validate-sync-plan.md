---
title: "docs: sync build ladder with odypatch validate fixture"
type: docs
status: completed
date: 2026-05-24
origin: validate fixture documented in run-tools-reference but missing from build-and-test-ladder step 5
---

# docs: sync build ladder with OdyPatch validate fixture

## Summary

Plan 055 added `tests/fixtures/odypatch-minimal-mod/` and CI validate smoke, documented in `run-tools-reference.md` but not in the canonical [build-and-test-ladder.md](../knowledgebase/50-execution/build-and-test-ladder.md). Add ladder steps and PR #4 tracking on branch `docs/post-pr3-tracker-sync`.

---

## Requirements

- R1. Add OdyPatch `--help` and `--validate` fixture commands to `build-and-test-ladder.md` Step 5.
- R2. Add PR #4 (open) section to `pr-merge-readiness.md` for plan 056 on this branch.
- R3. Sync `ci-release-risks.md` nuget-pack row with help + validate smoke.
- R4. Drift remediation **#48**; plans index **057**.

---

## Scope Boundaries

- Docs-only on existing PR #4 branch.
- No E2E mod install (still `[OPEN]`).

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Ladder doc | Contains validate fixture command matching CI and run-tools-reference |

---

## Repo Implications

- Agents using ladder get OdyPatch config validation without reading run-tools-reference separately.
