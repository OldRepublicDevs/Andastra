using System;
using System.IO;
using System.Runtime.InteropServices;
using Stride.Graphics;
using Stride.Engine;
using Andastra.Runtime.Graphics.Common.Enums;
using Andastra.Runtime.Graphics.Common.Remix;

namespace Andastra.Runtime.Stride.Remix
{
    /// <summary>
    /// Stride implementation of NVIDIA RTX Remix bridge.
    /// Inherits shared Remix logic from BaseRemixBridge.
    ///
    /// RTX Remix intercepts DirectX 9 calls and replaces rasterized output
    /// with path-traced rendering using RTX hardware.
    ///
    /// Based on RTX Remix: https://github.com/NVIDIAGameWorks/rtx-remix
    ///
    /// Features:
    /// - Path tracing with up to 8 bounces
    /// - Material replacement (PBR)
    /// - Automatic light extraction
    /// - DLSS integration
    /// - Denoising
    /// </summary>
    public class StrideRemixBridge : BaseRemixBridge
    {
        private GraphicsDevice _graphicsDevice;
        private IntPtr _d3d9Object; // IDirect3D9 COM object

        public StrideRemixBridge(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        #region BaseRemixBridge Implementation

        protected override bool LoadD3D9Library(string runtimePath)
        {
            string d3d9Path = "d3d9.dll";

            if (!string.IsNullOrEmpty(runtimePath))
            {
                string remixD3d9 = Path.Combine(runtimePath, "d3d9.dll");
                if (File.Exists(remixD3d9))
                {
                    d3d9Path = remixD3d9;
                }
            }

            _d3d9Handle = NativeMethods.LoadLibrary(d3d9Path);
            return _d3d9Handle != IntPtr.Zero;
        }

        protected override void UnloadD3D9Library()
        {
            if (_d3d9Handle != IntPtr.Zero)
            {
                NativeMethods.FreeLibrary(_d3d9Handle);
                _d3d9Handle = IntPtr.Zero;
            }
        }

        protected override bool CreateD3D9Device(IntPtr windowHandle, RemixSettings settings)
        {
            if (windowHandle == IntPtr.Zero)
            {
                Console.WriteLine("[StrideRemix] Invalid window handle");
                return false;
            }

            // Step 1: Get Direct3DCreate9 function from loaded library
            // If Remix is active, this will be Remix's interceptor function
            // Otherwise, it will be the standard DirectX 9 function
            IntPtr createFunc = NativeMethods.GetProcAddress(_d3d9Handle, "Direct3DCreate9");
            if (createFunc == IntPtr.Zero)
            {
                Console.WriteLine("[StrideRemix] Failed to get Direct3DCreate9 function pointer");
                return false;
            }

            // Create delegate for Direct3DCreate9
            Direct3DCreate9Delegate direct3DCreate9 = Marshal.GetDelegateForFunctionPointer<Direct3DCreate9Delegate>(createFunc);

            // Call Direct3DCreate9(D3D_SDK_VERSION)
            // Based on DirectX 9 SDK: Direct3DCreate9 creates the IDirect3D9 interface
            // Remix intercepts this call and returns its own IDirect3D9 wrapper
            // This allows Remix to hook all subsequent DirectX 9 calls
            IntPtr d3d9 = direct3DCreate9(D3D_SDK_VERSION);
            if (d3d9 == IntPtr.Zero)
            {
                Console.WriteLine("[StrideRemix] Direct3DCreate9 failed");
                return false;
            }

            _d3d9Object = d3d9;
            Console.WriteLine("[StrideRemix] Direct3DCreate9 succeeded (Remix interceptor active)");

            // Step 2: Get adapter display mode
            // Based on DirectX 9 SDK: IDirect3D9::GetAdapterDisplayMode
            // This gets the current display mode for the default adapter
            D3DDISPLAYMODE displayMode = new D3DDISPLAYMODE();
            int hr = GetAdapterDisplayMode(d3d9, D3DADAPTER_DEFAULT, ref displayMode);
            if (hr < 0)
            {
                Console.WriteLine($"[StrideRemix] GetAdapterDisplayMode failed with HRESULT 0x{hr:X8}");
                ReleaseComObject(_d3d9Object);
                _d3d9Object = IntPtr.Zero;
                return false;
            }

            Console.WriteLine($"[StrideRemix] Display mode: {displayMode.Width}x{displayMode.Height} @ {displayMode.RefreshRate}Hz, Format: 0x{displayMode.Format:X8}");

            // Step 3: Create D3DPRESENT_PARAMETERS structure
            // Based on DirectX 9 SDK: D3DPRESENT_PARAMETERS structure
            // Configure presentation parameters for windowed mode (Remix works with windowed applications)
            D3DPRESENT_PARAMETERS presentParams = new D3DPRESENT_PARAMETERS
            {
                BackBufferWidth = displayMode.Width,
                BackBufferHeight = displayMode.Height,
                BackBufferFormat = displayMode.Format,
                BackBufferCount = 1,
                MultiSampleType = D3DMULTISAMPLE_NONE,
                MultiSampleQuality = 0,
                SwapEffect = D3DSWAPEFFECT_DISCARD,
                hDeviceWindow = windowHandle,
                Windowed = 1, // Windowed mode (Remix works with windowed applications)
                EnableAutoDepthStencil = 1, // Enable depth/stencil buffer
                AutoDepthStencilFormat = D3DFMT_D24S8, // 24-bit depth, 8-bit stencil
                Flags = D3DPRESENTFLAG_DISCARD_DEPTHSTENCIL, // Discard depth/stencil after present
                FullScreen_RefreshRateInHz = 0, // Not used in windowed mode
                PresentationInterval = D3DPRESENT_INTERVAL_ONE // VSync enabled (can be changed to D3DPRESENT_INTERVAL_IMMEDIATE for no VSync)
            };

            // Step 4: Create D3D9 device with appropriate parameters
            // Based on DirectX 9 SDK: IDirect3D9::CreateDevice
            // Remix intercepts this call and returns its own IDirect3DDevice9 wrapper
            // This allows Remix to hook all rendering calls and replace them with path tracing
            IntPtr devicePtr = Marshal.AllocHGlobal(IntPtr.Size);
            try
            {
                Guid iidDevice = IID_IDirect3DDevice9;
                uint behaviorFlags = D3DCREATE_HARDWARE_VERTEXPROCESSING | D3DCREATE_MULTITHREADED;
                
                // Try hardware vertex processing first
                hr = CreateDevice(d3d9, D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, windowHandle,
                    behaviorFlags, ref presentParams, ref iidDevice, devicePtr);

                // If hardware vertex processing fails, try software vertex processing
                if (hr < 0)
                {
                    Console.WriteLine($"[StrideRemix] CreateDevice with hardware vertex processing failed (HRESULT 0x{hr:X8}), trying software vertex processing");
                    behaviorFlags = D3DCREATE_SOFTWARE_VERTEXPROCESSING | D3DCREATE_MULTITHREADED;
                    hr = CreateDevice(d3d9, D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, windowHandle,
                        behaviorFlags, ref presentParams, ref iidDevice, devicePtr);
                }

                if (hr < 0)
                {
                    Console.WriteLine($"[StrideRemix] CreateDevice failed with HRESULT 0x{hr:X8}");
                    ReleaseComObject(_d3d9Object);
                    _d3d9Object = IntPtr.Zero;
                    return false;
                }

                _deviceHandle = Marshal.ReadIntPtr(devicePtr);
                if (_deviceHandle == IntPtr.Zero)
                {
                    Console.WriteLine("[StrideRemix] Device pointer is null after CreateDevice");
                    ReleaseComObject(_d3d9Object);
                    _d3d9Object = IntPtr.Zero;
                    return false;
                }

                Console.WriteLine("[StrideRemix] DirectX 9 device created successfully");
                Console.WriteLine("[StrideRemix] Remix will intercept all DirectX 9 calls for path tracing");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideRemix] Exception during device creation: {ex.Message}");
                Console.WriteLine($"[StrideRemix] Stack trace: {ex.StackTrace}");
                if (_d3d9Object != IntPtr.Zero)
                {
                    ReleaseComObject(_d3d9Object);
                    _d3d9Object = IntPtr.Zero;
                }
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(devicePtr);
            }
        }

        protected override void ReleaseD3D9Device()
        {
            // Release D3D9 device (IDirect3DDevice9 COM object)
            // Based on DirectX 9 SDK: IUnknown::Release
            if (_deviceHandle != IntPtr.Zero)
            {
                uint refCount = ReleaseComObject(_deviceHandle);
                Console.WriteLine($"[StrideRemix] Released D3D9 device (ref count: {refCount})");
                _deviceHandle = IntPtr.Zero;
            }

            // Release D3D9 object (IDirect3D9 COM object)
            // Based on DirectX 9 SDK: IUnknown::Release
            if (_d3d9Object != IntPtr.Zero)
            {
                uint refCount = ReleaseComObject(_d3d9Object);
                Console.WriteLine($"[StrideRemix] Released D3D9 object (ref count: {refCount})");
                _d3d9Object = IntPtr.Zero;
            }
        }

        protected override void OnBeginFrame()
        {
            // Signal frame start to Remix
            // D3D9 BeginScene equivalent - Remix hooks this
        }

        protected override void OnEndFrame()
        {
            // Signal frame end to Remix
            // D3D9 EndScene + Present equivalent - Remix performs path tracing here
        }

        protected override void OnSubmitGeometry(RemixGeometry geometry)
        {
            // Convert geometry to D3D9 draw calls
            // Remix intercepts and builds acceleration structures

            // TODO:  In actual implementation:
            // - Set vertex buffer
            // - Set index buffer
            // - Set world transform
            // - Draw primitive
        }

        protected override void OnSubmitLight(RemixLight light)
        {
            // Convert light to D3D9 light
            // Remix intercepts and uses for path tracing

            // D3DLIGHT9 structure:
            // - Type (Point, Spot, Directional)
            // - Diffuse color
            // - Position/Direction
            // - Range, Attenuation

            // SetLight + LightEnable
        }

        protected override void OnSubmitMaterial(RemixMaterial material)
        {
            // Convert material to D3D9 material + textures
            // Remix converts these to PBR materials

            // D3DMATERIAL9 structure:
            // - Diffuse, Ambient, Specular colors
            // - Power (shininess)

            // SetMaterial + SetTexture
        }

        protected override void OnSetCamera(RemixCamera camera)
        {
            // Set view and projection matrices
            // Remix uses these for ray generation

            // SetTransform(D3DTS_VIEW, viewMatrix)
            // SetTransform(D3DTS_PROJECTION, projMatrix)
        }

        protected override void OnConfigureRendering(RemixRenderConfig config)
        {
            // Apply Remix-specific settings via runtime API
            // This would use Remix's configuration functions

            Console.WriteLine($"[StrideRemix] Configuring: SPP={config.SamplesPerPixel}, Bounces={config.MaxBounces}");
        }

        protected override bool CheckForRemixExports(string dllPath)
        {
            IntPtr testHandle = NativeMethods.LoadLibrary(dllPath);
            if (testHandle == IntPtr.Zero) return false;

            // Check for Remix-specific exports
            IntPtr remixExport = NativeMethods.GetProcAddress(testHandle, "remixInitialize");
            bool isRemix = remixExport != IntPtr.Zero;

            NativeMethods.FreeLibrary(testHandle);
            return isRemix;
        }

        #endregion

        #region Native Methods

        private static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
        }

        #endregion

        #region DirectX 9 Structures and Constants

        // DirectX 9 Constants
        // Based on DirectX 9 SDK: https://docs.microsoft.com/en-us/windows/win32/direct3d9/direct3d-constants
        private const uint D3D_SDK_VERSION = 32;
        private const uint D3DDEVTYPE_HAL = 1;
        private const uint D3DCREATE_HARDWARE_VERTEXPROCESSING = 0x00000040;
        private const uint D3DCREATE_MULTITHREADED = 0x00000004;
        private const uint D3DCREATE_SOFTWARE_VERTEXPROCESSING = 0x00000020;
        private const uint D3DMULTISAMPLE_NONE = 0;
        private const uint D3DSWAPEFFECT_DISCARD = 1;
        private const uint D3DFMT_D24S8 = 75; // D3DFMT_D24S8 (24-bit depth, 8-bit stencil)
        private const uint D3DFMT_A8R8G8B8 = 21; // D3DFMT_A8R8G8B8 (32-bit ARGB format)
        private const uint D3DPRESENT_INTERVAL_ONE = 0x00000001;
        private const uint D3DPRESENT_INTERVAL_IMMEDIATE = 0x80000000;
        private const uint D3DPRESENTFLAG_LOCKABLE_BACKBUFFER = 0x00000001;
        private const uint D3DPRESENTFLAG_DISCARD_DEPTHSTENCIL = 0x00000002;
        private const int D3DADAPTER_DEFAULT = 0;

        // DirectX 9 GUIDs
        // Based on DirectX 9 SDK: Interface IDs for COM objects
        private static readonly Guid IID_IDirect3D9 = new Guid("81BDCBCA-64D4-426d-AE8D-AD0147F4275C");
        private static readonly Guid IID_IDirect3DDevice9 = new Guid("D0223B96-BF7A-43fd-92BD-A43B0D82B9EB");

        // DirectX 9 Structures
        // Based on DirectX 9 SDK: D3DDISPLAYMODE structure
        [StructLayout(LayoutKind.Sequential)]
        private struct D3DDISPLAYMODE
        {
            public uint Width;
            public uint Height;
            public uint RefreshRate;
            public uint Format;
        }

        // Based on DirectX 9 SDK: D3DPRESENT_PARAMETERS structure
        [StructLayout(LayoutKind.Sequential)]
        private struct D3DPRESENT_PARAMETERS
        {
            public uint BackBufferWidth;
            public uint BackBufferHeight;
            public uint BackBufferFormat;
            public uint BackBufferCount;
            public uint MultiSampleType;
            public uint MultiSampleQuality;
            public uint SwapEffect;
            public IntPtr hDeviceWindow;
            public int Windowed;
            public int EnableAutoDepthStencil;
            public uint AutoDepthStencilFormat;
            public uint Flags;
            public uint FullScreen_RefreshRateInHz;
            public uint PresentationInterval;
        }

        // DirectX 9 Function Delegates
        // Based on DirectX 9 SDK: COM interface vtable method signatures
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr Direct3DCreate9Delegate(uint sdkVersion);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate uint ReleaseDelegate(IntPtr comObject);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int GetAdapterDisplayModeDelegate(IntPtr d3d, uint adapter, ref D3DDISPLAYMODE mode);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate int CreateDeviceDelegate(IntPtr d3d, uint adapter, uint deviceType, IntPtr hFocusWindow,
            uint behaviorFlags, ref D3DPRESENT_PARAMETERS presentationParameters, ref Guid returnedDeviceInterface, IntPtr ppDevice);

        // DirectX 9 P/Invoke Functions
        // Based on DirectX 9 SDK: Direct3DCreate9 function
        [DllImport("d3d9.dll", EntryPoint = "Direct3DCreate9", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr Direct3DCreate9(uint sdkVersion);

        /// <summary>
        /// Gets adapter display mode using DirectX 9 COM vtable.
        /// Based on DirectX 9 SDK: IDirect3D9::GetAdapterDisplayMode
        /// Vtable index 11: GetAdapterDisplayMode method
        /// </summary>
        private unsafe int GetAdapterDisplayMode(IntPtr d3d, uint adapter, ref D3DDISPLAYMODE mode)
        {
            if (d3d == IntPtr.Zero)
            {
                return -1;
            }

            // Access COM vtable (first pointer in object)
            IntPtr* vtable = *(IntPtr**)d3d;
            // GetAdapterDisplayMode is at index 11 in IDirect3D9 vtable
            // Based on DirectX 9 SDK: IDirect3D9 interface vtable layout
            IntPtr methodPtr = vtable[11];
            var getMode = Marshal.GetDelegateForFunctionPointer<GetAdapterDisplayModeDelegate>(methodPtr);
            return getMode(d3d, adapter, ref mode);
        }

        /// <summary>
        /// Creates device using DirectX 9 COM vtable.
        /// Based on DirectX 9 SDK: IDirect3D9::CreateDevice
        /// Vtable index 16: CreateDevice method
        /// </summary>
        private unsafe int CreateDevice(IntPtr d3d, uint adapter, uint deviceType, IntPtr hFocusWindow,
            uint behaviorFlags, ref D3DPRESENT_PARAMETERS presentationParameters, ref Guid returnedDeviceInterface, IntPtr ppDevice)
        {
            if (d3d == IntPtr.Zero)
            {
                return -1;
            }

            // Access COM vtable (first pointer in object)
            IntPtr* vtable = *(IntPtr**)d3d;
            // CreateDevice is at index 16 in IDirect3D9 vtable
            // Based on DirectX 9 SDK: IDirect3D9 interface vtable layout
            IntPtr methodPtr = vtable[16];
            var createDevice = Marshal.GetDelegateForFunctionPointer<CreateDeviceDelegate>(methodPtr);
            return createDevice(d3d, adapter, deviceType, hFocusWindow, behaviorFlags, ref presentationParameters, ref returnedDeviceInterface, ppDevice);
        }

        /// <summary>
        /// Releases a COM object using DirectX 9 COM vtable.
        /// Based on DirectX 9 SDK: IUnknown::Release
        /// Vtable index 2: Release method (standard COM IUnknown interface)
        /// </summary>
        private unsafe uint ReleaseComObject(IntPtr comObject)
        {
            if (comObject == IntPtr.Zero)
            {
                return 0;
            }

            // Access COM vtable (first pointer in object)
            IntPtr* vtable = *(IntPtr**)comObject;
            // Release is at index 2 in IUnknown vtable (standard COM interface)
            IntPtr methodPtr = vtable[2];
            var release = Marshal.GetDelegateForFunctionPointer<ReleaseDelegate>(methodPtr);
            return release(comObject);
        }

        #endregion

        #region Stride Integration

        /// <summary>
        /// Submits Stride geometry for Remix path tracing.
        /// </summary>
        public void SubmitStrideMesh(global::Stride.Rendering.Mesh mesh, global::Stride.Core.Mathematics.Matrix worldMatrix)
        {
            if (!IsActive || mesh == null) return;

            // Convert Stride mesh to RemixGeometry
            var geometry = new RemixGeometry
            {
                VertexBuffer = IntPtr.Zero, // Would get native handle
                IndexBuffer = IntPtr.Zero,
                VertexCount = mesh.Draw?.VertexBuffers[0].Count ?? 0,
                IndexCount = mesh.Draw?.IndexBuffer?.Count ?? 0,
                VertexStride = mesh.Draw?.VertexBuffers[0].Stride ?? 0,
                WorldMatrix = ConvertMatrix(worldMatrix),
                MaterialId = 0,
                CastShadows = true,
                Visible = true
            };

            SubmitGeometry(geometry);
        }

        /// <summary>
        /// Submits Stride light for Remix path tracing.
        /// </summary>
        public void SubmitStrideLight(Stride.Engine.LightComponent light)
        {
            if (!IsActive || light == null) return;

            // Convert Stride light to RemixLight
            var remixLight = new RemixLight
            {
                Type = LightType.Point, // Would determine from light type
                Position = ConvertVector(light.Entity?.Transform?.Position ?? global::Stride.Core.Mathematics.Vector3.Zero),
                Direction = System.Numerics.Vector3.UnitZ,
                Color = new System.Numerics.Vector3(1, 1, 1),
                Intensity = light.Intensity,
                Radius = 100f,
                ConeAngle = 45f,
                CastShadows = true
            };

            SubmitLight(remixLight);
        }

        private System.Numerics.Matrix4x4 ConvertMatrix(global::Stride.Core.Mathematics.Matrix m)
        {
            return new System.Numerics.Matrix4x4(
                m.M11, m.M12, m.M13, m.M14,
                m.M21, m.M22, m.M23, m.M24,
                m.M31, m.M32, m.M33, m.M34,
                m.M41, m.M42, m.M43, m.M44
            );
        }

        private System.Numerics.Vector3 ConvertVector(global::Stride.Core.Mathematics.Vector3 v)
        {
            return new System.Numerics.Vector3(v.X, v.Y, v.Z);
        }

        #endregion
    }
}

