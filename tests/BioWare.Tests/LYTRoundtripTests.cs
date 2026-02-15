using System.Numerics;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.LYT;
using NUnit.Framework;

namespace BioWare.Tests
{
    /// <summary>
    /// LYT serialization roundtrip tests. Validates that LYT -> bytes -> LYT preserves data.
    /// </summary>
    public class LYTRoundtripTests
    {
        [Test]
        public void LYT_Roundtrip_EmptyLayout()
        {
            var lyt = new LYT();
            byte[] data = LYTAuto.BytesLyt(lyt, ResourceType.LYT);
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));

            LYT loaded = LYTAuto.ReadLyt(data);
            Assert.That(loaded.Rooms.Count, Is.EqualTo(0));
            Assert.That(loaded.Tracks.Count, Is.EqualTo(0));
            Assert.That(loaded.Obstacles.Count, Is.EqualTo(0));
            Assert.That(loaded.DoorHooks.Count, Is.EqualTo(0));
        }

        [Test]
        public void LYT_Roundtrip_PreservesRoom()
        {
            var lyt = new LYT();
            lyt.Rooms.Add(new LYTRoom
            {
                Model = new ResRef("room_01"),
                Position = new Vector3(10f, 20f, 0f)
            });

            byte[] data = LYTAuto.BytesLyt(lyt, ResourceType.LYT);
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));

            LYT loaded = LYTAuto.ReadLyt(data);
            Assert.That(loaded.Rooms.Count, Is.EqualTo(1));
            Assert.That(loaded.Rooms[0].Model.ToString(), Is.EqualTo("room_01"));
            Assert.That(loaded.Rooms[0].Position.X, Is.EqualTo(10f).Within(0.0001f));
            Assert.That(loaded.Rooms[0].Position.Y, Is.EqualTo(20f).Within(0.0001f));
            Assert.That(loaded.Rooms[0].Position.Z, Is.EqualTo(0f).Within(0.0001f));
        }
    }
}
