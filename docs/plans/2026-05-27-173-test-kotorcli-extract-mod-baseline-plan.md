---
title: "test: kotorcli extract mod baseline"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-172-test-kotorcli-extract-erf-baseline-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract MOD baseline (plan 173)

## Summary

Add `ExecuteExtractMod_WritesExtractedResourceFiles` integration test in `ExtractCommandTests.cs`, mirroring `ExecuteExtractErf_WritesExtractedResourceFiles`. MOD filter paths exist; this closes the happy-path extract baseline gap for `.mod` without a filter.

## Requirements

- R1. `ExtractCommand.Execute` with a sample `.mod` exits zero when no filter is supplied.
- R2. Output directory contains both extracted resources from the sample archive.
- R3. Use `CreateSampleModWithTwoResources` so the archive has known resources; all resources are written when filter is null.

## Implementation

- Copy structure from `ExecuteExtractErf_WritesExtractedResourceFiles` (line ~88).
- Place after ERF baseline test; use temp dir + `finally` cleanup pattern consistent with sibling tests.
- Call `ExtractCommand.Execute(modPath, outputDir, null, null, logger)`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExecuteExtractMod_WritesExtractedResourceFiles`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update `src/Tools/KotorCLI/README.md` test count to match passing test total.
