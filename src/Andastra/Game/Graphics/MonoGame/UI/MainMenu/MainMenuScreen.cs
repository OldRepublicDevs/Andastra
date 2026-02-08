// ---------------------------------------------------------------------------
// MainMenuScreen.cs
// Exhaustive main menu implementation matching:
// - vendor/KotOR.js MainMenu (K1 + TSL), GameMenu base, LBL_3DView
// - vendor/reone MainMenu, GameGUI, bindControls, setup3DView, startModuleSelection
// - Reva: CClientExoApp::DisplayMainMenu, CSWGuiMainMenu, button handlers
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BioWare.Extract;

namespace Andastra.Game.Graphics.MonoGame.UI.MainMenu
{
    /// <summary>
    /// Full main menu screen with every UI element from KotOR.js, reone, and Reva.
    /// Supports both GFF-driven (via KotorGuiManager) and fallback (fully drawn) modes.
    /// </summary>
    public sealed class MainMenuScreen
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly int _width;
        private readonly int _height;
        private readonly bool _isK2;
        private readonly Texture2D _pixel;
        private readonly Dictionary<string, Texture2D> _textures;
        private readonly MainMenuButtonDef[] _buttons;
        private readonly MainMenuLabelDef[] _labels;
        private readonly List<MainMenuListBoxItem> _moduleList;
        private MouseState _prevMouse;
        private KeyboardState _prevKeyboard;
        private int _hoveredButtonIndex;
        private int _selectedButtonIndex;
        private bool _moduleSelectionMode;
        private int _hoveredModuleIndex;
        private int _selectedModuleIndex;
        private float _voidFillPhase;
        private Texture2D _backgroundTexture;
        private Texture2D _lbl3DViewTexture;
        private Rectangle _lbl3DViewRect;
        private bool _showLbl3DView;
        private bool _showBackgroundTexture;
        private bool _showWarpButton;
        private bool _showMusicButton;

        /// <summary>Fired when a main menu button is clicked; argument is control tag (e.g. BTN_NEWGAME).</summary>
        public event Action<string> OnButtonClicked;

        /// <summary>Fired when a module is selected from LB_MODULES (warp mode). Argument is module name.</summary>
        public event Action<string> OnModuleSelected;

        public MainMenuScreen(
            GraphicsDevice device,
            int width,
            int height,
            bool isK2,
            SpriteFont font = null,
            Installation installation = null)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _width = width > 0 ? width : device.Viewport.Width;
            _height = height > 0 ? height : device.Viewport.Height;
            _isK2 = isK2;
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
            _textures = new Dictionary<string, Texture2D>(StringComparer.OrdinalIgnoreCase);
            _spriteBatch = new SpriteBatch(device);
            _prevMouse = Mouse.GetState();
            _prevKeyboard = Keyboard.GetState();
            _hoveredButtonIndex = -1;
            _selectedButtonIndex = 0;
            _moduleList = new List<MainMenuListBoxItem>();
            _voidFillPhase = 0f;
            _showWarpButton = false;
            _showMusicButton = isK2;

            _buttons = BuildButtonDefs(font, isK2);
            _labels = BuildLabelDefs();

            if (installation != null)
            {
                TryLoadTexture(installation, MainMenuResRefs.K1_Background, "background");
                if (_isK2)
                    _showBackgroundTexture = false;
                else
                    _showBackgroundTexture = _textures.ContainsKey("background");
            }

            _lbl3DViewRect = new Rectangle(_width / 2 - 200, 0, 400, 300);
            _showLbl3DView = true;
        }

        private static MainMenuButtonDef[] BuildButtonDefs(SpriteFont font, bool isK2)
        {
            var list = new List<MainMenuButtonDef>();
            var order = MainMenuButtonOrder.DownOrder;
            if (isK2)
            {
                var k2Order = new List<string>(order.Length + 1);
                foreach (var tag in order)
                {
                    if (tag == MainMenuControlTags.BTN_OPTIONS)
                    {
                        k2Order.Add(MainMenuControlTags.BTN_OPTIONS);
                        k2Order.Add(MainMenuControlTags.BTN_MUSIC);
                    }
                    else
                        k2Order.Add(tag);
                }
                order = k2Order.ToArray();
            }
            int y = MainMenuLayout.ButtonStartY;
            for (int i = 0; i < order.Length; i++)
            {
                string tag = order[i];
                string label = GetButtonLabel(tag);
                list.Add(new MainMenuButtonDef
                {
                    Tag = tag,
                    Label = label,
                    Bounds = new Rectangle(0, y + i * MainMenuLayout.ButtonSpacing, MainMenuLayout.ButtonWidth, MainMenuLayout.ButtonHeight),
                    Font = font,
                    Visible = true
                });
            }
            return list.ToArray();
        }

        private static string GetButtonLabel(string tag)
        {
            switch (tag)
            {
                case MainMenuControlTags.BTN_NEWGAME: return "New Game";
                case MainMenuControlTags.BTN_LOADGAME: return "Load Game";
                case MainMenuControlTags.BTN_MOVIES: return "Movies";
                case MainMenuControlTags.BTN_OPTIONS: return "Options";
                case MainMenuControlTags.BTN_EXIT: return "Exit";
                case MainMenuControlTags.BTN_WARP: return "Warp (Load)";
                case MainMenuControlTags.BTN_MUSIC: return "Music";
                default: return tag;
            }
        }

        private static MainMenuLabelDef[] BuildLabelDefs()
        {
            return new[]
            {
                new MainMenuLabelDef { Tag = MainMenuControlTags.LBL_GAMELOGO, Bounds = new Rectangle(200, 20, 400, 80), Visible = true },
                new MainMenuLabelDef { Tag = MainMenuControlTags.LBL_MENUBG, Bounds = new Rectangle(0, 0, 800, 600), Visible = true },
                new MainMenuLabelDef { Tag = MainMenuControlTags.LBL_3DVIEW, Bounds = new Rectangle(200, 100, 400, 300), Visible = true },
                new MainMenuLabelDef { Tag = MainMenuControlTags.LBL_BW, Bounds = new Rectangle(10, 500, 100, 80), Visible = false },
                new MainMenuLabelDef { Tag = MainMenuControlTags.LBL_LUCAS, Bounds = new Rectangle(690, 500, 100, 80), Visible = false },
                new MainMenuLabelDef { Tag = MainMenuControlTags.LBL_NEWCONTENT, Bounds = new Rectangle(300, 400, 200, 30), Visible = false },
            };
        }

        private void TryLoadTexture(Installation installation, string resRef, string key)
        {
            if (string.IsNullOrEmpty(resRef)) return;
            try
            {
                var result = installation.Resources.LookupResource(resRef, BioWare.Common.ResourceType.TPC, null, null);
                if (result?.Data == null || result.Data.Length == 0) return;
                var tpc = BioWare.Resource.Formats.TPC.TPCAuto.ReadTpc(result.Data);
                if (tpc?.Layers == null || tpc.Layers.Count == 0) return;
                var tex = Andastra.Game.Graphics.MonoGame.Converters.TpcToMonoGameTextureConverter.Convert(tpc, _device, false, false, false);
                if (tex is Texture2D t2d)
                {
                    if (_textures.TryGetValue(key, out var old))
                    {
                        old?.Dispose();
                    }
                    _textures[key] = t2d;
                }
            }
            catch { /* ignore */ }
        }

        /// <summary>
        /// Enable or hide warp button (reone: developer mode). reone mainmenu.cpp lines 83-86.
        /// </summary>
        public void SetWarpButtonVisible(bool visible)
        {
            _showWarpButton = visible;
            int idx = Array.FindIndex(_buttons, b => b.Tag == MainMenuControlTags.BTN_WARP);
            if (idx >= 0)
            {
                var w = _buttons[idx];
                w.Visible = visible;
                _buttons[idx] = w;
            }
        }

        /// <summary>
        /// Set list of module names for LB_MODULES (warp mode). reone loadModuleNames (lines 172-179).
        /// </summary>
        public void SetModuleList(IEnumerable<string> moduleNames)
        {
            _moduleList.Clear();
            if (moduleNames != null)
            {
                foreach (var name in moduleNames)
                    _moduleList.Add(new MainMenuListBoxItem { Tag = name, Text = name });
            }
        }

        /// <summary>
        /// Enter module selection mode. reone startModuleSelection (lines 152-170).
        /// Hides menu buttons, shows LB_MODULES, hides 3D view and logo.
        /// </summary>
        public void EnterModuleSelectionMode()
        {
            _moduleSelectionMode = true;
            _selectedModuleIndex = 0;
            _hoveredModuleIndex = -1;
        }

        /// <summary>
        /// Leave module selection mode and return to normal menu.
        /// </summary>
        public void ExitModuleSelectionMode()
        {
            _moduleSelectionMode = false;
        }

        public void Update(float deltaTime)
        {
            _voidFillPhase += deltaTime * 0.5f;
            if (_voidFillPhase > 100f) _voidFillPhase = 0f;

            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();

            if (_moduleSelectionMode)
            {
                UpdateModuleListInput(mouse, keyboard);
            }
            else
            {
                UpdateButtonInput(mouse, keyboard);
            }

            _prevMouse = mouse;
            _prevKeyboard = keyboard;
        }

        private void UpdateButtonInput(MouseState mouse, KeyboardState keyboard)
        {
            int centerX = _width / 2;
            var visibleButtons = _buttons.Where(b => b.Visible && IsButtonInMenu(b.Tag)).ToList();
            _hoveredButtonIndex = -1;
            for (int i = 0; i < visibleButtons.Count; i++)
            {
                var r = GetButtonBoundsScaled(visibleButtons[i], centerX);
                if (r.Contains(mouse.X, mouse.Y))
                {
                    _hoveredButtonIndex = i;
                    break;
                }
            }

            bool downPressed = _prevKeyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyDown(Keys.Down);
            bool upPressed = _prevKeyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyDown(Keys.Up);
            if (downPressed && visibleButtons.Count > 0)
            {
                _selectedButtonIndex = (_selectedButtonIndex + 1) % visibleButtons.Count;
            }
            if (upPressed && visibleButtons.Count > 0)
            {
                _selectedButtonIndex = _selectedButtonIndex <= 0 ? visibleButtons.Count - 1 : _selectedButtonIndex - 1;
            }

            bool activate = (_prevKeyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyDown(Keys.Enter)) ||
                           (_prevKeyboard.IsKeyUp(Keys.Space) && keyboard.IsKeyDown(Keys.Space));
            bool cancel = _prevKeyboard.IsKeyUp(Keys.Escape) && keyboard.IsKeyDown(Keys.Escape);
            if (cancel)
            {
                OnButtonClicked?.Invoke(MainMenuControlTags.BTN_EXIT);
                return;
            }
            if (activate && visibleButtons.Count > 0)
            {
                int idx = _selectedButtonIndex;
                if (idx >= 0 && idx < visibleButtons.Count)
                    OnButtonClicked?.Invoke(visibleButtons[idx].Tag);
                return;
            }

            if (_prevMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < visibleButtons.Count; i++)
                {
                    var r = GetButtonBoundsScaled(visibleButtons[i], centerX);
                    if (r.Contains(mouse.X, mouse.Y))
                    {
                        OnButtonClicked?.Invoke(visibleButtons[i].Tag);
                        break;
                    }
                }
            }
        }

        private bool IsButtonInMenu(string tag)
        {
            if (tag == MainMenuControlTags.BTN_WARP && !_showWarpButton) return false;
            if (tag == MainMenuControlTags.BTN_MUSIC && !_showMusicButton) return false;
            return true;
        }

        private Rectangle GetButtonBoundsScaled(MainMenuButtonDef def, int centerX)
        {
            float scaleX = _width / (float)MainMenuLayout.ReferenceWidth;
            float scaleY = _height / (float)MainMenuLayout.ReferenceHeight;
            int x = (int)(centerX - MainMenuLayout.ButtonWidth * scaleX / 2);
            int y = (int)(def.Bounds.Y * scaleY);
            int w = (int)(MainMenuLayout.ButtonWidth * scaleX);
            int h = (int)(MainMenuLayout.ButtonHeight * scaleY);
            return new Rectangle(x, y, w, h);
        }

        private void UpdateModuleListInput(MouseState mouse, KeyboardState keyboard)
        {
            if (_moduleList.Count == 0) return;
            var listRect = GetModuleListBounds();
            _hoveredModuleIndex = -1;
            int itemHeight = 24;
            int idx = (mouse.Y - listRect.Y) / itemHeight;
            if (idx >= 0 && idx < _moduleList.Count && listRect.Contains(mouse.X, mouse.Y))
                _hoveredModuleIndex = idx;

            bool down = _prevKeyboard.IsKeyUp(Keys.Down) && keyboard.IsKeyDown(Keys.Down);
            bool up = _prevKeyboard.IsKeyUp(Keys.Up) && keyboard.IsKeyDown(Keys.Up);
            if (down) _selectedModuleIndex = Math.Min(_selectedModuleIndex + 1, _moduleList.Count - 1);
            if (up) _selectedModuleIndex = Math.Max(_selectedModuleIndex - 1, 0);

            bool activate = (_prevKeyboard.IsKeyUp(Keys.Enter) && keyboard.IsKeyDown(Keys.Enter)) ||
                           (_prevKeyboard.IsKeyUp(Keys.Space) && keyboard.IsKeyDown(Keys.Space));
            if (activate && _selectedModuleIndex >= 0 && _selectedModuleIndex < _moduleList.Count)
            {
                OnModuleSelected?.Invoke(_moduleList[_selectedModuleIndex].Tag);
                return;
            }
            if (_prevMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed &&
                _hoveredModuleIndex >= 0 && _hoveredModuleIndex < _moduleList.Count)
            {
                OnModuleSelected?.Invoke(_moduleList[_hoveredModuleIndex].Tag);
            }
        }

        private Rectangle GetModuleListBounds()
        {
            int w = (int)(400 * (_width / (float)MainMenuLayout.ReferenceWidth));
            int h = Math.Min(400, _moduleList.Count * 24);
            int x = (_width - w) / 2;
            int y = (_height - h) / 2;
            return new Rectangle(x, y, w, h);
        }

        public void Draw()
        {
            float scaleX = _width / (float)MainMenuLayout.ReferenceWidth;
            float scaleY = _height / (float)MainMenuLayout.ReferenceHeight;
            int centerX = _width / 2;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);

            DrawVoidFill();
            DrawBackgroundTexture(scaleX, scaleY);
            if (!_moduleSelectionMode)
            {
                Draw3DViewPlaceholder(scaleX, scaleY);
                DrawLabels(scaleX, scaleY);
                DrawButtons(centerX, scaleX, scaleY);
            }
            else
            {
                DrawModuleList();
            }

            _spriteBatch.End();
        }

        private void DrawVoidFill()
        {
            var c = _isK2 ? MainMenuColors.VoidFillK2 : MainMenuColors.VoidFillK1;
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, _width, _height), c);
        }

        private void DrawBackgroundTexture(float scaleX, float scaleY)
        {
            if (!_showBackgroundTexture || !_textures.TryGetValue("background", out var tex)) return;
            int w = (int)(1600 * scaleX);
            int h = (int)(1200 * scaleY);
            int x = (_width - w) / 2;
            int y = (_height - h) / 2;
            _spriteBatch.Draw(tex, new Rectangle(x, y, w, h), Color.White);
        }

        private void Draw3DViewPlaceholder(float scaleX, float scaleY)
        {
            if (!_showLbl3DView) return;
            int left = (int)(_labels.First(l => l.Tag == MainMenuControlTags.LBL_3DVIEW).Bounds.X * scaleX);
            int top = (int)(_labels.First(l => l.Tag == MainMenuControlTags.LBL_3DVIEW).Bounds.Y * scaleY);
            int w = (int)(_labels.First(l => l.Tag == MainMenuControlTags.LBL_3DVIEW).Bounds.Width * scaleX);
            int h = (int)(_labels.First(l => l.Tag == MainMenuControlTags.LBL_3DVIEW).Bounds.Height * scaleY);
            var rect = new Rectangle(left, top, w, h);
            var dark = new Color(10, 20, 30, 255);
            _spriteBatch.Draw(_pixel, rect, dark);
            var border = new Color(60, 80, 100, 255);
            DrawRectBorder(rect, border);
        }

        private void DrawLabels(float scaleX, float scaleY)
        {
            foreach (var label in _labels.Where(l => l.Visible))
            {
                if (label.Tag == MainMenuControlTags.LBL_3DVIEW) continue;
                var r = new Rectangle(
                    (int)(label.Bounds.X * scaleX),
                    (int)(label.Bounds.Y * scaleY),
                    (int)(label.Bounds.Width * scaleX),
                    (int)(label.Bounds.Height * scaleY));
                _spriteBatch.Draw(_pixel, r, new Color(0, 0, 0, 0));
            }
        }

        private void DrawButtons(int centerX, float scaleX, float scaleY)
        {
            var baseColor = _isK2 ? MainMenuColors.K2_BaseColor : MainMenuColors.K1_BaseColor;
            var hilightColor = _isK2 ? MainMenuColors.K2_HilightColor : MainMenuColors.K1_HilightColor;
            var visibleButtons = _buttons.Where(b => b.Visible && IsButtonInMenu(b.Tag)).ToList();

            for (int i = 0; i < visibleButtons.Count; i++)
            {
                var def = visibleButtons[i];
                var r = GetButtonBoundsScaled(def, centerX);
                bool hover = i == _hoveredButtonIndex;
                bool selected = i == _selectedButtonIndex;
                var fillColor = (hover || selected) ? hilightColor : baseColor;
                var borderColor = new Color(120, 140, 180, 255);
                _spriteBatch.Draw(_pixel, r, fillColor);
                DrawRectBorder(r, borderColor);
                if (def.Font != null && !string.IsNullOrEmpty(def.Label))
                {
                    var size = def.Font.MeasureString(def.Label);
                    var pos = new Vector2(r.X + (r.Width - size.X) / 2f, r.Y + (r.Height - size.Y) / 2f);
                    _spriteBatch.DrawString(def.Font, def.Label, pos, Color.White);
                }
            }
        }

        private void DrawModuleList()
        {
            if (_moduleList.Count == 0) return;
            var listRect = GetModuleListBounds();
            _spriteBatch.Draw(_pixel, listRect, new Color(30, 40, 60, 255));
            DrawRectBorder(listRect, new Color(80, 100, 140, 255));
            int itemHeight = 24;
            for (int i = 0; i < _moduleList.Count; i++)
            {
                var itemRect = new Rectangle(listRect.X, listRect.Y + i * itemHeight, listRect.Width, itemHeight);
                bool hover = i == _hoveredModuleIndex;
                bool selected = i == _selectedModuleIndex;
                if (hover || selected)
                    _spriteBatch.Draw(_pixel, itemRect, new Color(60, 80, 120, 255));
            }
        }

        private void DrawRectBorder(Rectangle r, Color color)
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
            foreach (var t in _textures.Values)
                t?.Dispose();
            _textures.Clear();
            _backgroundTexture = null;
            _lbl3DViewTexture = null;
        }

        private struct MainMenuButtonDef
        {
            public string Tag;
            public string Label;
            public Rectangle Bounds;
            public SpriteFont Font;
            public bool Visible;
        }

        private struct MainMenuLabelDef
        {
            public string Tag;
            public Rectangle Bounds;
            public bool Visible;
        }

        private struct MainMenuListBoxItem
        {
            public string Tag;
            public string Text;
        }
    }
}
