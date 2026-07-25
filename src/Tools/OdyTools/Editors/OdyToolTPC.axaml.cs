using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Platform;
using BioWare.Resource.Formats.TPC;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;

namespace OdyTools.Editors
{
    public partial class OdyToolTPC : Editor
    {
        private TPC _tpc;
        private Image _textureImage;
        private TextBlock _previewMessage;
        private Slider _zoomSlider;
        private TextBlock _zoomPercentLabel;
        private NumericUpDown _mipmapBox;
        private TextBox _txiEdit;
        private TextBlock _dimensionsValue;
        private TextBlock _formatValue;
        private TextBlock _layersValue;
        private TextBlock _mipmapsValue;
        private TextBlock _compressedValue;
        private TextBlock _animatedValue;
        private TextBlock _cubeMapValue;
        private TextBlock _alphaValue;
        private NumericUpDown _alphaTestBox;
        private bool _syncingUi;
        private bool _menuClicksBound;
        private byte[] _loadedPltBytes;
        private const string ClipboardTexturePrefix = "data:application/x-odytools-tpc;base64,";

        public OdyToolTPC() : this(null, null) { }
        public OdyToolTPC(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolTPC", "none",
                new[] { ResourceType.TPC, ResourceType.TGA, ResourceType.DDS, ResourceType.JPG, ResourceType.PNG, ResourceType.BMP, ResourceType.PLT },
                new[] { ResourceType.TPC, ResourceType.TGA, ResourceType.DDS, ResourceType.JPG, ResourceType.PNG, ResourceType.BMP, ResourceType.PLT },
                installation)
        {
            InitializeComponent();
            SetupUI();
            New();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupUI()
        {
            _textureImage = EditorHelpers.FindControlSafe<Image>(this, "textureLabel");
            _previewMessage = EditorHelpers.FindControlSafe<TextBlock>(this, "previewMessage");
            _zoomSlider = EditorHelpers.FindControlSafe<Slider>(this, "zoomSlider");
            _zoomPercentLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "zoomPercentLabel");
            _mipmapBox = EditorHelpers.FindControlSafe<NumericUpDown>(this, "mipmapBox");
            _txiEdit = EditorHelpers.FindControlSafe<TextBox>(this, "txiEdit");
            _dimensionsValue = EditorHelpers.FindControlSafe<TextBlock>(this, "dimensionsValue");
            _formatValue = EditorHelpers.FindControlSafe<TextBlock>(this, "formatValue");
            _layersValue = EditorHelpers.FindControlSafe<TextBlock>(this, "layersValue");
            _mipmapsValue = EditorHelpers.FindControlSafe<TextBlock>(this, "mipmapsValue");
            _compressedValue = EditorHelpers.FindControlSafe<TextBlock>(this, "compressedValue");
            _animatedValue = EditorHelpers.FindControlSafe<TextBlock>(this, "animatedValue");
            _cubeMapValue = EditorHelpers.FindControlSafe<TextBlock>(this, "cubeMapValue");
            _alphaValue = EditorHelpers.FindControlSafe<TextBlock>(this, "alphaValue");
            _alphaTestBox = EditorHelpers.FindControlSafe<NumericUpDown>(this, "alphaTestBox");

            var zoomOut = EditorHelpers.FindControlSafe<Button>(this, "zoomOutButton");
            var zoomIn = EditorHelpers.FindControlSafe<Button>(this, "zoomInButton");
            if (zoomOut != null) zoomOut.Click += (_, __) => AdjustZoom(-25);
            if (zoomIn != null) zoomIn.Click += (_, __) => AdjustZoom(25);
            if (_zoomSlider != null) _zoomSlider.ValueChanged += (_, __) => RefreshPreview();
            if (_mipmapBox != null) _mipmapBox.ValueChanged += (_, __) => RefreshPreview();
            if (_alphaTestBox != null) _alphaTestBox.ValueChanged += (_, __) => ApplyAlphaTestValue();
            if (_txiEdit != null) _txiEdit.LostFocus += (_, __) => ApplyTxiText();
            BindMenuActions();
            Opened += (_, __) =>
            {
                BindMenuActions();
                RefreshPreview();
            };
        }

        private void BindMenuActions()
        {
            if (_menuClicksBound)
            {
                return;
            }

            var copy = EditorHelpers.FindControlSafe<MenuItem>(this, "actionCopy");
            var paste = EditorHelpers.FindControlSafe<MenuItem>(this, "actionPaste");
            var rotateLeft = EditorHelpers.FindControlSafe<MenuItem>(this, "actionRotateLeft");
            var rotateRight = EditorHelpers.FindControlSafe<MenuItem>(this, "actionRotateRight");
            var flipHorizontal = EditorHelpers.FindControlSafe<MenuItem>(this, "actionFlipHorizontal");
            var flipVertical = EditorHelpers.FindControlSafe<MenuItem>(this, "actionFlipVertical");

            if (copy == null || paste == null || rotateLeft == null || rotateRight == null || flipHorizontal == null || flipVertical == null)
            {
                return;
            }

            copy.Command = new EditorActionCommand(() => _ = CopyTextureToClipboardAsync());
            paste.Command = new EditorActionCommand(() => _ = PasteTextureFromClipboardAsync());
            rotateLeft.Command = new EditorActionCommand(RotateLeft);
            rotateRight.Command = new EditorActionCommand(RotateRight);
            flipHorizontal.Command = new EditorActionCommand(FlipHorizontal);
            flipVertical.Command = new EditorActionCommand(FlipVertical);
            _menuClicksBound = true;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _loadedPltBytes = restype == ResourceType.PLT ? data?.ToArray() : null;
            _tpc = restype == ResourceType.PLT
                ? ReadPltPreviewAsTpc(data)
                : IsCommonBitmapType(restype)
                ? ReadCommonBitmapAsTpc(data)
                : TPCAuto.ReadTpc(data);
            LoadTPC(_tpc);
            if (restype == ResourceType.PLT && _txiEdit != null)
            {
                _txiEdit.Text = "PLT preview: color index rendered as greyscale; palette group bytes are not applied. PLT is an NWN/Aurora format and is not used by KotOR.";
            }
        }

        /// <summary>
        /// Loads TPC into editor state and refreshes the texture preview, properties, mipmap selector, and TXI text.
        /// </summary>
        private void LoadTPC(TPC tpc)
        {
            _tpc = tpc ?? _tpc;
            RefreshProperties();
            RefreshPreview();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            if (_restype == ResourceType.PLT && _loadedPltBytes != null)
            {
                return Tuple.Create(_loadedPltBytes.ToArray(), new byte[0]);
            }

            ApplyTxiText();
            ResourceType tpcType = GetBuildResourceType(_restype);
            byte[] data = TPCAuto.BytesTpc(_tpc, tpcType);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _loadedPltBytes = null;
            _tpc = TPC.FromBlank();
            LoadTPC(_tpc);
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            var storage = StorageProvider;
            if (storage == null) return;
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "texture";
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".tpc",
                FileTypeChoices = CreateSaveFileTypeChoices()
            };
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string ext = (Path.GetExtension(path) ?? "").TrimStart('.').ToLowerInvariant();
            _restype = ResourceType.FromExtension(ext) ?? ResourceType.TPC;
            _resname = Path.GetFileNameWithoutExtension(path);
            RefreshWindowTitle();
            Save();
        }

        public int TextureWidth => _tpc?.Dimensions().width ?? 0;
        public int TextureHeight => _tpc?.Dimensions().height ?? 0;
        public int LayerCount => _tpc?.Layers?.Count ?? 0;
        public int MipmapCount => _tpc?.Layers?.FirstOrDefault()?.Mipmaps?.Count ?? 0;
        public string TextureFormatName => _tpc?.Format().ToString() ?? TPCTextureFormat.Invalid.ToString();
        public string TxiText => _txiEdit?.Text ?? _tpc?.Txi ?? "";
        public float AlphaTestValue => _tpc?.AlphaTest ?? 0f;

        internal NumericUpDown AlphaTestBoxForTests => _alphaTestBox;

        public static FilePickerFileType[] CreateSaveFileTypeChoices()
        {
            return new[]
            {
                new FilePickerFileType("Texture (TPC)") { Patterns = new[] { "*.tpc" } },
                new FilePickerFileType("TGA") { Patterns = new[] { "*.tga" } },
                new FilePickerFileType("DDS") { Patterns = new[] { "*.dds" } },
                new FilePickerFileType("BMP") { Patterns = new[] { "*.bmp" } },
                new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
            };
        }

        private void RefreshProperties()
        {
            if (_tpc == null)
            {
                return;
            }

            _syncingUi = true;
            var dimensions = _tpc.Dimensions();
            SetText(_dimensionsValue, dimensions.width + " x " + dimensions.height);
            SetText(_formatValue, _tpc.Format().ToString());
            SetText(_layersValue, LayerCount.ToString());
            SetText(_mipmapsValue, MipmapCount.ToString());
            SetText(_compressedValue, _tpc.IsCompressed() ? "Yes" : "No");
            SetText(_animatedValue, _tpc.IsAnimated ? "Yes" : "No");
            SetText(_cubeMapValue, _tpc.IsCubeMap ? "Yes" : "No");
            SetText(_alphaValue, _tpc.AlphaTest.ToString("0.##"));
            if (_alphaTestBox != null)
            {
                _alphaTestBox.Value = (decimal)Math.Max(0f, Math.Min(1f, _tpc.AlphaTest));
            }

            if (_mipmapBox != null)
            {
                _mipmapBox.Minimum = 0;
                _mipmapBox.Maximum = Math.Max(0, MipmapCount - 1);
                if (_mipmapBox.Value > _mipmapBox.Maximum)
                {
                    _mipmapBox.Value = _mipmapBox.Maximum;
                }
            }

            if (_txiEdit != null)
            {
                _txiEdit.Text = _tpc.Txi ?? "";
            }
            _syncingUi = false;
        }

        private void RefreshPreview()
        {
            if (_syncingUi || _tpc == null || _textureImage == null)
            {
                return;
            }

            if (_zoomPercentLabel != null && _zoomSlider != null)
            {
                _zoomPercentLabel.Text = ((int)Math.Round(_zoomSlider.Value)).ToString() + "%";
            }

            if (!_textureImage.IsEffectivelyVisible)
            {
                return;
            }

            var mipmapIndex = GetSelectedMipmapIndex();
            var bitmap = CreateBitmapForMipmap(_tpc, 0, mipmapIndex);
            _textureImage.Source = bitmap;
            if (bitmap == null)
            {
                if (_previewMessage != null)
                {
                    _previewMessage.Text = "Texture preview unavailable";
                    _previewMessage.IsVisible = true;
                }
                return;
            }

            var zoom = (_zoomSlider?.Value ?? 100) / 100.0;
            var mipmap = _tpc.Get(0, mipmapIndex);
            _textureImage.Width = Math.Max(1, mipmap.Width * zoom);
            _textureImage.Height = Math.Max(1, mipmap.Height * zoom);
            if (_previewMessage != null)
            {
                _previewMessage.IsVisible = false;
            }
        }

        private int GetSelectedMipmapIndex()
        {
            var value = _mipmapBox?.Value ?? 0;
            var index = (int)Math.Round((double)value);
            return Math.Max(0, Math.Min(Math.Max(0, MipmapCount - 1), index));
        }

        private void AdjustZoom(double delta)
        {
            if (_zoomSlider == null)
            {
                return;
            }
            _zoomSlider.Value = Math.Max(_zoomSlider.Minimum, Math.Min(_zoomSlider.Maximum, _zoomSlider.Value + delta));
        }

        private void ApplyTxiText()
        {
            if (_syncingUi || _tpc == null || _txiEdit == null)
            {
                return;
            }

            var newText = _txiEdit.Text ?? "";
            if (!string.Equals(_tpc.Txi ?? "", newText, StringComparison.Ordinal))
            {
                _tpc.Txi = newText;
                MarkDocumentDirty();
            }
        }

        private void ApplyAlphaTestValue()
        {
            if (_syncingUi || _tpc == null || _alphaTestBox == null)
            {
                return;
            }

            float value = (float)(_alphaTestBox.Value ?? 0);
            value = Math.Max(0f, Math.Min(1f, value));
            if (Math.Abs(_tpc.AlphaTest - value) < 0.0001f)
            {
                return;
            }

            _tpc.AlphaTest = value;
            SetText(_alphaValue, _tpc.AlphaTest.ToString("0.##"));
            MarkDocumentDirty();
        }

        internal async Task<bool> CopyTextureToClipboardAsync()
        {
            if (_tpc == null || LayerCount == 0 || MipmapCount == 0)
            {
                return false;
            }

            var clipboard = Clipboard;
            if (clipboard == null)
            {
                return false;
            }

            ApplyTxiText();
            byte[] bytes = TPCAuto.BytesTpc(_tpc, ResourceType.DDS);
            await clipboard.SetTextAsync(ClipboardTexturePrefix + Convert.ToBase64String(bytes));
            return true;
        }

        internal async Task<bool> PasteTextureFromClipboardAsync()
        {
            var clipboard = Clipboard;
            if (clipboard == null)
            {
                return false;
            }

            string text = await clipboard.GetTextAsync();
            if (string.IsNullOrWhiteSpace(text) ||
                !text.StartsWith(ClipboardTexturePrefix, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(text.Substring(ClipboardTexturePrefix.Length));
            }
            catch (FormatException)
            {
                return false;
            }

            var pasted = TPCAuto.ReadTpc(bytes);
            _loadedPltBytes = null;
            _restype = ResourceType.TPC;
            LoadTPC(pasted);
            MarkDocumentDirty();
            return true;
        }

        private void RotateLeft()
        {
            TransformTexture(RotateMipmapLeft);
        }

        private void RotateRight()
        {
            TransformTexture(RotateMipmapRight);
        }

        private void FlipHorizontal()
        {
            TransformTexture(FlipMipmapHorizontal);
        }

        private void FlipVertical()
        {
            TransformTexture(FlipMipmapVertical);
        }

        private void TransformTexture(Func<TPCMipmap, TPCMipmap> transform)
        {
            if (_tpc == null || transform == null || _restype == ResourceType.PLT)
            {
                return;
            }

            if (_tpc.Format().IsDxt())
            {
                _tpc.Convert(TPCTextureFormat.RGBA);
            }

            foreach (var layer in _tpc.Layers ?? Enumerable.Empty<TPCLayer>())
            {
                if (layer?.Mipmaps == null)
                {
                    continue;
                }

                for (int i = 0; i < layer.Mipmaps.Count; i++)
                {
                    var transformed = transform(layer.Mipmaps[i]);
                    if (transformed == null)
                    {
                        return;
                    }
                    layer.Mipmaps[i] = transformed;
                }
            }

            _loadedPltBytes = null;
            RefreshProperties();
            RefreshPreview();
            MarkDocumentDirty();
        }

        private static void SetText(TextBlock block, string text)
        {
            if (block != null)
            {
                block.Text = text;
            }
        }

        public static Bitmap CreateBitmapForMipmap(TPC tpc, int layerIndex, int mipmapIndex)
        {
            if (tpc == null || tpc.Layers == null || layerIndex < 0 || layerIndex >= tpc.Layers.Count)
            {
                return null;
            }

            var layer = tpc.Layers[layerIndex];
            if (layer.Mipmaps == null || mipmapIndex < 0 || mipmapIndex >= layer.Mipmaps.Count)
            {
                return null;
            }

            var mipmap = layer.Mipmaps[mipmapIndex];
            var bgra = DecodeMipmapToBgra(mipmap);
            if (bgra == null || bgra.Length < mipmap.Width * mipmap.Height * 4)
            {
                return null;
            }

            var bitmap = new WriteableBitmap(
                new PixelSize(mipmap.Width, mipmap.Height),
                new Vector(96, 96),
                PixelFormat.Bgra8888,
                AlphaFormat.Premul);

            using (var framebuffer = bitmap.Lock())
            {
                Marshal.Copy(bgra, 0, framebuffer.Address, bgra.Length);
            }

            return bitmap;
        }

        public static byte[] DecodeMipmapToBgra(TPCMipmap mipmap)
        {
            if (mipmap == null || mipmap.Data == null || mipmap.Width <= 0 || mipmap.Height <= 0)
            {
                return null;
            }

            if (mipmap.TpcFormat.IsDxt())
            {
                var single = new TPC();
                single.SetSingle(mipmap.Data.ToArray(), mipmap.TpcFormat, mipmap.Width, mipmap.Height);
                single.Convert(TPCTextureFormat.RGBA);
                return DecodeMipmapToBgra(single.Get(0, 0));
            }

            int pixels = mipmap.Width * mipmap.Height;
            var output = new byte[pixels * 4];
            var data = mipmap.Data;
            for (int i = 0; i < pixels; i++)
            {
                int dst = i * 4;
                if (mipmap.TpcFormat == TPCTextureFormat.RGBA)
                {
                    int src = i * 4;
                    output[dst] = data[src + 2];
                    output[dst + 1] = data[src + 1];
                    output[dst + 2] = data[src];
                    output[dst + 3] = data[src + 3];
                }
                else if (mipmap.TpcFormat == TPCTextureFormat.BGRA)
                {
                    int src = i * 4;
                    output[dst] = data[src];
                    output[dst + 1] = data[src + 1];
                    output[dst + 2] = data[src + 2];
                    output[dst + 3] = data[src + 3];
                }
                else if (mipmap.TpcFormat == TPCTextureFormat.RGB)
                {
                    int src = i * 3;
                    output[dst] = data[src + 2];
                    output[dst + 1] = data[src + 1];
                    output[dst + 2] = data[src];
                    output[dst + 3] = 255;
                }
                else if (mipmap.TpcFormat == TPCTextureFormat.BGR)
                {
                    int src = i * 3;
                    output[dst] = data[src];
                    output[dst + 1] = data[src + 1];
                    output[dst + 2] = data[src + 2];
                    output[dst + 3] = 255;
                }
                else if (mipmap.TpcFormat == TPCTextureFormat.Greyscale)
                {
                    byte grey = data[i];
                    output[dst] = grey;
                    output[dst + 1] = grey;
                    output[dst + 2] = grey;
                    output[dst + 3] = 255;
                }
                else
                {
                    return null;
                }
            }

            return output;
        }

        public static TPCMipmap RotateMipmapLeft(TPCMipmap mipmap)
        {
            return TransformMipmap(mipmap, mipmap.Height, mipmap.Width, (x, y, sourceWidth, sourceHeight) =>
            {
                return (sourceWidth - 1 - y, x);
            });
        }

        public static TPCMipmap RotateMipmapRight(TPCMipmap mipmap)
        {
            return TransformMipmap(mipmap, mipmap.Height, mipmap.Width, (x, y, sourceWidth, sourceHeight) =>
            {
                return (y, sourceHeight - 1 - x);
            });
        }

        public static TPCMipmap FlipMipmapHorizontal(TPCMipmap mipmap)
        {
            return TransformMipmap(mipmap, mipmap.Width, mipmap.Height, (x, y, sourceWidth, sourceHeight) =>
            {
                return (sourceWidth - 1 - x, y);
            });
        }

        public static TPCMipmap FlipMipmapVertical(TPCMipmap mipmap)
        {
            return TransformMipmap(mipmap, mipmap.Width, mipmap.Height, (x, y, sourceWidth, sourceHeight) =>
            {
                return (x, sourceHeight - 1 - y);
            });
        }

        private static TPCMipmap TransformMipmap(
            TPCMipmap mipmap,
            int targetWidth,
            int targetHeight,
            Func<int, int, int, int, (int sourceX, int sourceY)> mapSource)
        {
            if (mipmap == null || mipmap.Data == null || mipmap.Width <= 0 || mipmap.Height <= 0 || mapSource == null)
            {
                return null;
            }

            if (mipmap.TpcFormat.IsDxt())
            {
                return null;
            }

            int bytesPerPixel = mipmap.TpcFormat.BytesPerPixel();
            int expectedLength = mipmap.Width * mipmap.Height * bytesPerPixel;
            if (bytesPerPixel <= 0 || mipmap.Data.Length < expectedLength)
            {
                return null;
            }

            var output = new byte[targetWidth * targetHeight * bytesPerPixel];
            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    var source = mapSource(x, y, mipmap.Width, mipmap.Height);
                    if (source.sourceX < 0 || source.sourceX >= mipmap.Width || source.sourceY < 0 || source.sourceY >= mipmap.Height)
                    {
                        return null;
                    }

                    int sourceOffset = (source.sourceY * mipmap.Width + source.sourceX) * bytesPerPixel;
                    int targetOffset = (y * targetWidth + x) * bytesPerPixel;
                    Buffer.BlockCopy(mipmap.Data, sourceOffset, output, targetOffset, bytesPerPixel);
                }
            }

            return new TPCMipmap(targetWidth, targetHeight, mipmap.TpcFormat, output);
        }

        private static bool IsCommonBitmapType(ResourceType restype)
        {
            return restype == ResourceType.PNG ||
                   restype == ResourceType.JPG ||
                   restype == ResourceType.BMP;
        }

        public static ResourceType GetBuildResourceType(ResourceType restype)
        {
            if (restype == ResourceType.PNG || restype == ResourceType.JPG)
            {
                return ResourceType.TPC;
            }

            return restype ?? ResourceType.TPC;
        }

        private static TPC ReadPltPreviewAsTpc(byte[] data)
        {
            if (data == null || data.Length < 24)
            {
                throw new ArgumentException("PLT data is too short.", nameof(data));
            }

            string signature = System.Text.Encoding.ASCII.GetString(data, 0, 4);
            string version = System.Text.Encoding.ASCII.GetString(data, 4, 4);
            if (signature != "PLT " || version != "V1  ")
            {
                throw new ArgumentException("Invalid PLT header.");
            }

            uint width = BitConverter.ToUInt32(data, 16);
            uint height = BitConverter.ToUInt32(data, 20);
            if (width == 0 || height == 0 || width > 16384 || height > 16384)
            {
                throw new ArgumentException("Invalid PLT dimensions.");
            }

            long pixelCount = (long)width * height;
            if (pixelCount > int.MaxValue / 4)
            {
                throw new ArgumentException("PLT dimensions are too large to preview.");
            }

            long requiredLength = 24 + pixelCount * 2;
            if (data.LongLength < requiredLength)
            {
                throw new ArgumentException("PLT pixel data is truncated.");
            }

            var rgba = new byte[pixelCount * 4];
            for (long i = 0; i < pixelCount; i++)
            {
                byte colorIndex = data[24 + i * 2];
                int dst = (int)(i * 4);
                rgba[dst] = colorIndex;
                rgba[dst + 1] = colorIndex;
                rgba[dst + 2] = colorIndex;
                rgba[dst + 3] = 255;
            }

            var tpc = new TPC();
            tpc.SetSingle(rgba, TPCTextureFormat.RGBA, (int)width, (int)height);
            return tpc;
        }

        private static TPC ReadCommonBitmapAsTpc(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Texture image data is empty.", nameof(data));
            }

            using (var stream = new MemoryStream(data))
            using (var bitmap = WriteableBitmap.Decode(stream))
            {
                if (bitmap == null)
                {
                    throw new ArgumentException("Unable to decode texture image data.", nameof(data));
                }

                var rgba = ExtractRgba(bitmap);
                var tpc = new TPC();
                tpc.SetSingle(rgba, TPCTextureFormat.RGBA, bitmap.PixelSize.Width, bitmap.PixelSize.Height);
                return tpc;
            }
        }

        private static unsafe byte[] ExtractRgba(WriteableBitmap bitmap)
        {
            var width = bitmap.PixelSize.Width;
            var height = bitmap.PixelSize.Height;
            var rgba = new byte[width * height * 4];

            using (var lockedBitmap = bitmap.Lock())
            {
                byte* pixelPtr = (byte*)lockedBitmap.Address;
                int rowStride = lockedBitmap.RowBytes;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int sourceIndex = y * rowStride + x * 4;
                        int targetIndex = (y * width + x) * 4;

                        rgba[targetIndex] = pixelPtr[sourceIndex];
                        rgba[targetIndex + 1] = pixelPtr[sourceIndex + 1];
                        rgba[targetIndex + 2] = pixelPtr[sourceIndex + 2];
                        rgba[targetIndex + 3] = pixelPtr[sourceIndex + 3];
                    }
                }
            }

            return rgba;
        }

        private sealed class EditorActionCommand : ICommand
        {
            private readonly Action _execute;

            public EditorActionCommand(Action execute)
            {
                _execute = execute;
            }

            public event EventHandler CanExecuteChanged
            {
                add { }
                remove { }
            }

            public bool CanExecute(object parameter)
            {
                return _execute != null;
            }

            public void Execute(object parameter)
            {
                _execute?.Invoke();
            }
        }
    }
}
