---
title: "LIP editor playback sync and 3D preview arc"
date: 2026-05-24
status: active
---

# LIP editor playback sync and 3D preview arc

## Summary

Extend OdyToolLIP in two plans: **379** adds Holocron-parity playback sync (active viseme label and keyframe list highlight while audio plays); **380** adds a 3D creature head preview driven by installation Appearance, going beyond Holocron’s text-only preview.

## Key Decisions

- **Phased delivery:** Ship Holocron playback sync (379) before 3D head preview (380) so plan 378 audio load/Play/Stop can merge independently.
- **379 uses Holocron discrete shape lookup** (last keyframe at or before playback time), not engine interpolation, for 1:1 parity with `lip_editor.py`.
- **380 head source:** Creature Appearance from active game installation (OdyToolUTC pattern), not a bundled offline mesh.
- **380 mouth motion:** Feasibility spike on one Appearance before committing to mesh deformation vs discrete part swap.

## Requirements

### Plan 379 — Playback sync (Holocron parity)

- **R1.** While preview audio plays, poll playback position and show the active lip shape name in a preview label (Holocron `preview_label`).
- **R2.** While preview audio plays, highlight/select the keyframe row matching the active shape time (extends Holocron list feedback).
- **R3.** Stop playback and reset preview label to `None`; stop position timer (Holocron `stop_preview` / `on_playback_state_changed`).
- **R4.** Play requires loaded audio; show warning when missing (Holocron guard).
- **R5.** Unit tests cover discrete shape/index resolution at playback times without audio hardware.

### Plan 380 — 3D head preview (follow-up)

- **R6.** Appearance picker loads creature head MDL from installation (reuse OdyToolUTC / ModelRenderer pattern).
- **R7.** During playback, drive visible mouth state from active LIP shape (implementation TBD after mesh spike).
- **R8.** Graceful degradation when no installation is configured (message, no crash).

## Scope Boundaries

**In scope (379):** Timer-driven position sync, preview label, list highlight, Space/Escape play/stop shortcuts optional if low cost.

**Deferred (380):** 3D head viewport, mesh deformation research, interpolated viseme blending in the tool UI.

**Out of scope:** Timeline scrubber, lip shape bitmap images, runtime LipSyncController integration inside OdyTools, Module Designer.

## Acceptance Examples

- **AE1:** Load WAV + keyframes, press Play → preview label shows shape at current time; matching list row highlights.
- **AE2:** Press Stop → label shows `None`, timer stops, list selection clears or stays without forced highlight.
- **AE3:** Test fixture with keyframes at 1.0s and 2.5s returns index 0 at t=1.5s and index 1 at t=3.0s.

## Dependencies

- Plan **378** (audio load, Play/Stop, NAudioMediaPlayer) merged or stacked on same branch.
- BioWare `LIP` keyframe list; Holocron reference `vendor/src/toolset/gui/editors/lip/lip_editor.py`.
