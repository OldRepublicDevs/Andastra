---
title: "feat: 2DA selection shortcuts and go to column"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA selection shortcuts and go to column (plan 470)

Plan **465** Day 6 slice per `docs/twoda_editor_ux_and_feature_completion.md` §14.

## Requirements

- R1. **Shift+Space** → `SelectCurrentRow()` in `OnWindowKeyDown` (skip when in-cell editing)
- R2. **Ctrl+Space** → `SelectCurrentColumn()`
- R3. **Go To Column** — menu item `actionGoToColumn` in Edit menu (near Go To Row); dialog like `ShowGoToRowDialog` but picks column by name/index; navigates and focuses column
- R4. Wire menu/sidebar bindings in `SetupMenuHandlers`; localize headers in `RefreshLocalizedStrings`
- R5. Update `TwoDAKeyboardShortcutsDialog.cs` with Shift+Space, Ctrl+Space, Go To Column
- R6. Tests in `OdyTool2DATests.cs`: Shift+Space row select, Ctrl+Space column select, Go To Column by name
- R7. Update plan 465 Day 6 section and README row for 470

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_
```

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#141](https://github.com/th3w1zard1/Andastra/pull/141) | `1abe9337e` | **100** `OdyTool2DA_*` |
