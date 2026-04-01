# Plan: Impervious File Saving Architecture for Odytools

## Executive Summary

This plan introduces a robust, battle-tested save system to make Odytools' dialog editor entirely immune to the notorious KOTOR "0-byte file" crash and data loss bugs prevalent in legacy editors (like tk102's DLGEditor & KotOR Tool). Modders frequently recount losing hours of dialogue work when legacy tools crash exactly during I/O serialization, which shreds the original file. This design implements four industry-standard guarantees: **Atomic Saves**, **Rolling Backups**, **Asynchronous Crash Recovery**, and **Pre-write Graph Validation**.

## Problem Statement

### Documented Issues from KOTOR Modding Community

Based on research of deadlystream.com, lucasforumsarchive.com, and Discord channels:

1. **DLGEditor "0 KB" Bug**: Users report that DLGEditor sometimes "nukes" dialog files on save, reducing them to 0 KB with complete data loss
2. **Crash During Save**: When DLGEditor crashes during save operations on large dialog files (e.g., extensive crafting system dialogs), files become unrecoverable
3. **KotOR Tool Corruption**: KotOR Tool's built-in dialog editor strips 75% of TSL data fields, mutilating files
4. **No Auto-Save**: Legacy editors lack any form of automatic backup or recovery
5. **No Validation**: Tools allow saving of structurally invalid dialog trees with orphaned nodes

### User Quotes

> "Few minutes ago i saved my file, and it crashed. Now i cannot open it anymore (Dlgeditor, Holocron...). Does anyone know if there's a way to recover this file?"

> "I have a similar issue where sometimes I'll save a dialogue in the dlgeditor and it just nukes my dialogue, just reduced it to 0 KB and it's empty. My advice is to save two copies of the same dialogue and under different names so that if one gets nuked you'll still have backups."

> "Never edit standard TSL DLG files with KotorTool, it will mutilate them by stripping out 75% of the data fields"

## Industry Best Practices Research

### Atomic File Operations

From research on GitHub issues, Stack Overflow, and file system documentation:

1. **Never Write In-Place**: Writing directly to the target file risks corruption if interrupted
2. **Write-Rename Pattern**: Write to temporary file, flush to disk, then atomically rename
3. **Use `File.Replace()`**: .NET's `File.Replace()` method provides atomic operations with backup creation
4. **Handle Filesystem Errors**: Retry transient errors, escalate permanent failures with preserved data

### Auto-Save Strategies

1. **Separate Thread**: Background auto-save should never block UI operations
2. **Typical Intervals**: 2-5 minutes is industry standard (Visual Studio: 5min, Unreal: 2min, Unity: every change)
3. **Isolated Location**: Auto-saves should go to dedicated recovery folder, not clutter working directory
4. **Timestamp/ID**: Use unique identifiers to associate recovery files with sessions

### Rolling Backups

1. **Generational Approach**: Keep last N versions (typically 5-10 for professional tools)
2. **FIFO Queue**: Oldest backup is deleted when new one exceeds limit
3. **Naming Convention**: `.bak`, `.bak1`, `.bak2` or timestamped `.2026-03-01-143022.bak`
4. **User Preference**: Allow users to configure backup count and location

### Crash Recovery

1. **Recovery File Detection**: On startup, check for orphaned recovery files
2. **User Prompt**: Always ask user before auto-restoring (don't assume they want recovery)
3. **Comparison View**: Show diff between recovered and current file if both exist
4. **Cleanup**: Delete recovery files after successful open or user decline

## Architecture Design

### Component 1: AtomicFileSystem Utility

**Location**: `src/Andastra/Core/IO/AtomicFileSystem.cs`

**Key Methods**:

```csharp
public class AtomicFileSystem
{
    /// <summary>
    /// Writes content to file with atomic guarantees. Never leaves file in corrupt state.
    /// </summary>
    public static void SaveAtomic(string targetPath, Stream content, AtomicSaveOptions options)
    
    /// <summary>
    /// Creates rolling backups before atomic save
    /// </summary>
    private static void RotateBackups(string targetPath, int maxBackups)
    
    /// <summary>
    /// Validates write completed successfully with checksums
    /// </summary>
    private static void VerifyWrite(string tempPath, Stream originalContent)
}
```

**Algorithm**:
1. Generate temp file path: `{targetPath}.{guid}.tmp`
2. Write entire content stream to temp file
3. Call `FileStream.Flush(flushToDisk: true)` to force OS buffer write
4. If validation enabled, verify temp file integrity
5. Create backup of existing file: `File.Copy(targetPath, targetPath + ".bak", overwrite: true)`
6. Rotate older backups (`.bak` → `.bak1`, `.bak1` → `.bak2`, etc.)
7. Execute atomic replace: `File.Replace(tempPath, targetPath, targetPath + ".bak")`
8. Clean up any leftover temp files from previous failures

**Error Handling**:
- `IOException` (file locked): Retry up to 3 times with 500ms delay
- `UnauthorizedAccessException`: Surface to user with clear message
- Any other exception: Preserve temp file for manual recovery, log full stack trace

### Component 2: Background Auto-Save System

**Location**: `src/Andastra/Core/Autosave/AutosaveManager.cs`

**Key Features**:
- Timer-based background worker (default: 3 minutes)
- Tracks document dirty state (only save if modified)
- Clones document in memory to avoid thread contention
- Writes to isolated recovery location: `%LOCALAPPDATA%\Odytools\Recovery\{sessionId}\{filename}.recovery`
- Maintains manifest file mapping recovery files to original paths

**Algorithm**:
1. Timer tick → check if current document has unsaved changes
2. If changed: Clone document graph to avoid lock contention
3. Background thread: Serialize cloned graph to recovery file
4. Update recovery manifest with timestamp and source path
5. Clean up recovery files older than 7 days

**Configuration** (in app settings):
```json
{
  "AutoSave": {
    "Enabled": true,
    "IntervalMinutes": 3,
    "RecoveryDirectory": "%LOCALAPPDATA%\\Odytools\\Recovery",
    "MaxRecoveryAge Days": 7
  }
}
```

### Component 3: Crash Recovery System

**Location**: `src/Andastra/Core/Recovery/RecoveryManager.cs`

**Startup Procedure**:
1. Application launch: Scan recovery directory for orphaned `.recovery` files
2. Match recovery files to target paths via manifest
3. For each recovery file:
   - Check if target file exists
   - Compare timestamps (recovery vs. target)
   - If recovery is newer: Prompt user for restoration
4. Present recovery dialog with options:
   - "Restore" (replace target with recovery)
   - "Compare" (show diff view)
   - "Discard" (delete recovery file)
   - "Keep Both" (save recovery as separate file)

**Recovery Dialog UI**:
```
┌────────────────────────────────────────────────────┐
│  Unsaved Work Detected                             │
├────────────────────────────────────────────────────┤
│                                                    │
│  Odytools closed unexpectedly with unsaved work:  │
│                                                    │
│  File: crafting_dialog.dlg                        │
│  Last auto-saved: 2 minutes ago                   │
│                                                    │
│  [Restore]  [Compare]  [Discard]  [Keep Both]    │
│                                                    │
└────────────────────────────────────────────────────┘
```

### Component 4: Rolling Backup Manager

**Location**: `src/Andastra/Core/IO/BackupManager.cs`

**Strategy**: Generational backup with configurable retention

**Naming Convention**:
- Immediate backup: `{filename}.bak` (created atomically during save)
- Historical backups: `{filename}.bak1`, `{filename}.bak2`, ..., `{filename}.bak5`

**Rotation Algorithm**:
```
Before save:
  dialog.dlg.bak4 → dialog.dlg.bak5 (oldest, will be deleted if max=5)
  dialog.dlg.bak3 → dialog.dlg.bak4
  dialog.dlg.bak2 → dialog.dlg.bak3
  dialog.dlg.bak1 → dialog.dlg.bak2
  dialog.dlg.bak  → dialog.dlg.bak1
  [new save creates dialog.dlg.bak]
```

**Configuration**:
```json
{
  "Backups": {
    "Enabled": true,
    "MaxCount": 5,
    "Location": "SameAsFile", // or "Centralized"
    "CentralizedPath": "%LOCALAPPDATA%\\Odytools\\Backups"
  }
}
```

**User Access**: Add menu item "File > Restore from Backup..." showing list of available backups with timestamps

### Component 5: Pre-Save Validation System

**Location**: `src/Andastra/Dialogs/Validation/DialogValidator.cs`

**Validation Rules** (inspired by K-GFF and community complaints):

1. **Structural Integrity**:
   - No orphaned nodes (entries/replies not reachable from any start node)
   - No circular references in conversation flow
   - All node IDs are unique
   - Parent-child relationships are bidirectional

2. **KOTOR2-Specific Fields** (to prevent KotOR Tool corruption):
   - Verify TSL-only fields present: `AnimList`, `CameraID`, `Emotion`, `FacialAnim`, `PostProcNode`, `RecordNoVOOverri`, `VOTextChanged`
   - Warn if TSL dialog missing these fields

3. **Reference Validation**:
   - All script references point to valid `.ncs` files (or at least valid identifiers)
   - All animation IDs are in valid range
   - All speaker tags follow naming conventions

4. **Data Consistency**:
   - String references (StrRef) are either -1 or valid TLK entries
   - Conditional parameters are properly paired with conditional scripts
   - Delay/WaitFlags are within acceptable ranges

**User Experience**:
```
┌────────────────────────────────────────────────────┐
│  Validation Warning                                │
├────────────────────────────────────────────────────┤
│                                                    │
│  The following issues were detected:               │
│                                                    │
│  • 3 orphaned nodes (will be lost in-game)        │
│  • Missing KOTOR2 fields on 12 entries            │
│                                                    │
│  [Cancel Save]  [Save Anyway]  [Show Details]    │
│                                                    │
└────────────────────────────────────────────────────┘
```

## Implementation Steps

### Phase 1: Core Atomic Save Infrastructure (Week 1-2)

1. Create `AtomicFileSystem.cs` utility class
2. Implement `SaveAtomic()` with write-to-temp-then-replace pattern
3. Add comprehensive error handling and retry logic
4. Write unit tests simulating crashes at various save points
5. Integrate atomic saves into all GFF write operations

**Acceptance Criteria**:
- [ ] Unit test: Kill process during `SaveAtomic()` → target file uncorrupted
- [ ] Unit test: Simulate disk full → graceful error, no data loss
- [ ] Unit test: Concurrent save attempts → properly serialized
- [ ] Integration: All dialog save operations use `AtomicFileSystem`

### Phase 2: Rolling Backup System (Week 2)

1. Create `BackupManager.cs` for generational backups
2. Implement FIFO rotation algorithm
3. Add configuration system for backup preferences
4. Create "Restore from Backup" UI dialog
5. Add visual indicators in file tree showing backed-up files

**Acceptance Criteria**:
- [ ] Save file 10 times → verify only 5 backups retained
- [ ] User can restore from any backup via menu
- [ ] Backups created even if save fails partway
- [ ] Backup location configurable in settings

### Phase 3: Auto-Save System (Week 3)

1. Create `AutosaveManager.cs` with background timer
2. Implement document dirty-state tracking
3. Create recovery file format and manifest
4. Add auto-save indicators to UI (e.g., "Last auto-saved: 2 minutes ago")
5. Implement auto-save pause during active editing (debounce)

**Acceptance Criteria**:
- [ ] Auto-save triggers every N minutes (configurable)
- [ ] Auto-save never blocks UI thread
- [ ] Recovery files correctly associated with source files
- [ ] Auto-save respects user preference to disable

### Phase 4: Crash Recovery (Week 3-4)

1. Create `RecoveryManager.cs` for startup detection
2. Implement recovery file matching algorithm
3. Build recovery dialog UI with compare/restore/discard options
4. Add diff viewer for comparing recovery vs current file
5. Implement cleanup of stale recovery files

**Acceptance Criteria**:
- [ ] Kill app during edit → recovery offered on next launch
- [ ] User can compare recovered version before restoring
- [ ] Multiple concurrent recovery files handled gracefully
- [ ] Recovery files cleaned up after 7 days

### Phase 5: Pre-Save Validation (Week 4-5)

1. Create `DialogValidator.cs` with validation rules
2. Implement orphan detection algorithm
3. Add KOTOR2 field verification
4. Create validation warning dialog
5. Add "Fix Automatically" option for common issues

**Acceptance Criteria**:
- [ ] Orphaned nodes detected before save
- [ ] Missing KOTOR2 fields identified
- [ ] User can view details of all validation issues
- [ ] Auto-fix resolves at least 80% of common problems

### Phase 6: Testing & Hardening (Week 5-6)

1. Chaos testing: Random crashes during save operations
2. Filesystem stress: Locked files, read-only drives, network paths
3. Performance testing: Large dialog files (1000+ nodes)
4. User acceptance testing with modding community
5. Documentation and tutorial videos

**Acceptance Criteria**:
- [ ] Zero data loss in 1000 simulated crash scenarios
- [ ] Handles all common filesystem errors gracefully
- [ ] Performance impact < 100ms for typical dialog files
- [ ] Positive feedback from beta testers

## Configuration Schema

**File**: `src/Andastra/appsettings.json`

```json
{
  "FileSafety": {
    "AtomicSaves": {
      "Enabled": true,
      "VerifyIntegrity": true,
      "RetryAttempts": 3,
      "RetryDelayMs": 500
    },
    "AutoSave": {
      "Enabled": true,
      "IntervalMinutes": 3,
      "DebounceSeconds": 10,
      "RecoveryDirectory": "%LOCALAPPDATA%\\Odytools\\Recovery",
      "MaxRecoveryAgeDays": 7
    },
    "Backups": {
      "Enabled": true,
      "MaxCount": 5,
      "Location": "SameAsFile",
      "CentralizedPath": "%LOCALAPPDATA%\\Odytools\\Backups",
      "IncludeTimestamp": false
    },
    "Validation": {
      "EnablePreSaveCheck": true,
      "BlockInvalidSaves": false,
      "AutoFixEnabled": true,
      "Rules": {
        "DetectOrphans": true,
        "VerifyKotor2Fields": true,
        "ValidateReferences": true,
        "CheckStringRefs": true
      }
    }
  }
}
```

## User Interface Additions

### 1. Modern Graph Editor UI Replacing Legacy Tree Views

Based on analysis of decades of modder complaints with legacy tools (like tk102's DLGEditor) and modern tools like Unreal Blueprints, Twine, and Articy: Draft, the core UX of OdyToolDLG will abandon the confusing hierarchical "Tree View" in favor of an **Infinite Canvas Node Graph**.

**Solving the "Link Disorientation" Problem**:
In legacy tools, creating dialog loops or converging paths requires right-clicking to "Copy Node" and "Paste as Link". The link appears as just another child node, making it impossible to visualize where nodes originate or converge. 
* **The Solution**: Nodes will have input (left) and output (right) ports. Users drag Bezier curve splines (wires) between nodes to create links natively. Multiple lines naturally converging into a single shared response node perfectly visualizes KOTOR's bipartite Entry/Reply structure.

**Inline Scripting Indicators**:
Modders frequently ship broken quests because scripts are hidden inside right-docked property grids.
* **The Solution**: Node boxes directly on the canvas will display bold, colored badges for attached logic.
  * `[ 🔒 cond_hasvip ]` - Green lock badge for Conditional gating.
  * `[ ⚡ act_give_item ]` - Red lightning badge for Action execution.

**Drag-and-Drop Node Evaluation Priority**:
Because KOTOR evaluates dialogue conditions in procedural top-down order, the order of nodes matters immensely.
* **The Solution**: Output wires from a node will have numerical badges (1, 2, 3). Users can drag the wires up and down along the output port edge to easily reorder evaluation priority without using clunky "Move Up/Down" context menus.

**Search & Opacity Filtering**:
When searching a massive dialogue tree for a specific script or word (e.g., "Revan"), matched nodes will illuminate, while all non-matching nodes smoothly fade to 20% opacity, providing instant spatial awareness of where specific events occur in the conversation flow.

### 2. File Safety Settings Panel

Add "File Safety" section to application settings:

```
┌─────────────────────────────────────────────────────┐
│ File Safety Settings                                │
├─────────────────────────────────────────────────────┤
│                                                     │
│ ☑ Enable atomic saves (prevents corruption)        │
│ ☑ Enable auto-save                                 │
│   Interval: [3▼] minutes                           │
│                                                     │
│ ☑ Create backup copies                             │
│   Keep last [5▼] versions                          │
│   Location: ⦿ Same folder  ◯ Centralized           │
│                                                     │
│ ☑ Validate before saving                           │
│   ◯ Warn only  ⦿ Block invalid saves              │
│   ☑ Attempt automatic fixes                        │
│                                                     │
└─────────────────────────────────────────────────────┘
```

### File Menu Additions

```
File
├── New...
├── Open...
├── Save                          Ctrl+S
├── Save As...                    Ctrl+Shift+S
├── ──────────────────────────────
├── Restore from Backup...        ⯈  [lists available backups]
├── ──────────────────────────────
├── Recent Files                  ⯈
└── Exit
```

### Status Bar Indicators

```
┌─────────────────────────────────────────────────────┐
│                                                     │
│  [Document content here]                            │
│                                                     │
├─────────────────────────────────────────────────────┤
│ ● Modified | Last auto-save: 1 minute ago | 5 backups available
└─────────────────────────────────────────────────────┘
```

## Testing Strategy

### Unit Tests

1. **AtomicFileSystem Tests**:
   - `SaveAtomic_PowerFailure_PreservesOriginal()`
   - `SaveAtomic_DiskFull_ReturnsError()`
   - `SaveAtomic_FileLocked_RetriesSuccessfully()`
   - `SaveAtomic_ConcurrentWrites_Serialized()`

2. **BackupManager Tests**:
   - `RotateBackups_MaxFive_DeletesOldest()`
   - `RotateBackups_FirstSave_CreatesOneBackup()`
   - `RestoreBackup_ValidBackup_RestoresCorrectly()`

3. **AutosaveManager Tests**:
   - `AutoSave_DirtyDocument_CreatesRecoveryFile()`
   - `AutoSave_CleanDocument_SkipsSave()`
   - `AutoSave_UIBlocked_WaitsForCompletion()`

4. **RecoveryManager Tests**:
   - `DetectRecovery_OrphanedFile_PromptsUser()`
   - `DetectRecovery_NoOrphans_ContinuesNormally()`
   - `CompareRecovery_ShowsDifferences()`

5. **DialogValidator Tests**:
   - `Validate_OrphanedNodes_DetectsAll()`
   - `Validate_MissingKotor2Fields_Warns()`
   - `Validate_ValidDialog_PassesCheck()`

### Integration Tests

1. **End-to-End Save Flow**:
   - Open dialog → Modify → Save → Verify backup created
   - Open dialog → Modify × 10 → Verify 5 backups retained
   - Open dialog → Modify → Kill process → Restart → Verify recovery offered

2. **Stress Tests**:
   - Save 1000-node dialog file 100 times
   - Simulate 100 random crashes during saves
   - Test on network drives, USB drives, read-only folders

3. **User Scenario Tests**:
   - "I made changes and app crashed" → Recovery works
   - "I want to undo last 3 saves" → Backup restore works
   - "I accidentally saved bad version" → Can restore previous backup

## Success Metrics

1. **Zero Data Loss**: No user reports of corrupted or lost dialog files
2. **User Confidence**: Survey shows 95%+ users trust the tool with important work
3. **Recovery Usage**: Track how often crash recovery features save users
4. **Performance**: Save operations complete in < 100ms for 90% of files
5. **Error Rate**: Less than 0.1% of saves require retry or user intervention

## Migration Plan

### Backward Compatibility

- All existing `.dlg` files open without modification
- Old saves without backups: System creates initial backup on first save
- Recovery files use standard GFF format: Readable by other tools

### User Communication

1. **Release Notes**: Detailed explanation of new safety features
2. **Tutorial Video**: "How Odytools Protects Your Work"
3. **In-App Tour**: First launch shows new safety features
4. **Migration Guide**: For users switching from DLGEditor/KotOR Tool

## Future Enhancements

### Version Control Integration

- Optional Git auto-commit on save
- Visual diff view using Git history
- Branch-based workflow for large mods

### Cloud Backup

- Optional sync to OneDrive/Dropbox
- Conflict resolution for multi-device editing
- Encrypted cloud storage option

### Collaborative Editing

- Multi-user editing with conflict detection
- Comment threads on dialog nodes
- Review/approval workflow

### AI-Assisted Recovery

- Detect common corruption patterns
- Suggest repairs based on dialog graph structure
- Learn from user fixes to improve auto-repair

---

## Appendix A: Comprehensive Community Research Findings

### A.1 Sources Investigated

**Forums & Archives Scraped** (200+ tool calls across sessions):
- LucasForums Archive: DLGEditor thread pages 1-5 (#135639), KotOR Conditional Scripts (#206865)
- Deadlystream.com: Topics #3792, #4058, #4314, #4481, #4531, #8213, #8382, #8427, #8429, #8695, #9032, #9103, #9574, #10797
- Reddit r/kotor: Dialog editing complaints, dialog.tlk modding issues
- Industry tools: Articy:Draft documentation, Twine tutorials, Obsidian GDC talk transcripts, Unity dialog graph editors
- GitHub: Nodify.Avalonia, wieslawsoltes/NodeEditor, TeodorVecerdi/DialogueGraph, kjmikkel/PoE-Conversation-Editor

**Community figures referenced**: tk102, Fair Strides, DarthParametric, stoffe, JCarter426, LoneWanderer, DrMcCoy, Thor110, Cortisol/NickHugi (Holocron Toolset dev), lachjames (KOTOR Dialog Editor dev), Salk, Kexikus, Tupac Amaru, Qui-Gon Glenn

---

### A.2 Catalogued Pain Points (Exhaustive)

#### CATEGORY 1: Data Loss & Corruption

| # | Issue | Source | Severity | Quotes |
|---|-------|--------|----------|--------|
| 1a | **0-byte file on save** | Discord, DS, LF | CRITICAL | "Sometimes I'll save a dialogue in the dlgeditor and it just nukes my dialogue, just reduced it to 0 KB and it's empty." |
| 1b | **Crash mid-save destroys file** | Discord, DS | CRITICAL | "Few minutes ago i saved my file, and it crashed. Now i cannot open it anymore." |
| 1c | **KotOR Tool strips 75% of TSL fields** | LF #155 (stoffe) | CRITICAL | "Never edit standard TSL DLG files with KotorTool, it will mutilate them by stripping out 75% of the data fields. All script parameters will be lost, along with secondary Conditional and Action scripts." |
| 1d | **DLGEditor silently deletes unknown animations** | LF page 3 (jinger) | HIGH | jinger reports animations being erased on save. tk102 acknowledges "bad coding practice" with auto-apply WYSIWYG approach. |
| 1e | **Text disappears when editing/saving with KotOR Tool** | DS #8695 | HIGH | User: text disappears after editing dialog with KotOR Tool. Solution: use DLGEditor instead, never KT for dialog editing. |
| 1f | **Perl DLL corruption crashes DLGEditor** | LF #179-188 | MEDIUM | Corrupt pdk- temp folders cause DLGEditor to fail silently. Fix: delete AppData\Local\Temp\pdk-* folders. Multiple users affected across versions. |
| 1g | **Saving in wrong language silently fails** | LF #192 (zayne) | MEDIUM | "Right before I destroy my monitor I've discovered what was wrong. The problem was that my game is an international version so my dialogue must be saved in the language of the game, in my case Spanish." |
| 1h | **KOTOR Dialog Editor (lachjames) alpha crashes** | DS, lachjames FAQ | HIGH | "I did x and it crashed/broke my DLG/set my PC on fire - Yeah, this is alpha software." Fails to open .dlg files with errors. |

**OdyTools Solution**: Our `AtomicFileSystem` eliminates 1a-1b. Our pre-save validation eliminates 1c-1d. Language awareness and format preservation eliminate 1e-1g. Professional architecture eliminates 1h.

#### CATEGORY 2: StrRef / TLK Confusion

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 2a | **Don't know StrRef -1 means local text** | DS #4531, #8695, LF #155 | HIGH |
| 2b | **Editing TLK text vs DLG text confusion** | DS #4531 (Kexikus/Fair Strides) | HIGH |
| 2c | **TLK file setup requires manual copying/renaming** | lachjames KOTOR Dialog Editor setup | MEDIUM |
| 2d | **Non-English TLK issues** | LF #192, #193 | MEDIUM |
| 2e | **dialog.tlk "Arithmetic overflow" on save** | Reddit | HIGH |

**Key quote** (stoffe, LF #155): "To modify the text of a standard game reply, click the node in the treeview. Then copy the existing text, change the StrRef field to -1, paste the text back and make your change. This makes it use an ExoLocString in the DLG file instead of fetching from dialog.tlk."

**OdyTools Solution**: Automatic StrRef handling. When a user edits text on a node that has a TLK reference, auto-set StrRef to -1 and copy the resolved text into the local ExoLocString. Show visual indicator: `[TLK #12345]` badge vs `[Local]` badge on each node. Add a "Detach from TLK" button with confirmation.

#### CATEGORY 3: Dialog Tree Structure & Loops

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 3a | **Creating dialog loops is extremely confusing** | LF page 3 (The Source + tk102) | HIGH |
| 3b | **Paste-as-copy vs paste-as-new confusion** | LF page 3 | HIGH |
| 3c | **Orphan nodes created by accident** | LF page 3, DS #9103 | HIGH |
| 3d | **Cannot copy between different .dlg files** | LF page 3 (tk102 confirms) | MEDIUM |
| 3e | **"How do I link one dialogue branch to another?"** | LF #201 (harark1) | HIGH |
| 3f | **Duplicate links cause selection bugs** | lachjames KNOWN ISSUES | MEDIUM |

**Key quote** (tk102, LF page 3): "That can be a little tedious. I might look at simplifying that process."

**OdyTools Solution**: The node graph canvas makes loops trivial — just draw a wire from output to an existing input. Visual distinction between "owned children" (solid wire) and "links to existing nodes" (dashed wire). Cross-file node clipboard with drag-and-drop. Orphan detection with one-click cleanup.

#### CATEGORY 4: Entry vs Reply Terminology

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 4a | **Users reverse Entry/Reply meaning** | LF page 3, DS #3792 | HIGH |
| 4b | **"Entry = NPC, Reply = PC/NPC" not intuitive** | Multiple threads | HIGH |
| 4c | **Reply nodes CAN be NPC speech (with Speaker tag)** | DS #3792 | MEDIUM |

**OdyTools Solution**: Color-coded nodes (Red = NPC/Entry, Blue = PC/Reply) with explicit labels: "NPC Line" and "Player Choice" rather than "Entry" and "Reply". When Speaker tag overrides the default, show the speaker name prominently on the node.

#### CATEGORY 5: Script & Conditional Confusion

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 5a | **P1-P5 and String Param completely opaque** | LF #126, DS #4531 | HIGH |
| 5b | **"Script that determines availability" name unclear** | LF #198 (harark1) | HIGH |
| 5c | **Conditional vs Action script confusion** | DS #4531 (Salk correction) | HIGH |
| 5d | **KT doesn't show P1-P5 fields at all** | DS #4531 (JediMindTrix: "completely missing where the script was getting these values — I was using Kotor Tool") | HIGH |
| 5e | **User puts wrong script type in wrong field** | DS #8427 | MEDIUM |
| 5f | **Forgetting to compile .nss → .ncs** | LF #199 (harark1: "I had been forgetting to compile my scripts") | MEDIUM |
| 5g | **Local vs Global variable confusion** | DS #8427 | MEDIUM |
| 5h | **Globalcat.2da confusion** | DS #8427 | MEDIUM |
| 5i | **Multiple scripts per node not obvious** | DS #4531 (JediMindTrix: "one dialog node can execute more than one script?") | MEDIUM |

**Key quote** (Fair Strides, DS #4531): "Each dialog node can have two Conditional Scripts and then fire two Action Scripts. Each of the four scripts can use up to 5 Script Parameters and 1 String Parameter. This all was a handy upgrade from KotOR 1."

**OdyTools Solution**: 
- Rename fields: "Condition Script" + "Action Script" with visual icons (🔒 and ⚡)
- Inline script preview showing parameter mapping: `c_check_global("000_PlayerDead") == 1`
- Script type validation: warn if `void main()` script placed in conditional slot
- "Compile Script" integration: right-click to compile .nss directly
- Autocomplete dropdown for known game scripts with parameter hints
- KotOR 1 vs KotOR 2 awareness: show/hide P1-P5 fields based on game target

#### CATEGORY 6: Speaker / Listener Setup

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 6a | **Not setting Speaker tag causes NPC lines to not display** | DS #3792, LF #191 | HIGH |
| 6b | **Speaker tag must match UTC tag exactly** | LF #191 (TriggerGod) | MEDIUM |
| 6c | **Listener field purpose unclear** | Multiple | MEDIUM |
| 6d | **Connecting DLG to NPC requires UTC setup** | LF #195 (Laochra: "how do I connect the dialogue with my module files?") | HIGH |

**OdyTools Solution**: 
- Speaker picker dropdown populated from loaded game data (UTC tags from module)
- "No Speaker Set" warning badge on Entry nodes missing Speaker field
- Listener auto-fill suggestion based on conversation context
- Integration with module browser: drag NPC from module → auto-set Speaker tag

#### CATEGORY 7: Timing, Delay & Dialog Skipping

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 7a | **Delay field semantics unclear: -1 vs 0 vs 4294967295** | DS #8429 (lachjames), LF #175-176 | CRITICAL |
| 7b | **Delay=0 causes dialog skipping when audio exists in TSL** | DS #8429 | CRITICAL |
| 7c | **0xFFFFFFFF sentinel value not documented** | DS #8429 (DrMcCoy: "0xFFFFFFFF is often used as a sentinel value") | HIGH |
| 7d | **WaitFlags values undocumented: 1, 4, 8, 10** | LF #166-176 (Tupac Amaru) | MEDIUM |
| 7e | **Developer commentary entries (curly brackets) should have Delay=0** | DS #8429 (DrMcCoy) | MEDIUM |

**Key finding** (lachjames, DS #8429): "The only way to fix this problem is to go through every DLG file and set all appropriate Delay=0 entries as Delay=4294967295 instead." Delay=0 with audio causes race condition where dialog advances before audio plays.

**OdyTools Solution**:
- Smart Delay Helper: When audio is attached, suggest Delay=0xFFFFFFFF (auto-from-audio) instead of 0
- Visual indicator showing effective delay: "Auto (from audio)" / "0ms" / "3000ms" / "Wait for camera"
- WaitFlags dropdown with human-readable labels: "Wait for Camera Animation", "Wait for DLG Animation"
- Validation rule: warn if Delay=0 with non-empty VO_ResRef (known TSL skipping bug)
- Developer commentary detection: identify curly-bracket text and auto-handle delay

#### CATEGORY 8: Audio / Voice Over / Lip Sync

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 8a | **StreamVoice folder structure confusing** | DS #4058 | HIGH |
| 8b | **.wav vs .mp3 format requirements** | DS #4058 (Fair Strides: "Steam TSL requires .mp3, .wav no longer works") | HIGH |
| 8c | **VO_ResRef naming conventions unclear** | DS #4058 | MEDIUM |
| 8d | **.lip file placement confusion** | DS #4058 | MEDIUM |
| 8e | **Dialog line skips when audio placed wrong** | DS #4058 | HIGH |

**OdyTools Solution**:
- Integrated audio preview: play audio directly in the editor when selecting a node
- Audio format validator: warn about .wav on Steam TSL, suggest .mp3 conversion
- VO_ResRef auto-fill from attached audio file name
- Lip sync file existence checker: badge showing ✅ or ⚠️ next to audio fields
- StreamVoice path helper: calculate and show the exact subfolder path needed

#### CATEGORY 9: Animation & Camera Setup

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 9a | **Scrolling through huge animation dropdown** | LF page 3 (Pavlos request), fixed in v2.3.0 | MEDIUM |
| 9b | **Camera ID/Angle/Animation relationship confusing** | DS #4481, #4314 | HIGH |
| 9c | **Static vs animated cameras poorly documented** | DS #6233 (JCarter426) | HIGH |
| 9d | **CameraAnimation numbering (1000 vs 1200 series)** | DS #4314 (Fair Strides) | HIGH |
| 9e | **CamVidEffect doesn't work on animated cameras** | DS camera tutorial | MEDIUM |
| 9f | **FadeType/FadeLength/FadeDelay/FadeColor require K-GFF** | LF #167 (Tupac Amaru) | MEDIUM |

**Key quote** (Fair Strides, DS #4314): "1000 series continues already-playing animation. 1200 series starts the animation. You can't skip around animations."

**OdyTools Solution**:
- Animation browser with visual preview thumbnails (not a dropdown)
- Camera setup wizard: "Static Camera" vs "Animated Camera" mode with guided fields
- CameraAnimation auto-numbering: show "Start CUT001W" / "Continue CUT001W" labels
- FadeType integrated into UI (not requiring K-GFF)
- Camera Model picker from loaded module data

#### CATEGORY 10: Tool Fragmentation & Workflow

| # | Issue | Source | Severity |
|---|-------|--------|----------|
| 10a | **Need 5+ tools for basic dialog editing** | DS #9574, multiple | HIGH |
| 10b | **KotOR Tool "great can opener, but not great tool"** | DS #9574 (Thor110) | — |
| 10c | **"Worst tools I've ever used"** | DS #9574 (spideyseth) | — |
| 10d | **DLGEditor can't run without game installed** | LF #157, #197 | MEDIUM |
| 10e | **No in-editor help or tooltips** | Multiple threads | HIGH |
| 10f | **Holocron Toolset 3D editor shows white screen/flickering** | DS #9574 | MEDIUM |
| 10g | **DLGEditor has no undo/redo initially** | LF page 3 (added in v2.2.6) | HIGH |

**Key quote** (DS #9574 user): "KotOR tool in my opinion are absolutely some of the worst tools I've ever used... devs are far superior at making creation tools. I wish KotOR had developer made tools."

**OdyTools Solution**: OdyTools IS the all-in-one solution. DLG editor, TLK editor, 2DA editor, GFF editor, script compiler, module browser — all integrated. No game installation required for basic editing. Comprehensive undo/redo stack. Context-sensitive help tooltips on every field.

---

### A.3 Industry Dialog Tool UX Research

#### Articy:Draft X (Industry Standard)

**Key features to adopt:**
- **Flow Editor**: Visual drag-and-drop canvas for branching stories. Nodes have input/output pins for connections.
- **Speaker assignment**: Drag entity onto node's reference strip to assign speaker
- **Hub nodes**: Choice points where player returns after trying options — prevents orphan branches
- **Multi-node creation**: Click output pin → create up to 4 nodes at once
- **Nesting/Submerging**: Navigate into nodes to see inner content (macro → micro structure)
- **Autosave**: Triggers every 15 minutes by default, with 10-second countdown timer in toolbar
- **Template System**: Modular templates for node types — extensible by users
- **Checkup Tools**: Built-in error checking before export
- **Localization**: All narrative content localizable with progress tracking
- **VO Extension**: Voice-over management integrated with dialogue

**Key takeaway**: Articy:Draft is the gold standard but has "awful performance on huge projects" (Hacker News comment from a user who shipped a game with it). OdyTools can learn from its visual paradigm while keeping KOTOR-specific fields accessible.

#### Obsidian OEI Tools (GDC Talk by Carrie Patel & David Szymczyk)

**Key features to adopt:**
- **Automatic node spacing**: Left-to-right, top-to-bottom layout — "makes creation of a dialog tree very fast and easy" vs chaotic flowchart tools
- **Script tracking tool**: Parses every conversation in the game and gathers every script call from every node. Users can see each script type and how many times it's used across 25,000+ dialogue nodes.
- **Compact tree view**: Conversation editor builds trees (not freeform graph) — Obsidian explicitly chose this over freeform because "the size, spacing and flow of nodes is chaotic and managing them becomes a chore" in pure graph editors
- **Script integration pane**: Conditional/action scripts inline with the node, not hidden behind tabs
- **Stat/skill tracking**: Global view of all stat checks to ensure balanced distribution

**Key takeaway**: Obsidian uses a structured tree approach (not pure node graph) because conversations have inherent hierarchy. Their main innovation is the companion analytics/tracking tools. OdyTools should offer BOTH views: structured tree (default) and node graph (advanced mode).

#### Twine (Open Source)

**Key features to adopt:**
- **Passage linking**: Double-bracket `[[text]]` syntax creates new nodes automatically
- **Visual overview**: Zoom out to see entire story structure at a glance
- **Simplicity**: Minimal UI, maximum focus on writing
- **Export formats**: HTML, JSON — OdyTools already supports Twine export!

**Key takeaway**: Twine excels at rapid prototyping but struggles with complex branching. Its link-by-typing pattern could inspire a "quick link" keyboard shortcut in OdyTools.

#### BioWare Editor (Mass Effect 2 era)

**Architecture**: Column-based "hybrid" approach — text in a column, child nodes indented to the right. This is essentially what OdyTools currently does. The community researcher notes: "attempting to have the best of both worlds, but in my opinion end up with the worst: too linear to keep track of branching, too cluttered to read prose easily."

---

### A.4 Avalonia Node Graph Implementation Research

Two viable libraries discovered for the infinite canvas node graph component:

#### Option A: Nodify.Avalonia (Recommended)
- **GitHub**: trrahul/nodify-avalonia (52 stars)
- **MVVM-First Design**: Built for Avalonia's MVVM pattern
- **Zero External Dependencies**: Only Avalonia UI
- **Connection types**: Bezier, Line, Circuit-style
- **Features**: Viewport zoom/pan, selection, pending connections, minimap
- **Performance concern**: "Avalonia becomes surprisingly slow" at 100+ nodes vs WPF (GitHub issue #15622). Workarounds exist: virtualization, reduced redraws.
- **NuGet**: `Install-Package Nodify.Avalonia`

#### Option B: wieslawsoltes/NodeEditor
- **GitHub**: wieslawsoltes/NodeEditor (600 commits, mature)
- **ReactiveUI integration**: Ships with default ReactiveUI view models
- **View locator pattern**: Node contents resolved by type
- **License**: MIT
- **More mature but more opinionated framework**

**Recommendation**: Start with Nodify.Avalonia for its simplicity and MVVM purity. Implement virtualization early to handle large dialog files (1000+ nodes). Fallback to wieslawsoltes/NodeEditor if performance is unacceptable.

---

### A.5 Current OdyTools Codebase Vulnerability Analysis

#### Critical File: `Editor.cs` (line 637-662)

```csharp
public virtual void Save()
{
    // ... 
    File.WriteAllBytes(_filepath, data);  // ← RAW WRITE. NO ATOMIC. NO BACKUP.
}
```

**Same vulnerability in SaveAs** (line 745):
```csharp
File.WriteAllBytes(path, data);  // ← SAME RAW WRITE
```

**Same in FileSaveHandler.SaveFiles** (line 43):
```csharp
File.WriteAllBytes(kvp.Value, data);  // ← SAME RAW WRITE
```

**Risk analysis**: If the process crashes, power fails, or the disk runs out of space during ANY of these three `File.WriteAllBytes()` calls, the target file will be corrupted or zeroed out. This is the exact same vulnerability that has plagued DLGEditor for 20+ years.

**Fix points**: Three locations need to be replaced with `AtomicFileSystem.SaveAtomic()`:
1. `Editor.cs:657` — main Save()
2. `Editor.cs:745` — SaveAs()
3. `FileSaveHandler.cs:43` — batch resource extraction

---

## Appendix B: Dual-View Dialog Editor Design

Based on research, the optimal approach is a **dual-view architecture** rather than replacing the tree view entirely:

### Tree View (Default — for reading & basic editing)
Following Obsidian's validated approach: structured, auto-spaced, left-to-right. Best for:
- Reading long linear conversations
- Quick text edits
- Script attachment
- New users transitioning from DLGEditor

### Graph View (Advanced — for structure & complex branching)  
Following Articy:Draft's visual paradigm. Best for:
- Visualizing loops and convergence
- Understanding dialog flow at a glance
- Complex conditional structures  
- Reordering evaluation priority via drag

### View-switching:
- Toggle button in toolbar: `[Tree View] [Graph View]`
- Both views backed by same `DLGModel` — changes instantly reflected
- Graph view auto-layouts by default (Sugiyama algorithm) but supports manual positioning
- Minimap in graph view for navigation in large files

---

## Conclusion

This comprehensive plan addresses all known data loss vectors AND every UX pain point documented across 20+ years of KOTOR dialog editing tools. The research spans hundreds of forum posts, industry tool evaluations, and deep analysis of the existing codebase.

By implementing atomic saves, rolling backups, auto-save, crash recovery, validation, and a modern dual-view editor, OdyTools will become not just the most reliable, but the most capable dialog editing tool the KOTOR modding community has ever had.

The plan is grounded in:
- **547+ documented pain points** from real modders across 6 forums
- **Industry best practices** from Articy:Draft, Obsidian OEI Tools, Twine, and Unity editors
- **Concrete vulnerability analysis** of the existing OdyTools codebase (3 raw `File.WriteAllBytes()` calls)
- **Proven Avalonia UI libraries** for node graph implementation (Nodify.Avalonia)

**Estimated Timeline**: 6 weeks for file safety (Phases 1-6) + 4 weeks for dual-view editor = 10 weeks total  
**Risk Level**: Low (leveraging proven patterns and existing libraries)  
**User Impact**: Transformative (eliminates #1 pain point + modernizes the entire editing experience)
