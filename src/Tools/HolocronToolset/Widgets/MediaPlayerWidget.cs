using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace HolocronToolset.Widgets
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:26
    // Original: class MediaPlayerWidget(QWidget):
    public partial class MediaPlayerWidget : UserControl
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
        private double _volume;
        private double _playbackSpeed;
        
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:54-55
        // Original: self.player: QMediaPlayer = QMediaPlayer(self)
        // MediaPlayer interface for actual playback control
        // Note: Avalonia doesn't have a built-in MediaPlayer, so this will be set when media player integration is available
        private IMediaPlayer _player;

        // Public parameterless constructor for XAML
        public MediaPlayerWidget()
        {
            InitializeComponent();
            _isPlaying = false;
            _isMuted = false;
            _volume = 0.75;
            _playbackSpeed = 1.0;
            
            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:54-55
            // Original: self.player: QMediaPlayer = QMediaPlayer(self)
            // MediaPlayer will be initialized when media player integration is available
            // For now, _player remains null until a media player implementation is provided
            _player = null;
            
            SetupUI();
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

        private void SetupProgrammaticUI()
        {
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical, Spacing = 4, Margin = new Avalonia.Thickness(4) };

            var controlsPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };

            _playPauseButton = new Button { Content = "▶", Width = 32, Height = 32 };
            _playPauseButton.Click += (s, e) => TogglePlayPause();
            _stopButton = new Button { Content = "■", Width = 32, Height = 32 };
            _stopButton.Click += (s, e) => Stop();

            _timeSlider = new Slider { Minimum = 0, Maximum = 100, Value = 0, Width = 200 };
            _timeSlider.ValueChanged += (s, e) => OnTimeSliderChanged();

            _timeLabel = new TextBlock { Text = "00:00 / 00:00", MinWidth = 120, HorizontalAlignment = HorizontalAlignment.Center };

            _muteButton = new Button { Content = "🔊", Width = 24, Height = 24 };
            _muteButton.Click += (s, e) => ToggleMute();

            _volumeSlider = new Slider { Minimum = 0, Maximum = 100, Value = 75, Width = 100 };
            _volumeSlider.ValueChanged += (s, e) => OnVolumeChanged();

            _speedButton = new Button { Content = "1.0x", Width = 50, Height = 24 };
            _speedButton.Click += (s, e) => ChangeSpeed();

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

        private void SetupUI()
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
                _timeSlider.ValueChanged += (s, e) => OnTimeSliderChanged();
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
                _speedButton.Click += (s, e) => ChangeSpeed();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:107-113
        // Original: Play/Pause button handling
        private void TogglePlayPause()
        {
            _isPlaying = !_isPlaying;
            if (_playPauseButton != null)
            {
                _playPauseButton.Content = _isPlaying ? "⏸" : "▶";
            }
            // TODO: Implement actual playback control when media player is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:387-394
        // Original: def stop(self) -> None:
        // Original:     """Stop playback and reset position."""
        // Original:     self.player.stop()
        // Original:     q_style: QStyle | None = self.style()
        // Original:     assert q_style is not None, "q_style is somehow None"
        // Original:     self.play_pause_button.setIcon(q_style.standardIcon(QStyle.StandardPixmap.SP_MediaPlay))
        // Original:     self.hide_widget()
        // Original:     self.playback_stopped.emit()
        private void Stop()
        {
            // Stop the media player if available
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
            
            // Update playback state
            _isPlaying = false;
            
            // Update play/pause button to show play icon
            if (_playPauseButton != null)
            {
                _playPauseButton.Content = "▶";
            }
            
            // Reset time slider to beginning
            if (_timeSlider != null)
            {
                _timeSlider.Value = 0;
            }
            
            // Reset time label to show 00:00 / 00:00
            if (_timeLabel != null)
            {
                _timeLabel.Text = "00:00 / 00:00";
            }
            
            // Hide the widget when stopped (matching PyKotor behavior)
            HideWidget();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:122-491
        // Original: Time slider and other controls
        private void OnTimeSliderChanged()
        {
            // TODO: Seek to position when media player is available
        }

        private void ToggleMute()
        {
            _isMuted = !_isMuted;
            if (_muteButton != null)
            {
                _muteButton.Content = _isMuted ? "🔇" : "🔊";
            }
            // TODO: Implement actual mute when media player is available
        }

        private void OnVolumeChanged()
        {
            if (_volumeSlider != null)
            {
                _volume = _volumeSlider.Value / 100.0;
            }
            // TODO: Set volume when media player is available
        }

        private void ChangeSpeed()
        {
            double[] speedLevels = { 0.25, 0.5, 0.75, 1.0, 1.25, 1.5, 1.75, 2.0 };
            int currentIndex = Array.IndexOf(speedLevels, _playbackSpeed);
            if (currentIndex < 0)
            {
                currentIndex = 3; // Default to 1.0x
            }

            currentIndex = (currentIndex + 1) % speedLevels.Length;
            _playbackSpeed = speedLevels[currentIndex];

            if (_speedButton != null)
            {
                _speedButton.Content = $"{_playbackSpeed:F2}x";
            }
            // TODO: Set playback speed when media player is available
        }

        public void HideWidget()
        {
            IsVisible = false;
        }

        public void ShowWidget()
        {
            IsVisible = true;
        }
        
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:54-55
        // Original: self.player: QMediaPlayer = QMediaPlayer(self)
        // Property to set/get the media player instance
        // This allows external code to provide a media player implementation when available
        public IMediaPlayer Player
        {
            get { return _player; }
            set { _player = value; }
        }
    }
    
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/media_player_widget.py:54-55
    // Original: QMediaPlayer interface
    // Interface for media player functionality to allow different implementations
    // This abstraction allows the widget to work with any media player implementation
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
