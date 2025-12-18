# Namespace Consolidation Plan

**Status**: 🔴 CRITICAL - Inconsistent namespace structure
**Date**: 2025-01-16

## Problem

TWO different namespace structures are being used:

1. **Old Structure** (82 files): `Andastra.Runtime.Kotor.*`
   - Used by: Combat, Components, Dialogue, Data, Systems, Templates, Save, Profiles, Loading, Game, Input
   - Example: `namespace Andastra.Runtime.Kotor.Combat`

2. **New Structure** (3 files): `Andastra.Runtime.Engines.Odyssey.*`
   - Used by: OdysseyEngine.cs, OdysseyK1EngineApi.cs, OdysseyK2EngineApi.cs
   - Example: `namespace Andastra.Runtime.Engines.Odyssey`

## Correct Structure

The `Runtime.Engines.*` structure is correct because:

- Matches Eclipse: `Runtime.Engines.Eclipse`
- Matches Aurora: `Runtime.Engines.Aurora`
- Matches Infinity: `Runtime.Engines.Infinity`
- Consistent with multi-engine architecture

## Action Plan

### Phase 1: Identify Duplicate Files

**Duplicates to DELETE** (Old structure):

1. `EngineApi/K1EngineApi.cs` - DELETE (keep OdysseyK1EngineApi.cs)
2. `EngineApi/K2EngineApi.cs` - DELETE (keep OdysseyK2EngineApi.cs)
3. `Profiles/K1GameProfile.cs` - DELETE (keep OdysseyK1GameProfile.cs)
4. `Profiles/K2GameProfile.cs` - DELETE (keep OdysseyK2GameProfile.cs)

### Phase 2: Rename All Namespaces (82 files)

**Global Replace**:

```
OLD: namespace Andastra.Runtime.Kotor
NEW: namespace Andastra.Runtime.Engines.Odyssey
```

**Subdirectory Structure**:

- `Combat` → `namespace Andastra.Runtime.Engines.Odyssey.Combat`
- `Components` → `namespace Andastra.Runtime.Engines.Odyssey.Components`
- `Dialogue` → `namespace Andastra.Runtime.Engines.Odyssey.Dialogue`
- `Data` → `namespace Andastra.Runtime.Engines.Odyssey.Data`
- `EngineApi` → `namespace Andastra.Runtime.Engines.Odyssey.EngineApi`
- `Game` → `namespace Andastra.Runtime.Engines.Odyssey.Game`
- `Input` → `namespace Andastra.Runtime.Engines.Odyssey.Input`
- `Loading` → `namespace Andastra.Runtime.Engines.Odyssey.Loading`
- `Profiles` → `namespace Andastra.Runtime.Engines.Odyssey.Profiles`
- `Save` → `namespace Andastra.Runtime.Engines.Odyssey.Save`
- `Systems` → `namespace Andastra.Runtime.Engines.Odyssey.Systems`
- `Templates` → `namespace Andastra.Runtime.Engines.Odyssey.Templates`

### Phase 3: Update All Using Statements

**Global Replace in ALL files**:

```
OLD: using Andastra.Runtime.Kotor
NEW: using Andastra.Runtime.Engines.Odyssey
```

### Phase 4: Update Project References

Check and update:

- `.csproj` files
- Assembly names
- Project references

### Phase 5: Verify Compilation

After all changes:

1. Build solution
2. Fix any remaining namespace issues
3. Verify all references resolved

## Execution Strategy

1. ✅ Create this plan document
2. ⏳ Delete duplicate EngineApi files (K1EngineApi, K2EngineApi)
3. ⏳ Delete duplicate Profile files (K1GameProfile, K2GameProfile)
4. ⏳ Global namespace replacement (Runtime.Kotor → Runtime.Engines.Odyssey)
5. ⏳ Global using statement replacement
6. ⏳ Build and fix compilation errors
7. ⏳ Commit changes with proper message

## Expected Impact

- 82 files will have namespace changes
- 4 files will be deleted
- All Runtime.Kotor references will become Runtime.Engines.Odyssey
- Consistent with Eclipse/Aurora/Infinity structure
- Enables proper cross-engine base class extraction
