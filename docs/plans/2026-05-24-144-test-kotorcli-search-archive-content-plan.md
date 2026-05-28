---
title: "test: kotorcli search-archive content mode"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-143-chore-kotorcli-pr7-archive-pr-refresh-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI search-archive content mode (plan 144)

## Summary

Cover `search-archive --content`: match pattern inside resource bytes when name does not match.

## Requirements

- R1. With `searchContent: true`, pattern found only in GFF payload exits 0.
- R2. With `searchContent: false`, same pattern exits non-zero when resource names do not match.
- R3. Reuse existing RIM test helper with distinctive Label string.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~SearchArchive_Content`
