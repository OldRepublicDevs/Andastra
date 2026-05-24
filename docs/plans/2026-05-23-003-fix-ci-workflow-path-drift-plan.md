---
title: "fix: Repair CI workflow path drift"
type: fix
status: completed
date: 2026-05-23
origin: docs/knowledgebase/40-operational-risk/documentation-drift-register.md remediation #2
---

# fix: Repair CI workflow path drift

## Summary

Fix broken CI and release workflow paths documented in KB caveat C4 and the documentation drift register. Align primary CI test job with the BioWare green-path ladder from `AGENTS.md`.

---

## Problem Frame

`.github/workflows/ci.yml` references missing `src/CSharpKOTOR.Tests/`. OdyPatch build workflows reference `src/OdyPatch/` instead of `src/Tools/OdyPatch/`. PR #2 CI checks are likely failing on these paths.

---

## Requirements

- R1. Replace `CSharpKOTOR.Tests` with `tests/BioWare.Tests/BioWare.Tests.csproj` in `ci.yml`
- R2. CI test job uses `--framework net9.0` and includes `Andastra.Tests`
- R3. Update OdyPatch csproj paths in `test-builds.yml`, `build-all-platforms.yml`, `build-release.yml`
- R4. Update KB drift register and caveat C4 when fixed
- R5. Fix README stale tool project names (NSSComp, NCSDecomp) in Running section

---

## Scope Boundaries

- Do not fix OdyTools compile errors (OdyPatch build may still fail until OdyTools fixed)
- Do not rewrite full README architecture diagram
- Do not change wiki Home.md

---

## Implementation Units

- U1. Fix `ci.yml` test and lint jobs to use BioWare green path
- U2. Fix OdyPatch paths in release/test-build workflows
- U3. Fix README tool paths; update KB drift/caveat registers

**Verification:** `git diff --check` clean; workflow paths exist on disk; local `dotnet test tests/BioWare.Tests` passes.

---

## Sources & References

- `docs/knowledgebase/50-execution/build-and-test-ladder.md`
- `docs/knowledgebase/40-operational-risk/ci-release-risks.md`
- `AGENTS.md`
