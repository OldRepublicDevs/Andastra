---
title: "fix(odypatch): route --help to cli and add ci smoke"
type: fix
status: completed
date: 2026-05-24
origin: pr-merge-readiness 053+; odypatch --help hung on gui launch in headless env
---

# fix(odypatch): route --help to CLI and add CI smoke

## Summary

`OdyPatch` parses `--help` but never handles it — headless runs launch Avalonia GUI and hang. Route `--help` to stdout usage text, add post-pack CLI smoke in `nuget-pack-smoke` CI job, and sync product-UX KB docs.

---

## Requirements

- R1. In `Program.cs`, handle `cmdlineArgs.Help` before GUI launch; print usage and exit 0.
- R2. Add `PrintHelp()` with documented CLI flags (`--install`, `--validate`, `--uninstall`, `--game-dir`, `--tslpatchdata`).
- R3. Extend `nuget-pack-smoke` CI job with `dotnet run ... --no-build -- --help` grep smoke.
- R4. Update `odypatch-installer-ux.md` verification table with CLI help smoke status.
- R5. Add evidence labels to `30-product-ux/README.md`; link layer from `agent-workflow.md`.
- R6. Drift remediation **#44**; plans index **053**; maintenance tracker update.

---

## Scope Boundaries

- No end-to-end mod install validation (still requires K1/TSL install).
- No Avalonia GUI automation in CI.

---

## Test Scenarios

| Scenario | Expected |
|----------|----------|
| `dotnet run ... -- --help` | Prints usage, exits 0, no GUI |
| CI smoke | Help output contains `--install` |

---

## Repo Implications

- Enables compile-time UX validation without game install.
- Agents can verify OdyPatch CLI surface in CI logs.
