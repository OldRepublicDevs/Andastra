---
title: "docs: odytools 2da reference search kb and build parity test"
type: docs
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-28-200-feat-odytools-2da-editor-find-references-plan.md
branch: feat/holocron-port-phase-b
---

# docs: OdyTools 2DA reference KB + build parity (plan 201)

## Summary

Close Holocron port documentation and test gaps after plan 200: document **2DA editor Find References** in the product UX knowledgebase, and prove `OdyTool2DA.Build()` produces a `TwoDA` suitable for `CollectTwoDARowReferences` (same object path as the context menu).

## PyKotor / Holocron parity

Holocron row reference sweep uses the in-memory 2DA table plus installation scan. Plan 200 wires the menu; plan 201 ensures KB agents know the surface exists and tests that `Build()` row labels/cells match BioWare sweep inputs.

## Requirements

- R1. Update `docs/knowledgebase/30-product-ux/odytools-editor-ux.md` verification table: 2DA row Find References (plan 200), reference search options dialog (module glob, StrRef NCS flags).
- R2. Mark `docs/plans/2026-05-24-101-feat-odytools-module-glob-search-options-plan.md` status **complete** (implementation landed).
- R3. Test `OdyTool2DA_Build_SuppliesTwoDAForRowReferenceCollect` in `OdyTool2DATests.cs` (no UI; `Build()` + `CollectTwoDARowReferences`).

## Verification

```bash
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~OdyTool2DA_Build_SuppliesTwoDA"
```
