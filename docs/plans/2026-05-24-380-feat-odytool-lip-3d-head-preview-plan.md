---
title: "feat: odyTool LIP 3D head preview with appearance picker"
type: feat
status: active
date: 2026-05-24
origin: docs/brainstorms/2026-05-24-lip-editor-playback-preview-requirements.md
branch: feat/plan-380-lip-3d-head-preview
depends_on: plan 379
base_branch: feat/plan-379-lip-playback-sync
---

# feat: OdyTool LIP 3D head preview (plan 380)

## Summary

Add a creature head preview panel to OdyToolLIP: Appearance picker backed by `appearance.2da`, `ModelRenderer` viewport loading head MDL from the active installation, and mouth-state UI feedback driven by playback sync (label overlay — mesh deformation deferred).

## Requirements

- **R6.** Appearance picker loads creature head MDL from installation (OdyToolUTC / `ModelRenderer` pattern).
- **R7.** During playback, drive visible mouth state from active LIP shape (vertical slice: overlay label on viewport; no mesh deformation).
- **R8.** Graceful degradation when no installation is configured (message, no crash).

## Implementation Units

### U1. Head preview helper

**Files:** `src/Tools/OdyTools/Utils/LipHeadPreviewHelper.cs`

- `TryPopulateAppearanceCombo(OdyInstallation, ComboBox)` — cache `appearance.2da`, fill labels.
- `TryLoadHeadModel(OdyInstallation, int appearanceId, out byte[] mdl, out byte[] mdx, out string modelName)` — UTC-style body model resolve + MDL/MDX load.
- `GetMouthStateLabel(LIPShape?)` — overlay text for active viseme.
- `NoInstallationMessage` constant for R8.

### U2. ModelRenderer mouth overlay

**Files:** `src/Tools/OdyTools/Widgets/ModelRenderer.cs`

- Public `SetPlaybackHint(string)` — bottom overlay TextBlock mouth viseme hint; non-GPU testable via `LipHeadPreviewHelper.FormatPlaybackOverlay`.

### U3. OdyToolLIP UI integration

**Files:** `src/Tools/OdyTools/Editors/OdyToolLIP.axaml.cs`, `src/Tools/OdyTools/Editors/OdyToolLIP.Standalone.csproj`

- Split layout: left = Appearance combo + `ModelRenderer` + status; right = existing LIP editor controls.
- Wire appearance selection → `RefreshHeadPreview()`.
- Wire `UpdatePlaybackSync()` / `ResetPreviewDisplay()` → `SetPlaybackHint`.
- Standalone csproj: include `ModelRenderer`, `MdlToDrawableConverter`, `LipHeadPreviewHelper`.

### U4. Tests (no GPU)

**Files:** `tests/OdyTools.Tests/OdyToolLIPTests.cs`

- `GetMouthStateLabel` shape/null cases.
- `TryPopulateAppearanceCombo` with null installation (false, no throw).
- Existing playback sync tests unchanged.

## Verification

```bash
dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0
dotnet test tests/OdyTools.Tests/OdyTools.Tests.csproj --framework net9.0 --filter FullyQualifiedName~OdyToolLIP
```

## Scope Boundaries

- No timeline scrubber.
- No runtime `LipSyncController` integration.
- No mesh mouth deformation (feasibility spike → label-only fallback for 380).
- No LIP editor settings dialog (uses installation passed from host / constructor).

## Branch

Created from `feat/plan-379-lip-playback-sync` (plan 379 playback sync is prerequisite).
