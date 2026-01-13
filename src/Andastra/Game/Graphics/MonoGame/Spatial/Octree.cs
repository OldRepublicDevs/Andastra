using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using BioWare.NET.Resource.Formats.VIS;
using JetBrains.Annotations;

namespace Andastra.Game.Graphics.MonoGame.Spatial
{
    /// <summary>
    /// Octree spatial partitioning structure for efficient spatial queries and culling.
    /// 
    /// Octrees divide 3D space into 8 octants recursively, enabling:
    /// - Fast frustum culling (only test relevant nodes)
    /// - Efficient ray casting
    /// - Proximity queries
    /// - Dynamic object management
    /// 
    /// Rendering optimization system (Scene::LoadVisibility, Scene::SaveVisibility, Scene::SetVisibility):
    /// - K1: Scene::LoadVisibility @ 0x004568d0, Scene::SaveVisibility @ 0x00452b70, Scene::SetVisibility @ 0x00454940
    /// - TSL: Scene::SaveVisibility @ 0x00466d90 (LoadVisibility and SetVisibility addresses to be determined)
    /// - Located via string references: "VIS" (visibility data) @ 0x007415e8 (K1), 0x007b972c (TSL), ".vis" @ 0x00741604 (K1)
    /// - Original implementation: Uses VIS (visibility) files for room-based frustum culling
    /// - VIS files: ASCII format containing room visibility graph for frustum culling optimization
    /// - Room culling: Original engine uses VIS data to determine which rooms are visible from camera position
    /// - Frustum culling: Tests room bounding boxes against camera frustum to skip rendering hidden rooms
    /// - Spatial queries: Original engine uses room-based spatial queries for entity lookup
    /// - Combined approach: VIS-based room culling + frustum culling + octree spatial partitioning for optimal performance
    /// - This implementation: Modern octree enhancement that integrates VIS data with frustum culling
    /// - Note: Original engine primarily uses VIS-based room culling, octree adds flexible spatial partitioning
    /// </summary>
    public class Octree<T> where T : class
    {
        /// <summary>
        /// Octree node.
        /// </summary>
        private class OctreeNode
        {
            public BoundingBox Bounds;
            public List<T> Objects;
            public OctreeNode[] Children;
            public bool IsLeaf;
            public int Depth;

            public OctreeNode(BoundingBox bounds, int depth)
            {
                Bounds = bounds;
                Objects = new List<T>();
                Children = null;
                IsLeaf = true;
                Depth = depth;
            }
        }

        private readonly OctreeNode _root;
        private readonly int _maxDepth;
        private readonly int _maxObjectsPerNode;
        private readonly Func<T, BoundingBox> _getBounds;

        /// <summary>
        /// Initializes a new octree.
        /// </summary>
        /// <param name="bounds">Root bounding box.</param>
        /// <param name="maxDepth">Maximum tree depth.</param>
        /// <param name="maxObjectsPerNode">Maximum objects per node before splitting.</param>
        /// <param name="getBounds">Function to get bounding box for an object.</param>
        public Octree(BoundingBox bounds, int maxDepth, int maxObjectsPerNode, Func<T, BoundingBox> getBounds)
        {
            if (getBounds == null)
            {
                throw new ArgumentNullException(nameof(getBounds));
            }

            _root = new OctreeNode(bounds, 0);
            _maxDepth = maxDepth;
            _maxObjectsPerNode = maxObjectsPerNode;
            _getBounds = getBounds;
        }

        /// <summary>
        /// Inserts an object into the octree.
        /// </summary>
        public void Insert(T obj)
        {
            BoundingBox objBounds = _getBounds(obj);
            InsertRecursive(_root, obj, objBounds);
        }

        private void InsertRecursive(OctreeNode node, T obj, BoundingBox objBounds)
        {
            // Check if object fits in this node
            if (!node.Bounds.Intersects(objBounds.Min, objBounds.Max))
            {
                return;
            }

            // If leaf and not full, add to leaf
            if (node.IsLeaf && node.Objects.Count < _maxObjectsPerNode)
            {
                node.Objects.Add(obj);
                return;
            }

            // If leaf and full, split
            if (node.IsLeaf && node.Depth < _maxDepth)
            {
                SplitNode(node);
            }

            // Insert into children
            if (!node.IsLeaf)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (node.Children[i].Bounds.Intersects(objBounds.Min, objBounds.Max))
                    {
                        InsertRecursive(node.Children[i], obj, objBounds);
                    }
                }
            }
            else
            {
                // Can't split further, add to this node
                node.Objects.Add(obj);
            }
        }

        /// <summary>
        /// Splits a node into 8 children.
        /// </summary>
        private void SplitNode(OctreeNode node)
        {
            Vector3 center = node.Bounds.GetCenter();
            Vector3 halfSize = node.Bounds.GetSize() * 0.5f;
            Vector3 min = node.Bounds.Min;
            Vector3 max = node.Bounds.Max;

            node.Children = new OctreeNode[8];
            int depth = node.Depth + 1;

            // Create 8 octants
            node.Children[0] = new OctreeNode(new BoundingBox(min, center), depth); // -X, -Y, -Z
            node.Children[1] = new OctreeNode(new BoundingBox(new Vector3(center.X, min.Y, min.Z), new Vector3(max.X, center.Y, center.Z)), depth); // +X, -Y, -Z
            node.Children[2] = new OctreeNode(new BoundingBox(new Vector3(min.X, center.Y, min.Z), new Vector3(center.X, max.Y, center.Z)), depth); // -X, +Y, -Z
            node.Children[3] = new OctreeNode(new BoundingBox(new Vector3(center.X, center.Y, min.Z), new Vector3(max.X, max.Y, center.Z)), depth); // +X, +Y, -Z
            node.Children[4] = new OctreeNode(new BoundingBox(new Vector3(min.X, min.Y, center.Z), new Vector3(center.X, center.Y, max.Z)), depth); // -X, -Y, +Z
            node.Children[5] = new OctreeNode(new BoundingBox(new Vector3(center.X, min.Y, center.Z), new Vector3(max.X, center.Y, max.Z)), depth); // +X, -Y, +Z
            node.Children[6] = new OctreeNode(new BoundingBox(new Vector3(min.X, center.Y, center.Z), new Vector3(center.X, max.Y, max.Z)), depth); // -X, +Y, +Z
            node.Children[7] = new OctreeNode(new BoundingBox(center, max), depth); // +X, +Y, +Z

            // Move objects to children
            List<T> objectsToMove = new List<T>(node.Objects);
            node.Objects.Clear();

            foreach (T obj in objectsToMove)
            {
                BoundingBox objBounds = _getBounds(obj);
                for (int i = 0; i < 8; i++)
                {
                    if (node.Children[i].Bounds.Intersects(objBounds.Min, objBounds.Max))
                    {
                        InsertRecursive(node.Children[i], obj, objBounds);
                    }
                }
            }

            node.IsLeaf = false;
        }

        /// <summary>
        /// Queries objects within a bounding box.
        /// </summary>
        public void Query(BoundingBox bounds, List<T> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException("results");
            }

            QueryRecursive(_root, bounds, results);
        }

        private void QueryRecursive(OctreeNode node, BoundingBox bounds, List<T> results)
        {
            if (!node.Bounds.Intersects(bounds.Min, bounds.Max))
            {
                return;
            }

            // Add objects in this node
            foreach (T obj in node.Objects)
            {
                BoundingBox objBounds = _getBounds(obj);
                if (bounds.Intersects(objBounds.Min, objBounds.Max))
                {
                    results.Add(obj);
                }
            }

            // Query children
            if (!node.IsLeaf)
            {
                for (int i = 0; i < 8; i++)
                {
                    QueryRecursive(node.Children[i], bounds, results);
                }
            }
        }

        /// <summary>
        /// Queries objects within a frustum.
        /// </summary>
        public void QueryFrustum(Culling.Frustum frustum, List<T> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException("results");
            }

            QueryFrustumRecursive(_root, frustum, results);
        }

        /// <summary>
        /// Queries objects within a frustum with VIS-based room culling optimization.
        /// 
        /// This method combines frustum culling with VIS (visibility) data for optimal rendering performance.
        /// Based on Scene::LoadVisibility @ 0x004568d0 (K1) and Scene::SetVisibility @ 0x00454940 (K1).
        /// 
        /// Original engine behavior:
        /// - Uses VIS file to determine which rooms are visible from current camera position
        /// - Tests room bounding boxes against camera frustum to skip rendering hidden rooms
        /// - Only renders rooms that are both VIS-visible and within frustum
        /// 
        /// This implementation:
        /// - Requires room identifier function to map objects to room names for VIS lookup
        /// - Uses VIS data to filter objects by room visibility before frustum testing
        /// - Falls back to standard frustum culling if VIS data or room identifier is not provided
        /// </summary>
        /// <param name="frustum">Camera frustum for culling.</param>
        /// <param name="results">List to store visible objects.</param>
        /// <param name="visData">Optional VIS data for room-based culling. If null, uses standard frustum culling only.</param>
        /// <param name="currentRoomName">Optional current room name for VIS lookup. If null or empty, VIS culling is skipped.</param>
        /// <param name="getRoomName">Optional function to get room name from object. If null, VIS culling is skipped.</param>
        /// <remarks>
        /// swkotor.exe: Scene::LoadVisibility @ 0x004568d0 - Loads VIS file for area visibility culling
        /// swkotor.exe: Scene::SetVisibility @ 0x00454940 - Establishes room visibility relationships
        /// swkotor.exe: Scene::SaveVisibility @ 0x00452b70 - Saves VIS file (editor tool)
        /// swkotor2.exe: Scene::SaveVisibility @ 0x00466d90 - TSL equivalent
        /// </remarks>
        public void QueryFrustumWithVIS(
            Culling.Frustum frustum,
            List<T> results,
            [CanBeNull] VIS visData,
            [CanBeNull] string currentRoomName,
            [CanBeNull] Func<T, string> getRoomName)
        {
            if (results == null)
            {
                throw new ArgumentNullException("results");
            }

            // If VIS data or room identifier not provided, fall back to standard frustum culling
            if (visData == null || string.IsNullOrEmpty(currentRoomName) || getRoomName == null)
            {
                QueryFrustumRecursive(_root, frustum, results);
                return;
            }

            // Get set of visible rooms from VIS data
            string lowerCurrentRoom = currentRoomName.ToLowerInvariant();
            HashSet<string> visibleRooms = visData.GetVisibleRooms(lowerCurrentRoom);

            // If current room not in VIS data or has no visible rooms, use standard frustum culling
            if (visibleRooms == null || visibleRooms.Count == 0)
            {
                QueryFrustumRecursive(_root, frustum, results);
                return;
            }

            // Query with combined VIS + frustum culling
            QueryFrustumWithVISRecursive(_root, frustum, results, visData, visibleRooms, getRoomName);
        }

        /// <summary>
        /// Recursive helper for VIS + frustum culling query.
        /// </summary>
        private void QueryFrustumWithVISRecursive(
            OctreeNode node,
            Culling.Frustum frustum,
            List<T> results,
            VIS visData,
            HashSet<string> visibleRooms,
            Func<T, string> getRoomName)
        {
            // Test node bounds against frustum first (early rejection)
            System.Numerics.Vector3 min = new System.Numerics.Vector3(node.Bounds.Min.X, node.Bounds.Min.Y, node.Bounds.Min.Z);
            System.Numerics.Vector3 max = new System.Numerics.Vector3(node.Bounds.Max.X, node.Bounds.Max.Y, node.Bounds.Max.Z);
            if (!frustum.AabbInFrustum(min, max))
            {
                return;
            }

            // Add objects in this node that pass both VIS and frustum tests
            foreach (T obj in node.Objects)
            {
                // VIS culling: Check if object's room is visible from current room
                string objRoomName = getRoomName(obj);
                if (!string.IsNullOrEmpty(objRoomName))
                {
                    string lowerObjRoom = objRoomName.ToLowerInvariant();
                    if (!visibleRooms.Contains(lowerObjRoom))
                    {
                        continue; // Room not visible - skip this object
                    }
                }

                // Frustum culling: Test object bounding box against frustum
                BoundingBox objBounds = _getBounds(obj);
                System.Numerics.Vector3 objMin = new System.Numerics.Vector3(objBounds.Min.X, objBounds.Min.Y, objBounds.Min.Z);
                System.Numerics.Vector3 objMax = new System.Numerics.Vector3(objBounds.Max.X, objBounds.Max.Y, objBounds.Max.Z);
                if (frustum.AabbInFrustum(objMin, objMax))
                {
                    results.Add(obj);
                }
            }

            // Query children
            if (!node.IsLeaf)
            {
                for (int i = 0; i < 8; i++)
                {
                    QueryFrustumWithVISRecursive(node.Children[i], frustum, results, visData, visibleRooms, getRoomName);
                }
            }
        }

        private void QueryFrustumRecursive(OctreeNode node, Culling.Frustum frustum, List<T> results)
        {
            // Test node bounds against frustum
            // Convert XNA Vector3 to System.Numerics.Vector3
            System.Numerics.Vector3 min = new System.Numerics.Vector3(node.Bounds.Min.X, node.Bounds.Min.Y, node.Bounds.Min.Z);
            System.Numerics.Vector3 max = new System.Numerics.Vector3(node.Bounds.Max.X, node.Bounds.Max.Y, node.Bounds.Max.Z);
            if (!frustum.AabbInFrustum(min, max))
            {
                return;
            }

            // Add objects in this node
            foreach (T obj in node.Objects)
            {
                BoundingBox objBounds = _getBounds(obj);
                System.Numerics.Vector3 objMin = new System.Numerics.Vector3(objBounds.Min.X, objBounds.Min.Y, objBounds.Min.Z);
                System.Numerics.Vector3 objMax = new System.Numerics.Vector3(objBounds.Max.X, objBounds.Max.Y, objBounds.Max.Z);
                if (frustum.AabbInFrustum(objMin, objMax))
                {
                    results.Add(obj);
                }
            }

            // Query children
            if (!node.IsLeaf)
            {
                for (int i = 0; i < 8; i++)
                {
                    QueryFrustumRecursive(node.Children[i], frustum, results);
                }
            }
        }

        /// <summary>
        /// Clears all objects from the octree.
        /// </summary>
        public void Clear()
        {
            ClearRecursive(_root);
        }

        private void ClearRecursive(OctreeNode node)
        {
            node.Objects.Clear();
            if (!node.IsLeaf)
            {
                for (int i = 0; i < 8; i++)
                {
                    ClearRecursive(node.Children[i]);
                }
                node.Children = null;
                node.IsLeaf = true;
            }
        }
    }

    /// <summary>
    /// Bounding box structure.
    /// </summary>
    public struct BoundingBox
    {
        public Vector3 Min;
        public Vector3 Max;

        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        public Vector3 GetCenter()
        {
            return (Min + Max) * 0.5f;
        }

        public Vector3 GetSize()
        {
            return Max - Min;
        }

        public bool Intersects(Vector3 otherMin, Vector3 otherMax)
        {
            return Min.X <= otherMax.X && Max.X >= otherMin.X &&
                   Min.Y <= otherMax.Y && Max.Y >= otherMin.Y &&
                   Min.Z <= otherMax.Z && Max.Z >= otherMin.Z;
        }
    }
}

