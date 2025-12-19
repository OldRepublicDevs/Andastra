using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Andastra.Parsing;
using Andastra.Parsing.Installation;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Formats.TPC;
using Andastra.Parsing.Formats.TXI;
using Andastra.Runtime.MonoGame.Converters;
using JetBrains.Annotations;

namespace Andastra.Runtime.MonoGame.Fonts
{
    /// <summary>
    /// Represents a bitmap font loaded from Odyssey engine (KOTOR) game resources.
    /// Fonts are texture-based with character glyphs stored in a grid, with metrics defined in TXI files.
    /// </summary>
    /// <remarks>
    /// Bitmap Font System (Odyssey Engine):
    /// - Based on swkotor.exe and swkotor2.exe font rendering system
    /// - Font textures: TPC/TGA files containing character glyphs in a fixed grid
    /// - Font metrics: TXI files containing character coordinates, spacing, and baseline information
    /// - Character rendering: Uses texture sampling with normalized UV coordinates from TXI
    /// - Text alignment: Supports all KOTOR alignment modes (top-left, top-center, top-right, center-left, center, center-right, bottom-left, bottom-center, bottom-right)
    /// - Font loading: Loads font texture and TXI metrics from game installation
    /// - Character mapping: Maps ASCII characters to texture coordinates using TXI upperleftcoords/lowerrightcoords
    /// 
    /// Ghidra Reverse Engineering Analysis Required:
    /// - swkotor.exe: Font loading functions (needs Ghidra address verification)
    /// - swkotor2.exe: Font rendering functions (needs Ghidra address verification)
    /// - nwmain.exe: Aurora engine font system (needs Ghidra analysis for equivalent implementation)
    /// - daorigins.exe: Eclipse engine font system (needs Ghidra analysis for equivalent implementation)
    /// - DragonAge2.exe: Eclipse engine font system (needs Ghidra analysis for equivalent implementation)
    /// - MassEffect.exe: Infinity engine font system (needs Ghidra analysis for equivalent implementation)
    /// - MassEffect2.exe: Infinity engine font system (needs Ghidra analysis for equivalent implementation)
    /// 
    /// Cross-Engine Inheritance Structure (to be implemented after Ghidra analysis):
    /// - Base Class: BaseBitmapFont (Runtime.Games.Common) - Common font loading/rendering patterns
    ///   - Odyssey: OdysseyBitmapFont : BaseBitmapFont (swkotor.exe: 0x..., swkotor2.exe: 0x...)
    ///   - Aurora: AuroraBitmapFont : BaseBitmapFont (nwmain.exe: 0x...)
    ///   - Eclipse: EclipseBitmapFont : BaseBitmapFont (daorigins.exe: 0x..., DragonAge2.exe: 0x...)
    ///   - Infinity: InfinityBitmapFont : BaseBitmapFont (MassEffect.exe: 0x..., MassEffect2.exe: 0x...)
    /// 
    /// Original implementation: Uses DirectX sprite rendering for text, this uses MonoGame SpriteBatch
    /// </remarks>
    public class BitmapFont
    {
        private readonly Texture2D _texture;
        private readonly TXIFeatures _fontMetrics;
        private readonly Dictionary<int, CharacterInfo> _characterMap;
        private readonly float _fontHeight;
        private readonly float _fontWidth;
        private readonly float _baselineHeight;
        private readonly float _spacingR;
        private readonly float _spacingB;
        private readonly int _textureWidth;
        private readonly int _textureHeight;

        /// <summary>
        /// Gets the font texture.
        /// </summary>
        public Texture2D Texture => _texture;

        /// <summary>
        /// Gets the font height in pixels.
        /// </summary>
        public float FontHeight => _fontHeight;

        /// <summary>
        /// Gets the font width in pixels (average character width).
        /// </summary>
        public float FontWidth => _fontWidth;

        /// <summary>
        /// Gets the baseline height for text alignment.
        /// </summary>
        public float BaselineHeight => _baselineHeight;

        /// <summary>
        /// Gets the horizontal spacing between characters.
        /// </summary>
        public float SpacingR => _spacingR;

        /// <summary>
        /// Gets the vertical spacing between lines.
        /// </summary>
        public float SpacingB => _spacingB;

        /// <summary>
        /// Gets the texture width in pixels.
        /// </summary>
        public int TextureWidth => _textureWidth;

        /// <summary>
        /// Gets the texture height in pixels.
        /// </summary>
        public int TextureHeight => _textureHeight;

        /// <summary>
        /// Internal structure for character glyph information.
        /// </summary>
        private class CharacterInfo
        {
            public Rectangle SourceRect { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
        }

        /// <summary>
        /// Loads a bitmap font from a ResRef.
        /// </summary>
        /// <param name="fontResRef">The font resource reference (without extension).</param>
        /// <param name="installation">The game installation for resource lookup.</param>
        /// <param name="graphicsDevice">The graphics device for texture creation.</param>
        /// <returns>The loaded bitmap font, or null if loading failed.</returns>
        [CanBeNull]
        public static BitmapFont Load([NotNull] string fontResRef, [NotNull] Installation installation, [NotNull] GraphicsDevice graphicsDevice)
        {
            if (string.IsNullOrEmpty(fontResRef))
            {
                Console.WriteLine("[BitmapFont] ERROR: Font ResRef cannot be null or empty");
                return null;
            }
            if (installation == null)
            {
                throw new ArgumentNullException("installation");
            }
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }

            try
            {
                // Load font texture (TPC or TGA)
                TPC fontTexture = null;
                var textureResult = installation.Resources.LookupResource(fontResRef, ResourceType.TPC, null, null);
                if (textureResult != null && textureResult.Data != null && textureResult.Data.Length > 0)
                {
                    fontTexture = TPCAuto.ReadTpc(textureResult.Data);
                }
                else
                {
                    // Try TGA as fallback
                    textureResult = installation.Resources.LookupResource(fontResRef, ResourceType.TGA, null, null);
                    if (textureResult != null && textureResult.Data != null && textureResult.Data.Length > 0)
                    {
                        fontTexture = TPCAuto.ReadTpc(textureResult.Data);
                    }
                }

                if (fontTexture == null)
                {
                    Console.WriteLine($"[BitmapFont] ERROR: Font texture not found: {fontResRef}");
                    return null;
                }

                // Convert TPC to MonoGame Texture2D
                Texture2D texture = TpcToMonoGameTextureConverter.Convert(fontTexture, graphicsDevice, false);
                if (texture == null)
                {
                    Console.WriteLine($"[BitmapFont] ERROR: Failed to convert font texture: {fontResRef}");
                    return null;
                }

                // Load TXI metrics
                TXI txi = null;
                string txiText = fontTexture.Txi;
                if (!string.IsNullOrEmpty(txiText))
                {
                    txi = new TXI(txiText);
                }
                else
                {
                    // Try loading separate TXI file
                    var txiResult = installation.Resources.LookupResource(fontResRef, ResourceType.TXI, null, null);
                    if (txiResult != null && txiResult.Data != null && txiResult.Data.Length > 0)
                    {
                        txi = TXIAuto.ReadTxi(txiResult.Data);
                    }
                }

                if (txi == null || txi.Features == null)
                {
                    Console.WriteLine($"[BitmapFont] WARNING: No TXI metrics found for font: {fontResRef}, using defaults");
                    // Create default TXI with basic metrics
                    txi = new TXI();
                }

                return new BitmapFont(texture, txi.Features, fontResRef);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BitmapFont] ERROR: Exception loading font {fontResRef}: {ex.Message}");
                Console.WriteLine($"[BitmapFont] Stack trace: {ex.StackTrace}");
                return null;
            }
        }

        /// <summary>
        /// Private constructor for creating a BitmapFont instance.
        /// </summary>
        private BitmapFont([NotNull] Texture2D texture, [NotNull] TXIFeatures fontMetrics, string fontResRef)
        {
            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }
            if (fontMetrics == null)
            {
                throw new ArgumentNullException("fontMetrics");
            }

            _texture = texture;
            _fontMetrics = fontMetrics;
            _textureWidth = texture.Width;
            _textureHeight = texture.Height;

            // Extract font metrics from TXI
            _fontHeight = fontMetrics.Fontheight ?? fontMetrics.Arturoheight ?? 16.0f;
            _fontWidth = fontMetrics.Fontwidth ?? fontMetrics.Arturowidth ?? 16.0f;
            _baselineHeight = fontMetrics.Baselineheight ?? _fontHeight;
            _spacingR = fontMetrics.SpacingR ?? 0.0f;
            _spacingB = fontMetrics.SpacingB ?? 0.0f;

            // Build character map from TXI coordinates
            _characterMap = BuildCharacterMap();
        }

        /// <summary>
        /// Builds the character map from TXI coordinates.
        /// </summary>
        private Dictionary<int, CharacterInfo> BuildCharacterMap()
        {
            Dictionary<int, CharacterInfo> charMap = new Dictionary<int, CharacterInfo>();

            if (_fontMetrics.Upperleftcoords == null || _fontMetrics.Lowerrightcoords == null)
            {
                // No coordinate data - use default grid-based mapping
                // This is a fallback for fonts without TXI coordinate data
                int charsPerRow = _fontMetrics.Cols ?? 16;
                int charsPerCol = _fontMetrics.Rows ?? 16;
                float cellWidth = _textureWidth / (float)charsPerRow;
                float cellHeight = _textureHeight / (float)charsPerCol;

                for (int i = 0; i < 256; i++)
                {
                    int row = i / charsPerRow;
                    int col = i % charsPerRow;
                    float x = col * cellWidth;
                    float y = row * cellHeight;

                    charMap[i] = new CharacterInfo
                    {
                        SourceRect = new Rectangle((int)x, (int)y, (int)cellWidth, (int)cellHeight),
                        Width = cellWidth,
                        Height = cellHeight
                    };
                }
            }
            else
            {
                // Use TXI coordinate data
                int numChars = Math.Min(_fontMetrics.Upperleftcoords.Count, _fontMetrics.Lowerrightcoords.Count);
                for (int i = 0; i < numChars && i < 256; i++)
                {
                    var upperLeft = _fontMetrics.Upperleftcoords[i];
                    var lowerRight = _fontMetrics.Lowerrightcoords[i];

                    // TXI coordinates are normalized (0.0-1.0), convert to pixel coordinates
                    // Note: TXI Y coordinates are inverted (0.0 = bottom, 1.0 = top)
                    float x1 = upperLeft.Item1 * _textureWidth;
                    float y1 = (1.0f - upperLeft.Item2) * _textureHeight; // Invert Y
                    float x2 = lowerRight.Item1 * _textureWidth;
                    float y2 = (1.0f - lowerRight.Item2) * _textureHeight; // Invert Y

                    float width = Math.Abs(x2 - x1);
                    float height = Math.Abs(y2 - y1);

                    charMap[i] = new CharacterInfo
                    {
                        SourceRect = new Rectangle((int)x1, (int)y1, (int)width, (int)height),
                        Width = width,
                        Height = height
                    };
                }
            }

            return charMap;
        }

        /// <summary>
        /// Gets character information for a specific character code.
        /// </summary>
        /// <param name="charCode">The character code (0-255).</param>
        /// <returns>Character information, or null if not found.</returns>
        [CanBeNull]
        public CharacterGlyph? GetCharacter(int charCode)
        {
            if (_characterMap.TryGetValue(charCode, out CharacterInfo info))
            {
                return new CharacterGlyph
                {
                    SourceRect = info.SourceRect,
                    Width = info.Width,
                    Height = info.Height
                };
            }
            return null;
        }

        /// <summary>
        /// Measures the size of text when rendered with this font.
        /// </summary>
        /// <param name="text">The text to measure.</param>
        /// <returns>The size of the text in pixels.</returns>
        public Vector2 MeasureString([CanBeNull] string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Vector2.Zero;
            }

            float maxWidth = 0.0f;
            float currentWidth = 0.0f;
            float totalHeight = _fontHeight;

            foreach (char c in text)
            {
                if (c == '\n')
                {
                    maxWidth = Math.Max(maxWidth, currentWidth);
                    currentWidth = 0.0f;
                    totalHeight += _fontHeight + _spacingB;
                }
                else
                {
                    int charCode = (int)c;
                    if (_characterMap.TryGetValue(charCode, out CharacterInfo info))
                    {
                        currentWidth += info.Width + _spacingR;
                    }
                    else
                    {
                        // Unknown character - use default width
                        currentWidth += _fontWidth + _spacingR;
                    }
                }
            }

            maxWidth = Math.Max(maxWidth, currentWidth);
            return new Vector2(maxWidth, totalHeight);
        }

        /// <summary>
        /// Character glyph information for rendering.
        /// </summary>
        public struct CharacterGlyph
        {
            public Rectangle SourceRect;
            public float Width;
            public float Height;
        }
    }
}

