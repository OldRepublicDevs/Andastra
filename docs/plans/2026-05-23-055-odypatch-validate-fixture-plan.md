---
title: "test: odypatch validate cli fixture and ci smoke"
type: test
status: completed
date: 2026-05-24
origin: pr-merge-readiness 055+; validate only needs directory + changes.ini, not full game install
---

# test: OdyPatch validate CLI fixture and CI smoke

## Summary

Add minimal `tests/fixtures/odypatch-minimal-mod/` with empty `changes.ini` `[Settings]` and fake game dir. Extend `nuget-pack-smoke` CI job with `--validate` smoke. Document compile-time validation path in product UX KB.

---

## Requirements

- R1. Add `tests/fixtures/odypatch-minimal-mod/tslpatchdata/changes.ini` with minimal `[Settings]`.
- R2. Add `tests/fixtures/odypatch-fake-game/` placeholder directory (`.gitkeep`).
- R3. Verify locally: `dotnet run ... -- --validate --game-dir ... --tslpatchdata ...` exits 0.
- R4. Add validate smoke step to `nuget-pack-smoke` CI job after CLI help smoke.
- R5. Update `odypatch-installer-ux.md`, `run-tools-reference.md`, maintenance tracker.
- R6. Drift remediation **#46**; plans index **055**.

---

## Scope Boundaries

- Not full mod install against real K1/TSL (still `[OPEN]`).
- No new xUnit project unless validate fails in CI without it.

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| Local `--validate` with fixture | Exit 0, validation completed message |
| CI validate smoke | Passes in `nuget-pack-smoke` job |

---

## Repo Implications

- Agents can verify OdyPatch config parsing without game install.
- Reduces gap between compile-green and config-validate-green.
