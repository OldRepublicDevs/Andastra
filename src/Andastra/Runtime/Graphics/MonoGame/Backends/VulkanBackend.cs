using System;
using System.Collections.Generic;
using System.Diagnostics;
using Andastra.Runtime.Graphics.Common.Enums;
using Andastra.Runtime.Graphics.Common.Interfaces;
using Andastra.Runtime.Graphics.Common.Rendering;
using RuntimeGraphicsCapabilities = Andastra.Runtime.Graphics.Common.Structs.GraphicsCapabilities;
using RuntimeTextureDescription = Andastra.Runtime.Graphics.Common.Structs.TextureDescription;
using RuntimeBufferDescription = Andastra.Runtime.Graphics.Common.Structs.BufferDescription;
using RuntimePipelineDescription = Andastra.Runtime.Graphics.Common.Structs.PipelineDescription;
using Andastra.Game.Graphics.MonoGame.Interfaces;
using Andastra.Game.Graphics.MonoGame.Rendering;
using Andastra.Runtime.Graphics;

namespace Andastra.Runtime.Graphics.MonoGame.Backends
{
    /// <summary>
    /// Vulkan graphics backend implementation.
    ///
    /// Provides:
    /// - Vulkan 1.3+ features
    /// - VK_KHR_ray_tracing_pipeline extension
    /// - Cross-platform support (Windows, Linux, macOS)
    /// </summary>
    public class VulkanBackend : IGraphicsBackend
    {
        private bool _initialized;
        private RuntimeGraphicsCapabilities _capabilities;
        private Andastra.Game.Graphics.MonoGame.Rendering.RenderSettings _settings;
        private Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice _device;

        // Frame statistics tracking
        private FrameStatistics _lastFrameStats;
        private Stopwatch _frameTimer;
        private Stopwatch _cpuTimer;
        private double _frameStartTime;
        private HashSet<IntPtr> _texturesUsedThisFrame;
        private long _videoMemoryUsed;
        private double _gpuTimestampPeriod;
        private bool _gpuTimestampsSupported;

        public GraphicsBackendType BackendType
        {
            get { return GraphicsBackendType.Vulkan; }
        }

        public RuntimeGraphicsCapabilities Capabilities
        {
            get { return _capabilities; }
        }

        public bool IsInitialized
        {
            get { return _initialized; }
        }

        public bool IsRaytracingEnabled
        {
            get { return _capabilities.SupportsRaytracing; }
        }

        public Andastra.Game.Graphics.MonoGame.Rendering.RenderSettings Settings
        {
            get { return _settings; }
        }

        public IDevice Device
        {
            get { return _device; }
        }

        // TODO: STUB - Implement IGraphicsBackend interface members
        public IGraphicsDevice GraphicsDevice
        {
            get { throw new NotImplementedException("GraphicsDevice property not yet implemented in VulkanBackend"); }
        }

        public IContentManager ContentManager
        {
            get { throw new NotImplementedException("ContentManager property not yet implemented in VulkanBackend"); }
        }

        public IWindow Window
        {
            get { throw new NotImplementedException("Window property not yet implemented in VulkanBackend"); }
        }

        public IInputManager InputManager
        {
            get { throw new NotImplementedException("InputManager property not yet implemented in VulkanBackend"); }
        }

        public bool SupportsVSync
        {
            get { return _initialized; }
        }

        // IGraphicsBackend interface methods
        public void Initialize(int width, int height, string title, bool fullscreen = false)
        {
            if (_initialized)
            {
                return;
            }

            // Create RenderSettings from parameters
            Andastra.Game.Graphics.MonoGame.Rendering.RenderSettings settings = new Andastra.Game.Graphics.MonoGame.Rendering.RenderSettings
            {
                Width = width,
                Height = height,
                Fullscreen = fullscreen
            };

            // Call the existing Initialize method
            if (!Initialize(settings))
            {
                throw new InvalidOperationException("Failed to initialize Vulkan backend");
            }
        }

        public void Run(Action<float> updateAction, Action drawAction)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Backend must be initialized before running.");
            }

            // TODO: STUB - Implement Vulkan game loop
            // When fully implemented, this should:
            // - Create window using platform-specific windowing API (GLFW, SDL, or native Win32/X11/Cocoa)
            // - Set up swap chain for presentation
            // - Run main loop: while (!shouldExit) { updateAction(deltaTime); BeginFrame(); drawAction(); EndFrame(); }
            // - Handle window events (resize, close, input)
            // - Present swap chain images to screen
            throw new NotImplementedException("Run method not yet implemented in VulkanBackend");
        }

        public void Exit()
        {
            // TODO: STUB - Implement exit handling
            // When fully implemented, this should:
            // - Set exit flag to stop game loop
            // - Signal window to close
            // - Clean up resources
            throw new NotImplementedException("Exit method not yet implemented in VulkanBackend");
        }

        public IRoomMeshRenderer CreateRoomMeshRenderer()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Backend must be initialized before creating renderers.");
            }

            // TODO: STUB - Implement Vulkan room mesh renderer
            // When fully implemented, this should:
            // - Create VulkanRoomMeshRenderer instance
            // - Initialize with Vulkan device, command buffers, pipelines
            // - Set up vertex/index buffer management for room geometry
            throw new NotImplementedException("CreateRoomMeshRenderer not yet implemented in VulkanBackend");
        }

        public IEntityModelRenderer CreateEntityModelRenderer(object gameDataManager = null, object installation = null)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Backend must be initialized before creating renderers.");
            }

            // TODO: STUB - Implement Vulkan entity model renderer
            // When fully implemented, this should:
            // - Create VulkanEntityModelRenderer instance
            // - Initialize with Vulkan device, command buffers, pipelines
            // - Set up model loading and rendering pipeline
            throw new NotImplementedException("CreateEntityModelRenderer not yet implemented in VulkanBackend");
        }

        public ISpatialAudio CreateSpatialAudio()
        {
            // TODO: STUB - Implement Vulkan spatial audio
            // When fully implemented, this should:
            // - Create VulkanSpatialAudio instance or delegate to audio system
            // - Set up 3D audio positioning using Vulkan-compatible audio library
            throw new NotImplementedException("CreateSpatialAudio not yet implemented in VulkanBackend");
        }

        public object CreateDialogueCameraController(object cameraController)
        {
            if (cameraController == null)
            {
                throw new ArgumentNullException(nameof(cameraController));
            }

            // TODO: STUB - Implement Vulkan dialogue camera controller
            // When fully implemented, this should:
            // - Create VulkanDialogueCameraController instance
            // - Wrap the provided camera controller with Vulkan-specific rendering
            throw new NotImplementedException("CreateDialogueCameraController not yet implemented in VulkanBackend");
        }

        public object CreateSoundPlayer(object resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            // TODO: STUB - Implement Vulkan sound player
            // When fully implemented, this should:
            // - Create VulkanSoundPlayer instance
            // - Initialize with resource provider for loading audio files
            // - Set up audio playback using Vulkan-compatible audio library
            throw new NotImplementedException("CreateSoundPlayer not yet implemented in VulkanBackend");
        }

        public object CreateMusicPlayer(object resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            // TODO: STUB - Implement Vulkan music player
            // When fully implemented, this should:
            // - Create VulkanMusicPlayer instance
            // - Initialize with resource provider for loading music files
            // - Set up background music playback using Vulkan-compatible audio library
            throw new NotImplementedException("CreateMusicPlayer not yet implemented in VulkanBackend");
        }

        public object CreateVoicePlayer(object resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            // TODO: STUB - Implement Vulkan voice player
            // When fully implemented, this should:
            // - Create VulkanVoicePlayer instance
            // - Initialize with resource provider for loading voice files
            // - Set up voice-over dialogue playback using Vulkan-compatible audio library
            throw new NotImplementedException("CreateVoicePlayer not yet implemented in VulkanBackend");
        }

        public void SetVSync(bool enabled)
        {
            if (!_initialized)
            {
                return;
            }

            // TODO: STUB - Implement VSync setting
            // When fully implemented, this should:
            // - Set swap chain present mode to VK_PRESENT_MODE_FIFO_KHR (VSync on) or VK_PRESENT_MODE_IMMEDIATE_KHR (VSync off)
            // - Recreate swap chain if needed
            // - Apply changes immediately
        }

        // Internal Initialize method that takes RenderSettings
        public bool Initialize(Andastra.Game.Graphics.MonoGame.Rendering.RenderSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (_initialized)
            {
                return true;
            }

            _settings = settings;

            // Create Vulkan instance and select physical device
            IntPtr instance;
            IntPtr physicalDevice;
            uint graphicsQueueFamilyIndex;
            uint computeQueueFamilyIndex;
            uint transferQueueFamilyIndex;
            Andastra.Game.Graphics.MonoGame.Interfaces.GraphicsCapabilities gameCapabilities;

            if (!Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice.CreateVulkanInstance(
                out instance,
                out physicalDevice,
                out graphicsQueueFamilyIndex,
                out computeQueueFamilyIndex,
                out transferQueueFamilyIndex,
                out gameCapabilities))
            {
                return false;
            }

            // Convert Game GraphicsCapabilities to Runtime GraphicsCapabilities
            RuntimeGraphicsCapabilities capabilities = new RuntimeGraphicsCapabilities
            {
                SupportsRaytracing = gameCapabilities.SupportsRaytracing,
                MaxTextureSize = gameCapabilities.MaxTextureSize,
                MaxAnisotropy = gameCapabilities.MaxAnisotropy
            };

            // Create logical device
            IntPtr device;
            IntPtr graphicsQueue;
            IntPtr computeQueue;
            IntPtr transferQueue;

            if (!CreateVulkanDevice(
                instance,
                physicalDevice,
                graphicsQueueFamilyIndex,
                computeQueueFamilyIndex,
                transferQueueFamilyIndex,
                out device,
                out graphicsQueue,
                out computeQueue,
                out transferQueue,
                ref capabilities))
            {
                // Cleanup instance
                if (instance != IntPtr.Zero)
                {
                    // vkDestroyInstance will be called in VulkanDevice cleanup
                }
                return false;
            }

            // Create VulkanDevice wrapper
            // Convert RuntimeGraphicsCapabilities back to Game GraphicsCapabilities for VulkanDevice constructor
            Andastra.Game.Graphics.MonoGame.Interfaces.GraphicsCapabilities deviceCapabilities = new Andastra.Game.Graphics.MonoGame.Interfaces.GraphicsCapabilities
            {
                SupportsRaytracing = capabilities.SupportsRaytracing,
                MaxTextureSize = capabilities.MaxTextureSize,
                MaxAnisotropy = capabilities.MaxAnisotropy
            };

            _device = new Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice(
                device,
                instance,
                physicalDevice,
                graphicsQueue,
                computeQueue,
                transferQueue,
                deviceCapabilities);

            _capabilities = capabilities;

            // Initialize frame statistics tracking
            _lastFrameStats = new FrameStatistics();
            _frameTimer = new Stopwatch();
            _cpuTimer = new Stopwatch();
            _frameStartTime = 0.0;
            _texturesUsedThisFrame = new HashSet<IntPtr>();
            _videoMemoryUsed = 0;

            // Query GPU timestamp period for accurate GPU timing
            // Based on Vulkan API: vkGetPhysicalDeviceProperties -> properties.limits.timestampPeriod
            // The timestamp period is in nanoseconds per timestamp tick
            // Most GPUs have a period of 1.0 (1 nanosecond per tick), but some older GPUs may have different values
            _gpuTimestampPeriod = 1.0; // Default to 1 ns per tick (will be queried from device properties if available)
            _gpuTimestampsSupported = true; // Assume supported unless device properties indicate otherwise

            _initialized = true;
            return true;
        }

        public void Shutdown()
        {
            if (!_initialized)
            {
                return;
            }

            if (_device != null)
            {
                _device.Dispose();
                _device = null;
            }

            // Clean up frame statistics tracking
            if (_frameTimer != null)
            {
                _frameTimer.Stop();
            }
            if (_cpuTimer != null)
            {
                _cpuTimer.Stop();
            }
            if (_texturesUsedThisFrame != null)
            {
                _texturesUsedThisFrame.Clear();
            }

            _initialized = false;
        }

        public void BeginFrame()
        {
            if (!_initialized)
            {
                return;
            }

            // Reset frame statistics for new frame
            _lastFrameStats = new FrameStatistics();
            _texturesUsedThisFrame.Clear();
            _videoMemoryUsed = 0;
            _lastFrameStats.RaytracingTimeMs = 0.0;

            // Start frame timing
            // Frame time will be calculated in EndFrame (measured from start to end)
            // CPU time is measured for CPU-side work during the frame
            _frameStartTime = Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency * 1000.0; // Convert to milliseconds
            _frameTimer.Restart();
            _cpuTimer.Restart();

            // TODO: STUB - Begin frame rendering
            // When fully implemented, this should:
            // - Acquire next swap chain image
            // - Begin command buffer recording
            // - Insert GPU timestamp at start of frame (vkCmdWriteTimestamp with VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT)
        }

        public void EndFrame()
        {
            if (!_initialized)
            {
                return;
            }

            // Stop CPU timer (measures CPU-side work during frame)
            _cpuTimer.Stop();
            _lastFrameStats.CpuTimeMs = _cpuTimer.Elapsed.TotalMilliseconds;

            // Calculate frame time (wall-clock time from start to end of frame)
            double frameEndTime = Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency * 1000.0; // Convert to milliseconds
            _lastFrameStats.FrameTimeMs = frameEndTime - _frameStartTime;

            // Finalize frame statistics
            _lastFrameStats.TexturesUsed = _texturesUsedThisFrame.Count;
            _lastFrameStats.VideoMemoryUsed = _videoMemoryUsed;

            // Stop frame timer
            _frameTimer.Stop();

            // GPU time will be calculated from GPU timestamp queries when they are resolved
            // TODO: STUB - For now, GPU time is estimated as frame time minus CPU time (not accurate but provides a baseline)
            // TODO: STUB -  When GPU timestamp queries are fully implemented, this will be replaced with actual GPU timing
            // Note: This estimation assumes GPU and CPU work is sequential, which is not always true
            // Actual GPU timestamps will provide accurate GPU-only execution time
            if (_lastFrameStats.FrameTimeMs > _lastFrameStats.CpuTimeMs)
            {
                _lastFrameStats.GpuTimeMs = _lastFrameStats.FrameTimeMs - _lastFrameStats.CpuTimeMs;
            }
            else
            {
                _lastFrameStats.GpuTimeMs = 0.0;
            }

            // TODO: STUB - End frame and present
            // When fully implemented, this should:
            // - Insert GPU timestamp at end of frame (vkCmdWriteTimestamp with VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT)
            // - End command buffer recording
            // - Submit command buffer to queue
            // - Present swap chain image
            // - Resolve GPU timestamp queries from previous frame (vkGetQueryPoolResults)
            // - Calculate actual GPU time from resolved timestamps using ResolveGpuTimestamps()
        }

        public void Resize(int width, int height)
        {
            if (!_initialized)
            {
                return;
            }

            _settings.Width = width;
            _settings.Height = height;
            // TODO: STUB - Resize swap chain
        }

        public IntPtr CreateTexture(RuntimeTextureDescription desc)
        {
            if (!_initialized)
            {
                return IntPtr.Zero;
            }

            // TODO: STUB - Create Vulkan texture
            return IntPtr.Zero;
        }

        public bool UploadTextureData(IntPtr handle, TextureUploadData data)
        {
            if (!_initialized)
            {
                return false;
            }

            // TODO: STUB - Upload texture data
            return false;
        }

        public IntPtr CreateBuffer(RuntimeBufferDescription desc)
        {
            if (!_initialized)
            {
                return IntPtr.Zero;
            }

            // TODO: STUB - Create Vulkan buffer
            return IntPtr.Zero;
        }

        public IntPtr CreatePipeline(RuntimePipelineDescription desc)
        {
            if (!_initialized)
            {
                return IntPtr.Zero;
            }

            // TODO: STUB - Create Vulkan pipeline
            return IntPtr.Zero;
        }

        public void DestroyResource(IntPtr handle)
        {
            if (!_initialized)
            {
                return;
            }

            // TODO: STUB - Destroy Vulkan resource
        }

        public void SetRaytracingLevel(RaytracingLevel level)
        {
            // TODO: STUB - Set raytracing level
        }

        public FrameStatistics GetFrameStatistics()
        {
            if (!_initialized)
            {
                return new FrameStatistics();
            }

            // Return the last frame's statistics
            // Statistics are accumulated during BeginFrame/EndFrame and draw operations
            return _lastFrameStats;
        }

        public IDevice GetDevice()
        {
            return _device;
        }

        public void Dispose()
        {
            Shutdown();
        }

        /// <summary>
        /// Creates a Vulkan logical device and retrieves queue handles.
        /// Based on Vulkan API: https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateDevice.html
        /// </summary>
        private bool CreateVulkanDevice(
            IntPtr instance,
            IntPtr physicalDevice,
            uint graphicsQueueFamilyIndex,
            uint computeQueueFamilyIndex,
            uint transferQueueFamilyIndex,
            out IntPtr device,
            out IntPtr graphicsQueue,
            out IntPtr computeQueue,
            out IntPtr transferQueue,
            ref RuntimeGraphicsCapabilities capabilities)
        {
            device = IntPtr.Zero;
            graphicsQueue = IntPtr.Zero;
            computeQueue = IntPtr.Zero;
            transferQueue = IntPtr.Zero;

            try
            {
                // Get required function pointers from VulkanDevice
                // These should already be loaded by CreateVulkanInstance
                System.Reflection.FieldInfo vkCreateDeviceField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField("vkCreateDevice", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                System.Reflection.FieldInfo vkGetDeviceQueueField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField("vkGetDeviceQueue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                System.Reflection.FieldInfo vkGetPhysicalDeviceFeaturesField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField("vkGetPhysicalDeviceFeatures", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                System.Reflection.FieldInfo vkEnumerateDeviceExtensionPropertiesField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField("vkEnumerateDeviceExtensionProperties", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (vkCreateDeviceField == null || vkGetDeviceQueueField == null || vkGetPhysicalDeviceFeaturesField == null)
                {
                    return false;
                }

                // Get function delegates
                object vkCreateDeviceObj = vkCreateDeviceField.GetValue(null);
                object vkGetDeviceQueueObj = vkGetDeviceQueueField.GetValue(null);
                object vkGetPhysicalDeviceFeaturesObj = vkGetPhysicalDeviceFeaturesField.GetValue(null);

                if (vkCreateDeviceObj == null || vkGetDeviceQueueObj == null || vkGetPhysicalDeviceFeaturesObj == null)
                {
                    return false;
                }

                // Convert RuntimeGraphicsCapabilities to Game GraphicsCapabilities for the method call
                Andastra.Game.Graphics.MonoGame.Interfaces.GraphicsCapabilities gameCapabilitiesForDevice = new Andastra.Game.Graphics.MonoGame.Interfaces.GraphicsCapabilities
                {
                    SupportsRaytracing = capabilities.SupportsRaytracing,
                    MaxTextureSize = capabilities.MaxTextureSize,
                    MaxAnisotropy = capabilities.MaxAnisotropy
                };

                // Call public static method in VulkanDevice
                bool result = Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice.CreateVulkanDeviceInternal(
                    instance,
                    physicalDevice,
                    graphicsQueueFamilyIndex,
                    computeQueueFamilyIndex,
                    transferQueueFamilyIndex,
                    out device,
                    out graphicsQueue,
                    out computeQueue,
                    out transferQueue,
                    ref gameCapabilitiesForDevice);

                // Update capabilities from the method call
                capabilities.SupportsRaytracing = gameCapabilitiesForDevice.SupportsRaytracing;
                capabilities.MaxTextureSize = gameCapabilitiesForDevice.MaxTextureSize;
                capabilities.MaxAnisotropy = gameCapabilitiesForDevice.MaxAnisotropy;

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Frame Statistics Tracking Helpers

        /// <summary>
        /// Tracks a draw call and triangle count for frame statistics.
        /// Called from draw methods (Draw, DrawIndexed, DrawIndirect, etc.).
        /// Based on pattern used in other graphics backends (Direct3D11Backend, OpenGLBackend).
        /// </summary>
        /// <param name="triangleCount">Number of triangles rendered in this draw call.</param>
        internal void TrackDrawCall(int triangleCount)
        {
            if (!_initialized)
            {
                return;
            }

            _lastFrameStats.DrawCalls++;
            _lastFrameStats.TrianglesRendered += triangleCount;
        }

        /// <summary>
        /// Tracks texture usage for frame statistics.
        /// Called when a texture is bound to a texture slot.
        /// Based on pattern: Track unique textures used per frame.
        /// </summary>
        /// <param name="textureHandle">Handle to the texture being used.</param>
        internal void TrackTextureUsage(IntPtr textureHandle)
        {
            if (!_initialized || textureHandle == IntPtr.Zero)
            {
                return;
            }

            _texturesUsedThisFrame.Add(textureHandle);
        }

        /// <summary>
        /// Tracks video memory allocation for frame statistics.
        /// Called when resources (textures, buffers) are created or destroyed.
        /// </summary>
        /// <param name="bytes">Number of bytes allocated (positive) or deallocated (negative).</param>
        internal void TrackVideoMemory(long bytes)
        {
            if (!_initialized)
            {
                return;
            }

            _videoMemoryUsed += bytes;
            if (_videoMemoryUsed < 0)
            {
                _videoMemoryUsed = 0;
            }
        }

        /// <summary>
        /// Tracks raytracing time for frame statistics.
        /// Called when raytracing operations complete.
        /// </summary>
        /// <param name="timeMs">Time spent in raytracing operations in milliseconds.</param>
        internal void TrackRaytracingTime(double timeMs)
        {
            if (!_initialized)
            {
                return;
            }

            _lastFrameStats.RaytracingTimeMs += timeMs;
        }

        /// <summary>
        /// Updates GPU timestamp period from device properties.
        /// Based on Vulkan API: vkGetPhysicalDeviceProperties -> properties.limits.timestampPeriod
        /// Called during initialization or when device properties are queried.
        /// </summary>
        /// <param name="timestampPeriod">GPU timestamp period in nanoseconds per timestamp tick.</param>
        internal void UpdateGpuTimestampPeriod(double timestampPeriod)
        {
            _gpuTimestampPeriod = timestampPeriod > 0.0 ? timestampPeriod : 1.0;
        }

        /// <summary>
        /// Resolves GPU timestamp queries and updates GPU time in frame statistics.
        /// Based on Vulkan API: vkGetQueryPoolResults to retrieve timestamp values,
        /// then calculate delta time using timestamp period.
        /// Should be called in EndFrame after command buffer submission.
        /// </summary>
        /// <param name="startTimestamp">GPU timestamp at frame start (in timestamp ticks).</param>
        /// <param name="endTimestamp">GPU timestamp at frame end (in timestamp ticks).</param>
        internal void ResolveGpuTimestamps(ulong startTimestamp, ulong endTimestamp)
        {
            if (!_initialized || !_gpuTimestampsSupported || startTimestamp == 0 || endTimestamp == 0)
            {
                return;
            }

            // Calculate GPU time: (endTimestamp - startTimestamp) * timestampPeriod (nanoseconds) / 1,000,000 (convert to milliseconds)
            if (endTimestamp > startTimestamp)
            {
                ulong deltaTicks = endTimestamp - startTimestamp;
                double gpuTimeNs = deltaTicks * _gpuTimestampPeriod;
                _lastFrameStats.GpuTimeMs = gpuTimeNs / 1000000.0; // Convert nanoseconds to milliseconds
            }
            else
            {
                // Handle timestamp wrap-around (64-bit timestamps wrap after ~584 years at 1ns resolution, unlikely but handle it)
                ulong deltaTicks = (ulong.MaxValue - startTimestamp) + endTimestamp;
                double gpuTimeNs = deltaTicks * _gpuTimestampPeriod;
                _lastFrameStats.GpuTimeMs = gpuTimeNs / 1000000.0;
            }
        }

        #endregion
    }
}

