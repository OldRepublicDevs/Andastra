using StrideGraphics = Stride.Graphics;
using Stride.Engine;

namespace Andastra.Runtime.Stride.Graphics
{
    /// <summary>
    /// Extension methods for Stride GraphicsDevice to provide compatibility with older API.
    /// </summary>
    /// <remarks>
    /// TODO: STUB - ImmediateContext and Services were removed from GraphicsDevice in newer Stride versions.
    /// Need to implement proper CommandList retrieval from GraphicsDevice.ResourceFactory or CommandListPool.
    /// Services should be obtained from Game.Services or ServiceRegistry, not from GraphicsDevice.
    /// For now, returns null to allow compilation. Code using this should handle null case.
    /// </remarks>
    public static class GraphicsDeviceExtensions
    {
        /// <summary>
        /// Gets the immediate command list for the graphics device.
        /// </summary>
        /// <remarks>
        /// TODO: STUB - This is a compatibility shim. In newer Stride versions, CommandList must be obtained
        /// from ResourceFactory.AllocateCommandList() or from a CommandListPool. This needs proper implementation.
        /// </remarks>
        [JetBrains.Annotations.CanBeNull]
        public static StrideGraphics.CommandList ImmediateContext(this StrideGraphics.GraphicsDevice device)
        {
            // TODO: STUB - Implement proper CommandList retrieval
            // In newer Stride, use: device.ResourceFactory.CreateCommandList() or get from CommandListPool
            // For now, return null to allow compilation - calling code must handle null
            return null;
        }

        /// <summary>
        /// Gets the service registry for the graphics device.
        /// </summary>
        /// <remarks>
        /// TODO: STUB - GraphicsDevice.Services was removed in newer Stride versions.
        /// Services should be obtained from Game.Services or passed as a parameter.
        /// For now, return null to allow compilation - calling code must handle null.
        /// </remarks>
        [JetBrains.Annotations.CanBeNull]
        public static object Services(this StrideGraphics.GraphicsDevice device)
        {
            // TODO: STUB - Implement proper Services retrieval
            // In newer Stride, services are accessed through Game.Services, not GraphicsDevice.Services
            // The return type should be ServiceRegistry or IServiceRegistry, but using object for now
            // For now, return null to allow compilation - calling code must handle null
            return null;
        }
    }
}

