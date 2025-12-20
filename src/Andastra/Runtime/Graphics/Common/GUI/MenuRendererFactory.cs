using System;
using System.Linq;
using System.Reflection;
using Andastra.Runtime.Graphics;
using Andastra.Runtime.Graphics.Common.Enums;
using Andastra.Runtime.MonoGame.Graphics;
using Andastra.Runtime.MonoGame.GUI;
using JetBrains.Annotations;
using Microsoft.Xna.Framework.Graphics;

namespace Andastra.Runtime.Graphics.Common.GUI
{
    /// <summary>
    /// Factory for creating menu renderers based on graphics backend type.
    /// </summary>
    /// <remarks>
    /// Menu Renderer Factory:
    /// - Creates appropriate menu renderer based on graphics backend (MonoGame, Stride)
    /// - Menu renderers are backend-specific but engine-agnostic (work for all engines)
    /// - Based on exhaustive reverse engineering of original engine menu initialization
    /// - All engines (Odyssey, Aurora, Eclipse, Infinity) use the same menu renderer interface
    /// - Engine-specific menu initialization is handled by the game session, not the renderer
    /// </remarks>
    public static class MenuRendererFactory
    {
        /// <summary>
        /// Creates a menu renderer for the specified graphics backend.
        /// </summary>
        /// <param name="graphicsBackend">The graphics backend to create a menu renderer for.</param>
        /// <returns>A menu renderer instance, or null if the backend type is not supported.</returns>
        /// <exception cref="ArgumentNullException">Thrown if graphicsBackend is null.</exception>
        public static BaseMenuRenderer CreateMenuRenderer([NotNull] IGraphicsBackend graphicsBackend)
        {
            if (graphicsBackend == null)
            {
                throw new ArgumentNullException(nameof(graphicsBackend));
            }

            if (!graphicsBackend.IsInitialized)
            {
                throw new InvalidOperationException("Graphics backend must be initialized before creating menu renderer");
            }

            var backendType = graphicsBackend.BackendType;
            var graphicsDevice = graphicsBackend.GraphicsDevice;

            switch (backendType)
            {
                case GraphicsBackendType.MonoGame:
                    return CreateMonoGameMenuRenderer(graphicsDevice);

                case GraphicsBackendType.Stride:
                    // TODO: STUB - Stride menu renderer not yet implemented
                    // Stride uses SpriteBatch for menu rendering
                    // StrideMenuRenderer requires Stride GraphicsDevice
                    Console.WriteLine($"[MenuRendererFactory] Stride menu renderer not yet implemented");
                    return null;

                default:
                    Console.WriteLine($"[MenuRendererFactory] Unsupported graphics backend type: {backendType}");
                    return null;
            }

            Console.WriteLine($"[MenuRendererFactory] Failed to create menu renderer for backend: {backendType}");
            return null;
        }

        /// <summary>
        /// Creates a MonoGame menu renderer using Myra UI library.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device wrapper from the backend.</param>
        /// <returns>A MyraMenuRenderer instance, or null if creation fails.</returns>
        /// <remarks>
        /// MonoGame Menu Renderer Creation:
        /// - Extracts the underlying MonoGame GraphicsDevice from the IGraphicsDevice wrapper
        /// - Uses reflection to access the private _device field in MonoGameGraphicsDevice
        /// - Creates MyraMenuRenderer with the extracted GraphicsDevice
        /// - Handles errors gracefully with detailed logging
        /// - Based on exhaustive reverse engineering of swkotor.exe and swkotor2.exe menu initialization
        /// - All engines (Odyssey, Aurora, Eclipse, Infinity) use the same menu renderer interface
        /// </remarks>
        private static BaseMenuRenderer CreateMonoGameMenuRenderer(IGraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null)
            {
                Console.WriteLine("[MenuRendererFactory] ERROR: GraphicsDevice is null for MonoGame backend");
                return null;
            }

            // Check if the graphics device is a MonoGameGraphicsDevice wrapper
            if (!(graphicsDevice is MonoGameGraphicsDevice monoGameDevice))
            {
                Console.WriteLine($"[MenuRendererFactory] ERROR: GraphicsDevice is not a MonoGameGraphicsDevice (type: {graphicsDevice.GetType().Name})");
                return null;
            }

            try
            {
                // Extract the underlying MonoGame GraphicsDevice using reflection
                // MonoGameGraphicsDevice wraps the actual GraphicsDevice in a private _device field
                GraphicsDevice mgGraphicsDevice = ExtractMonoGameGraphicsDevice(monoGameDevice);
                
                if (mgGraphicsDevice == null)
                {
                    Console.WriteLine("[MenuRendererFactory] ERROR: Failed to extract MonoGame GraphicsDevice from wrapper");
                    return null;
                }

                // Create MyraMenuRenderer with the extracted GraphicsDevice
                var renderer = new MyraMenuRenderer(mgGraphicsDevice);
                
                if (renderer == null)
                {
                    Console.WriteLine("[MenuRendererFactory] ERROR: Failed to create MyraMenuRenderer instance");
                    return null;
                }

                // Verify initialization
                if (!renderer.IsInitialized)
                {
                    Console.WriteLine("[MenuRendererFactory] WARNING: MyraMenuRenderer was created but not initialized");
                }

                Console.WriteLine($"[MenuRendererFactory] Successfully created MonoGame menu renderer (MyraMenuRenderer)");
                Console.WriteLine($"[MenuRendererFactory] Viewport: {renderer.ViewportWidth}x{renderer.ViewportHeight}");
                
                return renderer;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MenuRendererFactory] ERROR: Exception while creating MonoGame menu renderer: {ex.Message}");
                Console.WriteLine($"[MenuRendererFactory] Exception type: {ex.GetType().Name}");
                Console.WriteLine($"[MenuRendererFactory] Stack trace: {ex.StackTrace}");
                
                // Re-throw if it's a critical exception that should propagate
                if (ex is OutOfMemoryException || ex is StackOverflowException)
                {
                    throw;
                }
                
                return null;
            }
        }

        /// <summary>
        /// Extracts the underlying MonoGame GraphicsDevice from the MonoGameGraphicsDevice wrapper using reflection.
        /// </summary>
        /// <param name="wrapper">The MonoGameGraphicsDevice wrapper instance.</param>
        /// <returns>The underlying MonoGame GraphicsDevice, or null if extraction fails.</returns>
        /// <remarks>
        /// GraphicsDevice Extraction:
        /// - Uses reflection to access the private _device field in MonoGameGraphicsDevice
        /// - This is necessary because the wrapper doesn't expose the underlying device publicly
        /// - Handles reflection errors gracefully with detailed logging
        /// - Returns null if the field cannot be accessed or is null
        /// </remarks>
        private static GraphicsDevice ExtractMonoGameGraphicsDevice(MonoGameGraphicsDevice wrapper)
        {
            if (wrapper == null)
            {
                Console.WriteLine("[MenuRendererFactory] ERROR: MonoGameGraphicsDevice wrapper is null");
                return null;
            }

            try
            {
                // Get the type of MonoGameGraphicsDevice
                Type wrapperType = typeof(MonoGameGraphicsDevice);
                
                // Get the private _device field using reflection
                // BindingFlags.NonPublic | BindingFlags.Instance to access private instance field
                FieldInfo deviceField = wrapperType.GetField("_device", BindingFlags.NonPublic | BindingFlags.Instance);
                
                if (deviceField == null)
                {
                    Console.WriteLine("[MenuRendererFactory] ERROR: Could not find _device field in MonoGameGraphicsDevice");
                    Console.WriteLine($"[MenuRendererFactory] Available fields: {string.Join(", ", wrapperType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Select(f => f.Name))}");
                    return null;
                }

                // Get the value of the _device field from the wrapper instance
                object deviceValue = deviceField.GetValue(wrapper);
                
                if (deviceValue == null)
                {
                    Console.WriteLine("[MenuRendererFactory] ERROR: _device field is null in MonoGameGraphicsDevice wrapper");
                    return null;
                }

                // Cast to GraphicsDevice
                if (!(deviceValue is GraphicsDevice mgDevice))
                {
                    Console.WriteLine($"[MenuRendererFactory] ERROR: _device field is not a GraphicsDevice (type: {deviceValue.GetType().Name})");
                    return null;
                }

                Console.WriteLine($"[MenuRendererFactory] Successfully extracted MonoGame GraphicsDevice (Handle: {mgDevice.Handle})");
                return mgDevice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MenuRendererFactory] ERROR: Exception while extracting GraphicsDevice: {ex.Message}");
                Console.WriteLine($"[MenuRendererFactory] Exception type: {ex.GetType().Name}");
                Console.WriteLine($"[MenuRendererFactory] Stack trace: {ex.StackTrace}");
                return null;
            }
        }

    }
}

