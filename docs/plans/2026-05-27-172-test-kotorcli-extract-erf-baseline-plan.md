---
title: "test: kotorcli extract erf baseline"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-171-test-kotorcli-create-archive-erf-baseline-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract ERF baseline (plan 172)

## Summary

Add `ExecuteExtractErf_WritesExtractedResourceFiles` integration test in `ExtractCommandTests.cs`, mirroring `ExecuteExtractBif_WritesExtractedResourceFiles`. ERF filter paths exist at lines 452+; this closes the happy-path extract baseline gap for `.erf` without a filter.

## Requirements

- R1. `ExtractCommand.Execute` with a sample `.erf` exits zero when no filter is supplied.
- R2. Output directory contains at least one extracted file.
- R3. Use `CreateSampleErfWithTwoResources` (or equivalent) so the archive has known resources; all resources are written when filter is null.

## Implementation

- Copy structure from `ExecuteExtractBif_WritesExtractedResourceFiles` (line ~54).
- Place after BIF baseline test; use temp dir + `finally` cleanup pattern consistent with sibling tests.
- Call `ExtractCommand.Execute(erfPath, outputDir, null, null, logger)`.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ExecuteExtractErf_WritesExtractedResourceFiles`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update `src/Tools/KotorCLI/README.md` test count to match passing test total.
