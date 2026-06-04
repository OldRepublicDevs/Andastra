---
title: "feat: odyTool LIP load audio and preview playback"
type: feat
status: active
date: 2026-05-24
origin: vendor/src/toolset/gui/editors/lip/lip_editor.py
branch: feat/plan-378-lip-audio-preview
---

# feat: OdyTool LIP load audio and preview playback (plan 378)

## Summary

Port Holocron `LIPEditor.load_audio`, `play_preview`, and `stop_preview` into `OdyToolLIP`: pick a WAV, set duration from file length, wire `NAudioMediaPlayer` for playback.

## Requirements

- R1. Load Audio picks `*.wav`, displays path, sets `Duration` from WAV length (reuse `LipBatchProcessor.GetWavDurationSeconds`).
- R2. Play / Stop buttons call `NAudioMediaPlayer` when audio is loaded (Holocron `QMediaPlayer`).
- R3. Loading audio clears undo/redo stacks (Holocron clears undo manager on new audio).
- R4. Unit test covers `LoadAudioFile` duration wiring without GUI file picker.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolLIP
```

## Scope Boundaries

- No playback scrubber / position sync with keyframes (Holocron `on_playback_position_changed`) — follow-up.
- No lip shape preview image — follow-up.
