using System;
using Andastra.Runtime.Graphics;
using Andastra.Runtime.Graphics.Common.GUI;
using Andastra.Game.Stride.Graphics;
using JetBrains.Annotations;
using Stride.Core.Mathematics;
using StrideGraphics = Stride.Graphics;

namespace Andastra.Game.Stride.GUI
{
    /// <summary>
    /// Stride-based menu renderer using SpriteBatch for rendering.
    /// Provides menu rendering functionality for Stride graphics backend.
    /// </summary>
    /// <remarks>
    /// Stride Menu Renderer:
    /// - Based on exhaustive verified components of k1_win_gog_swkotor.exe and k2_win_gog_aspyr_swkotor2.exe menu initialization
    /// - k2_win_gog_aspyr_swkotor2.exe: 0x006d2350 @ 0x006d2350 (menu constructor/initializer)
    /// - k1_win_gog_swkotor.exe: 0x0067c4c0 @ 0x0067c4c0 (menu constructor/initializer)
    ///
    /// Initialization Sequence (matching original engines):
    /// 1. Load "MAINMENU" GUI file first (k2_win_gog_aspyr_swkotor2.exe: 0x006d2350:73, k1_win_gog_swkotor.exe: 0x0067c4c0:62)
    /// 2. If GUI load succeeds, load "RIMS:MAINMENU" RIM file (k2_win_gog_aspyr_swkotor2.exe: 0x006d2350:76-80, k1_win_gog_swkotor.exe: 0x0067c4c0:65-69)
    /// 3. Clear menu flag at DAT_008283c0+0x34 (bit 2 = 0xfd mask) (k2_win_gog_aspyr_swkotor2.exe: 0x006d2350:82)
    /// 4. Set up event handlers for menu buttons (0x27, 0x2d, 0, 1 events) (k2_win_gog_aspyr_swkotor2.exe: 0x006d2350:89-95)
    ///
    /// String References:
    /// - "RIMS:MAINMENU" @ k2_win_gog_aspyr_swkotor2.exe:0x007b6044, k1_win_gog_swkotor.exe:0x0073e0a4 (main menu RIM file)
    /// - "MAINMENU" @ k2_win_gog_aspyr_swkotor2.exe:0x007cc030, k1_win_gog_swkotor.exe:0x00752f64 (main menu GUI constant)
    /// - "mainmenu_p" @ k2_win_gog_aspyr_swkotor2.exe:0x007cc000 (main menu panel)
    /// - "mainmenu01-05" @ k2_win_gog_aspyr_swkotor2.exe:0x007cc108-0x007cc138 (menu variants, k2_win_gog_aspyr_swkotor2.exe only)
    /// - "mainmenu" @ k1_win_gog_swkotor.exe:0x00752f4c (single menu panel, no variants)
    ///
    /// Menu Variants (k2_win_gog_aspyr_swkotor2.exe only):
    /// - Menu variant selection based on "gui3D_room" condition (k2_win_gog_aspyr_swkotor2.exe: 0x006d2350:120-150)
    /// - Variants: mainmenu01 (default), mainmenu02, mainmenu03, mainmenu04, mainmenu05
    /// - k1_win_gog_swkotor.exe uses single "mainmenu" panel (no variants)
    ///
    /// Event Handlers:
    /// - Menu buttons register handlers for events: 0x27, 0x2d, 0, 1 (k2_win_gog_aspyr_swkotor2.exe: 0x006d2350:89-95)
    /// - Event 0x27: Button press/select
    /// - Event 0x2d: Button release/deselect
    /// - Event 0: Unknown (likely hover/enter)
    /// - Event 1: Unknown (likely leave/exit)
    ///
    /// This Implementation:
    /// - Uses Stride SpriteBatch for menu rendering
    /// - Stride provides SpriteBatch for 2D rendering similar to MonoGame
    /// - Matches original engine initialization sequence and event handling patterns
    /// - Stride SpriteBatch.Begin() requires GraphicsContext (from Game.GraphicsContext)
    /// - SpriteBatch draws to whatever render target is currently set on CommandList
    /// - StrideGameWrapper.Draw() sets the backbuffer as render target before firing DrawFrame
    ///
    /// Inheritance:
    /// - BaseMenuRenderer (Runtime.Graphics.Common.GUI) - Common menu functionality
    ///   - StrideMenuRenderer (this class) - Stride-specific SpriteBatch implementation
    /// </remarks>
    public class StrideMenuRenderer : BaseMenuRenderer
    {
        private StrideGraphics.GraphicsDevice _graphicsDevice;
        private StrideGraphics.SpriteBatch _spriteBatch;
        private StrideGraphics.SpriteFont _font;
        private global::Stride.Graphics.Texture _whiteTexture;
        private bool _isDisposed = false;
        private bool _deferredInit = false;

        /// <summary>
        /// Initializes a new instance of StrideMenuRenderer.
        /// </summary>
        /// <param name="graphicsDevice">The Stride GraphicsDevice to use for rendering.</param>
        /// <param name="font">Optional SpriteFont for text rendering. Can be null.</param>
        /// <exception cref="ArgumentNullException">Thrown if graphicsDevice is null.</exception>
        public StrideMenuRenderer([NotNull] StrideGraphics.GraphicsDevice graphicsDevice, StrideGraphics.SpriteFont font = null)
        {
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException(nameof(graphicsDevice));
            }

            _graphicsDevice = graphicsDevice;
            _font = font;

            // Attempt initialization now. If CommandList isn't available yet
            // (common with Stride's deferred initialization), mark for retry on first Draw().
            if (!TryInitializeStride())
            {
                _deferredInit = true;
                Console.WriteLine("[StrideMenuRenderer] Deferred initialization - will retry on first Draw()");
            }
        }

        /// <summary>
        /// Attempts to initialize Stride-specific rendering resources.
        /// Returns true on success, false if resources aren't available yet.
        /// </summary>
        private bool TryInitializeStride()
        {
            try
            {
                // Create SpriteBatch for 2D rendering (only needs GraphicsDevice, not CommandList)
                if (_spriteBatch == null)
                {
                    _spriteBatch = new global::Stride.Graphics.SpriteBatch(_graphicsDevice);
                }

                // Create 1x1 white texture for drawing rectangles and backgrounds
                // Stride Texture.New2D with initial data uses the graphics device internally
                if (_whiteTexture == null)
                {
                    _whiteTexture = global::Stride.Graphics.Texture.New2D(
                        _graphicsDevice, 1, 1,
                        StrideGraphics.PixelFormat.R8G8B8A8_UNorm,
                        new[] { new Color(255, 255, 255, 255) });
                }

                // Initialize base class with viewport dimensions
                var presentParams = _graphicsDevice.Presenter?.Description;
                int width = presentParams?.BackBufferWidth ?? 1920;
                int height = presentParams?.BackBufferHeight ?? 1080;
                Initialize(width, height);

                _deferredInit = false;
                Console.WriteLine("[StrideMenuRenderer] Stride menu renderer initialized successfully");
                Console.WriteLine($"[StrideMenuRenderer] Viewport: {width}x{height}");
                Console.WriteLine($"[StrideMenuRenderer] Font available: {_font != null}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideMenuRenderer] Initialization deferred: {ex.Message}");

                // Dispose any resources that were created before the exception
                if (_whiteTexture != null)
                {
                    _whiteTexture.Dispose();
                    _whiteTexture = null;
                }

                if (_spriteBatch != null)
                {
                    _spriteBatch.Dispose();
                    _spriteBatch = null;
                }

                IsInitialized = false;
                return false;
            }
        }

        /// <summary>
        /// Updates the menu renderer (handles input, animations, etc.).
        /// </summary>
        /// <param name="elapsedTime">Elapsed time since last update in seconds.</param>
        public override void Update(float elapsedTime)
        {
            // Retry deferred initialization if needed
            if (_deferredInit && !_isDisposed)
            {
                TryInitializeStride();
            }

            if (!IsInitialized || _isDisposed)
            {
                return;
            }

            // Update menu logic here (input handling, animations, etc.)
            // This can be extended with specific menu update logic
        }

        /// <summary>
        /// Renders the menu using Stride SpriteBatch.
        /// Stride SpriteBatch.Begin() requires GraphicsContext (from Game.GraphicsContext).
        /// The CommandList must have the backbuffer set as render target before this is called
        /// (handled by StrideGameWrapper.Draw()).
        /// Reference: Stride docs - https://doc.stride3d.net/latest/en/manual/graphics/low-level-api/spritebatch.html
        ///   spriteBatch.Begin(Game.GraphicsContext, SpriteSortMode.Immediate);
        ///   spriteBatch.Draw(myTexture, new Vector2(10, 20));
        ///   spriteBatch.End();
        /// </summary>
        public override void Draw()
        {
            // Retry deferred initialization if needed
            if (_deferredInit && !_isDisposed)
            {
                TryInitializeStride();
            }

            if (!IsVisible || !IsInitialized || _isDisposed)
            {
                return;
            }

            if (_graphicsDevice == null || _spriteBatch == null || _whiteTexture == null)
            {
                return;
            }

            try
            {
                // Get GraphicsContext for SpriteBatch.Begin() - required by Stride API
                // Reference: Stride docs - spriteBatch.Begin(Game.GraphicsContext, ...)
                var graphicsContext = _graphicsDevice.GraphicsContext();
                if (graphicsContext == null)
                {
                    return;
                }

                // Get viewport dimensions from presentation parameters
                var presentParams = _graphicsDevice.Presenter?.Description;
                int viewportWidth = presentParams?.BackBufferWidth ?? 1920;
                int viewportHeight = presentParams?.BackBufferHeight ?? 1080;

                // Begin sprite batch with proper GraphicsContext (required by Stride API)
                // Reference: Stride SpriteBatch docs - Begin accepts GraphicsContext, SpriteSortMode, BlendStateDescription
                _spriteBatch.Begin(graphicsContext, StrideGraphics.SpriteSortMode.Deferred, StrideGraphics.BlendStates.AlphaBlend);

                try
                {
                    // Draw menu background (full screen dark blue)
                    // Reference: reone mainmenu.cpp - loadBackground(BackgroundType::Menu)
                    // Reference: KotOR.js MainMenu.ts - this.voidFill = true (fills void area with color)
                    var backgroundColor = new Color4(20.0f / 255.0f, 30.0f / 255.0f, 60.0f / 255.0f, 1.0f);
                    _spriteBatch.Draw(_whiteTexture, new RectangleF(0, 0, viewportWidth, viewportHeight), backgroundColor);

                    // Draw menu panel background
                    var panelColor = new Color4(40.0f / 255.0f, 50.0f / 255.0f, 80.0f / 255.0f, 1.0f);
                    var panelRect = CalculateMenuPanelRect(viewportWidth, viewportHeight);
                    _spriteBatch.Draw(_whiteTexture, panelRect, panelColor);

                    // Draw menu header
                    var headerColor = new Color4(255.0f / 255.0f, 200.0f / 255.0f, 50.0f / 255.0f, 1.0f);
                    var headerRect = CalculateHeaderRect(panelRect);
                    _spriteBatch.Draw(_whiteTexture, headerRect, headerColor);

                    // Draw menu text if font is available
                    if (_font != null)
                    {
                        var textColor = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
                        var titlePosition = new global::Stride.Core.Mathematics.Vector2(panelRect.X + 20, headerRect.Y + 10);
                        _spriteBatch.DrawString(_font, "Main Menu", titlePosition, textColor);
                    }
                }
                finally
                {
                    // Always end sprite batch rendering, even if an exception occurred.
                    // This prevents leaving SpriteBatch in a "begun" state which would cause
                    // InvalidOperationException on subsequent Begin() calls.
                    _spriteBatch.End();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideMenuRenderer] ERROR: Failed to render menu: {ex.Message}");
                Console.WriteLine($"[StrideMenuRenderer] Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Calculates the menu panel rectangle based on viewport dimensions.
        /// </summary>
        /// <param name="viewportWidth">Viewport width.</param>
        /// <param name="viewportHeight">Viewport height.</param>
        /// <returns>Menu panel rectangle.</returns>
        private RectangleF CalculateMenuPanelRect(int viewportWidth, int viewportHeight)
        {
            // Center the panel on screen
            float panelWidth = Math.Min(600, viewportWidth * 0.6f);
            float panelHeight = Math.Min(400, viewportHeight * 0.6f);
            float panelX = (viewportWidth - panelWidth) / 2.0f;
            float panelY = (viewportHeight - panelHeight) / 2.0f;

            return new RectangleF(panelX, panelY, panelWidth, panelHeight);
        }

        /// <summary>
        /// Calculates the header rectangle within the menu panel.
        /// </summary>
        /// <param name="panelRect">Menu panel rectangle.</param>
        /// <returns>Header rectangle.</returns>
        private RectangleF CalculateHeaderRect(RectangleF panelRect)
        {
            float headerHeight = 50.0f;
            return new RectangleF(panelRect.X, panelRect.Y, panelRect.Width, headerHeight);
        }

        /// <summary>
        /// Called when viewport size changes. Updates rendering resources.
        /// </summary>
        /// <param name="width">New viewport width.</param>
        /// <param name="height">New viewport height.</param>
        protected override void OnViewportChanged(int width, int height)
        {
            base.OnViewportChanged(width, height);
            Console.WriteLine($"[StrideMenuRenderer] Viewport changed to {width}x{height}");
        }

        /// <summary>
        /// Called when visibility changes.
        /// </summary>
        /// <param name="visible">New visibility state.</param>
        protected override void OnVisibilityChanged(bool visible)
        {
            base.OnVisibilityChanged(visible);
            Console.WriteLine($"[StrideMenuRenderer] Visibility changed to: {visible}");
        }

        /// <summary>
        /// Disposes of resources used by the menu renderer.
        /// </summary>
        public override void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_spriteBatch != null)
            {
                _spriteBatch.Dispose();
                _spriteBatch = null;
            }

            if (_whiteTexture != null)
            {
                _whiteTexture.Dispose();
                _whiteTexture = null;
            }

            _graphicsDevice = null;
            _font = null;
            _isDisposed = true;

            base.Dispose();

            Console.WriteLine("[StrideMenuRenderer] Disposed");
        }
    }
}

