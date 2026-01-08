using System;
using System.Collections.Generic;
using Andastra.Parsing.Formats.NCS;
using JetBrains.Annotations;

namespace Andastra.Parsing.Formats.NCS
{
    /// <summary>
    /// Stub implementation of NCSAuto for Resource.NCS project to avoid circular dependencies
    /// </summary>
    public static class NCSAuto
    {
        [CanBeNull]
        public static NCS CompileNss(string source, object game, object optimizers = null, object libraryLookup = null, object globalVariables = null, object debug = null, bool useNWScript = false, string nwscriptPath = null)
        {
            // TODO: Implement compilation when circular dependency is resolved
            return new NCS();
        }

        public static byte[] BytesNcs([CanBeNull] NCS ncs)
        {
            // TODO: Implement bytecode generation when circular dependency is resolved
            return new byte[0];
        }

        [CanBeNull]
        public static NCS ReadNcs(string filepath)
        {
            // TODO: Implement reading when circular dependency is resolved
            return new NCS();
        }

        public static void WriteNcs([CanBeNull] NCS ncs, string filepath)
        {
            // TODO: Implement writing when circular dependency is resolved
        }

        [CanBeNull]
        public static string DecompileNcs([CanBeNull] NCS ncs, object game = null)
        {
            // TODO: Implement decompilation when circular dependency is resolved
            return "// TODO: Decompilation not implemented";
        }
    }
}