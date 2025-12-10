using System.Collections.Generic;
using System.Linq;
using CSharpKOTOR.Common;
using CSharpKOTOR.Resource.Formats.GFF;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores the path data for a module.
    ///
    /// PTH files are GFF-based format files that store pathfinding data including
    /// waypoints and connections for NPC navigation.
    /// </summary>
    [PublicAPI]
    public sealed class PTH
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:33
        // Original: BINARY_TYPE = ResourceType.PTH
        public static readonly ResourceType BinaryType = ResourceType.PTH;

        private readonly List<Vector2> _points = new List<Vector2>();
        private readonly List<PTHEdge> _connections = new List<PTHEdge>();

        public PTH()
        {
        }

        public IEnumerable<Vector2> GetPoints()
        {
            return _points;
        }

        public int Count => _points.Count;

        public void AddPoint(Vector2 point)
        {
            _points.Add(point);
        }

        public void AddConnection(PTHEdge edge)
        {
            _connections.Add(edge);
        }

        public List<PTHEdge> GetConnections()
        {
            return new List<PTHEdge>(_connections);
        }

        public Vector2 GetPoint(int index)
        {
            return _points[index];
        }
    }

    /// <summary>
    /// Represents a connection between two pathfinding points.
    /// </summary>
    [PublicAPI]
    public sealed class PTHEdge
    {
        public int SourceIndex { get; set; }
        public int TargetIndex { get; set; }

        public PTHEdge(int sourceIndex, int targetIndex)
        {
            SourceIndex = sourceIndex;
            TargetIndex = targetIndex;
        }
    }
}
