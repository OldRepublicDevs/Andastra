---
title: "test: kotorcli resolve sibling key path unit tests"
type: test
status: active
date: 2026-05-24
origin: docs/plans/2026-05-24-141-test-kotorcli-search-archive-bif-key-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI ResolveSiblingKeyPath unit tests (plan 142)

## Summary

Unit-test `ArchiveCommandHelpers.ResolveSiblingKeyPath` for `chitin.key` precedence, `{stem}.key` fallback, and null when missing.

## Requirements

- R1. When `chitin.key` exists beside BIF, return chitin path (even if `{stem}.key` also exists).
- R2. When only `{stem}.key` exists, return stem key path.
- R3. When no KEY exists, return null.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FullyQualifiedName~ResolveSiblingKeyPath`
