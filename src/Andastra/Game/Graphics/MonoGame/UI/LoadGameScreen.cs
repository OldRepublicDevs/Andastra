using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Andastra.Runtime.Content.ResourceProviders;
using Andastra.Game.Games.Odyssey.Save;
using Andastra.Runtime.Core.Save;
using BioWare.Extract;

namespace Andastra.Game.Graphics.MonoGame.UI
{
    /// <summary>
    /// Load game screen - 1:1 with Reva CSWGuiSaveLoad (load mode).
    /// swkotor.exe OnLoadSaveGame @ 0x0067b1a0: CSWGuiSaveLoad(manager, 0, 1) - param 0 = load mode.
    /// </summary>
    public class LoadGameScreen
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly int _width;
        private readonly int _height;
        private readonly SpriteFont _font;
        private MouseState _previousMouse;
        private KeyboardState _previousKeyboard;
        private int _selectedIndex;
        private List<SaveGameInfo> _saves;

        /// <summary>Fired when user selects a save to load; argument is the save folder name for LoadGameAsync.</summary>
        public event Action<string> OnLoad;

        /// <summary>Fired when user cancels (Escape or Back).</summary>
        public event Action OnCancel;

        /// <param name="savesDirectory">Path to saves folder (e.g. gamePath/saves).</param>
        /// <param name="installation">Game installation for resource provider.</param>
        public LoadGameScreen(GraphicsDevice device, int width, int height, SpriteFont font, string savesDirectory, Installation installation)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _width = width > 0 ? width : device.Viewport.Width;
            _height = height > 0 ? height : device.Viewport.Height;
            _font = font;
            _spriteBatch = new SpriteBatch(device);
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            _previousMouse = Mouse.GetState();
            _previousKeyboard = Keyboard.GetState();
            _saves = new List<SaveGameInfo>();
            RefreshSaves(savesDirectory, installation);
        }

        private void RefreshSaves(string savesDirectory, Installation installation)
        {
            _saves.Clear();
            if (string.IsNullOrEmpty(savesDirectory) || installation == null)
                return;
            try
            {
                var provider = new GameResourceProvider(installation);
                var mgr = new OdysseySaveGameManager(provider, savesDirectory);
                _saves.AddRange(mgr.ListSaves());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LoadGameScreen] Error listing saves: {ex.Message}");
            }
            _selectedIndex = _saves.Count > 0 ? 0 : -1;
        }

        public void SetSaves(List<SaveGameInfo> saves)
        {
            _saves = saves ?? new List<SaveGameInfo>();
            _selectedIndex = _saves.Count > 0 ? 0 : -1;
        }

        public void Update(float deltaTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            // Escape = cancel
            if (_previousKeyboard.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape))
            {
                OnCancel?.Invoke();
                _previousMouse = mouse;
                _previousKeyboard = keyboard;
                return;
            }

            // Mouse: update selection by hover
            int itemHeight = 50;
            int startY = 120;
            int mx = mouse.X, my = mouse.Y;
            int hoverIndex = -1;
            for (int i = 0; i < _saves.Count; i++)
            {
                var r = GetItemRect(i);
                if (r.Contains(mx, my))
                {
                    hoverIndex = i;
                    break;
                }
            }
            if (hoverIndex >= 0)
                _selectedIndex = hoverIndex;

            // Click to load
            if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
            {
                if (_selectedIndex >= 0 && _selectedIndex < _saves.Count)
                {
                    string loadKey = GetLoadKey(_saves[_selectedIndex]);
                    if (!string.IsNullOrEmpty(loadKey))
                        OnLoad?.Invoke(loadKey);
                }
            }

            // Keyboard: Up/Down
            if (_previousKeyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyDown(Keys.Up))
            {
                _selectedIndex = Math.Max(0, _selectedIndex - 1);
            }
            if (_previousKeyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyDown(Keys.Down))
            {
                _selectedIndex = Math.Min(_saves.Count - 1, _selectedIndex + 1);
            }
            if (_previousKeyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyDown(Keys.Enter) &&
                _selectedIndex >= 0 && _selectedIndex < _saves.Count)
            {
                string loadKey = GetLoadKey(_saves[_selectedIndex]);
                if (!string.IsNullOrEmpty(loadKey))
                    OnLoad?.Invoke(loadKey);
            }

            _previousMouse = mouse;
            _previousKeyboard = keyboard;
        }

        private Rectangle GetItemRect(int index)
        {
            int w = _width > 0 ? _width : _device.Viewport.Width;
            int h = _height > 0 ? _height : _device.Viewport.Height;
            if (w <= 0) w = 800;
            if (h <= 0) h = 600;
            int itemHeight = 50;
            int startY = 120;
            int left = 80;
            int right = w - 80;
            return new Rectangle(left, startY + index * (itemHeight + 8), right - left, itemHeight);
        }

        public void Draw()
        {
            int w = _width > 0 ? _width : _device.Viewport.Width;
            int h = _height > 0 ? _height : _device.Viewport.Height;
            if (w <= 0) w = 800;
            if (h <= 0) h = 600;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Background
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, w, h), new Microsoft.Xna.Framework.Color(20, 20, 30, 255));

            // Title
            string title = "Load Game";
            if (_font != null)
            {
                var ts = _font.MeasureString(title);
                _spriteBatch.DrawString(_font, title, new Vector2((w - ts.X) / 2f, 40), Microsoft.Xna.Framework.Color.White);
            }

            for (int i = 0; i < _saves.Count; i++)
            {
                var r = GetItemRect(i);
                bool sel = i == _selectedIndex;
                var fillColor = sel
                    ? new Microsoft.Xna.Framework.Color(80, 90, 120, 255)
                    : new Microsoft.Xna.Framework.Color(40, 45, 55, 255);
                _spriteBatch.Draw(_pixel, r, fillColor);
                int t = 2;
                var borderColor = new Microsoft.Xna.Framework.Color(100, 110, 140, 255);
                _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, r.Width, t), borderColor);
                _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Bottom - t, r.Width, t), borderColor);
                _spriteBatch.Draw(_pixel, new Rectangle(r.X, r.Y, t, r.Height), borderColor);
                _spriteBatch.Draw(_pixel, new Rectangle(r.Right - t, r.Y, t, r.Height), borderColor);

                if (_font != null)
                {
                    var save = _saves[i];
                    string line = $"{save.Name} - {save.ModuleName} - {save.SaveTime:g}";
                    var textPos = new Vector2(r.X + 10, r.Y + (r.Height - _font.LineSpacing) / 2f);
                    _spriteBatch.DrawString(_font, line, textPos, Microsoft.Xna.Framework.Color.White);
                }
            }

            if (_font != null)
            {
                string inst = "Select a save to load. Escape to cancel.";
                var instSize = _font.MeasureString(inst);
                _spriteBatch.DrawString(_font, inst, new Vector2((w - instSize.X) / 2f, h - 40), new Microsoft.Xna.Framework.Color(180, 180, 180, 255));
            }

            _spriteBatch.End();
        }

        /// <summary>Gets the save folder name for LoadGameAsync (e.g. "000001 - MySave"). SavePath is path to savegame.sav.</summary>
        private static string GetLoadKey(SaveGameInfo save)
        {
            if (save == null) return null;
            if (!string.IsNullOrEmpty(save.SavePath))
            {
                string dirPath = Path.GetDirectoryName(save.SavePath);
                string folderName = Path.GetFileName(dirPath);
                if (!string.IsNullOrEmpty(folderName)) return folderName;
            }
            return $"{save.SlotIndex:D6} - {save.Name}";
        }

        public void Dispose()
        {
            _pixel?.Dispose();
        }
    }
}
