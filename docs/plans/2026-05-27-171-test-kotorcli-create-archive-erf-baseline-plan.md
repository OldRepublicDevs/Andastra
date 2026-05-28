---
title: "test: kotorcli create-archive erf baseline"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-170-test-kotorcli-list-search-erf-edge-cases-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI create-archive ERF baseline (plan 171)

## Summary

Add `Execute_CreateErfFromDirectory_ProducesReadableArchive` integration test in `CreateArchiveCommandTests.cs`, mirroring existing RIM and MOD create-archive baseline tests. ERF filter paths are already covered; this closes the happy-path create baseline gap for `.erf`.

## Requirements

- R1. `CreateArchiveCommand.Execute` with type `erf` exits zero when packing a directory containing a valid `.utc` resource.
- R2. Output `.erf` file exists on disk.
- R3. `LazyCapsule` can read the archive and enumerate the packed `merchant.utc` resource by name and type.

## Implementation

- Copy structure from `Execute_CreateRimFromDirectory_ProducesReadableArchive` and `Execute_CreateModFromDirectory_ProducesReadableArchive`.
- Use `packed.erf` output path and archive type `"erf"`.
- Place after MOD baseline test; use `DeleteDirectorySafe` in `finally` like MOD/ERF filter tests.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CreateArchiveCommandTests`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update `src/Tools/KotorCLI/README.md` test count to match passing test total.
