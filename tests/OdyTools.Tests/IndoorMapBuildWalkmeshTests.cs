using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.ARE;
using BioWare.Resource.Formats.LYT;
using BioWare.Resource.Formats.VIS;
using NUnit.Framework;
using IndoorMapIo = BioWare.Tools.IndoorMapIo;
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

        private static KitComponent CreateHookedComponent(DataKit kit, string name, int dummyTransition)
        {
            var bwm = new BWM();
            var face = new BWMFace(new Vector3(0, 0, 0), new Vector3(10, 0, 0), new Vector3(0, 10, 0));
            face.Material = SurfaceMaterial.Stone;
            face.Trans1 = dummyTransition;
            bwm.Faces.Add(face);

            byte[] mdl = new byte[200];
            byte[] mdx = new byte[64];
            var component = new KitComponent(kit, name, null, bwm, mdl, mdx);
            component.Hooks.Add(new KitComponentHook(Vector3.Zero, 0f, dummyTransition, null));
            kit.Components.Add(component);
            return component;
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
        public void BuildWalkmeshForRoom_AppliesRoomRotationAndTranslationToVertices()
        {
            DataKit kit = CreateBuildKit();
            var room = new IndoorMapRoom(kit.Components[0], new Vector3(2, 3, 4), 90f);
            var rooms = new List<IndoorMapRoom> { room };

            BWM bwm = IndoorMap.BuildWalkmeshForRoom(room, rooms);
            var vertices = bwm.Vertices()
                .Select(vertex => new Vector3(
                    (float)Math.Round(vertex.X, 4),
                    (float)Math.Round(vertex.Y, 4),
                    (float)Math.Round(vertex.Z, 4)))
                .OrderBy(vertex => vertex.X)
                .ThenBy(vertex => vertex.Y)
                .ThenBy(vertex => vertex.Z)
                .ToArray();

            Assert.That(vertices, Is.EqualTo(new[]
            {
                new Vector3(-8, 3, 4),
                new Vector3(-8, 13, 4),
                new Vector3(2, 3, 4),
                new Vector3(2, 13, 4)
            }));
            Assert.That(bwm.Faces.All(face => face.Material == SurfaceMaterial.Stone), Is.True);
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

        [Test]
        public void BuildWalkmeshArchive_WritesReloadableRoomWokResource()
        {
            DataKit kit = CreateBuildKit();
            var map = new IndoorMap { ModuleId = "wokmod" };
            map.Rooms.Add(new IndoorMapRoom(kit.Components[0], new Vector3(4, 5, 0), 0f));

            ERF mod = map.BuildWalkmeshArchiveForTesting();
            string tempPath = Path.Combine(Path.GetTempPath(), "indoor_wok_" + Guid.NewGuid().ToString("N") + ".mod");
            try
            {
                ERFAuto.WriteErf(mod, tempPath, ResourceType.MOD);
                ERF roundtripMod = ERFAuto.ReadErf(tempPath);
                byte[] wokData = roundtripMod.Get("wokmod_room0", ResourceType.WOK);

                Assert.That(wokData, Is.Not.Null);
                BWM roundtripWok = BWMAuto.ReadBwm(wokData);
                Assert.That(roundtripWok.WalkmeshType, Is.EqualTo(BWMType.AreaModel));
                Assert.That(roundtripWok.Faces.Count, Is.EqualTo(2));
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
        }

        [Test]
        public void BuildWalkmeshArchive_ConnectedRoomsRemapHookTransitionsBothWays()
        {
            var kit = new DataKit("ConnectedWalkmeshKit");
            KitComponent firstComponent = CreateHookedComponent(kit, "FirstRoom", 5);
            KitComponent secondComponent = CreateHookedComponent(kit, "SecondRoom", 7);

            var firstRoom = new IndoorMapRoom(firstComponent, Vector3.Zero, 0f);
            var secondRoom = new IndoorMapRoom(secondComponent, Vector3.Zero, 0f);
            var map = new IndoorMap { ModuleId = "linkmod" };
            map.Rooms.Add(firstRoom);
            map.Rooms.Add(secondRoom);
            map.RebuildRoomConnections();

            ERF mod = map.BuildWalkmeshArchiveForTesting();
            BWM firstWok = BWMAuto.ReadBwm(mod.Get("linkmod_room0", ResourceType.WOK));
            BWM secondWok = BWMAuto.ReadBwm(mod.Get("linkmod_room1", ResourceType.WOK));

            Assert.That(firstRoom.Hooks[0], Is.SameAs(secondRoom));
            Assert.That(secondRoom.Hooks[0], Is.SameAs(firstRoom));
            Assert.That(firstWok.Faces[0].Trans1, Is.EqualTo(1), "Room 0 should transition through its hook to room index 1.");
            Assert.That(secondWok.Faces[0].Trans1, Is.EqualTo(0), "Room 1 should transition through its hook back to room index 0.");
        }

        [Test]
        public void BuildModuleMetadataArchive_WritesCoreModuleResourcesReferencingRoomWoks()
        {
            DataKit kit = CreateBuildKit();
            var map = new IndoorMap { ModuleId = "metamod", WarpPoint = new Vector3(3, 4, 5) };
            map.Rooms.Add(new IndoorMapRoom(kit.Components[0], new Vector3(0, 0, 0), 0f));
            map.Rooms.Add(new IndoorMapRoom(kit.Components[0], new Vector3(12, 0, 0), 0f));

            ERF mod = map.BuildModuleMetadataArchiveForTesting();
            string tempPath = Path.Combine(Path.GetTempPath(), "indoor_meta_" + Guid.NewGuid().ToString("N") + ".mod");
            try
            {
                ERFAuto.WriteErf(mod, tempPath, ResourceType.MOD);
                ERF roundtripMod = ERFAuto.ReadErf(tempPath);

                byte[] lytData = roundtripMod.Get("metamod", ResourceType.LYT);
                byte[] visData = roundtripMod.Get("metamod", ResourceType.VIS);
                byte[] areData = roundtripMod.Get("metamod", ResourceType.ARE);
                byte[] gitData = roundtripMod.Get("metamod", ResourceType.GIT);
                byte[] ifoData = roundtripMod.Get("module", ResourceType.IFO);

                Assert.That(lytData, Is.Not.Null);
                Assert.That(visData, Is.Not.Null);
                Assert.That(areData, Is.Not.Null);
                Assert.That(gitData, Is.Not.Null);
                Assert.That(ifoData, Is.Not.Null);

                LYT lyt = LYTAuto.ReadLyt(lytData);
                Assert.That(lyt.Rooms.Select(room => room.Model.ToString()).ToArray(),
                    Is.EquivalentTo(new[] { "metamod_room0", "metamod_room1" }));

                foreach (LYTRoom room in lyt.Rooms)
                {
                    byte[] wokData = roundtripMod.Get(room.Model.ToString(), ResourceType.WOK);
                    Assert.That(wokData, Is.Not.Null, "Missing WOK for LYT room " + room.Model);
                    Assert.That(BWMAuto.ReadBwm(wokData).WalkmeshType, Is.EqualTo(BWMType.AreaModel));
                }

                VIS vis = VISAuto.ReadVis(visData);
                Assert.That(vis.AllRooms(), Is.EquivalentTo(new[] { "metamod_room0", "metamod_room1" }));
                Assert.That(vis.GetVisible("metamod_room0", "metamod_room1"), Is.True);
                Assert.That(vis.GetVisible("metamod_room1", "metamod_room0"), Is.True);

                ARE are = AREHelpers.ConstructAre(GFF.FromBytes(areData));
                Assert.That(are.Tag, Is.EqualTo("metamod"));

                GIT git = GITHelpers.ConstructGit(GFF.FromBytes(gitData));
                Assert.That(git, Is.Not.Null);

                IFO ifo = IFOHelpers.ConstructIfo(GFF.FromBytes(ifoData));
                Assert.That(ifo.Tag, Is.EqualTo("metamod"));
                Assert.That(ifo.ResRef.ToString(), Is.EqualTo("metamod"));
                Assert.That(ifo.EntryX, Is.EqualTo(3).Within(0.001));
                Assert.That(ifo.EntryY, Is.EqualTo(4).Within(0.001));
                Assert.That(ifo.EntryZ, Is.EqualTo(5).Within(0.001));

                byte[] embedded = IndoorMapIo.TryExtractEmbeddedIndoorJsonFromModuleFiles(new[] { tempPath });
                Assert.That(embedded, Is.Not.Null);
                var reloaded = new IndoorMap();
                List<MissingRoomInfo> missing = reloaded.Load(embedded, new List<DataKit> { kit });
                Assert.That(missing, Is.Empty);
                Assert.That(reloaded.ModuleId, Is.EqualTo("metamod"));
                Assert.That(reloaded.Rooms.Count, Is.EqualTo(2));
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
        }
    }
}
