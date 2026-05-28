---
title: "test: referencefinder field value bytes and guards"
type: test
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-228-test-referencefinder-fieldvalue-nooverride-plan.md
branch: feat/holocron-port-phase-b
---

# test: ReferenceFinder field-value bytes and guards (plan 229)

## Summary

Complete OdyTools field-value search parity with BioWare.Tests and existing guard patterns for tag/script/conversation APIs.

## Requirements

- R1. `FindFieldValueInGffBytes_TagField_Matches` — bytes-level Tag field match (BioWare.Tests parity).
- R2. `FindFieldValueReferences_EmptyNeedleReturnsEmpty` — whitespace needle returns empty.
- R3. `FindFieldValueReferences_NullInstallation_ThrowsArgumentNullException`.
- R4. ReferenceFinder filter **29** tests pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter ReferenceFinder
```
