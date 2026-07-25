using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using BioWare.Resource.Formats.LYT;
using OdyTools.Blender;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    public class OdyToolLYTTests
    {
        [Test]
        public async Task OdyToolLYT_Constructor_BuildsProgrammaticEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);

                    Assert.That(editor.HasProgrammaticEditorSurfaceForTest, Is.True);
                    Assert.That(editor.SummaryText, Does.Contain("Rooms 0"));
                    Assert.That(editor.ModelBrowser, Is.Not.Null);
                    Assert.That(editor.TextureBrowser, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_AddElements_UpdatesEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);

                    editor.AddRoom();
                    editor.AddObstacle();
                    editor.AddDoorHook();

                    Assert.That(editor.RoomCount, Is.EqualTo(1));
                    Assert.That(editor.ObstacleCount, Is.EqualTo(1));
                    Assert.That(editor.DoorHookCount, Is.EqualTo(1));
                    Assert.That(editor.SummaryText, Does.Contain("Rooms 1"));
                    Assert.That(editor.SummaryText, Does.Contain("Door hooks 1"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_SelectedRoom_CanBeEditedAndBuilt()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);
                    editor.AddRoom();

                    editor.SelectRoomForTesting(0);
                    editor.SetSelectionEditsForTesting("m01aa_01a", 12.5f, -3.25f, 2f);
                    editor.ApplySelectionEditsForTesting();

                    Tuple<byte[], byte[]> result = editor.Build();
                    var lyt = LYTAuto.ReadLyt(result.Item1);

                    Assert.That(editor.SelectionTitleText, Does.Contain("m01aa_01a"));
                    Assert.That(lyt.Rooms, Has.Count.EqualTo(1));
                    Assert.That(lyt.Rooms[0].Model, Is.EqualTo(new ResRef("m01aa_01a")));
                    Assert.That(lyt.Rooms[0].Position.X, Is.EqualTo(12.5f).Within(0.001f));
                    Assert.That(lyt.Rooms[0].Position.Y, Is.EqualTo(-3.25f).Within(0.001f));
                    Assert.That(lyt.Rooms[0].Position.Z, Is.EqualTo(2f).Within(0.001f));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_SelectedModelResRef_TrimsAndClearsInvalidValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);
                    editor.AddRoom();

                    editor.SelectRoomForTesting(0);
                    editor.SetSelectionEditsForTesting(" m01aa_02a ", 1f, 2f, 3f);
                    editor.ApplySelectionEditsForTesting();

                    var lyt = LYTAuto.ReadLyt(editor.Build().Item1);
                    Assert.That(lyt.Rooms[0].Model.ToString(), Is.EqualTo("m01aa_02a"));

                    editor.SetSelectionEditsForTesting("bad*room", 4f, 5f, 6f);
                    editor.ApplySelectionEditsForTesting();

                    lyt = LYTAuto.ReadLyt(editor.Build().Item1);
                    Assert.That(lyt.Rooms[0].Model.ToString(), Is.EqualTo("default_room"));
                    Assert.That(lyt.Rooms[0].Position.X, Is.EqualTo(4f).Within(0.001f));
                    Assert.That(OdyToolLYT.ResRefFromEditableText("bad*room").IsBlank(), Is.True);
                    Assert.That(OdyToolLYT.ResRefFromEditableText(" more_than_16_chars ").IsBlank(), Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_SceneRoomSelection_PopulatesEditableDetails()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);
                    editor.AddRoom();

                    editor.SelectRoomInSceneForTesting(0);

                    Assert.That(editor.SelectionTitleText, Does.Contain("default_room"));

                    editor.SetSelectionEditsForTesting("m02ab_01a", 3f, 4f, 5f);
                    editor.ApplySelectionEditsForTesting();

                    Tuple<byte[], byte[]> result = editor.Build();
                    var lyt = LYTAuto.ReadLyt(result.Item1);

                    Assert.That(lyt.Rooms, Has.Count.EqualTo(1));
                    Assert.That(lyt.Rooms[0].Model, Is.EqualTo(new ResRef("m02ab_01a")));
                    Assert.That(lyt.Rooms[0].Position.X, Is.EqualTo(3f).Within(0.001f));
                    Assert.That(lyt.Rooms[0].Position.Y, Is.EqualTo(4f).Within(0.001f));
                    Assert.That(lyt.Rooms[0].Position.Z, Is.EqualTo(5f).Within(0.001f));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_LoadFullLayout_BuildPreservesAndEditsDoorHook()
        {
            var source = new LYT();
            source.Rooms.Add(new LYTRoom(new ResRef("m12aa_01a"), new Vector3(1f, 2f, 3f)));
            source.Rooms.Add(new LYTRoom(new ResRef("m12aa_02a"), new Vector3(4f, 5f, 6f)));
            source.Tracks.Add(new LYTTrack(new ResRef("swoop_boost01"), new Vector3(7f, 8f, 9f)));
            source.Obstacles.Add(new LYTObstacle(new ResRef("swoop_barrier"), new Vector3(10f, 11f, 12f)));
            source.DoorHooks.Add(new LYTDoorHook("m12aa_01a", "door_m12aa", new Vector3(13f, 14f, 15f), new Vector4(0f, 0f, 0.7071068f, 0.7071068f)));

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);
                    editor.Load("test.lyt", "test", ResourceType.LYT, LYTAuto.BytesLyt(source));

                    Tuple<byte[], byte[]> preservedResult = editor.Build();
                    var preserved = LYTAuto.ReadLyt(preservedResult.Item1);

                    Assert.That(editor.RoomCount, Is.EqualTo(2));
                    Assert.That(editor.TrackCount, Is.EqualTo(1));
                    Assert.That(editor.ObstacleCount, Is.EqualTo(1));
                    Assert.That(editor.DoorHookCount, Is.EqualTo(1));
                    Assert.That(editor.SummaryText, Does.Contain("Rooms 2"));
                    Assert.That(editor.SummaryText, Does.Contain("Tracks 1"));
                    Assert.That(editor.SummaryText, Does.Contain("Obstacles 1"));
                    Assert.That(editor.SummaryText, Does.Contain("Door hooks 1"));
                    AssertFullLayoutPreserved(preserved, source);

                    editor.SelectDoorHookForTesting(0);
                    Assert.That(editor.SelectionTitleText, Does.Contain("m12aa_01a"));
                    editor.SetSelectionEditsForTesting("m12aa_02a", 16f, 17f, 18f, "door_m12aa_alt");
                    editor.ApplySelectionEditsForTesting();

                    Tuple<byte[], byte[]> editedResult = editor.Build();
                    var edited = LYTAuto.ReadLyt(editedResult.Item1);

                    Assert.That(edited.DoorHooks, Has.Count.EqualTo(1));
                    Assert.That(edited.DoorHooks[0].Room, Is.EqualTo("m12aa_02a"));
                    Assert.That(edited.DoorHooks[0].Door, Is.EqualTo("door_m12aa_alt"));
                    AssertVector(edited.DoorHooks[0].Position, 16f, 17f, 18f);
                    Assert.That(edited.DoorHooks[0].Orientation.X, Is.EqualTo(source.DoorHooks[0].Orientation.X).Within(0.0001f));
                    Assert.That(edited.DoorHooks[0].Orientation.Y, Is.EqualTo(source.DoorHooks[0].Orientation.Y).Within(0.0001f));
                    Assert.That(edited.DoorHooks[0].Orientation.Z, Is.EqualTo(source.DoorHooks[0].Orientation.Z).Within(0.0001f));
                    Assert.That(edited.DoorHooks[0].Orientation.W, Is.EqualTo(source.DoorHooks[0].Orientation.W).Within(0.0001f));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_SelectedImportedModel_AddsSelectableRoom()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);

                    editor.RegisterImportedModelForTesting("m13aa_01a", "/tmp/m13aa_01a.mdl");
                    editor.SelectImportedModelForTesting("m13aa_01a");

                    Assert.That(editor.SelectionTitleText, Does.Contain("Model: m13aa_01a"));
                    Assert.That(editor.AddSelectedModelButtonEnabledForTesting, Is.True);

                    editor.AddSelectedModelAsRoomForTesting();

                    Assert.That(editor.RoomCount, Is.EqualTo(1));
                    Assert.That(editor.SelectionTitleText, Does.Contain("m13aa_01a"));
                    Assert.That(editor.SelectedRoomListTextForTesting, Does.Contain("m13aa_01a"));
                    Assert.That(editor.AddSelectedModelButtonEnabledForTesting, Is.False);

                    var lyt = LYTAuto.ReadLyt(editor.Build().Item1);
                    Assert.That(lyt.Rooms, Has.Count.EqualTo(1));
                    Assert.That(lyt.Rooms[0].Model, Is.EqualTo(new ResRef("m13aa_01a")));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_GridVisibility_IsCustomizable()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);

                    editor.IsGridVisibleForTesting = false;

                    Assert.That(editor.IsGridVisibleForTesting, Is.False);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_GenerateWalkmesh_RequiresSavedOrLoadedLayoutPath()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);
                    editor.AddRoom();

                    int generated = editor.GenerateWalkmeshFilesForTesting();

                    Assert.That(generated, Is.EqualTo(0));
                    Assert.That(editor.StatusText, Does.Contain("Save or open"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_GenerateWalkmesh_WritesRoundtrippableRoomWokWithoutOverwriting()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "odytools-lyt-wok-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                string lytPath = Path.Combine(tempDir, "m13aa.lyt");
                var source = new LYT();
                source.Rooms.Add(new LYTRoom(new ResRef("m13aa_01a"), new Vector3(10f, 20f, 3f)));

                using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
                {
                    await session.Dispatch(() =>
                    {
                        var editor = new OdyToolLYT(null, null);
                        editor.Load(lytPath, "m13aa", ResourceType.LYT, LYTAuto.BytesLyt(source));

                        int generated = editor.GenerateWalkmeshFilesForTesting();
                        string wokPath = Path.Combine(tempDir, "m13aa_01a.wok");

                        Assert.That(generated, Is.EqualTo(1));
                        Assert.That(File.Exists(wokPath), Is.True);
                        var bwm = BWMAuto.ReadBwm(File.ReadAllBytes(wokPath));
                        Assert.That(bwm.WalkmeshType, Is.EqualTo(BWMType.AreaModel));
                        Assert.That(bwm.Faces, Has.Count.EqualTo(2));
                        Assert.That(bwm.Faces.All(face => face.Material == SurfaceMaterial.Stone), Is.True);
                        Assert.That(bwm.Faces[0].V1.X, Is.EqualTo(5f).Within(0.001f));
                        Assert.That(bwm.Faces[0].V1.Y, Is.EqualTo(15f).Within(0.001f));
                        Assert.That(bwm.Faces[0].V1.Z, Is.EqualTo(3f).Within(0.001f));

                        File.WriteAllBytes(wokPath, new byte[] { 1, 2, 3 });
                        generated = editor.GenerateWalkmeshFilesForTesting();

                        Assert.That(generated, Is.EqualTo(0));
                        Assert.That(File.ReadAllBytes(wokPath), Is.EqualTo(new byte[] { 1, 2, 3 }));
                        Assert.That(editor.StatusText, Does.Contain("skipped"));
                    }, CancellationToken.None);
                }
            }
            finally
            {
                Directory.Delete(tempDir, recursive: true);
            }
        }

        [Test]
        public async Task OdyToolLYT_OpenInBlenderAction_EnablesAfterLoadAndUsesLayoutPath()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);
                    Assert.That(editor.OpenInBlenderMenuItem, Is.Not.Null);
                    Assert.That(editor.OpenInBlenderMenuItem.IsEnabled, Is.False);
                    Assert.That(editor.BlenderStatusText, Does.Contain("Open a LYT"));

                    var source = new LYT();
                    source.Rooms.Add(new LYTRoom(new ResRef("m13aa_01a"), new Vector3(1f, 2f, 3f)));
                    var path = "/tmp/test_layout.lyt";
                    string launchedModulePath = null;

                    editor.Load(path, "test_layout", ResourceType.LYT, LYTAuto.BytesLyt(source));
                    editor.SetBlenderServicesForTests(
                        _ => new BlenderInfo
                        {
                            IsValid = true,
                            HasKotorblender = true,
                            Executable = "/usr/bin/blender",
                            Version = (4, 2, 0)
                        },
                        (info, port, installationPath, modulePath, tempDir, backgroundLoop) =>
                        {
                            launchedModulePath = modulePath;
                            return new System.Diagnostics.Process();
                        });

                    Assert.That(editor.OpenInBlenderMenuItem.IsEnabled, Is.True);
                    Assert.That(editor.TryLaunchBlenderForCurrentLayout(), Is.True);
                    Assert.That(launchedModulePath, Is.EqualTo(path));
                    Assert.That(editor.BlenderStatusText, Does.Contain("Launched Blender"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLYT_OpenInBlenderAction_ReportsMissingKotorblenderWithoutLaunching()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLYT(null, null);
                    var source = new LYT();
                    source.Rooms.Add(new LYTRoom(new ResRef("m13aa_01a"), new Vector3(1f, 2f, 3f)));
                    editor.Load("/tmp/test_layout.lyt", "test_layout", ResourceType.LYT, LYTAuto.BytesLyt(source));

                    editor.SetBlenderServicesForTests(
                        _ => new BlenderInfo
                        {
                            IsValid = true,
                            HasKotorblender = false,
                            Executable = "/usr/bin/blender",
                            Version = (4, 2, 0),
                            Error = "Blender 4.2.0 found but kotorblender add-on is not installed."
                        },
                        (info, port, installationPath, modulePath, tempDir, backgroundLoop) =>
                        {
                            Assert.Fail("Blender should not launch without kotorblender.");
                            return null;
                        });

                    Assert.That(editor.TryLaunchBlenderForCurrentLayout(), Is.False);
                    Assert.That(editor.BlenderStatusText, Does.Contain("kotorblender"));
                }, CancellationToken.None);
            }
        }

        private static void AssertFullLayoutPreserved(LYT actual, LYT expected)
        {
            Assert.That(actual.Rooms, Has.Count.EqualTo(expected.Rooms.Count));
            Assert.That(actual.Tracks, Has.Count.EqualTo(expected.Tracks.Count));
            Assert.That(actual.Obstacles, Has.Count.EqualTo(expected.Obstacles.Count));
            Assert.That(actual.DoorHooks, Has.Count.EqualTo(expected.DoorHooks.Count));

            for (int i = 0; i < expected.Rooms.Count; i++)
            {
                Assert.That(actual.Rooms[i].Model, Is.EqualTo(expected.Rooms[i].Model), "Room model " + i);
                AssertVector(actual.Rooms[i].Position, expected.Rooms[i].Position.X, expected.Rooms[i].Position.Y, expected.Rooms[i].Position.Z);
            }

            for (int i = 0; i < expected.Tracks.Count; i++)
            {
                Assert.That(actual.Tracks[i].Model, Is.EqualTo(expected.Tracks[i].Model), "Track model " + i);
                AssertVector(actual.Tracks[i].Position, expected.Tracks[i].Position.X, expected.Tracks[i].Position.Y, expected.Tracks[i].Position.Z);
            }

            for (int i = 0; i < expected.Obstacles.Count; i++)
            {
                Assert.That(actual.Obstacles[i].Model, Is.EqualTo(expected.Obstacles[i].Model), "Obstacle model " + i);
                AssertVector(actual.Obstacles[i].Position, expected.Obstacles[i].Position.X, expected.Obstacles[i].Position.Y, expected.Obstacles[i].Position.Z);
            }

            for (int i = 0; i < expected.DoorHooks.Count; i++)
            {
                Assert.That(actual.DoorHooks[i].Room, Is.EqualTo(expected.DoorHooks[i].Room), "Door hook room " + i);
                Assert.That(actual.DoorHooks[i].Door, Is.EqualTo(expected.DoorHooks[i].Door), "Door hook door " + i);
                AssertVector(actual.DoorHooks[i].Position, expected.DoorHooks[i].Position.X, expected.DoorHooks[i].Position.Y, expected.DoorHooks[i].Position.Z);
                Assert.That(actual.DoorHooks[i].Orientation.X, Is.EqualTo(expected.DoorHooks[i].Orientation.X).Within(0.0001f), "Door hook orientation X " + i);
                Assert.That(actual.DoorHooks[i].Orientation.Y, Is.EqualTo(expected.DoorHooks[i].Orientation.Y).Within(0.0001f), "Door hook orientation Y " + i);
                Assert.That(actual.DoorHooks[i].Orientation.Z, Is.EqualTo(expected.DoorHooks[i].Orientation.Z).Within(0.0001f), "Door hook orientation Z " + i);
                Assert.That(actual.DoorHooks[i].Orientation.W, Is.EqualTo(expected.DoorHooks[i].Orientation.W).Within(0.0001f), "Door hook orientation W " + i);
            }
        }

        private static void AssertVector(Vector3 actual, float x, float y, float z)
        {
            Assert.That(actual.X, Is.EqualTo(x).Within(0.0001f), "X");
            Assert.That(actual.Y, Is.EqualTo(y).Within(0.0001f), "Y");
            Assert.That(actual.Z, Is.EqualTo(z).Within(0.0001f), "Z");
        }
    }
}
