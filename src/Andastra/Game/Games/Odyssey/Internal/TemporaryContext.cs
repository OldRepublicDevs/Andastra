using System;
using JetBrains.Annotations;

namespace Andastra.Game.Games.Odyssey.Internal
{
    /// <summary>
    /// Temporary context object management for exception-safe resource handling.
    /// Implements RAII pattern equivalent to k2_win_gog_aspyr_swkotor2.exe @ 0x00631f70 (constructor) and @ 0x00632000 (destructor).
    /// </summary>
    /// <remarks>
    /// Temporary Object Management (k2_win_gog_aspyr_swkotor2.exe):
    /// - Constructor: 0x00631f70 @ 0x00631f70 - Allocates 0xc (12) bytes via operator_new, calls 0x00635e30() to initialize context object, stores pointer in output parameter
    /// - Destructor: 0x00632000 @ 0x00632000 - Cleans up the 12-byte context object allocated by constructor
    /// - swkotor2_aspyr.exe equivalent: 0x00736240 (constructor), 0x007362c0 (destructor)
    /// - Used for exception-safe context initialization during game session startup
    /// - Context object is 12 bytes (0xc) and is initialized by 0x00635e30() function
    /// - Original implementation: Stack-based RAII wrapper for exception-safe resource management
    /// - Located via execution flow analysis: Used in 0x006d0b00 (New Game handler) for context initialization
    /// </remarks>
    internal sealed class TemporaryContext : IDisposable
    {
        private readonly IntPtr _contextPtr;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the TemporaryContext class.
        /// Equivalent to k2_win_gog_aspyr_swkotor2.exe @ 0x00631f70 @ 0x00631f70.
        /// </summary>
        /// <param name="contextPtr">Output parameter that receives the allocated context pointer.</param>
        /// <remarks>
        /// Constructor behavior (k2_win_gog_aspyr_swkotor2.exe @ 0x00631f70):
        /// - Allocates 0xc (12) bytes via operator_new
        /// - Calls 0x00635e30() to initialize context object
        /// - Stores pointer in output parameter (local_44 in original code)
        /// - Original implementation: Exception-safe allocation and initialization
        /// </remarks>
        public TemporaryContext(out IntPtr contextPtr)
        {
            // Allocate 12 bytes (0xc) for context object
            // Equivalent to operator_new(0xc) in original code
            _contextPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(12);

            // Initialize the context object
            // Equivalent to 0x00635e30() call in original code
            InitializeContext(_contextPtr);

            // Store pointer in output parameter
            // Equivalent to storing in local_44 in original code
            contextPtr = _contextPtr;
            _disposed = false;
        }

        /// <summary>
        /// Initializes the context object at the specified pointer.
        /// Equivalent to k2_win_gog_aspyr_swkotor2.exe @ 0x00635e30 @ 0x00635e30.
        /// </summary>
        /// <param name="contextPtr">Pointer to the 12-byte context object to initialize.</param>
        /// <remarks>
        /// Initialization function (k2_win_gog_aspyr_swkotor2.exe @ 0x00635e30):
        /// - Initializes the 12-byte context structure
        /// - Sets up context state for exception-safe resource management
        /// - Original implementation: Zero-initializes structure and sets up exception handling context
        /// - Called by constructor (0x00631f70) after allocation
        /// </remarks>
        private static void InitializeContext(IntPtr contextPtr)
        {
            if (contextPtr == IntPtr.Zero)
            {
                throw new ArgumentNullException(nameof(contextPtr));
            }

            // Zero-initialize the 12-byte context structure
            // Original implementation: memset(contextPtr, 0, 12) or equivalent zero-initialization
            byte[] zeroBuffer = new byte[12];
            System.Runtime.InteropServices.Marshal.Copy(zeroBuffer, 0, contextPtr, 12);

            // Context structure layout (12 bytes = 0xc):
            // Offset 0x0-0x3: Exception state tracking (4 bytes, int)
            // Offset 0x4-0x7: Context flags (4 bytes, int)
            // Offset 0x8-0xb: Reserved/padding (4 bytes, int)
            // Original implementation: Structure is zero-initialized and ready for use
        }

        /// <summary>
        /// Disposes of the temporary context object.
        /// Equivalent to k2_win_gog_aspyr_swkotor2.exe @ 0x00632000 @ 0x00632000.
        /// </summary>
        /// <remarks>
        /// Destructor behavior (k2_win_gog_aspyr_swkotor2.exe @ 0x00632000):
        /// - Cleans up the 12-byte context object allocated by constructor
        /// - Frees memory allocated via operator_new
        /// - Original implementation: Exception-safe cleanup in destructor
        /// - Called automatically when TemporaryContext goes out of scope (RAII pattern)
        /// </remarks>
        public void Dispose()
        {
            if (!_disposed)
            {
                if (_contextPtr != IntPtr.Zero)
                {
                    // Free the allocated memory
                    // Equivalent to operator_delete(_contextPtr) in original code
                    System.Runtime.InteropServices.Marshal.FreeHGlobal(_contextPtr);
                }

                _disposed = true;
            }
        }
    }
}
