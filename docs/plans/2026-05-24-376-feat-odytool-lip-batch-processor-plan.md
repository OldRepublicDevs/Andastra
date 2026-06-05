---
title: "feat: odyTool LIP batch WAV processor (holocron parity)"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: vendor/src/toolset/gui/editors/lip/batch_processor.py
branch: feat/plan-376-lip-batch-processor
---

# feat: OdyTool LIP batch WAV processor (plan 376)

## Summary

Port Holocron `BatchLIPProcessor` (`vendor/src/toolset/gui/editors/lip/batch_processor.py`) into OdyTools: batch-convert WAV files to placeholder LIP lip-sync files from audio duration. Wire from **Tools → Batch Process WAV to LIP…** in `OdyToolLIP`.

## Requirements

- R1. `LipBatchProcessor` generates LIP bytes from WAV duration using the same placeholder shape sequence as Holocron (MPB, AH, OH, MPB).
- R2. `LipBatchProcessorDialog` supports add/remove/clear audio list, output directory browse, and batch process with per-file error reporting.
- R3. `OdyToolLIP` exposes menu action opening the dialog.
- R4. Unit tests cover duration extraction and batch output without GUI.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~LipBatchProcessor
```

## Scope Boundaries

- Placeholder lip-sync shapes only (matches Holocron batch processor); no phoneme analysis.
- No documentation-only tracker sync in this slice.
