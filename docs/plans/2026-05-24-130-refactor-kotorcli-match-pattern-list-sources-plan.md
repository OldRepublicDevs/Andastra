---
title: "refactor: kotorcli shared match pattern and list source discovery"
type: refactor
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-129-feat-kotorcli-nss-compile-pack-test-plan.md
branch: feat/holocron-port-phase-b
---

# refactor: KotorCLI shared MatchPattern and list source discovery (plan 130)

## Summary

Deduplicate fnmatch-style `MatchPattern` into `GlobPatternMatcher` and fix `ListCommand` verbose source discovery to use include patterns directly.

## Requirements

- R1. Add `GlobPatternMatcher.MatchPattern(path, pattern)`.
- R2. Replace private copies in `ConvertCommand`, `PackCommand`, `CompileCommand`, and `ListCommand`.
- R3. Fix `ListCommand.GetTargetSourceFiles` to use patterns from `GetTargetSources` directly.
- R4. Test: `list --verbose` succeeds when `package.sources` include patterns match files under `src/`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ListCommand|FullyQualifiedName~ConvertCommand|FullyQualifiedName~PackCommand|FullyQualifiedName~CompileCommand`
