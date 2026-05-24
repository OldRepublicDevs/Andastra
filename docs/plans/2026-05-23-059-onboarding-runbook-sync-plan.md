---
title: "docs: sync onboarding docs with odypatch runbook"
type: docs
status: completed
date: 2026-05-24
origin: plan 058 added odypatch-e2e-runbook.md but 90-meta README and agent-workflow omit it
---

# docs: sync onboarding docs with OdyPatch runbook

## Summary

Plan 058 added `odypatch-e2e-runbook.md` and cross-links from execution/UX docs, but the canonical KB index (`90-meta/README.md`), agent workflow, definition-of-done, and contributing-paths still omit it. Sync onboarding entry points so agents discover validate fixture and manual E2E paths without grep.

---

## Requirements

- R1. Add `odypatch-e2e-runbook.md` to `90-meta/README.md` document index and modding reading order.
- R2. Extend `agent-workflow.md` OdyPatch section with `--validate` fixture and runbook link.
- R3. Add OdyPatch validate fixture check to `definition-of-done.md` tooling section.
- R4. Link runbook from `contributing-paths.md` mod-installer row.
- R5. Drift remediation **#50**; plans index **059**; extend PR #4 tracker.

---

## Scope Boundaries

- Docs-only on PR #4 branch.
- No E2E execution or outcome recording (still `[OPEN]`).

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| 90-meta README | Lists runbook under 50-execution index |
| agent-workflow | References validate fixture command and runbook |

---

## Repo Implications

- New agents following default reading order reach OdyPatch manual verification path.
