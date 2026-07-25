using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// GFF Editor Load/Build roundtrip tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolGFFTests
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

        [Test]
        public async Task OdyToolGFF_LoadAndBuild_PreservesData()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetUInt32("id", 42);
                gff.Root.SetString("name", "test");
                byte[] originalData = GFFAuto.BytesGff(gff, ResourceType.GFF);

                var editor = new OdyToolGFF(null, null);
                editor.Load("test.gff", "test", ResourceType.GFF, originalData);

                Tuple<byte[], byte[]> result = editor.Build();
                byte[] builtData = result.Item1;
                Assert.That(builtData, Is.Not.Null.And.Length.GreaterThan(0));

                GFF loaded = GFF.FromBytes(builtData);
                Assert.That(loaded.Root.GetUInt32("id"), Is.EqualTo(42u));
                Assert.That(loaded.Root.GetString("name"), Is.EqualTo("test"));
                Assert.That(loaded.Root, Is.Not.Null);
                Assert.That(loaded.Root.Count, Is.GreaterThanOrEqualTo(2));
                Assert.That(result.Item2, Is.Not.Null);
            }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGFF_RemoveTreeField_BuildDropsRemovedField()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetUInt32("id", 42);
                gff.Root.SetString("name", "test");
                byte[] originalData = GFFAuto.BytesGff(gff, ResourceType.GFF);

                var editor = new OdyToolGFF(null, null);
                editor.Load("test.gff", "test", ResourceType.GFF, originalData);
                Assert.That(editor.RootFieldCountForTests, Is.EqualTo(2));

                editor.SelectRootChildForTests("name");
                editor.RemoveSelectedNodeForTests();

                GFF rebuilt = GFF.FromBytes(editor.Build().Item1);
                Assert.That(rebuilt.Root.Exists("name"), Is.False);
                Assert.That(rebuilt.Root.Exists("id"), Is.True);
                Assert.That(rebuilt.Root.Count, Is.EqualTo(1));
            }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGFF_SelectedResRef_TrimsAndClearsInvalidValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetResRef("template", ResRef.FromString("old_ref"));
                byte[] originalData = GFFAuto.BytesGff(gff, ResourceType.GFF);

                var editor = new OdyToolGFF(null, null);
                editor.Load("test.gff", "test", ResourceType.GFF, originalData);

                editor.SelectRootChildForTests("template");
                editor.EditSelectedResRefForTests(" new_ref ");

                GFF trimmed = GFF.FromBytes(editor.Build().Item1);
                Assert.That(trimmed.Root.GetResRef("template").ToString(), Is.EqualTo("new_ref"));

                editor.EditSelectedResRefForTests("bad*ref");

                GFF cleared = GFF.FromBytes(editor.Build().Item1);
                Assert.That(cleared.Root.GetResRef("template").IsBlank(), Is.True);
                Assert.That(OdyToolGFF.ResRefFromEditableText(" more_than_16_chars ").IsBlank(), Is.True);
            }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGFF_LoadVendorGit_BuildPreservesNestedGffStructure()
        {
            byte[] originalData = File.ReadAllBytes(VendorTestFile("zio001.git"));
            GFF original = GFFAuto.ReadGff(originalData, fileFormat: ResourceType.GIT);

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var editor = new OdyToolGFF(null, null);
                editor.Load("zio001.git", "zio001", ResourceType.GIT, originalData);

                byte[] builtData = editor.Build().Item1;
                GFF rebuilt = GFFAuto.ReadGff(builtData, fileFormat: ResourceType.GIT);
                var differences = new List<string>();

                Assert.That(rebuilt.Content, Is.EqualTo(original.Content));
                Assert.That(original.Compare(rebuilt, differences.Add), Is.True, string.Join(Environment.NewLine, differences));
            }, CancellationToken.None);
            }
        }

        [TestCase("inventory.res", nameof(ResourceType.RES), GFFContent.GFF)]
        [TestCase("inventory.res.xml", nameof(ResourceType.RES_XML), GFFContent.GFF)]
        [TestCase("game.gam", nameof(ResourceType.GAM), GFFContent.GAM)]
        public async Task OdyToolGFF_LoadGenericGffFallbackType_BuildPreservesContent(
            string filename,
            string resourceTypeName,
            GFFContent content)
        {
            var restype = ResourceType.FromName(resourceTypeName);
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var gff = new GFF(content);
                gff.Root.SetString("tag", "generic_fallback");
                byte[] originalData = GFFAuto.BytesGff(gff, restype);
                string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + "-" + filename);
                File.WriteAllBytes(path, originalData);

                try
                {
                    var editor = new OdyToolGFF(null, null);
                    Assert.That(editor.CanLoadPath(path), Is.True);

                    editor.Load(path, Path.GetFileNameWithoutExtension(filename), restype, originalData);
                    byte[] builtData = editor.Build().Item1;
                    var rebuilt = GFFAuto.ReadGff(builtData, fileFormat: restype);

                    Assert.That(rebuilt.Content, Is.EqualTo(content));
                    Assert.That(rebuilt.Root.GetString("tag"), Is.EqualTo("generic_fallback"));
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

        [Test]
        public async Task OdyToolGFF_New_ProducesEmptyRoot()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var editor = new OdyToolGFF(null, null);
                editor.New();
                Tuple<byte[], byte[]> result = editor.Build();
                byte[] data = result.Item1;
                Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                GFF gff = GFF.FromBytes(data);
                Assert.That(gff.Root.Count, Is.EqualTo(0));
                Assert.That(gff.Root, Is.Not.Null);
                Assert.That(result.Item2, Is.Not.Null);
                Assert.That(editor, Is.Not.Null);
                Assert.That(data.Length, Is.GreaterThan(0));
            }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGFF_LoadEmptyData_CreatesEmptyGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var editor = new OdyToolGFF(null, null);
                editor.Load("x.gui", "x", ResourceType.GFF, null);
                Tuple<byte[], byte[]> result = editor.Build();
                byte[] data = result.Item1;
                Assert.That(data, Is.Not.Null);
                GFF gff = GFF.FromBytes(data);
                Assert.That(gff.Root.Count, Is.EqualTo(0));
                Assert.That(gff.Root, Is.Not.Null);
                Assert.That(result.Item2, Is.Not.Null);
                Assert.That(editor, Is.Not.Null);
                Assert.That(data.Length, Is.GreaterThanOrEqualTo(0));
            }, CancellationToken.None);
            }
        }

        [TestCase("sample.gui", "sample", "GUI")]
        [TestCase("module.ifo", "module", "IFO")]
        [TestCase("repute.fac", "repute", "FAC")]
        public async Task WindowUtils_OpenGffSpecializedResourceWithoutInstallation_RoutesToGenericGff(string filename, string resname, string typeName)
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var restype = ResourceType.FromExtension(typeName.ToLowerInvariant());
                var content = (GFFContent)Enum.Parse(typeof(GFFContent), typeName);
                var gff = new GFF(content);
                gff.Root.SetString("tag", resname);
                byte[] input = GFFAuto.BytesGff(gff, restype);
                string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + "-" + filename);
                File.WriteAllBytes(path, input);

                Window window = null;
                try
                {
                    Tuple<string, Window> result = WindowUtils.OpenResourceEditor(
                        path,
                        resname,
                        restype,
                        input,
                        null,
                        null,
                        true);

                    Assert.That(result, Is.Not.Null);
                    window = result.Item2;
                    Assert.That(window, Is.TypeOf<OdyToolGFF>());
                }
                finally
                {
                    window?.Close();
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }, CancellationToken.None);
            }
        }

        [Test]
        public async Task WindowUtils_OpenSameResourceTwice_FocusesExistingEditor()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
            await session.Dispatch(() =>
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("tag", "reuse");
                byte[] input = GFFAuto.BytesGff(gff, ResourceType.GFF);
                string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + "-reuse.gff");
                File.WriteAllBytes(path, input);
                WindowUtils.CloseAllWindows();

                Window first = null;
                try
                {
                    var firstResult = WindowUtils.OpenResourceEditor(path, "reuse", ResourceType.GFF, input, null, null, true);
                    Assert.That(firstResult, Is.Not.Null);
                    first = firstResult.Item2;
                    Assert.That(first, Is.TypeOf<OdyToolGFF>());
                    Assert.That(WindowUtils.WindowCount, Is.EqualTo(1));

                    var secondResult = WindowUtils.OpenResourceEditor(path, "reuse", ResourceType.GFF, input, null, null, true);
                    Assert.That(secondResult, Is.Not.Null);
                    Assert.That(secondResult.Item2, Is.SameAs(first));
                    Assert.That(WindowUtils.WindowCount, Is.EqualTo(1));
                }
                finally
                {
                    first?.Close();
                    WindowUtils.CloseAllWindows();
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }, CancellationToken.None);
            }
        }
    }
}
