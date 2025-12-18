# Architecture Coupling Analysis

**Status**: 🔄 IN PROGRESS
**Date**: 2025-01-16
**Purpose**: Identify and resolve tight coupling between Runtime and Parsing layers to ensure tools (HolocronToolset, HoloPatcher) can use Parsing independently

## Critical Architecture Principle

**Parsing layer MUST be independent of Runtime layer**

- Tools (HolocronToolset, HoloPatcher, NCSDecomp, KotorDiff) depend ONLY on Parsing
- Runtime depends on Parsing (correct direction)
- Parsing MUST NOT depend on Runtime (violation)

## Current Coupling Issues

### ✅ GOOD: No Runtime Dependencies in Parsing

- Verified: Parsing layer has NO `using Andastra.Runtime.*` statements
- Only uses `System.Runtime.*` (framework types, not our Runtime)
- Parsing is properly isolated

### ⚠️ ISSUE: Eclipse Save Format Parsing Missing in Parsing Layer

**Problem**: Eclipse save file formats (.das for Dragon Age, .pcsave for Mass Effect) have NO parsers in Parsing layer

**Current State**:

- ✅ Odyssey (KOTOR): `Parsing/Extract/SaveData/SaveInfo.cs` - Pure GFF parser, no Runtime dependency
- ❌ Eclipse (Dragon Age): Save parsing only in `Runtime/Games/Eclipse/Save/EclipseSaveSerializer.cs` - Uses `Runtime.Core.Save.SaveGameData`
- ❌ Eclipse (Mass Effect): Save parsing only in Runtime layer - Uses Runtime types

**Impact**:

- Tools cannot parse Eclipse save files without Runtime dependency
- HolocronToolset cannot extract/edit Eclipse save metadata
- HoloPatcher cannot analyze Eclipse save formats

**Solution Required**:

1. Create `Parsing/Extract/SaveData/DragonAgeSaveInfo.cs` - Pure binary parser for .das files
2. Create `Parsing/Extract/SaveData/MassEffectSaveInfo.cs` - Pure binary parser for .pcsave files
3. Keep Runtime serializers that convert between Parsing types and Runtime.SaveGameData

### ⚠️ ISSUE: Module/Package Format Parsers May Be Missing

**Problem**: Eclipse module/package formats may need parsers in Parsing layer

**Current State**:

- ✅ Odyssey: Module formats (IFO, LYT, VIS, GIT, ARE) all have parsers in `Parsing/Resource/Formats/`
- ❓ Eclipse: Dragon Age .rim files, Mass Effect packages - Need verification if parsers exist

**Action Required**:

- Verify if Dragon Age .rim format parser exists in Parsing
- Verify if Mass Effect package format parser exists in Parsing
- Create parsers if missing

### ✅ GOOD: Installation and Resource Management

**Current State**:

- `Parsing/Installation/Installation.cs` - Pure file system access, no Runtime dependency
- `Parsing/Extract/` - All extraction utilities are in Parsing
- Tools can use Installation and Extract independently

### ✅ GOOD: File Format Parsers

**Current State**:

- All file format parsers (GFF, ERF, RIM, BIF, KEY, etc.) are in `Parsing/Resource/Formats/`
- No Runtime dependencies
- Tools can parse all file formats independently

## Required Actions

### Priority 1: Eclipse Save Format Parsers in Parsing

**Files to Create**:

1. `Parsing/Extract/SaveData/DragonAgeOriginsSaveInfo.cs`
   - Parse .das save files (binary format)
   - Based on daorigins.exe: SaveGameMessage @ 0x00ae6276
   - Pure data structure, no Runtime types

2. `Parsing/Extract/SaveData/DragonAge2SaveInfo.cs`
   - Parse DA2 save files
   - Based on DragonAge2.exe: SaveGameMessage @ 0x00be37a8
   - Pure data structure, no Runtime types

3. `Parsing/Extract/SaveData/MassEffectSaveInfo.cs`
   - Parse .pcsave files
   - Based on MassEffect.exe: intABioWorldInfoexecBioSaveGame @ 0x11800ca0
   - Pure data structure, no Runtime types

4. `Parsing/Extract/SaveData/MassEffect2SaveInfo.cs`
   - Parse ME2 save files
   - Based on MassEffect2.exe save format
   - Pure data structure, no Runtime types

**Runtime Layer**:

- Keep `EclipseSaveSerializer` classes in Runtime
- Add conversion methods: `SaveGameData ToRuntimeSaveData(DragonAgeOriginsSaveInfo)` etc.
- Runtime serializers use Parsing parsers internally

### Priority 2: Verify Module Format Parsers

**Action**:

- Check if Dragon Age .rim format parser exists
- Check if Mass Effect package format parser exists
- Create if missing

### Priority 3: Document Architecture Boundaries

**Action**:

- Add architecture documentation clarifying:
  - Parsing = File format parsing, pure data structures
  - Runtime = Game logic, uses Parsing for file I/O
  - Tools = Use Parsing directly, never Runtime

## Architecture Layers

```
┌─────────────────────────────────────────────────────────────┐
│                    Tools (HolocronToolset, HoloPatcher)     │
│                    ↓ Uses                                    │
├─────────────────────────────────────────────────────────────┤
│                    Parsing Layer                             │
│                    - File format parsers (GFF, ERF, etc.)    │
│                    - Save file parsers (SaveInfo, etc.)      │
│                    - Installation management                 │
│                    - NO Runtime dependencies                 │
├─────────────────────────────────────────────────────────────┤
│                    Runtime Layer                             │
│                    ↓ Uses                                    │
│                    - Game logic                               │
│                    - Save serializers (convert Parsing→Runtime)│
│                    - Module loaders (use Parsing parsers)    │
└─────────────────────────────────────────────────────────────┘
```

## Verification Checklist

- [x] Parsing has no Runtime dependencies
- [ ] Eclipse save format parsers exist in Parsing
- [ ] Eclipse module format parsers exist in Parsing (if needed)
- [ ] Runtime serializers use Parsing parsers
- [ ] Tools can parse all save formats without Runtime
- [ ] Architecture boundaries documented

## Next Steps

1. Create Eclipse save format parsers in Parsing layer
2. Update Runtime serializers to use Parsing parsers
3. Verify module format parsers exist
4. Update roadmap with coupling analysis findings
5. Add architecture documentation
