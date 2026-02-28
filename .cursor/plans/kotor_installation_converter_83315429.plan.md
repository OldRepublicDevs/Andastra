---
name: KOTOR installation converter
overview: Create a standalone GUI tool `ConvertKotorGame` under `src/Tools/ConvertKotorGame` that auto-detects K1/TSL installs, shows install metadata, and performs an exhaustive “format portability” conversion into a new output folder by rewriting every supported resource across containers and loose files into the opposite game’s serialization variant with progress + color-coded logs.
todos:
  - id: new-tool-skeleton
    content: Create `src/Tools/ConvertKotorGame` Avalonia app skeleton (csproj + App + MainWindow + MVVM plumbing).
    status: completed
  - id: ui-requirements
    content: Implement the requested UI (auto-detect + editable combobox + browse + metadata + logs + progress + convert-to-inverse button).
    status: completed
  - id: conversion-engine
    content: Implement exhaustive traversal + container rewrite pipeline writing into a new sibling output directory.
    status: completed
  - id: resource-dispatcher
    content: Implement resource-type conversion dispatcher (GFF per-game emit, MDL variant emit, NCS decompile+compile, copy-as-is fallbacks with explicit logging).
    status: completed
  - id: exhaustive-coverage
    content: Close format gaps (missing GFF content types, WOK/DWK/PWK if needed) and add dry-run inventory + summary reporting to guarantee no silent omissions.
    status: completed
isProject: false
---

## Goals

- Create a **separate tool**: `src/Tools/ConvertKotorGame`.
- Provide a **GUI app** that:
  - Auto-detects installs and populates a top editable combobox.
  - Includes an `auto-detect` entry to re-run detection.
  - Allows manual typing of a path.
  - Has a `Browse` button to the right of the combobox.
  - Shows detected **game type** (K1/TSL) to the left of the combobox.
  - Shows detected **distribution (Steam/GOG/Amazon/etc)** + **OS/platform/arch** under the combobox.
  - Has a central, color-coded log list.
  - Has a bottom progress bar and a `Convert to <inverse>` button.
- Perform **format portability** conversion (not “make the other game’s content”) into a **new sibling output directory**.

## Project placement + build wiring

- **New project**: `[c:\GitHub\Andastra\src\Tools\ConvertKotorGame\ConvertKotorGame.csproj](c:\GitHub\Andastra\src\Tools\ConvertKotorGame\ConvertKotorGame.csproj)`.
- **Default wiring**: do **not** modify `[c:\GitHub\Andastra\Andastra.sln](c:\GitHub\Andastra\Andastra.sln)` (standalone tool).
  - If later desired, we can add it to `Andastra.sln`.

## UI framework choice

- Use **Avalonia** (already used by `OdyPatch` / `OdyPatch.UI`), so we can reuse styling + patterns.

## Reuse from existing code

- **Install detection**
  - `BioWare.Extract.Installation.DetermineGame()` in `[c:\GitHub\Andastra\src\BioWare\Extract\Installation.cs](c:\GitHub\Andastra\src\BioWare\Extract\Installation.cs)`.
  - `BioWare.Tools.Heuristics.DetermineGame()` in `[c:\GitHub\Andastra\src\BioWare\Tools\Heuristics.cs](c:\GitHub\Andastra\src\BioWare\Tools\Heuristics.cs)`.
  - Default path detection logic exists in:
    - `Andastra.Game.Core.GamePathDetector` (`DetectKotorPath`, `FindKotorPathsFromDefault`) in `[c:\GitHub\Andastra\src\Andastra\Game\Core\GamePathDetector.cs](c:\GitHub\Andastra\src\Andastra\Game\Core\GamePathDetector.cs)`
    - plus additional platform/Amazon handling patterns in `[c:\GitHub\Andastra\src\Tools\OdyPatch.UI\Core.cs](c:\GitHub\Andastra\src\Tools\OdyPatch.UI\Core.cs)` (we’ll port the relevant parts into this tool, not depend on OdyPatch.UI).
- **Containers and formats**
  - BIF/BZF: `BIFBinaryReader`/`BIFBinaryWriter`.
  - KEY: `KEYAuto`.
  - ERF/MOD/SAV/HAK: `ERFAuto`.
  - RIM: `RIMAuto`.
  - GFF-family helpers under `[c:\GitHub\Andastra\src\BioWare\Resource\Formats\GFF\Generics](c:\GitHub\Andastra\src\BioWare\Resource\Formats\GFF\Generics)` (many already accept `BioWareGame game` for target-specific emit).
  - MDL/MDX: `MDLAuto` + `MDLBinaryWriter` (supports game variant).
  - NCS tooling under `src\BioWare\Resource\Formats\NCS`.

## Conversion rules (must be exhaustive)

The converter runs in two layers:

- **Layer A**: detect and rewrite containers (convert every contained resource payload).
- **Layer B**: per-resource payload conversion/copy rules.

### A) Containers to rewrite

- `data/*.bif` and any `*.bif`/`*.bzf` found
  - Use `chitin.key` to map resource IDs to ResRefs.
  - Rewrite BIF resources with converted payload bytes.
- `*.erf`, `*.mod`, `*.sav`, `*.hak`, `*.nwm`
  - Read archive, convert each resource, write new archive.
- `*.rim`
  - Read archive, convert each resource, write new rim.

### B) Resource payload conversion matrix

- **GFF-based types** (`gff`, `are`, `git`, `ifo`, `utc`, `uti`, etc.)
  - Convert by parsing, then dismantling/writing with `targetGame` where helpers exist.
  - **Gap work**: implement remaining missing GFF content types required for exhaustiveness (`BIC/BTC/BTD/BTE/BTI/BTP/BTM/BTT/ITP/PT/GVT/INV/RES`) using the same PyKotor-port approach used by existing helpers.
  - Temporary fallback (only while a type is unimplemented): generic `GFFAuto.ReadGff` → `GFFAuto.WriteGff` (logged as “fallback used”, and fails the “exhaustive” checklist until eliminated).
- **Models** (`mdl`/`mdx`)
  - Convert: read → write using `MDLBinaryWriter(..., game: targetGame)`.
  - **Gap work**: expose a safe API surface so conversion can explicitly choose K1 vs K2 variant (today `MDLAuto.WriteMdl` doesn’t surface the game parameter).
- **Scripts**
  - `ncs`: convert via decompile → compile for `targetGame` (warn and keep original bytes if compilation fails).
  - `nss`: copy as-is.
- **Walkmeshes / nav**
  - `bwm`: convert using `BWMAuto`.
  - `wok/dwk/pwk`: confirm support; implement or explicitly classify.
- **Textures, audio, video, misc binaries**
  - Copy as-is unless we have proven read/write parity.

## GUI implementation (ConvertKotorGame)

- Files (planned):
  - `[c:\GitHub\Andastra\src\Tools\ConvertKotorGame\App.axaml](c:\GitHub\Andastra\src\Tools\ConvertKotorGame\App.axaml)` + `App.axaml.cs`
  - `[c:\GitHub\Andastra\src\Tools\ConvertKotorGame\Views\MainWindow.axaml](c:\GitHub\Andastra\src\Tools\ConvertKotorGame\Views\MainWindow.axaml)` + `.cs`
  - `[c:\GitHub\Andastra\src\Tools\ConvertKotorGame\ViewModels\MainWindowViewModel.cs](c:\GitHub\Andastra\src\Tools\ConvertKotorGame\ViewModels\MainWindowViewModel.cs)`
  - Services under `ConvertKotorGame/Services/` (detector + converter engine)
- UI layout matches your spec (top selector + metadata + log list + progress + convert button).

## Conversion engine structure

- `InstallationFormatConverter` service with:
  - Inputs: source path, inferred source game, target game, output dir, logger callback, progress callback, cancellation.
  - Enumerates all relevant files and dispatches conversion.
- `ResourceConverter` dispatcher:
  - `ConvertResourceBytes(resRef, resType, bytes, sourceGame, targetGame)` implementing the conversion matrix.

## Verification plan

- Add a “dry-run inventory” mode that only enumerates what would be converted and classifies every file/type (to prove we’re not skipping anything) before the first real conversion run.
- Add summary counts by resource type + by container.

