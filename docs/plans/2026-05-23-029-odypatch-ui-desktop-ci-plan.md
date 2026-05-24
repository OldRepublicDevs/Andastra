---
title: "ci: Add OdyPatch.UI to desktop CI and fix README build commands"
type: feat
status: completed
date: 2026-05-23
origin: PR #2 desktop CI + README building section gap
---

# ci: Add OdyPatch.UI to desktop CI and fix README build commands

## Summary

`OdyPatch.UI` builds on net9.0 but is absent from `dotnet-desktop.yml`. README Building section still documents `dotnet build src/Tools/` which fails (no csproj in that directory).

---

## Requirements

- R1. Add `OdyPatch.UI` restore + build to `dotnet-desktop.yml`.
- R2. Fix README Building quick-start commands to match QUICKSTART green path (`BioWare`, `Andastra.sln`, `--framework net9.0`).
- R3. Sync KB CI table and `ci-release-risks.md` for OdyPatch.UI coverage.
- R4. Drift register remediation **#20**.

---

## Scope Boundaries

- No OdyPatch.UI runtime/E2E testing in CI.
- No README architecture diagram changes (plan 007).

## Test Scenarios

- `dotnet build src/Tools/OdyPatch.UI/OdyPatch.UI.csproj --framework net9.0` exits 0 locally.
- README no longer suggests `dotnet build src/Tools/`.
