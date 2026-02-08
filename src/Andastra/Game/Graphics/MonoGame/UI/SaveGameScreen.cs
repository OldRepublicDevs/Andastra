using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andastra.Game.Graphics.MonoGame.UI
{
    /// <summary>
    /// Save game screen - 1:1 with Reva CSWGuiSaveLoad (save mode).
    /// k1_win_gog_swkotor.exe BTN_SAVEGAME @ 0x007d0dbc: CSWGuiSaveLoad(manager, 1, 1) - param 1 = save mode.
    /// </summary>
    public class SaveGameScreen
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly int _width;
        private readonly int _height;
        private readonly SpriteFont _font;
        private KeyboardState _previousKeyboard;
        private string _saveName = "";
        private bool _focused = true;

        /// <summary>Fired when user confirms save; argument is the save name.</summary>
        public event Action<string> OnSave;

        /// <summary>Fired when user cancels.</summary>
        public event Action OnCancel;

        public SaveGameScreen(GraphicsDevice device, int width, int height, SpriteFont font)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _width = width > 0 ? width : device.Viewport.Width;
            _height = height > 0 ? height : device.Viewport.Height;
            _font = font;
            _spriteBatch = new SpriteBatch(device);
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            _previousKeyboard = Keyboard.GetState();
        }

        public void Update(float deltaTime)
        {
            var keyboard = Keyboard.GetState();

            if (_previousKeyboard.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape))
            {
                OnCancel?.Invoke();
                _previousKeyboard = keyboard;
                return;
            }

            if (_previousKeyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyDown(Keys.Enter))
            {
                ConfirmSave();
                _previousKeyboard = keyboard;
                return;
            }

            if (_focused)
            {
                foreach (Keys key in keyboard.GetPressedKeys())
                {
                    if (_previousKeyboard.IsKeyUp(key))
                    {
                        if (key == Keys.Back && _saveName.Length > 0)
                            _saveName = _saveName.Substring(0, _saveName.Length - 1);
                        else if (key == Keys.Space)
                            _saveName += " ";
                        else if (key >= Keys.A && key <= Keys.Z)
                            _saveName += (char)('a' + (key - Keys.A));
                        else if (key >= Keys.D0 && key <= Keys.D9)
                            _saveName += (char)('0' + (key - Keys.D0));
                    }
                }
            }

            _previousKeyboard = keyboard;
        }

        private void ConfirmSave()
        {
            string name = (_saveName ?? "").Trim();
            if (string.IsNullOrEmpty(name))
                name = "Save " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            OnSave?.Invoke(name);
        }

        public void Draw()
        {
            int w = _width > 0 ? _width : _device.Viewport.Width;
            int h = _height > 0 ? _height : _device.Viewport.Height;
            if (w <= 0) w = 800;
            if (h <= 0) h = 600;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, w, h), new Microsoft.Xna.Framework.Color(20, 20, 30, 255));

            if (_font != null)
            {
                string title = "Save Game";
                var ts = _font.MeasureString(title);
                _spriteBatch.DrawString(_font, title, new Vector2((w - ts.X) / 2f, 40), Microsoft.Xna.Framework.Color.White);

                string label = "Save name:";
                _spriteBatch.DrawString(_font, label, new Vector2(80, 120), Microsoft.Xna.Framework.Color.White);

                string display = string.IsNullOrEmpty(_saveName) ? "_" : _saveName + "_";
                var inputRect = new Rectangle(80, 150, w - 160, 36);
                _spriteBatch.Draw(_pixel, inputRect, new Microsoft.Xna.Framework.Color(40, 45, 55, 255));
                int t = 2;
                _spriteBatch.Draw(_pixel, new Rectangle(inputRect.X, inputRect.Y, inputRect.Width, t), Microsoft.Xna.Framework.Color.White);
                _spriteBatch.Draw(_pixel, new Rectangle(inputRect.X, inputRect.Bottom - t, inputRect.Width, t), Microsoft.Xna.Framework.Color.White);
                _spriteBatch.Draw(_pixel, new Rectangle(inputRect.X, inputRect.Y, t, inputRect.Height), Microsoft.Xna.Framework.Color.White);
                _spriteBatch.Draw(_pixel, new Rectangle(inputRect.Right - t, inputRect.Y, t, inputRect.Height), Microsoft.Xna.Framework.Color.White);
                _spriteBatch.DrawString(_font, display, new Vector2(inputRect.X + 8, inputRect.Y + 6), Microsoft.Xna.Framework.Color.White);

                string inst = "Enter save name. Enter to save, Escape to cancel.";
                var instSize = _font.MeasureString(inst);
                _spriteBatch.DrawString(_font, inst, new Vector2((w - instSize.X) / 2f, h - 40), new Microsoft.Xna.Framework.Color(180, 180, 180, 255));
            }

            _spriteBatch.End();
        }

        public void Dispose()
        {
            _pixel?.Dispose();
        }
    }
}
