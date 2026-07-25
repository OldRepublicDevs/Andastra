using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Widgets;
using Window = Avalonia.Controls.Window;

namespace OdyTools.Editors
{
    /// <summary>
    /// Audio editor for WAV/MP3 resources. Uses MediaPlayerWidget with NAudio-backed
    /// playback from a temp file. Professional layout with format/size info and full transport controls.
    /// </summary>
    public partial class OdyToolWAV : Editor
    {
        private byte[] _audioData = Array.Empty<byte>();
        private string _tempFile;
        private string _detectedFormat = "Unknown";
        private NAudioMediaPlayer _mediaPlayer;
        private DispatcherTimer _positionTimer;
        private MediaPlayerWidget _mediaPlayerWidget;
        private TextBlock _formatLabel;
        private TextBlock _sizeLabel;
        private TextBlock _pathLabel;

        public byte[] AudioData => _audioData;
        public string DetectedFormat => _detectedFormat;
        public string TempFile => _tempFile;

        public OdyToolWAVUi Ui { get; private set; }

        public OdyToolWAV() : this(null, null) { }
        public OdyToolWAV(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolWAV", "audio",
                new[] { ResourceType.WAV, ResourceType.MP3, ResourceType.OGG, ResourceType.WMA, ResourceType.WMV, ResourceType.XMV, ResourceType.FLAC, ResourceType.BMU },
                new[] { ResourceType.WAV, ResourceType.MP3, ResourceType.OGG, ResourceType.WMA, ResourceType.WMV, ResourceType.XMV, ResourceType.FLAC, ResourceType.BMU },
                installation)
        {
            InitializeComponent();
            MinWidth = 560;
            MinHeight = 220;
            New();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
            }
            catch
            {
                // XAML not available
            }

            if (xamlLoaded)
            {
                _mediaPlayerWidget = EditorHelpers.FindControlSafe<MediaPlayerWidget>(this, "mediaPlayerWidget");
                _formatLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "formatLabel");
                _sizeLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "sizeLabel");
                _pathLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "pathLabel");
            }

            if (_mediaPlayerWidget == null)
                BuildFallbackUi();

            _mediaPlayer = new NAudioMediaPlayer();
            _mediaPlayer.PlaybackStopped += OnPlaybackStopped;
            if (_mediaPlayerWidget != null)
            {
                _mediaPlayerWidget.Player = _mediaPlayer;
                _mediaPlayerWidget.PlaybackStarted += (s, e) => _positionTimer.Start();
                _mediaPlayerWidget.PlaybackPaused += (s, e) => _positionTimer.Stop();
                _mediaPlayerWidget.PlaybackStopped += (s, e) => _positionTimer.Stop();
            }
            _positionTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _positionTimer.Tick += OnPositionTick;

            Ui = new OdyToolWAVUi
            {
                PlayButton = null,
                PauseButton = null,
                StopButton = null,
                TimeSlider = null,
                CurrentTimeLabel = null,
                TotalTimeLabel = null,
                FormatLabel = _formatLabel
            };
        }

        private void BuildFallbackUi()
        {
            var dock = new DockPanel();
            var infoBar = new Border
            {
                Background = Avalonia.Media.Brushes.Transparent,
                Padding = new Avalonia.Thickness(16, 12),
                Child = new StackPanel
                {
                    Children =
                    {
                        (_formatLabel = new TextBlock { Text = "Format: —" }),
                        (_pathLabel = new TextBlock { Text = "No file loaded", Opacity = 0.7 })
                    }
                }
            };
            DockPanel.SetDock(infoBar, Dock.Top);
            dock.Children.Add(infoBar);
            _mediaPlayerWidget = new MediaPlayerWidget();
            dock.Children.Add(_mediaPlayerWidget);
            _sizeLabel = new TextBlock();
            Content = dock;
        }

        private void OnPlaybackStopped(object sender, EventArgs e)
        {
            Dispatcher.UIThread.Post(() =>
            {
                _positionTimer.Stop();
                _mediaPlayerWidget?.UpdatePosition(TimeSpan.Zero);
            });
        }

        private void OnPositionTick(object sender, EventArgs e)
        {
            if (_mediaPlayer == null || _mediaPlayerWidget == null) return;
            try
            {
                var pos = _mediaPlayer.Position;
                _mediaPlayerWidget.UpdatePosition(pos);
                if (pos >= _mediaPlayer.Duration && _mediaPlayer.Duration.TotalSeconds > 0)
                    _positionTimer.Stop();
            }
            catch { }
        }

        public static string DetectAudioFormat(byte[] data)
        {
            if (data == null || data.Length < 4) return ".wav";
            if (data.Length >= 3 && data[0] == (byte)'I' && data[1] == (byte)'D' && data[2] == (byte)'3') return ".mp3";
            if (data.Length >= 2 && data[0] == 0xFF && (data[1] & 0xE0) == 0xE0) return ".mp3";
            if (data.Length >= 4 && data[0] == (byte)'L' && data[1] == (byte)'A' && data[2] == (byte)'M' && data[3] == (byte)'E') return ".mp3";
            if (data.Length >= 4 && data[0] == (byte)'R' && data[1] == (byte)'I' && data[2] == (byte)'F' && data[3] == (byte)'F') return ".wav";
            if (data.Length >= 4 && data[0] == (byte)'O' && data[1] == (byte)'g' && data[2] == (byte)'g' && data[3] == (byte)'S') return ".ogg";
            if (data.Length >= 4 && data[0] == 0x30 && data[1] == 0x26 && data[2] == 0xB2 && data[3] == 0x75) return ".wma";
            if (data.Length >= 4 && data[0] == (byte)'f' && data[1] == (byte)'L' && data[2] == (byte)'a' && data[3] == (byte)'C') return ".flac";
            return ".wav";
        }

        public static string GetFormatName(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return "Unknown";
            switch (extension.ToLowerInvariant())
            {
                case ".wav": return "WAV (RIFF)";
                case ".mp3": return "MP3";
                case ".ogg": return "OGG Vorbis";
                case ".wma": return "Windows Media Audio";
                case ".wmv": return "Windows Media Video";
                case ".xmv": return "Xbox Media Video";
                case ".flac": return "FLAC";
                case ".bmu": return "BMU (obfuscated MP3)";
                default: return "Unknown";
            }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            var dataBytes = data ?? Array.Empty<byte>();
            _audioData = dataBytes;
            _detectedFormat = restype == ResourceType.BMU || restype == ResourceType.WMV || restype == ResourceType.XMV
                ? GetFormatName("." + restype.Extension)
                : GetFormatName(DetectAudioFormat(dataBytes));

            EnsureTempFile(_restype);
            if (!string.IsNullOrEmpty(_tempFile))
            {
                _mediaPlayer.SetSource(_tempFile);
                var duration = _mediaPlayer.Duration;
                _mediaPlayerWidget.UpdateDuration(duration);
            }
            else
            {
                _mediaPlayer.SetSource(null);
                _mediaPlayerWidget.UpdateDuration(TimeSpan.Zero);
            }

            UpdateInfoLabels();
        }

        private void EnsureTempFile(ResourceType restype = null)
        {
            CleanupTempFile();
            if (_audioData == null || _audioData.Length == 0) return;
            var ext = GetPlaybackExtension(_audioData, restype);
            try
            {
                _tempFile = Path.Combine(Path.GetTempPath(), "OdyTools_" + Guid.NewGuid().ToString("N") + ext);
                File.WriteAllBytes(_tempFile, _audioData);
            }
            catch
            {
                _tempFile = null;
            }
        }

        private void UpdateInfoLabels()
        {
            if (_formatLabel != null)
                _formatLabel.Text = "Format: " + _detectedFormat;
            if (_sizeLabel != null)
                _sizeLabel.Text = _audioData != null && _audioData.Length > 0
                    ? FormatByteCount(_audioData.Length)
                    : "";
            if (_pathLabel != null)
                _pathLabel.Text = string.IsNullOrEmpty(Filepath) ? "No file loaded" : (Filepath + " — " + (_resname ?? "") + (_restype != null ? "." + _restype.Extension : ""));
        }

        private static string FormatByteCount(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("F1") + " KB";
            return (bytes / (1024.0 * 1024.0)).ToString("F2") + " MB";
        }

        public override Tuple<byte[], byte[]> Build()
        {
            return Tuple.Create(_audioData ?? Array.Empty<byte>(), new byte[0]);
        }

        public override void New()
        {
            base.New();
            _audioData = Array.Empty<byte>();
            _detectedFormat = "Unknown";
            CleanupTempFile();
            _mediaPlayer?.SetSource(null);
            _mediaPlayerWidget?.UpdateDuration(TimeSpan.Zero);
            _mediaPlayerWidget?.UpdatePosition(TimeSpan.Zero);
            UpdateInfoLabels();
        }

        public void CleanupTempFile()
        {
            if (string.IsNullOrEmpty(_tempFile)) return;
            try
            {
                if (File.Exists(_tempFile))
                    File.Delete(_tempFile);
            }
            catch { }
            _tempFile = null;
        }

        public static string GetPlaybackExtension(byte[] data, ResourceType restype)
        {
            if (restype == ResourceType.BMU ||
                restype == ResourceType.WMV ||
                restype == ResourceType.XMV ||
                restype == ResourceType.WMA ||
                restype == ResourceType.FLAC ||
                restype == ResourceType.OGG ||
                restype == ResourceType.MP3 ||
                restype == ResourceType.WAV)
            {
                return "." + restype.Extension;
            }

            return DetectAudioFormat(data);
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            var storage = StorageProvider;
            if (storage == null) return;
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "sound";
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".wav",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("WAV") { Patterns = new[] { "*.wav" } },
                    new FilePickerFileType("MP3") { Patterns = new[] { "*.mp3" } },
                    new FilePickerFileType("OGG Vorbis") { Patterns = new[] { "*.ogg" } },
                    new FilePickerFileType("WMA/WMV/XMV") { Patterns = new[] { "*.wma", "*.wmv", "*.xmv" } },
                    new FilePickerFileType("FLAC") { Patterns = new[] { "*.flac" } },
                    new FilePickerFileType("BMU") { Patterns = new[] { "*.bmu" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string ext = (Path.GetExtension(path) ?? "").TrimStart('.').ToLowerInvariant();
            _restype = ResourceType.FromExtension(ext) ?? ResourceType.WAV;
            _resname = Path.GetFileNameWithoutExtension(path);
            RefreshWindowTitle();
            Save();
        }

        protected override void OnClosing(WindowClosingEventArgs e)
        {
            _positionTimer?.Stop();
            try { _mediaPlayer?.Stop(); } catch { }
            try { (_mediaPlayer as IDisposable)?.Dispose(); } catch { }
            CleanupTempFile();
            base.OnClosing(e);
        }

        public void OnDurationChanged(long duration)
        {
            _mediaPlayerWidget?.UpdateDuration(TimeSpan.FromMilliseconds(duration));
        }

        public void OnPositionChanged(long position)
        {
            _mediaPlayerWidget?.UpdatePosition(TimeSpan.FromMilliseconds(position));
        }
    }

    public partial class OdyToolWAVUi
    {
        public Button PlayButton { get; set; }
        public Button PauseButton { get; set; }
        public Button StopButton { get; set; }
        public Slider TimeSlider { get; set; }
        public TextBlock CurrentTimeLabel { get; set; }
        public TextBlock TotalTimeLabel { get; set; }
        public TextBlock FormatLabel { get; set; }
    }
}
