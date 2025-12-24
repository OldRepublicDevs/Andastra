using StrideGraphics = Stride.Graphics;
using Stride.Core.Mathematics;
using Stride.Engine;
using Andastra.Runtime.Graphics;
using System.Numerics;
using RectangleF = Stride.Core.Mathematics.RectangleF;
using Andastra.Runtime.Stride.Graphics;

namespace Andastra.Runtime.Stride.Graphics
{
    /// <summary>
    /// Stride implementation of ISpriteBatch.
    /// </summary>
    public class StrideSpriteBatch : ISpriteBatch
    {
        private readonly StrideGraphics.SpriteBatch _spriteBatch;
        private readonly StrideGraphics.CommandList _commandList;
        private readonly StrideGraphics.GraphicsDevice _graphicsDevice;
        private bool _isBegun;

        internal StrideGraphics.SpriteBatch SpriteBatch => _spriteBatch;

        public StrideSpriteBatch(StrideGraphics.SpriteBatch spriteBatch, StrideGraphics.CommandList commandList = null, StrideGraphics.GraphicsDevice graphicsDevice = null)
        {
            _spriteBatch = spriteBatch ?? throw new System.ArgumentNullException(nameof(spriteBatch));
            _commandList = commandList;
            _graphicsDevice = graphicsDevice;
            // GraphicsDevice is obtained from SpriteBatch when Begin() is called
            // This ensures we always get the current GraphicsDevice, allowing for dynamic changes
        }

        public void Begin(Andastra.Runtime.Graphics.SpriteSortMode sortMode = Andastra.Runtime.Graphics.SpriteSortMode.Deferred, Andastra.Runtime.Graphics.BlendState blendState = null)
        {
            if (_isBegun)
            {
                throw new System.InvalidOperationException("SpriteBatch.Begin() called while already begun. Call End() first.");
            }

            // Get GraphicsContext for Begin() call
            // Stride SpriteBatch.Begin() requires GraphicsContext (not CommandList)
            StrideGraphics.GraphicsContext graphicsContext = null;
            if (_graphicsDevice != null)
            {
                graphicsContext = _graphicsDevice.GraphicsContext();
            }

            if (graphicsContext == null)
            {
                throw new System.InvalidOperationException("GraphicsContext is required for SpriteBatch.Begin(). StrideSpriteBatch must be created with a valid GraphicsDevice, or GraphicsDevice must be registered with GraphicsDeviceExtensions.");
            }

            var strideSortMode = ConvertSortMode(sortMode);

            // Convert blend state to Stride BlendStates values
            // BlendStates is a static class with static properties that return BlendStateDescription objects
            StrideGraphics.BlendStateDescription strideBlendStateValue;
            if (blendState == null)
            {
                strideBlendStateValue = StrideGraphics.BlendStates.AlphaBlend;
            }
            else if (blendState.Additive)
            {
                strideBlendStateValue = StrideGraphics.BlendStates.Additive;
            }
            else
            {
                strideBlendStateValue = StrideGraphics.BlendStates.AlphaBlend;
            }

            // Stride SpriteBatch.Begin accepts GraphicsContext, SpriteSortMode, and BlendStateDescription
            _spriteBatch.Begin(graphicsContext, strideSortMode, strideBlendStateValue);
            _isBegun = true;
        }

        public void End()
        {
            if (!_isBegun)
            {
                throw new System.InvalidOperationException("SpriteBatch.End() called without matching Begin().");
            }

            _spriteBatch.End();
            _isBegun = false;
        }

        public void Draw(ITexture2D texture, Andastra.Runtime.Graphics.Vector2 position, Andastra.Runtime.Graphics.Color color)
        {
            EnsureBegun();
            var strideTexture = GetStrideTexture(texture);
            var strideColor = new global::Stride.Core.Mathematics.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            var strideRect = new RectangleF(position.X, position.Y, texture.Width, texture.Height);
            _spriteBatch.Draw(strideTexture, strideRect, strideColor);
        }

        public void Draw(ITexture2D texture, Andastra.Runtime.Graphics.Rectangle destinationRectangle, Andastra.Runtime.Graphics.Color color)
        {
            EnsureBegun();
            var strideTexture = GetStrideTexture(texture);
            var strideColor = new global::Stride.Core.Mathematics.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            var strideRect = new RectangleF(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height);
            _spriteBatch.Draw(strideTexture, strideRect, strideColor);
        }

        public void Draw(ITexture2D texture, Andastra.Runtime.Graphics.Vector2 position, Andastra.Runtime.Graphics.Rectangle? sourceRectangle, Andastra.Runtime.Graphics.Color color)
        {
            EnsureBegun();
            var strideTexture = GetStrideTexture(texture);
            var strideColor = new global::Stride.Core.Mathematics.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            RectangleF? strideSrcRect = null;
            if (sourceRectangle.HasValue)
            {
                var rect = sourceRectangle.Value;
                strideSrcRect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
            }
            // For Vector2 position, use full texture size as destination (or source rectangle size if specified)
            RectangleF strideDestRect;
            if (strideSrcRect.HasValue)
            {
                strideDestRect = new RectangleF(position.X, position.Y, strideSrcRect.Value.Width, strideSrcRect.Value.Height);
            }
            else
            {
                strideDestRect = new RectangleF(position.X, position.Y, texture.Width, texture.Height);
            }
            // Stride SpriteBatch.Draw signature: Draw(Texture, Rectangle destination, Color)
            // Source rectangles are not directly supported, so we use the destination rectangle
            // TODO: IMPLEMENT - Full source rectangle support requires texture region/view or UV manipulation
            _spriteBatch.Draw(strideTexture, strideDestRect, strideColor);
        }

        public void Draw(ITexture2D texture, Andastra.Runtime.Graphics.Rectangle destinationRectangle, Andastra.Runtime.Graphics.Rectangle? sourceRectangle, Andastra.Runtime.Graphics.Color color, float rotation, Andastra.Runtime.Graphics.Vector2 origin, Andastra.Runtime.Graphics.SpriteEffects effects, float layerDepth)
        {
            EnsureBegun();
            var strideTexture = GetStrideTexture(texture);
            var strideColor = new global::Stride.Core.Mathematics.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);

            // Handle source rectangle with sprite effects (flipping)
            RectangleF? strideSrcRect = null;
            if (sourceRectangle.HasValue)
            {
                var rect = sourceRectangle.Value;
                strideSrcRect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);

                // Apply sprite effects by manipulating source rectangle coordinates
                if ((effects & Andastra.Runtime.Graphics.SpriteEffects.FlipHorizontally) != 0)
                {
                    // Flip horizontally by swapping left/right UV coordinates
                    strideSrcRect = new RectangleF(
                        strideSrcRect.Value.X + strideSrcRect.Value.Width,  // Start from right edge
                        strideSrcRect.Value.Y,
                        -strideSrcRect.Value.Width,  // Negative width flips horizontally
                        strideSrcRect.Value.Height
                    );
                }
                if ((effects & Andastra.Runtime.Graphics.SpriteEffects.FlipVertically) != 0)
                {
                    // Flip vertically by swapping top/bottom UV coordinates
                    strideSrcRect = new RectangleF(
                        strideSrcRect.Value.X,
                        strideSrcRect.Value.Y + strideSrcRect.Value.Height,  // Start from bottom edge
                        strideSrcRect.Value.Width,
                        -strideSrcRect.Value.Height  // Negative height flips vertically
                    );
                }
            }

            // If no rotation is needed, use the optimized path
            if (System.Math.Abs(rotation) < 0.001f)
            {
                var strideDestRect = new RectangleF(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height);
                // Stride SpriteBatch.Draw signature: Draw(Texture, Rectangle destination, Color)
                // Source rectangles are not directly supported
                // TODO: IMPLEMENT - Full source rectangle support requires texture region/view or UV manipulation
                _spriteBatch.Draw(strideTexture, strideDestRect, strideColor);
                return;
            }

            // Apply rotation and origin transformation
            // Calculate the transformed corners of the destination rectangle
            var destX = destinationRectangle.X;
            var destY = destinationRectangle.Y;
            var destWidth = destinationRectangle.Width;
            var destHeight = destinationRectangle.Height;

            // Origin offset from top-left of destination rectangle
            var originOffsetX = origin.X;
            var originOffsetY = origin.Y;

            // Calculate the four corners relative to origin
            var corners = new global::Stride.Core.Mathematics.Vector2[4];
            corners[0] = new global::Stride.Core.Mathematics.Vector2(-originOffsetX, -originOffsetY);  // Top-left
            corners[1] = new global::Stride.Core.Mathematics.Vector2(destWidth - originOffsetX, -originOffsetY);  // Top-right
            corners[2] = new global::Stride.Core.Mathematics.Vector2(destWidth - originOffsetX, destHeight - originOffsetY);  // Bottom-right
            corners[3] = new global::Stride.Core.Mathematics.Vector2(-originOffsetX, destHeight - originOffsetY);  // Bottom-left

            // Apply rotation to each corner
            var cos = (float)System.Math.Cos(rotation);
            var sin = (float)System.Math.Sin(rotation);
            for (int i = 0; i < 4; i++)
            {
                var x = corners[i].X * cos - corners[i].Y * sin;
                var y = corners[i].X * sin + corners[i].Y * cos;
                corners[i] = new global::Stride.Core.Mathematics.Vector2(x, y);
            }

            // Translate back to destination position
            for (int i = 0; i < 4; i++)
            {
                corners[i].X += destX + originOffsetX;
                corners[i].Y += destY + originOffsetY;
            }

            // Create destination rectangle from transformed corners
            // Use the axis-aligned bounding box of the rotated rectangle
            var minX = float.MaxValue;
            var minY = float.MaxValue;
            var maxX = float.MinValue;
            var maxY = float.MinValue;

            for (int i = 0; i < 4; i++)
            {
                minX = System.Math.Min(minX, corners[i].X);
                minY = System.Math.Min(minY, corners[i].Y);
                maxX = System.Math.Max(maxX, corners[i].X);
                maxY = System.Math.Max(maxY, corners[i].Y);
            }

            var strideDestRect = new RectangleF(minX, minY, maxX - minX, maxY - minY);
            // Stride SpriteBatch.Draw doesn't support source rectangles directly
            // Source rectangle is ignored - full texture is drawn to destination
            _spriteBatch.Draw(strideTexture, strideDestRect, strideColor);
        }

        public void DrawString(IFont font, string text, Andastra.Runtime.Graphics.Vector2 position, Andastra.Runtime.Graphics.Color color)
        {
            EnsureBegun();
            var strideFont = GetStrideFont(font);
            var strideColor = new global::Stride.Core.Mathematics.Color4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
            _spriteBatch.DrawString(strideFont, text, new global::Stride.Core.Mathematics.Vector2(position.X, position.Y), strideColor);
        }

        private void EnsureBegun()
        {
            if (!_isBegun)
            {
                throw new System.InvalidOperationException("SpriteBatch operations must be called between Begin() and End().");
            }
        }

        private global::Stride.Graphics.Texture GetStrideTexture(ITexture2D texture)
        {
            if (texture is StrideTexture2D strideTexture)
            {
                return strideTexture.Texture;
            }
            throw new System.ArgumentException("Texture must be a StrideTexture2D", nameof(texture));
        }

        private StrideGraphics.SpriteFont GetStrideFont(IFont font)
        {
            if (font is StrideFont strideFont)
            {
                return strideFont.Font;
            }
            throw new System.ArgumentException("Font must be a StrideFont", nameof(font));
        }

        private StrideGraphics.SpriteSortMode ConvertSortMode(Andastra.Runtime.Graphics.SpriteSortMode sortMode)
        {
            // Stride uses the same enum values, so we can cast directly
            return (StrideGraphics.SpriteSortMode)sortMode;
        }


        private StrideGraphics.SpriteEffects ConvertSpriteEffects(Andastra.Runtime.Graphics.SpriteEffects effects)
        {
            StrideGraphics.SpriteEffects result = StrideGraphics.SpriteEffects.None;
            if ((effects & Andastra.Runtime.Graphics.SpriteEffects.FlipHorizontally) != 0)
            {
                result |= StrideGraphics.SpriteEffects.FlipHorizontally;
            }
            if ((effects & Andastra.Runtime.Graphics.SpriteEffects.FlipVertically) != 0)
            {
                result |= StrideGraphics.SpriteEffects.FlipVertically;
            }
            return result;
        }

        public void Dispose()
        {
            _spriteBatch?.Dispose();
        }
    }
}

