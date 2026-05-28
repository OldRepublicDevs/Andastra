---
title: "docs: close plan 064 kotorcli converts and odytool fac"
type: docs
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-064-feat-holocron-u1-u2-kotorcli-fac-plan.md
branch: feat/holocron-port-phase-b
---

# docs: Close plan 064 — KotorCLI converts + OdyToolFAC (plan 281)

## Completion (2026-05-28)

- Plan **064** marked `status: complete` with R1–R5 evidence.
- Plan **063** U1/U2 rows note plan **064** closure.
- Tests: FormatConvert integration + **3** OdyToolFAC — all passed.

## Summary

Close plan **064** (Holocron phase A: U1 KotorCLI format converts + U2 OdyToolFAC). Implementation landed on `feat/holocron-fac-kotorcli`; verify and document closure.

## Requirements

- R1. KotorCLI TLK/SSF/JSON↔GFF converts delegate to BioWare `Conversions`.
- R2. `OdyToolFAC` editor with FACHelpers load/save.
- R3. `WindowUtils` routes `ResourceType.FAC` to `OdyToolFAC`.
- R4. `OdyToolFAC.Standalone.csproj` in solution.
- R5. `OdyToolFACTests` pass.
- R6. Mark plan **064** complete; update plan **063**.

## Verification

```bash
dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FormatConvert
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolFAC
```

## Scope Boundaries

- Doc/plan closure only; no production code changes.
