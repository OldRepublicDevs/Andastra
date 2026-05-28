---
title: "feat: odyTools 2DA editor find row references"
type: feat
status: complete
date: 2026-05-28
origin: docs/plans/2026-05-24-083-feat-holocron-phase-n-2da-memory-find-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: OdyTool2DA Find References (plan 200)

## Summary

Wire **Find References** on the 2DA spreadsheet editor context menu, matching Holocron `ComboBox2DA` / row reference sweep via `TwoDAMemoryReferenceHelper` and BioWare `CollectTwoDARowReferences`.

## PyKotor / Holocron parity

Holocron exposes **Find References...** on 2DA row pickers (`combobox_2da.py`). The full 2DA editor should offer the same sweep for the selected row using the open table's row index and in-memory `TwoDA` from `Build()`.

## Requirements

- R1. Data grid context menu item **Find References...** (`ctxFindRowReferences`).
- R2. Enabled when installation is set, `_resname` is set, and exactly one row is selected (or primary selected row index ≥ 0).
- R3. Calls `TwoDAMemoryReferenceHelper.FindAndShowTwoDARowReferences` with current `_resname`, row index, and `TwoDA` from `Build()`.
- R4. Test `GetPrimarySelectedRowIndex` selection mapping in `OdyTool2DATests`.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter "FullyQualifiedName~OdyTool2DA_GetPrimarySelectedRowIndex"
```
