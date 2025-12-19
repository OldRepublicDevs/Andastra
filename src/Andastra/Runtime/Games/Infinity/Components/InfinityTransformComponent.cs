using System.Numerics;
using Andastra.Runtime.Games.Common.Components;

namespace Andastra.Runtime.Games.Infinity.Components
{
    /// <summary>
    /// Infinity Engine (Mass Effect) specific transform component implementation.
    /// </summary>
    /// <remarks>
    /// Infinity Transform Component:
    /// - Based on MassEffect.exe and MassEffect2.exe entity transform system
    /// - Transform system similar to other engines (needs verification via Ghidra)
    /// - Infinity coordinate system: Y-up, positions in meters, facing in radians (0 = +X axis, counter-clockwise)
    /// - Transform component attached to all entities in InfinityEntity.AttachCommonComponents
    /// </remarks>
    public class InfinityTransformComponent : BaseTransformComponent
    {
        /// <summary>
        /// Creates a new Infinity transform component with default values.
        /// </summary>
        public InfinityTransformComponent() : base()
        {
        }

        /// <summary>
        /// Creates a new Infinity transform component with specified position and facing.
        /// </summary>
        /// <param name="position">Initial world position.</param>
        /// <param name="facing">Initial facing angle in radians.</param>
        public InfinityTransformComponent(Vector3 position, float facing) : base(position, facing)
        {
        }
    }
}

