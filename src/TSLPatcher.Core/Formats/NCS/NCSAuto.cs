using System;
using System.Collections.Generic;
using System.IO;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS.Compiler;
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

    /// <summary>
    /// Compile NSS source code to NCS bytecode.
    /// 1:1 port from Python compile_nss in pykotor/resource/formats/ncs/ncs_auto.py
    /// </summary>
    public static NCS CompileNss(
        string source,
        Game game,
        List<INCSOptimizer>? optimizers = null,
        List<string>? libraryLookup = null)
    {
        var compiler = new Compiler.NssCompiler(game, libraryLookup);
        NCS ncs = compiler.Compile(source);

        if (optimizers != null && optimizers.Count > 0)
        {
            ncs.Optimize(optimizers);
        }

        return ncs;
    }

    /// <summary>
    /// Decompile NCS bytecode to NSS source code.
    /// 1:1 port from Python decompile_ncs in pykotor/resource/formats/ncs/ncs_auto.py
    /// </summary>
    public static string DecompileNcs(
        NCS ncs,
        Game game,
        List<ScriptFunction>? functions = null,
        List<ScriptConstant>? constants = null)
    {
        // TODO: Implement full NCS decompiler
        // This requires:
        // 1. Instruction-to-NSS conversion logic
        // 2. Control flow reconstruction (if/else, loops, switch)
        // 3. Function call resolution
        // 4. Variable name reconstruction
        // 5. Struct handling
        // 6. All NWScript language features
        
        // For now, this is a placeholder that will need the full decompiler implementation
        throw new NotImplementedException(
            "NCS decompilation requires full decompiler implementation. " +
            "This is a large undertaking and needs to be ported from Python. " +
            "The Python implementation in pykotor/resource/formats/ncs/decompiler.py needs to be ported 1:1.");
    }
}

