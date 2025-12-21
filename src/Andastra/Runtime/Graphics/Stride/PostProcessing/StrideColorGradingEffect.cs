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
   