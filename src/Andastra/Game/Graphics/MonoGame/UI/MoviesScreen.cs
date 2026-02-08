using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BioWare.Extract;

namespace Andastra.Game.Graphics.MonoGame.UI
{
    /// <summary>
    /// Movies screen - 1:1 with Reva CSWGuiTitleMovies.
    /// Lists BIK movies from game movies folder, plays selected movie on Enter.
    /// k1_win_gog_swkotor.exe OnMoviesPicked @ 0x0067b250: CSWGuiTitleMovies.
    /// </summary>
    public class MoviesScreen
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
        private List<string> _movies = new List<string>();

        /// <summary>Fired when user selects a movie to play; argument is the movie ResRef (no extension).</summary>
        public event Action<string> OnPlayMovie;

        /// <summary>Fired when user cancels (Escape or Back).</summary>
        public event Action OnCancel;

        /// <param name="installation">Game installation for movies path.</param>
        public MoviesScreen(GraphicsDevice device, int width, int height, SpriteFont font, Installation installation)
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
            RefreshMovies(installation);
        }

        private void RefreshMovies(Installation installation)
        {
            _movies.Clear();
            if (installation == null) return;
            try
            {
                string moviesPath = Path.Combine(installation.Path, "movies");
                if (Directory.Exists(moviesPath))
                {
                    foreach (string path in Directory.EnumerateFiles(moviesPath, "*.bik", SearchOption.TopDirectoryOnly))
                    {
                        string name = Path.GetFileNameWithoutExtension(path);
                        if (!string.IsNullOrEmpty(name))
                            _movies.Add(name);
                    }
                    _movies.Sort(StringComparer.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MoviesScreen] Error listing movies: {ex.Message}");
            }
            _selectedIndex = _movies.Count > 0 ? 0 : -1;
        }

        public void Update(float deltaTime)
        {
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            if (_previousKeyboard.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape))
            {
                OnCancel?.Invoke();
                _previousMouse = mouse;
                _previousKeyboard = keyboard;
                return;
            }

            if (_previousKeyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyDown(Keys.Enter) &&
                _selectedIndex >= 0 && _selectedIndex < _movies.Count)
            {
                OnPlayMovie?.Invoke(_movies[_selectedIndex]);
                _previousMouse = mouse;
                _previousKeyboard = keyboard;
                return;
            }

            if (_previousKeyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyDown(Keys.Up))
            {
                _selectedIndex = Math.Max(0, _selectedIndex - 1);
            }
            if (_previousKeyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyDown(Keys.Down))
            {
                _selectedIndex = Math.Min(_movies.Count - 1, _selectedIndex + 1);
            }

            int itemHeight = 44;
            int startY = 120;
            int mx = mouse.X, my = mouse.Y;
            for (int i = 0; i < _movies.Count; i++)
            {
                var r = GetItemRect(i);
                if (r.Contains(mx, my))
                {
                    _selectedIndex = i;
                    if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
                    {
                        OnPlayMovie?.Invoke(_movies[i]);
                        break;
                    }
                }
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
            int itemHeight = 44;
            int startY = 120;
            int left = 80;
            int right = w - 80;
            int spacing = 4;
            return new Rectangle(left, startY + index * (itemHeight + spacing), right - left, itemHeight);
        }

        public void Draw()
        {
            int w = _width > 0 ? _width : _device.Viewport.Width;
            int h = _height > 0 ? _height : _device.Viewport.Height;
            if (w <= 0) w = 800;
            if (h <= 0) h = 600;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, w, h), new Microsoft.Xna.Framework.Color(20, 20, 30, 255));

            string title = "Movies";
            if (_font != null)
            {
                var ts = _font.MeasureString(title);
                _spriteBatch.DrawString(_font, title, new Vector2((w - ts.X) / 2f, 40), Microsoft.Xna.Framework.Color.White);
            }

            for (int i = 0; i < _movies.Count; i++)
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
                    string line = _movies[i];
                    var textPos = new Vector2(r.X + 10, r.Y + (r.Height - _font.LineSpacing) / 2f);
                    _spriteBatch.DrawString(_font, line, textPos, Microsoft.Xna.Framework.Color.White);
                }
            }

            if (_font != null)
            {
                string inst = "Select a movie to play. Escape to cancel.";
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
