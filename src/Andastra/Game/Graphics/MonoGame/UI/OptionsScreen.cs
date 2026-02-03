using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Andastra.Game.Graphics.MonoGame.UI
{
    /// <summary>
    /// Options screen - 1:1 with Reva CSWGuiOptionsMain.
    /// Graphics, Sound, Game options. Persists to config file.
    /// swkotor.exe OnOptionsPicked @ 0x0067b2f0: CSWGuiOptionsMain.
    /// </summary>
    public class OptionsScreen
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly int _width;
        private readonly int _height;
        private readonly SpriteFont _font;
        private readonly string _configPath;
        private KeyboardState _previousKeyboard;
        private int _selectedCategory;
        private int _selectedOption;
        private int _editOption = -1;
        private string _editValue = "";

        public int ResolutionWidth { get; set; } = 1024;
        public int ResolutionHeight { get; set; } = 768;
        public bool Fullscreen { get; set; }
        public bool VSync { get; set; } = true;
        public float MusicVolume { get; set; } = 1f;
        public float SoundVolume { get; set; } = 1f;
        public bool DisableSound { get; set; }

        /// <summary>Fired when user requests to apply changes and close.</summary>
        public event Action OnApply;

        /// <summary>Fired when user cancels.</summary>
        public event Action OnCancel;

        /// <summary>Fired when resolution or fullscreen change - caller should apply.</summary>
        public event Action<int, int, bool> OnGraphicsChanged;

        public OptionsScreen(GraphicsDevice device, int width, int height, SpriteFont font, string configPath = null)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _width = width > 0 ? width : device.Viewport.Width;
            _height = height > 0 ? height : device.Viewport.Height;
            _font = font;
            _configPath = configPath ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Andastra", "andastra.ini");
            _spriteBatch = new SpriteBatch(device);
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            _previousKeyboard = Keyboard.GetState();
            LoadConfig();
        }

        private void LoadConfig()
        {
            try
            {
                if (!File.Exists(_configPath)) return;
                foreach (string line in File.ReadAllLines(_configPath))
                {
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith(";") || !trimmed.Contains("=")) continue;
                    int eq = trimmed.IndexOf('=');
                    string key = trimmed.Substring(0, eq).Trim();
                    string val = trimmed.Substring(eq + 1).Trim();
                    switch (key.ToLowerInvariant())
                    {
                        case "width": if (int.TryParse(val, out int w) && w > 0) ResolutionWidth = w; break;
                        case "height": if (int.TryParse(val, out int h) && h > 0) ResolutionHeight = h; break;
                        case "fullscreen": Fullscreen = val.Equals("1", StringComparison.OrdinalIgnoreCase) || val.Equals("true", StringComparison.OrdinalIgnoreCase); break;
                        case "vsync": VSync = val.Equals("1", StringComparison.OrdinalIgnoreCase) || val.Equals("true", StringComparison.OrdinalIgnoreCase); break;
                        case "musicvolume": if (float.TryParse(val, out float mv)) MusicVolume = Math.Max(0, Math.Min(1, mv)); break;
                        case "soundvolume": if (float.TryParse(val, out float sv)) SoundVolume = Math.Max(0, Math.Min(1, sv)); break;
                        case "disablesound": DisableSound = val.Equals("1", StringComparison.OrdinalIgnoreCase) || val.Equals("true", StringComparison.OrdinalIgnoreCase); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OptionsScreen] LoadConfig error: {ex.Message}");
            }
        }

        private void SaveConfig()
        {
            try
            {
                string dir = Path.GetDirectoryName(_configPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(_configPath, $"[Graphics]\nWidth={ResolutionWidth}\nHeight={ResolutionHeight}\nFullscreen={(Fullscreen ? 1 : 0)}\nVSync={(VSync ? 1 : 0)}\n\n[Sound]\nMusicVolume={MusicVolume}\nSoundVolume={SoundVolume}\nDisableSound={(DisableSound ? 1 : 0)}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OptionsScreen] SaveConfig error: {ex.Message}");
            }
        }

        private (string Label, string Value)[] GetGraphicsOptions()
        {
            return new[]
            {
                ("Width", ResolutionWidth.ToString()),
                ("Height", ResolutionHeight.ToString()),
                ("Fullscreen", Fullscreen ? "Yes" : "No"),
                ("VSync", VSync ? "Yes" : "No")
            };
        }

        private (string Label, string Value)[] GetSoundOptions()
        {
            return new[]
            {
                ("Music Volume", $"{MusicVolume:P0}"),
                ("Sound Volume", $"{SoundVolume:P0}"),
                ("Disable Sound", DisableSound ? "Yes" : "No")
            };
        }

        public void Update(float deltaTime)
        {
            var keyboard = Keyboard.GetState();
            var opts = _selectedCategory == 0 ? GetGraphicsOptions() : GetSoundOptions();

            if (_editOption >= 0)
            {
                if (_previousKeyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyDown(Keys.Enter))
                {
                    ApplyEdit(opts, _editOption);
                    _editOption = -1;
                }
                else if (_previousKeyboard.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape))
                {
                    _editOption = -1;
                }
                else
                {
                    foreach (Keys key in keyboard.GetPressedKeys())
                    {
                        if (_previousKeyboard.IsKeyUp(key))
                        {
                            if (key >= Keys.D0 && key <= Keys.D9) _editValue += (char)('0' + (key - Keys.D0));
                            else if (key == Keys.Back && _editValue.Length > 0) _editValue = _editValue.Substring(0, _editValue.Length - 1);
                        }
                    }
                }
            }
            else
            {
                if (_previousKeyboard.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape))
                {
                    OnCancel?.Invoke();
                }
                else if (_previousKeyboard.IsKeyUp(Keys.Tab) && keyboard.IsKeyDown(Keys.Tab))
                {
                    _selectedCategory = 1 - _selectedCategory;
                    _selectedOption = 0;
                }
                else if (_previousKeyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyDown(Keys.Up))
                {
                    _selectedOption = Math.Max(0, _selectedOption - 1);
                }
                else if (_previousKeyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyDown(Keys.Down))
                {
                    int maxOpt = opts.Length + 1;
                    _selectedOption = Math.Min(maxOpt, _selectedOption + 1);
                }
                else if (_previousKeyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyDown(Keys.Enter))
                {
                    if (_selectedOption == opts.Length)
                    {
                        SaveConfig();
                        OnGraphicsChanged?.Invoke(ResolutionWidth, ResolutionHeight, Fullscreen);
                        OnApply?.Invoke();
                    }
                    else if (_selectedOption == opts.Length + 1)
                    {
                        OnCancel?.Invoke();
                    }
                    else
                    {
                        ToggleOrEdit(opts, _selectedOption);
                    }
                }
                else if (_previousKeyboard.IsKeyUp(Keys.Left) && keyboard.IsKeyDown(Keys.Left))
                {
                    AdjustOption(opts, _selectedOption, -1);
                }
                else if (_previousKeyboard.IsKeyUp(Keys.Right) && keyboard.IsKeyDown(Keys.Right))
                {
                    AdjustOption(opts, _selectedOption, 1);
                }
            }

            _previousKeyboard = keyboard;
        }

        private void ToggleOrEdit((string Label, string Value)[] opts, int idx)
        {
            if (_selectedCategory == 0)
            {
                if (idx == 2) { Fullscreen = !Fullscreen; return; }
                if (idx == 3) { VSync = !VSync; return; }
                _editOption = idx;
                _editValue = idx == 0 ? ResolutionWidth.ToString() : ResolutionHeight.ToString();
            }
            else
            {
                if (idx == 2) { DisableSound = !DisableSound; return; }
                _editOption = idx;
                _editValue = idx == 0 ? ((int)(MusicVolume * 100)).ToString() : ((int)(SoundVolume * 100)).ToString();
            }
        }

        private void ApplyEdit((string Label, string Value)[] opts, int idx)
        {
            if (_selectedCategory == 0 && (idx == 0 || idx == 1))
            {
                if (int.TryParse(_editValue, out int v) && v >= 320)
                {
                    if (idx == 0) ResolutionWidth = v;
                    else ResolutionHeight = v;
                }
            }
            else if (_selectedCategory == 1 && (idx == 0 || idx == 1))
            {
                if (int.TryParse(_editValue, out int p))
                {
                    float f = Math.Max(0, Math.Min(100, p)) / 100f;
                    if (idx == 0) MusicVolume = f;
                    else SoundVolume = f;
                }
            }
        }

        private void AdjustOption((string Label, string Value)[] opts, int idx, int delta)
        {
            if (idx < 0 || idx >= opts.Length) return;
            if (_selectedCategory == 0)
            {
                if (idx == 0) ResolutionWidth = Math.Max(320, Math.Min(7680, ResolutionWidth + delta * 64));
                else if (idx == 1) ResolutionHeight = Math.Max(240, Math.Min(4320, ResolutionHeight + delta * 64));
                else if (idx == 2) Fullscreen = !Fullscreen;
                else if (idx == 3) VSync = !VSync;
            }
            else
            {
                if (idx == 0) MusicVolume = Math.Max(0, Math.Min(1, MusicVolume + delta * 0.1f));
                else if (idx == 1) SoundVolume = Math.Max(0, Math.Min(1, SoundVolume + delta * 0.1f));
                else if (idx == 2) DisableSound = !DisableSound;
            }
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
                string title = "Options";
                var ts = _font.MeasureString(title);
                _spriteBatch.DrawString(_font, title, new Vector2((w - ts.X) / 2f, 30), Microsoft.Xna.Framework.Color.White);

                int catY = 80;
                _spriteBatch.DrawString(_font, _selectedCategory == 0 ? "[Graphics]" : "Graphics", new Vector2(80, catY), _selectedCategory == 0 ? Microsoft.Xna.Framework.Color.Yellow : Microsoft.Xna.Framework.Color.Gray);
                _spriteBatch.DrawString(_font, _selectedCategory == 1 ? "[Sound]" : "Sound", new Vector2(200, catY), _selectedCategory == 1 ? Microsoft.Xna.Framework.Color.Yellow : Microsoft.Xna.Framework.Color.Gray);

                var opts = _selectedCategory == 0 ? GetGraphicsOptions() : GetSoundOptions();
                int startY = 120;
                for (int i = 0; i < opts.Length; i++)
                {
                    bool sel = i == _selectedOption && _editOption < 0;
                    string line = opts[i].Label + ": " + (_editOption == i ? _editValue + "_" : opts[i].Value);
                    var c = sel ? Microsoft.Xna.Framework.Color.Yellow : Microsoft.Xna.Framework.Color.White;
                    _spriteBatch.DrawString(_font, line, new Vector2(80, startY + i * 28), c);
                }
                int applyIdx = opts.Length;
                int cancelIdx = opts.Length + 1;
                _spriteBatch.DrawString(_font, "Apply", new Vector2(80, startY + applyIdx * 28), _selectedOption == applyIdx ? Microsoft.Xna.Framework.Color.LightGreen : Microsoft.Xna.Framework.Color.White);
                _spriteBatch.DrawString(_font, "Cancel", new Vector2(80, startY + cancelIdx * 28), _selectedOption == cancelIdx ? Microsoft.Xna.Framework.Color.LightCoral : Microsoft.Xna.Framework.Color.White);

                if (_selectedOption >= 0 && _selectedOption < opts.Length)
                    _spriteBatch.DrawString(_font, "Left/Right to change, Enter to edit", new Vector2(80, h - 60), Microsoft.Xna.Framework.Color.Gray);
            }

            _spriteBatch.End();
        }

        public void Dispose()
        {
            _pixel?.Dispose();
        }
    }
}
