---
title: "docs: sync odypatch readme and verify validate fixture"
type: docs
status: completed
date: 2026-05-24
origin: plans 057-061 synced KB entry points but OdyPatch README and plans index intro still thin
---

# docs: sync OdyPatch README and verify validate fixture

## Summary

Plans 057–061 wired the OdyPatch `--validate` fixture across KB, AGENTS.md, and QUICKSTART. `src/Tools/OdyPatch/README.md` and `docs/plans/README.md` intro still omit the headless path. Add tool README validate section, update plans index for PR #4, and run local validate smoke to confirm fixture parity with CI.

---

## Requirements

- R1. Add `--help` / `--validate` fixture block and E2E runbook link to `src/Tools/OdyPatch/README.md`.
- R2. Add validate fixture note to root `README.md` Development Tools section with KB link.
- R3. Update `docs/plans/README.md` intro to note PR #4 (plans 056+).
- R4. Run local `--validate` smoke; record pass in plan verification note.
- R5. Drift remediation **#53**; plans index **062**; extend PR #4 tracker.

---

## Scope Boundaries

- Docs-only except local validate command execution.
- No E2E install against real game.

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Local validate | Exit 0 with fixture paths |
| OdyPatch README | Documents validate command matching build ladder |

---

## Repo Implications

- Tool README becomes self-contained for mod authors cloning the repo.
