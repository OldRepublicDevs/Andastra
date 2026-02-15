using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource.Formats.TwoDA;
using NUnit.Framework;

namespace BioWare.Tests
{
    /// <summary>
    /// TwoDA serialization roundtrip tests. Validates that TwoDA -> bytes -> TwoDA preserves data.
    /// </summary>
    public class TwoDARoundtripTests
    {
        [Test]
        public void TwoDA_Roundtrip_PreservesHeadersAndRows()
        {
            var twoDA = new TwoDA(new List<string> { "label", "name", "value" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["name"] = "first", ["value"] = "100" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["name"] = "second", ["value"] = "200" });

            byte[] data = TwoDAAuto.BytesTwoDA(twoDA);
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));

            TwoDA loaded = TwoDA.FromBytes(data);
            Assert.That(loaded.GetHeight(), Is.EqualTo(2));
            Assert.That(loaded.GetWidth(), Is.EqualTo(3));
            Assert.That(loaded.GetLabel(0), Is.EqualTo("0"));
            Assert.That(loaded.GetLabel(1), Is.EqualTo("1"));
            Assert.That(loaded.GetCellString(0, "name"), Is.EqualTo("first"));
            Assert.That(loaded.GetCellString(1, "value"), Is.EqualTo("200"));
        }

        [Test]
        public void TwoDA_Roundtrip_EmptyRows()
        {
            var twoDA = new TwoDA(new List<string> { "col1", "col2" });
            twoDA.AddRow("0", null);
            twoDA.AddRow("1", new Dictionary<string, object> { ["col1"] = "a", ["col2"] = "b" });

            byte[] data = twoDA.ToBytes();
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));

            TwoDA loaded = TwoDA.FromBytes(data);
            Assert.That(loaded.GetHeight(), Is.EqualTo(2));
            Assert.That(loaded.GetCellString(1, "col1"), Is.EqualTo("a"));
        }

        [Test]
        public void TwoDA_CSV_Roundtrip_PreservesData()
        {
            var twoDA = new TwoDA(new List<string> { "label", "name", "value" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["name"] = "first", ["value"] = "100" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["name"] = "second", ["value"] = "200" });

            byte[] csvBytes = TwoDAAuto.Bytes2DA(twoDA, ResourceType.TwoDA_CSV);
            Assert.That(csvBytes, Is.Not.Null.And.Length.GreaterThan(0));

            TwoDA loaded = TwoDACsvReader.Load(csvBytes);
            Assert.That(loaded.GetHeight(), Is.EqualTo(2));
            Assert.That(loaded.GetWidth(), Is.EqualTo(3));
            Assert.That(loaded.GetLabel(0), Is.EqualTo("0"));
            Assert.That(loaded.GetLabel(1), Is.EqualTo("1"));
            Assert.That(loaded.GetCellString(0, "name"), Is.EqualTo("first"));
            Assert.That(loaded.GetCellString(1, "value"), Is.EqualTo("200"));
        }
    }
}
