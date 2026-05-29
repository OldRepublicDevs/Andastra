---
title: "feat: NCS CONSTI instruction-aligned CPTOPBP scan"
type: feat
status: complete
date: 2026-05-29
completed: 2026-05-29
origin: docs/plans/2026-05-29-311-feat-ncs-consti-cptopbp-global-strref-plan.md
branch: feat/plan-315-cptopbp-aligned-scan
---

# feat: NCS CONSTI instruction-aligned CPTOPBP scan (plan 315)

## Summary

Optimize plan **311** `TryFindStrRefConsumerViaBpReload`: replace byte-by-byte `0x27` search with instruction-aligned walk via `GetInstructionSizeAt`. Eliminates false-positive matches on `0x27` bytes inside instruction operands.

## Requirements

- R1. Walk NCS from offset 13 using `GetInstructionSizeAt`; only evaluate `CPTOPBP` (0x27) at instruction boundaries.
- R1b. Extend `GetInstructionSizeAt` for `RETN`, `NOTx`, `SAVEBP`, `RESTOREBP` so cross-subroutine files scan completely.
- R2. Preserve plan **311** behavior: matching BP offset/size + StrRef ACTION slot validation.
- R3. All **31** existing NcsConsti tests pass.
- R4. Update plan **063** note.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Refactor only; no new scan semantics beyond alignment.
- No full stack simulation.
