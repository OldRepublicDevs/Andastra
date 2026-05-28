---
title: "feat: find-2da-ref and find-field-value json output"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-097-feat-find-strref-json-count-only-plan.md
branch: feat/holocron-port-phase-b
---

# feat: find-2da-ref and find-field-value JSON output (plan 098)

## Summary

Complete KotorCLI find-command machine-readable output parity by adding `--json` and `--count-only` to `find-2da-ref` and `find-field-value`.

## Requirements

- R1. Both commands accept `--json` and `--count-only` with the same semantics as `find-refs` / `find-strref`.
- R2. JSON `type` values: `2da-ref` (needle `twoda:row`) and `field-value` (needle = searched value).
- R3. Reuse `ReferenceSearchOutputFormatter`; add shared `EmitReferenceResults` helper to avoid duplicated output branches.
- R4. Tests: JSON hit/miss and count-only for each command.
- R5. README flags updated for both commands.

## Scope Boundaries

- No refactor of existing find-refs/find-strref output paths beyond optional helper adoption.

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~Find2DARef|FullyQualifiedName~FindFieldValue"
