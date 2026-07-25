using System;
using System.Linq;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.UTC;
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

        [Test]
        public void UTC_DismantleRoundtrip_PreservesNarrowNumericFields()
        {
            var utc = new UTC
            {
                Tag = "test_creature",
                ResRef = new ResRef("test_creature"),
                Conversation = new ResRef("test_dlg"),
                Alignment = 75,
                Plot = true,
                NoPermDeath = true,
                Min1Hp = true,
                Disarmable = true,
                Strength = 16,
                Dexterity = 14,
                Constitution = 13,
                Intelligence = 12,
                Wisdom = 11,
                Charisma = 10,
                NaturalAc = 4,
                Hp = 30,
                CurrentHp = 24,
                MaxHp = 35,
                OnSpawn = new ResRef("k_spawn"),
                OnHeartbeat = new ResRef("k_heart"),
                OnDeath = new ResRef("k_death"),
                Comment = "edited creature"
            };

            byte[] data = GFFAuto.BytesGff(UTCHelpers.DismantleUtc(utc, BioWareGame.K2), ResourceType.UTC);
            var loaded = UTCHelpers.ConstructUtc(GFF.FromBytes(data));

            Assert.That(loaded.Tag, Is.EqualTo("test_creature"));
            Assert.That(loaded.ResRef.ToString(), Is.EqualTo("test_creature"));
            Assert.That(loaded.Conversation.ToString(), Is.EqualTo("test_dlg"));
            Assert.That(loaded.Alignment, Is.EqualTo(75));
            Assert.That(loaded.Plot, Is.True);
            Assert.That(loaded.NoPermDeath, Is.True);
            Assert.That(loaded.Min1Hp, Is.True);
            Assert.That(loaded.Disarmable, Is.True);
            Assert.That(loaded.Strength, Is.EqualTo(16));
            Assert.That(loaded.Dexterity, Is.EqualTo(14));
            Assert.That(loaded.Constitution, Is.EqualTo(13));
            Assert.That(loaded.Intelligence, Is.EqualTo(12));
            Assert.That(loaded.Wisdom, Is.EqualTo(11));
            Assert.That(loaded.Charisma, Is.EqualTo(10));
            Assert.That(loaded.NaturalAc, Is.EqualTo(4));
            Assert.That(loaded.Hp, Is.EqualTo(30));
            Assert.That(loaded.CurrentHp, Is.EqualTo(24));
            Assert.That(loaded.MaxHp, Is.EqualTo(35));
            Assert.That(loaded.OnSpawn.ToString(), Is.EqualTo("k_spawn"));
            Assert.That(loaded.OnHeartbeat.ToString(), Is.EqualTo("k_heart"));
            Assert.That(loaded.OnDeath.ToString(), Is.EqualTo("k_death"));
            Assert.That(loaded.Comment, Is.EqualTo("edited creature"));
        }

        [Test]
        public void GIT_DismantleRoundtrip_PreservesEditableInstanceTags()
        {
            var git = new GIT();
            git.Placeables.Add(new GITPlaceable
            {
                ResRef = new ResRef("plc_test"),
                Tag = "placeable_tag",
                Position = new System.Numerics.Vector3(1.5f, -2.5f, 3.25f),
                Bearing = 1.125f
            });
            git.Sounds.Add(new GITSound
            {
                ResRef = new ResRef("snd_test"),
                Tag = "sound_tag",
                Position = new System.Numerics.Vector3(4.5f, 5.5f, -6.5f)
            });

            byte[] data = GFFAuto.BytesGff(GITHelpers.DismantleGit(git, BioWareGame.K2), ResourceType.GIT);
            var loaded = GITHelpers.ConstructGit(GFF.FromBytes(data));

            Assert.That(loaded.Placeables, Has.Count.EqualTo(1));
            Assert.That(loaded.Placeables[0].ResRef.ToString(), Is.EqualTo("plc_test"));
            Assert.That(loaded.Placeables[0].Tag, Is.EqualTo("placeable_tag"));
            Assert.That(loaded.Placeables[0].Position.X, Is.EqualTo(1.5f).Within(0.001f));
            Assert.That(loaded.Placeables[0].Position.Y, Is.EqualTo(-2.5f).Within(0.001f));
            Assert.That(loaded.Placeables[0].Position.Z, Is.EqualTo(3.25f).Within(0.001f));
            Assert.That(loaded.Placeables[0].Bearing, Is.EqualTo(1.125f).Within(0.001f));

            Assert.That(loaded.Sounds, Has.Count.EqualTo(1));
            Assert.That(loaded.Sounds[0].ResRef.ToString(), Is.EqualTo("snd_test"));
            Assert.That(loaded.Sounds[0].Tag, Is.EqualTo("sound_tag"));
            Assert.That(loaded.Sounds[0].Position.X, Is.EqualTo(4.5f).Within(0.001f));
            Assert.That(loaded.Sounds[0].Position.Y, Is.EqualTo(5.5f).Within(0.001f));
            Assert.That(loaded.Sounds[0].Position.Z, Is.EqualTo(-6.5f).Within(0.001f));
        }
    }
}
