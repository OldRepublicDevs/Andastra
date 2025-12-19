using System;
using Andastra.Runtime.Graphics;

namespace Andastra.Runtime.MonoGame.Graphics
{
    /// <summary>
    /// MonoGame implementation of IDepthStencilBuffer.
    /// Note: MonoGame doesn't support separate depth buffers, they're part of render targets.
    /// TODO: STUB - This is a stub implementation.
    /// </summary>
    public class MonoGameDepthStencilBuffer : IDepthStencilBuffer
    {
        private readonly int _width;
        private readonly int _height;

        public MonoGameDepthStencilBuffer(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int Width => _width;

        public int Height => _height;

        public IntPtr NativeHandle => IntPtr.Zero;

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}

