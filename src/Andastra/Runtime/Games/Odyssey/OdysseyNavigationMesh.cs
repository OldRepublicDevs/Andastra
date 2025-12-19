using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Navigation;

namespace Andastra.Runtime.Games.Odyssey
{
    /// <summary>
    /// Odyssey Engine walkmesh implementation for navigation and collision detection.
    /// </summary>
    /// <remarks>
    /// Odyssey Navigation Mesh Implementation:
    /// - Based on walkmesh format used in KotOR/KotOR2
    /// - Provides pathfinding and collision detection
    /// - Supports point projection to walkable surfaces
    ///
    /// Based on reverse engineering of:
    /// - swkotor.exe: Walkmesh loading and navigation functions
    /// - swkotor2.exe: Walkmesh projection (FUN_004f5070 @ 0x004f5070)
    /// - swkotor2.exe: Line-of-sight raycast (FUN_0054be70 @ 0x0054be70) - performs walkmesh raycasts for visibility checks
    /// - Walkmesh binary format: Vertices, faces, adjacency information
    ///
    /// Walkmesh features:
    /// - Triangle-based mesh for walkable surfaces
    /// - Collision detection against unwalkable geometry
    /// - Pathfinding support with A* algorithm
    /// - Point projection for accurate positioning
    /// - Raycast for line-of-sight and collision detection
    /// </remarks>
    [PublicAPI]
    public class OdysseyNavigationMesh : INavigationMesh
    {
        // Walkmesh data structures
        private Vector3[] _vertices;
        private int[] _faceIndices;        // 3 vertex indices per face
        private int[] _adjacency;          // 3 adjacency entries per face (-1 = no neighbor)
        private int[] _surfaceMaterials;   // Material per face
        private NavigationMesh.AabbNode _aabbRoot;
        private int _faceCount;

        // Surface material walkability lookup (based on surfacemat.2da)
        private static readonly HashSet<int> WalkableMaterials = new HashSet<int>
        {
            1,  // Dirt
            3,  // Grass
            4,  // Stone
            5,  // Wood
            6,  // Water (shallow)
            9,  // Carpet
            10, // Metal
            11, // Puddles
            12, // Swamp
            13, // Mud
            14, // Leaves
            16, // BottomlessPit (walkable but dangerous)
            18, // Door
            20, // Sand
            21, // BareBones
            22, // StoneBridge
            30  // Trigger (PyKotor extended)
        };

        /// <summary>
        /// Creates an empty Odyssey navigation mesh.
        /// </summary>
        public OdysseyNavigationMesh()
        {
            _vertices = new Vector3[0];
            _faceIndices = new int[0];
            _adjacency = new int[0];
            _surfaceMaterials = new int[0];
            _aabbRoot = null;
            _faceCount = 0;
        }

        /// <summary>
        /// Creates an Odyssey navigation mesh from walkmesh data.
        /// </summary>
        /// <param name="vertices">Walkmesh vertices.</param>
        /// <param name="faceIndices">Face vertex indices (3 per face).</param>
        /// <param name="adjacency">Face adjacency data (3 per face, -1 = no neighbor).</param>
        /// <param name="surfaceMaterials">Surface material indices per face.</param>
        /// <param name="aabbRoot">AABB tree root for spatial acceleration.</param>
        public OdysseyNavigationMesh(
            Vector3[] vertices,
            int[] faceIndices,
            int[] adjacency,
            int[] surfaceMaterials,
            NavigationMesh.AabbNode aabbRoot)
        {
            _vertices = vertices ?? throw new ArgumentNullException(nameof(vertices));
            _faceIndices = faceIndices ?? throw new ArgumentNullException(nameof(faceIndices));
            _adjacency = adjacency ?? new int[0];
            _surfaceMaterials = surfaceMaterials ?? throw new ArgumentNullException(nameof(surfaceMaterials));
            _aabbRoot = aabbRoot;
            _faceCount = faceIndices.Length / 3;
        }
        /// <summary>
        /// Tests if a point is on walkable ground.
        /// </summary>
        /// <remarks>
        /// Based on walkmesh projection logic in swkotor2.exe.
        /// Checks if point can be projected onto a walkable triangle.
        /// </remarks>
        public bool IsPointWalkable(Vector3 point)
        {
            // TODO: Implement walkmesh point testing
            // Project point vertically and check if it hits walkable surface
            throw new System.NotImplementedException("Walkmesh point testing not yet implemented");
        }

        /// <summary>
        /// Projects a point onto the walkmesh surface.
        /// </summary>
        /// <remarks>
        /// Based on FUN_004f5070 @ 0x004f5070 in swkotor2.exe.
        /// Projects points to the nearest walkable surface.
        /// Used for collision detection and pathfinding.
        /// </remarks>
        public bool ProjectToWalkmesh(Vector3 point, out Vector3 result, out float height)
        {
            // TODO: Implement walkmesh projection
            // Find nearest walkable triangle and project point onto it
            result = point;
            height = point.Y;
            throw new System.NotImplementedException("Walkmesh projection not yet implemented");
        }

        /// <summary>
        /// Finds a path between two points.
        /// </summary>
        /// <remarks>
        /// Implements A* pathfinding on walkmesh triangles.
        /// Returns waypoints for entity movement.
        /// </remarks>
        public bool FindPath(Vector3 start, Vector3 end, out Vector3[] waypoints)
        {
            // TODO: Implement A* pathfinding on walkmesh
            waypoints = new[] { start, end };
            throw new System.NotImplementedException("Pathfinding not yet implemented");
        }

        /// <summary>
        /// Gets the height at a specific point.
        /// </summary>
        /// <remarks>
        /// Returns the walkable height at the given X,Z coordinates.
        /// Returns false if point is not over walkable surface.
        /// </remarks>
        public bool GetHeightAtPoint(Vector3 point, out float height)
        {
            // TODO: Implement height sampling
            height = point.Y;
            throw new System.NotImplementedException("Height sampling not yet implemented");
        }

        /// <summary>
        /// Checks line of sight between two points.
        /// </summary>
        /// <remarks>
        /// Tests if line segment between points doesn't intersect unwalkable geometry.
        /// Used for AI perception and projectile collision.
        /// 
        /// Based on swkotor2.exe: FUN_0054be70 @ 0x0054be70 performs walkmesh raycasts for visibility checks.
        /// The original implementation:
        /// 1. Performs a raycast from start to end position
        /// 2. If a hit is found, checks if the hit face is walkable (walkable faces don't block line of sight)
        /// 3. Also checks if the hit point is very close to the destination (within tolerance) - if so, line of sight is considered clear
        /// 4. Returns true if no obstruction or if the obstruction is walkable/close to destination
        /// 
        /// This implementation matches the behavior used by:
        /// - Perception system for AI visibility checks
        /// - Projectile collision detection
        /// - Movement collision detection
        /// </remarks>
        public bool HasLineOfSight(Vector3 start, Vector3 end)
        {
            // Handle edge case: same point
            Vector3 direction = end - start;
            float distance = direction.Length();
            if (distance < 1e-6f)
            {
                return true; // Same point, line of sight is clear
            }

            // Normalize direction for raycast
            Vector3 normalizedDir = direction / distance;

            // Perform raycast to check for obstructions
            Vector3 hitPoint;
            int hitFace;
            if (Raycast(start, normalizedDir, distance, out hitPoint, out hitFace))
            {
                // A hit was found - check if it blocks line of sight
                
                // Calculate distances
                float distToHit = Vector3.Distance(start, hitPoint);
                float distToDest = distance;
                
                // If hit is very close to destination (within tolerance), consider line of sight clear
                // This handles cases where the raycast hits the destination face itself
                const float tolerance = 0.5f; // 0.5 unit tolerance for walkmesh precision
                if (distToDest - distToHit < tolerance)
                {
                    return true; // Hit is at or very close to destination, line of sight is clear
                }
                
                // Check if the hit face is walkable - walkable faces don't block line of sight
                // This allows entities to see through walkable surfaces (e.g., through doorways, over walkable terrain)
                if (hitFace >= 0 && IsWalkable(hitFace))
                {
                    return true; // Hit a walkable face, line of sight is clear
                }
                
                // Hit a non-walkable face that blocks line of sight
                return false;
            }

            // No hit found - line of sight is clear
            return true;
        }

        // INavigationMesh interface methods
        public IList<Vector3> FindPath(Vector3 start, Vector3 goal)
        {
            // TODO: STUB - Implement A* pathfinding on walkmesh
            throw new NotImplementedException("FindPath: Walkmesh pathfinding not yet implemented");
        }

        public int FindFaceAt(Vector3 position)
        {
            // TODO: STUB - Implement face lookup at position
            throw new NotImplementedException("FindFaceAt: Face lookup not yet implemented");
        }

        public Vector3 GetFaceCenter(int faceIndex)
        {
            // TODO: STUB - Implement face center calculation
            throw new NotImplementedException("GetFaceCenter: Face center calculation not yet implemented");
        }

        public IEnumerable<int> GetAdjacentFaces(int faceIndex)
        {
            // TODO: STUB - Implement adjacent face lookup
            throw new NotImplementedException("GetAdjacentFaces: Adjacent face lookup not yet implemented");
        }

        /// <summary>
        /// Checks if a face is walkable based on its surface material.
        /// </summary>
        /// <remarks>
        /// Based on swkotor.exe and swkotor2.exe walkmesh walkability checks.
        /// Surface materials are looked up from surfacemat.2da to determine walkability.
        /// Walkable materials include dirt, grass, stone, wood, water, carpet, metal, etc.
        /// Non-walkable materials include lava, deep water, non-walk surfaces, etc.
        /// </remarks>
        public bool IsWalkable(int faceIndex)
        {
            if (faceIndex < 0 || faceIndex >= _surfaceMaterials.Length)
            {
                return false;
            }

            int material = _surfaceMaterials[faceIndex];
            return WalkableMaterials.Contains(material);
        }

        /// <summary>
        /// Gets the surface material index for a given face.
        /// </summary>
        /// <remarks>
        /// Based on swkotor.exe and swkotor2.exe walkmesh surface material lookup.
        /// Surface materials are stored per-face and correspond to entries in surfacemat.2da.
        /// Material indices range from 0-30, with specific meanings:
        /// - 0: Undefined
        /// - 1: Dirt (walkable)
        /// - 3: Grass (walkable)
        /// - 4: Stone (walkable)
        /// - 7: NonWalk (non-walkable)
        /// - 15: Lava (non-walkable)
        /// - 17: DeepWater (non-walkable)
        /// - etc.
        /// </remarks>
        public int GetSurfaceMaterial(int faceIndex)
        {
            if (faceIndex < 0 || faceIndex >= _surfaceMaterials.Length)
            {
                return 0; // Return undefined material for invalid face index
            }

            return _surfaceMaterials[faceIndex];
        }

        /// <summary>
        /// Performs a raycast against the walkmesh.
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe: FUN_0054be70 @ 0x0054be70 performs walkmesh raycasts for visibility checks.
        /// Uses AABB tree for spatial acceleration when available, falls back to brute force.
        /// Returns the closest hit point and face index along the ray.
        /// </remarks>
        public bool Raycast(Vector3 origin, Vector3 direction, float maxDistance, out Vector3 hitPoint, out int hitFace)
        {
            hitPoint = Vector3.Zero;
            hitFace = -1;

            if (_faceCount == 0)
            {
                return false;
            }

            // Normalize direction
            float dirLength = direction.Length();
            if (dirLength < 1e-6f)
            {
                return false;
            }
            Vector3 normalizedDir = direction / dirLength;

            // Use AABB tree if available for faster spatial queries
            if (_aabbRoot != null)
            {
                return RaycastAabb(_aabbRoot, origin, normalizedDir, maxDistance, out hitPoint, out hitFace);
            }

            // Brute force fallback - test all faces
            float bestDist = maxDistance;
            for (int i = 0; i < _faceCount; i++)
            {
                float dist;
                if (RayTriangleIntersect(origin, normalizedDir, i, bestDist, out dist))
                {
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        hitFace = i;
                        hitPoint = origin + normalizedDir * dist;
                    }
                }
            }

            return hitFace >= 0;
        }

        /// <summary>
        /// Performs raycast using AABB tree traversal for spatial acceleration.
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe walkmesh AABB tree structure.
        /// Recursively traverses the tree, testing ray against bounding boxes first,
        /// then testing actual triangles in leaf nodes.
        /// </remarks>
        private bool RaycastAabb(NavigationMesh.AabbNode node, Vector3 origin, Vector3 direction, float maxDist, out Vector3 hitPoint, out int hitFace)
        {
            hitPoint = Vector3.Zero;
            hitFace = -1;

            if (node == null)
            {
                return false;
            }

            // Test ray against AABB bounds
            if (!RayAabbIntersect(origin, direction, node.BoundsMin, node.BoundsMax, maxDist))
            {
                return false;
            }

            // Leaf node - test ray against face
            if (node.FaceIndex >= 0)
            {
                float dist;
                if (RayTriangleIntersect(origin, direction, node.FaceIndex, maxDist, out dist))
                {
                    hitPoint = origin + direction * dist;
                    hitFace = node.FaceIndex;
                    return true;
                }
                return false;
            }

            // Internal node - test children
            float bestDist = maxDist;
            bool hit = false;

            if (node.Left != null)
            {
                Vector3 leftHit;
                int leftFace;
                if (RaycastAabb(node.Left, origin, direction, bestDist, out leftHit, out leftFace))
                {
                    float dist = Vector3.Distance(origin, leftHit);
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        hitPoint = leftHit;
                        hitFace = leftFace;
                        hit = true;
                    }
                }
            }

            if (node.Right != null)
            {
                Vector3 rightHit;
                int rightFace;
                if (RaycastAabb(node.Right, origin, direction, bestDist, out rightHit, out rightFace))
                {
                    float dist = Vector3.Distance(origin, rightHit);
                    if (dist < bestDist)
                    {
                        hitPoint = rightHit;
                        hitFace = rightFace;
                        hit = true;
                    }
                }
            }

            return hit;
        }

        /// <summary>
        /// Tests if a ray intersects an axis-aligned bounding box.
        /// </summary>
        /// <remarks>
        /// Uses the slab method for efficient AABB-ray intersection testing.
        /// Based on standard ray-AABB intersection algorithm used in swkotor2.exe.
        /// </remarks>
        private bool RayAabbIntersect(Vector3 origin, Vector3 direction, Vector3 bbMin, Vector3 bbMax, float maxDist)
        {
            // Avoid division by zero
            float invDirX = direction.X != 0f ? 1f / direction.X : float.MaxValue;
            float invDirY = direction.Y != 0f ? 1f / direction.Y : float.MaxValue;
            float invDirZ = direction.Z != 0f ? 1f / direction.Z : float.MaxValue;

            float tmin = (bbMin.X - origin.X) * invDirX;
            float tmax = (bbMax.X - origin.X) * invDirX;

            if (invDirX < 0)
            {
                float temp = tmin;
                tmin = tmax;
                tmax = temp;
            }

            float tymin = (bbMin.Y - origin.Y) * invDirY;
            float tymax = (bbMax.Y - origin.Y) * invDirY;

            if (invDirY < 0)
            {
                float temp = tymin;
                tymin = tymax;
                tymax = temp;
            }

            if (tmin > tymax || tymin > tmax)
            {
                return false;
            }

            if (tymin > tmin) tmin = tymin;
            if (tymax < tmax) tmax = tymax;

            float tzmin = (bbMin.Z - origin.Z) * invDirZ;
            float tzmax = (bbMax.Z - origin.Z) * invDirZ;

            if (invDirZ < 0)
            {
                float temp = tzmin;
                tzmin = tzmax;
                tzmax = temp;
            }

            if (tmin > tzmax || tzmin > tmax)
            {
                return false;
            }

            if (tzmin > tmin) tmin = tzmin;

            if (tmin < 0) tmin = tmax;
            return tmin >= 0 && tmin <= maxDist;
        }

        /// <summary>
        /// Tests if a ray intersects a triangle face.
        /// </summary>
        /// <remarks>
        /// Uses the Möller-Trumbore algorithm for ray-triangle intersection.
        /// Based on standard ray-triangle intersection used in swkotor2.exe walkmesh collision.
        /// </remarks>
        private bool RayTriangleIntersect(Vector3 origin, Vector3 direction, int faceIndex, float maxDist, out float distance)
        {
            distance = 0f;

            if (faceIndex < 0 || faceIndex >= _faceCount)
            {
                return false;
            }

            int baseIdx = faceIndex * 3;
            if (baseIdx + 2 >= _faceIndices.Length)
            {
                return false;
            }

            Vector3 v0 = _vertices[_faceIndices[baseIdx]];
            Vector3 v1 = _vertices[_faceIndices[baseIdx + 1]];
            Vector3 v2 = _vertices[_faceIndices[baseIdx + 2]];

            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;

            Vector3 h = Vector3.Cross(direction, edge2);
            float a = Vector3.Dot(edge1, h);

            // Ray is parallel to triangle
            if (Math.Abs(a) < 1e-6f)
            {
                return false;
            }

            float f = 1f / a;
            Vector3 s = origin - v0;
            float u = f * Vector3.Dot(s, h);

            if (u < 0f || u > 1f)
            {
                return false;
            }

            Vector3 q = Vector3.Cross(s, edge1);
            float v = f * Vector3.Dot(direction, q);

            if (v < 0f || u + v > 1f)
            {
                return false;
            }

            float t = f * Vector3.Dot(edge2, q);

            if (t > 1e-6f && t < maxDist)
            {
                distance = t;
                return true;
            }

            return false;
        }

        public bool TestLineOfSight(Vector3 from, Vector3 to)
        {
            return HasLineOfSight(from, to);
        }

        public bool ProjectToSurface(Vector3 point, out Vector3 result, out float height)
        {
            return ProjectToWalkmesh(point, out result, out height);
        }
    }
}
