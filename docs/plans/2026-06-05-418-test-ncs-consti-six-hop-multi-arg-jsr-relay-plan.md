---
title: "test: NCS CONSTI six-hop multi-arg JSR relay"
type: test
status: active
date: 2026-06-05
origin: docs/plans/2026-06-05-410-feat-ncs-consti-five-hop-multi-arg-jsr-relay-plan.md
branch: feat/plan-418-ncs-consti-six-hop-multi-arg-jsr-relay
---

# test: NCS CONSTI six-hop multi-arg JSR relay (plan 418)

## Summary

Plan **410** / PR **#79** raises `MaxNestedJsrRelayDepth` to **5** with five-hop multi-arg JSR relay tests. This slice stacks on **#79** and extends to **6** hops (`root→deepest→outer→relay→mid→inner→speak`) with **+2** NcsConsti tests.

## Requirements

- R1. `MaxNestedJsrRelayDepth` **5 → 6**.
- R2. `GetConstiUsageContext_SixHopNestedJsrMultiArgRelayStrRef_ReturnsStrRefConsumer` and cache indexing test.
- R3. **105** NcsConsti tests pass (103 + 2).

## Verification

```bash
dotnet build src/BioWare/BioWare.csproj --framework net9.0
dotnet test tests/BioWare.Tests/BioWare.Tests.csproj --framework net9.0 --filter FullyQualifiedName~NcsConsti
```
