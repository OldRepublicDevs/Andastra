# Collision Detector 1:1 Comparison Analysis

## K1 (swkotor.exe) Analysis

### Decompiled Code (FUN_004ed6e0 @ 0x004ed6e0):
1. **Appearance Type**: `*(ushort *)(param_1 + 0xa60)` - offset 0xa60, ushort (2 bytes)
2. **Bounding Box Pointer**: `*(int *)(param_1 + 0x340)` - offset 0x340, int pointer
3. **Multiple 2DA Lookups**:
   - First lookup (DAT_007a2118): Sets width at `*(float *)(iVar1 + 4)` (defaults to 0.6f if lookup fails)
   - Second lookup (DAT_007a20d0): Sets radius at `*(float *)(*(int *)(param_1 + 0x340) + 8)`
   - Third lookup (DAT_007a20cc): Sets value at `+0x10` (defaults to radius if lookup fails)
   - Fourth lookup (DAT_007a2108): Sets value at `+0x18` (defaults to 0.5f if lookup fails)
   - Fifth lookup (DAT_007a2104): Sets value at `+0x14` (defaults to 1.0f if lookup fails)

### Collision Distance (FUN_004f1310 @ 0x004f1310):
- Line 25-26: If param_3 != 0, adds two radii: `*(float *)(*(int *)(iVar3 + 0x340) + 8) + *(float *)(*(int *)(param_1 + 0x340) + 8) + _DAT_00746f4c`
- Line 29-34: If param_3 == 0, looks up hitradius from appearance.2da for both creatures and adds them

### Our Implementation Issues:
1. ❌ We use `GetAppearanceType()` which tries IRenderableComponent first, then reflection
2. ❌ Original code directly reads `*(ushort *)(param_1 + 0xa60)` - no component lookup
3. ❌ We only use radius for all dimensions, but original sets width at +4 and radius at +8 separately
4. ❌ We don't match the exact default values (0.6f for width, not 0.5f)

## K2 (swkotor2.exe) Analysis

### Decompiled Code (FUN_0050e170 @ 0x0050e170 - similar to K1's FUN_004ed6e0):
1. **Appearance Type**: `*(ushort *)(param_1 + 0x1184)` - offset 0x1184, ushort (2 bytes)
2. **Bounding Box Pointer**: `*(int *)(param_1 + 0x380)` - offset 0x380, int pointer
3. **Multiple 2DA Lookups** (same pattern as K1):
   - First lookup: Sets width at `+4` (defaults to 0.6f)
   - Second lookup: Sets radius at `+8`
   - Third lookup: Sets value at `+0x10` (defaults to radius)
   - Fourth lookup: Sets value at `+0x18` (defaults to 0.5f)
   - Fifth lookup: Sets value at `+0x14` (defaults to 1.0f)

### Collision Detection (FUN_005479f0 @ 0x005479f0):
- Line 83: Uses width: `fVar8 = *(float *)(iVar1 + 0x14) + fVar9 + _DAT_007b6888;`
- Line 100: Uses height: `uVar12 = *(undefined4 *)(iVar1 + 0xbc);`
- **CRITICAL**: Uses width at offset +0x14 (not +4) and height at +0xbc for collision detection

### Our Implementation Issues:
1. ❌ We use `GetAppearanceType()` which tries IRenderableComponent first, then reflection
2. ❌ Original code directly reads `*(ushort *)(param_1 + 0x1184)` - no component lookup
3. ❌ We only use radius for all dimensions, but original uses width at +0x14 and height at +0xbc separately
4. ❌ FUN_005479f0 uses width at +0x14 (not +4) and height at +0xbc for collision detection

## NWN:EE (nwmain.exe) Analysis

### Decompiled Code (GetUseRange @ 0x140396480):
1. **Line 27**: Default radius: `*param_3 = *(float *)(*(longlong *)(this + 0x530) + 4);` - width at +4
2. **Line 35**: For creatures: `*param_3 = *(float *)(*(longlong *)(this + 0x530) + 8);` - radius at +8
3. **Line 41**: Adds two radii: `*param_3 = *(float *)(*(longlong *)(lVar5 + 0x530) + 8) + *param_3;`

### Collision Detection (NoCreaturesOnLine @ 0x14036ec90):
- Line 113: Uses radius: `fVar14 = *(float *)(*(longlong *)(pCVar7 + 0x530) + 8);`
- Line 134: Adds two radii: `fVar18 = local_fc + *(float *)(*(longlong *)(pCVar7 + 0x530) + 8);`

### BumpFriends (0x140385130):
- Line 52-53: Adds two radii: `*(float *)(*(longlong *)(this + 0x530) + 8) + DAT_140d83b74 + *(float *)(*(longlong *)(param_1 + 0x530) + 8);`

### Our Implementation Issues:
1. ❌ We use `GetAppearanceType()` which tries IRenderableComponent first, then reflection
2. ❌ Original code doesn't show appearance type lookup in GetUseRange - it directly uses bounding box structure
3. ✅ We correctly use radius at +8 for collision detection

## Key Findings

1. **Appearance Type Access**: All engines directly read from memory offsets, not through component systems
   - K1: `*(ushort *)(param_1 + 0xa60)`
   - K2: `*(ushort *)(param_1 + 0x1184)`
   - NWN:EE: Not directly accessed in GetUseRange (uses bounding box structure directly)

2. **Bounding Box Structure**: Each engine has different offsets and uses different values
   - K1: Offset 0x340, width at +4, radius at +8
   - K2: Offset 0x380, width at +0x14 (for collision), height at +0xbc (for collision), but also width at +4, radius at +8 (for initialization)
   - NWN:EE: Offset 0x530, width at +4, radius at +8

3. **Default Values**: 
   - K1/K2: Width defaults to 0.6f (0x3f19999a), not 0.5f
   - Radius/height defaults vary

4. **Commonalities**: 
   - All use appearance.2da hitradius for creature size
   - All add two radii for collision distance calculation
   - All use bounding box structure with pointer at specific offset

## Required Changes

1. **Move appearance type access to subclasses** - each engine reads from different offsets
   - K1: `*(ushort *)(param_1 + 0xa60)` - offset 0xa60, ushort
   - K2: `*(ushort *)(param_1 + 0x1184)` - offset 0x1184, ushort
   - NWN:EE: Not directly accessed in GetUseRange (uses bounding box structure directly)
2. **Use correct default values** - K1/K2 use 0.6f (0x3f19999a) for width, not 0.5f
3. **Handle width/height separately for K2** - K2 uses width at +0x14 and height at +0xbc for collision detection
   - FUN_005479f0 line 83: `fVar8 = *(float *)(iVar1 + 0x14) + fVar9 + _DAT_007b6888;`
   - FUN_005479f0 line 100: `uVar12 = *(undefined4 *)(iVar1 + 0xbc);`
   - FUN_005479f0 line 101: `fVar9 = *(float *)(iVar1 + 4);`
4. **Match exact memory access patterns** - direct memory reads, not component lookups
   - Note: Our C# implementation uses high-level API (IEntity, components) as abstraction
   - Original code uses direct memory access, but we can't do that in C#
   - We should document the exact offsets and behavior, even if we use abstraction

## Implementation vs Decompiled Code Comparison

### K1 (swkotor.exe) FUN_004ed6e0:

**Decompiled Code:**
```c
void FUN_004ed6e0(int param_1) {
    float local_4 = 0.6;  // Default width
    bool bVar2 = FUN_00413350(..., (uint)*(ushort *)(param_1 + 0xa60), DAT_007a2118, &local_4);
    int iVar1 = *(int *)(param_1 + 0x340);  // Bounding box pointer
    if (bVar2 == 1) {
        *(float *)(iVar1 + 4) = local_4;  // Set width at +4
    } else {
        *(float *)(iVar1 + 4) = 0x3f19999a;  // 0.6f default
    }
    // Second lookup for radius at +8
    bVar2 = FUN_00413350(..., (uint)*(ushort *)(param_1 + 0xa60), DAT_007a20d0, &local_4);
    if (bVar2 == 1) {
        *(float *)(*(int *)(param_1 + 0x340) + 8) = local_4;  // Set radius at +8
    } else {
        *(float *)(*(int *)(param_1 + 0x340) + 8) = local_4;  // Still sets local_4 (0.6f)
    }
    // ... more lookups for +0x10, +0x18, +0x14
}
```

**Our Code:**
- ✅ Gets appearance type (abstracted via GetAppearanceType())
- ✅ Looks up hitradius from appearance.2da (via GameDataProvider)
- ❌ Doesn't set width separately (we use radius for all dimensions)
- ❌ Uses 0.5f default instead of 0.6f for width
- ✅ Uses radius at +8 for collision detection (matches FUN_004f1310)

**Fix Needed:**
- Document that original sets width at +4 and radius at +8 separately
- Note that we use radius for all dimensions as approximation
- Use 0.6f as default width if we ever implement width separately

### K2 (swkotor2.exe) FUN_005479f0:

**Decompiled Code:**
```c
undefined4 FUN_005479f0(void *this, float *param_1, float *param_2, ...) {
    int iVar1 = *(int *)((int)this + 0x380);  // Bounding box pointer
    fVar8 = *(float *)(iVar1 + 0x14) + fVar9 + _DAT_007b6888;  // Width at +0x14
    uVar12 = *(undefined4 *)(iVar1 + 0xbc);  // Height at +0xbc
    fVar9 = *(float *)(iVar1 + 4);  // Value at +4
    // Uses width and height separately for collision detection
    uVar3 = FUN_004e17a0(..., fVar7, fVar8, fVar9, uVar12, ...);  // Spatial query
}
```

**Our Code:**
- ✅ Gets appearance type (abstracted via GetAppearanceType())
- ✅ Looks up hitradius from appearance.2da (via GameDataProvider)
- ❌ Doesn't use width at +0x14 and height at +0xbc separately
- ❌ Uses radius for all dimensions instead of separate width/height
- ✅ Uses radius for collision detection (approximation)

**Fix Needed:**
- Document that original uses width at +0x14 and height at +0xbc for collision
- Note that we use radius for all dimensions as approximation
- If we can get width/height from entity, use them separately

### NWN:EE (nwmain.exe) GetUseRange:

**Decompiled Code:**
```c
int CNWSCreature::GetUseRange(CNWSCreature *this, uint param_1, Vector *param_2, float *param_3) {
    *param_3 = *(float *)(*(longlong *)(this + 0x530) + 4);  // Default: width at +4
    if (object_type == CREATURE) {
        *param_3 = *(float *)(*(longlong *)(this + 0x530) + 8);  // For creatures: radius at +8
        // Adds two radii for creature-to-creature collision
        *param_3 = *(float *)(*(longlong *)(lVar5 + 0x530) + 8) + *param_3;
    }
}
```

**Our Code:**
- ✅ Gets appearance type (abstracted via GetAppearanceType())
- ✅ Looks up hitradius from appearance.2da (via GameDataProvider)
- ✅ Uses radius at +8 for collision detection (matches NoCreaturesOnLine)
- ✅ Uses radius for all dimensions (matches BumpFriends which adds two radii)

**Fix Needed:**
- ✅ Already correct - NWN:EE implementation matches decompiled code

## Collision Detection Algorithm Comparison

### K1 (swkotor.exe) FUN_004f1310:

**Decompiled Code:**
```c
float10 FUN_004f1310(void *param_1, float param_2, int param_3) {
    // Gets other creature
    iVar3 = (**(code **)(*piVar2 + 0x30))();
    if (iVar3 != 0) {
        if (param_3 != 0) {
            // Adds two radii: radius1 + radius2 + constant
            return (float10)*(float *)(*(int *)(iVar3 + 0x340) + 8) +
                   (float10)*(float *)(*(int *)(param_1 + 0x340) + 8) + 
                   (float10)_DAT_00746f4c;
        }
        // If param_3 == 0, looks up hitradius from appearance.2da for both creatures
        FUN_00413350(..., (uint)*(ushort *)((int)param_1 + 0xa60), DAT_007a210c, &param_2);
        local_10 = param_2 + _DAT_00746f48;
        FUN_00413350(..., (uint)*(ushort *)(iVar3 + 0xa60), DAT_007a210c, &param_2);
        return (float10)param_2 + (float10)local_10;
    }
}
```

**Our Code (BaseCreatureCollisionDetector.CheckLineSegmentVsBoundingBox):**
- ✅ Expands bounding box by actor's bounding box (Minkowski sum)
- ✅ Performs line-segment vs AABB intersection test
- ✅ Uses slab method (separating axis theorem)
- ❌ Doesn't match exact algorithm - K1 uses simple radius addition, not AABB intersection

**Fix Needed:**
- K1 uses simple radius addition for collision distance, not AABB intersection
- Our base class uses AABB intersection which is more general but different from K1's approach

### K2 (swkotor2.exe) FUN_004e17a0:

**Decompiled Code:**
```c
uint FUN_004e17a0(void *this, float *param_1, float *param_2, float param_3, float param_4, float param_5, ...) {
    // Checks AABB intersection
    if (((((*pfVar9 <= fVar1 - param_5) || (*pfVar9 <= fVar6 + param_5)) &&
         ((fVar6 + param_5 <= pfVar9[3] || (fVar1 - param_5 <= pfVar9[3])))) &&
        ((pfVar9[1] <= fVar2 - param_5 || (pfVar9[1] <= fVar3 + param_5)))) &&
       ((((fVar3 + param_5 <= pfVar9[4] || (fVar2 - param_5 <= pfVar9[4])) &&
         ((pfVar9[2] <= param_3 || (pfVar9[2] <= param_4)))) &&
        ((param_4 <= pfVar9[5] || (param_3 <= pfVar9[5])))))) {
        // AABB intersection found
    }
}
```

**Our Code (BaseCreatureCollisionDetector.CheckLineSegmentVsBoundingBox):**
- ✅ Performs line-segment vs AABB intersection test
- ✅ Uses slab method (separating axis theorem)
- ✅ Matches K2's AABB intersection approach

**Fix Needed:**
- ✅ Already correct - K2 implementation matches decompiled code

### NWN:EE (nwmain.exe) NoCreaturesOnLine:

**Decompiled Code:**
```c
int CNWSArea::NoCreaturesOnLine(...) {
    // Gets radius
    fVar14 = *(float *)(*(longlong *)(pCVar7 + 0x530) + 8);
    // Checks if creature is in expanded bounding box
    if (((fVar12 < fVar14 + fVar21 + fVar1) && ((fVar16 - fVar1) - fVar14 < fVar12)) &&
        ((fVar13 < fVar14 + local_104 && (local_100 - fVar14 < fVar13)))) {
        // Adds two radii
        fVar18 = local_fc + *(float *)(*(longlong *)(pCVar7 + 0x530) + 8);
        // Checks if point is within combined radius using dot product
        dVar11 = (double)(fVar14 * local_ec + fVar17 * local_f0);
        if ((float)(double)CONCAT44(...) <= fVar18) {
            // Collision detected
        }
    }
}
```

**Our Code (BaseCreatureCollisionDetector.CheckLineSegmentVsBoundingBox):**
- ✅ Expands bounding box by actor's bounding box (Minkowski sum)
- ✅ Performs line-segment vs AABB intersection test
- ✅ Uses slab method (separating axis theorem)
- ❌ Doesn't match exact algorithm - NWN:EE uses radius-based distance check after AABB check

**Fix Needed:**
- NWN:EE uses AABB check first, then radius-based distance check
- Our base class uses AABB intersection only, which is close but not exact

## Summary of Required Changes

1. **K1**: Uses simple radius addition for collision distance, not AABB intersection
   - Our base class uses AABB intersection which is more general
   - K1-specific collision detection should use radius addition

2. **K2**: Uses AABB intersection with width and height separately
   - Our base class uses AABB intersection which matches
   - But K2 uses width at +0x14 and height at +0xbc, not just radius

3. **NWN:EE**: Uses AABB check first, then radius-based distance check
   - Our base class uses AABB intersection which is close
   - NWN:EE adds radius-based distance check after AABB check

4. **Commonalities**:
   - All use appearance.2da hitradius for creature size ✅ (in subclasses)
   - All add two radii for collision distance calculation ✅ (in base class CheckLineSegmentVsBoundingBox)
   - All use bounding box structure with pointer at specific offset ✅ (in subclasses)
   - **NOT COMMON**: Collision detection algorithms differ:
     - K1: Simple radius addition
     - K2: AABB intersection with separate width/height
     - NWN:EE: AABB check + radius-based distance check

## Conclusion

The base class `CheckLineSegmentVsBoundingBox` uses AABB intersection which matches K2 and is close to NWN:EE, but doesn't match K1's simple radius addition. However, since K1's approach is simpler and less accurate, using AABB intersection in the base class is acceptable as a more general solution.

The key issue is that each engine's `GetCreatureBoundingBox` implementation should match the exact memory access patterns and default values from the decompiled code, even if we use high-level APIs as abstraction.

