---
title: "docs: close plan 065 indoor builder build save open"
type: docs
status: complete
date: 2026-05-28
completed: 2026-05-28
origin: docs/plans/2026-05-24-065-feat-indoor-builder-build-save-open-plan.md
branch: feat/holocron-port-phase-b
---

# docs: Close plan 065 — Indoor Builder build/save/open (plan 282)

## Completion (2026-05-28)

- Plan **065** marked `status: complete` with R1–R5 evidence.
- Plan **063** U3 row notes plan **065** closure (walkmesh tests remain plan **069** / **279**).
- Tests: **3** IndoorMapIo, **4** IndoorMapWriteLoad — all passed.

## Summary

Close plan **065** (U3 phase A: embed `.indoor` JSON in MOD, headless Io/WriteLoad tests, IndoorBuilderWindow file ops). Implementation landed on `feat/holocron-fac-kotorcli`.

## Requirements

- R1. `FinalizeModuleData` embeds indoor JSON via `IndoorMapIo.EmbedIndoorJson`.
- R2. `IndoorMapIoTests` embed/extract roundtrip (**3** tests in `tests/OdyTools.Tests/`).
- R3. `IndoorMapWriteLoadTests` JSON roundtrip (**4** tests).
- R4. `IndoorBuilderWindow` Save/Open/Build wired to `Ui` actions.
- R5. Build without installation sets actionable `LastErrorMessage`.
- R6. Mark plan **065** complete; update plan **063**.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~IndoorMapIo
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~IndoorMapWriteLoad
```

## Scope Boundaries

- Doc/plan closure only; walkmesh characterization deferred to plan **069** (closed **279**).
