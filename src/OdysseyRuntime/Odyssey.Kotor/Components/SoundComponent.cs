using System.Collections.Generic;
using Odyssey.Core.Interfaces;

namespace Odyssey.Kotor.Components
{
    /// <summary>
    /// Component for ambient sound entities.
    /// </summary>
    /// <remarks>
    /// Sound Component:
    /// - Based on swkotor2.exe sound system
    /// - Located via string references: "SoundList" @ 0x007bd278 (GIT sound list), "Sound" @ 0x007bc548 (sound entity type)
    /// - "PlaySound" @ 0x007c5f70 (sound playback function), "Volume" @ 0x007c6110 (sound volume field)
    /// - "MinVolumeDist" @ 0x007c60c4, "MaxVolumeDist" @ 0x007c60d8 (sound distance falloff fields)
    /// - Template loading: FUN_004e08e0 @ 0x004e08e0 loads sound instances from GIT
    /// - Original implementation: Sound entities emit positional audio in the game world
    /// - UTS file format: GFF with "UTS " signature containing sound data (Active, Looping, Positional, ResRef, Volume, MaxDistance, MinDistance)
    /// - Sound entities can be active/inactive (Active field), looping (Looping field), positional (Positional field for 3D audio)
    /// - Volume: 0-127 range (Volume field), distance falloff: MinDistance (full volume) to MaxDistance (zero volume)
    /// - Continuous sounds: Play continuously when active (Continuous field)
    /// - Random sounds: Can play random sounds from SoundFiles list (Random field), randomize position (RandomPosition field)
    /// - Interval: Time between plays for non-looping sounds (Interval field, IntervalVrtn for variation)
    /// - Volume variation: VolumeVrtn field for random volume variation
    /// - Hours: Bitmask for time-based activation (Hours field, 0-23 hour range)
    /// - Based on UTS file format documentation in vendor/PyKotor/wiki/
    /// </remarks>
    public class SoundComponent : IComponent
    {
        public IEntity Owner { get; set; }

        public void OnAttach() { }
        public void OnDetach() { }

        public SoundComponent()
        {
            TemplateResRef = string.Empty;
            SoundFiles = new List<string>();
            Volume = 100;
            MaxDistance = 50f;
            MinDistance = 1f;
        }

        /// <summary>
        /// Template resource reference.
        /// </summary>
        public string TemplateResRef { get; set; }

        /// <summary>
        /// Whether the sound is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Whether the sound is continuous.
        /// </summary>
        public bool Continuous { get; set; }

        /// <summary>
        /// Whether the sound loops.
        /// </summary>
        public bool Looping { get; set; }

        /// <summary>
        /// Whether the sound is positional (3D).
        /// </summary>
        public bool Positional { get; set; }

        /// <summary>
        /// Whether to randomize position.
        /// </summary>
        public bool RandomPosition { get; set; }

        /// <summary>
        /// Whether to play random sounds from list.
        /// </summary>
        public bool Random { get; set; }

        /// <summary>
        /// Volume (0-127).
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// Volume variation.
        /// </summary>
        public int VolumeVrtn { get; set; }

        /// <summary>
        /// Maximum audible distance.
        /// </summary>
        public float MaxDistance { get; set; }

        /// <summary>
        /// Minimum distance (full volume).
        /// </summary>
        public float MinDistance { get; set; }

        /// <summary>
        /// Interval between plays (seconds).
        /// </summary>
        public uint Interval { get; set; }

        /// <summary>
        /// Interval variation.
        /// </summary>
        public uint IntervalVrtn { get; set; }

        /// <summary>
        /// Pitch variation.
        /// </summary>
        public float PitchVariation { get; set; }

        /// <summary>
        /// List of sound file resources.
        /// </summary>
        public List<string> SoundFiles { get; set; }

        /// <summary>
        /// Hours when sound is active (bitmask, 0-23).
        /// </summary>
        public uint Hours { get; set; }

        /// <summary>
        /// Time since last play.
        /// </summary>
        public float TimeSinceLastPlay { get; set; }

        /// <summary>
        /// Whether currently playing.
        /// </summary>
        public bool IsPlaying { get; set; }
    }
}
