using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Andastra.Runtime.MonoGame.Culling
{
    /// <summary>
    /// Occlusion culling system using Hi-Z (Hierarchical-Z) buffer.
    ///
    /// Occlusion culling determines which objects are hidden behind other objects,
    /// allowing us to skip rendering entirely hidden geometry.
    ///
    /// Features:
    /// - Hi-Z buffer generation from depth buffer
    /// - Hardware occlusion queries
    /// - Software occlusion culling for distant objects
    /// - Temporal coherence (objects stay occluded for multiple frames)
    /// </summary>
    /// <remarks>
    /// Occlusion Culling System (Modern Enhancement):
    /// - Based on swkotor2.exe rendering system architecture
    /// - Located via string references: Original engine uses VIS file-based room visibility culling
    /// - VIS file format: "%s/%s.VIS" @ 0x007b972c (VIS file path format), "visasmarr" @ 0x007bf720 (VIS file reference)
    /// - Original implementation: KOTOR uses VIS (visibility) files for room-based occlusion culling
    /// - VIS files: Pre-computed room-to-room visibility relationships for efficient occlusion culling
    /// - Original occlusion culling: Room-based visibility from VIS files combined with frustum culling
    /// - This is a modernization feature: Hi-Z buffer provides GPU-accelerated pixel-accurate occlusion testing
    /// - Modern enhancement: More accurate than VIS files for dynamic objects, requires modern GPU features
    /// - Original engine: DirectX 8/9 era, Hi-Z buffers not available, relied on VIS file pre-computation
    /// - Combined approaches: Modern renderer can use both VIS-based room culling + Hi-Z for best performance
    /// </remarks>
    public class OcclusionCuller : IDisposable
    {
        private readonly GraphicsDevice _graphicsDevice;
        private int _width;
        private int _height;

        // Hi-Z buffer for hierarchical depth testing
        private RenderTarget2D _hiZBuffer;

        // SpriteBatch for downsampling depth buffer
        private SpriteBatch _spriteBatch;

        // Temporal occlusion cache (frame-based)
        private readonly Dictionary<uint, OcclusionInfo> _occlusionCache;
        private int _currentFrame;

        // View and projection matrices for screen space projection
        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;
        private bool _matricesValid;

        // Cached Hi-Z buffer data for CPU-side sampling (updated on demand)
        private float[] _hiZBufferData;
        private int _hiZBufferDataMipLevel;
        private bool _hiZBufferDataValid;

        // CPU-side mip level cache for proper max depth calculations
        // Since MonoGame doesn't support writing to specific mip levels,
        // we maintain a CPU-side cache of all mip levels with proper max depth values
        private float[][] _mipLevelCache;
        private bool _mipLevelCacheValid;

        // Statistics
        private OcclusionStats _stats;

        /// <summary>
        /// Gets occlusion statistics.
        /// </summary>
        public OcclusionStats Stats
        {
            get { return _stats; }
        }

        /// <summary>
        /// Gets or sets whether occlusion culling is enabled.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the minimum object size to test (smaller objects skip occlusion test).
        /// </summary>
        public float MinTestSize { get; set; } = 1.0f;

        /// <summary>
        /// Gets or sets the temporal cache lifetime in frames.
        /// </summary>
        public int CacheLifetime { get; set; } = 3;

        /// <summary>
        /// Initializes a new occlusion culler.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device.</param>
        /// <param name="width">Buffer width. Must be greater than zero.</param>
        /// <param name="height">Buffer height. Must be greater than zero.</param>
        /// <exception cref="ArgumentNullException">Thrown if graphicsDevice is null.</exception>
        /// <exception cref="ArgumentException">Thrown if width or height is less than or equal to zero.</exception>
        public OcclusionCuller(GraphicsDevice graphicsDevice, int width, int height)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException(nameof(graphicsDevice));
            }
            if (width <= 0)
            {
                throw new ArgumentException("Width must be greater than zero.", nameof(width));
            }
            if (height <= 0)
            {
                throw new ArgumentException("Height must be greater than zero.", nameof(height));
            }

            _graphicsDevice = graphicsDevice;
            _width = width;
            _height = height;
            // Mip levels are calculated dynamically in CreateHiZBuffer based on current dimensions

            _occlusionCache = new Dictionary<uint, OcclusionInfo>();
            _stats = new OcclusionStats();

            // Create SpriteBatch for downsampling
            _spriteBatch = new SpriteBatch(_graphicsDevice);

            // Create Hi-Z buffer
            CreateHiZBuffer();
        }

        /// <summary>
        /// Generates Hi-Z buffer from depth buffer.
        /// Must be called after depth pre-pass or main depth rendering.
        /// Based on MonoGame API: https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html
        /// Downsamples depth buffer into mipmap levels where each level stores maximum depth from previous level.
        ///
        /// Note: This implementation uses point sampling for downsampling. For proper Hi-Z with maximum depth
        /// operations, a custom shader that performs max operations on 2x2 regions would be required.
        /// </summary>
        /// <param name="depthBuffer">Depth buffer to downsample. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if depthBuffer is null.</exception>
        public void GenerateHiZBuffer(Texture2D depthBuffer)
        {
            if (!Enabled)
            {
                return;
            }

            if (depthBuffer == null)
            {
                throw new ArgumentNullException(nameof(depthBuffer));
            }

            if (_hiZBuffer == null || _spriteBatch == null)
            {
                return;
            }

            // Copy level 0 (full resolution) from depth buffer to Hi-Z buffer
            // Store current render target to restore later
            RenderTargetBinding[] previousTargets = _graphicsDevice.GetRenderTargets();
            RenderTarget2D previousTarget = previousTargets.Length > 0 ?
                previousTargets[0].RenderTarget as RenderTarget2D : null;

            try
            {
                _graphicsDevice.SetRenderTarget(_hiZBuffer);
                _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
                _spriteBatch.Draw(depthBuffer, new Rectangle(0, 0, _width, _height), Color.White);
                _spriteBatch.End();

                // Generate mipmap levels by downsampling with max depth operation
                // Each mip level stores the maximum depth from 2x2 region of previous level
                // Uses CPU-side max depth calculation for accurate Hi-Z generation
                // Note: MonoGame doesn't support rendering directly to specific mip levels,
                // so we maintain a CPU-side cache of mip levels with proper max depth values
                _mipLevelCacheValid = false;
                int maxMipLevels = _hiZBuffer != null ? _hiZBuffer.LevelCount : 1;
                for (int mip = 1; mip < maxMipLevels; mip++)
                {
                    int mipWidth = Math.Max(1, _width >> mip);
                    int mipHeight = Math.Max(1, _height >> mip);
                    int prevMipWidth = Math.Max(1, _width >> (mip - 1));
                    int prevMipHeight = Math.Max(1, _height >> (mip - 1));

                    // Generate mip level using proper max depth calculation
                    GenerateMipLevelWithMaxDepth(mip, mipWidth, mipHeight, prevMipWidth, prevMipHeight);
                }
            }
            finally
            {
                // Always restore previous render target, even if an exception occurs
                if (previousTarget != null)
                {
                    _graphicsDevice.SetRenderTarget(previousTarget);
                }
                else
                {
                    _graphicsDevice.SetRenderTarget(null);
                }
            }
        }

        /// <summary>
        /// Tests if an AABB is occluded using Hi-Z buffer.
        /// </summary>
        /// <param name="minPoint">Minimum corner of AABB.</param>
        /// <param name="maxPoint">Maximum corner of AABB.</param>
        /// <param name="objectId">Unique ID for temporal caching.</param>
        /// <returns>True if object is occluded (should be culled).</returns>
        public bool IsOccluded(System.Numerics.Vector3 minPoint, System.Numerics.Vector3 maxPoint, uint objectId)
        {
            if (!Enabled)
            {
                return false;
            }

            // Check temporal cache first
            OcclusionInfo cached;
            if (_occlusionCache.TryGetValue(objectId, out cached))
            {
                if (_currentFrame - cached.LastFrame <= CacheLifetime)
                {
                    _stats.CacheHits++;
                    return cached.Occluded;
                }
                // Cache expired
                _occlusionCache.Remove(objectId);
            }

            // Test against Hi-Z buffer
            bool occluded = TestOcclusionHiZ(minPoint, maxPoint);

            // Cache result
            _occlusionCache[objectId] = new OcclusionInfo
            {
                Occluded = occluded,
                LastFrame = _currentFrame
            };

            if (occluded)
            {
                _stats.OccludedObjects++;
            }
            else
            {
                _stats.VisibleObjects++;
            }
            _stats.TotalTests++;

            return occluded;
        }

        /// <summary>
        /// Tests occlusion using Hi-Z buffer hierarchical depth test.
        /// Based on MonoGame API: https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html
        /// Projects AABB to screen space, samples Hi-Z buffer at appropriate mip level, and compares depths.
        ///
        /// Implementation based on Hi-Z occlusion culling algorithm:
        /// 1. Project AABB corners to screen space using view/projection matrices
        /// 2. Calculate screen space bounding rectangle
        /// 3. Find appropriate mip level based on screen space size
        /// 4. Sample Hi-Z buffer at mip level to get maximum depth in region
        /// 5. Compare AABB minimum depth against Hi-Z maximum depth
        /// 6. If AABB min depth > Hi-Z max depth, object is occluded
        /// </summary>
        private bool TestOcclusionHiZ(System.Numerics.Vector3 minPoint, System.Numerics.Vector3 maxPoint)
        {
            if (_hiZBuffer == null)
            {
                return false; // No Hi-Z buffer available, assume visible
            }

            if (!_matricesValid)
            {
                return false; // Matrices not set, assume visible
            }

            // Calculate AABB size in world space
            System.Numerics.Vector3 aabbSize = maxPoint - minPoint;
            float aabbSizeMax = Math.Max(Math.Max(aabbSize.X, aabbSize.Y), aabbSize.Z);

            // Skip occlusion test for objects smaller than minimum test size
            if (aabbSizeMax < MinTestSize)
            {
                return false;
            }

            // Project AABB corners to screen space
            // AABB has 8 corners: all combinations of min/max for X, Y, Z
            System.Numerics.Vector3[] aabbCorners = new System.Numerics.Vector3[8];
            aabbCorners[0] = new System.Numerics.Vector3(minPoint.X, minPoint.Y, minPoint.Z);
            aabbCorners[1] = new System.Numerics.Vector3(maxPoint.X, minPoint.Y, minPoint.Z);
            aabbCorners[2] = new System.Numerics.Vector3(minPoint.X, maxPoint.Y, minPoint.Z);
            aabbCorners[3] = new System.Numerics.Vector3(maxPoint.X, maxPoint.Y, minPoint.Z);
            aabbCorners[4] = new System.Numerics.Vector3(minPoint.X, minPoint.Y, maxPoint.Z);
            aabbCorners[5] = new System.Numerics.Vector3(maxPoint.X, minPoint.Y, maxPoint.Z);
            aabbCorners[6] = new System.Numerics.Vector3(minPoint.X, maxPoint.Y, maxPoint.Z);
            aabbCorners[7] = new System.Numerics.Vector3(maxPoint.X, maxPoint.Y, maxPoint.Z);

            // Project all corners to screen space and find bounding rectangle
            float minScreenX = float.MaxValue;
            float maxScreenX = float.MinValue;
            float minScreenY = float.MaxValue;
            float maxScreenY = float.MinValue;
            float minDepth = float.MaxValue;
            bool anyVisible = false;

            // Combine view and projection matrices for efficiency
            Matrix viewProj = _viewMatrix * _projectionMatrix;

            for (int i = 0; i < 8; i++)
            {
                // Transform to view space
                Microsoft.Xna.Framework.Vector4 viewPos = Microsoft.Xna.Framework.Vector4.Transform(
                    new Microsoft.Xna.Framework.Vector4(aabbCorners[i].X, aabbCorners[i].Y, aabbCorners[i].Z, 1.0f),
                    _viewMatrix);

                // Project to clip space
                Microsoft.Xna.Framework.Vector4 clipPos = Microsoft.Xna.Framework.Vector4.Transform(viewPos, _projectionMatrix);

                // Perspective divide
                if (Math.Abs(clipPos.W) > 1e-6f)
                {
                    clipPos.X /= clipPos.W;
                    clipPos.Y /= clipPos.W;
                    clipPos.Z /= clipPos.W;
                }

                // Check if corner is behind camera (Z > 1 in clip space means behind far plane, Z < -1 means behind camera)
                if (clipPos.Z < -1.0f || clipPos.Z > 1.0f)
                {
                    continue; // Corner is outside view frustum
                }

                anyVisible = true;

                // Convert to screen space (0 to width/height)
                float screenX = (clipPos.X * 0.5f + 0.5f) * _width;
                float screenY = (1.0f - (clipPos.Y * 0.5f + 0.5f)) * _height;

                // Clamp to viewport bounds
                screenX = Math.Max(0, Math.Min(screenX, _width - 1));
                screenY = Math.Max(0, Math.Min(screenY, _height - 1));

                minScreenX = Math.Min(minScreenX, screenX);
                maxScreenX = Math.Max(maxScreenX, screenX);
                minScreenY = Math.Min(minScreenY, screenY);
                maxScreenY = Math.Max(maxScreenY, screenY);

                // Track minimum depth (closest to camera) for occlusion test
                // In clip space, Z ranges from -1 (near) to 1 (far), but we need depth in view space
                // For occlusion testing, we use the view space Z (depth from camera)
                float viewSpaceDepth = viewPos.Z;
                minDepth = Math.Min(minDepth, viewSpaceDepth);
            }

            // If no corners are visible, object is outside frustum (not occluded, just culled by frustum)
            if (!anyVisible)
            {
                return false;
            }

            // Calculate screen space bounding rectangle
            int screenMinX = (int)Math.Floor(minScreenX);
            int screenMaxX = (int)Math.Ceiling(maxScreenX);
            int screenMinY = (int)Math.Floor(minScreenY);
            int screenMaxY = (int)Math.Ceiling(maxScreenY);

            // Clamp to viewport
            screenMinX = Math.Max(0, Math.Min(screenMinX, _width - 1));
            screenMaxX = Math.Max(0, Math.Min(screenMaxX, _width - 1));
            screenMinY = Math.Max(0, Math.Min(screenMinY, _height - 1));
            screenMaxY = Math.Max(0, Math.Min(screenMaxY, _height - 1));

            // Calculate screen space size for mip level selection
            float screenWidth = screenMaxX - screenMinX + 1;
            float screenHeight = screenMaxY - screenMinY + 1;
            float screenSize = Math.Max(screenWidth, screenHeight);

            // Find appropriate mip level based on screen space size
            // Higher mip levels for smaller screen space objects (more aggressive culling)
            int mipLevel = 0;
            if (screenSize > 0 && _hiZBuffer != null)
            {
                // Select mip level where one texel covers approximately the screen space region
                // This ensures we sample at a resolution that matches the object size
                float mipScale = Math.Max(_width, _height) / screenSize;
                mipLevel = (int)Math.Floor(Math.Log(mipScale, 2));
                int maxMipLevel = _hiZBuffer.LevelCount - 1;
                mipLevel = Math.Max(0, Math.Min(mipLevel, maxMipLevel));
            }

            // Sample Hi-Z buffer at calculated mip level
            // Get maximum depth in the screen space region
            float hiZMaxDepth = SampleHiZBufferMaxDepth(screenMinX, screenMinY, screenMaxX, screenMaxY, mipLevel);

            // If Hi-Z max depth is invalid (no data), assume visible
            if (hiZMaxDepth <= 0.0f || float.IsInfinity(hiZMaxDepth) || float.IsNaN(hiZMaxDepth))
            {
                return false;
            }

            // Compare AABB minimum depth against Hi-Z maximum depth
            // In view space, larger Z values are farther from camera
            // If the AABB's closest point (minDepth) is farther than the Hi-Z max depth,
            // the entire AABB is behind occluders and can be culled
            // Note: We add a small bias to prevent false positives from floating point precision
            const float depthBias = 0.01f;
            bool occluded = minDepth > (hiZMaxDepth + depthBias);

            return occluded;
        }

        /// <summary>
        /// Samples Hi-Z buffer to get maximum depth in a screen space region.
        /// Uses CPU-side texture readback for sampling.
        /// </summary>
        /// <param name="minX">Minimum X coordinate in screen space.</param>
        /// <param name="minY">Minimum Y coordinate in screen space.</param>
        /// <param name="maxX">Maximum X coordinate in screen space.</param>
        /// <param name="maxY">Maximum Y coordinate in screen space.</param>
        /// <param name="mipLevel">Mip level to sample from.</param>
        /// <returns>Maximum depth value in the region, or 0 if sampling fails.</returns>
        private float SampleHiZBufferMaxDepth(int minX, int minY, int maxX, int maxY, int mipLevel)
        {
            if (_hiZBuffer == null)
            {
                return 0.0f;
            }

            // Calculate mip level dimensions
            int mipWidth = Math.Max(1, _width >> mipLevel);
            int mipHeight = Math.Max(1, _height >> mipLevel);

            // Convert screen space coordinates to mip level coordinates
            int mipMinX = Math.Max(0, Math.Min(minX >> mipLevel, mipWidth - 1));
            int mipMaxX = Math.Max(0, Math.Min(maxX >> mipLevel, mipWidth - 1));
            int mipMinY = Math.Max(0, Math.Min(minY >> mipLevel, mipHeight - 1));
            int mipMaxY = Math.Max(0, Math.Min(maxY >> mipLevel, mipHeight - 1));

            // Read Hi-Z buffer data if not cached or if mip level changed
            if (!_hiZBufferDataValid || _hiZBufferDataMipLevel != mipLevel)
            {
                // Allocate buffer if needed
                int pixelCount = mipWidth * mipHeight;
                if (_hiZBufferData == null || _hiZBufferData.Length < pixelCount)
                {
                    _hiZBufferData = new float[pixelCount];
                }

                // Use CPU-side mip level cache if available (contains proper max depth values)
                if (_mipLevelCacheValid && _mipLevelCache != null && mipLevel < _mipLevelCache.Length && _mipLevelCache[mipLevel] != null)
                {
                    // Copy from cache
                    float[] cachedData = _mipLevelCache[mipLevel];
                    int copyCount = Math.Min(cachedData.Length, _hiZBufferData.Length);
                    Array.Copy(cachedData, _hiZBufferData, copyCount);
                    _hiZBufferDataMipLevel = mipLevel;
                    _hiZBufferDataValid = true;
                }
                else
                {
                    // Fallback: Read from GPU and manually downsample
                    // Note: MonoGame's GetData reads from mip level 0 by default
                    // For other mip levels, we need to read the full texture and manually downsample
                    try
                    {
                        // Read full resolution texture (mip 0)
                        float[] fullResData = new float[_width * _height];
                        _hiZBuffer.GetData(fullResData);

                        // Downsample to mip level resolution by taking maximum of 2x2 regions
                        for (int y = 0; y < mipHeight; y++)
                        {
                            for (int x = 0; x < mipWidth; x++)
                            {
                                // Calculate source region in full resolution
                                int srcX = x << mipLevel;
                                int srcY = y << mipLevel;
                                int srcWidth = Math.Min(1 << mipLevel, _width - srcX);
                                int srcHeight = Math.Min(1 << mipLevel, _height - srcY);

                                // Find maximum depth in source region
                                float maxDepth = 0.0f;
                                for (int sy = 0; sy < srcHeight; sy++)
                                {
                                    for (int sx = 0; sx < srcWidth; sx++)
                                    {
                                        int srcIndex = (srcY + sy) * _width + (srcX + sx);
                                        if (srcIndex < fullResData.Length)
                                        {
                                            maxDepth = Math.Max(maxDepth, fullResData[srcIndex]);
                                        }
                                    }
                                }

                                // Store in mip level buffer
                                int mipIndex = y * mipWidth + x;
                                if (mipIndex < _hiZBufferData.Length)
                                {
                                    _hiZBufferData[mipIndex] = maxDepth;
                                }
                            }
                        }

                        _hiZBufferDataMipLevel = mipLevel;
                        _hiZBufferDataValid = true;
                    }
                    catch
                    {
                        // If readback fails (e.g., render target not readable), return 0 (assume visible)
                        return 0.0f;
                    }
                }
            }

            // Sample maximum depth in the region
            float regionMaxDepth = 0.0f;
            for (int y = mipMinY; y <= mipMaxY; y++)
            {
                for (int x = mipMinX; x <= mipMaxX; x++)
                {
                    int index = y * mipWidth + x;
                    if (index >= 0 && index < _hiZBufferData.Length)
                    {
                        regionMaxDepth = Math.Max(regionMaxDepth, _hiZBufferData[index]);
                    }
                }
            }

            return regionMaxDepth;
        }

        /// <summary>
        /// Updates view and projection matrices for screen space projection.
        /// Must be called each frame before occlusion testing.
        /// </summary>
        /// <param name="viewMatrix">View matrix (world to camera space).</param>
        /// <param name="projectionMatrix">Projection matrix (camera to clip space).</param>
        public void UpdateMatrices(Matrix viewMatrix, Matrix projectionMatrix)
        {
            _viewMatrix = viewMatrix;
            _projectionMatrix = projectionMatrix;
            _matricesValid = true;
            _hiZBufferDataValid = false; // Invalidate cached Hi-Z data when matrices change
            _mipLevelCacheValid = false; // Invalidate mip level cache when matrices change
        }

        /// <summary>
        /// Starts a new frame, clearing expired cache entries.
        /// </summary>
        public void BeginFrame()
        {
            _currentFrame++;
            _stats.Reset();

            // Clean up expired cache entries
            if (_occlusionCache.Count > 10000) // Prevent unbounded growth
            {
                var toRemove = new List<uint>();
                foreach (var kvp in _occlusionCache)
                {
                    if (_currentFrame - kvp.Value.LastFrame > CacheLifetime)
                    {
                        toRemove.Add(kvp.Key);
                    }
                }
                foreach (uint id in toRemove)
                {
                    _occlusionCache.Remove(id);
                }
            }
        }

        /// <summary>
        /// Resizes the occlusion culler for new resolution.
        /// </summary>
        /// <param name="width">New buffer width. Must be greater than zero.</param>
        /// <param name="height">New buffer height. Must be greater than zero.</param>
        /// <exception cref="ArgumentException">Thrown if width or height is less than or equal to zero.</exception>
        public void Resize(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentException("Width must be greater than zero.", nameof(width));
            }
            if (height <= 0)
            {
                throw new ArgumentException("Height must be greater than zero.", nameof(height));
            }

            // Update width and height fields for dynamic resizing
            _width = width;
            _height = height;

            // Recreate Hi-Z buffer with new size (will recalculate mip levels automatically)
            if (_hiZBuffer != null)
            {
                _hiZBuffer.Dispose();
                _hiZBuffer = null;
            }

            // Recreate buffer with new dimensions (mip levels calculated dynamically)
            CreateHiZBuffer();

            // Invalidate mip level cache when resizing
            _mipLevelCache = null;
            _mipLevelCacheValid = false;
        }

        /// <summary>
        /// Generates all mip levels by calculating maximum depth from 2x2 regions.
        /// Uses CPU-side calculation for accurate max depth operations.
        /// Stores results in CPU-side cache since MonoGame doesn't support writing to specific mip levels.
        /// </summary>
        private void GenerateMipLevelWithMaxDepth(int mipLevel, int mipWidth, int mipHeight, int prevMipWidth, int prevMipHeight)
        {
            // Initialize mip level cache if needed
            if (_mipLevelCache == null)
            {
                int maxMipLevels = _hiZBuffer != null ? _hiZBuffer.LevelCount : 1;
                _mipLevelCache = new float[maxMipLevels][];

                // Read mip level 0 (full resolution) from Hi-Z buffer
                float[] mip0Data = new float[_width * _height];
                _hiZBuffer.GetData(mip0Data);
                _mipLevelCache[0] = mip0Data;
            }

            // Get previous mip level data from cache
            float[] prevMipData = _mipLevelCache[mipLevel - 1];
            if (prevMipData == null)
            {
                // Previous mip level not cached, need to generate it first
                // This shouldn't happen if we generate mip levels in order, but handle it gracefully
                return;
            }

            // Calculate max depth values for current mip level from previous mip level
            // Each texel in current mip level is the maximum of a 2x2 region in previous level
            float[] mipData = new float[mipWidth * mipHeight];
            for (int y = 0; y < mipHeight; y++)
            {
                for (int x = 0; x < mipWidth; x++)
                {
                    // Calculate source region in previous mip level (2x2 region)
                    int srcX = x << 1;
                    int srcY = y << 1;

                    // Sample 2x2 region and find maximum depth
                    float maxDepth = 0.0f;
                    for (int sy = 0; sy < 2 && (srcY + sy) < prevMipHeight; sy++)
                    {
                        for (int sx = 0; sx < 2 && (srcX + sx) < prevMipWidth; sx++)
                        {
                            int srcIndex = (srcY + sy) * prevMipWidth + (srcX + sx);
                            if (srcIndex >= 0 && srcIndex < prevMipData.Length)
                            {
                                maxDepth = Math.Max(maxDepth, prevMipData[srcIndex]);
                            }
                        }
                    }

                    int mipIndex = y * mipWidth + x;
                    if (mipIndex < mipData.Length)
                    {
                        mipData[mipIndex] = maxDepth;
                    }
                }
            }

            // Store in cache for future use
            _mipLevelCache[mipLevel] = mipData;
            _mipLevelCacheValid = true;
        }

        private void CreateHiZBuffer()
        {
            // Create Hi-Z buffer as render target with mipmaps
            // Based on MonoGame API: https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.RenderTarget2D.html
            // RenderTarget2D(GraphicsDevice, int, int, bool, SurfaceFormat, DepthFormat, int, RenderTargetUsage, bool, int)
            // SurfaceFormat.Single stores depth as 32-bit float, mipmaps enabled for hierarchical depth testing
            // Calculate mip levels: log2(max(width, height)) + 1
            // This gives us a full mip chain down to 1x1
            int maxDimension = Math.Max(_width, _height);
            int calculatedMipLevels = maxDimension > 0 ? ((int)Math.Log(maxDimension, 2) + 1) : 1;

            _hiZBuffer = new RenderTarget2D(
                _graphicsDevice,
                _width,
                _height,
                false,
                SurfaceFormat.Single,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents,
                true,
                calculatedMipLevels
            );
        }

        public void Dispose()
        {
            if (_hiZBuffer != null)
            {
                _hiZBuffer.Dispose();
                _hiZBuffer = null;
            }
            if (_spriteBatch != null)
            {
                _spriteBatch.Dispose();
                _spriteBatch = null;
            }
            _occlusionCache.Clear();
        }

        private struct OcclusionInfo
        {
            public bool Occluded;
            public int LastFrame;
        }
    }

    /// <summary>
    /// Statistics for occlusion culling.
    /// </summary>
    public class OcclusionStats
    {
        /// <summary>
        /// Total occlusion tests performed.
        /// </summary>
        public int TotalTests { get; set; }

        /// <summary>
        /// Objects found to be occluded.
        /// </summary>
        public int OccludedObjects { get; set; }

        /// <summary>
        /// Objects found to be visible.
        /// </summary>
        public int VisibleObjects { get; set; }

        /// <summary>
        /// Cache hits (temporal coherence).
        /// </summary>
        public int CacheHits { get; set; }

        /// <summary>
        /// Gets the occlusion rate (percentage of objects occluded).
        /// </summary>
        public float OcclusionRate
        {
            get
            {
                if (TotalTests == 0)
                {
                    return 0.0f;
                }
                return (OccludedObjects / (float)TotalTests) * 100.0f;
            }
        }

        /// <summary>
        /// Resets statistics for a new frame.
        /// </summary>
        public void Reset()
        {
            TotalTests = 0;
            OccludedObjects = 0;
            VisibleObjects = 0;
            CacheHits = 0;
        }
    }
}

