# Indoor Map Builder Walkability Bug Investigation

## Summary

This document provides an exhaustive explanation of a bug that was discovered and fixed in the Indoor Map Builder, where levels/modules were not walkable despite having correct surface materials.

## The Problem

**Symptoms:**
- Characters could not walk on indoor map levels/modules
- Pathfinding failed (characters could not find routes)
- Height calculation failed (characters would float or sink into the ground)
- Line of sight checks failed
- The walkmesh appeared to have correct surface materials, but nothing worked

**What Users Observed:**
When building a module in the Indoor Map Builder and testing it in the game, characters could not walk on the floor even though the walkmesh had the correct surface materials (like Stone, Dirt, or other walkable materials).

## Root Cause

The bug was caused by the `WalkmeshType` property not being set correctly when processing room walkmeshes in the Indoor Map Builder.

**How It Happened:**

1. When a room is placed in the Indoor Map Builder, the `ProcessBwm` method in `IndoorMap.cs` is called to prepare the walkmesh for use in the final module.

2. The method calls `DeepCopyBwm` to create a copy of the room's base walkmesh. The `DeepCopyBwm` method copies all properties from the original walkmesh, including the `WalkmeshType` property.

3. The original walkmesh might have had `WalkmeshType` set to `PlaceableOrDoor` (PWK/DWK) instead of `AreaModel` (WOK). This can happen when:
   - The walkmesh was loaded from a kit component that was created from a placeable object
   - The walkmesh was loaded from a door walkmesh
   - The walkmesh type was not correctly set when the kit component was created

4. After copying, the walkmesh was transformed (flipped, rotated, translated) but the `WalkmeshType` was not changed.

5. When the walkmesh was converted to a `NavigationMesh` by `BwmToNavigationMeshConverter`, the converter checks the `WalkmeshType` to decide whether to build an AABB tree:

```csharp
if (bwm.WalkmeshType == BWMType.AreaModel)
{
    // Build AABB tree for fast spatial queries
}
```

6. If the `WalkmeshType` was not `AreaModel`, the AABB tree was not built.

**Why This Caused the Problem:**

The AABB (Axis-Aligned Bounding Box) tree is a data structure that organizes triangles into boxes. It is used to quickly find which triangle contains a given point. Without the AABB tree:

- The `FindFaceAt` function cannot efficiently find which triangle contains a point. It would need to check every single triangle, which is too slow for real-time gameplay.
- Pathfinding fails because it cannot determine which triangles are walkable at a given location.
- Height calculation fails because it cannot find the triangle to calculate the height from.
- Line of sight checks fail because they cannot efficiently find triangles to test against.

**Why Surface Materials Appeared Correct:**

The surface materials were correct. Each face had the right material ID (like Stone, Dirt, etc.), and the `IsWalkable` function would correctly identify them as walkable. However, the navigation system could not find the faces to check because the AABB tree was missing.

## The Solution

The fix was implemented in the `ProcessBwm` method in `IndoorMap.cs`:

```csharp
// CRITICAL: Set walkmesh type to AreaModel (WOK) for indoor map walkmeshes.
// This ensures the AABB tree is built when converting to NavigationMesh.
// Without this, BwmToNavigationMeshConverter and BwmToEclipseNavigationMeshConverter
// will skip AABB tree construction (they check bwm.WalkmeshType == BWMType.AreaModel),
// causing pathfinding and spatial queries to fail.
bwm.WalkmeshType = BWMType.AreaModel;
```

**How the Fix Works:**

1. After applying all transformations (flip, rotate, translate), the method explicitly sets `bwm.WalkmeshType = BWMType.AreaModel`.

2. This ensures that when the walkmesh is converted to a `NavigationMesh`, the converter will build the AABB tree.

3. With the AABB tree in place, all navigation features work correctly:
   - `FindFaceAt` can quickly find which triangle contains a point
   - Pathfinding can determine walkable areas
   - Height calculation can find the correct triangle
   - Line of sight checks can efficiently test triangles

**Why This Is Correct:**

Indoor map walkmeshes are always area walkmeshes (WOK files), not placeable or door walkmeshes (PWK/DWK files). Area walkmeshes are used for the ground of entire areas, while placeable/door walkmeshes are used for individual objects. By setting the type to `AreaModel`, we tell the converter that this is an area walkmesh and it should build the AABB tree for efficient spatial queries.

## Technical Details

### Walkmesh Types

- **AreaModel (WOK)**: Used for area walkmeshes. Contains vertices in world coordinates, includes an AABB tree for fast spatial queries, has walkable adjacency information, and perimeter edges for transitions between areas.

- **PlaceableOrDoor (PWK/DWK)**: Used for placeable objects and doors. Contains vertices in local coordinates (relative to the object's position), collision-only (no pathfinding), typically no AABB tree.

### AABB Tree

The AABB tree is a binary tree structure that organizes triangles into bounding boxes. Each node in the tree contains:
- A bounding box (min/max coordinates) that contains all triangles in that subtree
- Child nodes (left and right subtrees)
- Leaf nodes contain a single triangle

The tree is built by recursively splitting triangles along the longest axis until each leaf contains a single triangle. This allows the system to quickly eliminate large groups of triangles that cannot contain a point, reducing the number of triangles that need to be tested.

### Material Preservation

Throughout all transformations (flip, rotate, translate, deep copy), the `Material` property of each face is preserved. This ensures that walkability is maintained even after rooms are flipped, rotated, or moved. The bug was not related to materials - materials were always correct. The bug was related to the missing AABB tree, which prevented the system from finding the faces to check their materials.

## Files Involved

- `src/Tools/HolocronToolset/Data/IndoorMap.cs`: Contains `ProcessBwm` method that applies the fix
- `src/Tools/HolocronToolset/Data/IndoorMap.cs`: Contains `DeepCopyBwm` method that copies walkmesh properties
- `src/Andastra/Runtime/Content/Converters/BwmToNavigationMeshConverter.cs`: Converts BWM to NavigationMesh, checks WalkmeshType to build AABB tree
- `src/Andastra/Runtime/Core/Navigation/NavigationMesh.cs`: Uses AABB tree for spatial queries

## Verification

To verify the fix is working:

1. Build a module in the Indoor Map Builder with multiple rooms
2. Export the module
3. Load the module in the game
4. Characters should be able to walk on all walkable surfaces
5. Pathfinding should work correctly
6. Height calculation should work correctly

## Related Issues

This bug was similar to issues where:
- Walkmeshes appear correct but navigation doesn't work
- Pathfinding fails even though surfaces are walkable
- Characters cannot find routes even on flat surfaces

All of these can be caused by missing AABB trees, which is why setting the `WalkmeshType` correctly is critical.

