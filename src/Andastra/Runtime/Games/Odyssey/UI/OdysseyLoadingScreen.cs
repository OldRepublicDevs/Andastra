using System;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;
using Andastra.Runtime.Graphics.MonoGame.GUI;

namespace Andastra.Runtime.Games.Odyssey.UI
{
    /// <summary>
    /// Odyssey engine loading screen implementation using GUI system.
    /// </summary>
    /// <remarks>
    /// Odyssey Loading Screen Implementation:
    /// - Based on swkotor.exe and swkotor2.exe loading screen system
    /// - Located via string references: "loadscreen_p" @ 0x007cbe40 (swkotor2.exe), "loadscreen" @ 0x00752db0 (swkotor.exe)
    /// - "LoadScreenID" @ 0x007bd54c (swkotor2.exe), "LoadScreenID" @ 0x00747880 (swkotor.exe)
    /// - "LBL_LOADING" @ 0x007cbe10 (swkotor2.exe), "Loading" @ 0x007c7e40 (swkotor2.exe)
    /// - "PB_PROGRESS" @ 0x007cb33c (progress bar), "LBL_HINT" (loading hints), "LBL_LOGO" (logo label)
    /// - Original implementation: FUN_006cff90 @ 0x006cff90 (swkotor2.exe) initializes loading screen GUI panel
    /// - Loading screen GUI: "loadscreen_p" GUI file contains panel with progress bar, hints, logo, and loading image
    /// - Loading screen image: Set via LoadScreenResRef from module IFO file (TPC format texture)
    /// - Loading screen display: Shown during module transitions, hidden after module load completes
    /// - Progress bar: Shows loading progress (0-100) during resource loading
    /// - Loading hints: Random hints from loadscreenhints.2da displayed during loading
    /// - Original engine: Uses DirectX sprite rendering for loading screen GUI
    /// - This implementation: Uses MonoGame SpriteBatch via KotorGuiManager for loading screen rendering
    /// </remarks>
    public class OdysseyLoadingScreen : ILoadingScreen
    {
        private readonly BaseGuiManager _guiManager;
        private bool _isVisible;
        private string _currentImageResRef;
        private int _currentProgress;

        /// <summary>
        /// Initializes a new instance of the Odyssey loading screen.
        /// </summary>
        /// <param name="guiManager">GUI manager for loading and displaying the loading screen GUI.</param>
        /// <exception cref="ArgumentNullException">Thrown when guiManager is null.</exception>
        public OdysseyLoadingScreen(BaseGuiManager guiManager)
        {
            if (guiManager == null)
            {
                throw new ArgumentNullException("guiManager");
            }

            _guiManager = guiManager;
            _isVisible = false;
            _currentImageResRef = null;
            _currentProgress = 0;
        }

        /// <summary>
        /// Gets whether the loading screen is currently visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
        }

        /// <summary>
        /// Shows the loading screen with the specified image.
        /// Based on swkotor2.exe: FUN_006cff90 @ 0x006cff90 initializes loading screen GUI
        /// - Loads "loadscreen_p" GUI panel
        /// - Sets loading screen image via LoadScreenResRef (TPC texture)
        /// - Displays progress bar, hints, and logo
        /// - Original implementation: Shows loading screen during module transitions
        /// </summary>
        /// <param name="imageResRef">Resource reference for the loading screen image (TPC format). If null or empty, uses default loading screen.</param>
        public void Show(string imageResRef)
        {
            if (_isVisible)
            {
                // Already visible, just update the image if different
                if (!string.Equals(_currentImageResRef, imageResRef, StringComparison.OrdinalIgnoreCase))
                {
                    _currentImageResRef = imageResRef;
                    // Update the loading screen image if GUI supports it
                    UpdateLoadingScreenImage(imageResRef);
                }
                return;
            }

            _isVisible = true;
            _currentImageResRef = imageResRef ?? "load_default";
            _currentProgress = 0;

            // Load the loading screen GUI panel
            // Based on swkotor2.exe: "loadscreen_p" GUI panel is loaded and displayed
            // Original implementation: FUN_006cff90 @ 0x006cff90 loads "loadscreen_p" GUI
            int screenWidth = 800; // Default resolution
            int screenHeight = 600;
            
            // Get screen dimensions from graphics device
            try
            {
                var graphicsDevice = _guiManager.GraphicsDevice;
                if (graphicsDevice != null && graphicsDevice.Viewport.Width > 0 && graphicsDevice.Viewport.Height > 0)
                {
                    screenWidth = graphicsDevice.Viewport.Width;
                    screenHeight = graphicsDevice.Viewport.Height;
                }
            }
            catch
            {
                // Fallback to default resolution if viewport is not available
            }

            // Load the loading screen GUI
            // Based on swkotor2.exe: Loads "loadscreen_p" GUI panel
            bool loaded = _guiManager.LoadGui("loadscreen_p", screenWidth, screenHeight);
            if (!loaded)
            {
                // Fallback: Try without "_p" suffix
                loaded = _guiManager.LoadGui("loadscreen", screenWidth, screenHeight);
            }

            if (loaded)
            {
                // Set the current GUI to the loading screen
                // Try "loadscreen_p" first, then fallback to "loadscreen"
                bool set = _guiManager.SetCurrentGui("loadscreen_p");
                if (!set)
                {
                    _guiManager.SetCurrentGui("loadscreen");
                }

                // Update the loading screen image
                UpdateLoadingScreenImage(_currentImageResRef);

                // Set initial progress to 0
                SetProgress(0);
            }
            else
            {
                System.Console.WriteLine("[OdysseyLoadingScreen] WARNING: Failed to load loadscreen_p GUI, loading screen may not display correctly");
            }
        }

        /// <summary>
        /// Hides the loading screen.
        /// Based on swkotor2.exe: Loading screen is hidden after module load completes
        /// - Hides "loadscreen_p" GUI panel
        /// - Clears loading screen state
        /// - Original implementation: Called after module transition completes
        /// </summary>
        public void Hide()
        {
            if (!_isVisible)
            {
                return;
            }

            _isVisible = false;
            _currentImageResRef = null;
            _currentProgress = 0;

            // Unload or hide the loading screen GUI
            // Based on swkotor2.exe: Loading screen GUI is hidden/unloaded after module transition
            _guiManager.SetCurrentGui(null);
        }

        /// <summary>
        /// Updates the loading screen progress bar.
        /// Based on swkotor2.exe: Progress bar updates during resource loading
        /// - "PB_PROGRESS" control shows loading progress
        /// - "Load Bar = %d" @ 0x007c760c (progress debug output)
        /// - Original implementation: Progress bar updates as resources are loaded
        /// </summary>
        /// <param name="progress">Progress value (0-100).</param>
        public void SetProgress(int progress)
        {
            _currentProgress = Math.Max(0, Math.Min(100, progress));

            if (!_isVisible)
            {
                return;
            }

            // Update progress bar if GUI is loaded
            // Based on swkotor2.exe: "PB_PROGRESS" control is updated with progress value
            // The GUI manager should handle progress bar updates through control value changes
            // This would typically be done by setting a control value on the "PB_PROGRESS" control
            // For now, we rely on the GUI manager's internal progress tracking
            // TODO: If GUI manager exposes progress bar update method, call it here
        }

        /// <summary>
        /// Updates the loading screen image.
        /// Based on swkotor2.exe: Loading screen image is set via LoadScreenResRef
        /// - Image is loaded from TPC texture resource
        /// - Set as background or border fill on loading screen panel
        /// - Original implementation: FUN_006cff90 @ 0x006cff90 sets loading screen image
        /// </summary>
        /// <param name="imageResRef">Resource reference for the loading screen image (TPC format).</param>
        private void UpdateLoadingScreenImage(string imageResRef)
        {
            if (string.IsNullOrEmpty(imageResRef))
            {
                return;
            }

            // Update the loading screen image
            // Based on swkotor2.exe: Loading screen image is set on the GUI panel
            // The image would typically be set as a background texture or border fill
            // This would be handled by the GUI manager when loading the GUI
            // For now, the image loading is handled by the GUI system when the GUI is loaded
            // TODO: If GUI manager exposes image update method, call it here to update the loading screen image
        }
    }
}

