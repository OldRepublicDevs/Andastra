using System.IO;
using NUnit.Framework;
using CSharpKOTOR.Common;

namespace CSharpKOTOR.Tests
{
    [TestFixture]
    public class StreamTests
    {
        [Test]
        public void TestBinaryReaderBasic()
        {
            // Test basic BinaryReader functionality
            byte[] data1 = { 0x01, 0x02, 0x00, 0x03, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            byte[] data2 = System.Text.Encoding.ASCII.GetBytes("helloworld\x00");

            using (var reader1 = BinaryReader.FromBytes(data1))
            {
                Assert.AreEqual(1, reader1.ReadUInt8());
                Assert.AreEqual(2, reader1.ReadUInt16());
                Assert.AreEqual(3U, reader1.ReadUInt32());
                Assert.AreEqual(4UL, reader1.ReadUInt64());
            }

            using (var reader2 = BinaryReader.FromBytes(data2))
            {
                string result = reader2.ReadString(10);
                Assert.AreEqual("helloworld", result);
                Assert.AreEqual(0, reader2.ReadUInt8()); // null terminator
            }
        }

        [Test]
        public void TestBinaryWriterBasic()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write((byte)1);
                writer.Write((ushort)2);
                writer.Write(3U);
                writer.Write(4UL);
                writer.WriteString("test");

                byte[] data = stream.ToArray();

                // Verify the written data
                using (var reader = BinaryReader.FromBytes(data))
                {
                    Assert.AreEqual(1, reader.ReadUInt8());
                    Assert.AreEqual(2, reader.ReadUInt16());
                    Assert.AreEqual(3U, reader.ReadUInt32());
                    Assert.AreEqual(4UL, reader.ReadUInt64());
                    Assert.AreEqual("test", reader.ReadString(4));
                }
            }
        }

        [Test]
        public void TestLocalizedStringReadWrite()
        {
            var originalLocString = new LocalizedString();
            originalLocString.SetData(Language.English, Gender.Male, "Hello World");
            originalLocString.SetData(Language.French, Gender.Female, "Bonjour le monde");
            originalLocString.StringRef = 12345;

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                writer.WriteLocalizedString(originalLocString);

                byte[] data = stream.ToArray();

                using (var reader = BinaryReader.FromBytes(data))
                {
                    LocalizedString readLocString = reader.ReadLocalizedString();

                    Assert.AreEqual(originalLocString.StringRef, readLocString.StringRef);
                    Assert.AreEqual("Hello World", readLocString.GetData(Language.English, Gender.Male));
                    Assert.AreEqual("Bonjour le monde", readLocString.GetData(Language.French, Gender.Female));
                }
            }
        }

        [Test]
        public void TestSeekAndPosition()
        {
            byte[] data = { 0x01, 0x02, 0x03, 0x04, 0x05 };

            using (var reader = BinaryReader.FromBytes(data))
            {
                Assert.AreEqual(0, reader.Position);
                Assert.AreEqual(1, reader.ReadUInt8());
                Assert.AreEqual(1, reader.Position);

                reader.Seek(3);
                Assert.AreEqual(3, reader.Position);
                Assert.AreEqual(4, reader.ReadUInt8());
                Assert.AreEqual(4, reader.Position);
            }
        }
    }
}
