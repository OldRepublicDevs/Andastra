---
title: "test: kotorcli extract default output directory"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-183-test-kotorcli-bif-key-list-verbose-case-sensitive-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI extract default output directory (plan 184)

## Summary

Add integration test verifying `extract` uses the archive stem under the current working directory when `--output` is omitted (`ExtractCommand` lines 75–84).

## Requirements

- R1. Extracting a RIM with `output` null creates `{cwd}/{archive_stem}/` and writes resource files there.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **246**.
