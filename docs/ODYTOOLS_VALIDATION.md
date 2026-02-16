# OdyTools Installer & Shell Integration Validation

This document provides validation steps for the OdyTools optional installer, file associations, shell integration, and NetSparkle update flow. Use it for manual verification and as a reference for automated checks.

## 1. Installer (Install / Modify / Repair / Uninstall)

### Prerequisites

- Build the installer: `.\helper_scripts\publish.ps1 -BuildOdyToolsInstaller`
- MSI output: `installer\odytools\bin\Release\*.msi` (or equivalent WiX output path)

### Per-Machine Install

- [ ] Run the MSI as administrator. Complete a **Fresh install**.
- [ ] Verify `OdyTools.exe` (AIO) is installed under `Program Files\OdyTools\`.
- [ ] Verify selected standalone editors appear under `Program Files\OdyTools\Editors\`.
- [ ] Run **Modify** from Add/Remove Programs; change feature selection (add/remove editors). Confirm changes apply.
- [ ] Run **Repair**. Confirm installation is repaired without errors.
- [ ] Run **Uninstall**. Confirm clean removal (shortcuts, registry entries, files).

### Per-User Install (if supported)

- [ ] Install without admin elevation (if per-user install is configured).
- [ ] Verify install location and uninstall behavior.

---

## 2. File Associations & Context Menus

### ProgIDs and Open With

After install with associations enabled (`ODyTOOLS_ENABLE_ASSOCIATIONS=1`):

- [ ] Right-click a `.2da` file → **Open with** → OdyTools / OdyTool2DA should appear.
- [ ] Repeat for `.gff`, `.tlk`, `.dlg`, `.erf`.

### Cascading OdyTools Context Menu

Right-click a supported file (e.g. `.gff`) → **OdyTools** submenu:

- [ ] **Convert to GFF** (or **Convert to 2DA** / **Convert to JSON** as applicable) runs and produces output.
- [ ] **Pick diff target 1** stores the selected path.
- [ ] **Set diff target 2 and start diff** runs the diff workflow.
- [ ] **Pick diff target 2 and start diff (GUI)** opens the TSLPatchData editor after diff.

Right-click a **directory** or **directory background** → **OdyTools**:

- [ ] **Pick diff target 1** stores the directory.
- [ ] **Set diff target 2 and start diff** and GUI variant work.

### Guided Defaults

- [ ] After first install, if `ODyTOOLS_GUIDED_DEFAULTS=1`, user is guided to set default apps (e.g. Settings → Apps → Default apps).

---

## 3. Shell Entry Points (Command-Line)

From a terminal, run from the installed OdyTools directory or with full path:

- [ ] `OdyTools.exe --shell-open "path\to\file.gff"` opens the file with the system default/associated editor.
- [ ] `OdyTools.exe --shell-convert gff "path\to\file.json"` produces a GFF file.
- [ ] `OdyTools.exe --shell-convert 2da "path\to\file.json"` produces a 2DA file.
- [ ] `OdyTools.exe --shell-convert json "path\to\file.gff"` produces a JSON file.
- [ ] `OdyTools.exe --shell-diff-pick1 "path\to\mod1"` stores target 1.
- [ ] `OdyTools.exe --shell-diff-run "path\to\mod2"` generates `tslpatchdata` and `changes.ini`.
- [ ] `OdyTools.exe --shell-diff-run "path\to\mod2" --gui` opens the TSLPatchData editor after diff.

---

## 4. AIO Open-With-Default-Editor Behavior

- [ ] Double-click or **Open** a resource in the AIO main window.
- [ ] On Windows, if a default editor is associated for that type, the file opens in that editor.
- [ ] Fallback: if no associated editor, internal OdyTools editor is used.
- [ ] Non-Windows: behavior unchanged (no registry dependency).

---

## 5. Diff Target 1/2 Workflow & TSLPatchData Validity

- [ ] Use context menu: **Pick diff target 1** on `mod_folder_1`, then **Set diff target 2 and start diff** on `mod_folder_2`.
- [ ] Confirm output under `%LocalAppData%\Andastra\OdyTools\DiffPatches\<timestamp>\`:
  - [ ] `tslpatchdata` folder with changed files.
  - [ ] `changes.ini` with valid TSLPatcher mod structure.
- [ ] Open the generated `tslpatchdata` folder in **OdyPatch** (or equivalent). Confirm it loads and lists changes correctly.

---

## 6. NetSparkle Update Flow

### Installer Channel

- [ ] Install OdyTools via MSI.
- [ ] Trigger **Check for Updates** from the application.
- [ ] If an update is available: prompt → download → install → relaunch.
- [ ] Confirm the updated version runs and appcast/artifact links are valid.

### Portable Channel

- [ ] Run OdyTools from a portable (e.g. extracted zip) package.
- [ ] Trigger **Check for Updates**.
- [ ] Verify update flow for portable artifacts (download and replace / extract as configured).

### Signature Validation

- [ ] Ensure signature validation is strict when Ed25519 public key is configured.
- [ ] Confirm unsigned or tampered appcast items are rejected.

---

## 7. CI Artifact Integrity

For release builds:

- [ ] Hash/signature checks for MSI, portable zips, and standalone editor zips.
- [ ] Appcast XML links point to correct artifact URLs.
- [ ] Version strings in appcast match built artifacts.

---

## Quick Smoke Test

Minimal check for a quick regression run:

1. Build: `.\helper_scripts\publish.ps1 -BuildOdyToolsInstaller`
2. Install the MSI (default options).
3. Right-click a `.gff` file → OdyTools → Pick diff target 1.
4. Right-click another folder → OdyTools → Set diff target 2 and start diff (GUI).
5. Confirm TSLPatchData editor opens with valid diff output.
6. In OdyTools, use **Check for Updates** (no update required; just verify no crash).
