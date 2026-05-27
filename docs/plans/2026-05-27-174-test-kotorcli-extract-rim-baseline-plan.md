---
title: "test: kotorcli extract rim baseline"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-173-test-kotorcli-extract-mod-baseline-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract RIM baseline (plan 174)

## Summary

Add `ExecuteExtractRim_WritesExtractedResourceFiles` integration test in `ExtractCommandTests.cs`, mirroring `ExecuteExtractMod_WritesExtractedResourceFiles` and `ExecuteExtractErf_WritesExtractedResourceFiles`. RIM filter paths exist; this closes the happy-path extract baseline gap for `.rim` without a filter.

## Requirements

- R1. `ExtractCommand.Execute` with a sample `.rim` exits zero when no filter is supplied.
- R2. Output directory contains both extracted resources from the sample archive.
- R3. Use `CreateSampleRimWithTwoResources` so the archive has known resources; all resources are written when filter is null.

## Implementation

- Copy structure from `ExecuteExtractMod_WritesExtractedResourceFiles` (line ~119).
- Place after MOD baseline test; use temp dir + `finally` cleanup pattern consistent with sibling tests.
- Call `ExtractCommand.Execute(rimPath, outputDir, null, null, logger)`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExecuteExtractRim_WritesExtractedResourceFiles`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update `src/Tools/KotorCLI/README.md` test count to match passing test total.
