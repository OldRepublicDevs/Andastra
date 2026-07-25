using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using NUnit.Framework;
using OdyTools.Editors.GUI;

namespace OdyTools.Tests
{
    /// <summary>
    /// GUI Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolGUITests
    {
        [Test]
        public async Task OdyToolGUI_New_BuildsGuiWithRootControlsList()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGUI(null, null);
                    try
                    {
                        byte[] bytes = editor.Build().Item1;

                        Assert.That(bytes, Is.Not.Null.And.Length.GreaterThan(0));
                        GFF loaded = GFF.FromBytes(bytes);
                        Assert.That(loaded.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var controls), Is.True);
                        Assert.That(controls.Count, Is.EqualTo(0));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGUI_LoadMinimalGui_BuildPreservesControlTree()
        {
            byte[] input = CreateMinimalGuiBytes();

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGUI(null, null);
                    try
                    {
                        editor.Load("test_screen.gui", "test_screen", ResourceType.GUI, input);
                        byte[] output = editor.Build().Item1;

                        Assert.That(output, Is.Not.Null.And.Length.GreaterThan(0));
                        GFF loaded = GFF.FromBytes(output);
                        Assert.That(loaded.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var controls), Is.True);
                        Assert.That(controls.Count, Is.EqualTo(1));

                        GFFStruct rootControl = controls.At(0);
                        Assert.That(rootControl.GetString(OdyToolGUIHelpers.TagLabel), Is.EqualTo("screen_root"));
                        AssertExtent(rootControl, 4, 8, 320, 240);

                        Assert.That(rootControl.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var children), Is.True);
                        Assert.That(children.Count, Is.EqualTo(1));
                        Assert.That(children.At(0).GetString(OdyToolGUIHelpers.TagLabel), Is.EqualTo("ok_button"));
                        AssertExtent(children.At(0), 12, 20, 90, 32);
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGUI_SelectAndEditChildControl_BuildsUpdatedControlTree()
        {
            byte[] input = CreateMinimalGuiBytes();

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGUI(null, null);
                    try
                    {
                        editor.Load("test_screen.gui", "test_screen", ResourceType.GUI, input);

                        Assert.That(editor.SelectControlByTagForTest("ok_button"), Is.True);
                        Assert.That(editor.SelectedControlTagForTest, Is.EqualTo("ok_button"));
                        Assert.That(editor.HasSelectedControlPropertyPanelForTest, Is.True);
                        Assert.That(editor.IsDirty, Is.False);

                        Assert.That(editor.EditSelectedControlForTest("cancel_button", 18, 26, 120, 36), Is.True);

                        Assert.That(editor.SelectedControlTagForTest, Is.EqualTo("cancel_button"));
                        Assert.That(editor.IsDirty, Is.True);

                        GFF loaded = GFF.FromBytes(editor.Build().Item1);
                        Assert.That(loaded.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var controls), Is.True);
                        Assert.That(controls.Count, Is.EqualTo(1));
                        Assert.That(controls.At(0).TryGetList(OdyToolGUIHelpers.ControllistLabel, out var children), Is.True);
                        Assert.That(children.Count, Is.EqualTo(1));

                        GFFStruct child = children.At(0);
                        Assert.That(child.GetString(OdyToolGUIHelpers.TagLabel), Is.EqualTo("cancel_button"));
                        AssertExtent(child, 18, 26, 120, 36);
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGUI_AddDuplicateDeleteControls_BuildsUpdatedControlTree()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGUI(null, null);
                    try
                    {
                        Assert.That(editor.ControlCountForTest, Is.EqualTo(0));

                        editor.AddRootControlForTest();
                        Assert.That(editor.ControlCountForTest, Is.EqualTo(1));
                        Assert.That(editor.SelectedControlTagForTest, Is.EqualTo("control"));
                        Assert.That(editor.HasSelectedControlPropertyPanelForTest, Is.True);

                        Assert.That(editor.AddChildControlForTest(), Is.True);
                        Assert.That(editor.ControlCountForTest, Is.EqualTo(2));
                        Assert.That(editor.SelectedControlTagForTest, Is.EqualTo("control_child"));

                        Assert.That(editor.EditSelectedControlForTest("ok_button", 12, 20, 90, 32), Is.True);
                        Assert.That(editor.DuplicateSelectedControlForTest(), Is.True);
                        Assert.That(editor.ControlCountForTest, Is.EqualTo(3));
                        Assert.That(editor.SelectedControlTagForTest, Is.EqualTo("ok_button_copy"));

                        Assert.That(editor.DeleteSelectedControlForTest(), Is.True);
                        Assert.That(editor.ControlCountForTest, Is.EqualTo(2));

                        GFF loaded = GFF.FromBytes(editor.Build().Item1);
                        Assert.That(loaded.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var controls), Is.True);
                        Assert.That(controls.Count, Is.EqualTo(1));

                        GFFStruct rootControl = controls.At(0);
                        Assert.That(rootControl.GetString(OdyToolGUIHelpers.TagLabel), Is.EqualTo("control"));
                        Assert.That(rootControl.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var children), Is.True);
                        Assert.That(children.Count, Is.EqualTo(1));
                        Assert.That(children.At(0).GetString(OdyToolGUIHelpers.TagLabel), Is.EqualTo("ok_button"));
                        AssertExtent(children.At(0), 12, 20, 90, 32);
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolGUI_SelectedResRefField_TrimsAndClearsInvalidValues()
        {
            byte[] input = CreateMinimalGuiBytes();

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGUI(null, null);
                    try
                    {
                        editor.Load("test_screen.gui", "test_screen", ResourceType.GUI, input);

                        Assert.That(editor.SelectControlByTagForTest("ok_button"), Is.True);
                        Assert.That(editor.EditSelectedResRefFieldForTest("FILL", " ui_button "), Is.True);

                        GFF loaded = GFF.FromBytes(editor.Build().Item1);
                        Assert.That(loaded.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var controls), Is.True);
                        Assert.That(controls.At(0).TryGetList(OdyToolGUIHelpers.ControllistLabel, out var children), Is.True);
                        Assert.That(children.At(0).GetResRef("FILL").ToString(), Is.EqualTo("ui_button"));

                        Assert.That(OdyToolGUI.ResRefFromEditableText("bad*fill").IsBlank(), Is.True);
                        Assert.That(OdyToolGUI.ResRefFromEditableText(" more_than_16_chars ").IsBlank(), Is.True);
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        private static byte[] CreateMinimalGuiBytes()
        {
            var gff = new GFF(GFFContent.GUI);
            var controls = new GFFList();

            GFFStruct rootControl = controls.Add(1);
            rootControl.SetString(OdyToolGUIHelpers.TagLabel, "screen_root");
            rootControl.SetStruct(OdyToolGUIHelpers.ExtentLabel, CreateExtent(4, 8, 320, 240));

            var childControls = new GFFList();
            GFFStruct child = childControls.Add(2);
            child.SetString(OdyToolGUIHelpers.TagLabel, "ok_button");
            child.SetResRef("FILL", ResRef.FromString("old_fill"));
            child.SetStruct(OdyToolGUIHelpers.ExtentLabel, CreateExtent(12, 20, 90, 32));
            rootControl.SetList(OdyToolGUIHelpers.ControllistLabel, childControls);

            gff.Root.SetList(OdyToolGUIHelpers.ControllistLabel, controls);
            return GFFAuto.BytesGff(gff, ResourceType.GUI);
        }

        private static GFFStruct CreateExtent(int left, int top, int width, int height)
        {
            var extent = new GFFStruct(0);
            extent.SetInt32("LEFT", left);
            extent.SetInt32("TOP", top);
            extent.SetInt32("WIDTH", width);
            extent.SetInt32("HEIGHT", height);
            return extent;
        }

        private static void AssertExtent(GFFStruct control, int left, int top, int width, int height)
        {
            Assert.That(control.TryGetStruct(OdyToolGUIHelpers.ExtentLabel, out var extent), Is.True);
            Assert.That(extent.GetInt32("LEFT"), Is.EqualTo(left));
            Assert.That(extent.GetInt32("TOP"), Is.EqualTo(top));
            Assert.That(extent.GetInt32("WIDTH"), Is.EqualTo(width));
            Assert.That(extent.GetInt32("HEIGHT"), Is.EqualTo(height));
        }
    }
}
