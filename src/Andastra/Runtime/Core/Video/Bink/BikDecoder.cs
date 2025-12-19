using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Andastra.Runtime.Graphics;

namespace Andastra.Runtime.Core.Video.Bink
{
    /// <summary>
    /// BIK video decoder using BINKW32.DLL.
    /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 (main playback loop)
    /// </summary>
    internal class BikDecoder : IDisposable
    {
        private IntPtr _binkHandle;
        private IntPtr _bufferHandle;
        private BinkApi.BINKSUMMARY _summary;
        private bool _isDisposed;
        private readonly string _moviePath;
        private readonly IGraphicsDevice _graphicsDevice;
        private ITexture2D _frameTexture;
        private int _frameWidth;
        private int _frameHeight;
        private byte[] _frameBuffer;

        /// <summary>
        /// Initializes a new instance of the BikDecoder class.
        /// </summary>
        /// <param name="moviePath">Path to BIK file.</param>
        /// <param name="graphicsDevice">Graphics device for rendering.</param>
        public BikDecoder(string moviePath, IGraphicsDevice graphicsDevice)
        {
            _moviePath = moviePath ?? throw new ArgumentNullException("moviePath");
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException("graphicsDevice");
            _binkHandle = IntPtr.Zero;
            _bufferHandle = IntPtr.Zero;
            _isDisposed = false;
        }

        /// <summary>
        /// Opens the BIK file and initializes playback.
        /// Based on swkotor.exe: FUN_004053e0 @ 0x004053e0 (movie initialization)
        /// </summary>
        public void Open()
        {
            if (_binkHandle != IntPtr.Zero)
            {
                throw new InvalidOperationException("BikDecoder is already open");
            }

            // Open BIK file
            // Based on swkotor.exe: FUN_004053e0 @ 0x004053e0 calls BinkOpen
            _binkHandle = BinkApi.BinkOpen(_moviePath, BinkApi.BINKOPEN_NORMAL);
            if (_binkHandle == IntPtr.Zero)
            {
                throw new IOException(string.Format("Failed to open BIK file: {0}", _moviePath));
            }

            // Get movie summary
            _summary = new BinkApi.BINKSUMMARY();
            BinkApi.BinkGetSummary(_binkHandle, ref _summary);

            _frameWidth = _summary.Width;
            _frameHeight = _summary.Height;

            // Create buffer for video frames
            // Based on swkotor.exe: FUN_004053e0 @ 0x004053e0 buffer setup
            _bufferHandle = BinkApi.BinkBufferOpen(_frameWidth, _frameHeight, BinkApi.BINKBUFFER_BLIT_DIRECT);
            if (_bufferHandle == IntPtr.Zero)
            {
                BinkApi.BinkClose(_binkHandle);
                _binkHandle = IntPtr.Zero;
                throw new InvalidOperationException("Failed to create Bink buffer");
            }

            // Allocate frame buffer for copying to texture
            _frameBuffer = new byte[_frameWidth * _frameHeight * 4]; // RGBA

            // Create texture for rendering
            _frameTexture = _graphicsDevice.CreateTexture2D(_frameWidth, _frameHeight, null);
        }

        /// <summary>
        /// Gets the width of the video.
        /// </summary>
        public int Width
        {
            get { return _frameWidth; }
        }

        /// <summary>
        /// Gets the height of the video.
        /// </summary>
        public int Height
        {
            get { return _frameHeight; }
        }

        /// <summary>
        /// Gets the total number of frames.
        /// </summary>
        public int TotalFrames
        {
            get { return _summary.TotalFrames; }
        }

        /// <summary>
        /// Gets the current frame number.
        /// </summary>
        public int CurrentFrame
        {
            get
            {
                if (_binkHandle == IntPtr.Zero)
                {
                    return 0;
                }
                // Read frame number from BINK structure (offset 0x0C based on decompilation)
                // This is a simplified version - actual implementation would read from structure
                return 0; // TODO: Read from BINK structure
            }
        }

        /// <summary>
        /// Gets whether playback is complete.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 13 checks frame count
        /// </summary>
        public bool IsComplete
        {
            get
            {
                if (_binkHandle == IntPtr.Zero)
                {
                    return true;
                }
                // Check if current frame >= total frames
                // Based on decompilation: *(int *)(iVar1 + 8) != *(int *)(iVar1 + 0xc)
                // This checks if FrameNum != LastFrameNum
                return CurrentFrame >= TotalFrames;
            }
        }

        /// <summary>
        /// Decodes and renders the current frame.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 (main playback loop)
        /// </summary>
        /// <returns>True if frame was decoded, false if playback is complete.</returns>
        public bool DecodeFrame()
        {
            if (_binkHandle == IntPtr.Zero || IsComplete)
            {
                return false;
            }

            // Decode current frame
            // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 15 calls BinkDoFrame
            int result = BinkApi.BinkDoFrame(_binkHandle);
            if (result != 0)
            {
                return false; // Error decoding frame
            }

            // Lock buffer for writing
            // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 16 calls BinkBufferLock
            int lockResult = BinkApi.BinkBufferLock(_bufferHandle);
            if (lockResult != 0)
            {
                // Get buffer pointer from BINKBUFFER structure
                // Based on decompilation: *(int *)(param_1 + 0x4c) is the buffer pointer
                // We need to read the buffer data and copy to our texture
                
                // Copy frame to buffer
                // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 19-22 calls BinkCopyToBuffer
                // Parameters: bink, dest, destpitch, destheight, destx, desty, flags
                IntPtr bufferPtr = Marshal.ReadIntPtr(_bufferHandle, 0); // Buffer pointer
                int bufferPitch = Marshal.ReadInt32(_bufferHandle, 4); // Buffer pitch
                int bufferHeight = Marshal.ReadInt32(_bufferHandle, 8); // Buffer height

                // Copy from buffer to our frame buffer
                // Note: This is a simplified version - actual implementation would need to handle
                // different pixel formats and pitch alignment
                if (bufferPtr != IntPtr.Zero)
                {
                    // Read buffer data (assuming RGBA format)
                    Marshal.Copy(bufferPtr, _frameBuffer, 0, _frameBuffer.Length);
                }

                // Unlock buffer
                // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 23 calls BinkBufferUnlock
                BinkApi.BinkBufferUnlock(_bufferHandle);
            }

            // Update texture with frame data
            if (_frameTexture != null && _frameBuffer != null)
            {
                // Update texture (this would need to be implemented in ITexture2D interface)
                // For now, we'll need to create a new texture each frame or update the existing one
                // This depends on the graphics backend implementation
            }

            // Get destination rectangles for blitting
            // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 25-26 calls BinkGetRects
            IntPtr rects = BinkApi.BinkGetRects(_binkHandle, IntPtr.Zero);

            // Blit to screen
            // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 27 calls BinkBufferBlit
            if (rects != IntPtr.Zero)
            {
                BinkApi.BinkBufferBlit(_bufferHandle, rects, IntPtr.Zero);
            }

            // Advance to next frame
            // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 28 calls BinkNextFrame
            BinkApi.BinkNextFrame(_binkHandle);

            return true;
        }

        /// <summary>
        /// Waits for frame timing.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 29-33 (BinkWait with Sleep loop)
        /// </summary>
        public void WaitForFrame()
        {
            if (_binkHandle == IntPtr.Zero)
            {
                return;
            }

            // Wait for frame timing
            // Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 29-33
            // BinkWait returns 1 if need to wait more, 0 if ready
            int waitResult = BinkApi.BinkWait(_binkHandle);
            while (waitResult == 1)
            {
                // Sleep for 1ms and check again
                // Based on decompilation: Sleep(1); iVar1 = _BinkWait_4(...);
                Thread.Sleep(1);
                waitResult = BinkApi.BinkWait(_binkHandle);
            }
        }

        /// <summary>
        /// Gets the frame texture for rendering.
        /// </summary>
        public ITexture2D FrameTexture
        {
            get { return _frameTexture; }
        }

        /// <summary>
        /// Disposes resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_bufferHandle != IntPtr.Zero)
            {
                BinkApi.BinkBufferClose(_bufferHandle);
                _bufferHandle = IntPtr.Zero;
            }

            if (_binkHandle != IntPtr.Zero)
            {
                BinkApi.BinkClose(_binkHandle);
                _binkHandle = IntPtr.Zero;
            }

            if (_frameTexture != null)
            {
                _frameTexture.Dispose();
                _frameTexture = null;
            }

            _isDisposed = true;
        }
    }
}

