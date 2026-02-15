using Stride.Core.Mathematics;
using Stride.Games;
using Stride.Graphics;
using Stride.Input;
using StrideGameFPS.Scenes;
using System;

namespace StrideGameFPS
{
    /// <summary>
    /// In-game 3D scene. Equivalent to MonoGameFPS.GameScene.
    /// </summary>
    public class GameScene : Scene
    {
        private FPSCamera _camera;
        private TerrainGenerator _terrain;
        private TerrainRenderer _terrainRenderer;
        private PlayerController _playerController;
        private ProceduralTextureGenerator _textureGenerator;
        private Texture _grassTexture;
        private Texture _crosshairPixel;
        private bool _isMouseLocked = true;
        private bool _lastEscape;
        private bool _lastTab;
        private bool _lastP;
        private bool _isPaused;
        private readonly SceneManager _sceneManager;

        public GameScene(SceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        public override void Initialize()
        {
            _camera = new FPSCamera(GraphicsDevice);
            _camera.Position = new Vector3(0, 50, 0);
            _camera.LookAtDirection = Vector3.UnitZ;
            _terrain = new TerrainGenerator(1337);
            _textureGenerator = new ProceduralTextureGenerator(1337);
        }

        public override void LoadContent()
        {
            _crosshairPixel = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm, new[] { new Color4(255, 255, 255, 255) });
            _grassTexture = _textureGenerator.GenerateGrassTexture(GraphicsDevice, 256);
            _terrainRenderer = new TerrainRenderer(GraphicsDevice, _terrain);
            _playerController = new PlayerController(_camera, _terrain, GraphicsDevice);
            _terrain.GenerateTerrainAroundPosition(Vector3.Zero, 5);
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            if (input.IsKeyDown(Keys.P) && !_lastP)
                _isPaused = !_isPaused;
            _lastP = input.IsKeyDown(Keys.P);
            if (_isPaused) return;
            if (input.IsKeyDown(Keys.Escape) && !_lastEscape)
            {
                _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
                return;
            }
            if (input.IsKeyDown(Keys.Tab) && !_lastTab)
                _isMouseLocked = !_isMouseLocked;
            _playerController.Update(gameTime, input, _isMouseLocked);
            _camera.UpdateViewMatrix();
            _terrain.GenerateTerrainAroundPosition(_camera.Position, 3);
            _lastEscape = input.IsKeyDown(Keys.Escape);
            _lastTab = input.IsKeyDown(Keys.Tab);
        }

        public override void Draw(GameTime gameTime)
        {
            var cmd = GraphicsContext.CommandList;
            var backBuffer = GraphicsDevice.Presenter?.BackBuffer;
            if (backBuffer != null) cmd.Clear(backBuffer, new Color4(0.5f, 0.6f, 0.7f, 1f));
            _terrainRenderer.Draw(_camera);
            int centerX = (GraphicsDevice.Presenter?.Description.BackBufferWidth ?? 1920) / 2;
            int centerY = (GraphicsDevice.Presenter?.Description.BackBufferHeight ?? 1080) / 2;
            int crosshairSize = 10, thickness = 2;
            SpriteBatch.Begin(GraphicsContext);
            SpriteBatch.Draw(_crosshairPixel, new RectangleF(centerX - crosshairSize, centerY - thickness / 2, crosshairSize * 2, thickness), new Color4(1, 1, 1, 0.8f));
            SpriteBatch.Draw(_crosshairPixel, new RectangleF(centerX - thickness / 2, centerY - crosshairSize, thickness, crosshairSize * 2), new Color4(1, 1, 1, 0.8f));
            SpriteBatch.End();
        }

        public override void Dispose()
        {
            _terrainRenderer?.Dispose();
            _grassTexture?.Dispose();
            _crosshairPixel?.Dispose();
            base.Dispose();
        }
    }
}
