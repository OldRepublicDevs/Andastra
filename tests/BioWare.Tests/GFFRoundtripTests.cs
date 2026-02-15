using System;
using System.Linq;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using NUnit.Framework;

namespace BioWare.Tests
{
    /// <summary>
    /// GFF serialization roundtrip tests. Validates that GFF -> bytes -> GFF preserves data.
    /// </summary>
    public class GFFRoundtripTests
    {
        [Test]
        public void GFF_Roundtrip_PreservesPrimitiveFields()
        {
            var gff = new GFF(GFFContent.GFF);
            gff.Root.SetUInt8("u8", 255);
            gff.Root.SetInt8("i8", -128);
            gff.Root.SetUInt16("u16", 0xFFFF);
            gff.Root.SetInt16("i16", -32768);
            gff.Root.SetUInt32("u32", 0xFFFFFFFF);
            gff.Root.SetInt32("i32", -0x7FFFFFFF);
            gff.Root.SetSingle("f", 3.14f);
            gff.Root.SetDouble("d", 2.71828);
            gff.Root.SetString("s", "hello");
            gff.Root.SetResRef("r", new ResRef("model"));
            gff.Root.SetVector3("v3", new System.Numerics.Vector3(1, 2, 3));
            gff.Root.SetVector4("v4", new System.Numerics.Vector4(1, 2, 3, 4));
            gff.Root.SetBinary("bin", new byte[] { 0x00, 0x01, 0x02 });
            gff.Root.SetLocString("loc", LocalizedString.FromEnglish("test"));

            byte[] data = GFFAuto.BytesGff(gff, ResourceType.GFF);
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));

            GFF loaded = GFF.FromBytes(data);
            Assert.That(loaded.Root.GetUInt8("u8"), Is.EqualTo(255));
            Assert.That(loaded.Root.GetInt8("i8"), Is.EqualTo(-128));
            Assert.That(loaded.Root.GetUInt16("u16"), Is.EqualTo(0xFFFF));
            Assert.That(loaded.Root.GetInt16("i16"), Is.EqualTo(-32768));
            Assert.That(loaded.Root.GetUInt32("u32"), Is.EqualTo(0xFFFFFFFF));
            Assert.That(loaded.Root.GetInt32("i32"), Is.EqualTo(-0x7FFFFFFF));
            Assert.That(loaded.Root.GetSingle("f"), Is.EqualTo(3.14f).Within(0.0001));
            Assert.That(loaded.Root.GetDouble("d"), Is.EqualTo(2.71828).Within(0.00001));
            Assert.That(loaded.Root.GetString("s"), Is.EqualTo("hello"));
            Assert.That(loaded.Root.GetResRef("r").ToString(), Is.EqualTo("model"));
            var v3 = loaded.Root.GetVector3("v3");
            Assert.That(v3.X, Is.EqualTo(1f).Within(0.0001));
            Assert.That(v3.Y, Is.EqualTo(2f).Within(0.0001));
            Assert.That(v3.Z, Is.EqualTo(3f).Within(0.0001));
            var v4 = loaded.Root.GetVector4("v4");
            Assert.That(v4.W, Is.EqualTo(4f).Within(0.0001));
            Assert.That(loaded.Root.GetBinary("bin"), Is.EqualTo(new byte[] { 0x00, 0x01, 0x02 }));
            Assert.That(loaded.Root.GetLocString("loc").Get(Language.English, Gender.Male), Is.EqualTo("test"));
        }

        [Test]
        public void GFF_Roundtrip_PreservesStructAndList()
        {
            var gff = new GFF(GFFContent.GFF);
            var inner = new GFFStruct(5);
            inner.SetString("inner", "value");
            gff.Root.SetStruct("nest", inner);
            var list = new GFFList();
            var elem = list.Add(10);
            elem.SetInt32("x", 42);
            gff.Root.SetList("items", list);

            byte[] data = GFFAuto.BytesGff(gff, ResourceType.GFF);
            GFF loaded = GFF.FromBytes(data);
            var s = loaded.Root.GetStruct("nest");
            Assert.That(s.StructId, Is.EqualTo(5));
            Assert.That(s.GetString("inner"), Is.EqualTo("value"));
            var l = loaded.Root.GetList("items");
            Assert.That(l.Count, Is.EqualTo(1));
            Assert.That(l.At(0).StructId, Is.EqualTo(10));
            Assert.That(l.At(0).GetInt32("x"), Is.EqualTo(42));
        }

        [Test]
        public void GFF_Roundtrip_EmptyRoot()
        {
            var gff = new GFF(GFFContent.GFF);
            byte[] data = GFFAuto.BytesGff(gff, ResourceType.GFF);
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
            GFF loaded = GFF.FromBytes(data);
            Assert.That(loaded.Root.Count, Is.EqualTo(0));
        }

        [Test]
        public void GFF_Roundtrip_ListWithMultipleStructs()
        {
            var gff = new GFF(GFFContent.GFF);
            var list = new GFFList();
            for (int i = 0; i < 3; i++)
            {
                var elem = list.Add(i);
                elem.SetInt32("id", i * 10);
                elem.SetString("name", "item" + i);
            }
            gff.Root.SetList("entries", list);

            byte[] data = GFFAuto.BytesGff(gff, ResourceType.GFF);
            GFF loaded = GFF.FromBytes(data);
            var l = loaded.Root.GetList("entries");
            Assert.That(l.Count, Is.EqualTo(3));
            for (int i = 0; i < 3; i++)
            {
                Assert.That(l.At(i).StructId, Is.EqualTo(i));
                Assert.That(l.At(i).GetInt32("id"), Is.EqualTo(i * 10));
                Assert.That(l.At(i).GetString("name"), Is.EqualTo("item" + i));
            }
        }

        [Test]
        public void GFF_Roundtrip_NestedStructs()
        {
            var gff = new GFF(GFFContent.GFF);
            var a = new GFFStruct(1);
            var b = new GFFStruct(2);
            b.SetString("leaf", "value");
            a.SetStruct("child", b);
            gff.Root.SetStruct("root", a);

            byte[] data = GFFAuto.BytesGff(gff, ResourceType.GFF);
            GFF loaded = GFF.FromBytes(data);
            var r = loaded.Root.GetStruct("root");
            Assert.That(r.StructId, Is.EqualTo(1));
            var c = r.GetStruct("child");
            Assert.That(c.StructId, Is.EqualTo(2));
            Assert.That(c.GetString("leaf"), Is.EqualTo("value"));
        }
    }
}
