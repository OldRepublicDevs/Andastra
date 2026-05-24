using System.Collections.Generic;
using System.Numerics;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using NUnit.Framework;
using OdyTools.Data;
using DataKit = OdyTools.Data.Kit;

namespace OdyTools.Tests
{
    /// <summary>
    /// Indoor Map Builder walkmesh characterization tests (plan 063 U3 phase B).
    /// </summary>
    [TestFixture]
    public class IndoorMapBuildWalkmeshTests
    {
        private static DataKit CreateBuildKit()
        {
            var kit = new DataKit("WalkmeshKit");
            var bwm = new BWM();
            var face1 = new BWMFace(new Vector3(0, 0, 0), new Vector3(10, 0, 0), new Vector3(10, 10, 0));
            face1.Material = SurfaceMaterial.Stone;
            var face2 = new BWMFace(new Vector3(0, 0, 0), new Vector3(10, 10, 0), new Vector3(0, 10, 0));
            face2.Material = SurfaceMaterial.Stone;
            bwm.Faces.Add(face1);
            bwm.Faces.Add(face2);

            byte[] mdl = new byte[200];
            byte[] mdx = new byte[64];
            var component = new KitComponent(kit, "WalkComponent", null, bwm, mdl, mdx);
            kit.Components.Add(component);
            return kit;
        }

        [Test]
        public void BuildWalkmeshForRoom_SerializesAsAreaModel()
        {
            DataKit kit = CreateBuildKit();
            var room = new IndoorMapRoom(kit.Components[0], new Vector3(2, 3, 0), 30f);
            var rooms = new List<IndoorMapRoom> { room };

            BWM bwm = IndoorMap.BuildWalkmeshForRoom(room, rooms);
            byte[] bytes = BWMAuto.BytesBwm(bwm);
            BWM roundtrip = BWMAuto.ReadBwm(bytes);

            Assert.That(roundtrip.WalkmeshType, Is.EqualTo(BWMType.AreaModel));
            Assert.That(roundtrip.Faces.Count, Is.EqualTo(2));
        }

        [Test]
        public void BuildWalkmeshForRoom_PlaceableSourceBecomesAreaModel()
        {
            DataKit kit = CreateBuildKit();
            kit.Components[0].Bwm.WalkmeshType = BWMType.PlaceableOrDoor;
            var room = new IndoorMapRoom(kit.Components[0], Vector3.Zero, 0f);
            var rooms = new List<IndoorMapRoom> { room };

            BWM bwm = IndoorMap.BuildWalkmeshForRoom(room, rooms);

            Assert.That(bwm.WalkmeshType, Is.EqualTo(BWMType.AreaModel));
        }
    }
}
