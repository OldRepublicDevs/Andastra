using System;
using StrideGraphics = Stride.Graphics;
using Andastra.Runtime.Graphics;

namespace Andastra.Runtime.Stride.Graphics
{
    /// <summary>
    /// Stride implementation of IRenderTarget.
    /// </summary>
    public class StrideRenderTarget : IRenderTarget
    {
        internal readonly StrideGraphics.Texture2D RenderTarget;
        private readonly StrideGraphics.Texture2D _depthBuffer;

        public StrideRenderTarget(StrideGraphics.Texture2D renderTarget, StrideGraphics.Texture2D depthBuffer = null)
        {
            RenderTarget = renderTarget ?? throw new ArgumentNullException(nameof(renderTarget));
            _depthBuffer = depthBuffer;
        }

        public int Width => RenderTarget.Width;

        public int Height => RenderTarget.Height;

        public ITexture2D ColorTexture => new StrideTexture2D(RenderTarget);

        public IDepthStencilBuffer DepthStencilBuffer
        {
            get
            {
                if (_depthBuffer != null)
                {
                    return new StrideDepthStencilBuffer(_depthBuffer);
                }
                return null;
            }
        }

        public IntPtr NativeHandle => RenderTarget.NativeDeviceTexture;

        public void Dispose()
        {
            RenderTarget?.Dispose();
            _depthBuffer?.Dispose();
        }
    }
}

