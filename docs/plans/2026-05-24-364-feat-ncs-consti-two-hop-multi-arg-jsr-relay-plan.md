---
title: "feat: ncs consti two-hop multi-arg nested jsr relay"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-362-feat-ncs-consti-nested-jsr-multi-arg-relay-plan.md
branch: feat/plan-364-two-hop-multi-arg-jsr-relay
---

# feat: NCS CONSTI two-hop multi-arg nested JSR relay (plan 364)

## Summary

Plan **362** covers single-hop multi-arg nested relay (`relay(a,s) → speak(a,s)`). Plan **360** covers two-hop single-arg relay. Extend characterization to **two-hop multi-arg** chains (`relay → mid → speak`) at relay depth 2.

## Requirements

- R1. Two-hop multi-arg nested relay with StrRef on second param → `StrRefConsumer`.
- R2. `StrRefReferenceCache` indexes the StrRef for the same NSS pattern.
- R3. Existing **86** NcsConsti tests pass; **+2** new tests (**88** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Characterization tests only; fix scanner only if two-hop multi-arg fails.
