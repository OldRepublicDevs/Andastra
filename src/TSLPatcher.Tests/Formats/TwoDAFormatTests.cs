using System;
using FluentAssertions;
using TSLPatcher.Core.Formats.TwoDA;
using Xunit;

namespace TSLPatcher.Tests.Formats;

/// <summary>
/// Tests for 2DA binary I/O.
/// 1:1 port of Python test_twoda.py from tests/resource/formats/test_twoda.py
/// </summary>
public class TwoDAFormatTests
{
    private const string TestTwoDAFile = "../../../../../../../tests/files/test.2da";

    [Fact]
    public void TestBinaryIO()
    {
        // Read 2DA file
        TwoDA twoda = new TwoDABinaryReader(TestTwoDAFile).Load();
        ValidateIO(twoda);

        // Write and re-read to validate round-trip
        byte[] data = new TwoDABinaryWriter(twoda).Write();
        twoda = new TwoDABinaryReader(data).Load();
        ValidateIO(twoda);
    }

    private void ValidateIO(TwoDA twoda)
    {
        twoda.GetCellString(0, "col1").Should().Be("abc");
        twoda.GetCellString(0, "col2").Should().Be("def");
        twoda.GetCellString(0, "col3").Should().Be("ghi");

        twoda.GetCellString(1, "col1").Should().Be("def");
        twoda.GetCellString(1, "col2").Should().Be("ghi");
        twoda.GetCellString(1, "col3").Should().Be("123");

        twoda.GetCellString(2, "col1").Should().Be("123");
        twoda.GetCellString(2, "col2").Should().Be("");
        twoda.GetCellString(2, "col3").Should().Be("abc");
    }

    [Fact]
    public void TestReadRaises()
    {
        // Invalid file
        Action act = () => new TwoDABinaryReader("../../../../../../../tests/files/test_corrupted.2da").Load();
        act.Should().Throw<System.IO.InvalidDataException>();
    }
}

