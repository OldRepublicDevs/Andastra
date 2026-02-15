using Stride.Core.Mathematics;
using Stride.Games;
using Stride.Graphics;
using Stride.Graphics.Font;
using Stride.Input;
using StrideGameFPS.Scenes;
using StrideGameFPS.UI;
using System;
using System.Collections.Generic;

namespace StrideGameFPS
{
    /// <summary>
    /// Main menu scene. Equivalent to MonoGameFPS.MainMenuScene.
    /// </summary>
    public class MainMenuScene : Scene
    {
        private SpriteFont _titleFont;
        private SpriteFont _buttonFont;
        private Texture _pixel;
        private List<Button> _buttons;
        private Button _newGameButton, _optionsButton, _exitButton;
        private Vector2 _lastMousePos;
        private bool _lastLeftDown;
        private bool _lastEscape;
        private string _gameTitle = "Stride 3D FPS";
        private string _subtitle = "Procedural Open World";
        private Vector2 _titlePosition, _subtitlePosition;
        private readonly SceneManager _sceneManager;

        public MainMenuScene(SceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        public override void LoadContent()
        {
            _pixel = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm, new[] { new Color4(255, 255, 255, 255) });
            try
            {
                _titleFont = Content.Load<SpriteFont>("Fonts/TitleFont");
                _buttonFont = Content.Load<SpriteFont>("Fonts/MenuFont");
            }
            catch { /* use fallback */ }

            int screenWidth = GraphicsDevice.Presenter?.Description.BackBufferWidth ?? 1920;
            int screenHeight = GraphicsDevice.Presenter?.Description.BackBufferHeight ?? 1080;
            int buttonWidth = 300, buttonHeight = 60;
            int buttonX = (screenWidth - buttonWidth) / 2;
            int buttonSpacing = 80;
            int startY = screenHeight / 2;

            _buttons = new List<Button>();
            _newGameButton = new Button("New Game", _buttonFont, new RectangleF(buttonX, startY, buttonWidth, buttonHeight), new Color4(50, 120, 40, 255), new Color4(70, 160, 60, 255));
            _newGameButton.Click += (s, e) => _sceneManager.ChangeScene(new GameScene(_sceneManager));
            _buttons.Add(_newGameButton);

            _optionsButton = new Button("Options", _buttonFont, new RectangleF(buttonX, startY + buttonSpacing, buttonWidth, buttonHeight), new Color4(50, 90, 150, 255), new Color4(70, 120, 190, 255));
            _optionsButton.Click += (s, e) => _sceneManager.ChangeScene(new OptionsScene(_sceneManager));
            _buttons.Add(_optionsButton);

            _exitButton = new Button("Exit", _buttonFont, new RectangleF(buttonX, startY + buttonSpacing * 2, buttonWidth, buttonHeight), new Color4(120, 40, 40, 255), new Color4(160, 60, 60, 255));
            _exitButton.Click += (s, e) => Environment.Exit(0);
            _buttons.Add(_exitButton);

            int titleY = screenHeight / 4;
            float titleW = _titleFont != null ? _titleFont.MeasureString(_gameTitle).X : 400;
            float subW = _buttonFont != null ? _buttonFont.MeasureString(_subtitle).X : 300;
            _titlePosition = new Vector2((screenWidth - titleW) / 2, titleY);
            _subtitlePosition = new Vector2((screenWidth - subW) / 2, titleY + 50);
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            var mousePos = input.MousePosition;
            bool leftDown = input.IsMouseButtonDown(MouseButton.Left);
            foreach (var btn in _buttons)
                btn.Update(input, mousePos, leftDown, _lastLeftDown);
            if (input.IsKeyDown(Keys.Escape) && !_lastEscape)
                Environment.Exit(0);
            _lastMousePos = mousePos;
            _lastLeftDown = leftDown;
            _lastEscape = input.IsKeyDown(Keys.Escape);
        }

        public override void Draw(GameTime gameTime)
        {
            var cmd = GraphicsContext.CommandList;
            var backBuffer = GraphicsDevice.Presenter?.BackBuffer;
            if (backBuffer != null) cmd.Clear(backBuffer, new Color4(0.12f, 0.16f, 0.2f, 1f));
            SpriteBatch.Begin(GraphicsContext);
            if (_titleFont != null)
            {
                SpriteBatch.DrawString(_titleFont, _gameTitle, _titlePosition + new Vector2(2, 2), new Color4(0, 0, 0, 0.5f));
                SpriteBatch.DrawString(_titleFont, _gameTitle, _titlePosition, Color4.White);
                SpriteBatch.DrawString(_buttonFont ?? _titleFont, _subtitle, _subtitlePosition + new Vector2(2, 2), new Color4(0, 0, 0, 0.5f));
                SpriteBatch.DrawString(_buttonFont ?? _titleFont, _subtitle, _subtitlePosition, new Color4(0.78f, 0.78f, 0.78f, 1f));
            }
            else
            {
                SpriteBatch.Draw(_pixel, new RectangleF(_titlePosition.X, _titlePosition.Y, 400, 40), new Color4(1, 1, 1, 0.5f));
                SpriteBatch.Draw(_pixel, new RectangleF(_subtitlePosition.X, _subtitlePosition.Y, 300, 25), new Color4(0.5f, 0.5f, 0.5f, 0.5f));
            }
            foreach (var btn in _buttons)
                btn.Draw(SpriteBatch, _pixel);
            if (_buttonFont != null)
                SpriteBatch.DrawString(_buttonFont, "Alpha v0.1", new Vector2(10, (GraphicsDevice.Presenter?.Description.BackBufferHeight ?? 1080) - 30), new Color4(0.5f, 0.5f, 0.5f, 1f));
            SpriteBatch.End();
        }

        public override void Dispose()
        {
            _pixel?.Dispose();
            base.Dispose();
        }
    }
}
