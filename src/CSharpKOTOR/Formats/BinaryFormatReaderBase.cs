using System.IO;
using System.Text;
using JetBrains.Annotations;

namespace CSharpKOTOR.Formats
{

    /// <summary>
    /// Base class for binary format readers to eliminate duplicate constructor patterns.
    /// </summary>
    public abstract class BinaryFormatReaderBase
    {
        protected readonly byte[] Data;
        protected readonly BinaryReader Reader;

        protected BinaryFormatReaderBase(byte[] data, [CanBeNull] Encoding encoding = null)
        {
            Data = data;
            Reader = encoding != null
                ? new BinaryReader(new MemoryStream(data), encoding)
                : new BinaryReader(new MemoryStream(data));
        }

        protected BinaryFormatReaderBase(string filepath, [CanBeNull] Encoding encoding = null)
        {
            Data = File.ReadAllBytes(filepath);
            Reader = encoding != null
                ? new BinaryReader(new MemoryStream(Data), encoding)
                : new BinaryReader(new MemoryStream(Data));
        }

        protected BinaryFormatReaderBase(Stream source, [CanBeNull] Encoding encoding = null)
        {
            using (var ms = new MemoryStream())
            {
                source.CopyTo(ms);
                Data = ms.ToArray();
                Reader = encoding != null
                    ? new BinaryReader(new MemoryStream(Data), encoding)
                    : new BinaryReader(new MemoryStream(Data));
            }
        }
    }
}

