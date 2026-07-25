using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.MDLData;
using OdyTools.Editors;
using NUnit.Framework;
using TPC = BioWare.Resource.Formats.TPC.TPC;
using TPCAuto = BioWare.Resource.Formats.TPC.TPCAuto;

namespace OdyTools.Tests
{
    /// <summary>
    /// ERF Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolERFTests
    {
        private static string VendorTestFile(string relativePath)
        {
            var directory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (directory != null)
            {
                string candidate = Path.Combine(directory.FullName, "vendor", "tests", "test_files", relativePath);
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }

            Assert.Fail("Could not locate vendor test file: " + relativePath);
            return null;
        }

        private static string ResourceSignature(ERFResource resource)
        {
            return resource.ResRef + "." + resource.ResType.Extension.ToLowerInvariant();
        }

        private static void AssertErfResourcesEqual(ERF actual, ERF expected)
        {
            Assert.That(actual.Count, Is.EqualTo(expected.Count));

            var actualByKey = actual.ToDictionary(ResourceSignature);
            var expectedByKey = expected.ToDictionary(ResourceSignature);
            Assert.That(actualByKey.Keys.OrderBy(key => key).ToArray(), Is.EqualTo(expectedByKey.Keys.OrderBy(key => key).ToArray()));

            foreach (string key in expectedByKey.Keys)
            {
                Assert.That(actualByKey[key].Data, Is.EqualTo(expectedByKey[key].Data), "Payload mismatch for " + key);
                Assert.That(actualByKey[key].ResType, Is.EqualTo(expectedByKey[key].ResType), "Type mismatch for " + key);
            }
        }

        [Test]
        public async Task OdyToolERF_New_BuildsValidEmptyERF()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolERF(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolERF_LoadVendorErf_BuildPreservesArchiveResources()
        {
            byte[] data = File.ReadAllBytes(VendorTestFile("test.erf"));
            ERF original = ERFAuto.ReadErf(data);

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolERF(null, null);
                    editor.Load("test.erf", "test", ResourceType.ERF, data);

                    ERF rebuilt = ERFAuto.ReadErf(editor.Build().Item1);

                    AssertErfResourcesEqual(rebuilt, original);
                    Assert.That(editor.ResourceCountForTest, Is.EqualTo(original.Count));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolERF_LoadHak_BuildsValidErfFamilyArchive()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var hak = new ERF(ERFType.ERF);
                    hak.SetData("test_hak", ResourceType.TXT, new byte[] { 1, 2, 3 });
                    byte[] input = ERFAuto.BytesErf(hak, ResourceType.HAK);
                    var editor = new OdyToolERF(null, null);

                    editor.Load("test.hak", "test", ResourceType.HAK, input);
                    Tuple<byte[], byte[]> result = editor.Build();
                    ERF rebuilt = ERFAuto.ReadErf(result.Item1);

                    Assert.That(result.Item1, Is.Not.Null.And.Length.GreaterThan(0));
                    Assert.That(editor.ResourceCountForTest, Is.EqualTo(1));
                    Assert.That(rebuilt.Get("test_hak", ResourceType.TXT), Is.EqualTo(new byte[] { 1, 2, 3 }));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolERF_LoadBif_BuildsValidBifArchive()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var bif = new BIF(BIFType.BIF);
                    bif.SetData(new ResRef("test_bif"), ResourceType.TXT, new byte[] { 4, 5, 6 }, 7);
                    byte[] input = new BIFBinaryWriter(bif).Write();
                    var editor = new OdyToolERF(null, null);

                    editor.Load("test.bif", "test", ResourceType.BIF, input);
                    Tuple<byte[], byte[]> result = editor.Build();
                    BIF rebuilt = new BIFBinaryReader(result.Item1).Load();

                    Assert.That(editor.ResourceCountForTest, Is.EqualTo(1));
                    Assert.That(rebuilt.Resources, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Resources[0].ResnameKeyIndex, Is.EqualTo(7));
                    Assert.That(rebuilt.Resources[0].ResType, Is.EqualTo(ResourceType.TXT));
                    Assert.That(rebuilt.Resources[0].Data, Is.EqualTo(new byte[] { 4, 5, 6 }));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolERF_AddRemoveUndo_RoundtripsArchiveResources()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                    Directory.CreateDirectory(tempDir);
                    string firstPath = Path.Combine(tempDir, "first.txt");
                    string secondPath = Path.Combine(tempDir, "second.nss");
                    File.WriteAllBytes(firstPath, new byte[] { 10, 11 });
                    File.WriteAllBytes(secondPath, new byte[] { 12, 13, 14 });

                    try
                    {
                        var editor = new OdyToolERF(null, null);
                        editor.New();
                        editor.AddResourceFilePathsForTest(firstPath, secondPath);

                        Assert.That(editor.ResourceCountForTest, Is.EqualTo(2));
                        Assert.That(editor.ResourceRowsForTest[0].ResRef, Is.EqualTo("first"));
                        Assert.That(editor.ResourceRowsForTest[1].Type, Is.EqualTo("NSS"));

                        ERF added = ERFAuto.ReadErf(editor.Build().Item1);
                        Assert.That(added.Get("first", ResourceType.TXT), Is.EqualTo(new byte[] { 10, 11 }));
                        Assert.That(added.Get("second", ResourceType.NSS), Is.EqualTo(new byte[] { 12, 13, 14 }));

                        editor.SelectResourceIndicesForTest(0);
                        editor.RemoveSelectedForTest();
                        Assert.That(editor.ResourceCountForTest, Is.EqualTo(1));
                        ERF removed = ERFAuto.ReadErf(editor.Build().Item1);
                        Assert.That(removed.Get("first", ResourceType.TXT), Is.Null);
                        Assert.That(removed.Get("second", ResourceType.NSS), Is.EqualTo(new byte[] { 12, 13, 14 }));

                        editor.UndoForTest();
                        Assert.That(editor.ResourceCountForTest, Is.EqualTo(2));
                        ERF undoneRemove = ERFAuto.ReadErf(editor.Build().Item1);
                        Assert.That(undoneRemove.Get("first", ResourceType.TXT), Is.EqualTo(new byte[] { 10, 11 }));
                        Assert.That(undoneRemove.Get("second", ResourceType.NSS), Is.EqualTo(new byte[] { 12, 13, 14 }));

                        editor.UndoForTest();
                        Assert.That(editor.ResourceCountForTest, Is.EqualTo(0), "Adding resources should undo back to the pre-add archive.");
                    }
                    finally
                    {
                        Directory.Delete(tempDir, true);
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolERF_RenameSelectedResource_RoundtripsAndRejectsDuplicateResRefType()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                    Directory.CreateDirectory(tempDir);
                    string firstPath = Path.Combine(tempDir, "first.txt");
                    string secondPath = Path.Combine(tempDir, "second.txt");
                    File.WriteAllBytes(firstPath, new byte[] { 21, 22 });
                    File.WriteAllBytes(secondPath, new byte[] { 31, 32 });

                    try
                    {
                        var editor = new OdyToolERF(null, null);
                        editor.New();
                        editor.AddResourceFilePathsForTest(firstPath, secondPath);

                        editor.SelectResourceIndicesForTest(0);
                        Assert.That(editor.RenameSelectedForTest("renamed", out string renameError), Is.True, renameError);
                        Assert.That(editor.ResourceRowsForTest[0].ResRef, Is.EqualTo("renamed"));

                        ERF renamed = ERFAuto.ReadErf(editor.Build().Item1);
                        Assert.That(renamed.Get("renamed", ResourceType.TXT), Is.EqualTo(new byte[] { 21, 22 }));
                        Assert.That(renamed.Get("first", ResourceType.TXT), Is.Null);
                        Assert.That(renamed.Get("second", ResourceType.TXT), Is.EqualTo(new byte[] { 31, 32 }));

                        editor.SelectResourceIndicesForTest(0);
                        Assert.That(editor.RenameSelectedForTest("second", out string duplicateError), Is.False);
                        Assert.That(duplicateError, Does.Contain("second"));
                        Assert.That(editor.ResourceRowsForTest[0].ResRef, Is.EqualTo("renamed"));

                        ERF afterRejectedRename = ERFAuto.ReadErf(editor.Build().Item1);
                        Assert.That(afterRejectedRename.Get("renamed", ResourceType.TXT), Is.EqualTo(new byte[] { 21, 22 }));
                        Assert.That(afterRejectedRename.Get("second", ResourceType.TXT), Is.EqualTo(new byte[] { 31, 32 }));
                    }
                    finally
                    {
                        Directory.Delete(tempDir, true);
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolERF_ArchiveToolbarActionsFollowSelectionState()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
                    Directory.CreateDirectory(tempDir);
                    string resourcePath = Path.Combine(tempDir, "selected.txt");
                    File.WriteAllBytes(resourcePath, new byte[] { 1, 2, 3 });

                    try
                    {
                        var editor = new OdyToolERF(null, null);
                        editor.New();

                        Assert.That(editor.ExtractButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.OpenButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.RemoveButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.RefreshButtonForTest.IsEnabled, Is.False);

                        editor.AddResourceFilePathsForTest(resourcePath);

                        Assert.That(editor.ExtractButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.OpenButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.RemoveButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.RefreshButtonForTest.IsEnabled, Is.True);

                        editor.SelectResourceIndicesForTest(0);

                        Assert.That(editor.ExtractButtonForTest.IsEnabled, Is.True);
                        Assert.That(editor.OpenButtonForTest.IsEnabled, Is.True);
                        Assert.That(editor.RemoveButtonForTest.IsEnabled, Is.True);

                        editor.New();

                        Assert.That(editor.ExtractButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.OpenButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.RemoveButtonForTest.IsEnabled, Is.False);
                        Assert.That(editor.RefreshButtonForTest.IsEnabled, Is.False);
                    }
                    finally
                    {
                        Directory.Delete(tempDir, true);
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public void ExtractResourceFilesForSave_ExtractsArchiveTxiSidecarWhenEnabled()
        {
            var tpc = TPC.FromBlank();
            byte[] tpcBytes = TPCAuto.BytesTpc(tpc, ResourceType.TPC);
            byte[] txiBytes = Encoding.ASCII.GetBytes("proceduretype cycle");
            var texture = CreateErfResourceRow("sample_texture", ResourceType.TPC, tpcBytes);
            var txi = CreateErfResourceRow("sample_texture", ResourceType.TXI, txiBytes);

            var extracted = OdyToolERF.ExtractResourceFilesForSave(
                texture,
                new[] { texture, txi },
                decompileTpc: true,
                extractTxi: true,
                decompileNcs: false,
                extractMdlTextures: false,
                savePath: Path.Combine(Path.GetTempPath(), "sample_texture.tpc"));

            Assert.That(extracted.Select(item => Path.GetExtension(item.Path)), Is.EquivalentTo(new[] { ".tga", ".txi" }));
            Assert.That(Encoding.ASCII.GetString(extracted.Single(item => Path.GetExtension(item.Path) == ".txi").Data), Is.EqualTo("proceduretype cycle"));
        }

        [Test]
        public void ExtractResourceFilesForSave_ExtractsModelTexturesFromArchiveWhenEnabled()
        {
            byte[] mdlBytes = CreateAsciiModelWithTexture("sample_texture");
            var mdl = CreateErfResourceRow("sample_model", ResourceType.MDL_ASCII, mdlBytes);
            var texture = TPC.FromBlank();
            byte[] textureBytes = TPCAuto.BytesTpc(texture, ResourceType.TPC);
            var textureRow = CreateErfResourceRow("sample_texture", ResourceType.TPC, textureBytes);

            var extracted = OdyToolERF.ExtractResourceFilesForSave(
                mdl,
                new[] { mdl, textureRow },
                decompileTpc: false,
                extractTxi: false,
                decompileNcs: false,
                extractMdlTextures: true,
                savePath: Path.Combine(Path.GetTempPath(), "sample_model.mdl.ascii"));

            Assert.That(extracted.Select(item => Path.GetFileName(item.Path)), Does.Contain("sample_model.mdl.ascii"));
            Assert.That(extracted.Single(item => Path.GetFileName(item.Path) == "sample_texture.tpc").Data, Is.EqualTo(textureBytes));
        }

        [Test]
        public void ExtractResourceFilesForSave_DecompilesAsciiMdlWhenEnabled()
        {
            byte[] mdlBytes = CreateAsciiModelWithTexture("sample_texture");
            var mdl = CreateErfResourceRow("sample_model", ResourceType.MDL_ASCII, mdlBytes);

            var extracted = OdyToolERF.ExtractResourceFilesForSave(
                mdl,
                new[] { mdl },
                decompileTpc: false,
                extractTxi: false,
                decompileNcs: false,
                extractMdlTextures: false,
                savePath: Path.Combine(Path.GetTempPath(), "sample_model.mdl.ascii"),
                decompileMdl: true);

            Assert.That(extracted, Has.Count.EqualTo(1));
            Assert.That(Path.GetFileName(extracted[0].Path), Is.EqualTo("sample_model.mdl.ascii"));
            Assert.That(Encoding.ASCII.GetString(extracted[0].Data), Does.Contain("newmodel sample_model"));
        }

        [Test]
        public async Task WindowUtils_OpenHak_RoutesToErfEditor()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] input = ERFAuto.BytesErf(new ERF(ERFType.ERF), ResourceType.HAK);
                    string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".hak");
                    File.WriteAllBytes(path, input);

                    try
                    {
                        Tuple<string, Avalonia.Controls.Window> result = WindowUtils.OpenResourceEditor(
                            path,
                            "test",
                            ResourceType.HAK,
                            input,
                            null,
                            null,
                            true);

                        Assert.That(result, Is.Not.Null);
                        Assert.That(result.Item2, Is.TypeOf<OdyToolERF>());
                        result.Item2.Close();
                    }
                    finally
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }, CancellationToken.None);
            }
        }

        private static ERFResourceViewModel CreateErfResourceRow(string resref, ResourceType restype, byte[] data)
        {
            return new ERFResourceViewModel
            {
                ResRef = resref,
                Type = restype.Extension.ToUpperInvariant(),
                Size = data.Length.ToString(),
                Offset = "0x0",
                ErfResource = new ERFResource(new ResRef(resref), restype, data)
            };
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
