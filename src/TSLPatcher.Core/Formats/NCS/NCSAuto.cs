using System;
using System.IO;
using TSLPatcher.Core.Resources;

namespace TSLPatcher.Core.Formats.NCS;

/// <summary>
/// Auto-loading functions for NCS files.
/// 1:1 port from pykotor.resource.formats.ncs.ncs_auto
/// </summary>
public static class NCSAuto
{
    /// <summary>
    /// Returns an NCS instance from the source.
    ///
    /// Args:
    ///     source: The source of the data (file path, byte array, or stream).
    ///     offset: The byte offset of the file inside the data.
    ///     size: Number of bytes to allowed to read from the stream. If not specified, uses the whole stream.
    ///
    /// Raises:
    ///     InvalidDataException: If the file was corrupted or in an unsupported format.
    ///
    /// Returns:
    ///     An NCS instance.
    /// </summary>
    public static NCS ReadNcs(string filepath, int offset = 0, int? size = null)
    {
        using var reader = new NCSBinaryReader(filepath, offset, size ?? 0);
        return reader.Load();
    }

    public static NCS ReadNcs(byte[] data, int offset = 0, int? size = null)
    {
        using var reader = new NCSBinaryReader(data, offset, size ?? 0);
        return reader.Load();
    }

    public static NCS ReadNcs(Stream source, int offset = 0, int? size = null)
    {
        using var reader = new NCSBinaryReader(source, offset, size ?? 0);
        return reader.Load();
    }

    /// <summary>
    /// Writes the NCS data to the target location with the specified format (NCS only).
    ///
    /// Args:
    ///     ncs: The NCS file being written.
    ///     target: The location to write the data to (file path or stream).
    ///     fileFormat: The file format (currently only NCS is supported).
    ///
    /// Raises:
    ///     ArgumentException: If an unsupported file format was given.
    /// </summary>
    public static void WriteNcs(NCS ncs, string filepath, ResourceType? fileFormat = null)
    {
        fileFormat ??= ResourceType.NCS;
        if (fileFormat != ResourceType.NCS)
        {
            throw new ArgumentException("Unsupported format specified; use NCS.", nameof(fileFormat));
        }

        byte[] data = new NCSBinaryWriter(ncs).Write();
        File.WriteAllBytes(filepath, data);
    }

    public static void WriteNcs(NCS ncs, Stream target, ResourceType? fileFormat = null)
    {
        fileFormat ??= ResourceType.NCS;
        if (fileFormat != ResourceType.NCS)
        {
            throw new ArgumentException("Unsupported format specified; use NCS.", nameof(fileFormat));
        }

        byte[] data = new NCSBinaryWriter(ncs).Write();
        target.Write(data, 0, data.Length);
    }

    /// <summary>
    /// Returns the NCS data in the specified format (NCS only) as a byte array.
    ///
    /// This is a convenience method that wraps the WriteNcs() method.
    ///
    /// Args:
    ///     ncs: The target NCS object.
    ///     fileFormat: The file format (currently only NCS is supported).
    ///
    /// Raises:
    ///     ArgumentException: If an unsupported file format was given.
    ///
    /// Returns:
    ///     The NCS data as a byte array.
    /// </summary>
    public static byte[] BytesNcs(NCS ncs, ResourceType? fileFormat = null)
    {
        fileFormat ??= ResourceType.NCS;
        if (fileFormat != ResourceType.NCS)
        {
            throw new ArgumentException("Unsupported format specified; use NCS.", nameof(fileFormat));
        }

        return new NCSBinaryWriter(ncs).Write();
    }

   