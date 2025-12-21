using System;
using System.Numerics;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Core.Mathematics;
using Andastra.Runtime.Graphics.Common.Enums;
using Andastra.Runtime.Graphics.Common.PostProcessing;
using Andastra.Runtime.Graphics.Common.Rendering;

namespace Andastra.Runtime.Stride.PostProcessing
{
    /// <summary>
    /// Stride implementation of Color Grading post-processing effect.
    /// Inherits shared color grading logic from BaseColorGradingEffect.
    ///
    /// Features:
    /// - 3D LUT (Look-Up Table) color grading
    /// - Contrast, saturation, brightness adjustments
    /// - LUT blending (strength control)
    /// - Support for 16x16x16 and 32x32x32 LUTs
    /// - Real-time parameter adjustment
    ///
    /// Based on Stride rendering pipeline: https://doc.stride3d.net/latest/en/manual/graphics/
    /// Color grading is used to achieve cinematic color aesthetics and mood.
    /// </summary>
    public class StrideColorGradingEffect : BaseColorGradingEffect
    {
        private GraphicsDevice _graphicsDevice;
        private EffectInstance _colorGradingEffect;
        private Texture _lutTexture;
        private Texture _temporaryTexture;
        private int _lutSize; // Size of the 3D LUT (16 or 32)
        private bool _effectInitialized;

        public StrideColorGradingEffect(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            _lutSize = 0;
            _effectInitialized = false;
        }

        #region BaseColorGradingEffect Implementation

        protected override void OnDispose()
        {
            _colorGradingEffect?.Dispose();
            _colorGradingEffect = null;

            // Note: Don't dispose LUT texture here if it's managed externally
            // Only dispose if we created it ourselves

            _temporaryTexture?.Dispose();
            _temporaryTexture = null;

            base.OnDispose();
        }

        #endregion

        /// <summary>
        /// Loads a 3D LUT texture for color grading.
        /// </summary>
        /// <param name="lutTexture">3D texture (16x16x16 or 32x32x32) containing color transform.</param>
        public void LoadLut(Texture lutTexture)
        {
            if (lutTexture == null)
            {
                throw new ArgumentNullException(nameof(lutTexture));
            }

            // Validate LUT dimensions
            // Common sizes: 16x16x16 (256x16) or 32x32x32 (1024x32) flattened to 2D
            if (lutTexture.Dimension != TextureDimension.Texture2D)
            {
                throw new ArgumentException("LUT must be a 2D texture (flattened 3D)", nameof(lutTexture));
            }

            // Determine LUT size from texture dimensions
            // 16x16x16 LUT: 256x16 (16 slices of 16x16)
            // 32x32x32 LUT: 1024x32 (32 slices of 32x32)
            int width = lutTexture.Width;
            int height = lutTexture.Height;
            
            if (width == 256 && height == 16)
            {
                _lutSize = 16;
            }
            else if (width == 1024 && height == 32)
            {
                _lutSize = 32;
            }
            else
            {
                // Try to infer from dimensions
                // For a 3D LUT flattened to 2D: width = size^2, height = size
                int inferredSize = (int)Math.Sqrt(width);
                if (inferredSize * inferredSize == width && inferredSize == height)
                {
                    _lutSize = inferredSize;
                }
                else
                {
                    throw new ArgumentException($"Unsupported LUT dimensions: {width}x{height}. Expected 256x16 (16^3) or 1024x32 (32^3)", nameof(lutTexture));
                }
            }

            _lutTexture = lutTexture;
            base.LutTexture = lutTexture;
        }

        /// <summary>
        /// Applies color grading to the input frame.
        /// </summary>
        /// <param name="input">LDR color buffer (after tone mapping).</param>
        /// <param name="width">Render width.</param>
        /// <param name="height">Render height.</param>
        /// <returns>Output texture with color grading applied.</returns>
        public Texture Apply(Texture input, int width, int height)
        {
            if (!_enabled || input == null)
            {
                return input;
            }

            if (_lutTexture == null && _strength <= 0.0f && Math.Abs(_contrast) < 0.01f && Math.Abs(_saturation - 1.0f) < 0.01f)
            {
                // No color grading to apply
                return input;
            }

            EnsureTextures(width, height, input.Format);

            // Color Grading Process:
            // 1. Apply contrast adjustment
            // 2. Apply saturation adjustment
            // 3. Sample 3D LUT (if available)
            // 4. Blend LUT result with original based on strength
            // 5. Clamp to valid color range

            ExecuteColorGrading(input, _temporaryTexture);

            return _temporaryTexture ?? input;
        }

        private void EnsureTextures(int width, int height, PixelFormat format)
        {
            if (_temporaryTexture != null &&
                _temporaryTexture.Width == width &&
                _temporaryTexture.Height == height)
            {
                return;
            }

            _temporaryTexture?.Dispose();

            var desc = TextureDescription.New2D(width, height, 1, format,
                TextureFlags.ShaderResource | TextureFlags.RenderTarget);

            _temporaryTexture = Texture.New(_graphicsDevice, desc);
        }

        private void ExecuteColorGrading(Texture input, Texture output)
        {
            // Color Grading Shader Execution:
            // - Input: LDR color buffer [0, 1]
            // - Parameters: contrast, saturation, LUT texture, LUT strength
            // - Process: Adjust contrast/saturation -> Sample LUT -> Blend
            // - Output: Color-graded LDR buffer

            // Try GPU shader path first, fall back to CPU if not available
            if (TryExecuteColorGradingGpu(input, output))
            {
                return;
            }

            // CPU fallback implementation
            ExecuteColorGradingCpu(input, output);
        }

        /// <summary>
        /// Attempts to execute color grading using GPU shader.
        /// Returns true if successful, false if CPU fallback is needed.
        /// </summary>
        private bool TryExecuteColorGradingGpu(Texture input, Texture output)
        {
            // Initialize effect if needed
            if (!_effectInitialized)
            {
                InitializeEffect();
            }

            if (_colorGradingEffect == null)
            {
                return false;
            }

            try
            {
                var commandList = _graphicsDevice.ImmediateContext;
                if (commandList == null)
                {
                    return false;
                }

                // Set shader parameters
                var parameters = _colorGradingEffect.Parameters;
                if (parameters != null)
                {
                    // Set input texture
                    try
                    {
                        parameters.Set("InputTexture", input);
                    }
                    catch (ArgumentException)
                    {
                        // Parameter doesn't exist - shader not loaded
                        return false;
                    }

                    // Set LUT texture if available
                    if (_lutTexture != null)
                    {
                        try
                        {
                            parameters.Set("LutTexture", _lutTexture);
                            parameters.Set("LutSize", (float)_lutSize);
                        }
                        catch (ArgumentException)
                        {
                            // LUT parameters don't exist
                        }
                    }

                    // Set color grading parameters
                    try
                    {
                        parameters.Set("Contrast", _contrast);
                        parameters.Set("Saturation", _saturation);
                        parameters.Set("Strength", _strength);
                    }
                    catch (ArgumentException)
                    {
                        // Parameters don't exist
                    }
                }

                // Set render target
                commandList.SetRenderTarget(null, output);
                commandList.SetViewport(new Viewport(0, 0, output.Width, output.Height));

                // Draw fullscreen quad
                // Note: This requires a fullscreen quad mesh and proper shader setup
                // For now, we'll fall back to CPU if the shader isn't fully set up
                // In a complete implementation, this would use:
                // - A fullscreen quad vertex/index buffer
                // - EffectInstance.Apply() to set pipeline state
                // - commandList.DrawIndexed() to render the quad

                // Check if we can actually render (would need fullscreen quad setup)
                // For now, return false to use CPU fallback
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideColorGrading] GPU shader execution failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Initializes the color grading effect instance.
        /// Attempts to load a shader effect, but gracefully falls back if not available.
        /// </summary>
        private void InitializeEffect()
        {
            if (_effectInitialized)
            {
                return;
            }

            try
            {
                // Attempt to load color grading shader effect
                // In a full implementation, this would load from .sdfx file:
                // Effect effect = Effect.Load(_graphicsDevice, "ColorGradingEffect");
                // _colorGradingEffect = new EffectInstance(effect);

                // For now, create a null effect instance (will trigger CPU fallback)
                // This allows the code structure to be in place for when shaders are added
                _colorGradingEffect = new EffectInstance(null);
                _effectInitialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideColorGrading] Failed to initialize effect: {ex.Message}");
                _colorGradingEffect = null;
                _effectInitialized = true; // Mark as initialized to avoid retrying
            }
        }

        /// <summary>
        /// CPU-side color grading execution (fallback implementation).
        /// Implements the complete color grading algorithm matching GPU shader behavior.
        /// Based on industry-standard color grading algorithms and LUT sampling techniques.
        /// </summary>
        private void ExecuteColorGradingCpu(Texture input, Texture output)
        {
            if (input == null || output == null || _graphicsDevice == null)
            {
                return;
            }

            int width = input.Width;
            int height = input.Height;

            try
            {
                var commandList = _graphicsDevice.ImmediateContext;
                if (commandList == null)
                {
                    Console.WriteLine("[StrideColorGrading] ImmediateContext not available");
                    return;
                }

                // Read input texture data
                Vector4[] inputData = ReadTextureData(input);
                if (inputData == null || inputData.Length != width * height)
                {
                    Console.WriteLine("[StrideColorGrading] Failed to read input texture data");
                    return;
                }

                // Read LUT texture data if available
                Vector4[] lutData = null;
                if (_lutTexture != null && _lutSize > 0)
                {
                    lutData = ReadTextureData(_lutTexture);
                    if (lutData == null)
                    {
                        Console.WriteLine("[StrideColorGrading] Failed to read LUT texture data");
                        // Continue without LUT
                    }
                }

                // Allocate output buffer
                Vector4[] outputData = new Vector4[width * height];

                // Process each pixel
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int index = y * width + x;
                        Vector4 inputColor = inputData[index];

                        // Step 1: Apply contrast adjustment
                        // Formula: color = (color - 0.5) * (1.0 + contrast) + 0.5
                        // Contrast range: [-1, 1], where 0 = no change
                        float contrastFactor = 1.0f + _contrast;
                        Vector3 color = new Vector3(
                            (inputColor.X - 0.5f) * contrastFactor + 0.5f,
                            (inputColor.Y - 0.5f) * contrastFactor + 0.5f,
                            (inputColor.Z - 0.5f) * contrastFactor + 0.5f
                        );

                        // Clamp to [0, 1] range
                        color.X = Math.Max(0.0f, Math.Min(1.0f, color.X));
                        color.Y = Math.Max(0.0f, Math.Min(1.0f, color.Y));
                        color.Z = Math.Max(0.0f, Math.Min(1.0f, color.Z));

                        // Step 2: Apply saturation adjustment
                        // Formula: lerp(grayscale, color, saturation)
                        // Saturation range: [0, 2], where 1.0 = no change, 0 = grayscale, >1 = oversaturated
                        float luminance = 0.299f * color.X + 0.587f * color.Y + 0.114f * color.Z; // ITU-R BT.601
                        Vector3 grayscale = new Vector3(luminance, luminance, luminance);
                        color = Vector3.Lerp(grayscale, color, _saturation);

                        // Clamp again after saturation
                        color.X = Math.Max(0.0f, Math.Min(1.0f, color.X));
                        color.Y = Math.Max(0.0f, Math.Min(1.0f, color.Y));
                        color.Z = Math.Max(0.0f, Math.Min(1.0f, color.Z));

                        // Step 3: Sample 3D LUT (if available)
                        Vector3 finalColor = color;
                        if (lutData != null && _lutSize > 0 && _strength > 0.0f)
                        {
                            Vector3 lutColor = SampleLut3D(color, lutData, _lutSize, _lutTexture.Width, _lutTexture.Height);
                            
                            // Step 4: Blend LUT result with adjusted color based on strength
                            // Formula: lerp(adjustedColor, lutColor, strength)
                            finalColor = Vector3.Lerp(color, lutColor, _strength);
                        }

                        // Step 5: Clamp to valid color range and preserve alpha
                        finalColor.X = Math.Max(0.0f, Math.Min(1.0f, finalColor.X));
                        finalColor.Y = Math.Max(0.0f, Math.Min(1.0f, finalColor.Y));
                        finalColor.Z = Math.Max(0.0f, Math.Min(1.0f, finalColor.Z));

                        outputData[index] = new Vector4(finalColor.X, finalColor.Y, finalColor.Z, inputColor.W);
                    }
                }

                // Write output data back to texture
                WriteTextureData(output, outputData, width, height);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideColorGrading] CPU execution failed: {ex.Message}");
            }
        }

        /// <summary>
        /// Samples a 3D LUT that has been flattened to a 2D texture.
        /// Implements the algorithm from three.js LUTPass.js for 2D flattened LUT sampling.
        /// Based on industry-standard LUT sampling techniques.
        /// </summary>
        /// <param name="rgb">Input RGB color in [0, 1] range</param>
        /// <param name="lutData">LUT texture data as Vector4 array</param>
        /// <param name="lutSize">Size of the 3D LUT (16 or 32)</param>
        /// <param name="lutWidth">Width of the flattened 2D LUT texture</param>
        /// <param name="lutHeight">Height of the flattened 2D LUT texture</param>
        /// <returns>Sampled color from LUT</returns>
        private Vector3 SampleLut3D(Vector3 rgb, Vector4[] lutData, int lutSize, int lutWidth, int lutHeight)
        {
            // Clamp the sample in by half a pixel to avoid interpolation artifacts
            // between slices laid out next to each other
            float halfPixelWidth = 0.5f / lutSize;
            float r = Math.Max(halfPixelWidth, Math.Min(1.0f - halfPixelWidth, rgb.X));
            float g = Math.Max(halfPixelWidth, Math.Min(1.0f - halfPixelWidth, rgb.Y));
            float b = Math.Max(0.0f, Math.Min(1.0f, rgb.Z));

            // Green offset into a LUT layer
            float gOffset = g / lutSize;

            // Calculate blue slice and interpolation factor
            float bNormalized = lutSize * b;
            int bSlice = (int)Math.Floor(bNormalized);
            bSlice = Math.Max(0, Math.Min(lutSize - 1, bSlice)); // Clamp to valid range
            float bMix = (bNormalized - bSlice) / lutSize;

            // Get the first LUT slice and then the one to interpolate to
            float b1 = bSlice / (float)lutSize;
            float b2 = (bSlice + 1) / (float)lutSize;

            // Calculate UV coordinates for both slices
            // For flattened 3D LUT: each row is a blue slice, each column within a row is R*G
            // UV: (r, gOffset + bSlice)
            float uv1X = r;
            float uv1Y = gOffset + b1;
            float uv2X = r;
            float uv2Y = gOffset + b2;

            // Clamp UV coordinates to valid texture range
            uv1X = Math.Max(0.0f, Math.Min(1.0f, uv1X));
            uv1Y = Math.Max(0.0f, Math.Min(1.0f, uv1Y));
            uv2X = Math.Max(0.0f, Math.Min(1.0f, uv2X));
            uv2Y = Math.Max(0.0f, Math.Min(1.0f, uv2Y));

            // Sample from LUT texture
            Vector3 sample1 = SampleLutTexture(lutData, uv1X, uv1Y, lutWidth, lutHeight);
            Vector3 sample2 = SampleLutTexture(lutData, uv2X, uv2Y, lutWidth, lutHeight);

            // Interpolate between the two blue slices
            return Vector3.Lerp(sample1, sample2, bMix);
        }

        /// <summary>
        /// Samples a 2D LUT texture at the given UV coordinates using bilinear filtering.
        /// </summary>
        private Vector3 SampleLutTexture(Vector4[] lutData, float u, float v, int width, int height)
        {
            // Convert UV to pixel coordinates
            float x = u * (width - 1);
            float y = v * (height - 1);

            // Get integer coordinates for bilinear filtering
            int x0 = (int)Math.Floor(x);
            int y0 = (int)Math.Floor(y);
            int x1 = Math.Min(width - 1, x0 + 1);
            int y1 = Math.Min(height - 1, y0 + 1);

            // Get fractional parts for interpolation
            float fx = x - x0;
            float fy = y - y0;

            // Sample four corners
            Vector3 c00 = GetLutPixel(lutData, x0, y0, width);
            Vector3 c10 = GetLutPixel(lutData, x1, y0, width);
            Vector3 c01 = GetLutPixel(lutData, x0, y1, width);
            Vector3 c11 = GetLutPixel(lutData, x1, y1, width);

            // Bilinear interpolation
            Vector3 c0 = Vector3.Lerp(c00, c10, fx);
            Vector3 c1 = Vector3.Lerp(c01, c11, fx);
            return Vector3.Lerp(c0, c1, fy);
        }

        /// <summary>
        /// Gets a pixel from the LUT texture data.
        /// </summary>
        private Vector3 GetLutPixel(Vector4[] lutData, int x, int y, int width)
        {
            int index = y * width + x;
            if (index >= 0 && index < lutData.Length)
            {
                Vector4 pixel = lutData[index];
                return new Vector3(pixel.X, pixel.Y, pixel.Z);
            }
            return Vector3.Zero;
        }

        /// <summary>
        /// Reads texture data from GPU to CPU memory.
        /// </summary>
        private Vector4[] ReadTextureData(Texture texture, GraphicsContext commandList)
        {
            if (texture == null || commandList == null)
            {
                return null;
            }

            try
            {
                int width = texture.Width;
                int height = texture.Height;
                int size = width * height;
                Vector4[] data = new Vector4[size];

                PixelFormat format = texture.Format;

                // Handle different texture formats
                if (format == PixelFormat.R8G8B8A8_UNorm ||
                    format == PixelFormat.R8G8B8A8_UNorm_SRgb ||
                    format == PixelFormat.R32G32B32A32_Float ||
                    format == PixelFormat.R16G16B16A16_Float ||
                    format == PixelFormat.B8G8R8A8_UNorm ||
                    format == PixelFormat.B8G8R8A8_UNorm_SRgb)
                {
                    // Read as Color array
                    var colorData = new Color[size];
                    texture.GetData(commandList, colorData);

                    // Convert Color[] to Vector4[]
                    for (int i = 0; i < size; i++)
                    {
                        var color = colorData[i];
                        if (format == PixelFormat.R32G32B32A32_Float)
                        {
                            // Already float format
                            data[i] = new Vector4(color.R, color.G, color.B, color.A);
                        }
                        else
                        {
                            // Convert from [0, 255] to [0, 1]
                            data[i] = new Vector4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
                        }
                    }
                }
                else if (format == PixelFormat.R16G16B16A16_Float)
                {
                    // Half-precision float format
                    var colorData = new Color[size];
                    texture.GetData(commandList, colorData);
                    for (int i = 0; i < size; i++)
                    {
                        var color = colorData[i];
                        data[i] = new Vector4(color.R, color.G, color.B, color.A);
                    }
                }
                else
                {
                    // Try generic Color readback
                    try
                    {
                        var colorData = new Color[size];
                        texture.GetData(commandList, colorData);
                        for (int i = 0; i < size; i++)
                        {
                            var color = colorData[i];
                            data[i] = new Vector4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[StrideColorGrading] ReadTextureData: Unsupported format {format}: {ex.Message}");
                        return null;
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideColorGrading] ReadTextureData failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Writes texture data from CPU memory to GPU texture.
        /// </summary>
        private void WriteTextureData(Texture texture, Vector4[] data, GraphicsContext commandList, int width, int height)
        {
            if (texture == null || data == null || commandList == null)
            {
                return;
            }

            try
            {
                if (texture.Width != width || texture.Height != height)
                {
                    Console.WriteLine($"[StrideColorGrading] WriteTextureData: Texture dimensions mismatch");
                    return;
                }

                int size = width * height;
                if (data.Length < size)
                {
                    Console.WriteLine($"[StrideColorGrading] WriteTextureData: Data array too small");
                    return;
                }

                PixelFormat format = texture.Format;

                // Convert Vector4[] to Color[] based on format
                if (format == PixelFormat.R8G8B8A8_UNorm ||
                    format == PixelFormat.R8G8B8A8_UNorm_SRgb ||
                    format == PixelFormat.B8G8R8A8_UNorm ||
                    format == PixelFormat.B8G8R8A8_UNorm_SRgb)
                {
                    var colorData = new Color[size];
                    for (int i = 0; i < size; i++)
                    {
                        var v = data[i];
                        float r = Math.Max(0.0f, Math.Min(1.0f, v.X));
                        float g = Math.Max(0.0f, Math.Min(1.0f, v.Y));
                        float b = Math.Max(0.0f, Math.Min(1.0f, v.Z));
                        float a = Math.Max(0.0f, Math.Min(1.0f, v.W));

                        colorData[i] = new Color(
                            (byte)(r * 255.0f),
                            (byte)(g * 255.0f),
                            (byte)(b * 255.0f),
                            (byte)(a * 255.0f)
                        );
                    }
                    texture.SetData(commandList, colorData);
                }
                else if (format == PixelFormat.R32G32B32A32_Float)
                {
                    var colorData = new Color[size];
                    for (int i = 0; i < size; i++)
                    {
                        var v = data[i];
                        colorData[i] = new Color(v.X, v.Y, v.Z, v.W);
                    }
                    texture.SetData(commandList, colorData);
                }
                else if (format == PixelFormat.R16G16B16A16_Float)
                {
                    var colorData = new Color[size];
                    for (int i = 0; i < size; i++)
                    {
                        var v = data[i];
                        colorData[i] = new Color(v.X, v.Y, v.Z, v.W);
                    }
                    texture.SetData(commandList, colorData);
                }
                else
                {
                    // Fallback to R8G8B8A8_UNorm conversion
                    var colorData = new Color[size];
                    for (int i = 0; i < size; i++)
                    {
                        var v = data[i];
                        float r = Math.Max(0.0f, Math.Min(1.0f, v.X));
                        float g = Math.Max(0.0f, Math.Min(1.0f, v.Y));
                        float b = Math.Max(0.0f, Math.Min(1.0f, v.Z));
                        float a = Math.Max(0.0f, Math.Min(1.0f, v.W));

                        colorData[i] = new Color(
                            (byte)(r * 255.0f),
                            (byte)(g * 255.0f),
                            (byte)(b * 255.0f),
                            (byte)(a * 255.0f)
                        );
                    }
                    texture.SetData(commandList, colorData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideColorGrading] WriteTextureData failed: {ex.Message}");
            }
        }
    }
}

