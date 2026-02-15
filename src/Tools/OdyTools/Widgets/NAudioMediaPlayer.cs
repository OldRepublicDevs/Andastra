using System;
using System.IO;
using NAudio.Wave;

namespace OdyTools.Widgets
{
    /// <summary>
    /// IMediaPlayer implementation using NAudio. Supports WAV, MP3 and other formats
    /// that NAudio's AudioFileReader can decode. Playback from a file path.
    /// </summary>
    public sealed class NAudioMediaPlayer : IMediaPlayer, IDisposable
    {
        private string _filePath;
        private AudioFileReader _reader;
        private WaveOutEvent _waveOut;
        private float _volume = 0.75f;
        private bool _isMuted;
        private double _playbackRate = 1.0;
        private bool _disposed;

        public event EventHandler PlaybackStopped;

        public NAudioMediaPlayer()
        {
        }

        public void SetSource(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Stop();
                _filePath = null;
                return;
            }

            Stop();
            _filePath = filePath;
            InitReader();
        }

        private void InitReader()
        {
            DisposeReader();
            if (string.IsNullOrEmpty(_filePath) || !File.Exists(_filePath))
                return;

            try
            {
                _reader = new AudioFileReader(_filePath);
                _reader.Volume = _isMuted ? 0f : _volume;

                _waveOut = new WaveOutEvent();
                _waveOut.Init(_reader);
                _waveOut.PlaybackStopped += OnPlaybackStopped;
            }
            catch
            {
                DisposeReader();
                throw;
            }
        }

        private void DisposeReader()
        {
            if (_waveOut != null)
            {
                try
                {
                    _waveOut.PlaybackStopped -= OnPlaybackStopped;
                    _waveOut.Stop();
                    _waveOut.Dispose();
                }
                catch { }
                _waveOut = null;
            }
            if (_reader != null)
            {
                try { _reader.Dispose(); }
                catch { }
                _reader = null;
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        public void Play()
        {
            if (string.IsNullOrEmpty(_filePath) || !File.Exists(_filePath))
                return;
            if (_waveOut == null)
                InitReader();
            if (_waveOut == null)
                return;
            try
            {
                _waveOut.Play();
            }
            catch { }
        }

        public void Pause()
        {
            try
            {
                _waveOut?.Pause();
            }
            catch { }
        }

        public void Stop()
        {
            try
            {
                _waveOut?.Stop();
                if (_reader != null)
                    _reader.Position = 0;
            }
            catch { }
        }

        public void SetPosition(TimeSpan position)
        {
            if (_reader == null) return;
            try
            {
                var pos = (long)(position.TotalSeconds * _reader.WaveFormat.AverageBytesPerSecond);
                pos = Math.Max(0, Math.Min(pos, _reader.Length));
                _reader.Position = pos;
            }
            catch { }
        }

        public TimeSpan Position
        {
            get
            {
                if (_reader == null) return TimeSpan.Zero;
                try
                {
                    return TimeSpan.FromSeconds((double)_reader.Position / _reader.WaveFormat.AverageBytesPerSecond);
                }
                catch { return TimeSpan.Zero; }
            }
        }

        public TimeSpan Duration
        {
            get
            {
                if (_reader == null) return TimeSpan.Zero;
                try
                {
                    return _reader.TotalTime;
                }
                catch { return TimeSpan.Zero; }
            }
        }

        public double Volume
        {
            get => _volume;
            set
            {
                _volume = (float)Math.Max(0.0, Math.Min(1.0, value));
                if (!_isMuted && _reader != null)
                    _reader.Volume = _volume;
            }
        }

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                if (_reader == null) return;
                _reader.Volume = _isMuted ? 0f : _volume;
            }
        }

        public double PlaybackRate
        {
            get => _playbackRate;
            set => _playbackRate = Math.Max(0.25, Math.Min(2.0, value));
        }

        public NAudio.Wave.PlaybackState PlaybackState =>
            _waveOut?.PlaybackState ?? NAudio.Wave.PlaybackState.Stopped;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            DisposeReader();
        }
    }
}
