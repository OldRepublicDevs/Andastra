---
title: "docs: close holocron u4 kotordiff integration"
type: docs
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md (U4)
branch: feat/holocron-port-phase-b
---

# docs: Close Holocron U4 — KotorDiff OdyTools integration (plan 284)

## Completion (2026-05-28)

- U4 requirements from plan **063** verified as landed.
- Plan **063** U4 row updated with closure note and test/build evidence.
- KotorDiff tests + OdyTools build passed.

## Summary

Document closure of **U4 KotorDiff integrate**: `KotorDiffWindow` hosts shared `KotorDiffApp` from `src/Tools/KotorDiff`, wired from `MainWindow`. No separate feature plan file existed; this slice records verification against plan **063** R4.

## Requirements (from plan 063 U4)

- R1. `KotorDiffWindow` extends `KotorDiffApp` (not a stub placeholder).
- R2. Constructor passes installation paths from `OdyInstallation` dictionary.
- R3. `MainWindow` opens `KotorDiffWindow` with active installation.
- R4. KotorDiff test project builds and passes on net9.0.
- R5. OdyTools project builds with KotorDiff reference.

## Verification

```bash
dotnet test tests/KotorDiff.Tests/KotorDiff.Tests.csproj --framework net9.0
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
```

## Scope Boundaries

- Doc/plan closure only; no production code changes.
