using System;
using Odyssey.Graphics;

namespace Odyssey.Game.Core
{
    /// <summary>
    /// Factory for creating graphics backend instances.
    /// Located in Odyssey.Game to avoid circular dependencies in the abstraction layer.
    /// </summary>
    public static class GraphicsBackendFactory
    {
        /// <summary>
        /// Creates a graphics backend of the specified type.
        /// </summary>
        /// <param name="backendType">The backend type to create.</param>
        /// <returns>An instance of the graphics backend.</returns>
        public static IGraphicsBackend CreateBackend(GraphicsBackendType backendType)
        {
            switch (backendType)
            {
                case GraphicsBackendType.MonoGame:
                    return new Odyssey.MonoGame.Graphics.MonoGameGraphicsBackend();
                case GraphicsBackendType.Stride:
                    return new Odyssey.Stride.Graphics.StrideGraphicsBackend();
                default:
                    throw new ArgumentException("Unknown graphics backend type: " + backendType, nameof(backendType));
            }
        }
    }
}

