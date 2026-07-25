using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using Avalonia.Interactivity;
using Avalonia.Threading;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.MDLData;
using BioWare.Resource.Formats.NCS;
using BioWare.Resource.Formats.TPC;
using NUnit.Framework;
using OdyTools.Data;
using OdyTools.Editors;
using OdyTools.Windows;

namespace OdyTools.Tests
{
    public class MainWindowMenuTests
    {
        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void FileNewMenuExposesHolocronEditorSurfaceWithoutInstallation()
        {
            MainWindow window = null;
            try
            {
                window = new MainWindow();
                window.Show();
                Dispatcher.UIThread.RunJobs();

                var expectedItems = new Dictionary<string, string>
                {
                    { "actionNew2DA", "_2DA Table" },
                    { "actionNewARE", "_Area" },
                    { "actionNewBWM", "_Walkmesh" },
                    { "actionNewDLG", "_Dialog" },
                    { "actionNewERF", "_ERF / MOD / RIM" },
                    { "actionNewFAC", "_Faction" },
                    { "actionNewGFF", "_GFF" },
                    { "actionNewGIT", "G_IT" },
                    { "actionNewGUI", "_GUI" },
                    { "actionNewIFO", "_Module Info" },
                    { "actionNewJRL", "_Journal" },
                    { "actionNewLIP", "_LIP Sync" },
                    { "actionNewLTR", "L_TR" },
                    { "actionNewLYT", "_Layout" },
                    { "actionNewMDL", "_Model" },
                    { "actionNewNSS", "_Script" },
                    { "actionNewPTH", "_Path" },
                    { "actionNewSAV", "_Save Game" },
                    { "actionNewSSF", "Sound _Set" },
                    { "actionNewTLK", "_Talk Table" },
                    { "actionNewTPC", "_Texture" },
                    { "actionNewTXT", "Te_xt" },
                    { "actionNewUTC", "_Creature" },
                    { "actionNewUTD", "_Door" },
                    { "actionNewUTE", "_Encounter" },
                    { "actionNewUTI", "_Item" },
                    { "actionNewUTM", "_Merchant" },
                    { "actionNewUTP", "_Placeable" },
                    { "actionNewUTS", "_Sound" },
                    { "actionNewUTT", "_Trigger" },
                    { "actionNewUTW", "_Waypoint" },
                    { "actionNewWAV", "_Audio" },
                };

                foreach (var expected in expectedItems)
                {
                    var item = window.FindControl<MenuItem>(expected.Key);
                    Assert.That(item, Is.Not.Null, expected.Key);
                    Assert.That(item.Header?.ToString(), Is.EqualTo(expected.Value), expected.Key);
                    Assert.That(item.IsEnabled, Is.True, expected.Key);
                }
            }
            finally
            {
                WindowUtils.CloseAllWindows();
                window?.Close();
            }
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void FileNewMenuItemsLaunchEditorsWithoutInstallation()
        {
            MainWindow window = null;
            try
            {
                window = new MainWindow();
                window.Show();
                Dispatcher.UIThread.RunJobs();
                WindowUtils.CloseAllWindows();

                var menuItems = new[]
                {
                    "actionNew2DA",
                    "actionNewARE",
                    "actionNewBWM",
                    "actionNewDLG",
                    "actionNewERF",
                    "actionNewFAC",
                    "actionNewGFF",
                    "actionNewGIT",
                    "actionNewGUI",
                    "actionNewIFO",
                    "actionNewJRL",
                    "actionNewLIP",
                    "actionNewLTR",
                    "actionNewLYT",
                    "actionNewMDL",
                    "actionNewNSS",
                    "actionNewPTH",
                    "actionNewSAV",
                    "actionNewSSF",
                    "actionNewTLK",
                    "actionNewTPC",
                    "actionNewTXT",
                    "actionNewUTC",
                    "actionNewUTD",
                    "actionNewUTE",
                    "actionNewUTI",
                    "actionNewUTM",
                    "actionNewUTP",
                    "actionNewUTS",
                    "actionNewUTT",
                    "actionNewUTW",
                    "actionNewWAV",
                };

                foreach (var name in menuItems)
                {
                    var item = window.FindControl<MenuItem>(name);
                    Assert.That(item, Is.Not.Null, name);

                    item.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                    Dispatcher.UIThread.RunJobs();

                    var editors = WindowUtils.GetTrackedWindows().OfType<Editor>().ToList();
                    Assert.That(editors, Has.Count.EqualTo(1), name);
                    Assert.That(editors[0].IsVisible, Is.True, name);
                    WindowUtils.CloseAllWindows();
                }
            }
            finally
            {
                WindowUtils.CloseAllWindows();
                window?.Close();
            }
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void RecentFilesMenuOpensRoutedEditorWithoutInstallation()
        {
            var settings = new Settings("Global");
            var originalRecentFiles = settings.GetValue("RecentFiles", new List<string>());
            var path = Path.Combine(Path.GetTempPath(), "odytools-recent-" + System.Guid.NewGuid().ToString("N") + ".txt");
            MainWindow window = null;
            try
            {
                File.WriteAllText(path, "recent file smoke");
                settings.SetValue("RecentFiles", new List<string> { path });

                window = new MainWindow();
                window.Show();
                Dispatcher.UIThread.RunJobs();
                WindowUtils.CloseAllWindows();

                var menuRecentFiles = window.FindControl<MenuItem>("menuRecentFiles");
                Assert.That(menuRecentFiles, Is.Not.Null);
                Assert.That(menuRecentFiles.Items.Count, Is.GreaterThanOrEqualTo(1));

                var firstRecent = menuRecentFiles.Items.OfType<MenuItem>().FirstOrDefault();
                Assert.That(firstRecent, Is.Not.Null);
                Assert.That(firstRecent.Header?.ToString(), Is.EqualTo(Path.GetFileName(path)));

                firstRecent.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));
                Dispatcher.UIThread.RunJobs();

                var editor = WindowUtils.GetTrackedWindows().OfType<Editor>().SingleOrDefault();
                Assert.That(editor, Is.Not.Null);
                Assert.That(editor.GetType().Name, Is.EqualTo("OdyToolTXT"));
                Assert.That(editor.IsVisible, Is.True);
            }
            finally
            {
                WindowUtils.CloseAllWindows();
                window?.Close();
                settings.SetValue("RecentFiles", originalRecentFiles);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void ExtractionSidebarExposesHolocronStyleConversionOptions()
        {
            MainWindow window = null;
            try
            {
                window = new MainWindow();
                window.Show();
                Dispatcher.UIThread.RunJobs();

                Assert.That(window.Ui.TpcDecompileCheckbox, Is.Not.Null);
                Assert.That(window.Ui.TpcDecompileCheckbox.IsChecked, Is.True);
                Assert.That(window.Ui.TpcTxiCheckbox, Is.Not.Null);
                Assert.That(window.Ui.TpcTxiCheckbox.IsChecked, Is.False);
                Assert.That(window.Ui.NcsDecompileCheckbox, Is.Not.Null);
                Assert.That(window.Ui.NcsDecompileCheckbox.IsChecked, Is.True);
                Assert.That(window.Ui.MdlDecompileCheckbox, Is.Not.Null);
                Assert.That(window.Ui.MdlDecompileCheckbox.IsEnabled, Is.True);
                Assert.That(window.Ui.MdlTexturesCheckbox, Is.Not.Null);
            }
            finally
            {
                WindowUtils.CloseAllWindows();
                window?.Close();
            }
        }

        [Test]
        public void ExtractResourceDataForSave_DecompilesNcsToNssWhenEnabled()
        {
            var path = Path.Combine(Path.GetTempPath(), "odytools-ncs-extract-" + System.Guid.NewGuid().ToString("N") + ".ncs");
            try
            {
                var ncs = NCSAuto.CompileNss("void main() { int value = 7; }", BioWareGame.K1);
                var bytes = NCSAuto.BytesNcs(ncs);
                File.WriteAllBytes(path, bytes);
                var resource = new FileResource("sample_script", ResourceType.NCS, bytes.Length, 0, path);

                var extracted = MainWindow.ExtractResourceDataForSave(resource, decompileTpc: false, decompileNcs: true, installation: null);
                var source = Encoding.UTF8.GetString(extracted.Data);

                Assert.That(extracted.Extension, Is.EqualTo("nss"));
                Assert.That(source, Does.Contain("void main").Or.Contain("Decompile failed"));
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Test]
        public void ExtractResourceDataForSave_PreservesRawNcsWhenDecompileDisabled()
        {
            var path = Path.Combine(Path.GetTempPath(), "odytools-ncs-raw-" + System.Guid.NewGuid().ToString("N") + ".ncs");
            try
            {
                var ncs = NCSAuto.CompileNss("void main() { }", BioWareGame.K1);
                var bytes = NCSAuto.BytesNcs(ncs);
                File.WriteAllBytes(path, bytes);
                var resource = new FileResource("sample_script", ResourceType.NCS, bytes.Length, 0, path);

                var extracted = MainWindow.ExtractResourceDataForSave(resource, decompileTpc: false, decompileNcs: false, installation: null);

                Assert.That(extracted.Extension, Is.EqualTo("ncs"));
                Assert.That(extracted.Data, Is.EqualTo(bytes));
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Test]
        public void ExtractResourceFilesForSave_ExtractsTxiSidecarWhenEnabled()
        {
            var path = Path.Combine(Path.GetTempPath(), "odytools-tpc-txi-" + System.Guid.NewGuid().ToString("N") + ".tpc");
            var txiPath = Path.ChangeExtension(path, ".txi");
            try
            {
                var tpc = TPC.FromBlank();
                var bytes = TPCAuto.BytesTpc(tpc, ResourceType.TPC);
                File.WriteAllBytes(path, bytes);
                File.WriteAllText(txiPath, "proceduretype cycle", Encoding.ASCII);
                var resource = new FileResource("sample_texture", ResourceType.TPC, bytes.Length, 0, path);
                var savePath = Path.Combine(Path.GetTempPath(), "sample_texture.tga");

                var extracted = MainWindow.ExtractResourceFilesForSave(
                    resource,
                    decompileTpc: true,
                    extractTxi: true,
                    decompileNcs: false,
                    installation: null,
                    savePath: savePath);

                Assert.That(extracted.Select(item => Path.GetExtension(item.Path)), Is.EquivalentTo(new[] { ".tga", ".txi" }));
                var txi = extracted.Single(item => Path.GetExtension(item.Path) == ".txi");
                Assert.That(Encoding.ASCII.GetString(txi.Data), Is.EqualTo("proceduretype cycle"));
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (File.Exists(txiPath))
                {
                    File.Delete(txiPath);
                }
            }
        }

        [Test]
        public void ExtractResourceFilesForSave_ExtractsTxiFromActiveInstallation()
        {
            var installRoot = Path.Combine(Path.GetTempPath(), "odytools-install-txi-" + System.Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(installRoot);
                var overrideDir = Path.Combine(installRoot, "Override");
                Directory.CreateDirectory(overrideDir);
                File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
                File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

                var tpc = TPC.FromBlank();
                var tpcBytes = TPCAuto.BytesTpc(tpc, ResourceType.TPC);
                var tpcPath = Path.Combine(overrideDir, "sample_texture.tpc");
                File.WriteAllBytes(tpcPath, tpcBytes);
                File.WriteAllText(Path.Combine(overrideDir, "sample_texture.txi"), "blending additive", Encoding.ASCII);

                var installation = new OdyInstallation(installRoot, "Fake K1");
                var resource = new FileResource("sample_texture", ResourceType.TPC, tpcBytes.Length, 0, tpcPath);
                var savePath = Path.Combine(Path.GetTempPath(), "sample_texture.tga");

                var extracted = MainWindow.ExtractResourceFilesForSave(
                    resource,
                    decompileTpc: true,
                    extractTxi: true,
                    decompileNcs: false,
                    installation: installation,
                    savePath: savePath);

                var txi = extracted.Single(item => Path.GetExtension(item.Path) == ".txi");
                Assert.That(Encoding.ASCII.GetString(txi.Data), Is.EqualTo("blending additive"));
            }
            finally
            {
                if (Directory.Exists(installRoot))
                {
                    Directory.Delete(installRoot, recursive: true);
                }
            }
        }

        [Test]
        public void ExtractResourceFilesForSave_ExtractsModelTexturesWhenEnabled()
        {
            var installRoot = Path.Combine(Path.GetTempPath(), "odytools-install-mdl-textures-" + System.Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(installRoot);
                var overrideDir = Path.Combine(installRoot, "Override");
                Directory.CreateDirectory(overrideDir);
                File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
                File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

                var texture = TPC.FromBlank();
                var textureBytes = TPCAuto.BytesTpc(texture, ResourceType.TPC);
                File.WriteAllBytes(Path.Combine(overrideDir, "sample_texture.tpc"), textureBytes);

                var mdlBytes = CreateAsciiModelWithTexture("sample_texture");
                var mdlPath = Path.Combine(overrideDir, "sample_model.mdl.ascii");
                File.WriteAllBytes(mdlPath, mdlBytes);

                var installation = new OdyInstallation(installRoot, "Fake K1");
                var resource = new FileResource("sample_model", ResourceType.MDL_ASCII, mdlBytes.Length, 0, mdlPath);
                var savePath = Path.Combine(Path.GetTempPath(), "sample_model.mdl.ascii");

                var extracted = MainWindow.ExtractResourceFilesForSave(
                    resource,
                    decompileTpc: false,
                    extractTxi: false,
                    decompileNcs: false,
                    installation: installation,
                    savePath: savePath,
                    extractMdlTextures: true);

                Assert.That(extracted.Select(item => Path.GetFileName(item.Path)), Does.Contain("sample_model.mdl.ascii"));
                var textureFile = extracted.Single(item => Path.GetFileName(item.Path) == "sample_texture.tpc");
                Assert.That(textureFile.Data, Is.EqualTo(textureBytes));
            }
            finally
            {
                if (Directory.Exists(installRoot))
                {
                    Directory.Delete(installRoot, recursive: true);
                }
            }
        }

        [Test]
        public void ExtractResourceFilesForSave_DecompilesAsciiMdlWhenEnabled()
        {
            var path = Path.Combine(Path.GetTempPath(), "odytools-mdl-ascii-" + System.Guid.NewGuid().ToString("N") + ".mdl.ascii");
            try
            {
                var mdlBytes = CreateAsciiModelWithTexture("sample_texture");
                File.WriteAllBytes(path, mdlBytes);
                var resource = new FileResource("sample_model", ResourceType.MDL_ASCII, mdlBytes.Length, 0, path);
                var savePath = Path.Combine(Path.GetTempPath(), "sample_model.mdl.ascii");

                var extracted = MainWindow.ExtractResourceFilesForSave(
                    resource,
                    decompileTpc: false,
                    extractTxi: false,
                    decompileNcs: false,
                    installation: null,
                    savePath: savePath,
                    extractMdlTextures: false,
                    decompileMdl: true);

                Assert.That(extracted, Has.Count.EqualTo(1));
                Assert.That(Path.GetFileName(extracted[0].Path), Is.EqualTo("sample_model.mdl.ascii"));
                Assert.That(Encoding.ASCII.GetString(extracted[0].Data), Does.Contain("newmodel sample_model"));
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        private static byte[] CreateAsciiModelWithTexture(string textureName)
        {
            var mdl = new MDL
            {
                Name = "sample_model",
                Supermodel = "null",
                Classification = MDLClassification.OTHER,
            };

            var mesh = new MDLMesh
            {
                Texture1 = textureName,
                Render = 1,
                Shadow = 0,
                Vertices =
                {
                    new Vector3(0.0f, 0.0f, 0.0f),
                    new Vector3(1.0f, 0.0f, 0.0f),
                    new Vector3(0.0f, 1.0f, 0.0f),
                },
                UV1 =
                {
                    Vector2.Zero,
                    Vector2.UnitX,
                    Vector2.UnitY,
                },
                Faces =
                {
                    new MDLFace { V1 = 0, V2 = 1, V3 = 2 },
                },
            };

            mdl.Root.Name = "root";
            mdl.Root.Children.Add(new MDLNode
            {
                Name = "mesh",
                NodeType = MDLNodeType.TRIMESH,
                Mesh = mesh,
            });

            return MDLAuto.BytesMdl(mdl, ResourceType.MDL_ASCII);
        }
    }
}
