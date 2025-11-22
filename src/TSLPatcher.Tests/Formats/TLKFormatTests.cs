using System;
using System.IO;
using FluentAssertions;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Formats.TLK;
using Xunit;

namespace TSLPatcher.Tests.Formats;

/// <summary>
/// Tests for TLK binary I/O operations.
/// 1:1 port from tests/resource/formats/test_tlk.py
/// </summary>
public class TLKFormatTests
{
    private static readonly string BinaryTestFile = Path.Combine("..", "..", "..", "test_data", "test.tlk");
    private static readonly string CorruptBinaryTestFile = Path.Combine("..", "..", "..", "test_data", "test_corrupted.tlk");

    [Fact]
    public void TestBinaryIO()
    {
        // test_binary_io from Python
        var reader = new TLKBinaryReader(BinaryTestFile);
        TLK tlk = reader.Load();
        ValidateIO(tlk);

        var writer = new TLKBinaryWriter(tlk);
        byte[] data = writer.Write();

        var newReader = new TLKBinaryReader(data);
        TLK newTlk = newReader.Load();
        ValidateIO(newTlk);
    }

    private void ValidateIO(TLK tlk)
    {
        // Validate based on Python test expectations
        tlk.Language.Should().Be(Language.English);
        tlk.Entries.Count.Should().BeGreaterThan(0);

        // Test first entry
        TLKEntry firstEntry = tlk.Entries[0];
        firstEntry.Text.Should().NotBeNullOrEmpty();

        // Test that we can access entries by index
        tlk[0].Should().NotBeNull();
    }

    [Fact]
    public void TestReadRaises()
    {
        // test_read_raises from Python
        Action act1 = () => new TLKBinaryReader(".").Load();
        act1.Should().Throw<UnauthorizedAccessException>();

        Action act2 = () => new TLKBinaryReader("./thisfiledoesnotexist").Load();
        act2.Should().Throw<FileNotFoundException>();

        Action act3 = () => new TLKBinaryReader(CorruptBinaryTestFile).Load();
        act3.Should().Throw<InvalidDataException>().WithMessage("Attempted to load an invalid TLK file.");
    }
}

