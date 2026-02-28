using System;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Interface for media player functionality to allow different implementations.
    /// Shared so that NAudioMediaPlayer and MediaPlayerWidget can be used without circular dependency.
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
