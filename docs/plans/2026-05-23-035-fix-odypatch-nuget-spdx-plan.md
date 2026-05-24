---
title: "fix: OdyPatch NuGet pack SPDX license expression"
type: fix
status: completed
date: 2026-05-23
origin: plan 034 pack failure NU5032 deprecated LGPL-3.0 identifier
---

# fix: OdyPatch NuGet pack SPDX license expression

## Summary

`dotnet pack` for OdyPatch fails with NU5032 because `PackageLicenseExpression` uses deprecated SPDX id `LGPL-3.0`. Replace with `LGPL-3.0-only`, verify pack on net9.0 Linux, and sync KB/build docs.

---

## Requirements

- R1. Update `OdyPatch.csproj` `PackageLicenseExpression` to valid SPDX `LGPL-3.0-only`.
- R2. Verify `helper_scripts/build-nuget.sh` produces `.nupkg` on Linux net9.0.
- R3. Add NuGet pack note to `run-tools-reference.md` and `build-health-matrix.md`.
- R4. Drift register remediation **#26**.

---

## Scope Boundaries

- Do not change project-wide AGPLv3 vs OdyPatch LGPL policy (C12 remains open).
- No CI workflow changes.

---

## Test Scenarios

- TS1. `bash helper_scripts/build-nuget.sh` exits 0 and emits `OdyPatch.*.nupkg`.
- TS2. `dotnet pack` without NU5032 license parse error.
