using Stride.Engine;
using Stride.Graphics;
using Stride.Games;
using Stride.Core.Mathematics;
using StrideGameFPS.Scenes;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace StrideGameFPS
{
    /// <summary>
    /// Main game class for the 3D FPS open world game.
    /// Now integrated with scene management system for proper menu support.
    /// Equivalent to MonoGameFPS.FPSGame using Stride APIs.
    /// </summary>
    public class FPSGame : Game
    {
        private SpriteBatch _spriteBatch;
        private SceneManager _sceneManager;

        public FPSGame()
        {
            IsMouseVisible = true;

            var graphicsDeviceManager = GraphicsDeviceManager;
            if (graphicsDeviceManager != null)
            {
                graphicsDeviceManager.PreferredBackBufferWidth = 1920;
                graphicsDeviceManager.PreferredBackBufferHeight = 1080;
                graphicsDeviceManager.PreferredGraphicsProfile = new[] { GraphicsProfile.Level_11_0 };
            }

            Window.Title = "Stride 3D FPS - Procedural Open World";
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
            // Apply borderless fullscreen main menu window (match MonoGameFPS behavior)
            if (Window != null)
            {
                try
                {
                    var clientSizeProp = Window.GetType().GetProperty("ClientSize");
                    if (clientSizeProp != null && clientSizeProp.CanWrite)
                        clientSizeProp.SetValue(Window, new Int2(1920, 1080));
                }
                catch { }
                Window.Title = "Stride 3D FPS - Procedural Open World";
                var borderlessProp = Window.GetType().GetProperty("FullscreenIsBorderlessWindow");
                if (borderlessProp != null && borderlessProp.CanWrite)
                    borderlessProp.SetValue(Window, true);
                Window.IsFullscreen = true;
                Window.IsMouseVisible = true;
            }
        }

        protected override Task LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sceneManager = new SceneManager(GraphicsDevice, Content, _spriteBatch, GraphicsContext, Input);
            _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
            return Task.CompletedTask;
        }

        protected override void Update(GameTime gameTime)
        {
            _sceneManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Ensure we draw to the backbuffer so the main menu and scenes are visible (Stride compositor may leave RT on a texture)
            try
            {
                if (GraphicsContext?.CommandList != null && GraphicsDevice?.Presenter != null)
                {
                    var backBuffer = GraphicsDevice.Presenter.BackBuffer;
                    var depthBuffer = GraphicsDevice.Presenter.DepthStencilBuffer;
                    if (backBuffer != null)
                        GraphicsContext.CommandList.SetRenderTargetAndViewport(depthBuffer, backBuffer);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[FPSGame] SetRenderTarget: {ex.Message}");
            }
            _sceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void Destroy()
        {
            if (_sceneManager != null)
            {
                // Scene manager will dispose of current scene
            }
            base.Destroy();
        }
    }
}
