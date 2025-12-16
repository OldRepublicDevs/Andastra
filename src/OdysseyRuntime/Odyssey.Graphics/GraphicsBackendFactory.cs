using System;

namespace Odyssey.Graphics
{
    /// <summary>
    /// Factory for creating graphics backend instances.
    /// </summary>
    public static class GraphicsBackendFactory
    {
        /// <summary>
        /// Creates a graphics backend instance based on the specified type.
        /// </summary>
        /// <param name="backendType">The type of backend to create.</param>
        /// <returns>An IGraphicsBackend instance.</returns>
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

    /// <summary>
    /// Enumeration of supported graphics backends.
    /// </summary>
    public enum GraphicsBackendType
    {
        MonoGame,
        Stride
    }
}

