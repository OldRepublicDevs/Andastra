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

## Implementation Units

### U1. Audio load UI and `LoadAudioFile`

**Goal:** WAV picker row, duration from WAV, undo clear, player source wiring.

**Requirements:** R1, R3

**Files:**
- `src/Tools/OdyTools/Editors/OdyToolLIP.axaml.cs`
- `src/Tools/OdyTools/Editors/OdyToolLIP.Standalone.csproj`

**Test scenarios:**
- Covers R4: `LoadAudioFile` sets `Duration` from WAV header length.
- Load audio after keyframe edit clears `CanUndo` / `CanRedo`.

### U2. Preview playback controls

**Goal:** Play/Stop wired to `NAudioMediaPlayer`; dispose on window close.

**Requirements:** R2

**Files:**
- `src/Tools/OdyTools/Editors/OdyToolLIP.axaml.cs`

**Test scenarios:** Test expectation: none — playback requires audio device; covered by manual/local validation.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolLIP
```

## Scope Boundaries

- No playback scrubber / position sync with keyframes (Holocron `on_playback_position_changed`) — follow-up plan 379.
- No lip shape preview image — follow-up.
