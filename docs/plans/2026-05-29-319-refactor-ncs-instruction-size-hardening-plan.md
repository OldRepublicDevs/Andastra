---
title: "refactor: NCS CONSTI GetInstructionSizeAt walk hardening"
type: refactor
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-29-318-chore-merge-pr31-post-merge-tracker-plan.md
branch: feat/plan-319-ncs-instruction-size-hardening
---

# refactor: NCS CONSTI GetInstructionSizeAt walk hardening (plan 319)

## Summary

Close CONSTI scanner maintenance gap: `GetInstructionSizeAt` returned **0** for unknown opcodes, aborting instruction-aligned walks (plan **315**/**317**). Align with `NCSActionPatcher` default **2-byte** fallback so forward/BP scans traverse full bytecode safely.

## Requirements

- R1. Add `GetInstructionStepSizeAt` for BP full-file walks: unknown opcodes step **2** bytes (align `NCSActionPatcher`); keep `GetInstructionSizeAt` returning **0** for unknown so forward/relay scans abort safely.
- R2. Remove duplicate `0x02` branch; merge `0x2D` into shared 2-byte opcode group (no behavior change).
- R3. All **33** NcsConsti tests pass.
- R4. Update plan **063** CONSTI note.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Size table only; no new StrRef heuristics.
- Full stack simulation remains deferred.
