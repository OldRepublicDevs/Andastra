using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF.Generics;
using NUnit.Framework;
using IndoorMapIo = BioWare.Tools.IndoorMapIo;
using OdyTools.Data;
using OdyTools.Blender;
using OdyTools.Windows;
using DataKit = OdyTools.Data.Kit;

namespace OdyTools.Tests
{
    [TestFixture]
    public class IndoorMapWriteLoadTests
    {
        private static DataKit CreateTestKit()
        {
            var kit = new DataKit("TestKit");
            var bwm = new BWM();
            var face1 = new BWMFace(new Vector3(0, 0, 0), new Vector3(10, 0, 0), new Vector3(10, 10, 0));
            face1.Material = SurfaceMaterial.Stone;
            var face2 = new BWMFace(new Vector3(0, 0, 0), new Vector3(10, 10, 0), new Vector3(0, 10, 0));
            face2.Material = SurfaceMaterial.Stone;
            bwm.Faces.Add(face1);
            bwm.Faces.Add(face2);

            var component = new KitComponent(kit, "TestComponent", null, bwm, System.Text.Encoding.UTF8.GetBytes("mdl"), System.Text.Encoding.UTF8.GetBytes("mdx"));
            kit.Components.Add(component);
            return kit;
        }

        [Test]
        public void WriteLoad_RoundtripsRoomData()
        {
            DataKit kit = CreateTestKit();
            var kits = new List<DataKit> { kit };
            var map = new IndoorMap { ModuleId = "test01" };
            map.Rooms.Add(new IndoorMapRoom(kit.Components[0], new Vector3(5, 5, 0), 45f));

            byte[] raw = map.Write();
            var loaded = new IndoorMap();
            List<MissingRoomInfo> missing = loaded.Load(raw, kits);

            Assert.That(missing, Is.Empty);
            Assert.That(loaded.ModuleId, Is.EqualTo("test01"));
            Assert.That(loaded.Rooms.Count, Is.EqualTo(1));
            Assert.That(loaded.Rooms[0].Component.Name, Is.EqualTo("TestComponent"));
            Assert.That(loaded.Rooms[0].Rotation, Is.EqualTo(45f).Within(0.01f));
        }

        [Test, Timeout(60000)]
        public async Task OpenModFromPath_LoadsEmbeddedIndoorJson()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    DataKit kit = CreateTestKit();
                    var kits = new List<DataKit> { kit };
                    var map = new IndoorMap { ModuleId = "embed01" };
                    map.Rooms.Add(new IndoorMapRoom(kit.Components[0], Vector3.Zero, 0f));
                    byte[] indoorJson = map.Write();

                    var mod = new ERF(ERFType.MOD);
                    IndoorMapIo.EmbedIndoorJson(mod, indoorJson);

                    string tempPath = Path.Combine(Path.GetTempPath(), "indoor_embed_" + Guid.NewGuid().ToString("N") + ".mod");
                    try
                    {
                        ERFAuto.WriteErf(mod, tempPath, ResourceType.MOD);

                        var window = new IndoorBuilderWindow(null, null);
                        window.SetKitsForTesting(kits);
                        bool loaded = window.OpenModFromPath(tempPath);

                        Assert.That(loaded, Is.True);
                        Assert.That(window.Map.ModuleId, Is.EqualTo("embed01"));
                        Assert.That(window.Map.Rooms.Count, Is.EqualTo(1));
                    }
                    finally
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task SaveMapToPath_WritesReloadableIndoorFile()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    DataKit kit = CreateTestKit();
                    var kits = new List<DataKit> { kit };
                    var window = new IndoorBuilderWindow(null, null);
                    window.SetKitsForTesting(kits);
                    window.Map.ModuleId = "save01";
                    window.Map.Rooms.Add(new IndoorMapRoom(kit.Components[0], new Vector3(1, 2, 0), 0f));

                    string tempPath = Path.Combine(Path.GetTempPath(), "indoor_save_" + Guid.NewGuid().ToString("N") + ".indoor");
                    try
                    {
                        window.SaveMapToPath(tempPath);
                        Assert.That(File.Exists(tempPath), Is.True);

                        var reloaded = new IndoorBuilderWindow(null, null);
                        reloaded.SetKitsForTesting(kits);
                        bool ok = reloaded.OpenFromPath(tempPath);
                        Assert.That(ok, Is.True);
                        Assert.That(reloaded.Map.ModuleId, Is.EqualTo("save01"));
                        Assert.That(reloaded.Map.Rooms.Count, Is.EqualTo(1));
                    }
                    finally
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task OpenInBlenderAction_EnablesAfterSaveAndUsesIndoorPath()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    DataKit kit = CreateTestKit();
                    var window = new IndoorBuilderWindow(null, null);
                    window.SetKitsForTesting(new List<DataKit> { kit });

                    Assert.That(window.Ui.ActionOpenInBlender, Is.Not.Null);
                    Assert.That(window.Ui.ActionOpenInBlenderEnabled, Is.False);
                    Assert.That(window.Ui.BlenderStatusText, Does.Contain("Save or open"));

                    window.Map.ModuleId = "blend01";
                    window.Map.Rooms.Add(new IndoorMapRoom(kit.Components[0], Vector3.Zero, 0f));

                    string tempPath = Path.Combine(Path.GetTempPath(), "indoor_blender_" + Guid.NewGuid().ToString("N") + ".indoor");
                    try
                    {
                        window.SaveMapToPath(tempPath);

                        bool launched = false;
                        string launchedPath = null;
                        window.SetBlenderServicesForTests(
                            _ =>
                            {
                                var info = new BlenderInfo
                                {
                                    Executable = "/usr/bin/blender",
                                    Version = (4, 2, 0),
                                    IsValid = true,
                                    HasKotorblender = true
                                };
                                info.UpdateVersionString();
                                return info;
                            },
                            (info, port, installationPath, modulePath, blendFile, background) =>
                            {
                                launched = true;
                                launchedPath = modulePath;
                                return System.Diagnostics.Process.GetCurrentProcess();
                            });

                        Assert.That(window.Ui.ActionOpenInBlenderEnabled, Is.True);
                        Assert.That(window.TryLaunchBlenderForCurrentMap(), Is.True);
                        Assert.That(launched, Is.True);
                        Assert.That(launchedPath, Is.EqualTo(tempPath));
                        Assert.That(window.Ui.BlenderStatusText, Does.Contain("Launched Blender"));
                    }
                    finally
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task OpenInBlenderAction_ReportsMissingKotorblenderWithoutLaunching()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    DataKit kit = CreateTestKit();
                    var window = new IndoorBuilderWindow(null, null);
                    window.SetKitsForTesting(new List<DataKit> { kit });
                    window.Map.ModuleId = "blend02";
                    window.Map.Rooms.Add(new IndoorMapRoom(kit.Components[0], Vector3.Zero, 0f));

                    string tempPath = Path.Combine(Path.GetTempPath(), "indoor_blender_missing_" + Guid.NewGuid().ToString("N") + ".indoor");
                    try
                    {
                        window.SaveMapToPath(tempPath);
                        window.SetBlenderServicesForTests(
                            _ =>
                            {
                                var info = new BlenderInfo
                                {
                                    Executable = "/usr/bin/blender",
                                    Version = (4, 2, 0),
                                    IsValid = true,
                                    HasKotorblender = false,
                                    Error = "Blender 4.2.0 found but kotorblender add-on is not installed."
                                };
                                info.UpdateVersionString();
                                return info;
                            },
                            (info, port, installationPath, modulePath, blendFile, background) =>
                            {
                                Assert.Fail("Blender should not launch without kotorblender.");
                                return null;
                            });

                        Assert.That(window.TryLaunchBlenderForCurrentMap(), Is.False);
                        Assert.That(window.Ui.BlenderStatusText, Does.Contain("kotorblender"));
                    }
                    finally
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task BuildMap_WithoutInstallation_ReturnsFalse()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var window = new IndoorBuilderWindow(null, null);
                    window.Map.ModuleId = "nobuild";
                    string tempPath = Path.Combine(Path.GetTempPath(), "indoor_nobuild_" + Guid.NewGuid().ToString("N") + ".mod");
                    try
                    {
                        bool built = window.BuildMap(tempPath);
                        Assert.That(built, Is.False);
                        Assert.That(File.Exists(tempPath), Is.False);
                    }
                    finally
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                }, CancellationToken.None);
            }
        }
    }
}
