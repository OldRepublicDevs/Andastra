using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using JetBrains.Annotations;

namespace HolocronToolset.Widgets
{
    /// <summary>
    /// Industry-standard media player widget with comprehensive controls and sleek modern design.
    ///
    /// Features:
    /// - Play/pause, stop, and mute controls
    /// - Volume control with slider
    /// - Playback speed control (0.25x - 2.0x)
    /// - Time slider with seeking
    /// - Keyboard shortcuts (Space, S, M, Left/Right arrows, Up/Down arrows, [ ])
    /// - Real-time position updates
    /// - Modern, sleek UI design inspired by React components
    /// - Cross-platform compatible
    ///
    /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:26
    /// </summary>
    public partial class MediaPlayerWidget : UserControl, INotifyPropertyChanged
    {
        private Button _playPauseButton;
        private Button _stopButton;
        private Slider _timeSlider;
        private TextBlock _timeLabel;
        private Button _muteButton;
        private Slider _volumeSlider;
        private Button _speedButton;

        private bool _isPlaying;
        private bool _isMuted;
        private double _volume = 0.75;
        private double _playbackSpeed = 1.0;
        private bool _isSeeking;
        private TimeSpan _currentPosition;
        private TimeSpan _duration;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:54-55
        // Original: self.player: QMediaPlayer = QMediaPlayer(self)
        // MediaPlayer interface for actual playback control
        private IMediaPlayer _player;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:68-69
        // Original: self.speed_levels: list[float] = [0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0]
        private static readonly double[] SpeedLevels = { 0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0 };
        private int _currentSpeedIndex = 3; // Default to 1.0x

        // Events for external integration (matching PyKotor signals)
        public event EventHandler PlaybackStarted;
        public event EventHandler PlaybackPaused;
        public event EventHandler PlaybackStopped;
        public event EventHandler<TimeSpan> PositionChanged;
        public event EventHandler<double> VolumeChanged;
        public event EventHandler<double> PlaybackSpeedChanged;

        public MediaPlayerWidget()
        {
            InitializeComponent();
            SetupControls();
            SetupKeyboardShortcuts();

            // Initialize state
            _isPlaying = false;
            _isMuted = false;
            _volume = 0.75;
            _playbackSpeed = 1.0;
            _currentPosition = TimeSpan.Zero;
            _duration = TimeSpan.Zero;
            _player = null;
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
                // XAML not available - will use programmatic UI
            }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupControls()
        {
            // Find controls from XAML
            _playPauseButton = this.FindControl<Button>("playPauseButton");
            _stopButton = this.FindControl<Button>("stopButton");
            _timeSlider = this.FindControl<Slider>("timeSlider");
            _timeLabel = this.FindControl<TextBlock>("timeLabel");
            _muteButton = this.FindControl<Button>("muteButton");
            _volumeSlider = this.FindControl<Slider>("volumeSlider");
            _speedButton = this.FindControl<Button>("speedButton");

            if (_playPauseButton != null)
            {
                _playPauseButton.Click += (s, e) => TogglePlayPause();
            }
            if (_stopButton != null)
            {
                _stopButton.Click += (s, e) => Stop();
            }
            if (_timeSlider != null)
            {
                _timeSlider.PointerPressed += (s, e) => OnTimeSliderPressed();
                _timeSlider.PointerMoved += (s, e) => OnTimeSliderMoved(e);
                _timeSlider.PointerReleased += (s, e) => OnTimeSliderReleased();
                _timeSlider.ValueChanged += (s, e) => OnTimeSliderValueChanged();
            }
            if (_muteButton != null)
            {
                _muteButton.Click += (s, e) => ToggleMute();
            }
            if (_volumeSlider != null)
            {
                _volumeSlider.ValueChanged += (s, e) => OnVolumeChanged();
            }
            if (_speedButton != null)
            {
                _speedButton.Click += (s, e) => CyclePlaybackSpeed();
            }
        }

        private void SetupProgrammaticUI()
        {
            // Fallback programmatic UI if XAML is not available
            var mainPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Spacing = 4,
                Margin = new Avalonia.Thickness(12, 8)
            };

            var controlsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                VerticalAlignment = VerticalAlignment.Center
            };

            _playPauseButton = new Button
            {
                Content = "▶",
                Width = 40,
                Height = 40,
                FontSize = 18
            };
            _playPauseButton.Click += (s, e) => TogglePlayPause();

            _stopButton = new Button
            {
                Content = "■",
                Width = 36,
                Height = 36,
                FontSize = 14,
                Margin = new Avalonia.Thickness(4, 0, 0, 0)
            };
            _stopButton.Click += (s, e) => Stop();

            _timeSlider = new Slider
            {
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Width = 200
            };
            _timeSlider.ValueChanged += (s, e) => OnTimeSliderValueChanged();

            _timeLabel = new TextBlock
            {
                Text = "00:00 / 00:00",
                MinWidth = 100,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 12
            };

            _muteButton = new Button
            {
                Content = "🔊",
                Width = 32,
                Height = 32,
                FontSize = 14
            };
            _muteButton.Click += (s, e) => ToggleMute();

            _volumeSlider = new Slider
            {
                Minimum = 0,
                Maximum = 100,
                Value = 75,
                Width = 80
            };
            _volumeSlider.ValueChanged += (s, e) => OnVolumeChanged();

            _speedButton = new Button
            {
                Content = "1.0x",
                Width = 50,
                Height = 28,
                FontSize = 11
            };
            _speedButton.Click += (s, e) => CyclePlaybackSpeed();

            controlsPanel.Children.Add(_playPauseButton);
            controlsPanel.Children.Add(_stopButton);
            controlsPanel.Children.Add(_timeSlider);
            controlsPanel.Children.Add(_timeLabel);
            controlsPanel.Children.Add(_muteButton);
            controlsPanel.Children.Add(_volumeSlider);
            controlsPanel.Children.Add(_speedButton);

            mainPanel.Children.Add(controlsPanel);
            Content = mainPanel;
        }

        private void SetupKeyboardShortcuts()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:301-328
            // Keyboard shortcuts are handled in KeyDown event
            KeyDown += OnKeyDown;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:330-348
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsVisible || !IsEnabled)
                return;

            switch (e.Key)
            {
                case Key.Space:
                    TogglePlayPause();
                    e.Handled = true;
                    break;
                case Key.S:
                    Stop();
                    e.Handled = true;
                    break;
                case Key.Left:
                    SeekRelative(e.KeyModifiers.HasFlag(KeyModifiers.Shift) ? -1000 : -5000);
                    e.Handled = true;
                    break;
                case Key.Right:
                    SeekRelative(e.KeyModifiers.HasFlag(KeyModifiers.Shift) ? 1000 : 5000);
                    e.Handled = true;
                    break;
                case Key.Up:
                    VolumeUp();
                    e.Handled = true;
                    break;
                case Key.Down:
                    VolumeDown();
                    e.Handled = true;
                    break;
                case Key.M:
                    ToggleMute();
                    e.Handled = true;
                    break;
                case Key.OemOpenBrackets: // [
                    ChangePlaybackSpeed(-1);
                    e.Handled = true;
                    break;
                case Key.OemCloseBrackets: // ]
                    ChangePlaybackSpeed(1);
                    e.Handled = true;
                    break;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:352-368
        public void TogglePlayPause()
        {
            if (_player == null)
            {
                // Update UI state even without player
                _isPlaying = !_isPlaying;
                UpdatePlayPauseButton();
                return;
            }

            try
            {
                if (_isPlaying)
                {
                    _player.Pause();
                    _isPlaying = false;
                    PlaybackPaused?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    _player.Play();
                    _isPlaying = true;
                    ShowWidget();
                    PlaybackStarted?.Invoke(this, EventArgs.Empty);
                }
                UpdatePlayPauseButton();
            }
            catch (Exception)
            {
                // Handle errors gracefully
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:387-394
        public void Stop()
        {
            if (_player != null)
            {
                try
                {
                    _player.Stop();
                }
                catch
                {
                    // Ignore errors if media player is not in a valid state
                }
            }

            _isPlaying = false;
            _currentPosition = TimeSpan.Zero;

            UpdatePlayPauseButton();
            UpdateTimeSlider();
            UpdateTimeLabel();
            HideWidget();

            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:396-402
        public void SeekRelative(int deltaMs)
        {
            if (_player == null || _duration.TotalMilliseconds <= 0)
                return;

            try
            {
                var currentMs = (int)_currentPosition.TotalMilliseconds;
                var newPosition = Math.Max(0, Math.Min((int)_duration.TotalMilliseconds, currentMs + deltaMs));
                SeekAbsolute(newPosition);
            }
            catch
            {
                // Handle errors gracefully
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:404-409
        public void SeekAbsolute(int positionMs)
        {
            if (_player == null)
                return;

            try
            {
                var position = TimeSpan.FromMilliseconds(positionMs);
                _player.SetPosition(position);
                _currentPosition = position;
                UpdateTimeSlider();
                UpdateTimeLabel();
                PositionChanged?.Invoke(this, position);
            }
            catch
            {
                // Handle errors gracefully
            }
        }

        private void OnTimeSliderPressed()
        {
            _isSeeking = true;
            if (_player != null)
            {
                try
                {
                    _player.Pause();
                }
                catch
                {
                    // Ignore errors
                }
            }
        }

        private void OnTimeSliderMoved(PointerEventArgs e)
        {
            if (!_isSeeking || _timeSlider == null)
                return;

            // Calculate position from pointer position
            var point = e.GetPosition(_timeSlider);
            var ratio = Math.Max(0.0, Math.Min(1.0, point.X / _timeSlider.Bounds.Width));
            var newValue = ratio * _timeSlider.Maximum;
            _timeSlider.Value = newValue;
        }

        private void OnTimeSliderReleased()
        {
            if (!_isSeeking)
                return;

            _isSeeking = false;

            if (_timeSlider != null && _duration.TotalMilliseconds > 0)
            {
                var ratio = _timeSlider.Value / _timeSlider.Maximum;
                var positionMs = (int)(_duration.TotalMilliseconds * ratio);
                SeekAbsolute(positionMs);
            }

            if (_player != null && _isPlaying)
            {
                try
                {
                    _player.Play();
                }
                catch
                {
                    // Ignore errors
                }
            }
        }

        private void OnTimeSliderValueChanged()
        {
            if (_isSeeking && _timeSlider != null && _duration.TotalMilliseconds > 0)
            {
                var ratio = _timeSlider.Value / _timeSlider.Maximum;
                var positionMs = (int)(_duration.TotalMilliseconds * ratio);
                _currentPosition = TimeSpan.FromMilliseconds(positionMs);
                UpdateTimeLabel();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:411-433
        public void ToggleMute()
        {
            if (_player != null)
            {
                try
                {
                    _player.IsMuted = !_player.IsMuted;
                    _isMuted = _player.IsMuted;
                }
                catch
                {
                    // Handle errors gracefully
                    _isMuted = !_isMuted;
                }
            }
            else
            {
                _isMuted = !_isMuted;
            }

            UpdateMuteButton();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:448-483
        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = Math.Max(0.0, Math.Min(1.0, value));
                if (_player != null)
                {
                    try
                    {
                        _player.Volume = _volume;
                    }
                    catch
                    {
                        // Handle errors gracefully
                    }
                }

                if (_volumeSlider != null)
                {
                    _volumeSlider.Value = _volume * 100;
                }

                if (_volume > 0)
                {
                    _isMuted = false;
                    UpdateMuteButton();
                }

                VolumeChanged?.Invoke(this, _volume);
                OnPropertyChanged();
            }
        }

        private void OnVolumeChanged()
        {
            if (_volumeSlider != null)
            {
                Volume = _volumeSlider.Value / 100.0;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:492-500
        public void VolumeUp()
        {
            Volume = Math.Min(1.0, _volume + 0.05);
        }

        public void VolumeDown()
        {
            Volume = Math.Max(0.0, _volume - 0.05);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:508-533
        public void ChangePlaybackSpeed(int direction)
        {
            _currentSpeedIndex = Math.Max(0, Math.Min(SpeedLevels.Length - 1, _currentSpeedIndex + direction));
            SetPlaybackSpeed(SpeedLevels[_currentSpeedIndex]);
        }

        public void CyclePlaybackSpeed()
        {
            _currentSpeedIndex = (_currentSpeedIndex + 1) % SpeedLevels.Length;
            SetPlaybackSpeed(SpeedLevels[_currentSpeedIndex]);
        }

        public void SetPlaybackSpeed(double rate)
        {
            // Find closest speed level
            var closestIndex = 0;
            var minDiff = Math.Abs(SpeedLevels[0] - rate);
            for (var i = 1; i < SpeedLevels.Length; i++)
            {
                var diff = Math.Abs(SpeedLevels[i] - rate);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    closestIndex = i;
                }
            }

            _currentSpeedIndex = closestIndex;
            _playbackSpeed = SpeedLevels[_currentSpeedIndex];

            if (_player != null)
            {
                try
                {
                    var wasPlaying = _isPlaying;
                    _player.PlaybackRate = _playbackSpeed;
                    if (wasPlaying && !_isPlaying)
                    {
                        _player.Play();
                        _isPlaying = true;
                    }
                }
                catch
                {
                    // Handle errors gracefully
                }
            }

            if (_speedButton != null)
            {
                _speedButton.Content = $"{_playbackSpeed:F2}x";
            }

            PlaybackSpeedChanged?.Invoke(this, _playbackSpeed);
            OnPropertyChanged(nameof(PlaybackSpeed));
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:592-603
        public string FormatTime(TimeSpan time)
        {
            var totalSeconds = (int)time.TotalSeconds;
            var hours = totalSeconds / 3600;
            var minutes = (totalSeconds % 3600) / 60;
            var seconds = totalSeconds % 60;

            if (hours > 0)
            {
                return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
            }
            return $"{minutes:D2}:{seconds:D2}";
        }

        public void UpdatePosition(TimeSpan position)
        {
            if (!_isSeeking)
            {
                _currentPosition = position;
                UpdateTimeSlider();
                UpdateTimeLabel();
                PositionChanged?.Invoke(this, position);
            }
        }

        public void UpdateDuration(TimeSpan duration)
        {
            _duration = duration;
            if (_timeSlider != null)
            {
                _timeSlider.Maximum = duration.TotalMilliseconds > 0 ? duration.TotalMilliseconds : 100;
            }
            UpdateTimeLabel();
        }

        private void UpdatePlayPauseButton()
        {
            if (_playPauseButton != null)
            {
                _playPauseButton.Content = _isPlaying ? "⏸" : "▶";
            }
        }

        private void UpdateMuteButton()
        {
            if (_muteButton != null)
            {
                _muteButton.Content = _isMuted ? "🔇" : "🔊";
            }
        }

        private void UpdateTimeSlider()
        {
            if (_timeSlider != null && _duration.TotalMilliseconds > 0 && !_isSeeking)
            {
                var ratio = _currentPosition.TotalMilliseconds / _duration.TotalMilliseconds;
                _timeSlider.Value = Math.Max(0, Math.Min(_timeSlider.Maximum, ratio * _timeSlider.Maximum));
            }
        }

        private void UpdateTimeLabel()
        {
            if (_timeLabel != null)
            {
                var currentTime = FormatTime(_currentPosition);
                var totalTime = FormatTime(_duration);
                _timeLabel.Text = $"{currentTime} / {totalTime}";
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:605-614
        public void ShowWidget()
        {
            IsVisible = true;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:616-625
        public void HideWidget()
        {
            IsVisible = false;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:54-55
        public IMediaPlayer Player
        {
            get { return _player; }
            set
            {
                _player = value;
                if (_player != null)
                {
                    // Sync volume and other properties
                    try
                    {
                        _player.Volume = _volume;
                        _player.PlaybackRate = _playbackSpeed;
                        _player.IsMuted = _isMuted;
                    }
                    catch
                    {
                        // Handle errors gracefully
                    }
                }
                OnPropertyChanged();
            }
        }

        public double PlaybackSpeed => _playbackSpeed;
        public bool IsPlaying => _isPlaying;
        public bool IsMuted => _isMuted;
        public TimeSpan CurrentPosition => _currentPosition;
        public TimeSpan Duration => _duration;

        public new event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:54-55
    // Original: QMediaPlayer interface
    /// <summary>
    /// Interface for media player functionality to allow different implementations.
    /// This abstraction allows the widget to work with any media player implementation.
    /// </summary>
    public interface IMediaPlayer
    {
        void Stop();
        void Play();
        void Pause();
        void SetPosition(TimeSpan position);
        TimeSpan Position { get; }
        TimeSpan Duration { get; }
        double Volume { get; set; }
        bool IsMuted { get; set; }
        double PlaybackRate { get; set; }
    }
}
