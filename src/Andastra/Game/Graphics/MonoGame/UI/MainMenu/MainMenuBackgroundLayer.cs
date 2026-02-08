// ---------------------------------------------------------------------------
// MainMenuBackgroundLayer.cs
// Void fill and background texture layer for main menu.
// References:
// - KotOR.js GameMenu.ts loadBackground (lines 147-188): voidFill, background, void-gui shader, background-gui shader
// - reone mainmenu.cpp loadBackground(BackgroundType::Menu) (line 64)
// - reone GameGUI preload/background loading
// ---------------------------------------------------------------------------

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BioWare.Extract;

namespace Andastra.Game.Graphics.MonoGame.UI.MainMenu
{
    /// <summary>
    /// Renders the main menu background: void fill (solid/animated color) and optional
    /// background texture (K1: 1600x1200back). KotOR.js: voidFill + background sprite.
    /// </summary>
    public sealed class MainMenuBackgroundLayer
    {
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;
        private readonly Texture2D _pixel;
        private readonly bool _isK2;
        private Texture2D _backgroundTexture;
        private float _voidPhase;
        private bool _disposed;

        public MainMenuBackgroundLayer(GraphicsDevice device, int width, int height, bool isK2)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _isK2 = isK2;
            _spriteBatch = new SpriteBatch(device);
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Load background texture from installation. K1: 1600x1200back (KotOR.js line 43).
        /// TSL: no background texture (KotOR.js TSL line 40: this.background = '').
        /// </summary>
        public void LoadBackgroundTexture(Installation installation)
        {
            if (installation == null || _isK2) return;
            string resRef = MainMenuResRefs.K1_Background;
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
                    _backgroundTexture?.Dispose();
                    _backgroundTexture = t2d;
                }
            }
            catch { /* ignore */ }
        }

        /// <summary>
        /// Draw void fill and optional background texture. Call from MainMenuScreen.
        /// KotOR.js: voidFill scale set from viewport (line 253), background 1600x1200 (line 211).
        /// </summary>
        public void Draw(int viewportWidth, int viewportHeight, float deltaTime)
        {
            _voidPhase += deltaTime * 0.5f;
            if (_voidPhase > 100f) _voidPhase = 0f;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            var voidColor = _isK2 ? MainMenuColors.VoidFillK2 : MainMenuColors.VoidFillK1;
            _spriteBatch.Draw(_pixel, new Rectangle(0, 0, viewportWidth, viewportHeight), voidColor);

            if (_backgroundTexture != null && !_isK2)
            {
                float scaleX = viewportWidth / 1600f;
                float scaleY = viewportHeight / 1200f;
                int w = (int)(1600 * scaleX);
                int h = (int)(1200 * scaleY);
                int x = (viewportWidth - w) / 2;
                int y = (viewportHeight - h) / 2;
                _spriteBatch.Draw(_backgroundTexture, new Rectangle(x, y, w, h), Color.White);
            }

            _spriteBatch.End();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _pixel?.Dispose();
            _backgroundTexture?.Dispose();
            _backgroundTexture = null;
            _disposed = true;
        }
    }
}
