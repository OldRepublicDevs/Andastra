using System;
using System.IO;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.MDLData;
using CSharpKOTOR.Formats.MDL;

namespace CSharpKOTOR.Formats.MDL
{
    // Simplified binary reader. Full parity with PyKotor io_mdl.py is pending.
    public class MDLBinaryReader : IDisposable
    {
        private readonly RawBinaryReader _reader;
        private readonly object _mdxSource;
        private readonly int _mdxOffset;
        private readonly int _mdxSize;
        private readonly bool _fastLoad;

        public MDLBinaryReader(object source, int offset = 0, int size = 0, object mdxSource = null, int mdxOffset = 0, int mdxSize = 0, bool fastLoad = false)
        {
            _reader = RawBinaryReader.FromAuto(source, offset, size > 0 ? (int?)size : null);
            _mdxSource = mdxSource;
            _mdxOffset = mdxOffset;
            _mdxSize = mdxSize;
            _fastLoad = fastLoad;
        }

        public MDLData.MDL Load(bool autoClose = true)
        {
            try
            {
                // Minimal stub: real parsing to be implemented for full parity.
                var mdl = new MDLData.MDL();
                // consume size
                _reader.Seek(0);
                return mdl;
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
            _reader?.Dispose();
        }
    }
}

