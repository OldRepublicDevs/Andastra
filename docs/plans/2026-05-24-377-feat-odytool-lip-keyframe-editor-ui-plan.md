---
title: "feat: odyTool LIP keyframe editor ui (holocron parity)"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: vendor/src/toolset/gui/editors/lip/lip_editor.py
branch: feat/plan-377-lip-keyframe-editor-ui
---

# feat: OdyTool LIP keyframe editor UI (plan 377)

## Summary

Port core Holocron `LIPEditor` keyframe editing UI into `OdyToolLIP`: duration field, keyframe list, time/shape inputs, and add/update/delete actions. Audio preview and viseme preview deferred.

## Requirements

- R1. Keyframe list displays sorted time + shape for each frame.
- R2. Duration numeric input bound to `Duration` property (Holocron `durationValueLabel` / length).
- R3. Add / Update / Delete keyframe buttons wired to existing editor methods with undo push.
- R4. Selecting a list row loads time and shape into inputs (Holocron `on_keyframe_selected`).
- R5. `LoadLIP` / `New` / undo-redo refresh the list; unit test covers keyframe round-trip via `Build`.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolLIP
```

## Scope Boundaries

- No WAV load/play preview (Holocron `load_audio` / QMediaPlayer) — follow-up slice.
- No lip shape preview image widget — follow-up slice.
