---
title: "feat: NCS CONSTI opcode-context StrRef disambiguation"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/plans/2026-05-24-063-feat-pykotor-holocron-port-continuation-plan.md
branch: feat/plan-303-consti-opcode-context
---

# feat: NCS CONSTI opcode-context disambiguation (plan 303)

## Summary

Extend `NcsConstiScanner` with bytecode context heuristics so `StrRefReferenceCache` can distinguish StrRef CONSTI from generic integer / 2DA-memory CONSTI beyond the value threshold alone. Explicit StrRef slow-path queries remain exact-match.

## Requirements

- R1. `NcsConstiScanner.GetConstiUsageContext(byte[] ncsData, ConstiInstruction)` classifies immediate post-CONSTI usage as `StrRefConsumer`, `GenericInteger`, or `Unknown`.
- R2. `NcsConstiScanner.ShouldIndexAsStrRefCandidate(byte[] ncsData, ConstiInstruction, int minimum)` combines threshold + context: exclude `GenericInteger`; include `StrRefConsumer` even below minimum; otherwise use threshold.
- R3. `StrRefReferenceCache.ScanNCS` uses `ShouldIndexAsStrRefCandidate` instead of threshold-only check.
- R4. Slow-path `ExtractConstiOffsetsForValue` unchanged.
- R5. BioWare.Tests: StrRef-action small literal indexed; comparison-bound literal excluded from cache; slow-path unchanged.
- R6. Update plan **063** deferred note and KB UX row for partial opcode-context landing.

## Scope Boundaries

- Heuristic v1: immediate next-instruction only (no deep stack analysis).
- No new CLI flags; no OdyTools UI changes.
- Full action-signature analysis deferred.

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
