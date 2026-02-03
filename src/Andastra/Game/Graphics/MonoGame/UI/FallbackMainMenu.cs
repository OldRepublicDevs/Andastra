using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andastra.Game.Graphics.MonoGame.UI
{
    /// <summary>
    /// Fallback main menu when KOTOR GUI (MAINMENU) cannot be loaded.
    /// Matches MonoGameFPS MainMenuScene pattern: draws buttons and handles input.
    /// Uses GraphicsDevice; optional SpriteFont for button labels when provided.
    /// </summary>
    /// <remarks>
    /// KOTOR 1/2 main menu fallback:
    /// - Buttons match original tags: BTN_NEWGAME, BTN_LOADGAME, BTN_OPTIONS, BTN_EXIT
    /// - Same behavior as KotorGuiManager button handlers for 1:1 compatibility
    /// </remarks>
    public class FallbackMainMenu
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly int _width;
        private readonly int _height;
        private readonly SpriteFont _font;
        private MouseState _previousMouse;
        private int _hoveredIndex = -1;

        private static readonly (string Tag, string Label)[] Buttons =
        {
            ("BTN_NEWGAME", "New Game"),
            ("BTN_LOADGAME", "Load Game"),
            ("BTN_WARP", "Warp (Load)"),
            ("BTN_OPTIONS", "Options"),
            ("BTN_MOVIES", "Movies"),
            ("BTN_EXIT", "Exit")
        };

        /// <summary>Fired when a button is clicked; argument is the button tag (e.g. BTN_NEWGAME).</summary>
        public event Action<string> OnButtonClicked;

        /// <summary>
        /// Creates the fallback main menu. If width or height is 0, layout uses device viewport in Draw.
        /// </summary>
        /// <param name="device">MonoGame GraphicsDevice.</param>
        /// <param name="width">Screen width (or 0 to use viewport).</param>
        /// <param name="height">Screen height (or 0 to use viewport).</param>
        /// <param name="font">Optional font for button labels; if null, only colored rectangles are drawn.</param>
        public FallbackMainMenu(GraphicsDevice device, int width, int height, SpriteFont font = null)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _width = width > 0 ? width : device.Viewport.Width;
            _height = height > 0 ? height : device.Viewport.Height;
            _font = font;
            _spriteBatch = new SpriteBatch(device);
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            _previousMouse = Mouse.GetState();
        }

        public void Update(float deltaTime)
        {
            MouseState mouse = Mouse.GetState();
            int x = mouse.X, y = mouse.Y;
            _hoveredIndex = -1;
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (GetButtonBounds(i).Contains(x, y))
                {
                    _hoveredIndex = i;
                    break;
                }
            }

            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < Buttons.Length; i++)
                {
                    if (GetButtonBounds(i).Contains(mouse.X, mouse.Y))
                    {
                        OnButtonClicked?.Invoke(Buttons[i].Tag);
                        break;
                    }
                }
            }

            _previousMouse = mouse;
        }

        public void Draw()
        {
            int w = _width > 0 ? _width : _device.Viewport.Width;
            int h = _height > 0 ? _height : _device.Viewport.Height;
            if (w <= 0) w = 800;
            if (h <= 0) h = 600;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Background (KOTOR-style dark blue)
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, w, h),
                new Microsoft.Xna.Framework.Color(20, 30, 60, 255));

            for (int i = 0; i < Buttons.Length; i++)
            {
                var bounds = GetButtonBounds(i, w, h);
                bool hover = i == _hoveredIndex;
                var fillColor = hover
                    ? new Microsoft.Xna.Framework.Color(80, 100, 140, 255)
                    : new Microsoft.Xna.Framework.Color(50, 70, 110, 255);
                var borderColor = new Microsoft.Xna.Framework.Color(120, 140, 180, 255);

                _spriteBatch.Draw(_pixel, bounds, fillColor);
                DrawRectBorder(bounds, borderColor);

                if (_font != null && !string.IsNullOrEmpty(Buttons[i].Label))
                {
                    var textSize = _font.MeasureString(Buttons[i].Label);
                    var textPos = new Vector2(
                        bounds.X + (bounds.Width - textSize.X) / 2f,
                        bounds.Y + (bounds.Height - textSize.Y) / 2f);
                    _spriteBatch.DrawString(_font, Buttons[i].Label, textPos, Microsoft.Xna.Framework.Color.White);
                }
            }

            _spriteBatch.End();
        }

        private Rectangle GetButtonBounds(int index)
        {
            int w = _width > 0 ? _width : _device.Viewport.Width;
            int h = _height > 0 ? _height : _device.Viewport.Height;
            if (w <= 0) w = 800;
            if (h <= 0) h = 600;
            return GetButtonBounds(index, w, h);
        }

        private static Rectangle GetButtonBounds(int index, int screenWidth, int screenHeight)
        {
            int bw = 280;
            int bh = 44;
            int centerX = screenWidth / 2;
            int startY = screenHeight / 3;
            int spacing = 56;
            int x = centerX - bw / 2;
            int y = startY + index * spacing;
            return new Rectangle(x, y, bw, bh);
        }

        private void DrawRectBorder(Rectangle r, Microsoft.Xna.Framework.Color color)
        {
            int t = 2;
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, r.Width, t), color);
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Bottom - t, r.Width, t), color);
            _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, t, r.Height), color);
            _spriteBatch.Draw(_pixel, new Rectangle(r.Right - t, r.Y, t, r.Height), color);
        }

        public void Dispose()
        {
            _pixel?.Dispose();
        }
    }
}
