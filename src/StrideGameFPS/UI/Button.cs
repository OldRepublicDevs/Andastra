using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Graphics.Font;
using Stride.Input;
using System;

namespace StrideGameFPS.UI
{
    /// <summary>
    /// Simple button for UI. Equivalent to MonoGameFPS.UI.Button using Stride APIs.
    /// </summary>
    public class Button
    {
        private RectangleF _bounds;
        private SpriteFont _font;
        private string _text;
        private Color4 _normalColor;
        private Color4 _hoverColor;
        private Color4 _currentColor;
        private bool _wasPressed;

        public event EventHandler<EventArgs> Click;

        public string Text { get => _text; set => _text = value; }
        public RectangleF Bounds => _bounds;

        public Button(string text, SpriteFont font, RectangleF bounds)
        {
            _text = text;
            _font = font;
            _bounds = bounds;
            _normalColor = Color.White;
            _hoverColor = new Color4(200, 200, 200, 255);
            _currentColor = _normalColor;
        }

        public Button(string text, SpriteFont font, RectangleF bounds, Color4 normalColor, Color4 hoverColor)
        {
            _text = text;
            _font = font;
            _bounds = bounds;
            _normalColor = normalColor;
            _hoverColor = hoverColor;
            _currentColor = _normalColor;
        }

        public void Update(InputManager input, Vector2 mousePos, bool leftDown, bool leftWasDown)
        {
            bool isHovering = _bounds.Contains(mousePos);
            _currentColor = isHovering ? _hoverColor : _normalColor;
            if (isHovering && leftDown) _wasPressed = true;
            if (isHovering && _wasPressed && !leftDown && leftWasDown)
            {
                Click?.Invoke(this, EventArgs.Empty);
                _wasPressed = false;
            }
            if (!leftDown) _wasPressed = false;
        }

        public void Draw(SpriteBatch spriteBatch, Texture pixel)
        {
            spriteBatch.Draw(pixel, _bounds, _currentColor * 0.5f);
            int t = 2;
            spriteBatch.Draw(pixel, new RectangleF(_bounds.X, _bounds.Y, _bounds.Width, t), Color4.White);
            spriteBatch.Draw(pixel, new RectangleF(_bounds.X, _bounds.Bottom - t, _bounds.Width, t), Color4.White);
            spriteBatch.Draw(pixel, new RectangleF(_bounds.X, _bounds.Y, t, _bounds.Height), Color4.White);
            spriteBatch.Draw(pixel, new RectangleF(_bounds.Right - t, _bounds.Y, t, _bounds.Height), Color4.White);
            if (_font != null && !string.IsNullOrEmpty(_text))
            {
                var textSize = _font.MeasureString(_text);
                var textPos = new Vector2(_bounds.X + (_bounds.Width - textSize.X) / 2, _bounds.Y + (_bounds.Height - textSize.Y) / 2);
                spriteBatch.DrawString(_font, _text, textPos, Color4.White);
            }
        }
    }
}
