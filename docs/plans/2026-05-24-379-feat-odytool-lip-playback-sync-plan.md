---
title: "feat: odyTool LIP playback position sync and preview label"
type: feat
status: complete
date: 2026-05-24
completed: 2026-05-24
origin: docs/brainstorms/2026-05-24-lip-editor-playback-preview-requirements.md
branch: feat/plan-379-lip-playback-sync
depends_on: plan 378
---

# feat: OdyTool LIP playback position sync (plan 379)

## Summary

Port Holocron `on_playback_position_changed`, `update_preview_display`, and playback timer behavior: while WAV preview plays, show active viseme name and highlight the matching keyframe row.

## Requirements

- R1. Preview label shows active shape during playback (Holocron `preview_label`).
- R2. Keyframe list highlights row for last keyframe at or before playback time.
- R3. Stop clears label to `None`, stops timer (Holocron `stop_preview`).
- R4. Discrete shape lookup matches Holocron (not engine interpolation) for parity.
- R5. Unit tests for index/shape resolution at sample times.

## Implementation Units

### U1. Playback sync UI and timer

**Files:** `src/Tools/OdyTools/Editors/OdyToolLIP.axaml.cs`

- Add preview label row below audio controls.
- `DispatcherTimer` (~16ms) polls `NAudioMediaPlayer.Position`.
- Wire `PlaybackStopped` to reset UI.
- Optional: Space/Escape shortcuts for play/stop.

### U2. Holocron discrete lookup + tests

**Files:** `src/Tools/OdyTools/Editors/OdyToolLIP.axaml.cs`, `tests/OdyTools.Tests/OdyToolLIPTests.cs`

- Public/static helpers: `GetKeyframeIndexAtTime(LIP lip, float time)`, `GetShapeAtPlaybackTime(LIP lip, float time)`.
- Tests without audio device.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolLIP
```

## Scope Boundaries

- No 3D head preview (plan 380).
- No timeline scrubber.
- No `LIP.GetShapeAtTime` interpolation in 379 UI (Holocron uses discrete last-keyframe logic).
