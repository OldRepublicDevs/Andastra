---
title: "feat: ncs consti four-hop multi-arg nested jsr relay"
type: feat
status: complete
completed: 2026-05-24
date: 2026-05-24
origin: docs/plans/2026-05-24-368-feat-ncs-consti-three-hop-multi-arg-jsr-relay-plan.md
branch: feat/plan-374-four-hop-multi-arg-jsr-relay
---

# feat: NCS CONSTI four-hop multi-arg nested JSR relay (plan 374)

## Summary

Plan **368** covers three-hop multi-arg nested relay (`relay → mid → inner → speak`). Extend to **four-hop multi-arg** (`outer → relay → mid → inner → speak`) at relay depth 4 — the `MaxNestedJsrRelayDepth = 4` ceiling from plan **362**.

## Requirements

- R1. Four-hop multi-arg nested relay with StrRef on second param → `StrRefConsumer`.
- R2. `StrRefReferenceCache` indexes the StrRef for the same NSS pattern.
- R3. Existing **96** NcsConsti tests pass; **+2** new tests (**98** total).

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```

## Scope Boundaries

- Characterization tests only; scanner from plan **362** should handle depth 4 without code changes.
