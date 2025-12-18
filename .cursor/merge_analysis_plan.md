# Merge Analysis Plan - NO DELETIONS

**Status**: 🔴 CRITICAL - Analyze before ANY deletions
**Date**: 2025-01-16

## Rule: NO DELETIONS until 100% merged/migrated

## Step 1: Analyze Differences

### EngineApi Files

1. **K1EngineApi.cs** vs **OdysseyK1EngineApi.cs**
   - Namespace: `Andastra.Runtime.Kotor.EngineApi` vs `Andastra.Runtime.Engines.Odyssey.EngineApi`
   - Check: Line count, implementation differences, which is more complete

2. **K2EngineApi.cs** vs **OdysseyK2EngineApi.cs**
   - Same namespace difference
   - Check: Line count, implementation differences, which is more complete

### Profile Files

3. **K1GameProfile.cs** vs **OdysseyK1GameProfile.cs**
   - Check: Implementation differences

4. **K2GameProfile.cs** vs **OdysseyK2GameProfile.cs**
   - Check: Implementation differences

## Step 2: Determine Canonical Version

For each pair:
- Compare line counts
- Compare implementation completeness
- Check which has more Ghidra references
- Check which has more complete function implementations
- Determine which to keep as canonical

## Step 3: Merge Process

1. **If versions are identical**: Keep newer namespace version, mark old for deletion
2. **If versions differ**: 
   - Merge unique content from both into canonical version
   - Document what was merged
   - Only then mark old version for deletion

## Step 4: Namespace Consolidation

After merging complete:
- Update all `namespace Andastra.Runtime.Kotor` → `namespace Andastra.Runtime.Engines.Odyssey`
- Update all `using Andastra.Runtime.Kotor` → `using Andastra.Runtime.Engines.Odyssey`
- Test compilation
- Commit changes

## Current Status

- [x] K1EngineApi.cs restored as placeholder
- [ ] Analyze K1EngineApi vs OdysseyK1EngineApi
- [ ] Analyze K2EngineApi vs OdysseyK2EngineApi
- [ ] Analyze Profile files
- [ ] Merge implementations
- [ ] Delete old versions (ONLY after merge complete)
- [ ] Namespace consolidation
- [ ] Test compilation

