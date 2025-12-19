using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Andastra.Parsing;
using Andastra.Parsing.Formats.MDLData;
using Andastra.Parsing.Formats.MDL;

namespace Andastra.Parsing.Formats.MDL
{
    // Simplified port of PyKotor io_mdl_ascii.MDLAsciiReader
    public class MDLAsciiReader : IDisposable
    {
        private readonly Andastra.Parsing.Common.RawBinaryReader _reader;
        private MDLData.MDL _mdl;

        public MDLAsciiReader(byte[] data, int offset = 0, int size = 0)
        {
            _reader = Andastra.Parsing.Common.RawBinaryReader.FromBytes(data, offset, size > 0 ? size : (int?)null);
        }

        public MDLAsciiReader(string filepath, int offset = 0, int size = 0)
        {
            _reader = Andastra.Parsing.Common.RawBinaryReader.FromFile(filepath, offset, size > 0 ? size : (int?)null);
        }

        public MDLAsciiReader(Stream source, int offset = 0, int size = 0)
        {
            _reader = Andastra.Parsing.Common.RawBinaryReader.FromStream(source, offset, size > 0 ? size : (int?)null);
        }

        public MDLData.MDL Load(bool autoClose = true)
        {
            try
            {
                // Minimal ASCII loader: reads entire file as text, not fully parsed.
                _mdl = new MDLData.MDL();
                string text = Encoding.ASCII.GetString(_reader.ReadAll());
                _mdl.Name = ""; // placeholder; full parsing not implemented
                return _mdl;
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

