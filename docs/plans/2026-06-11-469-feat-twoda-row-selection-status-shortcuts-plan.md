---
title: "feat: 2DA row selection, dirty status, keyboard shortcuts"
status: complete
origin: docs/plans/2026-06-11-465-chore-multi-day-pr-merge-holocron-integration-plan.md
---

# feat: 2DA row selection, dirty status, keyboard shortcuts (plan 469)

Plan **465** Day 5 first code slice per `docs/twoda_editor_ux_and_feature_completion.md`.

## Requirements

- R1. **Select Row** — menu, sidebar, and `#` column click select a single row (mirrors Select Column).
- R2. **Status bar dirty indicator** — show `Modified` when `IsDirty`.
- R3. **Status bar filter hint** — when column filter active, show hidden row count.
- R4. **Keyboard shortcuts dialog** — Help → Keyboard Shortcuts listing 2DA editor shortcuts.
- R5. Tests in `OdyTool2DATests` for select row, dirty status, and shortcuts dialog smoke.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyTool2DA_
```

## Landed

| PR | Merge | Tests |
|----|-------|-------|
| [#140](https://github.com/th3w1zard1/Andastra/pull/140) | `4ceb97899` | **97** `OdyTool2DA_*` |
