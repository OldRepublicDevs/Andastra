# Plan: Impervious File Saving — OdyToolsDLG Implementation Plan

## Why This Plan Exists

The KOTOR modding community has suffered 20+ years of data loss from dialog editors that write directly to target files. When DLGEditor crashes mid-save, the file becomes 0 KB — unrecoverable. Modders resort to manually maintaining duplicate files. OdyToolsDLG currently has the **exact same vulnerability**: three call sites use raw `File.WriteAllBytes` with zero atomicity, zero backup, and zero validation. Additionally, `MarkDirty()` is never called from OdyToolDLG, meaning users get no "unsaved changes" warning, no dirty indicator in the title bar, and no protection against accidental closure. This plan eliminates every single one of these failure modes.

---

## Codebase Audit Summary (Current State)

| Aspect | Current State | Risk |
|--------|---------------|------|
| `Editor.Save()` line 656 | `File.WriteAllBytes(_filepath, data)` — no atomic write | **CRITICAL**: Crash/power-loss = 0-byte file |
| `Editor.RunSaveAsAsync()` line 759 | `File.WriteAllBytes(path, data)` — same vulnerability | **CRITICAL** |
| `FileSaveHandler.SaveFiles()` line 42 | `File.WriteAllBytes(kvp.Value, data)` — batch extraction | **HIGH** |
| `OdyToolDLG` dirty tracking | `MarkDirty()` never called — zero call sites in 8981-line file | **CRITICAL**: Edits silently lost on close |
| `DLGActionHistory` | Does not call `MarkDirty()` after Apply/Undo/Redo | **CRITICAL**: Undo/redo operations invisible to dirty tracking |
| Pre-save validation | None — `Build()` serializes blindly, `Save()` writes blindly | **HIGH**: Corrupt DLGs written to disk |
| Keyboard shortcut | `<MenuItem x:Name="actionSave" Header="Save" />` — no `InputGesture="Ctrl+S"` | **MEDIUM**: Users expect Ctrl+S |
| Autosave | Does not exist | **MEDIUM**: Only crash-recovery backups (30s interval, deleted on clean exit) |
| Crash recovery | `EditorCrashRecoveryService` exists but writes without verification, no rotation, no UI status | **LOW**: Functional but fragile |
| Backup rotation | Does not exist | **MEDIUM**: No way to recover from "saved over good version with bad version" |

---

## Plan Structure

Seven problem domains, ordered by severity. Each domain states **WHAT** is wrong, **WHY** it matters to the end user, and **WHICH FILES** to modify at a high level.

---

## Problem 1: File Writes Are Not Atomic (CRITICAL)

### What
Every save operation writes directly to the target file. If the process is interrupted (crash, power loss, disk full, antivirus lock), the file is truncated or zeroed.

### Why
This is the #1 reported cause of data loss in KOTOR dialog editing tools. The exact bug that generated 20 years of forum threads ("DLGEditor nuked my file to 0 KB") exists in OdyTools today.

### Solution: Write-to-Temp-Then-Replace Pattern

Create a shared utility that all save operations route through:

1. Serialize to `{target}.{guid}.tmp` in the same directory
2. `FileStream.Flush(flushToDisk: true)` to guarantee OS buffers are committed
3. Verify the temp file is non-zero and ideally matches expected length
4. `File.Replace(tempPath, targetPath, backupPath)` — atomic on NTFS/ext4
5. Retry transient `IOException` (file locked) up to 3 times with backoff
6. On unrecoverable failure: preserve the temp file, surface clear error to user with the temp file path so they can manually recover

### Files to Modify
| File | Change |
|------|--------|
| **NEW**: `src/Tools/OdyTools/Utils/AtomicFileWriter.cs` | Static utility: `WriteAtomic(string targetPath, byte[] data, AtomicWriteOptions options)`. Implements write-temp-verify-replace. Options: `RetryCount`, `RetryDelayMs`, `CreateBackup`, `VerifyLength`. |
| `src/Tools/OdyTools/Editors/Editor.cs` | Replace `File.WriteAllBytes(_filepath, data)` at line 656 with `AtomicFileWriter.WriteAtomic(_filepath, data)`. Same for line 759 in `RunSaveAsAsync()`. |
| `src/Tools/OdyTools/Dialogs/FileSaveHandler.cs` | Replace `File.WriteAllBytes(kvp.Value, data)` at line 42 with `AtomicFileWriter.WriteAtomic(...)`. |

### Design Constraints
- The utility must be zero-dependency (no external packages) — just `System.IO`
- Must handle `UnauthorizedAccessException` separately from `IOException` (different user message)
- Must clean up orphaned `.tmp` files from previous failed attempts on startup
- `File.Replace` requires source and destination on the same volume — handle cross-volume by falling back to copy-delete

---

## Problem 2: Dirty Tracking is Completely Broken in OdyToolDLG (CRITICAL)

### What
`MarkDirty()` (defined in `Editor.cs` base class) is called by every other editor (OdyToolGFF: 16 call sites, OdyToolNSS: 4, OdyToolPTH: 7, OdyToolSAV: 17, etc.) but has **zero calls** in the entire 8,981-line `OdyToolDLG.axaml.cs`. The `DLGActionHistory` that manages undo/redo also never calls `MarkDirty()`.

This means:
- Window title never shows `*` for unsaved changes
- Closing the window with unsaved changes shows no warning
- The crash recovery service's `IsDirty` check always returns false for DLG editors

### Why
Users will lose work constantly. They'll edit a dialog, close the window (or close the app), and never be prompted. This is silent data destruction.

### Solution: Wire MarkDirty Into Every Mutation Path

There are exactly two mutation pathways in OdyToolDLG:

**Path A — Action History (structured edits)**: All tree mutations go through `DLGActionHistory.Apply()`. This is the single chokepoint.

**Path B — Property panel edits (direct field writes)**: When users edit node text, speaker, listener, scripts, delay, animations, etc. in the right-side property panel, these writes go directly to the `DLGNode`/`DLGLink` objects without going through the action history.

### Files to Modify
| File | Change |
|------|--------|
| `src/Tools/OdyTools/Editors/DLG/DLGActionHistory.cs` | In `Apply()`, `Undo()`, and `Redo()` — call `_editor.MarkDirtyFromAction()` (new protected method on Editor or internal method on OdyToolDLG) after each operation. This covers all structural mutations (add node, delete node, paste, reorder, etc.). |
| `src/Tools/OdyTools/Editors/DLG/OdyToolDLG.axaml.cs` | Add `MarkDirty()` call in every property-panel commit handler. Audit all `TextChanged`, `SelectionChanged`, `LostFocus`, checkbox toggle, and combobox selection handlers that write to `DLGNode`/`DLGLink` fields. Key locations: text edit commit, speaker/listener change, script field change, delay/waitflags change, animation change, camera field change, sound field change, comment change, emotion/facial anim change, plot index change, quest field change. A systematic audit of all `_coreDlg`/`node.`/`link.` property assignments that come from UI event handlers is required. |
| `src/Tools/OdyTools/Editors/Editor.cs` | Add `ClearDirty()` call after successful `Build()` + `File.WriteAllBytes` (already exists at line 654). Verify `ClearDirty()` also called after successful `RunSaveAsAsync()`. Verify `ClearDirty()` called after `Load()` and `New()`. |

### Design Constraint
- Use a debouncing/coalescing approach for rapid text edits (e.g., typing in node text) — don't call `MarkDirty()` on every keystroke, but do call it on focus-lost or after a short timer. The dirty *flag* itself is idempotent (second call is a no-op), so calling it often is fine; the concern is title bar flickering if `RefreshWindowTitle()` does expensive work.

---

## Problem 3: No Keyboard Shortcut for Save (MEDIUM-HIGH)

### What
The `actionSave` MenuItem in `OdyToolDLG.axaml` has no `InputGesture` attribute. Every other professional editor uses Ctrl+S. Users will press Ctrl+S, nothing will happen, and they'll assume their work is saved.

### Why
False confidence in save state leads to data loss. Ctrl+S is the most deeply ingrained reflex in any editor user.

### Files to Modify
| File | Change |
|------|--------|
| `src/Tools/OdyTools/Editors/DLG/OdyToolDLG.axaml` | Add `InputGesture="Ctrl+S"` to `actionSave` MenuItem (line 56). Add `InputGesture="Ctrl+Shift+S"` to `actionSaveAs` MenuItem (line 57). |

Additionally, audit ALL other OdyTool*.axaml editors for missing Ctrl+S:

| File | Check |
|------|-------|
| `OdyToolGFF.axaml` | Verify `InputGesture="Ctrl+S"` on save menu item |
| `OdyToolNSS.axaml` | Same |
| `OdyTool2DA.axaml` | Same |
| `OdyToolTLK.axaml` | Same |
| Every `OdyTool*.axaml` | Same |

---

## Problem 4: No Pre-Save Validation (HIGH)

### What
`Build()` serializes the DLG model as-is. `Save()` writes the bytes as-is. No structural integrity check occurs at any point. Users can save orphaned nodes (unreachable from any starter), circular link references, missing K2-specific fields (which KotOR 2 will crash on), and invalid StrRef values.

### Why
Saving a structurally invalid DLG creates bugs that are extremely difficult to diagnose in-game. The dialog may soft-lock, skip lines, crash, or silently discard content. Users won't know until they test in-game, by which point they can't easily identify which save introduced the corruption.

### Solution: Validation Pass Before Serialization

Run validation in `Save()` between `Build()` and `File.WriteAllBytes()`. Show results to user. Allow "Save Anyway" for warnings, block for errors.

### Files to Modify
| File | Change |
|------|--------|
| **NEW**: `src/Tools/OdyTools/Editors/DLG/DLGValidator.cs` | Validation engine. Takes a `DLG` object and returns `List<DLGValidationResult>` with Severity (Error/Warning/Info), Message, and optional node reference. Rules: (1) Orphan detection — BFS from starters, flag unreachable nodes; (2) K2 field presence — if target game is TSL, verify `AnimList`, `Emotion`, `FacialAnim`, `PostProcNode`, `AlienRaceNode`, `NodeID`, `ActionParam1-5`, `Script2` fields exist on all nodes; (3) StrRef consistency — flag StrRef values that are neither -1 nor plausibly valid TLK range; (4) Empty starters list — dialog with zero starters is invalid; (5) Script field type validation — warn if a `void main()` signature script is referenced in a conditional slot (conditionals must return `int`). |
| **NEW**: `src/Tools/OdyTools/Editors/DLG/DLGValidationResult.cs` | Simple data class: `Severity`, `Message`, `NodeReference` (nullable DLGNode), `RuleId` (string enum). |
| `src/Tools/OdyTools/Editors/DLG/OdyToolDLG.axaml.cs` | Override `Save()` to run `DLGValidator.Validate(_coreDlg, _installation)` before calling `base.Save()`. If errors exist, show validation dialog. If only warnings, show dialog with "Save Anyway" option. If clean, proceed silently. |
| **NEW or in existing**: Validation results dialog (simple Avalonia Window or MessageBox with scrollable list of issues and Save Anyway / Cancel buttons). Can be a method in OdyToolDLG that builds a dialog dynamically, or a new AXAML window if the UI is complex. |

### Design Constraints
- Validation must be fast (< 50ms for 1000-node dialogs) — BFS + linear field scan
- Orphan detection already exists in `DlgGraphScene.cs` (BFS from starters, marks orphans) — reuse that algorithm
- `DLGModel` already has `GetOrphanedLinks()` or equivalent in the graph scene — reference that logic
- K2 field validation should only trigger when `_installation?.Game` indicates TSL, not for K1/NWN

---

## Problem 5: No Rolling Backup System (MEDIUM)

### What
When a user saves, the previous version is permanently overwritten. There is no way to recover a previous save. The `EditorCrashRecoveryService` only preserves crash state and deletes everything on clean exit. `File.Replace()` (used in the atomic writer) creates a single `.bak` but doesn't rotate.

### Why
Users frequently save a bad version over a good one. "I accidentally deleted half my dialog tree, then hit Ctrl+S out of habit" is a universal modding complaint. Without versioned backups, the only recourse is starting over.

### Solution: FIFO Generational Backups

On each save, rotate existing backups and keep the last N versions.

### Files to Modify
| File | Change |
|------|--------|
| **NEW**: `src/Tools/OdyTools/Utils/BackupRotator.cs` | Static utility: `RotateBackups(string filePath, int maxBackups)`. Implements: `.bak` → `.bak1` → `.bak2` → ... → delete oldest. Called by `AtomicFileWriter` before the atomic replace step. `RestoreFromBackup(string filePath, int generation)` — copies `.bakN` back to original. `GetAvailableBackups(string filePath)` — returns list of backup files with timestamps. |
| `src/Tools/OdyTools/Utils/AtomicFileWriter.cs` | Integrate `BackupRotator.RotateBackups()` into the write flow when `options.CreateBackup` is true. |
| `src/Tools/OdyTools/Editors/DLG/OdyToolDLG.axaml.cs` | Add "Restore from Backup..." menu item handler. Shows a simple dialog listing available backups (filename, timestamp, size) with a Restore button. |
| `src/Tools/OdyTools/Editors/DLG/OdyToolDLG.axaml` | Add `<MenuItem Header="Restore from Backup..." x:Name="actionRestoreBackup" />` to the File menu, after Save As. |

### Configuration (via DLGSettings or a future shared FileSafetySettings)
- `BackupsEnabled`: bool, default `true`
- `MaxBackupCount`: int, default `5`
- Could live in `DLGSettings.cs` initially, or in a new shared settings class

---

## Problem 6: Autosave Does Not Exist (MEDIUM)

### What
The `EditorCrashRecoveryService` creates periodic backup snapshots every 30 seconds to `%APPDATA%/OdyToolsV3/Backup/`. These are deleted on clean exit. This is crash recovery, **not autosave**. There is no mechanism that periodically saves the user's work to the actual target file or to a dedicated autosave location that persists across sessions.

### Why
If the user forgets to save and the app crashes (or Windows updates force-restarts), up to 30 seconds of work may survive via crash recovery, but if the user simply forgets to save and closes normally, everything since the last manual save is gone — and there's no warning because `MarkDirty()` is never called (Problem 2).

### Solution: Timer-Based Autosave to Dedicated Recovery Location

Autosave is distinct from crash recovery. Autosave periodically writes the current state to a dedicated `.autosave` file alongside the target (or to a centralized directory). These persist across sessions until the user does a manual save, at which point the autosave is deleted.

### Files to Modify
| File | Change |
|------|--------|
| **NEW**: `src/Tools/OdyTools/Utils/AutosaveService.cs` | Per-editor autosave manager. Starts a timer (default: 3 minutes, configurable). On tick: if editor `IsDirty`, call `Build()` to serialize, write to `{filepath}.autosave` (or to centralized directory) using `AtomicFileWriter`. On successful manual save: delete the `.autosave` file. On editor close (clean): delete the `.autosave` file. |
| `src/Tools/OdyTools/Editors/Editor.cs` | In constructor or `OnEditorOpened`, initialize `AutosaveService` if enabled. In `Save()` after successful write, call `AutosaveService.ClearAutosave()`. In `Load()`, check for existing `.autosave` file — if newer than target, prompt user to restore. |
| `src/Tools/OdyTools/Editors/DLG/DLGSettings.cs` | Add settings: `AutosaveEnabled` (bool, default true), `AutosaveIntervalMinutes` (int, default 3). |

### Design Constraints
- Autosave must NOT block the UI thread — serialize on background thread using a cloned/snapshot byte array from `Build()`
- Autosave should be debounced: if user is actively typing (edits within last N seconds), delay the autosave
- Autosave indicator in status bar: "Last autosaved: 2 minutes ago" (this is a UI enhancement that can come later)

---

## Problem 7: Crash Recovery Service Gaps (LOW-MEDIUM)

### What
`EditorCrashRecoveryService` is functional but has several robustness gaps:
1. Backup files are written via raw `File.WriteAllText`/`File.WriteAllBytes` — not atomic
2. No verification that the backup file is readable/valid after write
3. No backup rotation — timestamped files accumulate without cleanup
4. `OnBackupTick` calls `editor.Build()` on the UI thread (via `DispatcherTimer`) which can freeze UI for large dialogs
5. Recovery dialog is a simple Yes/No — no per-file granularity, no compare, no timestamps shown
6. No status indicator — users don't know backups are happening

### Why
The crash recovery system is the last line of defense. If IT fails (writes a corrupt backup, or freezes the UI causing the very crash it's protecting against), users have nothing.

### Files to Modify
| File | Change |
|------|--------|
| `src/Tools/OdyTools/Editors/EditorCrashRecoveryService.cs` | (1) Use `AtomicFileWriter` for backup writes. (2) Add stale-backup cleanup: on startup, delete backup files older than 7 days. (3) After writing backup, verify file size > 0. (4) Consider moving `Build()` call to a background task (clone the model first on UI thread, serialize on background thread). (5) Limit backup accumulation — keep only the latest N backup files per editor instance. |
| Crash recovery prompt UI | Enhance to show per-file recovery entries with timestamps, file sizes, and individual Restore/Discard buttons. (This is a UI polish item, lower priority than the core safety work.) |

---

## Implementation Order (Priority Sequence)

| Phase | Problems | Rationale |
|-------|----------|-----------|
| **Phase 1** | Problem 2 (Dirty Tracking) + Problem 3 (Ctrl+S) | Zero-cost, immediate user impact. Users must be warned about unsaved changes before any save infrastructure matters. |
| **Phase 2** | Problem 1 (Atomic Saves) | The core safety mechanism. Once dirty tracking is working, save operations must be bulletproof. |
| **Phase 3** | Problem 5 (Rolling Backups) | Natural extension of atomic saves — `AtomicFileWriter` + `BackupRotator` work together. |
| **Phase 4** | Problem 4 (Pre-Save Validation) | Requires understanding of what "valid DLG" means per game target. Can be developed in parallel with Phase 2-3. |
| **Phase 5** | Problem 6 (Autosave) | Builds on atomic writer and dirty tracking. Lower urgency because crash recovery already provides basic protection. |
| **Phase 6** | Problem 7 (Crash Recovery Hardening) | Polish pass on existing infrastructure using the new atomic writer. |

---

## File Manifest (All Files Touched)

### New Files
| File | Purpose |
|------|---------|
| `src/Tools/OdyTools/Utils/AtomicFileWriter.cs` | Write-to-temp-verify-replace utility |
| `src/Tools/OdyTools/Utils/BackupRotator.cs` | FIFO generational backup rotation |
| `src/Tools/OdyTools/Utils/AutosaveService.cs` | Per-editor timer-based autosave |
| `src/Tools/OdyTools/Editors/DLG/DLGValidator.cs` | Pre-save DLG structural validation engine |
| `src/Tools/OdyTools/Editors/DLG/DLGValidationResult.cs` | Validation result data class |

### Modified Files
| File | Changes |
|------|---------|
| `src/Tools/OdyTools/Editors/Editor.cs` | Replace `File.WriteAllBytes` with `AtomicFileWriter.WriteAtomic` (2 sites: lines 656, 759). Initialize autosave service. Check for `.autosave` on load. |
| `src/Tools/OdyTools/Editors/DLG/OdyToolDLG.axaml` | Add `InputGesture="Ctrl+S"` to Save, `InputGesture="Ctrl+Shift+S"` to Save As. Add "Restore from Backup..." menu item. |
| `src/Tools/OdyTools/Editors/DLG/OdyToolDLG.axaml.cs` | Add `MarkDirty()` calls to all property-panel commit handlers. Override `Save()` to run validation before `base.Save()`. Add "Restore from Backup" handler. Wire autosave. |
| `src/Tools/OdyTools/Editors/DLG/DLGActionHistory.cs` | Call `MarkDirty()` (or equivalent callback) after `Apply()`, `Undo()`, `Redo()`. |
| `src/Tools/OdyTools/Editors/DLG/DLGSettings.cs` | Add `AutosaveEnabled`, `AutosaveIntervalMinutes`, `BackupsEnabled`, `MaxBackupCount`, `ValidateOnSave` settings. |
| `src/Tools/OdyTools/Dialogs/FileSaveHandler.cs` | Replace `File.WriteAllBytes` with `AtomicFileWriter.WriteAtomic` (line 42). |
| `src/Tools/OdyTools/Editors/EditorCrashRecoveryService.cs` | Use `AtomicFileWriter` for backup writes. Add stale-file cleanup. Add write verification. |

### Audit-Only (verify Ctrl+S exists, no other changes unless missing)
| File Pattern | Check |
|------|-------|
| `src/Tools/OdyTools/Editors/OdyTool*.axaml` (all ~30 editors) | Verify every `actionSave` MenuItem has `InputGesture="Ctrl+S"` |

---

## Testing Strategy

### Unit Tests (new test files in `src/TSLPatcher.Tests/` or new `OdyTools.Tests` project)

| Test Class | Key Tests |
|------------|-----------|
| `AtomicFileWriterTests` | Write succeeds and target matches expected bytes. Write to locked file retries and succeeds. Write with disk-full simulation preserves original. Temp file cleaned up after success. Cross-volume fallback works. |
| `BackupRotatorTests` | First save creates `.bak`. Fifth save rotates correctly. Sixth save deletes oldest. Restore from `.bak2` restores correct content. `GetAvailableBackups` returns sorted list. |
| `DLGValidatorTests` | Detects orphaned nodes. Detects missing K2 fields (TSL only). Detects empty starters. Passes valid dialog. Does not flag K1 dialog for missing K2 fields. |
| `AutosaveServiceTests` | Creates `.autosave` after interval when dirty. Does not create when clean. Deletes `.autosave` after manual save. Detects stale `.autosave` on load. |
| `DLGDirtyTrackingTests` | Adding a node via action history sets dirty. Editing text sets dirty. Save clears dirty. Undo sets dirty. New clears dirty. Load clears dirty. |

### Integration Tests

| Scenario | Verification |
|----------|-------------|
| Open DLG → edit text → close window | Prompted "Save changes?" |
| Open DLG → edit text → Ctrl+S | File saved, title loses `*`, `.bak` created |
| Open DLG → save 6 times | Only 5 `.bak` files exist |
| Open DLG → edit → kill process → relaunch | Crash recovery dialog shown with correct file |
| Open DLG → save invalid (orphan nodes) | Validation warning shown, user can save anyway or cancel |
| Open DLG → wait 3 minutes with edits | `.autosave` file exists alongside target |

---

## Out of Scope (Explicitly Deferred)

These items from the research prompt are valuable but are separate workstreams:

| Item | Reason for Deferral |
|------|---------------------|
| Node graph canvas / infinite canvas (Nodify.Avalonia) | Massive UI rewrite — separate plan |
| Dual-view (Tree + Graph) architecture | Depends on node graph canvas |
| StrRef/TLK auto-detach UX | Editing workflow improvement, not safety |
| Script compilation integration | Toolchain integration, not safety |
| Camera/animation browser | UX improvement, not safety |
| Cloud backup / Git integration | Future enhancement |
| File Safety settings panel UI | Can be added after the backend works; settings can use `DLGSettings` key-value pairs initially |
| Diff view for backup/recovery comparison | Polish item for Phase 6+ |
| Status bar autosave indicator | UI polish, can follow core autosave implementation |

---

## Success Criteria

1. **Zero 0-byte files**: Interrupting save at any point (process kill, power off) never corrupts the target file
2. **Dirty awareness**: Every edit operation in OdyToolDLG correctly marks the document as modified
3. **Save confirmation on close**: User always warned when closing with unsaved changes
4. **Ctrl+S works**: Keyboard shortcut triggers save in every editor
5. **Backup availability**: After 5+ saves, user can restore any of the last 5 versions
6. **Validation coverage**: Orphaned nodes, empty starters, and missing K2 fields are caught before save
7. **Autosave protection**: Work recoverable even if user forgets to save manually (within 3 minutes)
8. **No performance regression**: Save operations complete in < 200ms for 1000-node dialogs
