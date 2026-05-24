---
title: "ci: Add ConvertKotorGame to dotnet-desktop and sync NCS docs"
type: feat
status: completed
date: 2026-05-23
origin: PR #2 desktop CI coverage gap
---

# ci: Add ConvertKotorGame to dotnet-desktop and sync NCS docs

## Summary

`ConvertKotorGame` is green on net9.0 per build-health-matrix but absent from `.github/workflows/dotnet-desktop.yml`. NCS domain and dev-setup docs still carry pre-plan-011 path caveats.

---

## Requirements

- R1. Add `ConvertKotorGame` restore + build to `dotnet-desktop.yml` alongside KotorCLI.
- R2. Update `ncs-nwscript-vm.md` — NCSDecomp path drift resolved in plan 011.
- R3. Update `dev-environment-setup.md` — `dotnet restore Andastra.sln` succeeds on current branch.
- R4. Sync `90-meta/README.md` CI table to list ConvertKotorGame in dotnet-desktop job.
- R5. Drift register remediation **#18**.

---

## Scope Boundaries

- No ConvertKotorGame runtime/E2E testing (build only).
- No game install required.

## Test Scenarios

- Local: `dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0` exits 0.
- Workflow YAML restores and builds ConvertKotorGame in desktop job steps.
