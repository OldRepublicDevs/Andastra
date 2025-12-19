using System;
using System.IO;
using System.Text;
using Andastra.Runtime.Content.Save;
using Andastra.Runtime.Core.Save;

namespace Andastra.Runtime.Engines.Eclipse.Save
{
    /// <summary>
    /// Abstract base class for Eclipse Engine save serializer implementations.
    /// </summary>
    /// <remarks>
    /// Eclipse Save System Base:
    /// - Based on Eclipse/Unreal Engine save system
    /// - Eclipse uses UnrealScript message passing system - save format is different from Odyssey GFF/ERF
    /// - Architecture: Message-based (SaveGameMessage) vs Odyssey direct file I/O
    /// - Game-specific implementations: DragonAgeOriginsSaveSerializer, DragonAge2SaveSerializer, MassEffectSaveSerializer, MassEffect2SaveSerializer
    /// - Common functionality: Binary serialization helpers, signature validation, version checking
    /// </remarks>
    public abstract class EclipseSaveSerializer : ISaveSerializer
    {
        public abstract byte[] SerializeSaveNfo(SaveGameData saveData);
        public abstract SaveGameData DeserializeSaveNfo(byte[] data);
        public abstract byte[] SerializeSaveArchive(SaveGameData saveData);
        public abstract void DeserializeSaveArchive(byte[] data, SaveGameData saveData);

        #region Common Binary Serialization Helpers

        /// <summary>
        /// Writes a string to a binary writer (length-prefixed UTF-8).
        /// Common across all Eclipse save formats.
        /// </summary>
        protected void WriteString(BinaryWriter writer, string value)
        {
            if (value == null)
            {
                value = "";
            }

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes.Length);
            writer.Write(bytes);
        }

        /// <summary>
        /// Reads a string from a binary reader (length-prefixed UTF-8).
        /// Common across all Eclipse save formats.
        /// </summary>
        protected string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length < 0 || length > 65536) // Sanity check
            {
                throw new InvalidDataException($"Invalid string length: {length}");
            }

            byte[] bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Validates a save file signature.
        /// Common across all Eclipse save formats.
        /// </summary>
        protected void ValidateSignature(BinaryReader reader, string expectedSignature)
        {
            byte[] signature = reader.ReadBytes(4);
            string actualSignature = Encoding.UTF8.GetString(signature);
            if (actualSignature != expectedSignature)
            {
                throw new InvalidDataException($"Invalid save file signature. Expected '{expectedSignature}', got '{actualSignature}'");
            }
        }

        /// <summary>
        /// Validates a save file version.
        /// Common across all Eclipse save formats.
        /// </summary>
        protected void ValidateVersion(BinaryReader reader, int expectedVersion, string formatName)
        {
            int version = reader.ReadInt32();
            if (version != expectedVersion)
            {
                throw new NotSupportedException($"Unsupported {formatName} save version: {version} (expected {expectedVersion})");
            }
        }

        /// <summary>
        /// Writes save metadata fields common to all Eclipse games.
        /// </summary>
        protected void WriteCommonMetadata(BinaryWriter writer, SaveGameData saveData)
        {
            // Save name
            WriteString(writer, saveData.Name ?? "");

            // Module name
            WriteString(writer, saveData.CurrentModule ?? "");

            // Time played (seconds)
            writer.Write((int)saveData.PlayTime.TotalSeconds);

            // Timestamp (FileTime)
            writer.Write(saveData.SaveTime.ToFileTime());
        }

        /// <summary>
        /// Reads save metadata fields common to all Eclipse games.
        /// </summary>
        protected void ReadCommonMetadata(BinaryReader reader, SaveGameData saveData)
        {
            // Save name
            saveData.Name = ReadString(reader);

            // Module name
            saveData.CurrentModule = ReadString(reader);

            // Time played (seconds)
            int timePlayed = reader.ReadInt32();
            saveData.PlayTime = TimeSpan.FromSeconds(timePlayed);

            // Timestamp (FileTime)
            long fileTime = reader.ReadInt64();
            saveData.SaveTime = DateTime.FromFileTime(fileTime);
        }

        #endregion
    }
}
