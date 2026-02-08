using System;
using global::Stride.Engine;
using global::Stride.Games;
using Stride.Graphics;

namespace Andastra.Game.Stride.Graphics
{
    /// <summary>
    /// Wrapper for Stride.Engine.Game that provides UpdateFrame and DrawFrame events.
    /// Stride's Game class uses Update() and Draw() methods, but we need event-based callbacks.
    /// </summary>
    /// <remarks>
    /// Stride Game Loop:
    /// - base.Draw() runs the graphics compositor / scene rendering pipeline.
    /// - After base.Draw(), we fire DrawFrame so that our OdysseyGame drawAction can
    ///   render 2D overlays (main menu, etc.) on top using SpriteBatch.
    /// - Before firing DrawFrame we explicitly set the CommandList render target to
    ///   the backbuffer so SpriteBatch output is visible. Without this the render target
    ///   may be left pointing at an intermediate compositor texture, causing SpriteBatch
    ///   draws to go to the wrong surface (blank window).
    ///
    /// Reference: KotOR.js MainMenu renders 2D over the GL backbuffer.
    ///            reone MainMenu renders 2D via OpenGL directly to the default framebuffer.
    ///            Original engine (k1_win_gog_swkotor.exe / k2_win_gog_aspyr_swkotor2.exe): IDirect3DDevice9::Present after 2D draws to backbuffer.
    /// </remarks>
    public class StrideGameWrapper : global::Stride.Engine.Game
    {
        /// <summary>
        /// Event raised before each update frame.
        /// </summary>
        public event EventHandler<FrameEventArgs> UpdateFrame;

        /// <summary>
        /// Event raised before each draw frame.
        /// </summary>
        public event EventHandler<FrameEventArgs> DrawFrame;

        /// <summary>
        /// Event raised when the game is initialized.
        /// </summary>
        public event EventHandler Initialized;

        /// <summary>
        /// Frame event arguments containing elapsed time.
        /// </summary>
        public class FrameEventArgs : EventArgs
        {
            public TimeSpan Elapsed { get; set; }
        }

        protected override void Initialize()
        {
            base.Initialize();
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateFrame?.Invoke(this, new FrameEventArgs { Elapsed = gameTime.Elapsed });
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            // Ensure the CommandList render target is set to the backbuffer before
            // our custom DrawFrame code runs. After base.Draw() the compositor may have
            // left the render target pointing at an intermediate texture. SpriteBatch
            // will draw to whatever render target is currently set, so we must
            // redirect it to the presenter's backbuffer.
            // Reference: Stride docs - Low-level API, SpriteBatch must render to a
            //   correctly-bound render target via CommandList.
            // Original engine: k1_win_gog_swkotor.exe/k2_win_gog_aspyr_swkotor2.exe present after 2D draws to backbuffer.
            try
            {
                if (GraphicsContext != null && GraphicsDevice?.Presenter != null)
                {
                    var commandList = GraphicsContext.CommandList;
                    var backBuffer = GraphicsDevice.Presenter.BackBuffer;
                    var depthBuffer = GraphicsDevice.Presenter.DepthStencilBuffer;
                    if (commandList != null && backBuffer != null)
                    {
                        commandList.SetRenderTargetAndViewport(depthBuffer, backBuffer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[StrideGameWrapper] WARNING: Could not set backbuffer render target: {ex.Message}");
            }

            DrawFrame?.Invoke(this, new FrameEventArgs { Elapsed = gameTime.Elapsed });
        }
    }
}

