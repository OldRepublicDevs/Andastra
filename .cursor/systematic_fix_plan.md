# Systematic Fix Plan - Get Project Working

**Status**: 🔴 EXECUTING NOW
**Date**: 2025-01-16
**Goal**: Get project to compilable, working state with proper architecture

## Phase 1: Stop Bleeding (Current State Assessment)

### Files Currently Deleted (Need to Check Git)

- K1EngineApi.cs - WAS DELETED by me (need to restore or confirm Odyssey version has all content)

### Current Issues

1. Namespace inconsistency: 81 files use `Andastra.Runtime.Kotor.*` instead of `Andastra.Runtime.Engines.Odyssey.*`
2. Possible duplicate files (K1/K2 vs OdysseyK1/OdysseyK2)
3. Eclipse/Aurora engines have minimal implementations
4. No base classes in Common for cross-engine support

## Phase 2: Namespace Consolidation (Fix Compilation)

### Step 1: Global Namespace Replace

Replace in ALL 81 files:

- `namespace Andastra.Runtime.Kotor` → `namespace Andastra.Runtime.Engines.Odyssey`
- Keep subdirectories: `.Combat`, `.Components`, `.Dialogue`, etc.

### Step 2: Global Using Statement Replace

Replace in ALL files that import Odyssey:

- `using Andastra.Runtime.Kotor` → `using Andastra.Runtime.Engines.Odyssey`

### Step 3: Test Compilation

- Build solution
- Fix any remaining namespace issues
- Ensure all references resolve

## Phase 3: Consolidate Duplicates (AFTER namespace fix)

### EngineApi Files

1. Keep: `OdysseyK1EngineApi.cs`, `OdysseyK2EngineApi.cs`
2. Compare with: `K1EngineApi.cs` (deleted), `K2EngineApi.cs` (exists)
3. Action: If K2EngineApi has unique content, merge it into OdysseyK2EngineApi
4. Then: Delete K2EngineApi.cs ONLY after merge confirmed

### Profile Files

1. Keep: `OdysseyK1GameProfile.cs`, `OdysseyK2GameProfile.cs`
2. Compare with: `K1GameProfile.cs`, `K2GameProfile.cs`
3. Action: Merge any unique content
4. Then: Delete old files ONLY after merge confirmed

## Phase 4: Create Base Classes in Common

For each Odyssey system, create base class:

1. Combat → `Runtime/Games/Common/Combat/`
2. Dialogue → `Runtime/Games/Common/Dialogue/`
3. Components → `Runtime/Games/Common/Components/`
4. Systems → `Runtime/Games/Common/Systems/`
5. Templates → `Runtime/Games/Common/Templates/`

## Phase 5: Create Eclipse Implementations

For each base class, create Eclipse version:

1. `Runtime/Games/Eclipse/Combat/` - EclipseCombatManager, etc.
2. `Runtime/Games/Eclipse/Dialogue/` - EclipseDialogueManager, etc.
3. Continue for all systems

## Phase 6: Create Aurora Implementations

Same structure as Eclipse

## Phase 7: Create Parsing Layer Parsers

For Eclipse save formats:

1. `Parsing/Extract/SaveData/DragonAgeOriginsSaveInfo.cs`
2. `Parsing/Extract/SaveData/DragonAge2SaveInfo.cs`
3. `Parsing/Extract/SaveData/MassEffectSaveInfo.cs`
4. `Parsing/Extract/SaveData/MassEffect2SaveInfo.cs`

## Phase 8: Update Roadmap

Document all systems with complete status

## Phase 9: Final Compilation and Testing

- Build entire solution
- Fix any remaining issues
- Commit all changes with proper message

## Execution Order (DO NOT SKIP STEPS)

1. ✅ Create this plan
2. ⏳ Assess current git state
3. ⏳ Execute namespace consolidation
4. ⏳ Test compilation
5. ⏳ Handle duplicates (with merge)
6. ⏳ Create base classes
7. ⏳ Create Eclipse implementations
8. ⏳ Create Aurora implementations
9. ⏳ Create Parsing parsers
10. ⏳ Update roadmap
11. ⏳ Final build and commit

**NO MORE DELETIONS until explicit confirmation of merge**
