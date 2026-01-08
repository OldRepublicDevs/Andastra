using System;
using System.Collections.Generic;
using Andastra.Parsing.Formats.NCS.NCSDecomp.Utils;
using JetBrains.Annotations;

namespace Andastra.Parsing.Formats.NCS.NCSDecomp
{
    /// <summary>
    /// Stub implementation of FileDecompiler for Resource.NCS project
    /// </summary>
    public class FileDecompiler
    {
        // Properties used by Settings and RoundTripUtil
        public bool isK2Selected { get; set; }
        public bool preferSwitches { get; set; }
        public bool strictSignatures { get; set; }
        public string nwnnsscompPath { get; set; }

        // TODO: Implement when FileDecompiler.cs is restored
        public static FileDecompiler CreateDecompiler(object gameType, object settings)
        {
            return new FileDecompiler();
        }

        public int DecompileNcs([CanBeNull] object ncsFile, [CanBeNull] object outputFile, [CanBeNull] Utils.FileScriptData data)
        {
            // TODO: Implement decompilation
            return 0;
        }

        public int CompileAndCompare([CanBeNull] object file, [CanBeNull] string code, [CanBeNull] Utils.FileScriptData data)
        {
            // TODO: Implement compilation and comparison
            return 0;
        }

        public void LoadActionsData()
        {
            // TODO: Implement when FileDecompiler.cs is restored
        }

        public void DecompileToFile([CanBeNull] object ncsFile, [CanBeNull] string outputPath)
        {
            // TODO: Implement when FileDecompiler.cs is restored
        }
    }
}

namespace Andastra.Parsing.Formats.NCS.Compiler
{
    /// <summary>
    /// Stub classes for compiler functionality
    /// </summary>
    public class NwnnsscompConfig
    {
        // TODO: Implement when TSLPatcher dependency is resolved
        public string[] GetCompileArgs(string sourcePath, string outputPath)
        {
            return new string[] { sourcePath, outputPath };
        }
    }

    public class ExternalNCSCompiler
    {
        // TODO: Implement when TSLPatcher dependency is resolved
        public ExternalNCSCompiler(NwnnsscompConfig config)
        {
            // Stub constructor
        }
    }
}