# Walkmesh and BWM Documentation

## Table of Contents
1. [What is a Walkmesh?](#what-is-a-walkmesh)
2. [What is a BWM File?](#what-is-a-bwm-file)
3. [BWM File Format Structure](#bwm-file-format-structure)
4. [AABB Trees](#aabb-trees)
5. [Surface Materials and Walkability](#surface-materials-and-walkability)
6. [NavigationMesh Implementation](#navigationmesh-implementation)
7. [BWM to NavigationMesh Conversion](#bwm-to-navigationmesh-conversion)
8. [Indoor Map Builder and Kits](#indoor-map-builder-and-kits)
9. [Potential Walkability Bug Analysis](#potential-walkability-bug-analysis)

## What is a Walkmesh?

A walkmesh is a simplified 3D model made of triangles that defines where characters can walk in a game level. It's called a "walkmesh" because it's a mesh (collection of triangles) that defines walkable areas.

### Purpose of Walkmeshes

Walkmeshes serve four main purposes:

1. **Pathfinding**: The A* algorithm uses walkable faces to find paths between two points. Characters can only move along walkable triangles.

2. **Collision Detection**: Characters are prevented from walking through walls, falling through floors, or standing on non-walkable surfaces (like water that's too deep or lava).

3. **Height Calculation**: Determines the ground height (Z coordinate) at any given (X, Y) position. This is used to place characters on the ground and keep them at the correct height.

4. **Line of Sight**: Raycasting determines if two points can see each other, which is used for AI decision-making and combat.

### How Walkmeshes Work

A walkmesh is made of triangles (called "faces"). Each triangle has:
- Three vertices (corner points) with X, Y, Z coordinates
- A surface material ID that determines if the triangle is walkable
- Adjacency information (which triangles share edges)

Characters can only stand on and move across triangles that are marked as "walkable" based on their surface material.

## What is a BWM File?

BWM stands for "BioWare Walkmesh". It's the file format used by BioWare games (Knights of the Old Republic, Neverwinter Nights, Dragon Age, Mass Effect) to store walkmesh data.

### BWM File Types

There are three types of BWM files:

1. **WOK (Area Walkmesh)**: Full walkmesh for an entire area/room
   - Includes AABB tree for spatial acceleration
   - Used for area navigation and pathfinding
   - File extension: `.wok`

2. **PWK (Placeable Walkmesh)**: Walkmesh for a placeable object (door, chest, etc.)
   - No AABB tree (uses brute force checking)
   - Used for collision detection only
   - File extension: `.pwk`

3. **DWK (Door Walkmesh)**: Walkmesh for a door
   - No AABB tree (uses brute force checking)
   - Used for collision detection only
   - File extension: `.dwk`

The `BWMType` enum distinguishes between these:
- `BWMType.AreaModel` (1): WOK file, has AABB tree
- `BWMType.PlaceableOrDoor` (0): PWK/DWK file, no AABB tree

## BWM File Format Structure

The BWM file format consists of:

### Header (136 bytes)
- Signature: "BWM V1.0" (8 bytes)
- Walkmesh type: 0 (PWK/DWK) or 1 (WOK)
- Position: Vector3 (12 bytes) - offset for the walkmesh
- Relative hooks: 2x Vector3 (24 bytes) - connection points relative to origin
- Absolute hooks: 2x Vector3 (24 bytes) - connection points in world coordinates
- Padding/reserved: remaining bytes

### Data Tables
The file contains offset pointers to various data arrays:
- Vertices array: Array of Vector3 (X, Y, Z coordinates)
- Faces array: Array of face data (vertex indices, material, transitions)
- Materials array: Array of surface material IDs (one per face)
- Normals array: Array of Vector3 (face normal vectors)
- Planar distances array: Array of floats (plane equation D value)
- AABB tree nodes: Binary tree structure (only in WOK files)
- Adjacency array: Array of int32 (face/edge pairs, -1 = no neighbor)
- Edges array: Array of edge data (perimeter edges with transitions)
- Perimeters array: Array of int32 (perimeter lengths)

### Face Data Structure
Each face contains:
- Three vertex indices (pointing into the vertices array)
- Surface material ID (determines walkability)
- Three transition indices (Trans1, Trans2, Trans3) for room connections
- Face normal vector (calculated from vertices)
- Planar distance (for plane equation)

### Coordinate System
BWMs use a right-handed coordinate system:
- X: East/West (positive X = East)
- Y: North/South (positive Y = North)
- Z: Up/Down (positive Z = Up)

Face vertices are ordered counter-clockwise when viewed from above (positive Z direction). This ordering determines the face normal (which side is "up").

## AABB Trees

### What is an AABB Tree?

An AABB (Axis-Aligned Bounding Box) tree is a binary tree structure used for spatial acceleration. It speeds up queries like "which face contains this point?" or "which faces does this ray hit?" from O(n) to O(log n) time.

### How AABB Trees Work

An AABB tree is built recursively:

1. **Calculate Bounding Box**: For all faces in the current node, calculate the smallest box that contains all their vertices.

2. **Find Split Axis**: Find the longest dimension (X, Y, or Z) of the bounding box. This becomes the split axis.

3. **Partition Faces**: Split faces into two groups based on their center position along the split axis:
   - Left group: faces with center < split value
   - Right group: faces with center >= split value

4. **Handle Degenerate Cases**: If all faces are on one side, try splitting by median or switch to a different axis.

5. **Recurse**: Recursively build left and right subtrees.

6. **Create Leaf Nodes**: When only one face remains, create a leaf node that stores the face index.

### AABB Tree Structure

Each node contains:
- `BoundsMin`: Vector3 - minimum corner of bounding box
- `BoundsMax`: Vector3 - maximum corner of bounding box
- `FaceIndex`: int - face index if leaf node, -1 if internal node
- `Left`: AabbNode - left child (null if leaf)
- `Right`: AabbNode - right child (null if leaf)

### AABB Tree Traversal

When querying the tree (e.g., finding a face at a point):

1. **Test Bounding Box**: Check if the query point/ray intersects the node's bounding box.
2. **If Leaf Node**: Test the face directly (point-in-triangle test or ray-triangle intersection).
3. **If Internal Node**: Recursively test left and right children.
4. **Early Termination**: If the query doesn't intersect the bounding box, skip the entire subtree.

### Performance

- **Without AABB tree**: O(n) - must check every face
- **With AABB tree**: O(log n) - only checks faces in relevant subtrees
- **Typical speedup**: 10-100x faster for large walkmeshes (1000+ faces)

Only WOK files (AreaModel type) have AABB trees. PWK/DWK files use brute force because they typically have few faces (< 100).

## Surface Materials and Walkability

### What are Surface Materials?

Each face in a walkmesh has a surface material ID (0-30) that determines:
- Whether the face is walkable (characters can stand on it)
- Movement speed multiplier (some materials slow movement)
- Pathfinding cost (some materials are more expensive to traverse)

### Walkable Materials

The following material IDs are walkable (characters can stand on them):

- **1: Dirt** - normal movement speed
- **3: Grass** - normal movement speed
- **4: Stone** - normal movement speed (default for generated walkmeshes)
- **5: Wood** - normal movement speed
- **6: Water (shallow)** - slower movement, 1.5x pathfinding cost
- **9: Carpet** - normal movement speed
- **10: Metal** - normal movement speed
- **11: Puddles** - slower movement, 1.5x pathfinding cost
- **12: Swamp** - slower movement, 1.5x pathfinding cost
- **13: Mud** - slower movement, 1.5x pathfinding cost
- **14: Leaves** - normal movement speed
- **16: BottomlessPit** - walkable but dangerous, 10x pathfinding cost, AI avoids if possible
- **18: Door** - normal movement speed
- **20: Sand** - normal movement speed
- **21: BareBones** - normal movement speed
- **22: StoneBridge** - normal movement speed
- **30: Trigger** - walkable, PyKotor extended material

### Non-Walkable Materials

The following material IDs are NOT walkable (characters cannot stand on them):

- **0: NotDefined/UNDEFINED** - non-walkable, used for undefined surfaces
- **2: Obscuring** - non-walkable, blocks line of sight
- **7: Nonwalk/NON_WALK** - non-walkable, explicitly marked as impassable
- **8: Transparent** - non-walkable, see-through but solid
- **15: Lava** - non-walkable, dangerous
- **17: DeepWater** - non-walkable, characters can't stand in deep water
- **19: Snow/NON_WALK_GRASS** - non-walkable, marked as non-walkable grass

### How Walkability is Determined

Walkability is determined by checking if the material ID is in the `WalkableMaterials` HashSet:

```csharp
public bool IsWalkable(int faceIndex)
{
    if (faceIndex < 0 || faceIndex >= _surfaceMaterials.Length)
    {
        return false;
    }
    int material = _surfaceMaterials[faceIndex];
    return WalkableMaterials.Contains(material);
}
```

### Critical Consistency Requirement

The `WalkableMaterials` set in `NavigationMesh` MUST match `SurfaceMaterialExtensions.WalkableMaterials` exactly. If they differ, the indoor map builder and other tools will have incorrect walkability determination.

Both sets must contain the same material IDs:
- `NavigationMesh.WalkableMaterials` (HashSet<int>)
- `SurfaceMaterialExtensions.WalkableMaterials` (HashSet<SurfaceMaterial>)

Any mismatch will cause bugs where:
- Faces are marked as walkable in one place but non-walkable in another
- Pathfinding fails when it should succeed
- Characters can't walk on surfaces that should be walkable

## NavigationMesh Implementation

The `NavigationMesh` class (in `src/Andastra/Runtime/Core/Navigation/NavigationMesh.cs`) is the runtime representation of a walkmesh. It provides:

### Core Data Structures

- **Vertices**: Array of Vector3 - all unique vertex positions
- **FaceIndices**: Array of int - triplets of vertex indices (3 per face)
- **Adjacency**: Array of int - face/edge pairs encoded as `faceIndex * 3 + edgeIndex`, -1 = no neighbor
- **SurfaceMaterials**: Array of int - material ID per face
- **AabbRoot**: AabbNode - root of AABB tree (null for PWK/DWK)

### Key Methods

1. **FindFaceAt(x, y)**: Finds the face containing a 2D point (X, Y)
   - Uses AABB tree traversal for WOK files
   - Uses brute force for PWK/DWK files
   - Only checks walkable faces by default

2. **ProjectToSurface(x, y)**: Projects a 2D point onto the walkmesh surface, returning the 3D position
   - Finds the face at (X, Y)
   - Calculates Z coordinate using plane equation
   - Returns null if no walkable face found

3. **DetermineZ(x, y)**: Calculates the Z coordinate at a given (X, Y) position
   - Uses plane equation: `Z = -(Ax + By + D) / C` where (A, B, C) is the face normal
   - Handles vertical triangles (C = 0) by returning average Z of vertices

4. **Raycast(origin, direction, maxDistance)**: Performs a raycast against the walkmesh
   - Traverses AABB tree for WOK files
   - Tests ray-triangle intersection for each candidate face
   - Returns the closest hit face and distance

5. **FindPath(start, goal)**: Finds a path between two points using A* algorithm
   - Only considers walkable faces
   - Uses adjacency to move between connected faces
   - Applies material-based pathfinding costs
   - Returns smoothed path (straight-line segments between face centers)

6. **IsWalkable(faceIndex)**: Checks if a face is walkable based on its material
   - Validates face index
   - Checks material ID against WalkableMaterials set
   - Returns true if walkable, false otherwise

### A* Pathfinding Algorithm

The pathfinding algorithm works as follows:

1. **Initialization**: Find start and goal faces using `FindFaceAt`
2. **Search Loop**: 
   - Remove best node from open set (lowest f = g + h)
   - If goal reached, reconstruct path
   - For each neighbor face:
     - Calculate edge cost (distance + material cost)
     - If neighbor not in closed set, add to open set
3. **Path Reconstruction**: Trace back from goal to start using parent pointers
4. **Smoothing**: Simplify path by removing unnecessary waypoints

## BWM to NavigationMesh Conversion

The `BwmToNavigationMeshConverter` class (in `src/Andastra/Runtime/Content/Converters/BwmToNavigationMeshConverter.cs`) converts `BWM` objects to `NavigationMesh` objects.

### Conversion Process

1. **Extract Vertices**: Build unique vertex list from all faces
2. **Build Face Indices**: Create triplets of vertex indices (3 per face)
3. **Convert Materials**: Extract surface material ID from each face
4. **Compute Adjacency**: Build adjacency array from BWM adjacency data
5. **Build AABB Tree**: Convert BWM AABB tree to NavigationMesh AABB tree (for WOK files)

### Key Methods

- **Convert(bwm)**: Converts a BWM to NavigationMesh
- **ConvertWithOffset(bwm, offset)**: Converts with position offset (for room placement)
- **Merge(meshes)**: Merges multiple NavigationMesh instances into one
- **DetectAndConnectCrossMeshAdjacencies()**: Connects edges between different meshes

### Cross-Mesh Adjacency Detection

When merging multiple walkmeshes (e.g., from different rooms), the converter detects matching edges and connects them:

1. **Build Edge Map**: Create mapping from edge (vertex pair) to faces
2. **Find Matching Edges**: Find edges that share the same vertex positions (within tolerance)
3. **Connect Walkable Faces**: Only connect walkable faces to ensure proper pathfinding
4. **Update Adjacency Array**: Link faces bidirectionally in adjacency array

This is critical for the indoor map builder, which combines room walkmeshes into a single area navigation mesh.

## Indoor Map Builder and Kits

### What are Kits?

A `Kit` is a collection of reusable assets for building indoor maps:
- **Components**: Individual rooms (KitComponent) with BWM, MDL/MDX models, and hooks
- **Doors**: Door definitions (KitDoor) with UTD data and dimensions
- **Textures**: Shared texture files
- **Lightmaps**: Shared lightmap files
- **Skyboxes**: Skybox models

### KitComponent

A `KitComponent` represents a single room or module piece:
- **Bwm**: The walkmesh for the room
- **Mdl/Mdx**: 3D model data
- **Hooks**: Connection points (KitComponentHook) for linking rooms together

### IndoorMap

The `IndoorMap` class builds a complete module from multiple rooms:

1. **Process Room Components**: Extract used rooms and kits
2. **Handle Textures**: Rename and copy texture files
3. **Handle Lightmaps**: Process and rename lightmap files
4. **Process BWMs**: Transform room walkmeshes (flip, rotate, translate)
5. **Remap Transitions**: Update transition indices to connect rooms
6. **Add BWM Resources**: Write WOK files to module

### ProcessBwm Function

The `ProcessBwm` function (in `IndoorMap.cs`) processes a room's walkmesh:

```csharp
private BWM ProcessBwm(IndoorMapRoom room)
{
    var bwm = DeepCopyBwm(room.BaseWalkmesh());
    bwm.Flip(room.FlipX, room.FlipY);
    bwm.Rotate(room.Rotation);
    bwm.Translate(room.Position.X, room.Position.Y, room.Position.Z);
    
    // Remap transitions to connect rooms
    for (int hookIndex = 0; hookIndex < room.Hooks.Count; hookIndex++)
    {
        var connection = room.Hooks[hookIndex];
        int dummyIndex = (int)room.Component.Hooks[hookIndex].Edge;
        int? actualIndex = connection == null ? (int?)null : Rooms.IndexOf(connection);
        RemapTransitions(bwm, dummyIndex, actualIndex);
    }
    
    return bwm;
}
```

**Critical**: Materials are preserved during all transformations:
- `DeepCopyBwm` copies `Material = face.Material` for each face
- `Flip`, `Rotate`, `Translate` only modify vertices, not materials
- `RemapTransitions` only modifies `Trans1/Trans2/Trans3`, not materials

## Potential Walkability Bug Analysis

### Problem Statement

Levels/modules built with the indoor map builder are NOT walkable despite having the correct surface material.

### Investigation

After thorough code analysis, materials should be preserved during BWM transformations:

1. **DeepCopyBwm**: ✅ Copies materials correctly (`newFace.Material = face.Material`)
2. **Flip/Rotate/Translate**: ✅ Only modify vertices, not materials
3. **RemapTransitions**: ✅ Only modifies transitions, not materials

### Potential Bug Sources

#### 1. Material ID Mismatch

**Issue**: The `IsFaceWalkable` functions in converters use hardcoded switch statements that might not match `NavigationMesh.WalkableMaterials`.

**Location**: 
- `BwmToNavigationMeshConverter.IsFaceWalkable()` (lines 414-437)
- `BwmToEclipseNavigationMeshConverter.IsFaceWalkable()` (lines 565-588)

**Fix**: These should use `NavigationMesh.WalkableMaterials.Contains(material)` instead of hardcoded switch statements to ensure consistency.

#### 2. Cross-Mesh Adjacency Only Connects Walkable Faces

**Issue**: `DetectAndConnectCrossMeshAdjacencies` only connects walkable faces. If a face has the correct material but isn't recognized as walkable, it won't be connected to adjacent faces, breaking pathfinding.

**Location**: `BwmToNavigationMeshConverter.DetectAndConnectCrossMeshAdjacencies()` (line 301)

**Fix**: Ensure `IsFaceWalkable` matches `NavigationMesh.WalkableMaterials` exactly.

#### 3. Material Not Preserved During Serialization

**Issue**: Materials might be lost when writing/reading BWM files.

**Location**: `BWMAuto.BytesBwm()` and `BWMAuto.ReadBwm()`

**Fix**: Verify that material IDs are correctly serialized and deserialized.

#### 4. WalkableMaterials Set Mismatch

**Issue**: `NavigationMesh.WalkableMaterials` might not match `SurfaceMaterialExtensions.WalkableMaterials`.

**Location**: 
- `NavigationMesh.WalkableMaterials` (HashSet<int>)
- `SurfaceMaterialExtensions.WalkableMaterials` (HashSet<SurfaceMaterial>)

**Fix**: Ensure both sets contain the same material IDs. Consider using a shared constant or validation check.

### Recommended Fixes

1. **Unify Walkability Checks**: Replace hardcoded switch statements in converters with `NavigationMesh.WalkableMaterials.Contains(material)` or a shared helper method.

2. **Add Validation**: Add a validation check that ensures `NavigationMesh.WalkableMaterials` matches `SurfaceMaterialExtensions.WalkableMaterials` at startup.

3. **Add Debug Logging**: Log material IDs during BWM processing to verify they're preserved.

4. **Test Material Preservation**: Add unit tests that verify materials are preserved during Flip/Rotate/Translate operations.

### Conclusion

The most likely bug is a mismatch between the hardcoded `IsFaceWalkable` switch statements in the converters and the `WalkableMaterials` set in `NavigationMesh`. This would cause faces to be incorrectly marked as non-walkable during cross-mesh adjacency detection, breaking pathfinding connectivity.

