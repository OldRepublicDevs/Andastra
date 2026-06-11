---
title: "feat: ncs consti five-hop multi-arg nested jsr relay"
type: feat
status: active
date: 2026-06-05
origin: docs/plans/2026-05-24-374-feat-ncs-consti-four-hop-multi-arg-jsr-relay-plan.md
branch: feat/plan-410-five-hop-multi-arg-jsr-relay
---

# feat: NCS CONSTI five-hop multi-arg nested JSR relay (plan 410)

## Summary

Plan **374** landed four-hop multi-arg nested relay at `MaxNestedJsrRelayDepth = 4`. Extend to **five-hop multi-arg** (`deepest → outer → relay → mid → inner → speak`) by raising the depth ceiling to **5** and adding characterization tests.

## Requirements

- R1. Raise `MaxNestedJsrRelayDepth` from **4** to **5** in `NcsConstiScanner`.
- R2. Five-hop multi-arg nested relay with StrRef on second param → `StrRefConsumer`.
- R3. `StrRefReferenceCache` indexes the StrRef for the same NSS pattern.
- R4. Index plan **410** in `docs/plans/README.md`; refresh tracker Step 3b NcsConsti count.

## Scope Boundaries

- Does not duplicate PR **#70** four-hop mixed CONST+CPTOPSP tests or PR **#77** arithmetic relay.
- Scanner depth constant change only if tests require it.

## Verification

```bash
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~FiveHopNestedJsrMultiArg
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
