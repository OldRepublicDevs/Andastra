using System.Numerics;

namespace Odyssey.Scripting.Types
{
    /// <summary>
    /// Represents a location (position + facing) in NWScript.
    /// </summary>
    public class Location
    {
        public Vector3 Position { get; set; }
        public float Facing { get; set; }

        public Location()
        {
            Position = Vector3.Zero;
            Facing = 0f;
        }

        public Location(Vector3 position, float facing)
        {
            Position = position;
            Facing = facing;
        }
    }
}

