---
title: "feat: kotordiff batch strref reference cache"
type: feat
status: complete
completed: 2026-06-05
date: 2026-06-05
origin: docs/plans/2026-06-05-002-feat-kotordiff-strref-mappings-batch-path-plan.md
branch: feat/plan-383-kotordiff-installation-ref-search
---

# feat: KotorDiff batch StrRef reference cache (plan 004)

## Summary

Plan **002** wired `strref_mappings` into batch `GenerateTSLPatcherData`. Pass an optional `StrRefReferenceCache` into `AnalyzeTlkStrrefReferences` so installation StrRef discovery uses the cache-fast path when the base path resolves to a game install.

## Requirements

- R1. `AnalyzeTlkStrrefReferences` accepts optional `StrRefReferenceCache` and forwards it to `CollectInstallationStrRefResources`.
- R2. Batch `GenerateTSLPatcherData` builds a cache when `paths[0]` is an `Installation` or directory path valid for `Installation`.
- R3. Folder-only paths continue with null cache (slow path unchanged).

## Verification

```bash
dotnet build src/Tools/KotorDiff/KotorDiff.csproj --framework net9.0
dotnet test tests/KotorDiff.Tests/KotorDiff.Tests.csproj --framework net9.0
```

## Scope Boundaries

- Batch path only; incremental writer already owns cache construction.
