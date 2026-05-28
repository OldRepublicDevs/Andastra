---
title: "feat: kotorcli mixed json nss pipeline integration test"
type: feat
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-130-refactor-kotorcli-match-pattern-list-sources-plan.md
branch: feat/holocron-port-phase-b
---

# feat: KotorCLI mixed JSON + NSS pipeline integration test (plan 131)

## Summary

Verify full `pack` orchestration (convert + compile) and `install` with a project containing both JSON GFF sources and NSS scripts.

## Requirements

- R1. Integration test uses `package.sources` include patterns for both `src/**/*.json` and `src/**/*.nss`.
- R2. Test: `pack` (default orchestration) produces MOD containing UTC and NCS resrefs.
- R3. Test: `install` copies MOD into fake game `modules/` with both resources intact.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~BuildPipelineIntegration`
