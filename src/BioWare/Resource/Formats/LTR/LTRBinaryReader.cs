using System;
using System.IO;
using BioWare.Resource.Formats.LTR;

namespace BioWare.Resource.Formats.LTR
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/ltr/io_ltr.py:14-104
    // Original: class LTRBinaryReader(ResourceReader)
    public class LTRBinaryReader : IDisposable
    {
        private readonly BioWare.Common.RawBinaryReader _reader;
        private LTR _ltr;

        public LTRBinaryReader(byte[] data, int offset = 0, int size = 0)
        {
            int? sizeNullable = size > 0 ? (int?)size : null;
            _reader = BioWare.Common.RawBinaryReader.FromBytes(data, offset, sizeNullable);
        }

        public LTRBinaryReader(string filepath, int offset = 0, int size = 0)
        {
            int? sizeNullable = size > 0 ? (int?)size : null;
            _reader = BioWare.Common.RawBinaryReader.FromFile(filepath, offset, sizeNullable);
        }

        public LTRBinaryReader(Stream source, int offset = 0, int size = 0)
        {
            int? sizeNullable = size > 0 ? (int?)size : null;
            _reader = BioWare.Common.RawBinaryReader.FromStream(source, offset, sizeNullable);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/ltr/io_ltr.py:40-104
        // Original: def load(self, auto_close: bool = True) -> LTR
        public LTR Load(bool autoClose = true)
        {
            try
            {
                _ltr = new LTR();

                string fileType = _reader.ReadString(4);
                string fileVersion = _reader.ReadString(4);

                if (fileType != "LTR ")
                {
                    throw new ArgumentException("The file type that was loaded is invalid.");
                }

                if (fileVersion != "V1.0")
                {
                    throw new ArgumentException("The LTR version that was loaded is not supported.");
                }

                byte letterCount = _reader.ReadUInt8();
                if (letterCount != LTR.NumCharacters)
                {
                    throw new ArgumentException("LTR files that do not handle exactly " + LTR.NumCharacters + " characters are not supported.");
                }

                // Read single-letter probability block
                for (int i = 0; i < LTR.NumCharacters; i++)
                {
                    _ltr.Singles.Start[i] = _reader.ReadSingle();
                }
                for (int i = 0; i < LTR.NumCharacters; i++)
                {
                    _ltr.Singles.Middle[i] = _reader.ReadSingle();
                }
                for (int i = 0; i < LTR.NumCharacters; i++)
                {
                    _ltr.Singles.End[i] = _reader.ReadSingle();
                }

                // Read double-letter probability blocks
                for (int i = 0; i < LTR.NumCharacters; i++)
                {
                    for (int j = 0; j < LTR.NumCharacters; j++)
                    {
                        _ltr.Doubles[i].Start[j] = _reader.ReadSingle();
                    }
                    for (int j = 0; j < LTR.NumCharacters; j++)
                    {
                        _ltr.Doubles[i].Middle[j] = _reader.ReadSingle();
                    }
                    for (int j = 0; j < LTR.NumCharacters; j++)
                    {
                        _ltr.Doubles[i].End[j] = _reader.ReadSingle();
                    }
                }

                // Read triple-letter probability blocks
                for (int i = 0; i < LTR.NumCharacters; i++)
                {
                    for (int j = 0; j < LTR.NumCharacters; j++)
                    {
                        for (int k = 0; k < LTR.NumCharacters; k++)
                        {
                            _ltr.Triples[i][j].Start[k] = _reader.ReadSingle();
                        }
                        for (int k = 0; k < LTR.NumCharacters; k++)
                        {
                            _ltr.Triples[i][j].Middle[k] = _reader.ReadSingle();
                        }
                        for (int k = 0; k < LTR.NumCharacters; k++)
                        {
                            _ltr.Triples[i][j].End[k] = _reader.ReadSingle();
                        }
                    }
                }

                return _ltr;
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

