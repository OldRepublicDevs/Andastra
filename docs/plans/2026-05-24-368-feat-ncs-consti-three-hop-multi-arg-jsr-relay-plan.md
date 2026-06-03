---
title: "feat: ncs consti three-hop multi-arg nested jsr relay"
type: feat
status: complete
completed: 2026-06-03
date: 2026-05-24
origin: docs/plans/2026-05-24-364-feat-ncs-consti-two-hop-multi-arg-jsr-relay-plan.md
branch: feat/plan-368-three-hop-multi-arg-jsr-relay
---

# feat: NCS CONSTI three-hop multi-arg nested JSR relay (plan 368)

## Summary

Plan **364** covers two-hop multi-arg nested relay (`relay → mid → speak`). Extend characterization to **three-hop multi-arg** chains (`relay → mid → inner → speak`) at relay depth 3.

## Requirements

- R1. Three-hop multi-arg nested relay with StrRef on second param → `StrRefConsumer`.
- R2. `StrRefReferenceCache` indexes the StrRef for the same NSS pattern.
- R3. Existing **90** NcsConsti tests pass; **+2** new tests (**92** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Characterization tests only; scanner from plan **362** already handles depth 3 without code changes.

## Implementation Notes

- **Test-only outcome (confirmed):** three-hop multi-arg probe passes without scanner changes; `MaxNestedJsrRelayDepth = 4` already covers depth 3 (same as plan **364**).
