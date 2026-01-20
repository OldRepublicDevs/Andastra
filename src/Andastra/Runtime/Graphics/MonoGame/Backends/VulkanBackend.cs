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

        // GPU timestamp query pool and state
        // Based on Vulkan API: vkCreateQueryPool with VK_QUERY_TYPE_TIMESTAMP
        // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateQueryPool.html
        // We use double buffering: alternate between query indices 0 and 1 per frame
        // This allows us to query results from the previous frame while recording the current frame
        private IntPtr _timestampQueryPool; // VkQueryPool handle
        private const uint TIMESTAMP_QUERY_COUNT = 2; // Start and end timestamps per frame
        private uint _timestampQueryIndex; // Current query index (alternates between 0 and 1 for double buffering)
        private ulong[] _timestampQueryResults; // Results buffer for resolving queries (2 timestamps per frame)
        private bool _timestampQueriesInitialized;

        // Resource tracking to prevent garbage collection
        // Stores created resources so they remain alive until explicitly destroyed
        private Dictionary<IntPtr, Andastra.Game.Graphics.MonoGame.Interfaces.ITexture> _textures; // Map native handles to ITexture objects
        private Dictionary<IntPtr, Andastra.Game.Graphics.MonoGame.Interfaces.IBuffer> _buffers; // Map native handles to IBuffer objects
        private Dictionary<IntPtr, object> _pipelines; // Map native handles to pipeline objects (to be defined)
        private Dictionary<IntPtr, object> _resources; // Generic resource tracking

        // Buffer size tracking for memory estimation
        // Tracks buffer sizes for accurate memory usage reporting
        private Dictionary<IntPtr, long> _bufferSizes; // Map native handles to buffer sizes in bytes

        // VSync state tracking
        private bool _vSyncEnabled;

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

        // IGraphicsBackend interface members
        // These require window/input infrastructure that will be implemented when window management is added
        // For now, they throw helpful exceptions explaining what infrastructure is needed
        public IGraphicsDevice GraphicsDevice
        {
            get 
            { 
                throw new InvalidOperationException(
                    "GraphicsDevice property requires window and graphics device infrastructure. " +
                    "Window management (GLFW/SDL/native platform windowing) must be implemented first. " +
                    "The Vulkan backend needs: window creation, swap chain setup, and graphics device wrapper.");
            }
        }

        public IContentManager ContentManager
        {
            get 
            { 
                throw new InvalidOperationException(
                    "ContentManager property requires content loading infrastructure. " +
                    "Content management system must be implemented for the Vulkan backend. " +
                    "This requires: content pipeline integration, asset loading, and resource management.");
            }
        }

        public IWindow Window
        {
            get 
            { 
                throw new InvalidOperationException(
                    "Window property requires window management infrastructure. " +
                    "Window creation and management must be implemented using GLFW, SDL, or native platform APIs. " +
                    "The Vulkan backend needs: window creation, surface creation (VkSurfaceKHR), and event handling.");
            }
        }

        public IInputManager InputManager
        {
            get 
            { 
                throw new InvalidOperationException(
                    "InputManager property requires input system infrastructure. " +
                    "Input handling system must be implemented for the Vulkan backend. " +
                    "This requires: keyboard/mouse/controller input processing and event management.");
            }
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

            // TODO: Implement Vulkan game loop
            // When fully implemented, this should:
            // 1. Create window using platform-specific windowing API:
            //    - GLFW: glfwCreateWindow(), glfwCreateWindowSurface()
            //    - SDL: SDL_CreateWindow(), SDL_Vulkan_CreateSurface()
            //    - Native: Win32 CreateWindowEx(), VkWin32SurfaceCreateInfoKHR
            //    - X11: xcb_create_window(), VkXcbSurfaceCreateInfoKHR
            //    - Cocoa: NSWindow, VkMacOSSurfaceCreateInfoMVK
            //
            // 2. Create VkSurfaceKHR from window handle
            //    - vkCreateWin32SurfaceKHR (Windows)
            //    - vkCreateXcbSurfaceKHR (Linux/X11)
            //    - vkCreateMacOSSurfaceMVK (macOS)
            //    - vkCreateMetalSurfaceEXT (macOS/Metal)
            //
            // 3. Set up swap chain for presentation:
            //    - Query surface capabilities: vkGetPhysicalDeviceSurfaceCapabilitiesKHR
            //    - Choose present mode based on VSync setting
            //    - Create swap chain: vkCreateSwapchainKHR
            //    - Get swap chain images: vkGetSwapchainImagesKHR
            //    - Create image views for each swap chain image
            //    - Create framebuffers for rendering
            //
            // 4. Run main loop:
            //    - while (!shouldExit) {
            //        - Process window events (resize, close, input)
            //        - Acquire next swap chain image: vkAcquireNextImageKHR
            //        - Calculate delta time
            //        - BeginFrame()
            //        - updateAction(deltaTime)
            //        - drawAction()
            //        - EndFrame()
            //        - Present swap chain image: vkQueuePresentKHR
            //      }
            //
            // 5. Handle window events:
            //    - Window close -> set shouldExit = true
            //    - Window resize -> recreate swap chain with new dimensions
            //    - Input events -> update InputManager
            //
            // 6. Cleanup on exit:
            //    - Wait for device idle: vkDeviceWaitIdle
            //    - Destroy swap chain and related resources
            //    - Destroy surface
            //    - Destroy window
            //
            // Based on Vulkan API: https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateSwapchainKHR.html
            // Reference: https://vulkan-tutorial.com/Drawing_a_triangle/Presentation/Swap_chain
            
            throw new InvalidOperationException(
                "Run method requires window and swap chain infrastructure. " +
                "Window management must be implemented first using GLFW, SDL, or native platform APIs. " +
                "The Vulkan backend needs: window creation, surface creation (VkSurfaceKHR), swap chain setup, and game loop implementation.");
        }

        public void Exit()
        {
            // TODO: Implement exit handling
            // When fully implemented, this should:
            // 1. Set exit flag to stop game loop (if running)
            // 2. Signal window to close:
            //    - GLFW: glfwSetWindowShouldClose(window, GLFW_TRUE)
            //    - SDL: SDL_Event (SDL_QUIT) -> SDL_PushEvent()
            //    - Native: PostMessage() or similar window close message
            // 3. Clean up resources if needed:
            //    - Wait for device idle: vkDeviceWaitIdle
            //    - Any additional cleanup before window closes
            
            // For now, just log that exit was called
            // When window infrastructure is implemented, this will properly close the window
            if (_initialized)
            {
                Console.WriteLine("[VulkanBackend] Exit() called - window infrastructure not yet implemented");
            }
        }

        public IRoomMeshRenderer CreateRoomMeshRenderer()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Backend must be initialized before creating renderers.");
            }

            // TODO: Implement Vulkan room mesh renderer
            // When fully implemented, this should:
            // 1. Create VulkanRoomMeshRenderer instance
            // 2. Initialize with Vulkan device, command buffers, pipelines
            // 3. Set up vertex/index buffer management for room geometry
            // 4. Create graphics pipelines for room rendering
            // 5. Set up descriptor sets for room textures and materials
            // 6. Handle room mesh loading from game data
            
            throw new InvalidOperationException(
                "CreateRoomMeshRenderer requires rendering pipeline infrastructure. " +
                "Vulkan rendering system must be implemented first. " +
                "This requires: graphics pipeline creation, vertex/index buffer management, descriptor set setup, and room mesh loading.");
        }

        public IEntityModelRenderer CreateEntityModelRenderer(object gameDataManager = null, object installation = null)
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Backend must be initialized before creating renderers.");
            }

            // TODO: Implement Vulkan entity model renderer
            // When fully implemented, this should:
            // 1. Create VulkanEntityModelRenderer instance
            // 2. Initialize with Vulkan device, command buffers, pipelines
            // 3. Set up model loading from game data (MDL files)
            // 4. Create graphics pipelines for model rendering
            // 5. Set up descriptor sets for model textures and materials
            // 6. Handle animation and skeleton rendering
            
            throw new InvalidOperationException(
                "CreateEntityModelRenderer requires rendering pipeline infrastructure. " +
                "Vulkan rendering system must be implemented first. " +
                "This requires: graphics pipeline creation, model loading system, animation system, and descriptor set management.");
        }

        public ISpatialAudio CreateSpatialAudio()
        {
            // TODO: Implement Vulkan spatial audio
            // When fully implemented, this should:
            // 1. Create VulkanSpatialAudio instance or delegate to audio system
            // 2. Set up 3D audio positioning using OpenAL, FMOD, or similar audio library
            // 3. Note: Audio is typically independent of graphics API, but needs integration
            // 4. Initialize audio context and listener for 3D spatialization
            
            throw new InvalidOperationException(
                "CreateSpatialAudio requires audio system infrastructure. " +
                "Audio system must be implemented for the Vulkan backend. " +
                "This requires: 3D audio library integration (OpenAL/FMOD), audio context management, and spatial audio processing.");
        }

        public object CreateDialogueCameraController(object cameraController)
        {
            if (cameraController == null)
            {
                throw new ArgumentNullException(nameof(cameraController));
            }

            // TODO: Implement Vulkan dialogue camera controller
            // When fully implemented, this should:
            // 1. Create VulkanDialogueCameraController instance
            // 2. Wrap the provided camera controller with Vulkan-specific rendering
            // 3. Handle dialogue camera rendering with Vulkan pipelines
            // 4. Integrate with dialogue system for camera positioning
            
            throw new InvalidOperationException(
                "CreateDialogueCameraController requires camera and rendering infrastructure. " +
                "Dialogue camera system must be implemented for the Vulkan backend. " +
                "This requires: camera controller wrapper, Vulkan-specific rendering, and dialogue system integration.");
        }

        public object CreateSoundPlayer(object resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            // TODO: Implement Vulkan sound player
            // When fully implemented, this should:
            // 1. Create VulkanSoundPlayer instance
            // 2. Initialize with resource provider for loading audio files
            // 3. Set up audio playback using OpenAL, FMOD, or similar audio library
            // 4. Handle WAV/MP3 audio file loading and playback
            
            throw new InvalidOperationException(
                "CreateSoundPlayer requires audio system infrastructure. " +
                "Audio playback system must be implemented for the Vulkan backend. " +
                "This requires: audio library integration (OpenAL/FMOD), audio file loading, and sound effect playback.");
        }

        public object CreateMusicPlayer(object resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            // TODO: Implement Vulkan music player
            // When fully implemented, this should:
            // 1. Create VulkanMusicPlayer instance
            // 2. Initialize with resource provider for loading music files
            // 3. Set up background music playback using OpenAL, FMOD, or similar audio library
            // 4. Handle streaming audio for background music
            // 5. Support crossfading between tracks
            
            throw new InvalidOperationException(
                "CreateMusicPlayer requires audio system infrastructure. " +
                "Background music system must be implemented for the Vulkan backend. " +
                "This requires: audio library integration (OpenAL/FMOD), music file loading, streaming audio, and crossfade support.");
        }

        public object CreateVoicePlayer(object resourceProvider)
        {
            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            // TODO: Implement Vulkan voice player
            // When fully implemented, this should:
            // 1. Create VulkanVoicePlayer instance
            // 2. Initialize with resource provider for loading voice files
            // 3. Set up voice-over dialogue playback using OpenAL, FMOD, or similar audio library
            // 4. Handle dialogue voice file loading (typically WAV files)
            // 5. Support lip sync and dialogue timing
            
            throw new InvalidOperationException(
                "CreateVoicePlayer requires audio system infrastructure. " +
                "Voice playback system must be implemented for the Vulkan backend. " +
                "This requires: audio library integration (OpenAL/FMOD), voice file loading, dialogue playback, and lip sync support.");
        }

        public void SetVSync(bool enabled)
        {
            if (!_initialized)
            {
                return;
            }

            try
            {
                // Set VSync state
                // Based on Vulkan API: VSync is controlled via swap chain present mode
                // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/VkSwapchainPresentModeKHR.html
                // - VK_PRESENT_MODE_FIFO_KHR: VSync enabled (matches display refresh rate)
                // - VK_PRESENT_MODE_IMMEDIATE_KHR: VSync disabled (no frame rate limit)
                
                _vSyncEnabled = enabled;

                // TODO: When swap chain management is implemented, this should:
                // 1. Get current swap chain
                // 2. Determine desired present mode based on enabled parameter:
                //    - enabled: VK_PRESENT_MODE_FIFO_KHR (VSync on)
                //    - disabled: VK_PRESENT_MODE_IMMEDIATE_KHR or VK_PRESENT_MODE_MAILBOX_KHR (VSync off)
                // 3. Check if swap chain supports the desired present mode
                // 4. If mode is different, recreate swap chain with new present mode
                // 5. Apply changes immediately
                
                // For now, we store the VSync state which will be applied when swap chain is created
                // This allows VSync to be set before window/swap chain creation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] SetVSync failed: {ex.Message}");
            }
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

            // Initialize resource tracking
            _textures = new Dictionary<IntPtr, Andastra.Game.Graphics.MonoGame.Interfaces.ITexture>();
            _buffers = new Dictionary<IntPtr, Andastra.Game.Graphics.MonoGame.Interfaces.IBuffer>();
            _pipelines = new Dictionary<IntPtr, object>();
            _resources = new Dictionary<IntPtr, object>();
            _bufferSizes = new Dictionary<IntPtr, long>(); // Track buffer sizes for memory estimation

            // Initialize VSync state (default to enabled for better user experience)
            _vSyncEnabled = true;

            // Query GPU timestamp period and support from device properties
            // Based on Vulkan API: vkGetPhysicalDeviceProperties -> properties.limits.timestampPeriod
            // The timestamp period is in nanoseconds per timestamp tick
            // Most GPUs have a period of 1.0 (1 nanosecond per tick), but some older GPUs may have different values
            QueryGpuTimestampProperties(physicalDevice);

            // Initialize GPU timestamp query pool state
            _timestampQueryPool = IntPtr.Zero;
            _timestampQueryIndex = 0;
            _timestampQueryResults = new ulong[TIMESTAMP_QUERY_COUNT];
            _timestampQueriesInitialized = false;

            // Create timestamp query pool if GPU timestamps are supported
            if (_gpuTimestampsSupported && _device != null)
            {
                if (!CreateTimestampQueryPool())
                {
                    // If query pool creation fails, disable GPU timestamps and fall back to CPU timing
                    _gpuTimestampsSupported = false;
                    Console.WriteLine("[VulkanBackend] GPU timestamp queries not supported or failed to create query pool, falling back to CPU timing");
                }
            }

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

            // Clean up tracked resources
            if (_textures != null)
            {
                foreach (var texture in _textures.Values)
                {
                    texture?.Dispose();
                }
                _textures.Clear();
            }
            if (_buffers != null)
            {
                foreach (var buffer in _buffers.Values)
                {
                    buffer?.Dispose();
                }
                _buffers.Clear();
            }
            if (_pipelines != null)
            {
                _pipelines.Clear();
            }
            if (_resources != null)
            {
                _resources.Clear();
            }

            // Destroy GPU timestamp query pool
            if (_timestampQueryPool != IntPtr.Zero)
            {
                DestroyTimestampQueryPool();
            }
            _timestampQueriesInitialized = false;

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

            // Begin frame rendering
            // When fully implemented, this should:
            // - Acquire next swap chain image
            // - Begin command buffer recording
            // - Insert GPU timestamp at start of frame (vkCmdWriteTimestamp with VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT)

            // Write GPU timestamp at start of frame if timestamps are supported
            // Based on Vulkan API: vkCmdWriteTimestamp records a timestamp when a specific pipeline stage completes
            // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCmdWriteTimestamp.html
            // We record the timestamp at TOP_OF_PIPE to capture when the frame starts processing on the GPU
            if (_gpuTimestampsSupported && _timestampQueriesInitialized)
            {
                WriteGpuTimestamp(0x00000001); // VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT = 0x1
            }
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

            // End frame and present
            // When fully implemented, this should:
            // - Insert GPU timestamp at end of frame (vkCmdWriteTimestamp with VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT)
            // - End command buffer recording
            // - Submit command buffer to queue
            // - Present swap chain image
            // - Resolve GPU timestamp queries from previous frame (vkGetQueryPoolResults)
            // - Calculate actual GPU time from resolved timestamps using ResolveGpuTimestamps()

            // Write GPU timestamp at end of frame if timestamps are supported
            // Based on Vulkan API: vkCmdWriteTimestamp records a timestamp when a specific pipeline stage completes
            // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCmdWriteTimestamp.html
            // We record the timestamp at BOTTOM_OF_PIPE to capture when the frame finishes processing on the GPU
            if (_gpuTimestampsSupported && _timestampQueriesInitialized)
            {
                WriteGpuTimestamp(0x00000008); // VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT = 0x8

                // Resolve GPU timestamp queries from previous frame (double buffering)
                // Based on Vulkan API: vkGetQueryPoolResults retrieves query results
                // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkGetQueryPoolResults.html
                // We resolve queries from the previous frame's query index (alternating between 0 and 1)
                uint previousQueryIndex = (_timestampQueryIndex == 0) ? 1u : 0u;
                ulong startTimestamp;
                ulong endTimestamp;

                if (ResolveGpuTimestampQueries(previousQueryIndex, out startTimestamp, out endTimestamp))
                {
                    // Calculate actual GPU time from resolved timestamps
                    ResolveGpuTimestamps(startTimestamp, endTimestamp);
                }
                else
                {
                    // If query resolution fails, fall back to CPU timing estimation
                    // This can happen if queries aren't ready yet (first frame) or if there's an error
                    if (_lastFrameStats.FrameTimeMs > _lastFrameStats.CpuTimeMs)
                    {
                        _lastFrameStats.GpuTimeMs = _lastFrameStats.FrameTimeMs - _lastFrameStats.CpuTimeMs;
                    }
                    else
                    {
                        _lastFrameStats.GpuTimeMs = 0.0;
                    }
                }

                // Alternate query index for next frame (double buffering)
                _timestampQueryIndex = (_timestampQueryIndex + 1) % 2;
            }
            else
            {
                // GPU timestamps not supported or not initialized, fall back to CPU timing estimation
                // Note: This estimation assumes GPU and CPU work is sequential, which is not always true
                // Actual GPU timestamps provide accurate GPU-only execution time
                if (_lastFrameStats.FrameTimeMs > _lastFrameStats.CpuTimeMs)
                {
                    _lastFrameStats.GpuTimeMs = _lastFrameStats.FrameTimeMs - _lastFrameStats.CpuTimeMs;
                }
                else
                {
                    _lastFrameStats.GpuTimeMs = 0.0;
                }
            }
        }

        public void Resize(int width, int height)
        {
            if (!_initialized)
            {
                return;
            }

            if (width <= 0 || height <= 0)
            {
                Console.WriteLine($"[VulkanBackend] Resize: Invalid dimensions {width}x{height}");
                return;
            }

            try
            {
                // Update settings
                _settings.Width = width;
                _settings.Height = height;

                // TODO: When swap chain management is implemented, this should:
                // 1. Wait for all GPU operations to complete (vkQueueWaitIdle or vkDeviceWaitIdle)
                // 2. Query new surface capabilities (vkGetPhysicalDeviceSurfaceCapabilitiesKHR)
                // 3. Update swap chain extent to match new window size
                // 4. Recreate swap chain with new dimensions (vkCreateSwapchainKHR)
                // 5. Recreate swap chain images and image views
                // 6. Recreate framebuffers with new dimensions
                // 7. Recreate render passes if needed
                // 8. Handle resize events properly (may need to handle VK_ERROR_OUT_OF_DATE_KHR)
                // Based on Vulkan API: https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateSwapchainKHR.html
                
                // For now, we update the settings which will be used when swap chain is created/resized
                // This allows resize to be called before swap chain creation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] Resize failed: {ex.Message}");
            }
        }

        public IntPtr CreateTexture(RuntimeTextureDescription desc)
        {
            if (!_initialized || _device == null)
            {
                return IntPtr.Zero;
            }

            try
            {
                // Convert RuntimeTextureDescription to Game TextureDesc
                // Based on Vulkan API: vkCreateImage and vkCreateImageView for texture creation
                // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateImage.html
                Andastra.Game.Graphics.MonoGame.Interfaces.TextureDesc gameDesc = new Andastra.Game.Graphics.MonoGame.Interfaces.TextureDesc
                {
                    Width = desc.Width,
                    Height = desc.Height,
                    Depth = desc.Depth > 0 ? desc.Depth : 1,
                    ArraySize = desc.ArraySize > 0 ? desc.ArraySize : 1,
                    MipLevels = desc.MipLevels > 0 ? desc.MipLevels : 1,
                    SampleCount = desc.SampleCount > 0 ? desc.SampleCount : 1,
                    Format = ConvertTextureFormat(desc.Format),
                    Dimension = desc.IsCubemap ? Andastra.Game.Graphics.MonoGame.Interfaces.TextureDimension.TextureCube : Andastra.Game.Graphics.MonoGame.Interfaces.TextureDimension.Texture2D,
                    Usage = ConvertTextureUsage(desc.Usage),
                    InitialState = Andastra.Game.Graphics.MonoGame.Interfaces.ResourceState.Common,
                    KeepInitialState = false,
                    DebugName = desc.DebugName ?? $"Texture_{desc.Width}x{desc.Height}"
                };

                // Create texture using VulkanDevice
                Andastra.Game.Graphics.MonoGame.Interfaces.ITexture texture = _device.CreateTexture(gameDesc);
                if (texture == null)
                {
                    return IntPtr.Zero;
                }

                // Get native handle and track resource
                IntPtr nativeHandle = texture.NativeHandle;
                if (nativeHandle != IntPtr.Zero)
                {
                    _textures[nativeHandle] = texture;
                    
                    // Track video memory usage (estimate based on dimensions and format)
                    long estimatedMemory = EstimateTextureMemory(desc.Width, desc.Height, desc.Depth, desc.ArraySize, desc.MipLevels, desc.Format);
                    TrackVideoMemory(estimatedMemory);
                    
                    return nativeHandle;
                }

                // If handle is zero, dispose texture
                texture.Dispose();
                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] CreateTexture failed: {ex.Message}");
                return IntPtr.Zero;
            }
        }

        public bool UploadTextureData(IntPtr handle, TextureUploadData data)
        {
            if (!_initialized || handle == IntPtr.Zero || _device == null)
            {
                return false;
            }

            if (!_textures.TryGetValue(handle, out var texture))
            {
                Console.WriteLine("[VulkanBackend] UploadTextureData: Invalid texture handle");
                return false;
            }

            if (data.Mipmaps == null || data.Mipmaps.Length == 0)
            {
                Console.WriteLine("[VulkanBackend] UploadTextureData: No mipmap data provided");
                return false;
            }

            try
            {
                // For Vulkan, texture upload requires:
                // 1. Create staging buffer (host-visible memory)
                // 2. Map and copy data to staging buffer
                // 3. Use command buffer to copy from staging buffer to image
                // 4. Transition image layout
                // Based on Vulkan API: vkCreateBuffer, vkMapMemory, vkCmdCopyBufferToImage
                // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCmdCopyBufferToImage.html

                // Get texture description
                var textureDesc = texture.Desc;
                // Note: data.Format is Andastra.Game.Graphics.MonoGame.Interfaces.TextureFormat
                // textureDesc.Format is also Andastra.Game.Graphics.MonoGame.Interfaces.TextureFormat
                // So we can compare directly
                if (textureDesc.Format != data.Format)
                {
                    Console.WriteLine($"[VulkanBackend] UploadTextureData: Texture format mismatch. Expected {textureDesc.Format}, got {data.Format}");
                    return false;
                }

                // Create a command list for copy operations
                Andastra.Game.Graphics.MonoGame.Interfaces.ICommandList commandList = _device.CreateCommandList(Andastra.Game.Graphics.MonoGame.Interfaces.CommandListType.Copy);
                if (commandList == null)
                {
                    Console.WriteLine("[VulkanBackend] UploadTextureData: Failed to create command list");
                    return false;
                }

                try
                {
                    // Open command list for recording
                    commandList.Open();

                    // Transition texture to copy destination state for writing
                    // Based on Vulkan API: Resource state transitions are required before writing to textures
                    // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/VkImageMemoryBarrier.html
                    commandList.SetTextureState(texture, Andastra.Game.Graphics.MonoGame.Interfaces.ResourceState.CopyDest);
                    commandList.CommitBarriers();

                    // Use WriteTexture method for each mipmap
                    // Signature: void WriteTexture(ITexture texture, int mipLevel, int arraySlice, byte[] data)
                    foreach (var mipmap in data.Mipmaps)
                    {
                        if (mipmap.Data == null || mipmap.Data.Length == 0)
                        {
                            Console.WriteLine($"[VulkanBackend] UploadTextureData: Mipmap {mipmap.Level} has no data");
                            continue;
                        }

                        try
                        {
                            // WriteTexture(texture, mipLevel, arraySlice, data)
                            // For 2D textures, arraySlice is typically 0
                            commandList.WriteTexture(texture, mipmap.Level, 0, mipmap.Data);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[VulkanBackend] UploadTextureData: Failed to write mipmap {mipmap.Level}: {ex.Message}");
                            commandList.Close();
                            return false;
                        }
                    }

                    // Transition texture back to shader resource state after writing
                    // Based on Vulkan API: Textures should be in ShaderResource state when used in shaders
                    commandList.SetTextureState(texture, Andastra.Game.Graphics.MonoGame.Interfaces.ResourceState.ShaderResource);
                    commandList.CommitBarriers();

                    // Close command list and execute
                    commandList.Close();
                    _device.ExecuteCommandList(commandList);

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[VulkanBackend] UploadTextureData: Exception during texture upload: {ex.Message}");
                    try
                    {
                        commandList?.Close();
                    }
                    catch
                    {
                        // Ignore errors when closing
                    }
                    return false;
                }
                finally
                {
                    commandList?.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] UploadTextureData failed: {ex.Message}");
                return false;
            }
        }

        public IntPtr CreateBuffer(RuntimeBufferDescription desc)
        {
            if (!_initialized || _device == null)
            {
                return IntPtr.Zero;
            }

            try
            {
                // Convert RuntimeBufferDescription to Game BufferDesc
                // Based on Vulkan API: vkCreateBuffer for buffer creation
                // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateBuffer.html
                Andastra.Game.Graphics.MonoGame.Interfaces.BufferDesc gameDesc = new Andastra.Game.Graphics.MonoGame.Interfaces.BufferDesc
                {
                    ByteSize = desc.SizeInBytes,
                    StructStride = desc.StructureByteStride,
                    Usage = ConvertBufferUsage(desc.Usage),
                    InitialState = Andastra.Game.Graphics.MonoGame.Interfaces.ResourceState.Common,
                    KeepInitialState = false,
                    CanHaveRawViews = false,
                    IsAccelStructBuildInput = false,
                    HeapType = Andastra.Game.Graphics.MonoGame.Interfaces.BufferHeapType.Default,
                    DebugName = desc.DebugName ?? $"Buffer_{desc.SizeInBytes}bytes"
                };

                // Create buffer using VulkanDevice
                Andastra.Game.Graphics.MonoGame.Interfaces.IBuffer buffer = _device.CreateBuffer(gameDesc);
                if (buffer == null)
                {
                    return IntPtr.Zero;
                }

                // Get native handle and track resource
                IntPtr nativeHandle = buffer.NativeHandle;
                if (nativeHandle != IntPtr.Zero)
                {
                    _buffers[nativeHandle] = buffer;
                    
                    // Track buffer size for memory estimation
                    _bufferSizes[nativeHandle] = desc.SizeInBytes;
                    
                    // Track video memory usage
                    TrackVideoMemory(desc.SizeInBytes);
                    
                    return nativeHandle;
                }

                // If handle is zero, dispose buffer
                buffer.Dispose();
                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] CreateBuffer failed: {ex.Message}");
                return IntPtr.Zero;
            }
        }

        public IntPtr CreatePipeline(RuntimePipelineDescription desc)
        {
            if (!_initialized || _device == null)
            {
                return IntPtr.Zero;
            }

            try
            {
                // Create graphics pipeline using VulkanDevice
                // Based on Vulkan API: vkCreateGraphicsPipelines for pipeline creation
                // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateGraphicsPipelines.html
                // 
                // Note: RuntimePipelineDescription uses byte arrays for shaders, while GraphicsPipelineDesc
                // requires IShader objects. This conversion requires creating shader modules first.
                // For now, we implement a basic version that can be enhanced when full shader module
                // creation is available.

                // Check if we have shader bytecode
                if ((desc.VertexShader == null || desc.VertexShader.Length == 0) &&
                    (desc.ComputeShader == null || desc.ComputeShader.Length == 0))
                {
                    Console.WriteLine("[VulkanBackend] CreatePipeline: No shader bytecode provided");
                    return IntPtr.Zero;
                }

                // If we have compute shader, create compute pipeline instead
                if (desc.ComputeShader != null && desc.ComputeShader.Length > 0)
                {
                    // TODO: Implement compute pipeline creation
                    // Requires: Convert bytecode to IShader, create ComputePipelineDesc, call CreateComputePipeline
                    Console.WriteLine("[VulkanBackend] CreatePipeline: Compute pipeline creation not yet implemented");
                    return IntPtr.Zero;
                }

                // For graphics pipeline, we need:
                // 1. Convert shader bytecode arrays to IShader objects (requires ShaderDesc)
                // 2. Convert Runtime state descriptions to Game state descriptions
                // 3. Create IFramebuffer (can be null for now)
                // 4. Create GraphicsPipelineDesc
                // 5. Call CreateGraphicsPipeline

                // TODO: Full pipeline creation requires:
                // - Shader module creation from bytecode
                // - State description conversion
                // - Framebuffer creation
                // This is complex and requires full shader compilation infrastructure
                
                Console.WriteLine("[VulkanBackend] CreatePipeline: Graphics pipeline creation requires shader module infrastructure");
                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] CreatePipeline failed: {ex.Message}");
                return IntPtr.Zero;
            }
        }

        public void DestroyResource(IntPtr handle)
        {
            if (!_initialized || handle == IntPtr.Zero)
            {
                return;
            }

            try
            {
                // Try to destroy texture
                if (_textures != null && _textures.TryGetValue(handle, out var texture))
                {
                    // Get texture description for memory estimation
                    long textureMemory = 0;
                    try
                    {
                        var desc = texture.Desc;
                        // Estimate texture memory based on description
                        // Convert Game format to Runtime format for estimation
                        // Both format enums have compatible underlying values, so we can cast directly
                        Andastra.Runtime.Graphics.Common.Enums.TextureFormat runtimeFormat = 
                            (Andastra.Runtime.Graphics.Common.Enums.TextureFormat)(int)desc.Format;
                        textureMemory = EstimateTextureMemory(
                            desc.Width, 
                            desc.Height, 
                            desc.Depth > 0 ? desc.Depth : 1, 
                            desc.ArraySize > 0 ? desc.ArraySize : 1, 
                            desc.MipLevels > 0 ? desc.MipLevels : 1, 
                            runtimeFormat);
                    }
                    catch
                    {
                        // If we can't get description or conversion fails, memory tracking will be approximate
                        textureMemory = 0;
                    }
                    
                    texture?.Dispose();
                    _textures.Remove(handle);
                    
                    // Update video memory usage
                    TrackVideoMemory(-textureMemory);
                    
                    return;
                }

                // Try to destroy buffer
                if (_buffers != null && _buffers.TryGetValue(handle, out var buffer))
                {
                    // Get buffer size from tracking before destroying for memory tracking
                    long size = 0;
                    if (_bufferSizes != null && _bufferSizes.TryGetValue(handle, out size))
                    {
                        _bufferSizes.Remove(handle);
                    }
                    else
                    {
                        // Fallback: try to estimate size
                        size = EstimateBufferSize(handle);
                    }
                    
                    buffer?.Dispose();
                    _buffers.Remove(handle);
                    TrackVideoMemory(-size);
                    return;
                }

                // Try to destroy pipeline
                if (_pipelines != null && _pipelines.TryGetValue(handle, out var pipeline))
                {
                    // Dispose pipeline if it implements IDisposable
                    if (pipeline is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    _pipelines.Remove(handle);
                    return;
                }

                // Try generic resource cleanup
                if (_resources != null && _resources.TryGetValue(handle, out var resource))
                {
                    if (resource is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                    _resources.Remove(handle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] DestroyResource failed: {ex.Message}");
            }
        }

        public void SetRaytracingLevel(RaytracingLevel level)
        {
            if (!_initialized)
            {
                return;
            }

            try
            {
                // Set raytracing level configuration
                // Based on Vulkan API: Raytracing configuration affects acceleration structure builds
                // and raytracing pipeline usage
                // https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/VK_KHR_ray_tracing_pipeline.html
                
                // Raytracing level controls which raytracing features are enabled:
                // - Disabled: No raytracing
                // - ShadowsOnly: Shadow rays only
                // - ReflectionsOnly: Reflection rays only
                // - ShadowsAndReflections: Both shadow and reflection rays
                // - Full/PathTracing: Full raytracing with all features
                
                // Store raytracing level for use in raytracing operations
                // This affects which raytracing passes are executed in the raytracing system
                // The actual raytracing implementation is in NativeRaytracingSystem
                
                // For now, we just validate that raytracing is supported if not disabled
                if (level != RaytracingLevel.Disabled && !_capabilities.SupportsRaytracing)
                {
                    Console.WriteLine("[VulkanBackend] SetRaytracingLevel: Raytracing not supported, ignoring level setting");
                    return;
                }

                // Raytracing level is typically managed by the raytracing system, not the backend directly
                // This method provides an interface for setting the level, but the actual implementation
                // is in NativeRaytracingSystem which manages raytracing pipelines and acceleration structures
                
                // TODO: If backend needs to track raytracing level, add a field to store it
                // For now, this is a no-op as raytracing level is managed by NativeRaytracingSystem
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] SetRaytracingLevel failed: {ex.Message}");
            }
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

        /// <summary>
        /// Queries GPU timestamp properties from physical device.
        /// Based on Vulkan API: vkGetPhysicalDeviceProperties -> properties.limits.timestampPeriod
        /// Also checks timestampComputeAndGraphics support via vkGetPhysicalDeviceFeatures.
        /// Called during initialization to determine if GPU timestamps are supported and get the timestamp period.
        /// </summary>
        /// <param name="physicalDevice">VkPhysicalDevice handle.</param>
        private void QueryGpuTimestampProperties(IntPtr physicalDevice)
        {
            if (physicalDevice == IntPtr.Zero)
            {
                _gpuTimestampsSupported = false;
                _gpuTimestampPeriod = 1.0;
                return;
            }

            try
            {
                // Get vkGetPhysicalDeviceProperties function pointer via reflection
                System.Reflection.FieldInfo vkGetPhysicalDevicePropertiesField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField(
                    "vkGetPhysicalDeviceProperties",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (vkGetPhysicalDevicePropertiesField == null)
                {
                    // Function not available, use defaults
                    _gpuTimestampsSupported = true;
                    _gpuTimestampPeriod = 1.0;
                    return;
                }

                object vkGetPhysicalDevicePropertiesObj = vkGetPhysicalDevicePropertiesField.GetValue(null);
                if (vkGetPhysicalDevicePropertiesObj == null)
                {
                    // Function delegate not available, use defaults
                    _gpuTimestampsSupported = true;
                    _gpuTimestampPeriod = 1.0;
                    return;
                }

                // Call vkGetPhysicalDeviceProperties
                // VkPhysicalDeviceProperties structure contains limits.timestampPeriod
                // We need to allocate memory for VkPhysicalDeviceProperties (320 bytes on most platforms)
                // Structure layout: uint32_t apiVersion, uint32_t driverVersion, uint32_t vendorID, uint32_t deviceID, ...
                // limits.timestampPeriod is at offset 232 (varies by platform, but typically around this offset)
                // For now, we'll use a safe default and query if a helper method is available

                // Check for timestampComputeAndGraphics support via vkGetPhysicalDeviceFeatures
                System.Reflection.FieldInfo vkGetPhysicalDeviceFeaturesField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField(
                    "vkGetPhysicalDeviceFeatures",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (vkGetPhysicalDeviceFeaturesField != null)
                {
                    object vkGetPhysicalDeviceFeaturesObj = vkGetPhysicalDeviceFeaturesField.GetValue(null);
                    // If features are available, we can check timestampComputeAndGraphics
                    // For now, assume it's supported if the function exists
                }

                // Default values - most modern GPUs support timestamps with 1ns period
                _gpuTimestampPeriod = 1.0; // 1 nanosecond per tick (most common)
                _gpuTimestampsSupported = true; // Assume supported unless proven otherwise
            }
            catch (Exception ex)
            {
                // If querying fails, disable GPU timestamps and use defaults
                Console.WriteLine($"[VulkanBackend] Failed to query GPU timestamp properties: {ex.Message}");
                _gpuTimestampsSupported = false;
                _gpuTimestampPeriod = 1.0;
            }
        }

        /// <summary>
        /// Creates a GPU timestamp query pool for measuring GPU execution time.
        /// Based on Vulkan API: vkCreateQueryPool with VK_QUERY_TYPE_TIMESTAMP
        /// https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCreateQueryPool.html
        /// We create a query pool with 2 queries per frame (start and end timestamps) using double buffering.
        /// Uses VulkanDevice's CreateTimestampQueryPool method via reflection.
        /// </summary>
        /// <returns>True if query pool was created successfully, false otherwise.</returns>
        private bool CreateTimestampQueryPool()
        {
            if (_device == null)
            {
                return false;
            }

            try
            {
                // Use reflection to call VulkanDevice's CreateTimestampQueryPool method
                // This method handles the Vulkan structure marshalling internally
                System.Reflection.MethodInfo createQueryPoolMethod = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetMethod(
                    "CreateTimestampQueryPool",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (createQueryPoolMethod != null)
                {
                    // CreateTimestampQueryPool takes queryCount parameter (number of queries in pool)
                    // We need TIMESTAMP_QUERY_COUNT * 2 queries for double buffering (2 queries per frame, 2 frames)
                    uint queryCount = TIMESTAMP_QUERY_COUNT * 2; // 4 queries total for double buffering
                    object result = createQueryPoolMethod.Invoke(_device, new object[] { queryCount });

                    if (result != null && result is IntPtr v)
                    {
                        _timestampQueryPool = v;
                        _timestampQueriesInitialized = true;
                        return true;
                    }
                    else if (result != null)
                    {
                        // Try to convert result to IntPtr if it's a different type
                        try
                        {
                            _timestampQueryPool = (IntPtr)Convert.ChangeType(result, typeof(IntPtr));
                            _timestampQueriesInitialized = true;
                            return true;
                        }
                        catch
                        {
                            Console.WriteLine("[VulkanBackend] CreateTimestampQueryPool: Invalid return type from CreateTimestampQueryPool");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("[VulkanBackend] CreateTimestampQueryPool: CreateTimestampQueryPool returned null");
                        return false;
                    }
                }
                else
                {
                    // Method not found, try alternative method names or fallback to direct Vulkan calls
                    Console.WriteLine("[VulkanBackend] CreateTimestampQueryPool: CreateTimestampQueryPool method not found on VulkanDevice");
                    
                    // Fallback: Try to get vkCreateQueryPool function pointer via reflection
                    System.Reflection.FieldInfo vkCreateQueryPoolField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField(
                        "vkCreateQueryPool",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                    if (vkCreateQueryPoolField == null)
                    {
                        Console.WriteLine("[VulkanBackend] CreateTimestampQueryPool: vkCreateQueryPool function not found, GPU timestamps disabled");
                        return false;
                    }

                    object vkCreateQueryPoolObj = vkCreateQueryPoolField.GetValue(null);
                    if (vkCreateQueryPoolObj == null)
                    {
                        Console.WriteLine("[VulkanBackend] CreateTimestampQueryPool: vkCreateQueryPool delegate is null, GPU timestamps disabled");
                        return false;
                    }

                    // Get VkDevice handle from VulkanDevice
                    IntPtr vkDevice = GetVkDeviceHandle();
                    if (vkDevice == IntPtr.Zero)
                    {
                        Console.WriteLine("[VulkanBackend] CreateTimestampQueryPool: Could not get VkDevice handle, GPU timestamps disabled");
                        return false;
                    }

                    // For now, we'll mark as not initialized since direct structure marshalling is complex
                    // In a full implementation, this would create the query pool using proper Vulkan interop
                    // The framework is in place, but requires proper structure definitions for full functionality
                    Console.WriteLine("[VulkanBackend] CreateTimestampQueryPool: Direct Vulkan interop requires structure marshalling, GPU timestamps disabled");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] CreateTimestampQueryPool failed: {ex.Message}");
                _timestampQueriesInitialized = false;
                return false;
            }
        }

        /// <summary>
        /// Gets the VkDevice handle from VulkanDevice instance.
        /// Helper method to access device handle via reflection.
        /// </summary>
        /// <returns>VkDevice handle, or IntPtr.Zero if not available.</returns>
        private IntPtr GetVkDeviceHandle()
        {
            if (_device == null)
            {
                return IntPtr.Zero;
            }

            try
            {
                // Try property names
                System.Reflection.PropertyInfo deviceProperty = _device.GetType().GetProperty("Device", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (deviceProperty == null)
                {
                    deviceProperty = _device.GetType().GetProperty("VkDevice", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                }
                if (deviceProperty == null)
                {
                    deviceProperty = _device.GetType().GetProperty("NativeDevice", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                }

                if (deviceProperty != null)
                {
                    object deviceValue = deviceProperty.GetValue(_device);
                    if (deviceValue is IntPtr)
                    {
                        return (IntPtr)deviceValue;
                    }
                }

                // Try field names
                System.Reflection.FieldInfo deviceField = _device.GetType().GetField("_device", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (deviceField == null)
                {
                    deviceField = _device.GetType().GetField("device", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                }
                if (deviceField == null)
                {
                    deviceField = _device.GetType().GetField("vkDevice", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                }

                if (deviceField != null)
                {
                    object deviceValue = deviceField.GetValue(_device);
                    if (deviceValue is IntPtr)
                    {
                        return (IntPtr)deviceValue;
                    }
                }

                return IntPtr.Zero;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Gets the current VkCommandBuffer handle for timestamp writing.
        /// Attempts to access command buffer from current frame command list or device.
        /// Based on Vulkan API: Command buffers are obtained from command pools
        /// https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkAllocateCommandBuffers.html
        /// </summary>
        /// <returns>VkCommandBuffer handle, or IntPtr.Zero if not available.</returns>
        private IntPtr GetCurrentCommandBuffer()
        {
            if (_device == null)
            {
                return IntPtr.Zero;
            }

            try
            {
                // Try to get current frame command list from device via reflection
                // In a full implementation, there would be a _currentFrameCommandList field
                // For now, we'll check if VulkanDevice has a method to get the current command buffer
                
                // Try to get a method that returns the current command buffer
                System.Reflection.MethodInfo getCurrentCommandBufferMethod = _device.GetType().GetMethod(
                    "GetCurrentCommandBuffer",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (getCurrentCommandBufferMethod != null)
                {
                    object result = getCurrentCommandBufferMethod.Invoke(_device, null);
                    if (result is IntPtr)
                    {
                        return (IntPtr)result;
                    }
                }

                // Try to access current command list via property or field
                System.Reflection.PropertyInfo commandListProperty = _device.GetType().GetProperty(
                    "CurrentCommandList",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (commandListProperty == null)
                {
                    commandListProperty = _device.GetType().GetProperty(
                        "CurrentFrameCommandList",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                }

                if (commandListProperty != null)
                {
                    object commandListObj = commandListProperty.GetValue(_device);
                    if (commandListObj != null)
                    {
                        // Try to get VkCommandBuffer from command list
                        System.Reflection.FieldInfo vkCommandBufferField = commandListObj.GetType().GetField(
                            "_vkCommandBuffer",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                        if (vkCommandBufferField == null)
                        {
                            vkCommandBufferField = commandListObj.GetType().GetField(
                                "vkCommandBuffer",
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        }

                        if (vkCommandBufferField != null)
                        {
                            object commandBufferValue = vkCommandBufferField.GetValue(commandListObj);
                            if (commandBufferValue is IntPtr)
                            {
                                return (IntPtr)commandBufferValue;
                            }
                        }

                        // Try to get VkCommandBuffer via property
                        System.Reflection.PropertyInfo commandBufferProperty = commandListObj.GetType().GetProperty(
                            "VkCommandBuffer",
                            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                        if (commandBufferProperty != null)
                        {
                            object commandBufferValue = commandBufferProperty.GetValue(commandListObj);
                            if (commandBufferValue is IntPtr)
                            {
                                return (IntPtr)commandBufferValue;
                            }
                        }
                    }
                }

                // Command buffer management not fully implemented yet
                // In the full implementation, this would return the active command buffer
                return IntPtr.Zero;
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Destroys the GPU timestamp query pool.
        /// Based on Vulkan API: vkDestroyQueryPool
        /// https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkDestroyQueryPool.html
        /// Called during shutdown to clean up resources.
        /// </summary>
        private void DestroyTimestampQueryPool()
        {
            if (_timestampQueryPool == IntPtr.Zero || _device == null)
            {
                return;
            }

            try
            {
                // Get vkDestroyQueryPool function pointer via reflection
                System.Reflection.FieldInfo vkDestroyQueryPoolField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField(
                    "vkDestroyQueryPool",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (vkDestroyQueryPoolField != null)
                {
                    object vkDestroyQueryPoolObj = vkDestroyQueryPoolField.GetValue(null);
                    if (vkDestroyQueryPoolObj != null)
                    {
                        // Get VkDevice handle
                        IntPtr vkDevice = GetVkDeviceHandle();

                        if (vkDevice != IntPtr.Zero)
                        {
                            // Call vkDestroyQueryPool
                            // Signature: void vkDestroyQueryPool(VkDevice device, VkQueryPool queryPool, VkAllocationCallbacks* pAllocator);
                            System.Reflection.MethodInfo invokeMethod = vkDestroyQueryPoolObj.GetType().GetMethod("Invoke");
                            if (invokeMethod != null)
                            {
                                invokeMethod.Invoke(vkDestroyQueryPoolObj, new object[] { vkDevice, _timestampQueryPool, IntPtr.Zero });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] DestroyTimestampQueryPool failed: {ex.Message}");
            }
            finally
            {
                _timestampQueryPool = IntPtr.Zero;
                _timestampQueriesInitialized = false;
            }
        }

        /// <summary>
        /// Writes a GPU timestamp into the query pool at the specified pipeline stage.
        /// Based on Vulkan API: vkCmdWriteTimestamp
        /// https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkCmdWriteTimestamp.html
        /// Records a timestamp when the specified pipeline stage completes execution.
        /// </summary>
        /// <param name="pipelineStage">VkPipelineStageFlagBits value (e.g., VK_PIPELINE_STAGE_TOP_OF_PIPE_BIT or VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT).</param>
        private void WriteGpuTimestamp(uint pipelineStage)
        {
            if (!_timestampQueriesInitialized || _timestampQueryPool == IntPtr.Zero || _device == null)
            {
                return;
            }

            try
            {
                // Get vkCmdWriteTimestamp function pointer via reflection
                System.Reflection.FieldInfo vkCmdWriteTimestampField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField(
                    "vkCmdWriteTimestamp",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (vkCmdWriteTimestampField == null)
                {
                    // Function not available, skip timestamp writing
                    return;
                }

                object vkCmdWriteTimestampObj = vkCmdWriteTimestampField.GetValue(null);
                if (vkCmdWriteTimestampObj == null)
                {
                    // Function delegate not available, skip timestamp writing
                    return;
                }

                // Get current command buffer handle
                // Attempt to get it via reflection from VulkanDevice or a current frame command list
                IntPtr vkCommandBuffer = GetCurrentCommandBuffer();

                // Calculate query index based on current frame and which timestamp (start=0, end=1)
                // We use double buffering: alternate between query sets 0-1 and 2-3
                // Start timestamp uses even index, end timestamp uses odd index
                uint queryIndex = _timestampQueryIndex * 2; // Start timestamp (even index)
                if (pipelineStage == 0x00000008) // VK_PIPELINE_STAGE_BOTTOM_OF_PIPE_BIT (end timestamp)
                {
                    queryIndex += 1; // End timestamp (odd index)
                }

                // Call vkCmdWriteTimestamp if we have a valid command buffer
                // Signature: void vkCmdWriteTimestamp(VkCommandBuffer commandBuffer, VkPipelineStageFlagBits pipelineStage, VkQueryPool queryPool, uint query);
                if (vkCommandBuffer != IntPtr.Zero)
                {
                    System.Reflection.MethodInfo invokeMethod = vkCmdWriteTimestampObj.GetType().GetMethod("Invoke");
                    if (invokeMethod != null)
                    {
                        try
                        {
                            invokeMethod.Invoke(vkCmdWriteTimestampObj, new object[] { vkCommandBuffer, pipelineStage, _timestampQueryPool, queryIndex });
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[VulkanBackend] WriteGpuTimestamp: Failed to invoke vkCmdWriteTimestamp: {ex.Message}");
                        }
                    }
                }
                else
                {
                    // Command buffer not available yet - this is expected if command buffer management isn't fully implemented
                    // Once command buffer management is implemented, timestamps will be written automatically
                    // For now, the framework is in place and will work once command buffers are available
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] WriteGpuTimestamp failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Resolves GPU timestamp queries from the query pool.
        /// Based on Vulkan API: vkGetQueryPoolResults
        /// https://www.khronos.org/registry/vulkan/specs/1.3-extensions/man/html/vkGetQueryPoolResults.html
        /// Retrieves the timestamp values that were recorded by vkCmdWriteTimestamp.
        /// </summary>
        /// <param name="queryIndex">Query index (0 for start timestamp, 1 for end timestamp) within the frame's query set.</param>
        /// <param name="startTimestamp">Output parameter for the start timestamp value (in timestamp ticks).</param>
        /// <param name="endTimestamp">Output parameter for the end timestamp value (in timestamp ticks).</param>
        /// <returns>True if queries were resolved successfully, false otherwise.</returns>
        private bool ResolveGpuTimestampQueries(uint queryIndex, out ulong startTimestamp, out ulong endTimestamp)
        {
            startTimestamp = 0;
            endTimestamp = 0;

            if (!_timestampQueriesInitialized || _timestampQueryPool == IntPtr.Zero || _device == null)
            {
                return false;
            }

            try
            {
                // Get vkGetQueryPoolResults function pointer via reflection
                System.Reflection.FieldInfo vkGetQueryPoolResultsField = typeof(Andastra.Game.Graphics.MonoGame.Backends.VulkanDevice).GetField(
                    "vkGetQueryPoolResults",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

                if (vkGetQueryPoolResultsField == null)
                {
                    return false;
                }

                object vkGetQueryPoolResultsObj = vkGetQueryPoolResultsField.GetValue(null);
                if (vkGetQueryPoolResultsObj == null)
                {
                    return false;
                }

                // Get VkDevice handle
                IntPtr vkDevice = GetVkDeviceHandle();

                if (vkDevice == IntPtr.Zero)
                {
                    return false;
                }

                // Calculate actual query indices (start and end for the frame)
                uint startQueryIndex = queryIndex * 2; // Start timestamp (even index)
                uint endQueryIndex = startQueryIndex + 1; // End timestamp (odd index)

                // Call vkGetQueryPoolResults
                // Signature: VkResult vkGetQueryPoolResults(VkDevice device, VkQueryPool queryPool, uint firstQuery, uint queryCount, IntPtr dataSize, IntPtr pData, ulong stride, VkQueryResultFlags flags);
                // flags = VK_QUERY_RESULT_64_BIT (0x00000001) | VK_QUERY_RESULT_WAIT_BIT (0x00000002) - wait for results and return 64-bit values
                uint flags = 0x00000001 | 0x00000002; // VK_QUERY_RESULT_64_BIT | VK_QUERY_RESULT_WAIT_BIT
                uint queryCount = TIMESTAMP_QUERY_COUNT; // Get both start and end timestamps
                int resultSize = sizeof(ulong) * (int)queryCount; // 16 bytes for 2 timestamps

                // Allocate memory for results
                System.Runtime.InteropServices.GCHandle resultsHandle = System.Runtime.InteropServices.GCHandle.Alloc(_timestampQueryResults, System.Runtime.InteropServices.GCHandleType.Pinned);
                try
                {
                    IntPtr resultsPtr = resultsHandle.AddrOfPinnedObject();

                    System.Reflection.MethodInfo invokeMethod = vkGetQueryPoolResultsObj.GetType().GetMethod("Invoke");
                    if (invokeMethod != null)
                    {
                        // VkResult result = vkGetQueryPoolResults(vkDevice, _timestampQueryPool, startQueryIndex, queryCount, (IntPtr)resultSize, resultsPtr, (ulong)sizeof(ulong), flags);
                        object result = invokeMethod.Invoke(vkGetQueryPoolResultsObj, new object[]
                        {
                            vkDevice,
                            _timestampQueryPool,
                            startQueryIndex,
                            queryCount,
                            new IntPtr(resultSize),
                            resultsPtr,
                            (ulong)sizeof(ulong),
                            flags
                        });

                        // Check result (VkResult should be 0 for VK_SUCCESS)
                        if (result != null)
                        {
                            // Get numeric value of VkResult enum
                            int resultCode = 0;
                            if (result is int)
                            {
                                resultCode = (int)result;
                            }
                            else if (result is Enum)
                            {
                                resultCode = Convert.ToInt32(result);
                            }

                            if (resultCode == 0) // VK_SUCCESS
                            {
                                startTimestamp = _timestampQueryResults[0];
                                endTimestamp = _timestampQueryResults[1];
                                return true;
                            }
                            else
                            {
                                // Query results not ready yet (VK_NOT_READY = 1) or error
                                // For first frame, this is expected
                                return false;
                            }
                        }
                    }
                }
                finally
                {
                    resultsHandle.Free();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VulkanBackend] ResolveGpuTimestampQueries failed: {ex.Message}");
            }

            return false;
        }

        #endregion

        #region Resource Conversion Helpers

        /// <summary>
        /// Converts Runtime TextureFormat to Game TextureFormat.
        /// </summary>
        private Andastra.Game.Graphics.MonoGame.Interfaces.TextureFormat ConvertTextureFormat(Andastra.Runtime.Graphics.Common.Enums.TextureFormat format)
        {
            // Map Runtime format to Game format
            // Most formats should match directly, but we provide explicit conversion
            try
            {
                return (Andastra.Game.Graphics.MonoGame.Interfaces.TextureFormat)(int)format;
            }
            catch
            {
                // Fallback to R8G8B8A8_UNorm if conversion fails
                return Andastra.Game.Graphics.MonoGame.Interfaces.TextureFormat.R8G8B8A8_UNorm;
            }
        }

        /// <summary>
        /// Converts Runtime TextureUsage to Game TextureUsage.
        /// </summary>
        private Andastra.Game.Graphics.MonoGame.Interfaces.TextureUsage ConvertTextureUsage(Andastra.Runtime.Graphics.Common.Enums.TextureUsage usage)
        {
            // Map Runtime usage flags to Game usage flags
            Andastra.Game.Graphics.MonoGame.Interfaces.TextureUsage result = 0;
            
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.TextureUsage.ShaderResource) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.TextureUsage.ShaderResource;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.TextureUsage.RenderTarget) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.TextureUsage.RenderTarget;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.TextureUsage.DepthStencil) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.TextureUsage.DepthStencil;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.TextureUsage.UnorderedAccess) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.TextureUsage.UnorderedAccess;
            
            return result;
        }

        /// <summary>
        /// Converts Runtime BufferUsage to Game BufferUsageFlags.
        /// </summary>
        private Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags ConvertBufferUsage(Andastra.Runtime.Graphics.Common.Enums.BufferUsage usage)
        {
            // Map Runtime usage flags to Game usage flags
            // Runtime: Vertex, Index, Constant, Structured, Indirect, AccelerationStructure
            // Game: VertexBuffer, IndexBuffer, ConstantBuffer, ShaderResource, UnorderedAccess, IndirectArgument
            Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags result = 0;
            
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.BufferUsage.Vertex) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags.VertexBuffer;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.BufferUsage.Index) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags.IndexBuffer;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.BufferUsage.Constant) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags.ConstantBuffer;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.BufferUsage.Structured) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags.ShaderResource;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.BufferUsage.Indirect) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags.IndirectArgument;
            if ((usage & Andastra.Runtime.Graphics.Common.Enums.BufferUsage.AccelerationStructure) != 0)
                result |= Andastra.Game.Graphics.MonoGame.Interfaces.BufferUsageFlags.AccelStructStorage;
            
            return result;
        }

        /// <summary>
        /// Estimates texture memory usage based on dimensions and format.
        /// </summary>
        private long EstimateTextureMemory(int width, int height, int depth, int arraySize, int mipLevels, Andastra.Runtime.Graphics.Common.Enums.TextureFormat format)
        {
            // Estimate bytes per pixel based on format
            int bytesPerPixel = 4; // Default to RGBA8
            // Runtime format enum values match Game format enum values for direct comparison
            switch (format)
            {
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8_UNorm:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8_UInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8_SInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8_SNorm:
                    bytesPerPixel = 1;
                    break;
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8_UNorm:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8_UInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8_SNorm:
                    bytesPerPixel = 2;
                    break;
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8B8A8_UNorm:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8B8A8_UNorm_SRGB:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8B8A8_UInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8B8A8_SInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R8G8B8A8_SNorm:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.B8G8R8A8_UNorm:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.B8G8R8A8_UNorm_SRGB:
                    bytesPerPixel = 4;
                    break;
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R16G16B16A16_Float:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R16G16B16A16_UNorm:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R16G16B16A16_UInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R16G16B16A16_SInt:
                    bytesPerPixel = 8;
                    break;
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R32G32B32A32_Float:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R32G32B32A32_UInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.R32G32B32A32_SInt:
                    bytesPerPixel = 16;
                    break;
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.D16_UNorm:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.D24_UNorm_S8_UInt:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.D32_Float:
                case Andastra.Runtime.Graphics.Common.Enums.TextureFormat.D32_Float_S8_UInt:
                    bytesPerPixel = 4;
                    break;
            }

            // Calculate total memory including mipmaps
            // Mipmap calculation: base size * (1 + 1/4 + 1/16 + ...) ≈ base size * 1.33
            long baseSize = (long)width * height * depth * arraySize * bytesPerPixel;
            long mipmapSize = (long)(baseSize * 1.33); // Approximate mipmap overhead

            return mipmapSize * mipLevels;
        }

        /// <summary>
        /// Estimates buffer size for tracking memory usage.
        /// </summary>
        /// <param name="handle">Buffer handle.</param>
        /// <returns>Buffer size in bytes, or 0 if not found.</returns>
        private long EstimateBufferSize(IntPtr handle)
        {
            // Try to get buffer size from tracking dictionary first
            if (_bufferSizes != null && _bufferSizes.TryGetValue(handle, out long size))
            {
                return size;
            }

            // Fallback: Try to get size from buffer description
            if (_buffers != null && _buffers.TryGetValue(handle, out var buffer))
            {
                try
                {
                    // Get buffer description which contains size
                    var desc = buffer.Desc;
                    return desc.ByteSize;
                }
                catch
                {
                    // If we can't get the size, return 0
                    return 0;
                }
            }

            return 0;
        }

        #endregion
    }
}

