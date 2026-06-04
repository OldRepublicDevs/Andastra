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
        private Stream _ownedStream; // for in-memory WAV via SetSourceFromBytes
        private AudioFileReader _reader;
        private WaveOutEvent _waveOut;
        private WaveStream _readerStream; // WaveFileReader when using _ownedStream
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
            Stop();
            DisposeReader();
            DisposeOwnedStream();
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                _filePath = null;
                return;
            }
            _filePath = filePath;
        }

        /// <summary>
        /// Sets the playback source from in-memory WAV bytes. Cross-platform; no temp files.
        /// </summary>
        public void SetSourceFromBytes(byte[] wavBytes)
        {
            Stop();
            DisposeReader();
            DisposeOwnedStream();
            _filePath = null;
            if (wavBytes != null && wavBytes.Length > 0)
            {
                _ownedStream = new MemoryStream(wavBytes);
            }
        }

        private void DisposeOwnedStream()
        {
            if (_ownedStream != null)
            {
                try { _ownedStream.Dispose(); }
                catch { }
                _ownedStream = null;
            }
        }

        private void InitReader()
        {
            DisposeReader();
            if (_ownedStream != null)
            {
                try
                {
                    _readerStream = new WaveFileReader(_ownedStream);
                    _waveOut = new WaveOutEvent();
                    _waveOut.Init(_readerStream);
                    _waveOut.PlaybackStopped += OnPlaybackStopped;
                }
                catch
                {
                    DisposeReader();
                    throw;
                }
                return;
            }
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
            if (_readerStream != null)
            {
                try { _readerStream.Dispose(); }
                catch { }
                _readerStream = null;
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlaybackStopped?.Invoke(this, EventArgs.Empty);
        }

        public void Play()
        {
            bool hasSource = (_ownedStream != null) || (!string.IsNullOrEmpty(_filePath) && File.Exists(_filePath));
            if (!hasSource)
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
                if (_readerStream != null)
                    _readerStream.Position = 0;
            }
            catch { }
        }

        private WaveStream GetCurrentStream()
        {
            if (_reader != null) return _reader;
            return _readerStream;
        }

        public void SetPosition(TimeSpan position)
        {
            var stream = GetCurrentStream();
            if (stream == null) return;
            try
            {
                var pos = (long)(position.TotalSeconds * stream.WaveFormat.AverageBytesPerSecond);
                pos = Math.Max(0, Math.Min(pos, stream.Length));
                stream.Position = pos;
            }
            catch { }
        }

        public TimeSpan Position
        {
            get
            {
                var stream = GetCurrentStream();
                if (stream == null) return TimeSpan.Zero;
                try
                {
                    return TimeSpan.FromSeconds((double)stream.Position / stream.WaveFormat.AverageBytesPerSecond);
                }
                catch { return TimeSpan.Zero; }
            }
        }

        public TimeSpan Duration
        {
            get
            {
                var stream = GetCurrentStream();
                if (stream == null) return TimeSpan.Zero;
                try
                {
                    return stream.TotalTime;
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
            DisposeOwnedStream();
        }
    }
}
