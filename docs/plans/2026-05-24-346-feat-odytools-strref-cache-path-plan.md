---
title: "feat: odyTools strref helper cache-path ncs gating"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-339-test-bioware-find-strref-cf-slow-cache-plan.md
branch: feat/plan-346-odytools-strref-cache-path
---

# feat: OdyTools StrRefReferenceHelper cache-path NCS gating (plan 346)

## Summary

KotorCLI and BioWare tests validate cache-path NCS control-flow gating (plans **337**–**339**). `StrRefReferenceHelper.CollectStrRefReferences` still passes `cache: null` (slow path), so OdyTools TLK/SSF **Find References** may report dead-path NCS CONSTI hits. Build `StrRefReferenceCache` when `IncludeNcsStrRefScan` is enabled.

## Requirements

- R1. `CollectStrRefReferences` builds cache via `ReferenceCacheHelpers.BuildStrRefReferenceCache` when `IncludeNcsStrRefScan` is true.
- R2. Test: dead early-return NCS consumer → empty results (cache path).
- R3. Test: live early-return NCS consumer → finds hit (cache path).
- R4. Existing `StrRefReferenceHelperTests` pass.

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~StrRefReferenceHelperTests
```

## Scope Boundaries

- No persistent cache file in OdyTools UI (future slice); in-memory cache per search only.
- Browser tests skipped (headless unit tests sufficient).
