---
title: "chore: Sync post-018 docs and add KotorCLI to desktop CI"
type: chore
status: completed
date: 2026-05-23
origin: docs/plans/2026-05-23-018-fix-kotorcli-commandline-api-plan.md
---

# chore: Sync post-018 docs and add KotorCLI to desktop CI

## Summary

QUICKSTART and AGENTS.md still carry pre-plan-018 build claims. Add KotorCLI to `dotnet-desktop.yml` so the Windows CI job validates the fixed CLI. Remove resolved stale README note from contributing-paths.

---

## Problem Frame

- `docs/QUICKSTART.md` line 29 still says KotorCLI may fail. `[REPO]`
- `AGENTS.md` claims solution restore fails on missing MonoGameFPS/StrideGameFPS — restore succeeds. `[REPO]`
- `contributing-paths.md` claims README contributing uses stale names — README corrected plan 007. `[REPO]`
- Desktop CI builds OdyTools/OdyPatch but not KotorCLI after plan 018 fix. `[REPO]`

---

## Requirements

- R1. QUICKSTART reflects tool-chain green status including KotorCLI.
- R2. AGENTS.md restore note matches observed `dotnet restore Andastra.sln` behavior.
- R3. contributing-paths removes stale README claim.
- R4. dotnet-desktop.yml restores, builds, and smoke-tests KotorCLI `--help`.
- R5. Drift register remediation 10.

---

## Implementation Units

- U1. **QUICKSTART.md** — update build caveat + add KotorCLI command
- U2. **AGENTS.md + contributing-paths.md** — accuracy fixes
- U3. **dotnet-desktop.yml** — KotorCLI restore/build/help step
- U4. **documentation-drift-register.md** — remediation 10

---

## Scope Boundaries

- Do not attempt Stride assembly-processor Linux fix in this slice.
- Do not add KotorCLI to ubuntu ci.yml unless already building tools there.
