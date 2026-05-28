---
title: "test: kotorcli list-archive bif without sibling key"
type: test
status: active
date: 2026-05-27
origin: docs/plans/2026-05-27-184-test-kotorcli-extract-default-output-plan.md
branch: feat/holocron-port-phase-b
---

# test: KotorCLI list-archive BIF without sibling KEY (plan 185)

## Summary

Add integration tests for `list-archive` on a standalone `.bif` with no `chitin.key` or `{stem}.key` beside it. BIF files store resource data and KEY indices only; without KEY merge, `ArchiveHelpers.ListBif` yields blank ResRefs and listing uses extension-only display names (e.g. `.utc`).

## Requirements

- R1. `ListArchiveCommand.Execute` on a BIF with no sibling KEY exits 0 when no filter is applied.
- R2. Wildcard filter on extension (e.g. `*.utc`) matches blank-ResRef BIF entries without KEY.
- R3. A filter that matches no listed name exits non-zero, consistent with BIF+KEY filter tests.

## Verification

- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0`
- Update README test count to **249**.
