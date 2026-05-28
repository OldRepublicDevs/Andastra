---
title: "test: kotorcli search-archive case-sensitive happy path"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-154-test-kotorcli-extract-error-paths-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive case-sensitive happy path (plan 155)

## Summary

Positive integration tests pairing with plan 153: `--case-sensitive` succeeds when name or content pattern matches exact case.

## Requirements

- R1. Case-sensitive name wildcard `sample_*` matches `sample_npc` resref.
- R2. Case-sensitive content search matches lowercase `archive-test` in payload.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SearchArchive`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
