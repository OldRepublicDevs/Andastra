---
title: "ci: Serialize solution-build to fix Andastra.Core deps lock"
type: fix
status: completed
date: 2026-05-23
origin: CI job 77569690413 investigation on PR #2
---

# ci: Serialize solution-build to fix Andastra.Core deps lock

## Summary

`solution-build` failed on `70ea6b56e` with `GenerateDepsFile` IO lock on `Andastra.Core.deps.json` during parallel MSBuild. Multiple projects reference `Andastra.Core`; CI orphan dotnet/VBCSCompiler processes indicate contention.

---

## Requirements

- R1. Add `-m:1` to `dotnet build Andastra.sln` in `.github/workflows/ci.yml` `solution-build` job.
- R2. Document parallel-build lock mitigation in `ci-release-risks.md`.
- R3. Drift register remediation **#22**.

---

## Scope Boundaries

- No per-project Directory.Build.props for Andastra.Core (CI serialization only).
- Local dev builds remain parallel by default.

## Test Scenarios

- Workflow YAML valid (validate-workflows job).
- Local: `dotnet build Andastra.sln --framework net9.0 -c Release -m:1` exits 0.
