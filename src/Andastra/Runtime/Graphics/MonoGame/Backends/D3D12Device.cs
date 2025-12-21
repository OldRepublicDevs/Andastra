using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Andastra.Runtime.MonoGame.Enums;
using Andastra.Runtime.MonoGame.Interfaces;
using Andastra.Runtime.MonoGame.Rendering;

namespace Andastra.Runtime.MonoGame.Backends
{
    /// <summary>
    /// DirectX 12 device wrapper implementing IDevice interface for raytracing operations.
    ///
    /// Provides NVRHI-style abstractions for DirectX 12 raytracing resources:
    /// - Acceleration structures (BLAS/TLAS)
    /// - Raytracing pipelines
    /// - Resource creation and management
    ///
    /// Wraps native ID3D12Device5 with DXR 1.1 support.
    /// </summary>
    public class D3D12Device : IDevice
    {
        #region DirectX 12 API Interop

        private const string D3D12Library = "d3d12.dll";

        // Windows API functions for event synchronization
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateEvent(IntPtr lpEventAttributes, [MarshalAs(UnmanagedType.Bool)] bool bManualReset, [MarshalAs(UnmanagedType.Bool)] bool bInitialState, string lpName);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        private const uint WAIT_OBJECT_0 = 0x00000000;
        private const uint WAIT_TIMEOUT = 0x00000102;
        private const uint WAIT_FAILED = 0xFFFFFFFF;
        private const uint INFINITE = 0xFFFFFFFF;

        // DirectX 12 HRESULT values
        private enum HRESULT
        {
            S_OK = 0x00000000,
            E_FAIL = unchecked((int)0x80004005),
            E_INVALIDARG = unchecked((int)0x80070057),
            E_OUTOFMEMORY = unchecked((int)0x8007000E),
            E_NOTIMPL = unchecked((int)0x80004001),
            DXGI_ERROR_DEVICE_REMOVED = unchecked((int)0x887A0005),
        }

        // DirectX 12 GUIDs
        private static readonly Guid IID_ID3D12Device = new Guid(0x189819f1, 0x1db6, 0x4b57, 0xbe, 0x54, 0x18, 0x21, 0x33, 0x9b, 0x85, 0xf7);
        private static readonly Guid IID_ID3D12Device5 = new Guid(0x8b4f173b, 0x2fea, 0x4b80, 0xb4, 0xc4, 0x52, 0x46, 0xa8, 0xe9, 0xda, 0x52);

        // DirectX 12 COM interface declarations (simplified - full implementation would require complete COM interop)
        [ComImport]
        [Guid("189819f1-1db6-4b57-be54-1821339b85f7")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ID3D12Device
        {
            // Methods would be declared here in full implementation
            // This is a placeholder structure
        }

        [ComImport]
        [Guid("8b4f173b-2fea-4b80-b4c4-5246a8e9da52")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface ID3D12Device5
        {
            // ID3D12Device5 methods for raytracing
            // This is a placeholder structure
        }

        // DirectX 12 function pointers for P/Invoke (simplified - full implementation requires extensive declarations)
        // In a complete implementation, these would be loaded via GetProcAddress or use SharpDX/Vortice wrapper
        
        #endregion

        private readonly IntPtr _device;
        private readonly IntPtr _device5; // ID3D12Device5 for raytracing
        private readonly IntPtr _commandQueue;
        private readonly GraphicsCapabilities _capabilities;
        private bool _disposed;

        // Resource tracking
        private readonly Dictionary<IntPtr, IResource> _resources;
        private uint _nextResourceHandle;

        // Frame tracking for multi-buffering
        private int _currentFrameIndex;

        // Sampler descriptor heap management
        private IntPtr _samplerDescriptorHeap;
        private IntPtr _samplerHeapCpuStartHandle;
        private uint _samplerHeapDescriptorIncrementSize;
        private int _samplerHeapCapacity;
        private int _samplerHeapNextIndex;
        private const int DefaultSamplerHeapCapacity = 2048;

        // Command signature cache for indirect execution
        // Command signatures are expensive to create, so we cache them per device
        private IntPtr _dispatchIndirectCommandSignature;
        private IntPtr _drawIndirectCommandSignature;
        private IntPtr _drawIndexedIndirectCommandSignature;

        // DSV descriptor heap fields
        private IntPtr _dsvDescriptorHeap;
        private IntPtr _dsvHeapCpuStartHandle;
        private uint _dsvHeapDescriptorIncrementSize;
        private int _dsvHeapCapacity;
        private int _dsvHeapNextIndex;
        private const int DefaultDsvHeapCapacity = 1024;
        private readonly Dictionary<IntPtr, IntPtr> _textureDsvHandles; // Cache of texture -> DSV handle mappings
        private readonly Dictionary<IntPtr, IntPtr> _textureRtvHandles; // Cache of texture -> RTV handle mappings

        // UAV descriptor heap fields
        private IntPtr _uavDescriptorHeap;
        private IntPtr _uavHeapCpuStartHandle;
        private uint _uavHeapDescriptorIncrementSize;
        private int _uavHeapCapacity;
        private int _uavHeapNextIndex;
        private const int DefaultUavHeapCapacity = 1024;
        private readonly Dictionary<IntPtr, IntPtr> _textureUavHandles; // Cache of texture -> UAV handle mappings

        public GraphicsCapabilities Capabilities
        {
            get { return _capabilities; }
        }

        public GraphicsBackend Backend
        {
            get { return GraphicsBackend.Direct3D12; }
        }

        public bool IsValid
        {
            get { return !_disposed && _device != IntPtr.Zero; }
        }

        internal D3D12Device(
            IntPtr device,
            IntPtr device5,
            IntPtr commandQueue,
            GraphicsCapabilities capabilities)
        {
            if (device == IntPtr.Zero)
            {
                throw new ArgumentException("Device handle must be valid", nameof(device));
            }

            _device = device;
            _device5 = device5;
            _commandQueue = commandQueue;
            if (capabilities == null)
            {
                throw new ArgumentNullException(nameof(capabilities));
            }
            _capabilities = capabilities;
            _resources = new Dictionary<IntPtr, IResource>();
            _nextResourceHandle = 1;
            _currentFrameIndex = 0;
            _textureDsvHandles = new Dictionary<IntPtr, IntPtr>();
            _textureRtvHandles = new Dictionary<IntPtr, IntPtr>();
            _textureUavHandles = new Dictionary<IntPtr, IntPtr>();
        }

        #region Resource Creation

        public ITexture CreateTexture(TextureDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            if (desc.Width == 0 || desc.Height == 0)
            {
                throw new ArgumentException("Texture dimensions must be greater than zero", nameof(desc));
            }

            // Platform check: DirectX 12 COM is Windows-only
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                // On non-Windows platforms, return a texture with zero handle
                // The application should use VulkanDevice for cross-platform support
                IntPtr handle = new IntPtr(_nextResourceHandle++);
                var texture = new D3D12Texture(handle, desc, IntPtr.Zero, IntPtr.Zero, _device);
                _resources[handle] = texture;
                return texture;
            }

            // Convert TextureFormat to DXGI_FORMAT for texture resource creation
            uint dxgiFormat = ConvertTextureFormatToDxgiFormatForTexture(desc.Format);
            if (dxgiFormat == 0) // DXGI_FORMAT_UNKNOWN
            {
                throw new NotSupportedException($"Texture format {desc.Format} is not supported for D3D12 texture creation");
            }

            // Map TextureDimension to D3D12_RESOURCE_DIMENSION
            uint resourceDimension = MapTextureDimensionToD3D12(desc.Dimension);

            // Determine heap type - textures almost always use DEFAULT heap (GPU-only, best performance)
            // Upload/Readback heaps are typically only used for staging textures
            uint heapType = D3D12_HEAP_TYPE_DEFAULT;

            // Create heap properties
            D3D12_HEAP_PROPERTIES heapProperties = new D3D12_HEAP_PROPERTIES
            {
                Type = heapType,
                CPUPageProperty = D3D12_CPU_PAGE_PROPERTY_UNKNOWN,
                MemoryPoolPreference = D3D12_MEMORY_POOL_UNKNOWN,
                CreationNodeMask = 0,
                VisibleNodeMask = 0
            };

            // Convert TextureDesc to D3D12_RESOURCE_DESC
            D3D12_RESOURCE_DESC resourceDesc = new D3D12_RESOURCE_DESC
            {
                Dimension = resourceDimension,
                Alignment = 0, // D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT (0 means default alignment)
                Width = unchecked((ulong)desc.Width),
                Height = unchecked((uint)desc.Height),
                DepthOrArraySize = unchecked((ushort)(desc.Dimension == TextureDimension.Texture3D ? desc.Depth : desc.ArraySize)),
                MipLevels = unchecked((ushort)desc.MipLevels),
                Format = dxgiFormat,
                SampleDesc = new D3D12_SAMPLE_DESC
                {
                    Count = unchecked((uint)desc.SampleCount),
                    Quality = 0 // Standard quality
                },
                Layout = 0, // D3D12_TEXTURE_LAYOUT_UNKNOWN (0 means default layout)
                Flags = D3D12_RESOURCE_FLAG_NONE
            };

            // Set resource flags based on usage
            if ((desc.Usage & TextureUsage.RenderTarget) != 0)
            {
                resourceDesc.Flags |= D3D12_RESOURCE_FLAG_ALLOW_RENDER_TARGET;
            }

            if ((desc.Usage & TextureUsage.DepthStencil) != 0)
            {
                resourceDesc.Flags |= D3D12_RESOURCE_FLAG_ALLOW_DEPTH_STENCIL;
            }

            if ((desc.Usage & TextureUsage.UnorderedAccess) != 0)
            {
                resourceDesc.Flags |= D3D12_RESOURCE_FLAG_ALLOW_UNORDERED_ACCESS;
            }

            // Determine initial resource state
            uint initialResourceState = MapResourceStateToD3D12(desc.InitialState);
            if (initialResourceState == 0)
            {
                // Default to COMMON state if no initial state specified
                initialResourceState = D3D12_RESOURCE_STATE_COMMON;
            }

            // Create optimized clear value if this is a render target or depth stencil
            // Note: ClearValue is a struct, so we check if it's been initialized (non-zero depth or non-zero color)
            IntPtr optimizedClearValuePtr = IntPtr.Zero;
            bool hasClearValue = false;
            bool clearValueIsValid = (desc.ClearValue.Depth != 0.0f || desc.ClearValue.R != 0.0f || 
                                     desc.ClearValue.G != 0.0f || desc.ClearValue.B != 0.0f || 
                                     desc.ClearValue.A != 0.0f || desc.ClearValue.Stencil != 0);
            
            if ((desc.Usage & (TextureUsage.RenderTarget | TextureUsage.DepthStencil)) != 0 && clearValueIsValid)
            {
                D3D12_CLEAR_VALUE clearValue = new D3D12_CLEAR_VALUE
                {
                    Format = dxgiFormat
                };

                if ((desc.Usage & TextureUsage.DepthStencil) != 0)
                {
                    // Depth-stencil clear value
                    clearValue.DepthStencil = new D3D12_DEPTH_STENCIL_VALUE
                    {
                        Depth = desc.ClearValue.Depth,
                        Stencil = desc.ClearValue.Stencil
                    };
                }
                else
                {
                    // Render target clear value (color)
                    clearValue.Color = new float[4]
                    {
                        desc.ClearValue.R,
                        desc.ClearValue.G,
                        desc.ClearValue.B,
                        desc.ClearValue.A
                    };
                }

                int clearValueSize = Marshal.SizeOf(typeof(D3D12_CLEAR_VALUE));
                optimizedClearValuePtr = Marshal.AllocHGlobal(clearValueSize);
                Marshal.StructureToPtr(clearValue, optimizedClearValuePtr, false);
                hasClearValue = true;
            }

            // Allocate memory for structures
            int heapPropertiesSize = Marshal.SizeOf(typeof(D3D12_HEAP_PROPERTIES));
            IntPtr heapPropertiesPtr = Marshal.AllocHGlobal(heapPropertiesSize);
            int resourceDescSize = Marshal.SizeOf(typeof(D3D12_RESOURCE_DESC));
            IntPtr resourceDescPtr = Marshal.AllocHGlobal(resourceDescSize);
            IntPtr resourcePtr = Marshal.AllocHGlobal(IntPtr.Size);

            IntPtr d3d12Resource = IntPtr.Zero;
            IntPtr srvHandle = IntPtr.Zero;

            try
            {
                // Marshal structures to unmanaged memory
                Marshal.StructureToPtr(heapProperties, heapPropertiesPtr, false);
                Marshal.StructureToPtr(resourceDesc, resourceDescPtr, false);

                // IID_ID3D12Resource
                Guid iidResource = new Guid("696442be-a72e-4059-bc79-5b5c98040fad");

                // Call CreateCommittedResource
                int hr = CallCreateCommittedResource(
                    _device,
                    heapPropertiesPtr,
                    D3D12_HEAP_FLAG_NONE,
                    resourceDescPtr,
                    initialResourceState,
                    hasClearValue ? optimizedClearValuePtr : IntPtr.Zero,
                    ref iidResource,
                    resourcePtr);

                if (hr < 0)
                {
                    throw new InvalidOperationException($"CreateCommittedResource failed with HRESULT 0x{hr:X8}");
                }

                // Get the created resource pointer
                d3d12Resource = Marshal.ReadIntPtr(resourcePtr);
                if (d3d12Resource == IntPtr.Zero)
                {
                    throw new InvalidOperationException("CreateCommittedResource returned null resource pointer");
                }

                // Create SRV descriptor if texture is used as shader resource
                if ((desc.Usage & TextureUsage.ShaderResource) != 0)
                {
                    srvHandle = CreateSrvDescriptorForTexture(d3d12Resource, desc, dxgiFormat, resourceDimension);
                }

                // Create texture wrapper
                IntPtr handle = new IntPtr(_nextResourceHandle++);
                var texture = new D3D12Texture(handle, desc, d3d12Resource, srvHandle, _device);
                _resources[handle] = texture;

                return texture;
            }
            finally
            {
                // Free allocated memory
                if (heapPropertiesPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(heapPropertiesPtr);
                }
                if (resourceDescPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(resourceDescPtr);
                }
                if (resourcePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(resourcePtr);
                }
                if (optimizedClearValuePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(optimizedClearValuePtr);
                }
            }
        }

        public IBuffer CreateBuffer(BufferDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            if (desc.ByteSize == 0)
            {
                throw new ArgumentException("Buffer size must be greater than zero", nameof(desc));
            }

            // Platform check: DirectX 12 COM is Windows-only
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                // On non-Windows platforms, return a buffer with zero handle
                // The application should use VulkanDevice for cross-platform support
            IntPtr handle = new IntPtr(_nextResourceHandle++);
            var buffer = new D3D12Buffer(handle, desc, IntPtr.Zero, _device);
            _resources[handle] = buffer;
                return buffer;
            }

            // Determine heap type based on usage
            // DEFAULT: GPU-accessible, best performance for GPU-only resources
            // UPLOAD: CPU-writable, GPU-readable (for dynamic/staging buffers)
            // READBACK: CPU-readable, GPU-writable (for reading back GPU results)
            uint heapType = D3D12_HEAP_TYPE_DEFAULT; // Default to GPU-only heap for best performance

            // Check if buffer needs CPU access for upload/staging
            // Note: In practice, upload heaps are typically used for dynamic buffers that are updated every frame
            // For now, we default to DEFAULT heap unless explicitly needed for staging
            // TODO: Consider adding explicit staging flags to BufferDesc if needed

            // Create heap properties
            D3D12_HEAP_PROPERTIES heapProperties = new D3D12_HEAP_PROPERTIES
            {
                Type = heapType,
                CPUPageProperty = D3D12_CPU_PAGE_PROPERTY_UNKNOWN,
                MemoryPoolPreference = D3D12_MEMORY_POOL_UNKNOWN,
                CreationNodeMask = 0,
                VisibleNodeMask = 0
            };

            // Convert BufferDesc to D3D12_RESOURCE_DESC
            D3D12_RESOURCE_DESC resourceDesc = new D3D12_RESOURCE_DESC
            {
                Dimension = D3D12_RESOURCE_DIMENSION_BUFFER,
                Alignment = 0, // D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT (0 means default alignment)
                Width = (ulong)desc.ByteSize, // Buffer size in bytes
                Height = 1,
                DepthOrArraySize = 1,
                MipLevels = 1,
                Format = 0, // DXGI_FORMAT_UNKNOWN - buffers don't use formats
                SampleDesc = new D3D12_SAMPLE_DESC { Count = 1, Quality = 0 },
                Layout = 0, // D3D12_TEXTURE_LAYOUT_ROW_MAJOR - not used for buffers (0 = undefined)
                Flags = D3D12_RESOURCE_FLAG_NONE
            };

            // Add unordered access flag if buffer can be used as UAV
            if ((desc.Usage & BufferUsageFlags.UnorderedAccess) != 0)
            {
                resourceDesc.Flags |= D3D12_RESOURCE_FLAG_ALLOW_UNORDERED_ACCESS;
            }

            // Determine initial resource state
            uint initialResourceState = MapResourceStateToD3D12(desc.InitialState);
            if (initialResourceState == 0)
            {
                // Default to COMMON state if no initial state specified
                initialResourceState = D3D12_RESOURCE_STATE_COMMON;
            }

            // Allocate memory for structures
            int heapPropertiesSize = Marshal.SizeOf(typeof(D3D12_HEAP_PROPERTIES));
            IntPtr heapPropertiesPtr = Marshal.AllocHGlobal(heapPropertiesSize);
            int resourceDescSize = Marshal.SizeOf(typeof(D3D12_RESOURCE_DESC));
            IntPtr resourceDescPtr = Marshal.AllocHGlobal(resourceDescSize);
            IntPtr resourcePtr = Marshal.AllocHGlobal(IntPtr.Size);

            try
            {
                // Marshal structures to unmanaged memory
                Marshal.StructureToPtr(heapProperties, heapPropertiesPtr, false);
                Marshal.StructureToPtr(resourceDesc, resourceDescPtr, false);

                // IID_ID3D12Resource
                Guid iidResource = new Guid("696442be-a72e-4059-bc79-5b5c98040fad");

                // Call CreateCommittedResource
                int hr = CallCreateCommittedResource(
                    _device,
                    heapPropertiesPtr,
                    D3D12_HEAP_FLAG_NONE,
                    resourceDescPtr,
                    initialResourceState,
                    IntPtr.Zero, // pOptimizedClearValue - not needed for buffers
                    ref iidResource,
                    resourcePtr);

                if (hr < 0)
                {
                    throw new InvalidOperationException($"CreateCommittedResource failed with HRESULT 0x{hr:X8}");
                }

                // Get the created resource pointer
                IntPtr d3d12Resource = Marshal.ReadIntPtr(resourcePtr);
                if (d3d12Resource == IntPtr.Zero)
                {
                    throw new InvalidOperationException("CreateCommittedResource returned null resource pointer");
                }

                // Create buffer wrapper
                IntPtr handle = new IntPtr(_nextResourceHandle++);
                var buffer = new D3D12Buffer(handle, desc, d3d12Resource, _device);
                _resources[handle] = buffer;

            return buffer;
            }
            finally
            {
                // Free allocated memory
                if (heapPropertiesPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(heapPropertiesPtr);
                }
                if (resourceDescPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(resourceDescPtr);
                }
                if (resourcePtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(resourcePtr);
                }
            }
        }

        public ISampler CreateSampler(SamplerDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            // Platform check: DirectX 12 COM is Windows-only
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                // On non-Windows platforms, return a sampler with zero handle
                // The application should use VulkanDevice for cross-platform support
                IntPtr nonWindowsHandle = new IntPtr(_nextResourceHandle++);
                var nonWindowsSampler = new D3D12Sampler(nonWindowsHandle, desc, IntPtr.Zero, _device);
                _resources[nonWindowsHandle] = nonWindowsSampler;
                return nonWindowsSampler;
            }

            // Convert SamplerDesc to D3D12_SAMPLER_DESC
            D3D12_SAMPLER_DESC d3d12SamplerDesc = ConvertSamplerDescToD3D12(desc);

            // Allocate descriptor handle from sampler heap
            IntPtr cpuDescriptorHandle = AllocateSamplerDescriptor();
            if (cpuDescriptorHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to allocate sampler descriptor from heap");
            }

            // Create sampler descriptor in the allocated slot
            try
            {
                // Allocate memory for the sampler descriptor structure
                int samplerDescSize = Marshal.SizeOf(typeof(D3D12_SAMPLER_DESC));
                IntPtr samplerDescPtr = Marshal.AllocHGlobal(samplerDescSize);
                try
                {
                    Marshal.StructureToPtr(d3d12SamplerDesc, samplerDescPtr, false);

                    // Call ID3D12Device::CreateSampler to create the sampler descriptor
                    CallCreateSampler(_device, samplerDescPtr, cpuDescriptorHandle);
                }
                finally
                {
                    Marshal.FreeHGlobal(samplerDescPtr);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create D3D12 sampler descriptor: {ex.Message}", ex);
            }

            // Wrap in D3D12Sampler and return
            IntPtr handle = new IntPtr(_nextResourceHandle++);
            var sampler = new D3D12Sampler(handle, desc, cpuDescriptorHandle, _device);
            _resources[handle] = sampler;

            return sampler;
        }

        public IShader CreateShader(ShaderDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            if (desc.Bytecode == null || desc.Bytecode.Length == 0)
            {
                throw new ArgumentException("Shader bytecode must be provided", nameof(desc));
            }

            // D3D12 doesn't create shader objects directly - shaders are part of PSO
            // This method stores the bytecode for later use in pipeline creation
            IntPtr handle = new IntPtr(_nextResourceHandle++);
            var shader = new D3D12Shader(handle, desc, _device);
            _resources[handle] = shader;

            return shader;
        }

        public IGraphicsPipeline CreateGraphicsPipeline(GraphicsPipelineDesc desc, IFramebuffer framebuffer)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            // Create D3D12 graphics pipeline state object
            // Based on DirectX 12 Graphics Pipeline State: https://docs.microsoft.com/en-us/windows/win32/api/d3d12/nf-d3d12-id3d12device-creategraphicspipelinestate
            // VTable index 43 for ID3D12Device::CreateGraphicsPipelineState
            // Based on daorigins.exe/DragonAge2.exe: Graphics pipeline creation for rendering

            // Platform check: DirectX 12 COM is Windows-only
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                // On non-Windows platforms, return a pipeline with zero handles
                IntPtr handle = new IntPtr(_nextResourceHandle++);
                var pipeline = new D3D12GraphicsPipeline(handle, desc, IntPtr.Zero, IntPtr.Zero, _device, this);
                _resources[handle] = pipeline;
                return pipeline;
            }

            IntPtr rootSignature = IntPtr.Zero;
            IntPtr pipelineState = IntPtr.Zero;

            try
            {
                // Step 1: Get or create root signature from binding layouts
                // Note: CreateBindingLayout also has a TODO for root signature creation
                // For now, we try to get root signature from binding layouts if they exist
                if (desc.BindingLayouts != null && desc.BindingLayouts.Length > 0)
                {
                    // Get root signature from first binding layout (pipeline typically uses one root signature)
                    // In a full implementation, multiple root signatures would be combined
                    var firstLayout = desc.BindingLayouts[0] as D3D12BindingLayout;
                    if (firstLayout != null)
                    {
                        // D3D12BindingLayout stores root signature internally
                        // We need to access it via a method or property
                        // For now, we'll need to create root signature if not already created
                        // This will be fully implemented when CreateBindingLayout is implemented
                        rootSignature = IntPtr.Zero; // Will be set when root signature is created
                    }
                }

                // Step 2: Convert GraphicsPipelineDesc to D3D12_GRAPHICS_PIPELINE_STATE_DESC
                var pipelineDesc = ConvertGraphicsPipelineDescToD3D12(desc, framebuffer, rootSignature);

                // Step 3: Create the pipeline state object
                IntPtr pipelineStatePtr = Marshal.AllocHGlobal(IntPtr.Size);
                try
                {
                    Guid iidPipelineState = IID_ID3D12PipelineState;
                    int hr = CallCreateGraphicsPipelineState(_device, ref pipelineDesc, ref iidPipelineState, pipelineStatePtr);
                    if (hr < 0)
                    {
                        throw new InvalidOperationException($"CreateGraphicsPipelineState failed with HRESULT 0x{hr:X8}");
                    }

                    pipelineState = Marshal.ReadIntPtr(pipelineStatePtr);
                    if (pipelineState == IntPtr.Zero)
                    {
                        throw new InvalidOperationException("Pipeline state pointer is null");
                    }
                }
                finally
                {
                    // Free marshalled structures
                    FreeGraphicsPipelineStateDesc(ref pipelineDesc);
                    Marshal.FreeHGlobal(pipelineStatePtr);
                }

            IntPtr handle = new IntPtr(_nextResourceHandle++);
                var pipeline = new D3D12GraphicsPipeline(handle, desc, pipelineState, rootSignature, _device, this);
            _resources[handle] = pipeline;

            return pipeline;
            }
            catch (Exception ex)
            {
                // Clean up on failure - release any successfully created COM objects
                if (pipelineState != IntPtr.Zero)
                {
                    try
                    {
                        ReleaseComObject(pipelineState);
                    }
                    catch (Exception releaseEx)
                    {
                        // Log error but continue cleanup
                        Console.WriteLine($"[D3D12Device] Error releasing pipeline state in error handler: {releaseEx.Message}");
                    }
                }

                // Note: rootSignature is passed to the pipeline constructor, so it will be released by the pipeline's Dispose()
                // If rootSignature needs to be released here (when not passed to pipeline), it should be released above
                // Currently rootSignature is typically IntPtr.Zero at this point due to incomplete CreateBindingLayout implementation

                // Return pipeline with zero handles on failure (allows graceful degradation)
                IntPtr handle = new IntPtr(_nextResourceHandle++);
                var pipeline = new D3D12GraphicsPipeline(handle, desc, IntPtr.Zero, rootSignature, _device);
                _resources[handle] = pipeline;
                
                Console.WriteLine($"[D3D12Device] WARNING: Failed to create graphics pipeline state: {ex.Message}");
                return pipeline;
            }
        }

        public IComputePipeline CreateComputePipeline(ComputePipelineDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            // TODO: IMPLEMENT - Create D3D12 compute pipeline state object
            // - Convert ComputePipelineDesc to D3D12_COMPUTE_PIPELINE_STATE_DESC
            // - Set compute shader bytecode
            // - Convert RootSignature from BindingLayouts
            // - Call ID3D12Device::CreateComputePipelineState
            // - Wrap in D3D12ComputePipeline and return

            IntPtr handle = new IntPtr(_nextResourceHandle++);
            var pipeline = new D3D12ComputePipeline(handle, desc, IntPtr.Zero, IntPtr.Zero, _device);
            _resources[handle] = pipeline;

            return pipeline;
        }

        public IFramebuffer CreateFramebuffer(FramebufferDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            // D3D12 doesn't use framebuffers - uses render targets directly
            // This wrapper stores attachment information for compatibility
            IntPtr handle = new IntPtr(_nextResourceHandle++);
            var framebuffer = new D3D12Framebuffer(handle, desc);
            _resources[handle] = framebuffer;

            return framebuffer;
        }

        public IBindingLayout CreateBindingLayout(BindingLayoutDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            if (desc.Items == null || desc.Items.Length == 0)
            {
                throw new ArgumentException("Binding layout must have at least one item", nameof(desc));
            }

            // Platform check: DirectX 12 COM is Windows-only
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new PlatformNotSupportedException("DirectX 12 binding layouts are only supported on Windows");
            }

            // Convert BindingLayoutItems to D3D12 root parameters
            // Group items by type into descriptor tables for efficiency
            var rootParameters = new List<D3D12_ROOT_PARAMETER>();
            var descriptorRanges = new List<List<D3D12_DESCRIPTOR_RANGE>>();
            var rangePointers = new List<IntPtr>();

            // Group items by binding type and shader visibility
            var groupedItems = new Dictionary<uint, List<BindingLayoutItem>>(); // Key: (ShaderVisibility << 16) | BindingType

            foreach (var item in desc.Items)
            {
                uint bindingType = ConvertBindingTypeToD3D12RangeType(item.Type);
                uint shaderVisibility = ConvertShaderStageFlagsToD3D12Visibility(item.Stages);
                uint key = (shaderVisibility << 16) | bindingType;

                if (!groupedItems.ContainsKey(key))
                {
                    groupedItems[key] = new List<BindingLayoutItem>();
                }
                groupedItems[key].Add(item);
            }

            // Create descriptor tables for each group
            foreach (var group in groupedItems)
            {
                uint shaderVisibility = (group.Key >> 16) & 0xFFFF;
                uint bindingType = group.Key & 0xFFFF;
                var items = group.Value;

                // Sort items by slot
                items.Sort((a, b) => a.Slot.CompareTo(b.Slot));

                // Create descriptor ranges for this group
                var ranges = new List<D3D12_DESCRIPTOR_RANGE>();
                uint currentOffset = 0;
                foreach (var item in items)
                {
                    var range = new D3D12_DESCRIPTOR_RANGE
                    {
                        RangeType = bindingType,
                        NumDescriptors = (uint)item.Count,
                        BaseShaderRegister = (uint)item.Slot,
                        RegisterSpace = 0, // Default register space
                        OffsetInDescriptorsFromTableStart = currentOffset
                    };
                    ranges.Add(range);
                    currentOffset += (uint)item.Count;
                }

                if (ranges.Count > 0)
                {
                    // Allocate memory for descriptor ranges array
                    int rangesSize = Marshal.SizeOf(typeof(D3D12_DESCRIPTOR_RANGE)) * ranges.Count;
                    IntPtr rangesPtr = Marshal.AllocHGlobal(rangesSize);
                    for (int i = 0; i < ranges.Count; i++)
                    {
                        IntPtr rangePtr = new IntPtr(rangesPtr.ToInt64() + i * Marshal.SizeOf(typeof(D3D12_DESCRIPTOR_RANGE)));
                        Marshal.StructureToPtr(ranges[i], rangePtr, false);
                    }

                    rangePointers.Add(rangesPtr);
                    descriptorRanges.Add(ranges);

                    // Create root parameter for this descriptor table
                    var rootParam = new D3D12_ROOT_PARAMETER
                    {
                        ParameterType = D3D12_ROOT_PARAMETER_TYPE_DESCRIPTOR_TABLE,
                        ShaderVisibility = shaderVisibility
                    };
                    rootParam.DescriptorTable.NumDescriptorRanges = (uint)ranges.Count;
                    rootParam.DescriptorTable.pDescriptorRanges = rangesPtr;

                    rootParameters.Add(rootParam);
                }
            }

            // Create root signature description
            int rootParamsSize = Marshal.SizeOf(typeof(D3D12_ROOT_PARAMETER)) * rootParameters.Count;
            IntPtr rootParamsPtr = Marshal.AllocHGlobal(rootParamsSize);
            try
            {
                for (int i = 0; i < rootParameters.Count; i++)
                {
                    IntPtr paramPtr = new IntPtr(rootParamsPtr.ToInt64() + i * Marshal.SizeOf(typeof(D3D12_ROOT_PARAMETER)));
                    Marshal.StructureToPtr(rootParameters[i], paramPtr, false);
                }

                var rootSignatureDesc = new D3D12_ROOT_SIGNATURE_DESC
                {
                    NumParameters = (uint)rootParameters.Count,
                    pParameters = rootParamsPtr,
                    NumStaticSamplers = 0,
                    pStaticSamplers = IntPtr.Zero,
                    Flags = D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT
                };

                // Serialize root signature
                IntPtr pRootSignatureDesc = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(D3D12_ROOT_SIGNATURE_DESC)));
                try
                {
                    Marshal.StructureToPtr(rootSignatureDesc, pRootSignatureDesc, false);

                    IntPtr blobPtr;
                    IntPtr errorBlobPtr;
                    int hr = D3D12SerializeRootSignature(pRootSignatureDesc, D3D_ROOT_SIGNATURE_VERSION_1_0, out blobPtr, out errorBlobPtr);

                    if (hr < 0)
                    {
                        // Free allocated memory
                        foreach (var ptr in rangePointers)
                        {
                            Marshal.FreeHGlobal(ptr);
                        }
                        throw new InvalidOperationException($"D3D12SerializeRootSignature failed with HRESULT 0x{hr:X8}");
                    }

                    if (errorBlobPtr != IntPtr.Zero)
                    {
                        // Log error blob if available (would need ID3DBlob interface to read)
                        // For now, just release it
                        ReleaseComObject(errorBlobPtr);
                    }

                    if (blobPtr == IntPtr.Zero)
                    {
                        // Free allocated memory
                        foreach (var ptr in rangePointers)
                        {
                            Marshal.FreeHGlobal(ptr);
                        }
                        throw new InvalidOperationException("D3D12SerializeRootSignature returned null blob");
                    }

                    // Get blob size (ID3DBlob::GetBufferSize is at vtable index 3)
                    IntPtr* blobVtable = *(IntPtr**)blobPtr;
                    IntPtr getBufferSizePtr = blobVtable[3];
                    GetBufferSizeDelegate getBufferSize = (GetBufferSizeDelegate)Marshal.GetDelegateForFunctionPointer(getBufferSizePtr, typeof(GetBufferSizeDelegate));
                    IntPtr blobSize = getBufferSize(blobPtr);

                    // Get blob buffer (ID3DBlob::GetBufferPointer is at vtable index 4)
                    IntPtr getBufferPointerPtr = blobVtable[4];
                    GetBufferPointerDelegate getBufferPointer = (GetBufferPointerDelegate)Marshal.GetDelegateForFunctionPointer(getBufferPointerPtr, typeof(GetBufferPointerDelegate));
                    IntPtr blobBuffer = getBufferPointer(blobPtr);

                    // Create root signature
                    Guid iidRootSignature = new Guid(0xc54a6b66, 0x72df, 0x4ee8, 0x8b, 0xe5, 0xa9, 0x46, 0xa1, 0x42, 0x92, 0x14); // IID_ID3D12RootSignature
                    IntPtr rootSignaturePtr = Marshal.AllocHGlobal(IntPtr.Size);
                    try
                    {
                        hr = CallCreateRootSignature(_device, blobBuffer, blobSize, ref iidRootSignature, rootSignaturePtr);
                        if (hr < 0)
                        {
                            ReleaseComObject(blobPtr);
                            // Free allocated memory
                            foreach (var ptr in rangePointers)
                            {
                                Marshal.FreeHGlobal(ptr);
                            }
                            throw new InvalidOperationException($"CreateRootSignature failed with HRESULT 0x{hr:X8}");
                        }

                        IntPtr rootSignature = Marshal.ReadIntPtr(rootSignaturePtr);
                        if (rootSignature == IntPtr.Zero)
                        {
                            ReleaseComObject(blobPtr);
                            // Free allocated memory
                            foreach (var ptr in rangePointers)
                            {
                                Marshal.FreeHGlobal(ptr);
                            }
                            throw new InvalidOperationException("CreateRootSignature returned null root signature pointer");
                        }

                        // Release blob (no longer needed after root signature is created)
                        ReleaseComObject(blobPtr);

                        // Wrap in D3D12BindingLayout and return
            IntPtr handle = new IntPtr(_nextResourceHandle++);
                        var layout = new D3D12BindingLayout(handle, desc, rootSignature, _device, this);
            _resources[handle] = layout;

                        // Store range pointers for cleanup on disposal (would need to track these in D3D12BindingLayout)
                        // For now, we'll free them immediately after root signature creation
                        // In a full implementation, these would be stored and freed when the layout is disposed
                        foreach (var ptr in rangePointers)
                        {
                            Marshal.FreeHGlobal(ptr);
                        }

            return layout;
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(rootSignaturePtr);
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(pRootSignatureDesc);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(rootParamsPtr);
            }
        }

        // Helper delegates for ID3DBlob interface
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr GetBufferSizeDelegate(IntPtr blob);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate IntPtr GetBufferPointerDelegate(IntPtr blob);

        /// <summary>
        /// Converts BindingType to D3D12_DESCRIPTOR_RANGE_TYPE.
        /// </summary>
        private uint ConvertBindingTypeToD3D12RangeType(BindingType type)
        {
            switch (type)
            {
                case BindingType.ConstantBuffer:
                    return D3D12_DESCRIPTOR_RANGE_TYPE_CBV;
                case BindingType.Texture:
                    return D3D12_DESCRIPTOR_RANGE_TYPE_SRV;
                case BindingType.Sampler:
                    return D3D12_DESCRIPTOR_RANGE_TYPE_SAMPLER;
                case BindingType.RWTexture:
                case BindingType.RWBuffer:
                    return D3D12_DESCRIPTOR_RANGE_TYPE_UAV;
                case BindingType.StructuredBuffer:
                    return D3D12_DESCRIPTOR_RANGE_TYPE_SRV; // Structured buffers are SRVs
                case BindingType.AccelStruct:
                    return D3D12_DESCRIPTOR_RANGE_TYPE_SRV; // Acceleration structures are SRVs
                default:
                    return D3D12_DESCRIPTOR_RANGE_TYPE_SRV; // Default to SRV
            }
        }

        /// <summary>
        /// Converts ShaderStageFlags to D3D12_SHADER_VISIBILITY.
        /// </summary>
        private uint ConvertShaderStageFlagsToD3D12Visibility(ShaderStageFlags stages)
        {
            // If all graphics stages are present, use ALL
            if ((stages & ShaderStageFlags.AllGraphics) == ShaderStageFlags.AllGraphics)
            {
                return D3D12_SHADER_VISIBILITY_ALL;
            }

            // Check individual stages (priority order: Pixel > Geometry > Domain > Hull > Vertex)
            if ((stages & ShaderStageFlags.Pixel) != 0)
            {
                return D3D12_SHADER_VISIBILITY_PIXEL;
            }
            if ((stages & ShaderStageFlags.Geometry) != 0)
            {
                return D3D12_SHADER_VISIBILITY_GEOMETRY;
            }
            if ((stages & ShaderStageFlags.Domain) != 0)
            {
                return D3D12_SHADER_VISIBILITY_DOMAIN;
            }
            if ((stages & ShaderStageFlags.Hull) != 0)
            {
                return D3D12_SHADER_VISIBILITY_HULL;
            }
            if ((stages & ShaderStageFlags.Vertex) != 0)
            {
                return D3D12_SHADER_VISIBILITY_VERTEX;
            }
            if ((stages & ShaderStageFlags.Compute) != 0)
            {
                return D3D12_SHADER_VISIBILITY_ALL; // Compute shaders use ALL visibility
            }

            // Default to ALL if no specific stage is set
            return D3D12_SHADER_VISIBILITY_ALL;
        }
        }

        public IBindingSet CreateBindingSet(IBindingLayout layout, BindingSetDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            if (layout == null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            // TODO: IMPLEMENT - Allocate and populate D3D12 descriptor set
            // - Allocate descriptor handles from descriptor heap
            // - Create SRV/UAV/CBV descriptors using CreateShaderResourceView, CreateUnorderedAccessView, CreateConstantBufferView
            // - Wrap in D3D12BindingSet and return

            IntPtr handle = new IntPtr(_nextResourceHandle++);
            var bindingSet = new D3D12BindingSet(handle, layout, desc, IntPtr.Zero, _device, this);
            _resources[handle] = bindingSet;

            return bindingSet;
        }

        public ICommandList CreateCommandList(CommandListType type = CommandListType.Graphics)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            // Platform check: DirectX 12 COM is Windows-only
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new PlatformNotSupportedException("DirectX 12 command lists are only supported on Windows");
            }

            // Map CommandListType to D3D12_COMMAND_LIST_TYPE
            uint d3d12CommandListType = MapCommandListTypeToD3D12(type);

            // Create command allocator for this command list
            // Each command list needs its own allocator (allocators can be reused after command lists are executed)
            IntPtr commandAllocatorPtr = Marshal.AllocHGlobal(IntPtr.Size);
            try
            {
                Guid iidCommandAllocator = IID_ID3D12CommandAllocator;
                int hr = CallCreateCommandAllocator(_device, d3d12CommandListType, ref iidCommandAllocator, commandAllocatorPtr);
                if (hr < 0)
                {
                    throw new InvalidOperationException($"CreateCommandAllocator failed with HRESULT 0x{hr:X8}");
                }

                IntPtr commandAllocator = Marshal.ReadIntPtr(commandAllocatorPtr);
                if (commandAllocator == IntPtr.Zero)
                {
                    throw new InvalidOperationException("CreateCommandAllocator returned null allocator pointer");
                }

                // Create command list with the allocator
                IntPtr commandListPtr = Marshal.AllocHGlobal(IntPtr.Size);
                try
                {
                    Guid iidCommandList = IID_ID3D12GraphicsCommandList;
                    // nodeMask: 0 for single GPU, pInitialState: NULL (no initial pipeline state)
                    hr = CallCreateCommandList(_device, 0, d3d12CommandListType, commandAllocator, IntPtr.Zero, ref iidCommandList, commandListPtr);
                    if (hr < 0)
                    {
                        // Release the allocator if command list creation fails
                        ReleaseComObject(commandAllocator);
                        throw new InvalidOperationException($"CreateCommandList failed with HRESULT 0x{hr:X8}");
                    }

                    IntPtr commandList = Marshal.ReadIntPtr(commandListPtr);
                    if (commandList == IntPtr.Zero)
                    {
                        // Release the allocator if command list creation fails
                        ReleaseComObject(commandAllocator);
                        throw new InvalidOperationException("CreateCommandList returned null command list pointer");
                    }

                    // Wrap in D3D12CommandList and return
            IntPtr handle = new IntPtr(_nextResourceHandle++);
                    var cmdList = new D3D12CommandList(handle, type, this, commandList, commandAllocator, _device);
            _resources[handle] = cmdList;

            return cmdList;
                }
                finally
                {
                    Marshal.FreeHGlobal(commandListPtr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(commandAllocatorPtr);
            }
        }

        public ITexture CreateHandleForNativeTexture(IntPtr nativeHandle, TextureDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            if (nativeHandle == IntPtr.Zero)
            {
                throw new ArgumentException("Native handle must be valid", nameof(nativeHandle));
            }

            // Wrap existing native texture (e.g., from swapchain)
            IntPtr handle = new IntPtr(_nextResourceHandle++);
            var texture = new D3D12Texture(handle, desc, nativeHandle);
            _resources[handle] = texture;

            return texture;
        }

        #endregion

        #region Raytracing Resources

        public IAccelStruct CreateAccelStruct(AccelStructDesc desc)
        {
            if (!IsValid)
            {
                throw new ObjectDisposedException(nameof(D3D12Device));
            }

            if (!_capabilities.SupportsRaytracing)
            {
                throw new NotSupportedException("Raytracing is not supported on this device");
            }

            if (_device5 == IntPtr.Zero)
            {
                throw new InvalidOperationException("ID3D12Device5 is not available for raytracing");
            }

            // Platform check: DirectX 12 COM is Windows-only
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new PlatformNotSupportedException("D3D12 acceleration structures are only supported on Windows");
            }

            IntPtr handle = new IntPtr(_nextResourceHandle++);

            try
            {
                if (desc.IsTopLevel)
                {
                    // Create Top-Level Acceleration Structure (TLAS)
                    return CreateTopLevelAccelStruct(handle, desc);
                }
                else
                {
                    // Create Bottom-Level Acceleration Structure (BLAS)
                    return CreateBottomLevelAccelStruct(handle, desc);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create acceleration structure: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Creates a bottom-level acceleration structure (BLAS).
        /// Based on D3D12 DXR API: ID3D12Device5::GetRaytracingAccelerationStructurePrebuildInfo
        /// </summary>
        private IAccelStruct CreateBottomLevelAccelStruct(IntPtr handle, AccelStructDesc desc)
        {
            if (desc.BottomLevelGeometries == null || desc.BottomLevelGeometries.Length == 0)
            {
                throw new ArgumentException("Bottom-level acceleration structure requires at least one geometry", nameof(desc));
            }

            // Convert GeometryDesc[] to D3D12_RAYTRACING_GEOMETRY_DESC[]
            int geometryCount = desc.BottomLevelGeometries.Length;
            int geometryDescSize = Marshal.SizeOf(typeof(D3D12_RAYTRACING_GEOMETRY_DESC));
            IntPtr geometryDescsPtr = Marshal.AllocHGlobal(geometryDescSize * geometryCount);

            try
            {
                IntPtr currentGeometryPtr = geometryDescsPtr;
                for (int i = 0; i < geometryCount; i++)
                {
                    var geometry = desc.BottomLevelGeometries[i];
                    
                    if (geometry.Type != GeometryType.Triangles)
                    {
                        throw new NotSupportedException($"Geometry type {geometry.Type} is not yet supported. Only Triangles are supported.");
                    }

                    var triangles = geometry.Triangles;

                    // Get GPU virtual addresses for buffers
                    IntPtr vertexBufferResource = triangles.VertexBuffer.NativeHandle;
                    if (vertexBufferResource == IntPtr.Zero)
                    {
                        throw new ArgumentException($"Geometry at index {i} has an invalid vertex buffer", nameof(desc));
                    }

                    ulong vertexBufferGpuVa = GetGpuVirtualAddress(vertexBufferResource);
                    if (vertexBufferGpuVa == 0UL)
                    {
                        throw new InvalidOperationException($"Failed to get GPU virtual address for vertex buffer in geometry at index {i}");
                    }

                    // Calculate vertex buffer start address with offset
                    ulong vertexBufferStartAddress = vertexBufferGpuVa + (ulong)triangles.VertexOffset;

                    // Handle index buffer (optional for some geometries)
                    IntPtr indexBufferGpuVa = IntPtr.Zero;
                    uint indexCount = 0;
                    uint indexFormat = D3D12_RAYTRACING_INDEX_FORMAT_UINT32;

                    if (triangles.IndexBuffer != null)
                    {
                        IntPtr indexBufferResource = triangles.IndexBuffer.NativeHandle;
                        if (indexBufferResource != IntPtr.Zero)
                        {
                            ulong indexBufferGpuVaUlong = GetGpuVirtualAddress(indexBufferResource);
                            if (indexBufferGpuVaUlong != 0UL)
                            {
                                indexBufferGpuVa = new IntPtr((long)(indexBufferGpuVaUlong + (ulong)triangles.IndexOffset));
                                indexCount = (uint)triangles.IndexCount;
                                
                                // Determine index format from TextureFormat
                                indexFormat = ConvertIndexFormatToD3D12(triangles.IndexFormat);
                            }
                        }
                    }

                    // Handle transform buffer (optional)
                    IntPtr transformBufferGpuVa = IntPtr.Zero;
                    if (triangles.TransformBuffer != null)
                    {
                        IntPtr transformBufferResource = triangles.TransformBuffer.NativeHandle;
                        if (transformBufferResource != IntPtr.Zero)
                        {
                            ulong transformBufferGpuVaUlong = GetGpuVirtualAddress(transformBufferResource);
                            if (transformBufferGpuVaUlong != 0UL)
                            {
                                transformBufferGpuVa = new IntPtr((long)(transformBufferGpuVaUlong + (ulong)triangles.TransformOffset));
                            }
                        }
                    }

                    // Build D3D12_RAYTRACING_GEOMETRY_TRIANGLES_DESC
                    var trianglesDesc = new D3D12_RAYTRACING_GEOMETRY_TRIANGLES_DESC
                    {
                        Transform3x4 = transformBufferGpuVa,
                        IndexFormat = indexFormat,
                        VertexFormat = ConvertVertexFormatToD3D12(triangles.VertexFormat),
                        IndexCount = indexCount,
                        VertexCount = (uint)triangles.VertexCount,
                        IndexBuffer = indexBufferGpuVa,
                        VertexBuffer = new D3D12_GPU_VIRTUAL_ADDRESS_AND_STRIDE
                        {
                            StartAddress = new IntPtr((long)vertexBufferStartAddress),
                            StrideInBytes = (uint)triangles.VertexStride
                        }
                    };

                    // Build D3D12_RAYTRACING_GEOMETRY_DESC
                    var geometryDesc = new D3D12_RAYTRACING_GEOMETRY_DESC
                    {
                        Type = D3D12_RAYTRACING_GEOMETRY_TYPE_TRIANGLES,
                        Flags = ConvertGeometryFlagsToD3D12(geometry.Flags),
                        Triangles = trianglesDesc
                    };

                    // Marshal structure to unmanaged memory
                    Marshal.StructureToPtr(geometryDesc, currentGeometryPtr, false);
                    currentGeometryPtr = new IntPtr(currentGeometryPtr.ToInt64() + geometryDescSize);
                }

                // Get build flags
                uint buildFlags = ConvertAccelStructBuildFlagsToD3D12(desc.BuildFlags);

                // Build D3D12_BUILD_RAYTRACING_ACCELERATION_STRUCTURE_INPUTS
                var buildInputs = new D3D12_BUILD_RAYTRACING_ACCELERATION_STRUCTURE_INPUTS
                {
                    Type = D3D12_RAYTRACING_ACCELERATION_STRUCTURE_TYPE_BOTTOM_LEVEL,
                    Flags = buildFlags,
                    NumDescs = (uint)geometryCount,
                    DescsLayout = D3D12_ELEMENTS_LAYOUT_ARRAY,
                    pGeometryDescs = geometryDescsPtr
                };

                // Marshal build inputs to unmanaged memory for prebuild info query
                int buildInputsSize = Marshal.SizeOf(typeof(D3D12_BUILD_RAYTRACING_ACCELERATION_STRUCTURE_INPUTS));
                IntPtr buildInputsPtr = Marshal.AllocHGlobal(buildInputsSize);
                try
                {
                    Marshal.StructureToPtr(buildInputs, buildInputsPtr, false);

                    // Get prebuild information to determine buffer sizes
                    D3D12_RAYTRACING_ACCELERATION_STRUCTURE_PREBUILD_INFO prebuildInfo;
                    CallGetRaytracingAccelerationStructurePrebuildInfo(_device5, buildInputsPtr, out prebuildInfo);

                    // Validate prebuild info
                    if (prebuildInfo.ResultDataMaxSizeInBytes == 0 || prebuildInfo.ScratchDataSizeInBytes == 0)
                    {
                        throw new InvalidOperationException("GetRaytracingAccelerationStructurePrebuildInfo returned invalid sizes");
                    }

                    // Round up result buffer size to 256-byte alignment (D3D12 requirement)
                    ulong resultBufferSize = (prebuildInfo.ResultDataMaxSizeInBytes + 255UL) & ~255UL;

                    // Create result buffer for the acceleration structure
                    // Acceleration structure buffers must be in DEFAULT heap with D3D12_RESOURCE_FLAG_ALLOW_UNORDERED_ACCESS
                    var resultBufferDesc = new BufferDesc
                    {
                        ByteSize = (int)resultBufferSize,
                        Usage = BufferUsageFlags.AccelStructStorage,
                        InitialState = ResourceState.AccelStructRead,
                        DebugName = desc.DebugName ?? "BLAS_ResultBuffer"
                    };

                    IBuffer resultBuffer = CreateBuffer(resultBufferDesc);
                    if (resultBuffer == null || resultBuffer.NativeHandle == IntPtr.Zero)
                    {
                        throw new InvalidOperationException("Failed to create result buffer for acceleration structure");
                    }

                    // Get GPU virtual address for result buffer
                    ulong resultBufferGpuVa = GetGpuVirtualAddress(resultBuffer.NativeHandle);
                    if (resultBufferGpuVa == 0UL)
                    {
                        resultBuffer.Dispose();
                        throw new InvalidOperationException("Failed to get GPU virtual address for result buffer");
                    }

                    // Create acceleration structure wrapper
                    // Note: The actual building happens later via BuildBottomLevelAccelStruct on a command list
                    var accelStruct = new D3D12AccelStruct(handle, desc, IntPtr.Zero, resultBuffer, resultBufferGpuVa, _device5);
                    _resources[handle] = accelStruct;

                    return accelStruct;
                }
                finally
                {
                    Marshal.FreeHGlobal(buildInputsPtr);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(geometryDescsPtr);
            }
        }

        /// <summary>
        /// Creates a top-level acceleration structure (TLAS).
        /// Based on D3D12 DXR API: ID3D12Device5::GetRaytracingAccelerationStructurePrebuildInfo
        /// </summary>
        private IAccelStruct CreateTopLevelAccelStruct(IntPtr handle, AccelStructDesc desc)
        {
            if (desc.TopLevelMaxInstances <= 0)
            {
                throw new ArgumentException("Top-level acceleration structure requires TopLevelMaxInstances > 0", nameof(desc));
            }

            uint maxInstances = (uint)desc.TopLevelMaxInstances;
            uint buildFlags = ConvertAccelStructBuildFlagsToD3D12(desc.BuildFlags);

            // Build D3D12_BUILD_RAYTRACING_ACCELERATION_STRUCTURE_INPUTS for TLAS
            // For TLAS, we specify the number of instances and layout, but instance data is provided later during build
            var buildInputs = new D3D12_BUILD_RAYTRACING_ACCELERATION_STRUCTURE_INPUTS
            {
                Type = D3D12_RAYTRACING_ACCELERATION_STRUCTURE_TYPE_TOP_LEVEL,
                Flags = buildFlags,
                NumDescs = maxInstances,
                DescsLayout = D3D12_ELEMENTS_LAYOUT_ARRAY,
                pGeometryDescs = IntPtr.Zero // For TLAS, instance descriptors are provided during build, not here
            };

            // Marshal build inputs to unmanaged memory for prebuild info query
            int buildInputsSize = Marshal.SizeOf(typeof(D3D12_BUILD_RAYTRACING_ACCELERATION_STRUCTURE_INPUTS));
            IntPtr buildInputsPtr = Marshal.AllocHGlobal(buildInputsSize);
            try
            {
                Marshal.StructureToPtr(buildInputs, buildInputsPtr, false);

                // Get prebuild information to determine buffer sizes
                D3D12_RAYTRACING_ACCELERATION_STRUCTURE_PREBUILD_INFO prebuildInfo;
                CallGetRaytracingAccelerationStructurePrebuildInfo(_device5, buildInputsPtr, out prebuildInfo);

                // Validate prebuild info
                if (prebuildInfo.ResultDataMaxSizeInBytes == 0 || prebuildInfo.ScratchDataSizeInBytes == 0)
                {
                    throw new InvalidOperationException("GetRaytracingAccelerationStructurePrebuildInfo returned invalid sizes");
                }

                // Round up result buffer size to 256-byte alignment (D3D12 requirement)
                ulong resultBufferSize = (prebuildInfo.ResultDataMaxSizeInBytes + 255UL) & ~255UL;

                // Create result buffer for the acceleration structure
                var resultBufferDesc = new BufferDesc
                {
                    ByteSize = (int)resultBufferSize,
                    Usage = BufferUsageFlags.AccelStructStorage,
                    InitialState = ResourceState.AccelStructRead,
                    DebugName = desc.DebugName ?? "TLAS_ResultBuffer"
                };

                IBuffer resultBuffer = CreateBuffer(resultBufferDesc);
                if (resultBuffer == null || resultBuffer.NativeHandle == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Failed to create result buffer for acceleration structure");
                }

                // Get GPU virtual address for result buffer
                ulong resultBufferGpuVa = GetGpuVirtualAddress(resultBuffer.NativeHandle);
                if (resultBufferGpuVa == 0UL)
                {
                    resultBuffer.Dispose();
                    throw new InvalidOperationException("Failed to get GPU virtual address for result buffer");
                }

                // Create acceleration structure wrapper
                // Note: The actual building happens later via BuildTopLevelAccelStruct on a command list
                var accelStruct = new D3D12AccelStruct(handle, desc, IntPtr.Zero, resultBuffer, resultBufferGpuVa, _device5);
                _resources[handle] = accelStruct;

                return accelStruct;
            }
            finally
            {
                Marshal.FreeHGlobal(buildInputsPtr);
            }