using System;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Games.Common.Components;

namespace Andastra.Runtime.Games.Infinity.Components
{
    /// <summary>
    /// Infinity engine-specific door component implementation.
    /// </summary>
    /// <remarks>
    /// Infinity Door Component:
    /// - Inherits from BaseDoorComponent for common door functionality
    /// - Infinity-specific: May use different door systems or may not support doors
    /// - Based on MassEffect.exe and MassEffect2.exe (Mass Effect, Mass Effect 2)
    /// - Note: Infinity engines may not have traditional door systems like Odyssey/Aurora
    /// - If doors are supported, they would use Infinity-specific file formats and systems
    /// - Original implementation: Needs reverse engineering from MassEffect.exe and MassEffect2.exe
    /// </remarks>
    public class InfinityDoorComponent : BaseDoorComponent
    {
        /// <summary>
        /// Linked flags for transitions (Infinity-specific implementation, if supported).
        /// </summary>
        public int LinkedToFlags { get; set; }

        /// <summary>
        /// Whether this door is a module transition.
        /// </summary>
        /// <remarks>
        /// Module Transition Check:
        /// - Infinity engines may not support module transitions via doors
        /// - If supported, implementation would be Infinity-specific
        /// - Original implementation: Needs reverse engineering from MassEffect.exe and MassEffect2.exe
        /// </remarks>
        public override bool IsModuleTransition
        {
            get { return (LinkedToFlags & 1) != 0 && !string.IsNullOrEmpty(LinkedToModule); }
        }

        /// <summary>
        /// Whether this door is an area transition.
        /// </summary>
        /// <remarks>
        /// Area Transition Check:
        /// - Infinity engines may not support area transitions via doors
        /// - If supported, implementation would be Infinity-specific
        /// - Original implementation: Needs reverse engineering from MassEffect.exe and MassEffect2.exe
        /// </remarks>
        public override bool IsAreaTransition
        {
            get { return (LinkedToFlags & 2) != 0 && !string.IsNullOrEmpty(LinkedTo); }
        }
    }
}

