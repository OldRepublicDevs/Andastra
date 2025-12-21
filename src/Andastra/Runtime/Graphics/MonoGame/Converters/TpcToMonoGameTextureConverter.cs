using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andastra.Parsing.Formats.TPC;
using JetBrains.Annotations;

namespace Andastra.Runtime.MonoGame.Converters
{
    /// <summary>
    /// Converts Andastra.Parsing TPC texture data to MonoGame Texture2D.
    /// Handles DXT1/DXT3/DXT5 compressed formats, RGB/RGBA uncompressed,
    /// and grayscale textures.
    /// </summary>
    /// <remarks>
    /// TPC to MonoGame Texture Converter:
    /// - Based on swkotor2.exe texture loading system (modern MonoGame adaptation)
    /// - Located via string references: "Texture" @ 0x007c71b4, "texture" @ 0x007bab24
    /// - "texturewidth" @ 0x007b6e98, "texturenames" @ 0x007bacb0
    /// - "texture0" @ 0x007bb018, "texture1" @ 0x007bb00c (texture unit references)
    /// - "depth_texture" @ 0x007bab5c, "m_sDepthTextureName" @ 0x007baaa8 (depth texture)
    /// - "envmaptexture" @ 0x007bb284, "bumpmaptexture" @ 0x007bb2a8, "bumpyshinytexture" @ 0x007bb294
    /// - "dirt_texture" @ 0x007bae9c, "rotatetexture" @ 0x007baf14
    /// - Texture properties: "TextureVar" @ 0x007c0974, "TextureVariation" @ 0x007c84b4
    /// - "ALTTEXTURE" @ 0x007cdc04 (alternate texture reference)
    /// - Texture directories: "TEXTUREPACKS" @ 0x007c6a08, "TEXTUREPACKS:" @ 0x007c7190
    /// - ".\texturepacks" @ 0x007c6a18, "d:\texturepacks" @ 0x007c6a28
    /// - "LIVE%d:OVERRIDE\textures" @ 0x007c72ac (override texture path format)
    /// - "Texture Quality" @ 0x007c7528 (texture quality setting)
    /// - OpenGL texture functions: glBindTexture, glGenTextures, glDeleteTextures, glIsTexture
    /// - OpenGL texture extensions: GL_EXT_texture_compression_s3tc, GL_ARB_texture_compression
    /// - GL_EXT_texture_cube_map, GL_EXT_texture_filter_anisotropic, GL_ARB_multitexture
    /// - Original implementation: KOTOR loads TPC files and creates DirectX textures (D3DTexture8/9)
    /// - TPC format: BioWare texture format supporting DXT1/DXT3/DXT5 compression, RGB/RGBA, grayscale
    /// - Original engine: Uses DirectX texture creation APIs (D3DXCreateTextureFromFileInMemory, etc.)
    /// - This MonoGame implementation: Converts TPC format to MonoGame Texture2D
    /// - Compression: Handles DXT compression formats, converts to RGBA for MonoGame compatibility
    /// - Mipmaps: Preserves mipmap chain from TPC or generates mipmaps if missing
    /// - Cube maps: TPC cube maps converted to MonoGame TextureCube (if supported)
    /// - Note: Original engine used DirectX APIs, this is a modern MonoGame adaptation
    /// </remarks>
    public static class TpcToMonoGameTextureConverter
    {
        /// <summary>
        /// Converts a TPC texture to a MonoGame Texture (Texture2D for 2D textures, TextureCube for cube maps).
        /// </summary>
        /// <param name="tpc">The TPC texture to convert.</param>
        /// <param name="device">The graphics device.</param>
        /// <param name="generateMipmaps">Whether to generate mipmaps if not present.</param>
        /// <returns>A MonoGame Texture ready for rendering (Texture2D for 2D textures, TextureCube for cube maps).</returns>
        // Convert TPC texture format to MonoGame Texture (Texture2D or TextureCube)
        // Based on MonoGame API: https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html
        // Texture2D represents 2D image data for rendering
        // TextureCube represents cube map textures (6 faces) for environment mapping
        // Method signature: static Texture Convert(TPC tpc, GraphicsDevice device, bool generateMipmaps)
        // Based on MonoGame API: https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.GraphicsDevice.html
        // GraphicsDevice parameter provides access to graphics hardware for texture creation
        // Source: https://docs.monogame.net/articles/getting_to_know/howto/graphics/HowTo_Load_Texture.html
        public static Texture Convert([NotNull] TPC tpc, [NotNull] GraphicsDevice device, bool generateMipmaps = true)
        {
            if (tpc == null)
            {
                throw new ArgumentNullException("tpc");
            }
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }

            if (tpc.Layers.Count == 0 || tpc.Layers[0].Mipmaps.Count == 0)
            {
                throw new ArgumentException("TPC has no texture data", "tpc");
            }

            // Get dimensions from first layer, first mipmap
            TPCMipmap baseMipmap = tpc.Layers[0].Mipmaps[0];
            int width = baseMipmap.Width;
            int height = baseMipmap.Height;
            TPCTextureFormat format = tpc.Format();

            // Handle cube maps - Convert to MonoGame TextureCube
            // Based on swkotor2.exe cube map texture loading (swkotor2.exe: texture cube map handling)
            // TPC cube maps have 6 layers, one for each face in DirectX/OpenGL order:
            // 0: PositiveX (right), 1: NegativeX (left), 2: PositiveY (top), 
            // 3: NegativeY (bottom), 4: PositiveZ (front), 5: NegativeZ (back)
            // Reference: vendor/xoreos/src/graphics/images/tpc.cpp:420-482 (cube map fixup)
            // Reference: vendor/reone/include/reone/graphics/types.h:88-95 (CubeMapFace enum)
            if (tpc.IsCubeMap && tpc.Layers.Count == 6)
            {
                return ConvertCubeMap(tpc, device, generateMipmaps);
            }

            // Convert standard 2D texture
            return Convert2DTexture(tpc.Layers[0], device, generateMipmaps);
        }

        /// <summary>
        /// Converts a TPC texture to RGBA byte array for manual processing.
        /// </summary>
        public static byte[] ConvertToRgba([NotNull] TPC tpc)
        {
            if (tpc == null)
            {
                throw new ArgumentNullException("tpc");
            }

            if (tpc.Layers.Count == 0 || tpc.Layers[0].Mipmaps.Count == 0)
            {
                return new byte[0];
            }

            TPCMipmap mipmap = tpc.Layers[0].Mipmaps[0];
            return ConvertMipmapToRgba(mipmap);
        }

        /// <summary>
        /// Converts a TPC cube map to a MonoGame TextureCube.
        /// </summary>
        /// <param name="tpc">The TPC cube map texture (must have 6 layers).</param>
        /// <param name="device">The graphics device.</param>
        /// <param name="generateMipmaps">Whether to generate mipmaps if not present.</param>
        /// <returns>A MonoGame TextureCube ready for rendering.</returns>
        /// <remarks>
        /// Cube Map Conversion:
        /// - Based on swkotor2.exe cube map texture loading system
        /// - TPC cube maps store 6 faces as separate layers
        /// - DirectX/XNA cube map face order: PositiveX, NegativeX, PositiveY, NegativeY, PositiveZ, NegativeZ
        /// - MonoGame TextureCube uses CubeMapFace enum for face indexing
        /// - Supports all TPC formats: DXT1/DXT3/DXT5, RGB/RGBA, grayscale
        /// - Handles mipmaps for each cube face
        /// - Based on MonoGame API: TextureCube(GraphicsDevice, int, bool, SurfaceFormat)
        /// - TextureCube.SetData&lt;T&gt;(CubeMapFace, T[]) sets face pixel data
        /// </remarks>
        private static TextureCube ConvertCubeMap([NotNull] TPC tpc, [NotNull] GraphicsDevice device, bool generateMipmaps)
        {
            if (tpc.Layers.Count != 6)
            {
                throw new ArgumentException("Cube map must have exactly 6 layers", "tpc");
            }

            // Get dimensions from first layer, first mipmap
            TPCMipmap baseMipmap = tpc.Layers[0].Mipmaps[0];
            int size = baseMipmap.Width; // Cube maps are square (width == height)
            if (baseMipmap.Height != size)
            {
                throw new ArgumentException("Cube map faces must be square", "tpc");
            }

            // Determine mipmap count
            int mipmapCount = tpc.Layers[0].Mipmaps.Count;
            if (generateMipmaps && mipmapCount == 1)
            {
                // Calculate mipmap count for generation
                int tempSize = size;
                while (tempSize > 1)
                {
                    mipmapCount++;
                    tempSize >>= 1;
                }
            }

            // Create TextureCube
            // Based on MonoGame API: TextureCube(GraphicsDevice graphicsDevice, int size, bool mipmap, SurfaceFormat format)
            TextureCube cubeMap = new TextureCube(device, size, generateMipmaps || mipmapCount > 1, SurfaceFormat.Color);

            // DirectX/XNA cube map face order mapping
            // TPC layers are stored in the same order as DirectX cube map faces
            CubeMapFace[] faceOrder = new CubeMapFace[]
            {
                CubeMapFace.PositiveX, // Layer 0: Right face (+X)
                CubeMapFace.NegativeX, // Layer 1: Left face (-X)
                CubeMapFace.PositiveY, // Layer 2: Top face (+Y)
                CubeMapFace.NegativeY, // Layer 3: Bottom face (-Y)
                CubeMapFace.PositiveZ, // Layer 4: Front face (+Z)
                CubeMapFace.NegativeZ  // Layer 5: Back face (-Z)
            };

            // Convert each face
            // Store previous mipmap data for each face to enable proper downsampling
            byte[][] previousMipmapDataPerFace = new byte[6][];
            for (int faceIndex = 0; faceIndex < 6; faceIndex++)
            {
                TPCLayer layer = tpc.Layers[faceIndex];
                CubeMapFace face = faceOrder[faceIndex];

                // Process each mipmap level for this face
                int currentSize = size;
                for (int mipLevel = 0; mipLevel < mipmapCount; mipLevel++)
                {
                    // Get or generate mipmap data
                    byte[] rgbaData;
                    if (mipLevel < layer.Mipmaps.Count)
                    {
                        // Use existing mipmap
                        TPCMipmap mipmap = layer.Mipmaps[mipLevel];
                        if (mipmap.Width != currentSize || mipmap.Height != currentSize)
                        {
                            throw new ArgumentException($"Cube map face {faceIndex} mipmap {mipLevel} has incorrect dimensions", "tpc");
                        }
                        rgbaData = ConvertMipmapToRgba(mipmap);
                        // Store for potential use in next mipmap generation
                        previousMipmapDataPerFace[faceIndex] = rgbaData;
                    }
                    else if (generateMipmaps)
                    {
                        // Generate mipmap by downsampling previous mipmap level
                        // Based on swkotor2.exe: Mipmaps are generated by downsampling previous level
                        // Located via string references: Texture mipmap generation
                        // Original implementation: Each mipmap level is half the size of previous level
                        // Use previous mipmap level for proper downsampling (not base level)
                        // This ensures each mipmap is properly filtered from its immediate parent
                        byte[] previousMipmapData = previousMipmapDataPerFace[faceIndex];
                        if (previousMipmapData == null)
                        {
                            throw new InvalidOperationException($"Cannot generate mipmap {mipLevel} for face {faceIndex}: previous mipmap data not available");
                        }
                        // Downsample using bilinear filtering for high-quality mipmaps
                        int prevSize = mipLevel == 1 ? size : (currentSize << 1);
                        rgbaData = DownsampleBilinear(previousMipmapData, prevSize, prevSize, currentSize, currentSize);
                        // Store for next mipmap generation
                        previousMipmapDataPerFace[faceIndex] = rgbaData;
                    }
                    else
                    {
                        break; // No more mipmaps to process
                    }

                    // Convert RGBA byte array to Color array
                    Color[] colorData = new Color[currentSize * currentSize];
                    for (int i = 0; i < colorData.Length; i++)
                    {
                        int offset = i * 4;
                        if (offset + 3 < rgbaData.Length)
                        {
                            colorData[i] = new Color(rgbaData[offset], rgbaData[offset + 1], rgbaData[offset + 2], rgbaData[offset + 3]);
                        }
                    }

                    // Set face data for this mipmap level
                    // Based on MonoGame API: void SetData&lt;T&gt;(CubeMapFace face, int level, Rectangle? rect, T[] data, int startIndex, int elementCount)
                    cubeMap.SetData(face, mipLevel, null, colorData, 0, colorData.Length);

                    // Next mipmap is half the size
                    currentSize = Math.Max(1, currentSize >> 1);
                }
            }

            return cubeMap;
        }

        /// <summary>
        /// Downsamples an RGBA image using bilinear filtering for high-quality mipmap generation.
        /// </summary>
        /// <param name="source">Source RGBA data (width x height x 4 bytes).</param>
        /// <param name="sourceWidth">Source image width.</param>
        /// <param name="sourceHeight">Source image height.</param>
        /// <param name="targetWidth">Target image width (must be half of source width for mipmaps).</param>
        /// <param name="targetHeight">Target image height (must be half of source height for mipmaps).</param>
        /// <returns>Downsampled RGBA data.</returns>
        /// <remarks>
        /// Bilinear Downsampling for Mipmap Generation:
        /// - Based on swkotor2.exe mipmap generation system
        /// - Located via string references: Texture mipmap filtering
        /// - Original implementation: Each mipmap level is generated by downsampling previous level using bilinear filtering
        /// - Bilinear filtering averages 2x2 pixel regions from source to produce each target pixel
        /// - This provides smooth, high-quality mipmaps compared to nearest-neighbor sampling
        /// - For mipmap generation, target dimensions should be exactly half of source dimensions
        /// - Each output pixel is the average of a 2x2 region in the source image
        /// </remarks>
        private static byte[] DownsampleBilinear(byte[] source, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight)
        {
            if (sourceWidth == targetWidth && sourceHeight == targetHeight)
            {
                return source;
            }

            byte[] target = new byte[targetWidth * targetHeight * 4];

            // For mipmap generation, we downsample by averaging 2x2 regions
            // This is the standard approach for generating mipmaps
            if (targetWidth == sourceWidth >> 1 && targetHeight == sourceHeight >> 1)
            {
                // Standard mipmap downsampling: each target pixel is average of 2x2 source region
                for (int y = 0; y < targetHeight; y++)
                {
                    for (int x = 0; x < targetWidth; x++)
                    {
                        // Source coordinates for 2x2 region
                        int srcX0 = x << 1;
                        int srcY0 = y << 1;
                        int srcX1 = Math.Min(srcX0 + 1, sourceWidth - 1);
                        int srcY1 = Math.Min(srcY0 + 1, sourceHeight - 1);

                        // Sample 4 pixels from source (2x2 region)
                        int idx00 = (srcY0 * sourceWidth + srcX0) * 4;
                        int idx01 = (srcY0 * sourceWidth + srcX1) * 4;
                        int idx10 = (srcY1 * sourceWidth + srcX0) * 4;
                        int idx11 = (srcY1 * sourceWidth + srcX1) * 4;

                        // Average the 4 pixels (bilinear filtering)
                        int dstIdx = (y * targetWidth + x) * 4;
                        if (idx00 + 3 < source.Length && idx01 + 3 < source.Length &&
                            idx10 + 3 < source.Length && idx11 + 3 < source.Length &&
                            dstIdx + 3 < target.Length)
                        {
                            // Average R, G, B, A channels
                            target[dstIdx] = (byte)((source[idx00] + source[idx01] + source[idx10] + source[idx11]) >> 2);
                            target[dstIdx + 1] = (byte)((source[idx00 + 1] + source[idx01 + 1] + source[idx10 + 1] + source[idx11 + 1]) >> 2);
                            target[dstIdx + 2] = (byte)((source[idx00 + 2] + source[idx01 + 2] + source[idx10 + 2] + source[idx11 + 2]) >> 2);
                            target[dstIdx + 3] = (byte)((source[idx00 + 3] + source[idx01 + 3] + source[idx10 + 3] + source[idx11 + 3]) >> 2);
                        }
                    }
                }
            }
            else
            {
                // General bilinear resampling for non-mipmap cases
                float scaleX = (float)sourceWidth / targetWidth;
                float scaleY = (float)sourceHeight / targetHeight;

                for (int y = 0; y < targetHeight; y++)
                {
                    for (int x = 0; x < targetWidth; x++)
                    {
                        // Calculate source coordinates with fractional parts
                        float srcX = (x + 0.5f) * scaleX - 0.5f;
                        float srcY = (y + 0.5f) * scaleY - 0.5f;

                        int srcX0 = (int)Math.Floor(srcX);
                        int srcY0 = (int)Math.Floor(srcY);
                        int srcX1 = Math.Min(srcX0 + 1, sourceWidth - 1);
                        int srcY1 = Math.Min(srcY0 + 1, sourceHeight - 1);

                        float fx = srcX - srcX0;
                        float fy = srcY - srcY0;

                        // Sample 4 pixels for bilinear interpolation
                        int idx00 = (srcY0 * sourceWidth + srcX0) * 4;
                        int idx01 = (srcY0 * sourceWidth + srcX1) * 4;
                        int idx10 = (srcY1 * sourceWidth + srcX0) * 4;
                        int idx11 = (srcY1 * sourceWidth + srcX1) * 4;

                        int dstIdx = (y * targetWidth + x) * 4;
                        if (idx00 + 3 < source.Length && idx01 + 3 < source.Length &&
                            idx10 + 3 < source.Length && idx11 + 3 < source.Length &&
                            dstIdx + 3 < target.Length)
                        {
                            // Bilinear interpolation
                            for (int c = 0; c < 4; c++)
                            {
                                float v00 = source[idx00 + c];
                                float v01 = source[idx01 + c];
                                float v10 = source[idx10 + c];
                                float v11 = source[idx11 + c];

                                float v0 = v00 * (1.0f - fx) + v01 * fx;
                                float v1 = v10 * (1.0f - fx) + v11 * fx;
                                float v = v0 * (1.0f - fy) + v1 * fy;

                                target[dstIdx + c] = (byte)Math.Max(0, Math.Min(255, (int)(v + 0.5f)));
                            }
                        }
                    }
                }
            }

            return target;
        }

        private static Texture2D Convert2DTexture(TPCLayer layer, GraphicsDevice device, bool generateMipmaps)
        {
            TPCMipmap baseMipmap = layer.Mipmaps[0];
            int width = baseMipmap.Width;
            int height = baseMipmap.Height;

            // Determine mipmap count
            int mipmapCount = layer.Mipmaps.Count;
            if (generateMipmaps && mipmapCount == 1)
            {
                // Calculate mipmap count for generation
                int tempWidth = width;
                int tempHeight = height;
                while (tempWidth > 1 || tempHeight > 1)
                {
                    mipmapCount++;
                    tempWidth = Math.Max(1, tempWidth >> 1);
                    tempHeight = Math.Max(1, tempHeight >> 1);
                }
            }

            // Create Texture2D with mipmap support
            // Based on MonoGame API: https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.Texture2D.html
            // Texture2D(GraphicsDevice, int, int, bool, SurfaceFormat) constructor
            // Method signature: Texture2D(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format)
            // Source: https://docs.monogame.net/articles/getting_to_know/howto/graphics/HowTo_Load_Texture.html
            Texture2D texture = new Texture2D(device, width, height, generateMipmaps || mipmapCount > 1, SurfaceFormat.Color);

            // Process each mipmap level
            int currentWidth = width;
            int currentHeight = height;
            for (int mipLevel = 0; mipLevel < mipmapCount; mipLevel++)
            {
                byte[] rgbaData;
                if (mipLevel < layer.Mipmaps.Count)
                {
                    // Use existing mipmap from TPC
                    TPCMipmap mipmap = layer.Mipmaps[mipLevel];
                    if (mipmap.Width != currentWidth || mipmap.Height != currentHeight)
                    {
                        throw new ArgumentException($"TPC mipmap {mipLevel} has incorrect dimensions: expected {currentWidth}x{currentHeight}, got {mipmap.Width}x{mipmap.Height}", "layer");
                    }
                    rgbaData = ConvertMipmapToRgba(mipmap);
                }
                else if (generateMipmaps)
                {
                    // Generate mipmap by downsampling previous mipmap level
                    // Based on swkotor2.exe: Mipmaps are generated by downsampling previous level
                    // Located via string references: Texture mipmap generation
                    // Original implementation: Each mipmap level is half the size of previous level
                    // Use previous mipmap level for proper downsampling
                    // Store previous mipmap data as we process each level
                    byte[] previousMipmapData;
                    if (mipLevel == 1)
                    {
                        // First generated mipmap: downsample from base level (mipLevel 0)
                        TPCMipmap baseMip = layer.Mipmaps[0];
                        previousMipmapData = ConvertMipmapToRgba(baseMip);
                    }
                    else
                    {
                        // Subsequent mipmaps: downsample from previous generated mipmap
                        // Get the previous mipmap data that was just generated
                        int prevWidth = currentWidth << 1;
                        int prevHeight = currentHeight << 1;
                        previousMipmapData = new byte[prevWidth * prevHeight * 4];
                        // Retrieve previous mipmap data from texture
                        Color[] prevColorData = new Color[prevWidth * prevHeight];
                        texture.GetData(mipLevel - 1, null, prevColorData, 0, prevColorData.Length);
                        // Convert Color[] back to RGBA byte[]
                        for (int i = 0; i < prevColorData.Length; i++)
                        {
                            int offset = i * 4;
                            previousMipmapData[offset] = prevColorData[i].R;
                            previousMipmapData[offset + 1] = prevColorData[i].G;
                            previousMipmapData[offset + 2] = prevColorData[i].B;
                            previousMipmapData[offset + 3] = prevColorData[i].A;
                        }
                    }
                    // Downsample using bilinear filtering for high-quality mipmaps
                    int prevWidthForDownsample = mipLevel == 1 ? width : (currentWidth << 1);
                    int prevHeightForDownsample = mipLevel == 1 ? height : (currentHeight << 1);
                    rgbaData = DownsampleBilinear(previousMipmapData, prevWidthForDownsample, prevHeightForDownsample, currentWidth, currentHeight);
                }
                else
                {
                    break; // No more mipmaps to process
                }

                // Convert byte array to Color array for SetData
                Color[] colorData = new Color[currentWidth * currentHeight];
                for (int i = 0; i < colorData.Length; i++)
                {
                    int offset = i * 4;
                    if (offset + 3 < rgbaData.Length)
                    {
                        colorData[i] = new Color(rgbaData[offset], rgbaData[offset + 1], rgbaData[offset + 2], rgbaData[offset + 3]);
                    }
                }

                // Set mipmap level data
                // Based on MonoGame API: void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount)
                texture.SetData(mipLevel, null, colorData, 0, colorData.Length);

                // Next mipmap is half the size
                currentWidth = Math.Max(1, currentWidth >> 1);
                currentHeight = Math.Max(1, currentHeight >> 1);
            }

            return texture;
        }

        /// <summary>
        /// Converts a single TPC mipmap to RGBA format.
        /// Made internal for use by PbrMaterialFactory to upload individual mipmap levels.
        /// </summary>
        internal static byte[] ConvertMipmapToRgba(TPCMipmap mipmap)
        {
            int width = mipmap.Width;
            int height = mipmap.Height;
            byte[] data = mipmap.Data;
            TPCTextureFormat format = mipmap.TpcFormat;
            byte[] output = new byte[width * height * 4];

            switch (format)
            {
                case TPCTextureFormat.RGBA:
                    Array.Copy(data, output, Math.Min(data.Length, output.Length));
                    break;

                case TPCTextureFormat.BGRA:
                    ConvertBgraToRgba(data, output, width, height);
                    break;

                case TPCTextureFormat.RGB:
                    ConvertRgbToRgba(data, output, width, height);
                    break;

                case TPCTextureFormat.BGR:
                    ConvertBgrToRgba(data, output, width, height);
                    break;

                case TPCTextureFormat.Greyscale:
                    ConvertGreyscaleToRgba(data, output, width, height);
                    break;

                case TPCTextureFormat.DXT1:
                    DecompressDxt1(data, output, width, height);
                    break;

                case TPCTextureFormat.DXT3:
                    DecompressDxt3(data, output, width, height);
                    break;

                case TPCTextureFormat.DXT5:
                    DecompressDxt5(data, output, width, height);
                    break;

                default:
                    // Fill with magenta to indicate error
                    for (int i = 0; i < output.Length; i += 4)
                    {
                        output[i] = 255;     // R
                        output[i + 1] = 0;   // G
                        output[i + 2] = 255; // B
                        output[i + 3] = 255; // A
                    }
                    break;
            }

            return output;
        }

        private static void ConvertBgraToRgba(byte[] input, byte[] output, int width, int height)
        {
            int pixelCount = width * height;
            for (int i = 0; i < pixelCount; i++)
            {
                int srcIdx = i * 4;
                int dstIdx = i * 4;
                if (srcIdx + 3 < input.Length)
                {
                    output[dstIdx] = input[srcIdx + 2];     // R <- B
                    output[dstIdx + 1] = input[srcIdx + 1]; // G <- G
                    output[dstIdx + 2] = input[srcIdx];     // B <- R
                    output[dstIdx + 3] = input[srcIdx + 3]; // A <- A
                }
            }
        }

        private static void ConvertRgbToRgba(byte[] input, byte[] output, int width, int height)
        {
            int pixelCount = width * height;
            for (int i = 0; i < pixelCount; i++)
            {
                int srcIdx = i * 3;
                int dstIdx = i * 4;
                if (srcIdx + 2 < input.Length)
                {
                    output[dstIdx] = input[srcIdx];         // R
                    output[dstIdx + 1] = input[srcIdx + 1]; // G
                    output[dstIdx + 2] = input[srcIdx + 2]; // B
                    output[dstIdx + 3] = 255;               // A
                }
            }
        }

        private static void ConvertBgrToRgba(byte[] input, byte[] output, int width, int height)
        {
            int pixelCount = width * height;
            for (int i = 0; i < pixelCount; i++)
            {
                int srcIdx = i * 3;
                int dstIdx = i * 4;
                if (srcIdx + 2 < input.Length)
                {
                    output[dstIdx] = input[srcIdx + 2];     // R <- B
                    output[dstIdx + 1] = input[srcIdx + 1]; // G <- G
                    output[dstIdx + 2] = input[srcIdx];     // B <- R
                    output[dstIdx + 3] = 255;               // A
                }
            }
        }

        private static void ConvertGreyscaleToRgba(byte[] input, byte[] output, int width, int height)
        {
            int pixelCount = width * height;
            for (int i = 0; i < pixelCount; i++)
            {
                if (i < input.Length)
                {
                    byte grey = input[i];
                    int dstIdx = i * 4;
                    output[dstIdx] = grey;     // R
                    output[dstIdx + 1] = grey; // G
                    output[dstIdx + 2] = grey; // B
                    output[dstIdx + 3] = 255;  // A
                }
            }
        }

        #region DXT Decompression

        private static void DecompressDxt1(byte[] input, byte[] output, int width, int height)
        {
            int blockCountX = (width + 3) / 4;
            int blockCountY = (height + 3) / 4;

            int srcOffset = 0;
            for (int by = 0; by < blockCountY; by++)
            {
                for (int bx = 0; bx < blockCountX; bx++)
                {
                    if (srcOffset + 8 > input.Length)
                    {
                        break;
                    }

                    // Read color endpoints
                    ushort c0 = (ushort)(input[srcOffset] | (input[srcOffset + 1] << 8));
                    ushort c1 = (ushort)(input[srcOffset + 2] | (input[srcOffset + 3] << 8));
                    uint indices = (uint)(input[srcOffset + 4] | (input[srcOffset + 5] << 8) |
                                         (input[srcOffset + 6] << 16) | (input[srcOffset + 7] << 24));
                    srcOffset += 8;

                    // Decode colors
                    byte[] colors = new byte[16]; // 4 colors * 4 components
                    DecodeColor565(c0, colors, 0);
                    DecodeColor565(c1, colors, 4);

                    if (c0 > c1)
                    {
                        // 4-color mode
                        colors[8] = (byte)((2 * colors[0] + colors[4]) / 3);
                        colors[9] = (byte)((2 * colors[1] + colors[5]) / 3);
                        colors[10] = (byte)((2 * colors[2] + colors[6]) / 3);
                        colors[11] = 255;

                        colors[12] = (byte)((colors[0] + 2 * colors[4]) / 3);
                        colors[13] = (byte)((colors[1] + 2 * colors[5]) / 3);
                        colors[14] = (byte)((colors[2] + 2 * colors[6]) / 3);
                        colors[15] = 255;
                    }
                    else
                    {
                        // 3-color + transparent mode
                        colors[8] = (byte)((colors[0] + colors[4]) / 2);
                        colors[9] = (byte)((colors[1] + colors[5]) / 2);
                        colors[10] = (byte)((colors[2] + colors[6]) / 2);
                        colors[11] = 255;

                        colors[12] = 0;
                        colors[13] = 0;
                        colors[14] = 0;
                        colors[15] = 0; // Transparent
                    }

                    // Write pixels
                    for (int py = 0; py < 4; py++)
                    {
                        for (int px = 0; px < 4; px++)
                        {
                            int x = bx * 4 + px;
                            int y = by * 4 + py;
                            if (x >= width || y >= height)
                            {
                                continue;
                            }

                            int idx = (int)((indices >> ((py * 4 + px) * 2)) & 3);
                            int dstOffset = (y * width + x) * 4;

                            output[dstOffset] = colors[idx * 4];
                            output[dstOffset + 1] = colors[idx * 4 + 1];
                            output[dstOffset + 2] = colors[idx * 4 + 2];
                            output[dstOffset + 3] = colors[idx * 4 + 3];
                        }
                    }
                }
            }
        }

        private static void DecompressDxt3(byte[] input, byte[] output, int width, int height)
        {
            int blockCountX = (width + 3) / 4;
            int blockCountY = (height + 3) / 4;

            int srcOffset = 0;
            for (int by = 0; by < blockCountY; by++)
            {
                for (int bx = 0; bx < blockCountX; bx++)
                {
                    if (srcOffset + 16 > input.Length)
                    {
                        break;
                    }

                    // Read explicit alpha (8 bytes)
                    byte[] alphas = new byte[16];
                    for (int i = 0; i < 4; i++)
                    {
                        ushort row = (ushort)(input[srcOffset + i * 2] | (input[srcOffset + i * 2 + 1] << 8));
                        for (int j = 0; j < 4; j++)
                        {
                            int a = (row >> (j * 4)) & 0xF;
                            alphas[i * 4 + j] = (byte)(a | (a << 4));
                        }
                    }
                    srcOffset += 8;

                    // Read color block (same as DXT1)
                    ushort c0 = (ushort)(input[srcOffset] | (input[srcOffset + 1] << 8));
                    ushort c1 = (ushort)(input[srcOffset + 2] | (input[srcOffset + 3] << 8));
                    uint indices = (uint)(input[srcOffset + 4] | (input[srcOffset + 5] << 8) |
                                         (input[srcOffset + 6] << 16) | (input[srcOffset + 7] << 24));
                    srcOffset += 8;

                    byte[] colors = new byte[16];
                    DecodeColor565(c0, colors, 0);
                    DecodeColor565(c1, colors, 4);

                    // Always 4-color mode for DXT3/5
                    colors[8] = (byte)((2 * colors[0] + colors[4]) / 3);
                    colors[9] = (byte)((2 * colors[1] + colors[5]) / 3);
                    colors[10] = (byte)((2 * colors[2] + colors[6]) / 3);
                    colors[11] = 255;

                    colors[12] = (byte)((colors[0] + 2 * colors[4]) / 3);
                    colors[13] = (byte)((colors[1] + 2 * colors[5]) / 3);
                    colors[14] = (byte)((colors[2] + 2 * colors[6]) / 3);
                    colors[15] = 255;

                    // Write pixels
                    for (int py = 0; py < 4; py++)
                    {
                        for (int px = 0; px < 4; px++)
                        {
                            int x = bx * 4 + px;
                            int y = by * 4 + py;
                            if (x >= width || y >= height)
                            {
                                continue;
                            }

                            int idx = (int)((indices >> ((py * 4 + px) * 2)) & 3);
                            int dstOffset = (y * width + x) * 4;

                            output[dstOffset] = colors[idx * 4];
                            output[dstOffset + 1] = colors[idx * 4 + 1];
                            output[dstOffset + 2] = colors[idx * 4 + 2];
                            output[dstOffset + 3] = alphas[py * 4 + px];
                        }
                    }
                }
            }
        }

        private static void DecompressDxt5(byte[] input, byte[] output, int width, int height)
        {
            int blockCountX = (width + 3) / 4;
            int blockCountY = (height + 3) / 4;

            int srcOffset = 0;
            for (int by = 0; by < blockCountY; by++)
            {
                for (int bx = 0; bx < blockCountX; bx++)
                {
                    if (srcOffset + 16 > input.Length)
                    {
                        break;
                    }

                    // Read interpolated alpha (8 bytes)
                    byte a0 = input[srcOffset];
                    byte a1 = input[srcOffset + 1];
                    ulong alphaIndices = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        alphaIndices |= (ulong)input[srcOffset + 2 + i] << (i * 8);
                    }
                    srcOffset += 8;

                    // Calculate alpha lookup table
                    byte[] alphaTable = new byte[8];
                    alphaTable[0] = a0;
                    alphaTable[1] = a1;
                    if (a0 > a1)
                    {
                        alphaTable[2] = (byte)((6 * a0 + 1 * a1) / 7);
                        alphaTable[3] = (byte)((5 * a0 + 2 * a1) / 7);
                        alphaTable[4] = (byte)((4 * a0 + 3 * a1) / 7);
                        alphaTable[5] = (byte)((3 * a0 + 4 * a1) / 7);
                        alphaTable[6] = (byte)((2 * a0 + 5 * a1) / 7);
                        alphaTable[7] = (byte)((1 * a0 + 6 * a1) / 7);
                    }
                    else
                    {
                        alphaTable[2] = (byte)((4 * a0 + 1 * a1) / 5);
                        alphaTable[3] = (byte)((3 * a0 + 2 * a1) / 5);
                        alphaTable[4] = (byte)((2 * a0 + 3 * a1) / 5);
                        alphaTable[5] = (byte)((1 * a0 + 4 * a1) / 5);
                        alphaTable[6] = 0;
                        alphaTable[7] = 255;
                    }

                    // Read color block
                    ushort c0 = (ushort)(input[srcOffset] | (input[srcOffset + 1] << 8));
                    ushort c1 = (ushort)(input[srcOffset + 2] | (input[srcOffset + 3] << 8));
                    uint indices = (uint)(input[srcOffset + 4] | (input[srcOffset + 5] << 8) |
                                         (input[srcOffset + 6] << 16) | (input[srcOffset + 7] << 24));
                    srcOffset += 8;

                    byte[] colors = new byte[16];
                    DecodeColor565(c0, colors, 0);
                    DecodeColor565(c1, colors, 4);

                    colors[8] = (byte)((2 * colors[0] + colors[4]) / 3);
                    colors[9] = (byte)((2 * colors[1] + colors[5]) / 3);
                    colors[10] = (byte)((2 * colors[2] + colors[6]) / 3);
                    colors[11] = 255;

                    colors[12] = (byte)((colors[0] + 2 * colors[4]) / 3);
                    colors[13] = (byte)((colors[1] + 2 * colors[5]) / 3);
                    colors[14] = (byte)((colors[2] + 2 * colors[6]) / 3);
                    colors[15] = 255;

                    // Write pixels
                    for (int py = 0; py < 4; py++)
                    {
                        for (int px = 0; px < 4; px++)
                        {
                            int x = bx * 4 + px;
                            int y = by * 4 + py;
                            if (x >= width || y >= height)
                            {
                                continue;
                            }

                            int colorIdx = (int)((indices >> ((py * 4 + px) * 2)) & 3);
                            int alphaIdx = (int)((alphaIndices >> ((py * 4 + px) * 3)) & 7);
                            int dstOffset = (y * width + x) * 4;

                            output[dstOffset] = colors[colorIdx * 4];
                            output[dstOffset + 1] = colors[colorIdx * 4 + 1];
                            output[dstOffset + 2] = colors[colorIdx * 4 + 2];
                            output[dstOffset + 3] = alphaTable[alphaIdx];
                        }
                    }
                }
            }
        }

        private static void DecodeColor565(ushort color, byte[] output, int offset)
        {
            int r = (color >> 11) & 0x1F;
            int g = (color >> 5) & 0x3F;
            int b = color & 0x1F;

            output[offset] = (byte)((r << 3) | (r >> 2));
            output[offset + 1] = (byte)((g << 2) | (g >> 4));
            output[offset + 2] = (byte)((b << 3) | (b >> 2));
            output[offset + 3] = 255;
        }

        #endregion
    }
}

