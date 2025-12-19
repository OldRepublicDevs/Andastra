using System;
using System.IO;
using Andastra.Parsing;
using Andastra.Parsing.Formats.MDLData;
using Andastra.Parsing.Formats.MDL;
using Andastra.Parsing.Common;

namespace Andastra.Parsing.Formats.MDL
{
    // Simplified binary writer. Full parity with PyKotor io_mdl.py is pending.
    public class MDLBinaryWriter : IDisposable
    {
        private readonly MDLData.MDL _mdl;
        private readonly RawBinaryWriter _writer;
        private readonly RawBinaryWriter _mdxWriter;

        public MDLBinaryWriter(MDLData.MDL mdl, string mdlPath, string mdxPath)
        {
            _mdl = mdl ?? throw new ArgumentNullException(nameof(mdl));
            _writer = RawBinaryWriter.ToFile(mdlPath);
            _mdxWriter = RawBinaryWriter.ToFile(mdxPath);
        }

        public MDLBinaryWriter(MDLData.MDL mdl, object mdlTarget, object mdxTarget)
        {
            _mdl = mdl ?? throw new ArgumentNullException(nameof(mdl));
            _writer = RawBinaryWriter.ToAuto(mdlTarget);
            _mdxWriter = RawBinaryWriter.ToAuto(mdxTarget);
        }

        public void Write(bool autoClose = true)
        {
            try
            {
                // Minimal stub: real binary writing pending
                _writer.WriteUInt32(0);
                _mdxWriter.WriteUInt32(0);
            }
            finally
            {
                if (autoClose)
                {
                    Dispose();
                }
            }
        }

        public void Dispose()
        {
            _writer?.Dispose();
            _mdxWriter?.Dispose();
        }
    }
}
