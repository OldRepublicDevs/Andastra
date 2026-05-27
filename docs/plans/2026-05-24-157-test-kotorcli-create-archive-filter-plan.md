---
title: "test: kotorcli create-archive filter"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-156-test-kotorcli-list-archive-filter-empty-path-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI create-archive filter (plan 157)

## Summary

Integration tests for `create-archive --filter` wildcard selection when packing a RIM from a directory.

## Requirements

- R1. Filter `merchant*` packs only matching `.utc` files into the output RIM.
- R2. Filter with no matches still succeeds and produces an empty RIM archive.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~CreateArchive`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
