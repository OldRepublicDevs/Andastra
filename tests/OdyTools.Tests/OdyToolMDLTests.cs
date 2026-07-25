using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.MDLData;
using OdyTools.Editors;
using OdyTools.Widgets;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// MDL Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolMDLTests
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

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_New_InitializesRendererAndHelpWithoutLogicalTreeCrash()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolMDL(null, null);
                    try
                    {
                        Assert.That(FindControl<ModelRenderer>(editor), Is.Not.Null);
                        Assert.That(FindMenuItem(editor, "Help"), Is.Not.Null);
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_LoadVendorMdlPair_PopulatesRenderer()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string mdlPath = VendorTestFile("mdl/c_dewback.mdl");
                    byte[] mdlData = File.ReadAllBytes(mdlPath);
                    var editor = new OdyToolMDL(null, null);
                    try
                    {
                        editor.Load(mdlPath, "c_dewback", ResourceType.MDL, mdlData);

                        var renderer = FindControl<ModelRenderer>(editor);
                        Assert.That(renderer, Is.Not.Null);
                        Assert.That(renderer.ParsedModel, Is.Not.Null);
                        Assert.That(renderer.ConvertedModel, Is.Not.Null);
                        Assert.That(renderer.ConvertedModel.Meshes.Count, Is.GreaterThan(0));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_LoadVendorMdlPair_PopulatesSelectableInspector()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string mdlPath = VendorTestFile("mdl/c_dewback.mdl");
                    byte[] mdlData = File.ReadAllBytes(mdlPath);
                    var editor = new OdyToolMDL(null, null);
                    try
                    {
                        editor.Load(mdlPath, "c_dewback", ResourceType.MDL, mdlData);

                        Assert.That(editor.ModelSummaryForTests, Does.Contain("Nodes:"));
                        Assert.That(editor.ModelSummaryForTests, Does.Contain("Textures:"));
                        Assert.That(editor.NodeNamesForTests.Count, Is.GreaterThan(0));
                        Assert.That(editor.TextureNamesForTests.Count, Is.GreaterThan(0));

                        string nodeName = editor.NodeNamesForTests[0];
                        editor.SelectNodeForTests(nodeName);

                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain("Node:"));
                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain(nodeName));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_SelectedDetailsFollowActiveInspectorTab()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolMDL(null, null);
                    try
                    {
                        var mdl = CreateEditableMdl();
                        mdl.Root.Mesh = new MDLMesh { Texture1 = "body_tex" };
                        editor.LoadModelForTests(mdl, ResourceType.MDL_ASCII);

                        editor.SelectNodeForTests("root");
                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain("Node: root"));

                        editor.SelectTextureForTests("body_tex");
                        Assert.That(editor.SelectedModelDetailsForTests, Is.EqualTo("Texture: body_tex"));

                        editor.SelectAnimationForTests("walk");
                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain("Animation: walk"));
                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain("Transition: 0.25s"));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_EditMetadata_BuildsUpdatedAsciiModelAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolMDL(null, null);
                    try
                    {
                        var mdl = new MDL
                        {
                            Name = "test",
                            Supermodel = "null",
                            Classification = MDLClassification.OTHER
                        };
                        mdl.Root.Name = "root";
                        mdl.Root.Children.Add(new MDLNode
                        {
                            Name = "test_node",
                            NodeType = MDLNodeType.DUMMY,
                            Position = Vector3.Zero,
                            Orientation = new Vector4(0.0f, 0.0f, 0.0f, 1.0f)
                        });
                        editor.LoadModelForTests(mdl, ResourceType.MDL_ASCII);

                        editor.EditMetadataForTests("c_dewback_edit", "s_male02", MDLClassification.CHARACTER);

                        Assert.That(editor.IsDirty, Is.True);
                        Assert.That(editor.ModelNameForTests, Is.EqualTo("c_dewback_edit"));
                        Assert.That(editor.ModelSupermodelForTests, Is.EqualTo("s_male02"));

                        var built = editor.Build();
                        Assert.That(built.Item1, Is.Not.Null.And.Length.GreaterThan(0));
                        Assert.That(built.Item2, Is.Not.Null.And.Length.EqualTo(0));
                        string builtText = Encoding.UTF8.GetString(built.Item1);
                        Assert.That(builtText, Does.Contain("newmodel c_dewback_edit"));
                        Assert.That(builtText, Does.Contain("setsupermodel c_dewback_edit s_male02"));
                        Assert.That(builtText.ToLowerInvariant(), Does.Contain("classification character"));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_EditSelectedAnimation_BuildsUpdatedAsciiAnimationAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolMDL(null, null);
                    try
                    {
                        editor.LoadModelForTests(CreateEditableMdl(), ResourceType.MDL_ASCII);

                        editor.SelectAnimationForTests("walk");
                        editor.EditSelectedAnimationForTests("run", 2.5f, 0.75f);

                        Assert.That(editor.IsDirty, Is.True);
                        Assert.That(editor.AnimationNamesForTests, Does.Contain("run"));
                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain("Animation: run"));
                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain("Length: 2.5s"));
                        Assert.That(editor.SelectedModelDetailsForTests, Does.Contain("Transition: 0.75s"));

                        var built = editor.Build();
                        string builtText = Encoding.UTF8.GetString(built.Item1);
                        Assert.That(builtText, Does.Contain("newanim run test"));
                        Assert.That(builtText, Does.Contain("length 2.5"));
                        Assert.That(builtText, Does.Contain("transtime 0.75"));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_LoadVendorMdxPair_PopulatesRenderer()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string mdxPath = VendorTestFile("mdl/c_dewback.mdx");
                    byte[] mdxData = File.ReadAllBytes(mdxPath);
                    var editor = new OdyToolMDL(null, null);
                    try
                    {
                        editor.Load(mdxPath, "c_dewback", ResourceType.MDX, mdxData);

                        var renderer = FindControl<ModelRenderer>(editor);
                        Assert.That(renderer, Is.Not.Null);
                        Assert.That(renderer.ParsedModel, Is.Not.Null);
                        Assert.That(renderer.ConvertedModel, Is.Not.Null);
                        Assert.That(renderer.ConvertedModel.Meshes.Count, Is.GreaterThan(0));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolMDL_CanLoadMdxStartupPath()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".mdx");
                    File.WriteAllBytes(path, new byte[] { 1, 2, 3, 4 });
                    try
                    {
                        var editor = new OdyToolMDL(null, null);
                        try
                        {
                            Assert.That(editor.CanLoadPath(path), Is.True);
                        }
                        finally
                        {
                            editor.Close();
                        }
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

        private static MDL CreateEditableMdl()
        {
            var mdl = new MDL
            {
                Name = "test",
                Supermodel = "null",
                Classification = MDLClassification.OTHER
            };
            mdl.Root.Name = "root";
            mdl.Root.NodeType = MDLNodeType.TRIMESH;
            mdl.Root.Position = Vector3.Zero;
            mdl.Root.Orientation = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            mdl.Anims.Add(new MDLAnimation
            {
                Name = "walk",
                RootModel = "root",
                AnimLength = 1.0f,
                TransitionLength = 0.25f
            });
            return mdl;
        }

        private static T FindControl<T>(Control parent) where T : Control
        {
            if (parent is T match)
            {
                return match;
            }

            if (parent is Panel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is Control control)
                    {
                        var result = FindControl<T>(control);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            else if (parent is ContentControl contentControl && contentControl.Content is Control content)
            {
                return FindControl<T>(content);
            }

            return null;
        }

        private static MenuItem FindMenuItem(Control parent, string header)
        {
            var menu = FindControl<Menu>(parent);
            if (menu == null)
            {
                return null;
            }

            foreach (var item in menu.Items)
            {
                if (item is MenuItem menuItem && menuItem.Header?.ToString() == header)
                {
                    return menuItem;
                }
            }

            return null;
        }
    }
}
