using Stride.Core.Mathematics;
using Stride.Games;
using Stride.Graphics;
using Stride.Graphics.Font;
using Stride.Input;
using StrideGameFPS.Scenes;
using StrideGameFPS.UI;
using System.Collections.Generic;

namespace StrideGameFPS
{
    /// <summary>
    /// Options menu scene. Equivalent to MonoGameFPS.OptionsScene.
    /// </summary>
    public class OptionsScene : Scene
    {
        private SpriteFont _font;
        private Texture _pixel;
        private List<Button> _buttons;
        private Button _backButton;
        private bool _lastLeftDown;
        private readonly SceneManager _sceneManager;
        private static readonly string[] _settingsText = { "Graphics Settings", "Audio Settings", "Controls", "", "Settings will be saved automatically" };

        public OptionsScene(SceneManager sceneManager)
        {
            _sceneManager = sceneManager;
        }

        public override void LoadContent()
        {
            _pixel = Texture.New2D(GraphicsDevice, 1, 1, PixelFormat.R8G8B8A8_UNorm, new[] { new Color4(255, 255, 255, 255) });
            try { _font = Content.Load<SpriteFont>("Fonts/MenuFont"); } catch { }
            int screenWidth = GraphicsDevice.Presenter?.Description.BackBufferWidth ?? 1920;
            int screenHeight = GraphicsDevice.Presenter?.Description.BackBufferHeight ?? 1080;
            int buttonWidth = 200, buttonHeight = 50;
            _buttons = new List<Button>();
            _backButton = new Button("Back", _font, new RectangleF((screenWidth - buttonWidth) / 2, screenHeight - 100, buttonWidth, buttonHeight), new Color4(80, 80, 80, 255), new Color4(120, 120, 120, 255));
            _backButton.Click += (s, e) => _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
            _buttons.Add(_backButton);
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            var mousePos = input.MousePosition;
            bool leftDown = input.IsMouseButtonDown(MouseButton.Left);
            foreach (var btn in _buttons)
                btn.Update(input, mousePos, leftDown, _lastLeftDown);
            if (input.IsKeyDown(Keys.Escape))
                _sceneManager.ChangeScene(new MainMenuScene(_sceneManager));
            _lastLeftDown = leftDown;
        }

        public override void Draw(GameTime gameTime)
        {
            var cmd = GraphicsContext.CommandList;
            var backBuffer = GraphicsDevice.Presenter?.BackBuffer;
            if (backBuffer != null) cmd.Clear(backBuffer, new Color4(0.16f, 0.14f, 0.2f, 1f));
            SpriteBatch.Begin(GraphicsContext);
            string title = "OPTIONS";
            if (_font != null)
            {
                var titleSize = _font.MeasureString(title);
                int vw = GraphicsDevice.Presenter?.Description.BackBufferWidth ?? 1920;
                var titlePos = new Vector2((vw - titleSize.X) / 2, 100);
                SpriteBatch.DrawString(_font, title, titlePos + new Vector2(2, 2), new Color4(0, 0, 0, 0.5f));
                SpriteBatch.DrawString(_font, title, titlePos, Color4.White);
                float y = 250;
                foreach (var text in _settingsText)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        var textSize = _font.MeasureString(text);
                        int vw2 = GraphicsDevice.Presenter?.Description.BackBufferWidth ?? 1920;
                        SpriteBatch.DrawString(_font, text, new Vector2((vw2 - textSize.X) / 2, y), new Color4(0.83f, 0.83f, 0.83f, 1f));
                    }
                    y += 40;
                }
            }
            foreach (var btn in _buttons)
                btn.Draw(SpriteBatch, _pixel);
            SpriteBatch.End();
        }

        public override void Dispose()
        {
            _pixel?.Dispose();
            base.Dispose();
        }
    }
}
