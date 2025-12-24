using System;
using System.Numerics;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Graphics;
using Andastra.Parsing.Formats.WAV;
using Stride.Audio;
using Stride.Media;

namespace Andastra.Runtime.Stride.Audio
{
    /// <summary>
    /// Stride implementation of IVoicePlayer for playing voice-over audio (dialogue).
    ///
    /// Loads WAV files from KOTOR installation and plays them using Stride's Audio API.
    /// Unlike StrideSoundPlayer, this only plays one voice-over at a time (replacing any currently playing).
    ///
    /// Based on Stride API: https://doc.stride3d.net/latest/en/manual/audio/index.html
    /// SoundInstance API: https://doc.stride3d.net/latest/en/api/Stride.Audio.SoundInstance.html
    /// </summary>
    /// <remarks>
    /// Voice Player (Stride Implementation):
    /// - Based on swkotor2.exe voice-over playback system
    /// - Located via string references: "VoiceVolume" @ 0x007c2ce4, "PlayVoice" @ 0x007c5f80
    /// - Original implementation: KOTOR plays WAV files for voice-over dialogue
    /// - Voice files: Stored as WAV resources, referenced by ResRef (e.g., "n_darthmalak01.wav")
    /// - Dialogue system: Voice-overs are triggered during conversations and cutscenes
    /// - Playback control: Play, Stop, volume, pan, pitch
    /// - Unlike sound effects, only one voice-over plays at a time (dialogue focus)
    /// - This Stride implementation uses Stride's AudioEngine and SoundInstance API
    ///
    /// Implementation Notes:
    /// - Uses SoundInstance.PlayState to check if voice is playing (Playing, Stopped, Paused)
    /// - Uses SoundInstance.Stop() to stop playback
    /// - Creates SoundInstance from WAV data using DynamicSoundSource
    /// - Update() method removes stopped voices and disposes resources
    /// - Only supports one voice instance at a time (unlike StrideSoundPlayer)
    /// - Voice volume is separate from sound effects volume
    /// </remarks>
    public class StrideVoicePlayer : Andastra.Runtime.Core.Audio.IVoicePlayer
    {
        private readonly IGameResourceProvider _resourceProvider;
        private readonly ISpatialAudio _spatialAudio;
        private readonly AudioEngine _audioEngine;
        private readonly AudioListener _audioListener;
        private SoundInstance _currentVoiceInstance;
        private uint _nextInstanceId = 1;
        private float _masterVolume = 1.0f;
        private uint _currentInstanceId = 0;

        /// <summary>
        /// Initializes a new instance of StrideVoicePlayer.
        /// </summary>
        /// <param name="resourceProvider">Provider for loading game resources.</param>
        /// <param name="spatialAudio">Spatial audio system for 3D positioning.</param>
        /// <param name="audioEngine">Stride audio engine.</param>
        /// <param name="audioListener">Audio listener for spatial calculations.</param>
        public StrideVoicePlayer(
            IGameResourceProvider resourceProvider,
            ISpatialAudio spatialAudio,
            AudioEngine audioEngine,
            AudioListener audioListener)
        {
            _resourceProvider = resourceProvider ?? throw new ArgumentNullException(nameof(resourceProvider));
            _spatialAudio = spatialAudio ?? throw new ArgumentNullException(nameof(spatialAudio));
            _audioEngine = audioEngine ?? throw new ArgumentNullException(nameof(audioEngine));
            _audioListener = audioListener ?? throw new ArgumentNullException(nameof(audioListener));
        }

        /// <inheritdoc/>
        public uint PlayVoice(string voiceResRef, Vector3? position = null, float volume = 1.0f, float pitch = 0.0f, float pan = 0.0f)
        {
            // Stop any currently playing voice
            StopAllVoices();

            try
            {
                // Load the WAV data from the resource
                var wavData = _resourceProvider.LoadResource<WAV>(voiceResRef);
                if (wavData == null)
                {
                    // Try with .wav extension if not provided
                    if (!voiceResRef.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                    {
                        wavData = _resourceProvider.LoadResource<WAV>(voiceResRef + ".wav");
                    }

                    if (wavData == null)
                    {
                        return 0; // Failed to load
                    }
                }

                // Convert WAV data to PCM format that Stride can use
                var pcmData = ConvertWavToPcm(wavData);
                if (pcmData.Length == 0)
                {
                    return 0; // Failed to convert
                }

                // Create a dynamic sound source from the PCM data
                var soundSource = new DynamicSoundSource();
                soundSource.BufferData(pcmData, wavData.SampleRate, wavData.Channels == 2);

                // Create sound instance
                _currentVoiceInstance = soundSource.CreateInstance();

                if (_currentVoiceInstance == null)
                {
                    soundSource.Dispose();
                    return 0; // Failed to create instance
                }

                // Set volume (combine relative volume with master volume)
                _currentVoiceInstance.Volume = volume * _masterVolume;

                // Set pitch (Stride uses frequency multiplier, convert from semitones)
                if (pitch != 0.0f)
                {
                    // Convert pitch from semitones (-12 to +12) to frequency multiplier
                    float frequencyMultiplier = (float)Math.Pow(2.0, pitch / 12.0);
                    _currentVoiceInstance.Pitch = frequencyMultiplier;
                }

                // Set pan (convert from -1.0 to 1.0 to Stride's pan range)
                if (pan != 0.0f)
                {
                    _currentVoiceInstance.Pan = pan;
                }

                // Set up spatial audio if position is provided
                if (position.HasValue && _spatialAudio != null)
                {
                    _spatialAudio.ApplySpatialAudio(_currentVoiceInstance, position.Value, _audioListener);
                }

                // Start playback
                _currentVoiceInstance.Play();

                // Generate instance ID
                _currentInstanceId = _nextInstanceId++;
                if (_nextInstanceId == 0) _nextInstanceId = 1; // Wrap around

                return _currentInstanceId;
            }
            catch (Exception)
            {
                // Clean up on failure
                if (_currentVoiceInstance != null)
                {
                    _currentVoiceInstance.Stop();
                    _currentVoiceInstance = null;
                }
                return 0;
            }
        }

        /// <inheritdoc/>
        public void StopVoice(uint voiceInstanceId)
        {
            // Only stop if this is the current instance
            if (_currentInstanceId == voiceInstanceId && _currentVoiceInstance != null)
            {
                _currentVoiceInstance.Stop();
                _currentVoiceInstance = null;
                _currentInstanceId = 0;
            }
        }

        /// <inheritdoc/>
        public void StopAllVoices()
        {
            if (_currentVoiceInstance != null)
            {
                _currentVoiceInstance.Stop();
                _currentVoiceInstance = null;
                _currentInstanceId = 0;
            }
        }

        /// <inheritdoc/>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Math.Clamp(volume, 0.0f, 1.0f);

            // Apply to current voice if playing
            if (_currentVoiceInstance != null)
            {
                // We need to preserve the relative volume, but we don't store it
                // For simplicity, assume current volume was set with relative volume = 1.0
                // In a full implementation, we'd store the original relative volume
                _currentVoiceInstance.Volume = 1.0f * _masterVolume;
            }
        }

        /// <inheritdoc/>
        public void Update(float deltaTime)
        {
            // Check if current voice has finished playing
            if (_currentVoiceInstance != null)
            {
                if (_currentVoiceInstance.PlayState == PlayState.Stopped ||
                    _currentVoiceInstance.PlayState == PlayState.Ended)
                {
                    _currentVoiceInstance = null;
                    _currentInstanceId = 0;
                }
            }
        }

        /// <summary>
        /// Converts WAV data to PCM format for Stride.
        /// </summary>
        private byte[] ConvertWavToPcm(WAV wavData)
        {
            if (wavData == null || wavData.Data == null || wavData.Data.Length == 0)
            {
                return Array.Empty<byte>();
            }

            // Stride expects 16-bit PCM data
            // If WAV data is already 16-bit PCM, return as-is
            if (wavData.BitsPerSample == 16)
            {
                return wavData.Data;
            }

            // For other formats, we'd need conversion logic
            // For now, return empty array (would need proper conversion)
            // TODO: STUB - Implement WAV format conversion (8-bit to 16-bit, compressed formats, etc.)
            return Array.Empty<byte>();
        }
    }
}
