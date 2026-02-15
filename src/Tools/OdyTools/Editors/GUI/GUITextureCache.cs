using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Media.Imaging;
using BioWare.Common;
using BioWare.Resource.Formats.TPC;
using OdyTools.Data;

namespace OdyTools.Editors.GUI
{
    /// <summary>
    /// Loads GUI textures from an asset folder (TPC/TGA). Caches by resref for preview.
    /// </summary>
    public class GUITextureCache
    {
        private readonly string _assetPath;
        private readonly Dictionary<string, Bitmap> _cache = new Dictionary<string, Bitmap>(StringComparer.OrdinalIgnoreCase);

        public GUITextureCache(string assetPath)
        {
            _assetPath = assetPath ?? string.Empty;
        }

        public void Clear()
        {
            foreach (var b in _cache.Values)
                b?.Dispose();
            _cache.Clear();
        }

        /// <summary>Gets a bitmap for the given resref (no extension). Tries .tga then .tpc.</summary>
        public Bitmap GetBitmap(string resref)
        {
            if (string.IsNullOrWhiteSpace(resref)) return null;
            string key = resref.Trim();
            if (_cache.TryGetValue(key, out var existing)) return existing;

            string basePath = string.IsNullOrEmpty(_assetPath) ? null : Path.Combine(_assetPath, key);
            if (string.IsNullOrEmpty(basePath)) return null;

            Bitmap bitmap = null;
            string tgaPath = basePath + ".tga";
            string tpcPath = basePath + ".tpc";
            string pngPath = basePath + ".png";

            if (File.Exists(pngPath))
            {
                try
                {
                    using (var fs = File.OpenRead(pngPath))
                        bitmap = new Bitmap(fs);
                }
                catch { /* ignore */ }
            }
            if (bitmap == null && File.Exists(tgaPath))
            {
                try
                {
                    using (var fs = File.OpenRead(tgaPath))
                        bitmap = new Bitmap(fs);
                }
                catch { /* ignore */ }
            }
            if (bitmap == null && File.Exists(tpcPath))
            {
                try
                {
                    var tpc = TPCAuto.ReadTpc(tpcPath);
                    if (tpc?.Layers != null && tpc.Layers.Count > 0 &&
                        tpc.Layers[0].Mipmaps != null && tpc.Layers[0].Mipmaps.Count > 0)
                    {
                        var mipmap = tpc.Layers[0].Mipmaps[0];
                        bitmap = OdyInstallation.ConvertTpcMipmapToAvaloniaBitmap(mipmap);
                    }
                }
                catch { /* ignore */ }
            }

            if (bitmap != null)
                _cache[key] = bitmap;
            return bitmap;
        }
    }
}
