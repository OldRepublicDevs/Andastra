using JetBrains.Annotations;

namespace Andastra.Parsing.Resource.Generics
{
    /// <summary>
    /// Represents a room definition within an area.
    /// 
    /// Rooms define audio properties, weather behavior, and force rating for specific
    /// regions within an area. Rooms are referenced by VIS (visibility) files and
    /// used for audio occlusion and weather control.
    /// </summary>
    /// <remarks>
    /// References:
    /// - vendor/reone/include/reone/resource/parser/gff/are.h:185-191 - ARE_Rooms struct
    /// - vendor/reone/src/libs/resource/parser/gff/are.cpp:244-251 - parseARE_Rooms function
    /// - vendor/Kotor.NET/Kotor.NET/Resources/KotorARE/ARE.cs:99-106 - ARERoom class
    /// - vendor/KotOR.js/src/module/ModuleRoom.ts - ModuleRoom class (runtime room handling)
    /// - Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/are.py:300-376
    /// 
    /// Cross-Engine Analysis:
    /// - Odyssey (swkotor.exe, swkotor2.exe): Rooms used for audio zones and weather regions
    /// - Aurora (nwmain.exe): Rooms used for environment audio and minimap regions
    /// - Eclipse (daorigins.exe, DragonAge2.exe): Rooms used for audio zones
    /// 
    /// Room structure in GFF:
    /// - RoomName (String): Unique identifier for this room (referenced by VIS files)
    /// - EnvAudio (Int32): Index into environment audio system for room acoustics
    /// - AmbientScale (Single): Scaling factor for ambient audio volume in this room
    /// - DisableWeather (UInt8): If true, weather effects are disabled in this room (KotOR 2 only)
    /// - ForceRating (Int32): Force rating modifier applied in this room (KotOR 2 only)
    /// </remarks>
    [PublicAPI]
    public sealed class ARERoom
    {
        /// <summary>
        /// Room name identifier.
        /// </summary>
        /// <remarks>
        /// Reference: reone/are.cpp:250 (strct.RoomName = gff.getString("RoomName"))
        /// Reference: Kotor.NET/ARE.cs:105 (RoomName String property)
        /// Reference: KotOR.js/ModuleRoom.ts (room name)
        /// Unique identifier for this room (referenced by VIS files)
        /// </remarks>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Disable weather flag for this room.
        /// </summary>
        /// <remarks>
        /// Reference: reone/are.cpp:247 (strct.DisableWeather = gff.getUint("DisableWeather"))
        /// Reference: Kotor.NET/ARE.cs:102 (DisableWeather Byte property)
        /// Reference: KotOR.js/ModuleArea.ts:463 (room_struct.set_uint8("DisableWeather", room.weather))
        /// If True, weather effects are disabled in this room (KotOR 2 only)
        /// </remarks>
        public bool Weather { get; set; }

        /// <summary>
        /// Environment audio index.
        /// </summary>
        /// <remarks>
        /// Reference: reone/are.cpp:248 (strct.EnvAudio = gff.getInt("EnvAudio"))
        /// Reference: Kotor.NET/ARE.cs:103 (EnvAudio Int32 property)
        /// Reference: KotOR.js/ModuleArea.ts:138 (audio.environmentAudio = 0)
        /// Index into environment audio system for room acoustics
        /// </remarks>
        public int EnvAudio { get; set; }

        /// <summary>
        /// Force rating modifier for this room.
        /// </summary>
        /// <remarks>
        /// Reference: reone/are.cpp:249 (strct.ForceRating = gff.getInt("ForceRating"))
        /// Reference: Kotor.NET/ARE.cs:104 (ForceRating Int32 property)
        /// Reference: KotOR.js/ModuleArea.ts:464 (room_struct.set_int32("ForceRating", room.force_rating))
        /// Force rating modifier applied in this room (KotOR 2 only)
        /// </remarks>
        public int ForceRating { get; set; }

        /// <summary>
        /// Ambient audio scaling factor.
        /// </summary>
        /// <remarks>
        /// Reference: reone/are.cpp:246 (strct.AmbientScale = gff.getFloat("AmbientScale"))
        /// Reference: Kotor.NET/ARE.cs:101 (AmbientScale Single property)
        /// Reference: KotOR.js/ModuleArea.ts:459 (room_struct.set_single("AmbientScale", room.ambient_scale))
        /// Scaling factor for ambient audio volume in this room
        /// </remarks>
        public float AmbientScale { get; set; }

        /// <summary>
        /// Initializes a new instance of the ARERoom class.
        /// </summary>
        /// <param name="name">Room name identifier.</param>
        /// <param name="weather">Disable weather flag (KotOR 2 only).</param>
        /// <param name="envAudio">Environment audio index.</param>
        /// <param name="forceRating">Force rating modifier (KotOR 2 only).</param>
        /// <param name="ambientScale">Ambient audio scaling factor.</param>
        public ARERoom(string name = "", bool weather = false, int envAudio = 0, int forceRating = 0, float ambientScale = 0.0f)
        {
            Name = name;
            Weather = weather;
            EnvAudio = envAudio;
            ForceRating = forceRating;
            AmbientScale = ambientScale;
        }
    }
}

