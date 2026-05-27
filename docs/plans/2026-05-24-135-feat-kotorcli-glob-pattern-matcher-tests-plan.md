---
title: "feat: kotorcli GlobPatternMatcher unit tests"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-134-feat-kotorcli-init-file-unpack-test-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI GlobPatternMatcher unit tests (plan 135)

## Summary

Add direct unit coverage for `GlobPatternMatcher`, the shared glob helper used by convert/compile/pack/list.

## Requirements

- R1. Expose `GlobPatternMatcher` to `KotorCLI.Tests` via `InternalsVisibleTo`.
- R2. Test `FindFilesMatchingPattern` for `src/**/*.json` recursive discovery.
- R3. Test `FindFilesMatchingPattern` for exact relative path and shallow `*.json` glob.
- R4. Test `MatchPattern` for extension wildcards and path segments (case-insensitive).

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~GlobPatternMatcher`
