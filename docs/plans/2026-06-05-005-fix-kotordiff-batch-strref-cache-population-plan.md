---
title: "fix: kotordiff batch strref cache population"
type: fix
status: complete
completed: 2026-06-05
date: 2026-06-05
origin: docs/plans/2026-06-05-004-feat-kotordiff-batch-strref-cache-plan.md
branch: feat/plan-383-kotordiff-installation-ref-search
---

# fix: KotorDiff batch StrRef cache population (plan 005)

## Summary

Plan **004** passed a freshly constructed empty `StrRefReferenceCache` into `FindStrRefReferences`, which returns no hits when the cache lacks entries. Use `ReferenceCacheHelpers.BuildStrRefReferenceCache` to scan the installation before batch StrRef analysis.

## Requirements

- R1. Batch `GenerateTSLPatcherData` builds a **populated** cache via `BuildStrRefReferenceCache`.
- R2. Add test proving `CollectInstallationStrRefResources` finds hits when given a built cache.

## Verification

```bash
dotnet test tests/KotorDiff.Tests/KotorDiff.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ReferenceAnalyzersInstallation
```
