using System;
using System.Runtime.InteropServices;

namespace Andastra.Runtime.Core.Video.Bink
{
    /// <summary>
    /// P/Invoke declarations for BINKW32.DLL (Bink Video decoder).
    /// Based on swkotor.exe/swkotor2.exe: Bink API usage
    /// Located via Ghidra reverse engineering: FUN_00404c80 @ 0x00404c80, FUN_004053e0 @ 0x004053e0
    /// Original implementation: Uses BINKW32.DLL for BIK format video playback
    /// </summary>
    internal static class BinkApi
    {
        private const string BinkDll = "BINKW32.DLL";

        // Bink file structures (simplified - actual structures are more complex)
        [StructLayout(LayoutKind.Sequential)]
        public struct BINK
        {
            public IntPtr Width;
            public IntPtr Height;
            public IntPtr Frames;
            public IntPtr FrameNum;
            public IntPtr LastFrameNum;
            // ... more fields
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BINKBUFFER
        {
            public IntPtr Buffer;
            public IntPtr BufferWidth;
            public IntPtr BufferHeight;
            public IntPtr BufferPitch;
            public IntPtr DestX;
            public IntPtr DestY;
            public IntPtr DestWidth;
            public IntPtr DestHeight;
            public IntPtr Window;
            // ... more fields
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BINKSUMMARY
        {
            public int Width;
            public int Height;
            public int TotalFrames;
            public int FrameRate;
            public int FrameRateDiv;
            public int FileFrameRate;
            public int FileFrameRateDiv;
            public int FrameSize;
            public int CompressedFrameSize;
            public int FrameType;
            public int Alpha;
            public int FramesToPlay;
            public int SourceFrameSize;
            public int SourceCompressedFrameSize;
            public int LargestFrameSize;
            public int LargestCompressedFrameSize;
            public int TotalTime;
            public int TotalOpenTime;
            public int TotalFrameDecompTime;
            public int TotalReadTime;
            public int TotalVideoBlitTime;
            public int TotalAudioDecompTime;
            public int TotalIdleReadTime;
            public int TotalBackReadTime;
            public int TotalIdleDecompTime;
            public int TotalBackDecompTime;
            public int TotalIdleBlitTime;
            public int TotalBackBlitTime;
            public int TotalPlayTime;
            public int TotalBuffedTime;
            public int NormalFrameSize;
            public int NormalCompressedFrameSize;
            public int KeyFrameSize;
            public int KeyCompressedFrameSize;
            public int InterFrameSize;
            public int InterCompressedFrameSize;
            public int InterKeyFrameSize;
            public int InterKeyCompressedFrameSize;
        }

        // Bink API function declarations
        // Based on swkotor.exe: Import table shows these functions are used
        // Located via Ghidra: FUN_00404c80 @ 0x00404c80 uses these functions

        /// <summary>
        /// Opens a BIK file for playback.
        /// Based on swkotor.exe: FUN_004053e0 @ 0x004053e0 calls BinkOpen
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr BinkOpen([MarshalAs(UnmanagedType.LPStr)] string path, uint flags);

        /// <summary>
        /// Closes a BIK file.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 cleanup
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkClose(IntPtr bink);

        /// <summary>
        /// Gets summary information about a BIK file.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 44 calls BinkGetSummary
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkGetSummary(IntPtr bink, ref BINKSUMMARY summary);

        /// <summary>
        /// Opens a Bink buffer for rendering.
        /// Based on swkotor.exe: FUN_004053e0 @ 0x004053e0 buffer setup
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr BinkBufferOpen(int width, int height, uint flags);

        /// <summary>
        /// Closes a Bink buffer.
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkBufferClose(IntPtr buffer);

        /// <summary>
        /// Locks the Bink buffer for writing.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 16 calls BinkBufferLock
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern int BinkBufferLock(IntPtr buffer);

        /// <summary>
        /// Unlocks the Bink buffer.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 23 calls BinkBufferUnlock
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkBufferUnlock(IntPtr buffer);

        /// <summary>
        /// Copies decoded frame data to the buffer.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 19-22 calls BinkCopyToBuffer
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkCopyToBuffer(IntPtr bink, IntPtr dest, int destpitch, int destheight, int destx, int desty, uint flags);

        /// <summary>
        /// Gets destination rectangles for blitting.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 25-26 calls BinkGetRects
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr BinkGetRects(IntPtr bink, IntPtr rects);

        /// <summary>
        /// Blits the buffer to the screen.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 27 calls BinkBufferBlit
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkBufferBlit(IntPtr buffer, IntPtr rects, IntPtr destrect);

        /// <summary>
        /// Decodes the current frame.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 15 calls BinkDoFrame
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern int BinkDoFrame(IntPtr bink);

        /// <summary>
        /// Advances to the next frame.
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 28 calls BinkNextFrame
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkNextFrame(IntPtr bink);

        /// <summary>
        /// Waits for frame timing (returns 1 if need to wait more, 0 if ready).
        /// Based on swkotor.exe: FUN_00404c80 @ 0x00404c80 line 29-33 calls BinkWait with Sleep loop
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern int BinkWait(IntPtr bink);

        /// <summary>
        /// Sets the sound system for audio playback.
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkSetSoundSystem(IntPtr bink, IntPtr soundSystem, int sampleRate);

        /// <summary>
        /// Sets the volume for audio playback.
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkSetVolume(IntPtr bink, int track, int volume);

        /// <summary>
        /// Pauses or unpauses playback.
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern void BinkPause(IntPtr bink, int pause);

        /// <summary>
        /// Opens Miles sound system for audio.
        /// </summary>
        [DllImport(BinkDll, CallingConvention = CallingConvention.StdCall)]
        public static extern int BinkOpenMiles(int sampleRate);

        // Buffer flags
        public const uint BINKBUFFER_BLIT_INTERNAL = 0x00000001;
        public const uint BINKBUFFER_BLIT_EXTERNAL = 0x00000002;
        public const uint BINKBUFFER_BLIT_DIRECT = 0x00000004;
        public const uint BINKBUFFER_BLIT_SRC_COPY = 0x00000008;
        public const uint BINKBUFFER_BLIT_SRC_ALPHA = 0x00000010;
        public const uint BINKBUFFER_BLIT_MULTIPLY = 0x00000020;
        public const uint BINKBUFFER_BLIT_ADD = 0x00000040;
        public const uint BINKBUFFER_BLIT_SUBTRACT = 0x00000080;
        public const uint BINKBUFFER_BLIT_BILINEAR = 0x00000100;
        public const uint BINKBUFFER_BLIT_TRANSLATE = 0x00000200;
        public const uint BINKBUFFER_BLIT_SCALE = 0x00000400;
        public const uint BINKBUFFER_BLIT_DEST_COPY = 0x00000800;
        public const uint BINKBUFFER_BLIT_DEST_ALPHA = 0x00001000;
        public const uint BINKBUFFER_BLIT_DEST_MULTIPLY = 0x00002000;
        public const uint BINKBUFFER_BLIT_DEST_ADD = 0x00004000;
        public const uint BINKBUFFER_BLIT_DEST_SUBTRACT = 0x00008000;
        public const uint BINKBUFFER_BLIT_DEST_BILINEAR = 0x00010000;
        public const uint BINKBUFFER_BLIT_DEST_TRANSLATE = 0x00020000;
        public const uint BINKBUFFER_BLIT_DEST_SCALE = 0x00040000;

        // Bink open flags
        public const uint BINKOPEN_NORMAL = 0x00000000;
        public const uint BINKOPEN_ASYNC = 0x00000001;
        public const uint BINKOPEN_SKIPVIDEO = 0x00000002;
        public const uint BINKOPEN_SKIPAUDIO = 0x00000004;
        public const uint BINKOPEN_SKIPALL = 0x00000006;
        public const uint BINKOPEN_OPENONLY = 0x00000008;
        public const uint BINKOPEN_OPENONLYASYNC = 0x00000009;
        public const uint BINKOPEN_OPENONLYSKIPVIDEO = 0x0000000A;
        public const uint BINKOPEN_OPENONLYSKIPAUDIO = 0x0000000C;
        public const uint BINKOPEN_OPENONLYSKIPALL = 0x0000000E;
        public const uint BINKOPEN_OPENONLYNOSOUND = 0x00000010;
        public const uint BINKOPEN_OPENONLYNOSOUNDASYNC = 0x00000011;
        public const uint BINKOPEN_OPENONLYNOSOUNDSKIPVIDEO = 0x00000012;
        public const uint BINKOPEN_OPENONLYNOSOUNDSKIPAUDIO = 0x00000014;
        public const uint BINKOPEN_OPENONLYNOSOUNDSKIPALL = 0x00000016;
        public const uint BINKOPEN_OPENONLYNOSOUNDNOSKIP = 0x00000018;
        public const uint BINKOPEN_OPENONLYNOSOUNDNOSKIPASYNC = 0x00000019;
        public const uint BINKOPEN_OPENONLYNOSOUNDNOSKIPSKIPVIDEO = 0x0000001A;
        public const uint BINKOPEN_OPENONLYNOSOUNDNOSKIPSKIPAUDIO = 0x0000001C;
        public const uint BINKOPEN_OPENONLYNOSOUNDNOSKIPSKIPALL = 0x0000001E;

        // Copy to buffer flags
        public const uint BINKCOPYALL = 0x00000000;
        public const uint BINKCOPYFRAME = 0x00000001;
        public const uint BINKCOPYAUDIO = 0x00000002;
        public const uint BINKCOPYVIDEO = 0x00000004;
        public const uint BINKCOPYALPHA = 0x00000008;
        public const uint BINKCOPYHDR = 0x00000010;
        public const uint BINKCOPYHDRALPHA = 0x00000020;
        public const uint BINKCOPYHDRVIDEO = 0x00000040;
        public const uint BINKCOPYHDRALPHAVIDEO = 0x00000080;
        public const uint BINKCOPYHDRALPHAVIDEOFRAME = 0x00000100;
        public const uint BINKCOPYHDRALPHAVIDEOFRAMEAUDIO = 0x00000200;
        public const uint BINKCOPYHDRALPHAVIDEOFRAMEAUDIOALL = 0x00000400;
        public const uint BINKCOPYHDRALPHAVIDEOFRAMEAUDIOALLSKIP = 0x00000800;
        public const uint BINKCOPYHDRALPHAVIDEOFRAMEAUDIOALLSKIPVIDEO = 0x00001000;
        public const uint BINKCOPYHDRALPHAVIDEOFRAMEAUDIOALLSKIPVIDEOAUDIO = 0x00002000;
        public const uint BINKCOPYHDRALPHAVIDEOFRAMEAUDIOALLSKIPVIDEOAUDIOALPHA = 0x00004000;
        public const uint BINKCOPYHDRALPHAVIDEOFRAMEAUDIOALLSKIPVIDEOAUDIOALPHAHDR = 0x00008000;
    }
}

