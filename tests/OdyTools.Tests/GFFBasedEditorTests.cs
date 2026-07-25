using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.ARE;
using BioWare.Resource.Formats.GFF.Generics.UTI;
using OdyTools.Blender;
using OdyTools.Editors;
using NUnit.Framework;
using UTC = BioWare.Resource.Formats.GFF.Generics.UTC.UTC;
using UTCHelpers = BioWare.Resource.Formats.GFF.Generics.UTC.UTCHelpers;

namespace OdyTools.Tests
{
    /// <summary>
    /// Load/Build roundtrip tests for GFF-based editors (UTC, UTD, UTE, UTI, UTM, UTP, UTS, UTT, UTW, ARE, GIT, IFO, JRL, PTH).
    /// Uses minimal valid GFF data (empty root) so Construct* uses defaults. Uses Avalonia headless session.
    /// </summary>
    public class OdyToolGFFBasedTests
    {
        private static byte[] MinimalGffBytes(GFFContent content, ResourceType restype)
        {
            var gff = new GFF(content);
            return GFFAuto.BytesGff(gff, restype);
        }

        private static Editor CreateTypedGffAliasEditor(string editorKey)
        {
            switch (editorKey)
            {
                case "are":
                    return new OdyToolARE(null, null);
                case "fac":
                    return new OdyToolFAC(null, null);
                case "git":
                    return new OdyToolGIT(null, null);
                case "ifo":
                    return new OdyToolIFO(null, null);
                case "utc":
                    return new OdyToolUTC(null, null);
                case "uti":
                    return new OdyToolUTI(null, null);
                case "utd":
                    return new OdyToolUTD(null, null);
                case "ute":
                    return new OdyToolUTE(null, null);
                case "uts":
                    return new OdyToolUTS(null, null);
                case "utt":
                    return new OdyToolUTT(null, null);
                default:
                    throw new ArgumentException("Unknown typed GFF alias editor: " + editorKey, nameof(editorKey));
            }
        }

        [Test, Timeout(90000)]
        [TestCase("are", "test.are.xml", nameof(ResourceType.ARE_XML), GFFContent.ARE)]
        [TestCase("fac", "test.fac.xml", nameof(ResourceType.FAC_XML), GFFContent.FAC)]
        [TestCase("git", "test.git.xml", nameof(ResourceType.GIT_XML), GFFContent.GIT)]
        [TestCase("ifo", "test.ifo.xml", nameof(ResourceType.IFO_XML), GFFContent.IFO)]
        [TestCase("utc", "test.utc.xml", nameof(ResourceType.UTC_XML), GFFContent.UTC)]
        [TestCase("uti", "test.uti.xml", nameof(ResourceType.UTI_XML), GFFContent.UTI)]
        [TestCase("utd", "test.utd.xml", nameof(ResourceType.UTD_XML), GFFContent.UTD)]
        [TestCase("ute", "test.ute.xml", nameof(ResourceType.UTE_XML), GFFContent.UTE)]
        [TestCase("uts", "test.uts.xml", nameof(ResourceType.UTS_XML), GFFContent.UTS)]
        [TestCase("utt", "test.utt.xml", nameof(ResourceType.UTT_XML), GFFContent.UTT)]
        public async Task ObjectEditors_LoadXmlGffAlias_BuildPreservesXmlFormat(
            string editorKey,
            string path,
            string resourceTypeName,
            GFFContent content)
        {
            var restype = ResourceType.FromName(resourceTypeName);
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(content, restype);
                    Editor editor = CreateTypedGffAliasEditor(editorKey);

                    editor.Load(path, "test", restype, data);
                    byte[] built = editor.Build().Item1;
                    var rebuilt = GFFAuto.ReadGff(built, fileFormat: restype);

                    Assert.That(rebuilt.Content, Is.EqualTo(content));
                    Assert.That(System.Text.Encoding.UTF8.GetString(built).TrimStart()[0], Is.EqualTo('<'));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTC, ResourceType.UTC);
                    var editor = new OdyToolUTC(null, null);
                    editor.Load("test.utc", "test", ResourceType.UTC, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        [TestCase(nameof(ResourceType.BTC), GFFContent.BTC, "test.btc")]
        [TestCase(nameof(ResourceType.BIC), GFFContent.BIC, "test.bic")]
        public async Task UTCEditor_LoadCreatureAlias_BuildPreservesContent(string resourceTypeName, GFFContent content, string path)
        {
            var restype = ResourceType.FromName(resourceTypeName);
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(content, restype);
                    var editor = new OdyToolUTC(null, null);
                    editor.Load(path, "test", restype, data);

                    byte[] built = editor.Build().Item1;
                    GFF gff = GFF.FromBytes(built);

                    Assert.That(gff.Content, Is.EqualTo(content));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_ApplyInventoryResult_RefreshesCreaturePreview()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTC(null, null);
                    int before = editor.PreviewRefreshCount;

                    editor.ApplyInventoryResult(
                        new System.Collections.Generic.List<InventoryItem>(),
                        new System.Collections.Generic.Dictionary<EquipmentSlot, InventoryItem>());

                    Assert.That(editor.PreviewRefreshCount, Is.GreaterThan(before));
                    Assert.That(editor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_ApplyInventoryResult_BuildsInventoryLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTC(null, null);

                    editor.ApplyInventoryResult(
                        new System.Collections.Generic.List<InventoryItem>
                        {
                            new InventoryItem(new ResRef("g_w_blstrpstl001"), droppable: true)
                        },
                        new System.Collections.Generic.Dictionary<EquipmentSlot, InventoryItem>());

                    var rebuilt = UTCHelpers.ConstructUtc(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.Inventory, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Inventory[0].ResRef.ToString(), Is.EqualTo("g_w_blstrpstl001"));
                    Assert.That(rebuilt.Inventory[0].Droppable, Is.True);
                    Assert.That(editor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_AllowsInventoryDialogRequestWithoutInstallation()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTC(null, null);
                    editor.ApplyInventoryResult(
                        new System.Collections.Generic.List<InventoryItem>
                        {
                            new InventoryItem(new ResRef("g_w_vbroswrd001"), droppable: true)
                        },
                        new System.Collections.Generic.Dictionary<EquipmentSlot, InventoryItem>());

                    Assert.That(editor.CanOpenInventoryWithoutInstallationForTest(), Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_ScriptFields_TrimWhitespaceAndClearBlankValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTC
                    {
                        Tag = "test_creature",
                        ResRef = new ResRef("old_res"),
                        Conversation = new ResRef("old_conv"),
                        OnSpawn = new ResRef("old_spawn"),
                        OnHeartbeat = new ResRef("old_heart"),
                        OnDeath = new ResRef("old_death"),
                        OnUserDefined = new ResRef("old_user")
                    };
                    byte[] data = GFFAuto.BytesGff(UTCHelpers.DismantleUtc(source), ResourceType.UTC);

                    var editor = new OdyToolUTC(null, null);
                    editor.Load("test.utc", "test", ResourceType.UTC, data);
                    editor.ResrefEdit.Text = "  new_res  ";
                    editor.ConversationEdit.Text = "   ";
                    editor.ScriptFields["OnSpawn"].Text = "  k_spawn  ";
                    editor.ScriptFields["OnHeartbeat"].Text = "";
                    editor.ScriptFields["OnDeath"].Text = "  k_death";
                    editor.ScriptFields["OnUserDefined"].Text = "k_user  ";

                    var rebuilt = UTCHelpers.ConstructUtc(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("new_res"));
                    Assert.That(rebuilt.Conversation.ToString(), Is.EqualTo(""));
                    Assert.That(rebuilt.OnSpawn.ToString(), Is.EqualTo("k_spawn"));
                    Assert.That(rebuilt.OnHeartbeat.ToString(), Is.EqualTo(""));
                    Assert.That(rebuilt.OnDeath.ToString(), Is.EqualTo("k_death"));
                    Assert.That(rebuilt.OnUserDefined.ToString(), Is.EqualTo("k_user"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTC(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.FirstNameEdit, Is.Not.Null);
                    Assert.That(editor.LastNameEdit, Is.Not.Null);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.ConversationEdit, Is.Not.Null);
                    Assert.That(editor.StrengthSpin, Is.Not.Null);
                    Assert.That(editor.BaseHpSpin, Is.Not.Null);
                    Assert.That(editor.ScriptFields, Does.ContainKey("OnSpawn"));
                    Assert.That(editor.ScriptFields, Does.ContainKey("OnHeartbeat"));
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_GenerateTag_MatchesHolocronAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTC(null, null);
                    editor.ResrefEdit.Text = "m12aa_cre";
                    editor.TagEdit.Text = "";

                    editor.TagGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));

                    Assert.That(editor.TagEdit.Text, Is.EqualTo("m12aa_cre"));
                    Assert.That(editor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTCEditor_FieldEdits_BuildIntoUtc()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTC(null, null);
                    editor.TagEdit.Text = "test_creature_tag";
                    editor.ResrefEdit.Text = "test_creature";
                    editor.ConversationEdit.Text = "test_dlg";
                    editor.AlignmentSlider.Value = 75;
                    editor.PlotCheckbox.IsChecked = true;
                    editor.NoPermDeathCheckbox.IsChecked = true;
                    editor.Min1HpCheckbox.IsChecked = true;
                    editor.DisarmableCheckbox.IsChecked = true;
                    editor.StrengthSpin.Value = 16;
                    editor.DexteritySpin.Value = 14;
                    editor.ConstitutionSpin.Value = 13;
                    editor.IntelligenceSpin.Value = 12;
                    editor.WisdomSpin.Value = 11;
                    editor.CharismaSpin.Value = 10;
                    editor.ArmorClassSpin.Value = 4;
                    editor.BaseHpSpin.Value = 30;
                    editor.CurrentHpSpin.Value = 24;
                    editor.MaxHpSpin.Value = 35;
                    editor.ScriptFields["OnSpawn"].Text = "k_spawn";
                    editor.ScriptFields["OnHeartbeat"].Text = "k_heart";
                    editor.ScriptFields["OnDeath"].Text = "k_death";
                    editor.CommentsEdit.Text = "edited creature";

                    byte[] built = editor.Build().Item1;
                    GFF gff = GFF.FromBytes(built);
                    var utc = UTCHelpers.ConstructUtc(gff);

                    Assert.That(utc.Tag, Is.EqualTo("test_creature_tag"));
                    Assert.That(utc.ResRef.ToString(), Is.EqualTo("test_creature"));
                    Assert.That(utc.Conversation.ToString(), Is.EqualTo("test_dlg"));
                    Assert.That(utc.Alignment, Is.EqualTo(75));
                    Assert.That(utc.Plot, Is.True);
                    Assert.That(utc.NoPermDeath, Is.True);
                    Assert.That(utc.Min1Hp, Is.True);
                    Assert.That(utc.Disarmable, Is.True);
                    Assert.That(utc.Strength, Is.EqualTo(16));
                    Assert.That(utc.Dexterity, Is.EqualTo(14));
                    Assert.That(utc.Constitution, Is.EqualTo(13));
                    Assert.That(utc.Intelligence, Is.EqualTo(12));
                    Assert.That(utc.Wisdom, Is.EqualTo(11));
                    Assert.That(utc.Charisma, Is.EqualTo(10));
                    Assert.That(utc.NaturalAc, Is.EqualTo(4));
                    Assert.That(utc.Hp, Is.EqualTo(30));
                    Assert.That(utc.CurrentHp, Is.EqualTo(24));
                    Assert.That(utc.MaxHp, Is.EqualTo(35));
                    Assert.That(utc.OnSpawn.ToString(), Is.EqualTo("k_spawn"));
                    Assert.That(utc.OnHeartbeat.ToString(), Is.EqualTo("k_heart"));
                    Assert.That(utc.OnDeath.ToString(), Is.EqualTo("k_death"));
                    Assert.That(utc.Comment, Is.EqualTo("edited creature"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTI, ResourceType.UTI);
                    var editor = new OdyToolUTI(null, null);
                    editor.Load("test.uti", "test", ResourceType.UTI, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_LoadBtiAlias_BuildPreservesBtiContent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.BTI, ResourceType.BTI);
                    var editor = new OdyToolUTI(null, null);

                    editor.Load("test.bti", "test", ResourceType.BTI, data);
                    Tuple<byte[], byte[]> result = editor.Build();

                    Assert.That(result.Item1, Is.Not.Null.And.Length.GreaterThan(0));
                    var rebuilt = GFF.FromBytes(result.Item1);
                    Assert.That(rebuilt.Root, Is.Not.Null);
                    Assert.That(rebuilt.Content, Is.EqualTo(GFFContent.BTI));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTI(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.AvailablePropertyList, Is.Not.Null);
                    Assert.That(editor.AssignedPropertiesList, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_GenerateTagAndResref_MatchesHolocronAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTI(null, null);
                    editor.Load("m12aa_item.uti", "m12aa_item", ResourceType.UTI, MinimalGffBytes(GFFContent.UTI, ResourceType.UTI));

                    editor.ResrefGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.ResrefEdit.Text, Is.EqualTo("m12aa_item"));
                    Assert.That(editor.IsDirty, Is.True);

                    editor.TagEdit.Text = "";
                    editor.TagGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.TagEdit.Text, Is.EqualTo("m12aa_item"));

                    var emptyEditor = new OdyToolUTI(null, null);
                    emptyEditor.ResrefGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(emptyEditor.ResrefEdit.Text, Is.EqualTo("m00xx_itm_000"));
                    Assert.That(emptyEditor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_FieldEdits_BuildIntoUti()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTI(null, null);
                    editor.TagEdit.Text = "test_item_tag";
                    editor.ResrefEdit.Text = "test_item";
                    editor.CostSpin.Value = 42;
                    editor.AdditionalCostSpin.Value = 7;
                    editor.StackSpin.Value = 3;
                    editor.CommentsEdit.Text = "edited item";

                    byte[] built = editor.Build().Item1;
                    var uti = UTIHelpers.ConstructUti(GFF.FromBytes(built));

                    Assert.That(uti.Tag, Is.EqualTo("test_item_tag"));
                    Assert.That(uti.ResRef.ToString(), Is.EqualTo("test_item"));
                    Assert.That(uti.Cost, Is.EqualTo(42));
                    Assert.That(uti.AddCost, Is.EqualTo(7));
                    Assert.That(uti.StackSize, Is.EqualTo(3));
                    Assert.That(uti.Comment, Is.EqualTo("edited item"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_ResRef_TrimsWhitespaceAndClearsBlankValue()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTI
                    {
                        Tag = "test_item",
                        ResRef = new ResRef("old_item")
                    };
                    byte[] data = GFFAuto.BytesGff(UTIHelpers.DismantleUti(source), ResourceType.UTI);

                    var editor = new OdyToolUTI(null, null);
                    editor.Load("test.uti", "test", ResourceType.UTI, data);
                    editor.ResrefEdit.Text = "  new_item  ";

                    var rebuilt = UTIHelpers.ConstructUti(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("new_item"));

                    editor.ResrefEdit.Text = "   ";
                    rebuilt = UTIHelpers.ConstructUti(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo(""));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_PropertyButtonsTrackAssignedSelection()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTI
                    {
                        Tag = "test_item",
                        ResRef = ResRef.FromString("test_item")
                    };
                    source.Properties.Add(new UTIProperty
                    {
                        PropertyName = 1,
                        Subtype = 2,
                        CostTable = 255,
                        CostValue = 0,
                        Param1 = 255,
                        Param1Value = 0,
                        ChanceAppear = 100
                    });
                    byte[] data = GFFAuto.BytesGff(UTIHelpers.DismantleUti(source), ResourceType.UTI);

                    var editor = new OdyToolUTI(null, null);
                    editor.Load("test.uti", "test", ResourceType.UTI, data);

                    Assert.That(editor.AddPropertyBtn.IsEnabled, Is.False);
                    Assert.That(editor.RemovePropertyBtn.IsEnabled, Is.False);
                    Assert.That(editor.EditPropertyBtn.IsEnabled, Is.False);
                    Assert.That(editor.AssignedPropertiesListItemCount, Is.EqualTo(1));

                    editor.AssignedPropertiesList.SelectedIndex = 0;

                    Assert.That(editor.RemovePropertyBtn.IsEnabled, Is.True);
                    Assert.That(editor.EditPropertyBtn.IsEnabled, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTIEditor_RemoveAssignedProperty_MarksDirtyAndBuildsLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTI
                    {
                        Tag = "test_item",
                        ResRef = ResRef.FromString("test_item")
                    };
                    source.Properties.Add(new UTIProperty
                    {
                        PropertyName = 1,
                        Subtype = 2,
                        CostTable = 255,
                        CostValue = 0,
                        Param1 = 255,
                        Param1Value = 0,
                        ChanceAppear = 100
                    });
                    byte[] data = GFFAuto.BytesGff(UTIHelpers.DismantleUti(source), ResourceType.UTI);

                    var editor = new OdyToolUTI(null, null);
                    editor.Load("test.uti", "test", ResourceType.UTI, data);
                    editor.AssignedPropertiesList.SelectedIndex = 0;

                    editor.RemovePropertyBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));

                    Assert.That(editor.AssignedPropertiesListItemCount, Is.EqualTo(0));
                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = UTIHelpers.ConstructUti(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Properties, Is.Empty);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(120000)]
        public async Task UTDEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTD, ResourceType.UTD);
                    var editor = new OdyToolUTD(null, null);
                    editor.Load("test.utd", "test", ResourceType.UTD, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTDEditor_LoadBtd_BuildPreservesBtdContent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTD, ResourceType.UTD);
                    var editor = new OdyToolUTD(null, null);
                    editor.Load("test.btd", "test", ResourceType.BTD, data);

                    byte[] built = editor.Build().Item1;
                    GFF rebuilt = GFF.FromBytes(built);

                    Assert.That(rebuilt.Content, Is.EqualTo(GFFContent.BTD));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTDEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTD(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.AppearanceSelect, Is.Not.Null);
                    Assert.That(editor.LockedCheckbox, Is.Not.Null);
                    Assert.That(editor.NeedKeyCheckbox, Is.Not.Null);
                    Assert.That(editor.ScriptFields.Keys, Does.Contain("OnOpen"));
                    Assert.That(editor.ScriptFields.Keys, Does.Contain("OnPower"));
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTDEditor_GenerateTagAndResref_MatchesHolocronAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTD(null, null);
                    editor.Load("m12aa_door.utd", "m12aa_door", ResourceType.UTD, MinimalGffBytes(GFFContent.UTD, ResourceType.UTD));

                    editor.ResrefGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.ResrefEdit.Text, Is.EqualTo("m12aa_door"));
                    Assert.That(editor.IsDirty, Is.True);

                    editor.TagEdit.Text = "";
                    editor.TagGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.TagEdit.Text, Is.EqualTo("m12aa_door"));

                    var emptyEditor = new OdyToolUTD(null, null);
                    emptyEditor.ResrefGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(emptyEditor.ResrefEdit.Text, Is.EqualTo("m00xx_dor_000"));
                    Assert.That(emptyEditor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTDEditor_FieldEdits_BuildIntoUtd()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTD(null, null);
                    editor.TagEdit.Text = "test_door_tag";
                    editor.ResrefEdit.Text = "test_door";
                    editor.CurrentHpSpin.Value = 12;
                    editor.MaxHpSpin.Value = 34;
                    editor.HardnessSpin.Value = 5;
                    editor.LockedCheckbox.IsChecked = true;
                    editor.NeedKeyCheckbox.IsChecked = true;
                    editor.RemoveKeyCheckbox.IsChecked = true;
                    editor.KeyEdit.Text = "door_key";
                    editor.OpenLockSpin.Value = 15;
                    editor.ScriptFields["OnOpen"].Text = "k_open";
                    editor.ScriptFields["OnPower"].Text = "k_power";
                    editor.CommentsEdit.Text = "edited door";

                    byte[] built = editor.Build().Item1;
                    var utd = UTDHelpers.ConstructUtd(GFF.FromBytes(built));

                    Assert.That(utd.Tag, Is.EqualTo("test_door_tag"));
                    Assert.That(utd.ResRef.ToString(), Is.EqualTo("test_door"));
                    Assert.That(utd.CurrentHp, Is.EqualTo(12));
                    Assert.That(utd.MaximumHp, Is.EqualTo(34));
                    Assert.That(utd.Hardness, Is.EqualTo(5));
                    Assert.That(utd.Locked, Is.True);
                    Assert.That(utd.KeyRequired, Is.True);
                    Assert.That(utd.AutoRemoveKey, Is.True);
                    Assert.That(utd.KeyName, Is.EqualTo("door_key"));
                    Assert.That(utd.UnlockDc, Is.EqualTo(15));
                    Assert.That(utd.OnOpen.ToString(), Is.EqualTo("k_open"));
                    Assert.That(utd.OnPower.ToString(), Is.EqualTo("k_power"));
                    Assert.That(utd.Comment, Is.EqualTo("edited door"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTDEditor_ScriptFields_TrimWhitespaceAndClearBlankValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTD(null, null);

                    editor.ScriptFields["OnClick"].Text = "  k_click  ";
                    editor.ScriptFields["OnClosed"].Text = "   ";
                    editor.ScriptFields["OnOpen"].Text = " k_open ";
                    editor.ScriptFields["OnPower"].Text = " k_power ";
                    editor.ScriptFields["OnUserDefined"].Text = " k_user ";

                    var utd = UTDHelpers.ConstructUtd(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(utd.OnClick.ToString(), Is.EqualTo("k_click"));
                    Assert.That(utd.OnClosed.ToString(), Is.EqualTo(""));
                    Assert.That(utd.OnOpen.ToString(), Is.EqualTo("k_open"));
                    Assert.That(utd.OnPower.ToString(), Is.EqualTo("k_power"));
                    Assert.That(utd.OnUserDefined.ToString(), Is.EqualTo("k_user"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTDEditor_VisibleFieldEdits_MarkDirtyAndBuildIntoUtd()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTD(null, null);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.TagEdit.Text = "dirty_door_tag";
                    editor.ResrefEdit.Text = "dirty_door";
                    editor.CurrentHpSpin.Value = 22;
                    editor.MaxHpSpin.Value = 44;
                    editor.HardnessSpin.Value = 7;
                    editor.LockedCheckbox.IsChecked = true;
                    editor.NeedKeyCheckbox.IsChecked = true;
                    editor.RemoveKeyCheckbox.IsChecked = true;
                    editor.KeyEdit.Text = "dirty_key";
                    editor.OpenLockSpin.Value = 21;
                    editor.ScriptFields["OnOpen"].Text = "k_dirty_open";
                    editor.ScriptFields["OnPower"].Text = "k_dirty_power";
                    editor.CommentsEdit.Text = "dirty door";

                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = UTDHelpers.ConstructUtd(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Tag, Is.EqualTo("dirty_door_tag"));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("dirty_door"));
                    Assert.That(rebuilt.CurrentHp, Is.EqualTo(22));
                    Assert.That(rebuilt.MaximumHp, Is.EqualTo(44));
                    Assert.That(rebuilt.Hardness, Is.EqualTo(7));
                    Assert.That(rebuilt.Locked, Is.True);
                    Assert.That(rebuilt.KeyRequired, Is.True);
                    Assert.That(rebuilt.AutoRemoveKey, Is.True);
                    Assert.That(rebuilt.KeyName, Is.EqualTo("dirty_key"));
                    Assert.That(rebuilt.UnlockDc, Is.EqualTo(21));
                    Assert.That(rebuilt.OnOpen.ToString(), Is.EqualTo("k_dirty_open"));
                    Assert.That(rebuilt.OnPower.ToString(), Is.EqualTo("k_dirty_power"));
                    Assert.That(rebuilt.Comment, Is.EqualTo("dirty door"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTE, ResourceType.UTE);
                    var editor = new OdyToolUTE(null, null);
                    editor.Load("test.ute", "test", ResourceType.UTE, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTE(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.NameEdit, Is.Not.Null);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.DifficultySelect, Is.Not.Null);
                    Assert.That(editor.SpawnSelect, Is.Not.Null);
                    Assert.That(editor.MinCreatureSpin, Is.Not.Null);
                    Assert.That(editor.MaxCreatureSpin, Is.Not.Null);
                    Assert.That(editor.ActiveCheckbox, Is.Not.Null);
                    Assert.That(editor.CreatureTable, Is.Not.Null);
                    Assert.That(editor.OnEnterSelect, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public void UTEEditor_EncounterCreatureActionsUseUtcResources()
        {
            Assert.That(OdyToolUTE.EncounterCreatureResourceType, Is.EqualTo(ResourceType.UTC));
            Assert.That(OdyToolUTE.EncounterCreatureMissingMessage("c_test"), Does.Contain("c_test.utc"));
            Assert.That(OdyToolUTE.EncounterCreatureMissingMessage("c_test"), Does.Not.Contain("c_test.utp"));
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_FieldEdits_BuildIntoUte()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string tag = null;
                string resref = null;
                bool singleShot = true;
                int recCreatures = 0;
                int maxCreatures = 0;
                bool active = false;
                int playerOnly = 0;
                int reset = 0;
                int resetTime = 0;
                int respawns = 0;
                string onEntered = null;
                string onExit = null;
                string onExhausted = null;
                string onHeartbeat = null;
                string onUserDefined = null;
                string comment = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTE(null, null);
                    editor.TagEdit.Text = "test_enc_tag";
                    editor.ResrefEdit.Text = "test_enc";
                    editor.SpawnSelect.SelectedIndex = 1;
                    editor.MinCreatureSpin.Value = 2;
                    editor.MaxCreatureSpin.Value = 4;
                    editor.ActiveCheckbox.IsChecked = true;
                    editor.PlayerOnlyCheckbox.IsChecked = true;
                    editor.RespawnsCheckbox.IsChecked = true;
                    editor.RespawnTimeSpin.Value = 30;
                    editor.RespawnCountSpin.Value = 3;
                    editor.OnEnterSelect.Text = "k_enc_enter";
                    editor.OnExitSelect.Text = "k_enc_exit";
                    editor.OnExhaustedEdit.Text = "k_enc_done";
                    editor.OnHeartbeatSelect.Text = "k_enc_heart";
                    editor.OnUserDefinedSelect.Text = "k_enc_user";
                    editor.CommentsEdit.Text = "edited encounter";

                    byte[] built = editor.Build().Item1;
                    var ute = UTEHelpers.ConstructUte(GFF.FromBytes(built));

                    tag = ute.Tag;
                    resref = ute.ResRef.ToString();
                    singleShot = ute.SingleShot;
                    recCreatures = ute.RecCreatures;
                    maxCreatures = ute.MaxCreatures;
                    active = ute.Active;
                    playerOnly = ute.PlayerOnly;
                    reset = ute.Reset;
                    resetTime = ute.ResetTime;
                    respawns = ute.Respawns;
                    onEntered = ute.OnEntered.ToString();
                    onExit = ute.OnExit.ToString();
                    onExhausted = ute.OnExhausted.ToString();
                    onHeartbeat = ute.OnHeartbeat.ToString();
                    onUserDefined = ute.OnUserDefined.ToString();
                    comment = ute.Comment;
                }, CancellationToken.None);

                Assert.That(tag, Is.EqualTo("test_enc_tag"));
                Assert.That(resref, Is.EqualTo("test_enc"));
                Assert.That(singleShot, Is.False);
                Assert.That(recCreatures, Is.EqualTo(2));
                Assert.That(maxCreatures, Is.EqualTo(4));
                Assert.That(active, Is.True);
                Assert.That(playerOnly, Is.EqualTo(1));
                Assert.That(reset, Is.EqualTo(1));
                Assert.That(resetTime, Is.EqualTo(30));
                Assert.That(respawns, Is.EqualTo(3));
                Assert.That(onEntered, Is.EqualTo("k_enc_enter"));
                Assert.That(onExit, Is.EqualTo("k_enc_exit"));
                Assert.That(onExhausted, Is.EqualTo("k_enc_done"));
                Assert.That(onHeartbeat, Is.EqualTo("k_enc_heart"));
                Assert.That(onUserDefined, Is.EqualTo("k_enc_user"));
                Assert.That(comment, Is.EqualTo("edited encounter"));
            }
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_VisibleFieldEdits_MarkDirtyAndBuildIntoUte()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTE(null, null);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.TagEdit.Text = "dirty_enc_tag";
                    editor.ResrefEdit.Text = "dirty_enc";
                    editor.SpawnSelect.SelectedIndex = 1;
                    editor.MinCreatureSpin.Value = 3;
                    editor.MaxCreatureSpin.Value = 5;
                    editor.ActiveCheckbox.IsChecked = true;
                    editor.PlayerOnlyCheckbox.IsChecked = true;
                    editor.RespawnsCheckbox.IsChecked = true;
                    editor.RespawnTimeSpin.Value = 45;
                    editor.RespawnCountSpin.Value = 6;
                    editor.OnEnterSelect.Text = "k_dirty_enter";
                    editor.OnExitSelect.Text = "k_dirty_exit";
                    editor.OnExhaustedEdit.Text = "k_dirty_done";
                    editor.OnHeartbeatSelect.Text = "k_dirty_heart";
                    editor.OnUserDefinedSelect.Text = "k_dirty_user";
                    editor.CommentsEdit.Text = "dirty encounter";

                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = UTEHelpers.ConstructUte(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Tag, Is.EqualTo("dirty_enc_tag"));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("dirty_enc"));
                    Assert.That(rebuilt.SingleShot, Is.False);
                    Assert.That(rebuilt.RecCreatures, Is.EqualTo(3));
                    Assert.That(rebuilt.MaxCreatures, Is.EqualTo(5));
                    Assert.That(rebuilt.Active, Is.True);
                    Assert.That(rebuilt.PlayerOnly, Is.EqualTo(1));
                    Assert.That(rebuilt.Reset, Is.EqualTo(1));
                    Assert.That(rebuilt.ResetTime, Is.EqualTo(45));
                    Assert.That(rebuilt.Respawns, Is.EqualTo(6));
                    Assert.That(rebuilt.OnEntered.ToString(), Is.EqualTo("k_dirty_enter"));
                    Assert.That(rebuilt.OnExit.ToString(), Is.EqualTo("k_dirty_exit"));
                    Assert.That(rebuilt.OnExhausted.ToString(), Is.EqualTo("k_dirty_done"));
                    Assert.That(rebuilt.OnHeartbeat.ToString(), Is.EqualTo("k_dirty_heart"));
                    Assert.That(rebuilt.OnUserDefined.ToString(), Is.EqualTo("k_dirty_user"));
                    Assert.That(rebuilt.Comment, Is.EqualTo("dirty encounter"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_RemoveAllCreatures_BuildsEmptyCreatureTable()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTE
                    {
                        Tag = "test_enc",
                        ResRef = new ResRef("test_enc")
                    };
                    source.Creatures.Add(new UTECreature
                    {
                        ResRef = new ResRef("c_test"),
                        Appearance = 2,
                        CR = 3,
                        SingleSpawn = 1
                    });
                    byte[] data = GFFAuto.BytesGff(UTEHelpers.DismantleUte(source), ResourceType.UTE);

                    var editor = new OdyToolUTE(null, null);
                    editor.Load("test.ute", "test", ResourceType.UTE, data);
                    editor.CreatureTable.SelectedIndex = 0;

                    editor.RemoveCreatureButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));

                    Assert.That(editor.IsDirty, Is.True);
                    var rebuilt = UTEHelpers.ConstructUte(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Creatures, Is.Empty);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_CreatureRowEdit_MarksDirtyAndBuildsIntoUte()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTE
                    {
                        Tag = "test_enc",
                        ResRef = new ResRef("test_enc")
                    };
                    source.Creatures.Add(new UTECreature
                    {
                        ResRef = new ResRef("c_old"),
                        Appearance = 2,
                        CR = 3,
                        SingleSpawn = 1
                    });
                    byte[] data = GFFAuto.BytesGff(UTEHelpers.DismantleUte(source), ResourceType.UTE);

                    var editor = new OdyToolUTE(null, null);
                    editor.Load("test.ute", "test", ResourceType.UTE, data);

                    Assert.That(editor.IsDirty, Is.False);

                    object row = ((System.Collections.IEnumerable)editor.CreatureTable.ItemsSource).Cast<object>().First();
                    row.GetType().GetProperty("ResRef")?.SetValue(row, "c_new");
                    row.GetType().GetProperty("Appearance")?.SetValue(row, 4);
                    row.GetType().GetProperty("CR")?.SetValue(row, 7.0f);
                    row.GetType().GetProperty("SingleSpawn")?.SetValue(row, false);

                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = UTEHelpers.ConstructUte(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Creatures, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Creatures[0].ResRef.ToString(), Is.EqualTo("c_new"));
                    Assert.That(rebuilt.Creatures[0].AppearanceId, Is.EqualTo(4));
                    Assert.That(rebuilt.Creatures[0].ChallengeRating, Is.EqualTo(7.0f).Within(0.001f));
                    Assert.That(rebuilt.Creatures[0].SingleSpawnBool, Is.False);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_LoadBte_BuildPreservesBteContent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.BTE, ResourceType.BTE);
                    var editor = new OdyToolUTE(null, null);
                    editor.Load("test.bte", "test", ResourceType.BTE, data);

                    byte[] built = editor.Build().Item1;
                    GFF gff = GFF.FromBytes(built);

                    Assert.That(gff.Content, Is.EqualTo(GFFContent.BTE));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTEEditor_ScriptFields_TrimWhitespaceAndClearBlankValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTE
                    {
                        Tag = "test_enc",
                        ResRef = new ResRef("test_enc"),
                        OnEntered = new ResRef("old_enter"),
                        OnExit = new ResRef("old_exit"),
                        OnExhausted = new ResRef("old_done"),
                        OnHeartbeat = new ResRef("old_heart"),
                        OnUserDefined = new ResRef("old_user")
                    };
                    byte[] data = GFFAuto.BytesGff(UTEHelpers.DismantleUte(source), ResourceType.UTE);

                    var editor = new OdyToolUTE(null, null);
                    editor.Load("test.ute", "test", ResourceType.UTE, data);
                    editor.OnEnterSelect.Text = "  k_enter  ";
                    editor.OnExitSelect.Text = "   ";
                    editor.OnExhaustedEdit.Text = "  k_done";
                    editor.OnHeartbeatSelect.Text = "k_heart  ";
                    editor.OnUserDefinedSelect.Text = "";

                    var rebuilt = UTEHelpers.ConstructUte(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.OnEntered.ToString(), Is.EqualTo("k_enter"));
                    Assert.That(rebuilt.OnExit.ToString(), Is.EqualTo(""));
                    Assert.That(rebuilt.OnExhausted.ToString(), Is.EqualTo("k_done"));
                    Assert.That(rebuilt.OnHeartbeat.ToString(), Is.EqualTo("k_heart"));
                    Assert.That(rebuilt.OnUserDefined.ToString(), Is.EqualTo(""));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTM, ResourceType.UTM);
                    var editor = new OdyToolUTM(null, null);
                    editor.Load("test.utm", "test", ResourceType.UTM, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_LoadBtm_BuildPreservesBtmContent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTM, ResourceType.UTM);
                    var editor = new OdyToolUTM(null, null);
                    editor.Load("test.btm", "test", ResourceType.BTM, data);

                    byte[] built = editor.Build().Item1;
                    GFF rebuilt = GFF.FromBytes(built);

                    Assert.That(rebuilt.Content, Is.EqualTo(GFFContent.BTM));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        [TestCase("test.utm.xml", nameof(ResourceType.UTM_XML))]
        [TestCase("test.utm.json", nameof(ResourceType.UTM_JSON))]
        public async Task UTMEditor_LoadTextGffAlias_BuildPreservesTextFormat(string path, string resourceTypeName)
        {
            var restype = ResourceType.FromName(resourceTypeName);
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new GFF(GFFContent.UTM);
                    byte[] data = GFFAuto.BytesGff(source, restype);
                    var editor = new OdyToolUTM(null, null);

                    editor.Load(path, "test", restype, data);
                    byte[] built = editor.Build().Item1;
                    var rebuilt = GFFAuto.ReadGff(built, fileFormat: restype);

                    Assert.That(rebuilt.Content, Is.EqualTo(GFFContent.UTM));
                    Assert.That(System.Text.Encoding.UTF8.GetString(built).TrimStart()[0], Is.AnyOf('<', '{'));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTM(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.NameEdit, Is.Not.Null);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.IdSpin, Is.Not.Null);
                    Assert.That(editor.MarkUpSpin, Is.Not.Null);
                    Assert.That(editor.MarkDownSpin, Is.Not.Null);
                    Assert.That(editor.OnOpenEdit, Is.Not.Null);
                    Assert.That(editor.StoreFlagSelect, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_BuildsMerchantFieldsFromStructuredSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTM(null, null);
                    editor.TagEdit.Text = "test_store_tag";
                    editor.ResrefEdit.Text = "test_store";
                    editor.IdSpin.Value = 42;
                    editor.MarkUpSpin.Value = 150;
                    editor.MarkDownSpin.Value = 25;
                    editor.OnOpenEdit.Text = "k_store_open";
                    editor.StoreFlagSelect.SelectedIndex = 2;
                    editor.CommentsEdit.Text = "edited merchant";

                    Tuple<byte[], byte[]> result = editor.Build();
                    var gff = GFF.FromBytes(result.Item1);
                    var utm = BioWare.Resource.Formats.GFF.Generics.UTM.UTMHelpers.ConstructUtm(gff);

                    Assert.That(utm.Tag, Is.EqualTo("test_store_tag"));
                    Assert.That(utm.ResRef.ToString(), Is.EqualTo("test_store"));
                    Assert.That(utm.Id, Is.EqualTo(42));
                    Assert.That(utm.MarkUp, Is.EqualTo(150));
                    Assert.That(utm.MarkDown, Is.EqualTo(25));
                    Assert.That(utm.OnOpenScript.ToString(), Is.EqualTo("k_store_open"));
                    Assert.That(utm.CanBuy, Is.True);
                    Assert.That(utm.CanSell, Is.True);
                    Assert.That(utm.Comment, Is.EqualTo("edited merchant"));
                    Assert.That(result.Item2, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_OnOpenScript_TrimsWhitespaceAndClearsBlankValue()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTM(null, null);

                    editor.OnOpenEdit.Text = "  k_store_open  ";
                    var trimmed = BioWare.Resource.Formats.GFF.Generics.UTM.UTMHelpers.ConstructUtm(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(trimmed.OnOpenScript.ToString(), Is.EqualTo("k_store_open"));

                    editor.OnOpenEdit.Text = "   ";
                    var blank = BioWare.Resource.Formats.GFF.Generics.UTM.UTMHelpers.ConstructUtm(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(blank.OnOpenScript.ToString(), Is.EqualTo(""));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_VisibleFieldEdits_MarkDirtyAndBuildIntoUtm()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTM(null, null);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.TagEdit.Text = "dirty_store_tag";
                    editor.ResrefEdit.Text = "dirty_store";
                    editor.IdSpin.Value = 73;
                    editor.MarkUpSpin.Value = 125;
                    editor.MarkDownSpin.Value = 15;
                    editor.OnOpenEdit.Text = "k_dirty_store";
                    editor.StoreFlagSelect.SelectedIndex = 1;
                    editor.CommentsEdit.Text = "dirty merchant";

                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = BioWare.Resource.Formats.GFF.Generics.UTM.UTMHelpers.ConstructUtm(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Tag, Is.EqualTo("dirty_store_tag"));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("dirty_store"));
                    Assert.That(rebuilt.Id, Is.EqualTo(73));
                    Assert.That(rebuilt.MarkUp, Is.EqualTo(125));
                    Assert.That(rebuilt.MarkDown, Is.EqualTo(15));
                    Assert.That(rebuilt.OnOpenScript.ToString(), Is.EqualTo("k_dirty_store"));
                    Assert.That(rebuilt.CanBuy, Is.False);
                    Assert.That(rebuilt.CanSell, Is.True);
                    Assert.That(rebuilt.Comment, Is.EqualTo("dirty merchant"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_ApplyInventoryResult_BuildsStoreItemsAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTM(null, null);

                    editor.ApplyInventoryResult(new System.Collections.Generic.List<InventoryItem>
                    {
                        new InventoryItem(new ResRef("g_i_mask01"), droppable: true, infinite: true)
                    });

                    var rebuilt = BioWare.Resource.Formats.GFF.Generics.UTM.UTMHelpers.ConstructUtm(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.Items, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Items[0].ResRef.ToString(), Is.EqualTo("g_i_mask01"));
                    Assert.That(rebuilt.Items[0].Droppable, Is.EqualTo(1));
                    Assert.That(rebuilt.Items[0].Infinite, Is.EqualTo(1));
                    Assert.That(editor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTMEditor_AllowsStoreInventoryDialogRequestWithoutInstallation()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTM(null, null);
                    editor.ApplyInventoryResult(new System.Collections.Generic.List<InventoryItem>
                    {
                        new InventoryItem(new ResRef("g_i_mask01"), droppable: true, infinite: true)
                    });

                    Assert.That(editor.CanOpenInventoryWithoutInstallationForTest(), Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(120000)]
        public async Task UTPEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTP, ResourceType.UTP);
                    var editor = new OdyToolUTP(null, null);
                    editor.Load("test.utp", "test", ResourceType.UTP, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_LoadBtp_BuildPreservesBtpContent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTP, ResourceType.UTP);
                    var editor = new OdyToolUTP(null, null);
                    editor.Load("test.btp", "test", ResourceType.BTP, data);

                    byte[] built = editor.Build().Item1;
                    GFF rebuilt = GFF.FromBytes(built);

                    Assert.That(rebuilt.Content, Is.EqualTo(GFFContent.BTP));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        [TestCase("test.utp.xml", nameof(ResourceType.UTP_XML))]
        [TestCase("test.utp.json", nameof(ResourceType.UTP_JSON))]
        public async Task UTPEditor_LoadTextGffAlias_BuildPreservesTextFormat(string path, string resourceTypeName)
        {
            var restype = ResourceType.FromName(resourceTypeName);
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new GFF(GFFContent.UTP);
                    byte[] data = GFFAuto.BytesGff(source, restype);
                    var editor = new OdyToolUTP(null, null);

                    editor.Load(path, "test", restype, data);
                    byte[] built = editor.Build().Item1;
                    var rebuilt = GFFAuto.ReadGff(built, fileFormat: restype);

                    Assert.That(rebuilt.Content, Is.EqualTo(GFFContent.UTP));
                    Assert.That(System.Text.Encoding.UTF8.GetString(built).TrimStart()[0], Is.AnyOf('<', '{'));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTP(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.NameEdit, Is.Not.Null);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.ConversationEdit, Is.Not.Null);
                    Assert.That(editor.HasInventoryCheckbox, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                    Assert.That(editor.ScriptFields.ContainsKey("OnPower"), Is.True);
                    Assert.That(editor.ScriptFields["OnPower"], Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_ApplyInventoryResult_BuildsInventoryAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTP(null, null);

                    editor.ApplyInventoryResult(new System.Collections.Generic.List<InventoryItem>
                    {
                        new InventoryItem(new ResRef("g_i_parts01"), droppable: true)
                    });

                    var rebuilt = UTPHelpers.ConstructUtp(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.Inventory, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Inventory[0].ResRef.ToString(), Is.EqualTo("g_i_parts01"));
                    Assert.That(rebuilt.Inventory[0].Droppable, Is.True);
                    Assert.That(editor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_AllowsInventoryDialogRequestWithoutInstallation()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTP(null, null);
                    editor.ApplyInventoryResult(new System.Collections.Generic.List<InventoryItem>
                    {
                        new InventoryItem(new ResRef("g_i_parts01"), droppable: true)
                    });

                    Assert.That(editor.CanOpenInventoryWithoutInstallationForTest(), Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_GenerateTagAndResref_MatchesHolocronAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTP(null, null);
                    editor.Load("m12aa_plc.utp", "m12aa_plc", ResourceType.UTP, MinimalGffBytes(GFFContent.UTP, ResourceType.UTP));

                    editor.ResrefGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.ResrefEdit.Text, Is.EqualTo("m12aa_plc"));
                    Assert.That(editor.IsDirty, Is.True);

                    editor.TagEdit.Text = "";
                    editor.TagGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.TagEdit.Text, Is.EqualTo("m12aa_plc"));

                    var emptyEditor = new OdyToolUTP(null, null);
                    emptyEditor.ResrefGenerateBtn.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(emptyEditor.ResrefEdit.Text, Is.EqualTo("m00xx_plc_000"));
                    Assert.That(emptyEditor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_FieldEdits_BuildIntoUtp()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string name = null;
                string tag = null;
                string resref = null;
                string conversation = null;
                bool hasInventory = false;
                bool plot = false;
                bool stat = false;
                bool locked = false;
                int unlockDc = 0;
                string keyName = null;
                string onPower = null;
                string comment = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTP(null, null);
                    editor.NameEdit.SetLocString(LocalizedString.FromEnglish("Placeable name"));
                    editor.TagEdit.Text = "test_plc_tag";
                    editor.ResrefEdit.Text = "test_plc";
                    editor.ConversationEdit.Text = "test_dlg";
                    editor.HasInventoryCheckbox.IsChecked = true;
                    editor.PlotCheckbox.IsChecked = true;
                    editor.StaticCheckbox.IsChecked = true;
                    editor.LockedCheckbox.IsChecked = true;
                    editor.OpenLockSpin.Value = 42;
                    editor.KeyEdit.Text = "test_key";
                    editor.ScriptFields["OnPower"].Text = "k_plc_force";
                    editor.CommentsEdit.Text = "edited placeable";

                    byte[] built = editor.Build().Item1;
                    var utp = UTPHelpers.ConstructUtp(GFF.FromBytes(built));

                    name = utp.Name.GetString(Language.English, Gender.Male);
                    tag = utp.Tag;
                    resref = utp.ResRef.ToString();
                    conversation = utp.Conversation.ToString();
                    hasInventory = utp.HasInventory;
                    plot = utp.Plot;
                    stat = utp.Static;
                    locked = utp.Locked;
                    unlockDc = utp.UnlockDc;
                    keyName = utp.KeyName;
                    onPower = utp.OnPower.ToString();
                    comment = utp.Comment;
                }, CancellationToken.None);

                Assert.That(name, Is.EqualTo("Placeable name"));
                Assert.That(tag, Is.EqualTo("test_plc_tag"));
                Assert.That(resref, Is.EqualTo("test_plc"));
                Assert.That(conversation, Is.EqualTo("test_dlg"));
                Assert.That(hasInventory, Is.True);
                Assert.That(plot, Is.True);
                Assert.That(stat, Is.True);
                Assert.That(locked, Is.True);
                Assert.That(unlockDc, Is.EqualTo(42));
                Assert.That(keyName, Is.EqualTo("test_key"));
                Assert.That(onPower, Is.EqualTo("k_plc_force"));
                Assert.That(comment, Is.EqualTo("edited placeable"));
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_ScriptFields_TrimWhitespaceAndClearBlankValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTP(null, null);

                    editor.ScriptFields["OnClosed"].Text = "  k_closed  ";
                    editor.ScriptFields["OnDamaged"].Text = "   ";
                    editor.ScriptFields["OnPower"].Text = " k_power ";
                    editor.ScriptFields["OnOpen"].Text = " k_open ";
                    editor.ScriptFields["OnUserDefined"].Text = " k_user ";

                    var utp = UTPHelpers.ConstructUtp(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(utp.OnClosed.ToString(), Is.EqualTo("k_closed"));
                    Assert.That(utp.OnDamaged.ToString(), Is.EqualTo(""));
                    Assert.That(utp.OnPower.ToString(), Is.EqualTo("k_power"));
                    Assert.That(utp.OnOpen.ToString(), Is.EqualTo("k_open"));
                    Assert.That(utp.OnUserDefined.ToString(), Is.EqualTo("k_user"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTPEditor_VisibleFieldEdits_MarkDirtyAndBuildIntoUtp()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTP(null, null);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.TagEdit.Text = "dirty_plc_tag";
                    editor.ResrefEdit.Text = "dirty_plc";
                    editor.ConversationEdit.Text = "dirty_dlg";
                    editor.HasInventoryCheckbox.IsChecked = true;
                    editor.PlotCheckbox.IsChecked = true;
                    editor.StaticCheckbox.IsChecked = true;
                    editor.LockedCheckbox.IsChecked = true;
                    editor.OpenLockSpin.Value = 51;
                    editor.KeyEdit.Text = "dirty_key";
                    editor.ScriptFields["OnPower"].Text = "k_dirty_power";
                    editor.CommentsEdit.Text = "dirty placeable";

                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = UTPHelpers.ConstructUtp(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Tag, Is.EqualTo("dirty_plc_tag"));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("dirty_plc"));
                    Assert.That(rebuilt.Conversation.ToString(), Is.EqualTo("dirty_dlg"));
                    Assert.That(rebuilt.HasInventory, Is.True);
                    Assert.That(rebuilt.Plot, Is.True);
                    Assert.That(rebuilt.Static, Is.True);
                    Assert.That(rebuilt.Locked, Is.True);
                    Assert.That(rebuilt.UnlockDc, Is.EqualTo(51));
                    Assert.That(rebuilt.KeyName, Is.EqualTo("dirty_key"));
                    Assert.That(rebuilt.OnPower.ToString(), Is.EqualTo("k_dirty_power"));
                    Assert.That(rebuilt.Comment, Is.EqualTo("dirty placeable"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public void UTPHelpers_DismantleConstruct_PreservesPlaceableFields()
        {
            var source = new UTP
            {
                Name = LocalizedString.FromEnglish("Placeable name"),
                Tag = "test_plc_tag",
                ResRef = new ResRef("test_plc"),
                Conversation = new ResRef("test_dlg"),
                HasInventory = true,
                Plot = true,
                Static = true,
                Locked = true,
                UnlockDc = 42,
                KeyName = "test_key",
                OnPower = new ResRef("k_plc_force"),
                Comment = "edited placeable"
            };

            var utp = UTPHelpers.ConstructUtp(UTPHelpers.DismantleUtp(source));

            Assert.That(utp.Name.GetString(Language.English, Gender.Male), Is.EqualTo("Placeable name"));
            Assert.That(utp.Tag, Is.EqualTo("test_plc_tag"));
            Assert.That(utp.ResRef.ToString(), Is.EqualTo("test_plc"));
            Assert.That(utp.Conversation.ToString(), Is.EqualTo("test_dlg"));
            Assert.That(utp.HasInventory, Is.True);
            Assert.That(utp.Plot, Is.True);
            Assert.That(utp.Static, Is.True);
            Assert.That(utp.Locked, Is.True);
            Assert.That(utp.UnlockDc, Is.EqualTo(42));
            Assert.That(utp.KeyName, Is.EqualTo("test_key"));
            Assert.That(utp.OnPower.ToString(), Is.EqualTo("k_plc_force"));
            Assert.That(utp.Comment, Is.EqualTo("edited placeable"));
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTS, ResourceType.UTS);
                    var editor = new OdyToolUTS(null, null);
                    editor.Load("test.uts", "test", ResourceType.UTS, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTS(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.VolumeSlider, Is.Not.Null);
                    Assert.That(editor.SoundList, Is.Not.Null);
                    Assert.That(editor.SoundEdit, Is.Not.Null);
                    Assert.That(editor.StyleRepeatRadio, Is.Not.Null);
                    Assert.That(editor.PlaySpecificRadio, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_FieldEdits_BuildIntoUts()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string tag = null;
                string resref = null;
                int volume = 0;
                bool active = false;
                bool positional = false;
                bool random = false;
                int interval = 0;
                int intervalVariance = 0;
                int volumeVariance = 0;
                float pitchVariance = 0;
                bool looping = false;
                float minDistance = 0;
                float maxDistance = 0;
                float elevation = 0;
                float randomRangeY = 0;
                float randomRangeX = 0;
                string firstSound = null;
                string secondSound = null;
                string comment = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTS(null, null);
                    editor.TagEdit.Text = "test_sound_tag";
                    editor.ResrefEdit.Text = "test_sound";
                    editor.VolumeSlider.Value = 180;
                    editor.ActiveCheckbox.IsChecked = true;
                    editor.PlaySpecificRadio.IsChecked = true;
                    editor.OrderRandomRadio.IsChecked = true;
                    editor.IntervalSpin.Value = 30;
                    editor.IntervalVariationSpin.Value = 7;
                    editor.VolumeVariationSlider.Value = 12;
                    editor.PitchVariationSlider.Value = 25;
                    editor.StyleRepeatRadio.IsChecked = true;
                    editor.CutoffSpin.Value = 40;
                    editor.MaxVolumeDistanceSpin.Value = 15;
                    editor.HeightSpin.Value = 2;
                    editor.NorthRandomSpin.Value = 3;
                    editor.EastRandomSpin.Value = 4;
                    editor.SoundList.Items.Add("amb_wind");
                    editor.SoundList.Items.Add("amb_mach");
                    editor.CommentsEdit.Text = "edited sound";

                    byte[] built = editor.Build().Item1;
                    var uts = UTSHelpers.ConstructUts(GFF.FromBytes(built));

                    tag = uts.Tag;
                    resref = uts.ResRef.ToString();
                    volume = uts.Volume;
                    active = uts.Active;
                    positional = uts.Positional;
                    random = uts.Random;
                    interval = uts.Interval;
                    intervalVariance = uts.IntervalVariance;
                    volumeVariance = uts.VolumeVariance;
                    pitchVariance = uts.PitchVariance;
                    looping = uts.Looping;
                    minDistance = uts.MinDistance;
                    maxDistance = uts.MaxDistance;
                    elevation = uts.Elevation;
                    randomRangeY = uts.RandomRangeY;
                    randomRangeX = uts.RandomRangeX;
                    firstSound = uts.Sounds[0].ToString();
                    secondSound = uts.Sounds[1].ToString();
                    comment = uts.Comment;
                }, CancellationToken.None);

                Assert.That(tag, Is.EqualTo("test_sound_tag"));
                Assert.That(resref, Is.EqualTo("test_sound"));
                Assert.That(volume, Is.EqualTo(180));
                Assert.That(active, Is.True);
                Assert.That(positional, Is.True);
                Assert.That(random, Is.True);
                Assert.That(interval, Is.EqualTo(30));
                Assert.That(intervalVariance, Is.EqualTo(7));
                Assert.That(volumeVariance, Is.EqualTo(12));
                Assert.That(pitchVariance, Is.EqualTo(0.25f).Within(0.001f));
                Assert.That(looping, Is.True);
                Assert.That(minDistance, Is.EqualTo(40f).Within(0.001f));
                Assert.That(maxDistance, Is.EqualTo(15f).Within(0.001f));
                Assert.That(elevation, Is.EqualTo(2f).Within(0.001f));
                Assert.That(randomRangeY, Is.EqualTo(3f).Within(0.001f));
                Assert.That(randomRangeX, Is.EqualTo(4f).Within(0.001f));
                Assert.That(firstSound, Is.EqualTo("amb_wind"));
                Assert.That(secondSound, Is.EqualTo("amb_mach"));
                Assert.That(comment, Is.EqualTo("edited sound"));
            }
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_VisibleFieldEdits_MarkDirtyAndBuildIntoUts()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTS(null, null);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.TagEdit.Text = "dirty_sound_tag";
                    editor.PlaySpecificRadio.IsChecked = true;
                    editor.StyleRepeatRadio.IsChecked = true;
                    editor.OrderRandomRadio.IsChecked = true;
                    editor.VolumeSlider.Value = 181;
                    editor.ActiveCheckbox.IsChecked = true;
                    editor.IntervalSpin.Value = 42;
                    editor.CommentsEdit.Text = "dirty sound";

                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = UTSHelpers.ConstructUts(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Tag, Is.EqualTo("dirty_sound_tag"));
                    Assert.That(rebuilt.Positional, Is.True);
                    Assert.That(rebuilt.Looping, Is.True);
                    Assert.That(rebuilt.Continuous, Is.False);
                    Assert.That(rebuilt.Random, Is.True);
                    Assert.That(rebuilt.Volume, Is.EqualTo(181));
                    Assert.That(rebuilt.Active, Is.True);
                    Assert.That(rebuilt.Interval, Is.EqualTo(42));
                    Assert.That(rebuilt.Comment, Is.EqualTo("dirty sound"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_SelectedSoundEdit_UpdatesListAndBuildsLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTS(null, null);
                    editor.SoundList.Items.Add("amb_old");
                    editor.SoundList.SelectedIndex = 0;

                    editor.SoundEdit.Text = "amb_new";
                    editor.SoundEdit.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Input.InputElement.LostFocusEvent));

                    Assert.That(editor.SoundList.Items[0], Is.EqualTo("amb_new"));
                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = UTSHelpers.ConstructUts(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Sounds, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Sounds[0].ToString(), Is.EqualTo("amb_new"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_SoundListActions_UseResRefDefaultsAndBuildOrder()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTS(null, null);

                    editor.AddSoundForTest();
                    editor.AddSoundForTest();

                    Assert.That(editor.SoundList.Items, Has.Count.EqualTo(2));
                    Assert.That(editor.SoundList.Items[0], Is.EqualTo("new_sound"));
                    Assert.That(editor.SoundList.Items[1], Is.EqualTo("new_sound1"));
                    Assert.That(editor.SoundList.SelectedIndex, Is.EqualTo(1));
                    Assert.That(editor.SoundEdit.Text, Is.EqualTo("new_sound1"));

                    editor.SoundEdit.Text = "  amb_machine  ";
                    editor.SoundEdit.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Input.InputElement.LostFocusEvent));
                    Assert.That(editor.SoundList.Items[1], Is.EqualTo("amb_machine"));

                    editor.MoveSoundUpForTest();
                    Assert.That(editor.SoundList.Items[0], Is.EqualTo("amb_machine"));
                    Assert.That(editor.SoundList.Items[1], Is.EqualTo("new_sound"));

                    var rebuilt = UTSHelpers.ConstructUts(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Sounds.Select(s => s.ToString()), Is.EqualTo(new[] { "amb_machine", "new_sound" }));

                    editor.RemoveSoundForTest();
                    Assert.That(editor.SoundList.Items, Has.Count.EqualTo(1));
                    Assert.That(editor.SoundList.Items[0], Is.EqualTo("new_sound"));

                    var rebuiltAfterRemove = UTSHelpers.ConstructUts(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuiltAfterRemove.Sounds.Select(s => s.ToString()), Is.EqualTo(new[] { "new_sound" }));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_Build_TrimsAndSkipsBlankSoundEntries()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTS(null, null);
                    editor.SoundList.Items.Add("  amb_wind  ");
                    editor.SoundList.Items.Add("   ");
                    editor.SoundList.Items.Add("amb_mach");

                    var rebuilt = UTSHelpers.ConstructUts(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.Sounds.Select(s => s.ToString()), Is.EqualTo(new[] { "amb_wind", "amb_mach" }));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTSEditor_ResRef_TrimsWhitespaceAndClearsBlankValue()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTS
                    {
                        Tag = "test_sound",
                        ResRef = new ResRef("old_sound")
                    };
                    byte[] data = GFFAuto.BytesGff(UTSHelpers.DismantleUts(source), ResourceType.UTS);

                    var editor = new OdyToolUTS(null, null);
                    editor.Load("test.uts", "test", ResourceType.UTS, data);
                    editor.ResrefEdit.Text = "  new_sound  ";

                    var rebuilt = UTSHelpers.ConstructUts(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("new_sound"));

                    editor.ResrefEdit.Text = "   ";
                    rebuilt = UTSHelpers.ConstructUts(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo(""));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public void UTSHelpers_DismantleConstruct_PreservesSoundFields()
        {
            var source = new UTS
            {
                Tag = "test_sound_tag",
                ResRef = new ResRef("test_sound"),
                Volume = 180,
                Active = true,
                Looping = true,
                Continuous = false,
                Random = true,
                Positional = true,
                Interval = 30,
                IntervalVariance = 7,
                VolumeVariance = 12,
                PitchVariance = 0.25f,
                MinDistance = 40,
                MaxDistance = 15,
                Elevation = 2,
                RandomRangeY = 3,
                RandomRangeX = 4,
                Comment = "edited sound"
            };
            source.Sounds.Add(new ResRef("amb_wind"));
            source.Sounds.Add(new ResRef("amb_mach"));

            var uts = UTSHelpers.ConstructUts(UTSHelpers.DismantleUts(source));

            Assert.That(uts.Tag, Is.EqualTo("test_sound_tag"));
            Assert.That(uts.ResRef.ToString(), Is.EqualTo("test_sound"));
            Assert.That(uts.Volume, Is.EqualTo(180));
            Assert.That(uts.Active, Is.True);
            Assert.That(uts.Looping, Is.True);
            Assert.That(uts.Continuous, Is.False);
            Assert.That(uts.Random, Is.True);
            Assert.That(uts.Positional, Is.True);
            Assert.That(uts.Interval, Is.EqualTo(30));
            Assert.That(uts.IntervalVariance, Is.EqualTo(7));
            Assert.That(uts.VolumeVariance, Is.EqualTo(12));
            Assert.That(uts.PitchVariance, Is.EqualTo(0.25f).Within(0.001f));
            Assert.That(uts.MinDistance, Is.EqualTo(40f).Within(0.001f));
            Assert.That(uts.MaxDistance, Is.EqualTo(15f).Within(0.001f));
            Assert.That(uts.Elevation, Is.EqualTo(2f).Within(0.001f));
            Assert.That(uts.RandomRangeY, Is.EqualTo(3f).Within(0.001f));
            Assert.That(uts.RandomRangeX, Is.EqualTo(4f).Within(0.001f));
            Assert.That(uts.Sounds, Has.Count.EqualTo(2));
            Assert.That(uts.Sounds[0].ToString(), Is.EqualTo("amb_wind"));
            Assert.That(uts.Sounds[1].ToString(), Is.EqualTo("amb_mach"));
            Assert.That(uts.Comment, Is.EqualTo("edited sound"));
        }

        [Test, Timeout(60000)]
        public async Task UTTEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTT, ResourceType.UTT);
                    var editor = new OdyToolUTT(null, null);
                    editor.Load("test.utt", "test", ResourceType.UTT, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTTEditor_LoadBtt_BuildPreservesBttContent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTT, ResourceType.UTT);
                    var editor = new OdyToolUTT(null, null);
                    editor.Load("test.btt", "test", ResourceType.BTT, data);

                    byte[] built = editor.Build().Item1;
                    GFF rebuilt = GFF.FromBytes(built);

                    Assert.That(rebuilt.Content, Is.EqualTo(GFFContent.BTT));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTTEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTT(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.TypeSelect, Is.Not.Null);
                    Assert.That(editor.IsTrapCheckbox, Is.Not.Null);
                    Assert.That(editor.DetectDcSpin, Is.Not.Null);
                    Assert.That(editor.OnEnterSelect, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTTEditor_GenerateTagAndResref_MatchesHolocronAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTT(null, null);
                    editor.Load("m12aa_trg.utt", "m12aa_trg", ResourceType.UTT, MinimalGffBytes(GFFContent.UTT, ResourceType.UTT));

                    editor.ResrefGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.ResrefEdit.Text, Is.EqualTo("m12aa_trg"));
                    Assert.That(editor.IsDirty, Is.True);

                    editor.TagEdit.Text = "";
                    editor.TagGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.TagEdit.Text, Is.EqualTo("m12aa_trg"));

                    var emptyEditor = new OdyToolUTT(null, null);
                    emptyEditor.ResrefGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(emptyEditor.ResrefEdit.Text, Is.EqualTo("m00xx_trg_000"));
                    Assert.That(emptyEditor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTTEditor_FieldEdits_BuildIntoUtt()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string name = null;
                string tag = null;
                string resref = null;
                int typeId = 0;
                bool autoRemoveKey = false;
                string keyName = null;
                float highlightHeight = 0;
                bool isTrap = false;
                bool trapOnce = false;
                bool trapDetectable = false;
                int trapDetectDc = 0;
                bool trapDisarmable = false;
                int trapDisarmDc = 0;
                string onEnter = null;
                string onTrapTriggered = null;
                string comment = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTT(null, null);
                    editor.NameEdit.SetLocString(LocalizedString.FromEnglish("Trigger name"));
                    editor.TagEdit.Text = "test_trigger_tag";
                    editor.ResrefEdit.Text = "test_trigger";
                    editor.TypeSelect.SelectedIndex = 2;
                    editor.AutoRemoveKeyCheckbox.IsChecked = true;
                    editor.KeyEdit.Text = "trigger_key";
                    editor.HighlightHeightSpin.Value = 1.5m;
                    editor.IsTrapCheckbox.IsChecked = true;
                    editor.ActivateOnceCheckbox.IsChecked = true;
                    editor.DetectableCheckbox.IsChecked = true;
                    editor.DetectDcSpin.Value = 12;
                    editor.DisarmableCheckbox.IsChecked = true;
                    editor.DisarmDcSpin.Value = 18;
                    editor.OnEnterSelect.Text = "k_enter";
                    editor.OnTrapTriggeredEdit.Text = "k_trap";
                    editor.CommentsEdit.Text = "edited trigger";

                    byte[] built = editor.Build().Item1;
                    var utt = UTTHelpers.ConstructUtt(GFF.FromBytes(built));

                    name = utt.Name.GetString(Language.English, Gender.Male);
                    tag = utt.Tag;
                    resref = utt.ResRef.ToString();
                    typeId = utt.TypeId;
                    autoRemoveKey = utt.AutoRemoveKey;
                    keyName = utt.KeyName;
                    highlightHeight = utt.HighlightHeight;
                    isTrap = utt.IsTrap;
                    trapOnce = utt.TrapOnce;
                    trapDetectable = utt.TrapDetectable;
                    trapDetectDc = utt.TrapDetectDc;
                    trapDisarmable = utt.TrapDisarmable;
                    trapDisarmDc = utt.TrapDisarmDc;
                    onEnter = utt.OnEnterScript.ToString();
                    onTrapTriggered = utt.OnTrapTriggeredScript.ToString();
                    comment = utt.Comment;
                }, CancellationToken.None);

                Assert.That(name, Is.EqualTo("Trigger name"));
                Assert.That(tag, Is.EqualTo("test_trigger_tag"));
                Assert.That(resref, Is.EqualTo("test_trigger"));
                Assert.That(typeId, Is.EqualTo(2));
                Assert.That(autoRemoveKey, Is.True);
                Assert.That(keyName, Is.EqualTo("trigger_key"));
                Assert.That(highlightHeight, Is.EqualTo(1.5f).Within(0.001f));
                Assert.That(isTrap, Is.True);
                Assert.That(trapOnce, Is.True);
                Assert.That(trapDetectable, Is.True);
                Assert.That(trapDetectDc, Is.EqualTo(12));
                Assert.That(trapDisarmable, Is.True);
                Assert.That(trapDisarmDc, Is.EqualTo(18));
                Assert.That(onEnter, Is.EqualTo("k_enter"));
                Assert.That(onTrapTriggered, Is.EqualTo("k_trap"));
                Assert.That(comment, Is.EqualTo("edited trigger"));
            }
        }

        [Test, Timeout(60000)]
        public async Task UTTEditor_ScriptFields_TrimWhitespaceAndClearBlankValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTT(null, null);

                    editor.OnClickEdit.Text = "  k_click  ";
                    editor.OnDisarmEdit.Text = "   ";
                    editor.OnEnterSelect.Text = " k_enter ";
                    editor.OnExitSelect.Text = " k_exit ";
                    editor.OnHeartbeatSelect.Text = " k_hb ";
                    editor.OnTrapTriggeredEdit.Text = " k_trap ";
                    editor.OnUserDefinedSelect.Text = " k_user ";

                    var utt = UTTHelpers.ConstructUtt(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(utt.OnClickScript.ToString(), Is.EqualTo("k_click"));
                    Assert.That(utt.OnDisarmScript.ToString(), Is.EqualTo(""));
                    Assert.That(utt.OnEnterScript.ToString(), Is.EqualTo("k_enter"));
                    Assert.That(utt.OnExitScript.ToString(), Is.EqualTo("k_exit"));
                    Assert.That(utt.OnHeartbeatScript.ToString(), Is.EqualTo("k_hb"));
                    Assert.That(utt.OnTrapTriggeredScript.ToString(), Is.EqualTo("k_trap"));
                    Assert.That(utt.OnUserDefinedScript.ToString(), Is.EqualTo("k_user"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public void UTTHelpers_DismantleConstruct_PreservesTriggerFields()
        {
            var source = new UTT
            {
                Tag = "test_trigger_tag",
                ResRef = new ResRef("test_trigger"),
                TypeId = 2,
                AutoRemoveKey = true,
                KeyName = "trigger_key",
                FactionId = 3,
                Cursor = 4,
                HighlightHeight = 1.5f,
                IsTrap = true,
                TrapOnce = true,
                TrapDetectable = true,
                TrapDetectDc = 12,
                TrapDisarmable = true,
                TrapDisarmDc = 18,
                TrapType = 5,
                OnEnterScript = new ResRef("k_enter"),
                OnTrapTriggeredScript = new ResRef("k_trap"),
                OnUserDefinedScript = new ResRef("k_user"),
                Comment = "edited trigger"
            };

            var utt = UTTHelpers.ConstructUtt(UTTHelpers.DismantleUtt(source));

            Assert.That(utt.Tag, Is.EqualTo("test_trigger_tag"));
            Assert.That(utt.ResRef.ToString(), Is.EqualTo("test_trigger"));
            Assert.That(utt.TypeId, Is.EqualTo(2));
            Assert.That(utt.AutoRemoveKey, Is.True);
            Assert.That(utt.KeyName, Is.EqualTo("trigger_key"));
            Assert.That(utt.FactionId, Is.EqualTo(3));
            Assert.That(utt.Cursor, Is.EqualTo(4));
            Assert.That(utt.HighlightHeight, Is.EqualTo(1.5f).Within(0.001f));
            Assert.That(utt.IsTrap, Is.True);
            Assert.That(utt.TrapOnce, Is.True);
            Assert.That(utt.TrapDetectable, Is.True);
            Assert.That(utt.TrapDetectDc, Is.EqualTo(12));
            Assert.That(utt.TrapDisarmable, Is.True);
            Assert.That(utt.TrapDisarmDc, Is.EqualTo(18));
            Assert.That(utt.TrapType, Is.EqualTo(5));
            Assert.That(utt.OnEnterScript.ToString(), Is.EqualTo("k_enter"));
            Assert.That(utt.OnTrapTriggeredScript.ToString(), Is.EqualTo("k_trap"));
            Assert.That(utt.OnUserDefinedScript.ToString(), Is.EqualTo("k_user"));
            Assert.That(utt.Comment, Is.EqualTo("edited trigger"));
        }

        [Test, Timeout(60000)]
        public async Task UTWEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.UTW, ResourceType.UTW);
                    var editor = new OdyToolUTW(null, null);
                    editor.Load("test.utw", "test", ResourceType.UTW, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTWEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTW(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.NameEdit, Is.Not.Null);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.ResrefEdit, Is.Not.Null);
                    Assert.That(editor.IsNoteCheckbox, Is.Not.Null);
                    Assert.That(editor.NoteEnabledCheckbox, Is.Not.Null);
                    Assert.That(editor.NoteEdit, Is.Not.Null);
                    Assert.That(editor.NoteChangeButton, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTWEditor_GenerateTagAndResref_MatchesHolocronAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTW(null, null);
                    editor.Load("m12aa_way.utw", "m12aa_way", ResourceType.UTW, MinimalGffBytes(GFFContent.UTW, ResourceType.UTW));

                    editor.ResrefGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.ResrefEdit.Text, Is.EqualTo("m12aa_way"));
                    Assert.That(editor.IsDirty, Is.True);

                    editor.TagEdit.Text = "";
                    editor.TagGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(editor.TagEdit.Text, Is.EqualTo("m12aa_way"));

                    var emptyEditor = new OdyToolUTW(null, null);
                    emptyEditor.ResrefGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    Assert.That(emptyEditor.ResrefEdit.Text, Is.EqualTo("m00xx_way_000"));
                    Assert.That(emptyEditor.IsDirty, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task UTWEditor_FieldEdits_BuildIntoUtw()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string tag = null;
                string resref = null;
                bool hasMapNote = false;
                bool mapNoteEnabled = false;
                string mapNote = null;
                string comment = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolUTW(null, null);
                    editor.TagEdit.Text = "test_way_tag";
                    editor.ResrefEdit.Text = "test_way";
                    editor.IsNoteCheckbox.IsChecked = true;
                    editor.NoteEnabledCheckbox.IsChecked = true;
                    editor.NoteEdit.SetLocString(LocalizedString.FromEnglish("Waypoint note"));
                    editor.CommentsEdit.Text = "edited waypoint";

                    byte[] built = editor.Build().Item1;
                    var utw = UTWHelpers.ConstructUtw(GFF.FromBytes(built));

                    tag = utw.Tag;
                    resref = utw.ResRef.ToString();
                    hasMapNote = utw.HasMapNote;
                    mapNoteEnabled = utw.MapNoteEnabled;
                    mapNote = utw.MapNote.GetString(Language.English, Gender.Male);
                    comment = utw.Comment;
                }, CancellationToken.None);

                Assert.That(tag, Is.EqualTo("test_way_tag"));
                Assert.That(resref, Is.EqualTo("test_way"));
                Assert.That(hasMapNote, Is.True);
                Assert.That(mapNoteEnabled, Is.True);
                Assert.That(mapNote, Is.EqualTo("Waypoint note"));
                Assert.That(comment, Is.EqualTo("edited waypoint"));
            }
        }

        [Test, Timeout(60000)]
        public async Task UTWEditor_ResRef_TrimsWhitespaceAndClearsBlankValue()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new UTW
                    {
                        Tag = "test_way",
                        ResRef = new ResRef("old_way")
                    };
                    byte[] data = GFFAuto.BytesGff(UTWHelpers.DismantleUtw(source), ResourceType.UTW);

                    var editor = new OdyToolUTW(null, null);
                    editor.Load("test.utw", "test", ResourceType.UTW, data);
                    editor.ResrefEdit.Text = "  new_way  ";

                    var rebuilt = UTWHelpers.ConstructUtw(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("new_way"));

                    editor.ResrefEdit.Text = "   ";
                    rebuilt = UTWHelpers.ConstructUtw(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo(""));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public void UTWHelpers_DismantleConstruct_PreservesWaypointFields()
        {
            var source = new UTW
            {
                Tag = "test_way_tag",
                ResRef = new ResRef("test_way"),
                Name = LocalizedString.FromEnglish("Waypoint name"),
                HasMapNote = true,
                MapNoteEnabled = true,
                MapNote = LocalizedString.FromEnglish("Waypoint note"),
                Comment = "edited waypoint"
            };

            var utw = UTWHelpers.ConstructUtw(UTWHelpers.DismantleUtw(source));

            Assert.That(utw.Tag, Is.EqualTo("test_way_tag"));
            Assert.That(utw.ResRef.ToString(), Is.EqualTo("test_way"));
            Assert.That(utw.Name.GetString(Language.English, Gender.Male), Is.EqualTo("Waypoint name"));
            Assert.That(utw.HasMapNote, Is.True);
            Assert.That(utw.MapNoteEnabled, Is.True);
            Assert.That(utw.MapNote.GetString(Language.English, Gender.Male), Is.EqualTo("Waypoint note"));
            Assert.That(utw.Comment, Is.EqualTo("edited waypoint"));
        }

        [Test, Timeout(60000)]
        public async Task AREEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.ARE, ResourceType.ARE);
                    var editor = new OdyToolARE(null, null);
                    editor.Load("test.are", "test", ResourceType.ARE, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task AREEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolARE(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.NameEdit, Is.Not.Null);
                    Assert.That(editor.TagEdit, Is.Not.Null);
                    Assert.That(editor.CameraStyleSelect, Is.Not.Null);
                    Assert.That(editor.EnvmapEdit, Is.Not.Null);
                    Assert.That(editor.DisableTransitCheck, Is.Not.Null);
                    Assert.That(editor.UnescapableCheck, Is.Not.Null);
                    Assert.That(editor.MapAxisSelect, Is.Not.Null);
                    Assert.That(editor.FogEnabledCheck, Is.Not.Null);
                    Assert.That(editor.OnEnterSelect, Is.Not.Null);
                    Assert.That(editor.CommentsEdit, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public void AREEditor_ScriptLocationMissingMessageListsNssAndNcs()
        {
            string message = OdyToolARE.ScriptLocationMissingMessage("k_area_enter");

            Assert.That(message, Does.Contain("k_area_enter.nss"));
            Assert.That(message, Does.Contain("k_area_enter.ncs"));
            Assert.That(message, Does.Not.Contain("standalone ARE editor"));
        }

        [Test, Timeout(60000)]
        public async Task AREEditor_GenerateTag_UsesResrefOrNewAreaLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string generatedFromResref = null;
                string generatedWithoutResref = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolARE(null, null);
                    editor.Load("m12aa.are", "m12aa", ResourceType.ARE, MinimalGffBytes(GFFContent.ARE, ResourceType.ARE));
                    editor.TagGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    generatedFromResref = editor.TagEdit.Text;

                    var emptyEditor = new OdyToolARE(null, null);
                    emptyEditor.TagGenerateButton.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    generatedWithoutResref = emptyEditor.TagEdit.Text;
                }, CancellationToken.None);

                Assert.That(generatedFromResref, Is.EqualTo("m12aa"));
                Assert.That(generatedWithoutResref, Is.EqualTo("newarea"));
            }
        }

        [Test, Timeout(60000)]
        public async Task AREEditor_FieldEdits_BuildIntoAre()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string name = null;
                string tag = null;
                string envmap = null;
                bool disableTransit = false;
                bool unescapable = false;
                float alphaTest = 0;
                bool stealthXp = false;
                int stealthMax = 0;
                int mapZoom = 0;
                int mapResX = 0;
                string onEnter = null;
                string onExit = null;
                string onHeartbeat = null;
                string onUserDefined = null;
                string comment = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolARE(null, null);
                    editor.NameEdit.SetLocString(LocalizedString.FromEnglish("Area name"));
                    editor.TagEdit.Text = "test_area_tag";
                    editor.EnvmapEdit.Text = "test_env";
                    editor.DisableTransitCheck.IsChecked = true;
                    editor.UnescapableCheck.IsChecked = true;
                    editor.AlphaTestSpin.Value = 0.75m;
                    editor.StealthCheck.IsChecked = true;
                    editor.StealthMaxSpin.Value = 125;
                    editor.StealthLossSpin.Value = 7;
                    editor.MapAxisSelect.SelectedIndex = 2;
                    editor.MapZoomSpin.Value = 3;
                    editor.MapResXSpin.Value = 512;
                    editor.MapImageX1Spin.Value = 0.1m;
                    editor.MapImageY1Spin.Value = 0.2m;
                    editor.MapImageX2Spin.Value = 0.9m;
                    editor.MapImageY2Spin.Value = 0.8m;
                    editor.MapWorldX1Spin.Value = -10m;
                    editor.MapWorldY1Spin.Value = -20m;
                    editor.MapWorldX2Spin.Value = 100m;
                    editor.MapWorldY2Spin.Value = 200m;
                    editor.FogEnabledCheck.IsChecked = true;
                    editor.FogNearSpin.Value = 12.5m;
                    editor.FogFarSpin.Value = 300.5m;
                    editor.WindPowerSelect.SelectedIndex = 2;
                    editor.ShadowsCheck.IsChecked = true;
                    editor.ShadowsSpin.Value = 64;
                    editor.GrassTextureEdit.Text = "test_grass";
                    editor.GrassDensitySpin.Value = 1.25m;
                    editor.GrassSizeSpin.Value = 0.75m;
                    editor.OnEnterSelect.Text = "k_area_enter";
                    editor.OnExitSelect.Text = "k_area_exit";
                    editor.OnHeartbeatSelect.Text = "k_area_hb";
                    editor.OnUserDefinedSelect.Text = "k_area_ud";
                    editor.CommentsEdit.Text = "edited area";

                    byte[] built = editor.Build().Item1;
                    var are = AREHelpers.ConstructAre(GFF.FromBytes(built));

                    name = are.Name.GetString(Language.English, Gender.Male);
                    tag = are.Tag;
                    envmap = are.DefaultEnvMap.ToString();
                    disableTransit = are.DisableTransit;
                    unescapable = are.Unescapable;
                    alphaTest = are.AlphaTest;
                    stealthXp = are.StealthXp;
                    stealthMax = are.StealthXpMax;
                    mapZoom = are.MapZoom;
                    mapResX = are.MapResX;
                    onEnter = are.OnEnter.ToString();
                    onExit = are.OnExit.ToString();
                    onHeartbeat = are.OnHeartbeat.ToString();
                    onUserDefined = are.OnUserDefined.ToString();
                    comment = are.Comment;
                }, CancellationToken.None);

                Assert.That(name, Is.EqualTo("Area name"));
                Assert.That(tag, Is.EqualTo("test_area_tag"));
                Assert.That(envmap, Is.EqualTo("test_env"));
                Assert.That(disableTransit, Is.True);
                Assert.That(unescapable, Is.True);
                Assert.That(alphaTest, Is.EqualTo(0.75f).Within(0.001f));
                Assert.That(stealthXp, Is.True);
                Assert.That(stealthMax, Is.EqualTo(125));
                Assert.That(mapZoom, Is.EqualTo(3));
                Assert.That(mapResX, Is.EqualTo(512));
                Assert.That(onEnter, Is.EqualTo("k_area_enter"));
                Assert.That(onExit, Is.EqualTo("k_area_exit"));
                Assert.That(onHeartbeat, Is.EqualTo("k_area_hb"));
                Assert.That(onUserDefined, Is.EqualTo("k_area_ud"));
                Assert.That(comment, Is.EqualTo("edited area"));
            }
        }

        [Test, Timeout(60000)]
        public async Task AREEditor_ResRefFields_TrimWhitespaceAndClearBlankValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new ARE
                    {
                        Tag = "test_area",
                        DefaultEnvMap = new ResRef("old_env"),
                        GrassTexture = new ResRef("old_grass"),
                        OnEnter = new ResRef("old_enter"),
                        OnExit = new ResRef("old_exit"),
                        OnHeartbeat = new ResRef("old_hb"),
                        OnUserDefined = new ResRef("old_user")
                    };
                    byte[] data = GFFAuto.BytesGff(AREHelpers.DismantleAre(source), ResourceType.ARE);

                    var editor = new OdyToolARE(null, null);
                    editor.Load("test.are", "test", ResourceType.ARE, data);
                    editor.EnvmapEdit.Text = "  new_env  ";
                    editor.GrassTextureEdit.Text = "   ";
                    editor.OnEnterSelect.Text = "  k_enter  ";
                    editor.OnExitSelect.Text = "";
                    editor.OnHeartbeatSelect.Text = "k_hb  ";
                    editor.OnUserDefinedSelect.Text = "  ";

                    var rebuilt = AREHelpers.ConstructAre(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.DefaultEnvMap.ToString(), Is.EqualTo("new_env"));
                    Assert.That(rebuilt.GrassTexture.ToString(), Is.EqualTo(""));
                    Assert.That(rebuilt.OnEnter.ToString(), Is.EqualTo("k_enter"));
                    Assert.That(rebuilt.OnExit.ToString(), Is.EqualTo(""));
                    Assert.That(rebuilt.OnHeartbeat.ToString(), Is.EqualTo("k_hb"));
                    Assert.That(rebuilt.OnUserDefined.ToString(), Is.EqualTo(""));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public void AREHelpers_DismantleConstruct_PreservesAreaFields()
        {
            var source = new ARE
            {
                Name = LocalizedString.FromEnglish("Area name"),
                Tag = "test_area_tag",
                DefaultEnvMap = new ResRef("test_env"),
                DisableTransit = true,
                Unescapable = true,
                AlphaTest = 0.75f,
                StealthXp = true,
                StealthXpMax = 125,
                StealthXpLoss = 7,
                NorthAxis = ARENorthAxis.PositiveX,
                MapZoom = 3,
                MapResX = 512,
                OnEnter = new ResRef("k_area_enter"),
                OnExit = new ResRef("k_area_exit"),
                OnHeartbeat = new ResRef("k_area_hb"),
                OnUserDefined = new ResRef("k_area_ud"),
                Comment = "edited area"
            };

            var are = AREHelpers.ConstructAre(AREHelpers.DismantleAre(source));

            Assert.That(are.Name.GetString(Language.English, Gender.Male), Is.EqualTo("Area name"));
            Assert.That(are.Tag, Is.EqualTo("test_area_tag"));
            Assert.That(are.DefaultEnvMap.ToString(), Is.EqualTo("test_env"));
            Assert.That(are.DisableTransit, Is.True);
            Assert.That(are.Unescapable, Is.True);
            Assert.That(are.AlphaTest, Is.EqualTo(0.75f).Within(0.001f));
            Assert.That(are.StealthXp, Is.True);
            Assert.That(are.StealthXpMax, Is.EqualTo(125));
            Assert.That(are.StealthXpLoss, Is.EqualTo(7));
            Assert.That(are.NorthAxis, Is.EqualTo(ARENorthAxis.PositiveX));
            Assert.That(are.MapZoom, Is.EqualTo(3));
            Assert.That(are.MapResX, Is.EqualTo(512));
            Assert.That(are.OnEnter.ToString(), Is.EqualTo("k_area_enter"));
            Assert.That(are.OnExit.ToString(), Is.EqualTo("k_area_exit"));
            Assert.That(are.OnHeartbeat.ToString(), Is.EqualTo("k_area_hb"));
            Assert.That(are.OnUserDefined.ToString(), Is.EqualTo("k_area_ud"));
            Assert.That(are.Comment, Is.EqualTo("edited area"));
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.GIT, ResourceType.GIT);
                    var editor = new OdyToolGIT(null, null);
                    editor.Load("test.git", "test", ResourceType.GIT, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_InstanceVisibility_FiltersRendererAndList()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    editor.AddInstanceForTest(new GITCreature { ResRef = new ResRef("c_test"), Position = new System.Numerics.Vector3(1, 2, 0) });
                    editor.AddInstanceForTest(new GITDoor { ResRef = new ResRef("d_test"), Position = new System.Numerics.Vector3(3, 4, 0) });

                    Assert.That(editor.InstanceCount, Is.EqualTo(2));
                    Assert.That(editor.VisibleInstanceCount, Is.EqualTo(2));

                    editor.SetInstanceTypeVisibleForTest("Creature", false);

                    Assert.That(editor.VisibleInstanceCount, Is.EqualTo(1));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.FilterEdit, Is.Not.Null);
                    Assert.That(editor.InstanceList, Is.Not.Null);
                    Assert.That(editor.DetailResRef, Is.Not.Null);
                    Assert.That(editor.DetailPosX, Is.Not.Null);
                    Assert.That(editor.DetailPosY, Is.Not.Null);
                    Assert.That(editor.DetailPosZ, Is.Not.Null);
                    Assert.That(editor.DetailBearing, Is.Not.Null);
                    Assert.That(editor.DetailTag, Is.Not.Null);
                    Assert.That(editor.AddInstanceType, Is.Not.Null);
                    Assert.That(editor.AddInstanceButton, Is.Not.Null);
                    Assert.That(editor.DuplicateInstanceButton, Is.Not.Null);
                    Assert.That(editor.RemoveInstanceButton, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_AddNewInstances_SelectsAndBuildsEditableGitLists()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);

                    Assert.That(editor.AddInstanceType.ItemsSource, Is.EquivalentTo(new[]
                    {
                        "Creature", "Door", "Placeable", "Trigger", "Waypoint", "Sound", "Store", "Encounter", "Camera"
                    }));

                    var creature = editor.AddNewInstanceForTest("Creature");
                    var door = editor.AddNewInstanceForTest("Door");
                    var placeable = editor.AddNewInstanceForTest("Placeable");
                    var trigger = editor.AddNewInstanceForTest("Trigger");
                    var waypoint = editor.AddNewInstanceForTest("Waypoint");
                    var sound = editor.AddNewInstanceForTest("Sound");
                    var store = editor.AddNewInstanceForTest("Store");
                    var encounter = editor.AddNewInstanceForTest("Encounter");
                    var camera = editor.AddNewInstanceForTest("Camera");

                    Assert.That(creature, Is.TypeOf<GITCreature>());
                    Assert.That(door, Is.TypeOf<GITDoor>());
                    Assert.That(placeable, Is.TypeOf<GITPlaceable>());
                    Assert.That(trigger, Is.TypeOf<GITTrigger>());
                    Assert.That(waypoint, Is.TypeOf<GITWaypoint>());
                    Assert.That(sound, Is.TypeOf<GITSound>());
                    Assert.That(store, Is.TypeOf<GITStore>());
                    Assert.That(encounter, Is.TypeOf<GITEncounter>());
                    Assert.That(camera, Is.TypeOf<GITCamera>());
                    Assert.That(editor.SelectedInstanceForTest, Is.SameAs(camera));
                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(editor.DetailResRef.Text, Is.EqualTo("new_camera"));

                    var rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Creatures, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Doors, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Placeables, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Triggers, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Waypoints, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Sounds, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Stores, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Encounters, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Cameras, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Creatures[0].ResRef.ToString(), Is.EqualTo("new_creature"));
                    Assert.That(rebuilt.Doors[0].ResRef.ToString(), Is.EqualTo("new_door"));
                    Assert.That(rebuilt.Placeables[0].ResRef.ToString(), Is.EqualTo("new_placeable"));
                    Assert.That(rebuilt.Triggers[0].ResRef.ToString(), Is.EqualTo("new_trigger"));
                    Assert.That(rebuilt.Triggers[0].Geometry, Has.Count.EqualTo(3));
                    Assert.That(rebuilt.Waypoints[0].ResRef.ToString(), Is.EqualTo("new_waypoint"));
                    Assert.That(rebuilt.Sounds[0].ResRef.ToString(), Is.EqualTo("new_sound"));
                    Assert.That(rebuilt.Stores[0].ResRef.ToString(), Is.EqualTo("new_store"));
                    Assert.That(rebuilt.Encounters[0].ResRef.ToString(), Is.EqualTo("new_encounter"));
                    Assert.That(rebuilt.Encounters[0].Geometry, Has.Count.EqualTo(3));
                    Assert.That(rebuilt.Cameras[0].ResRef.ToString(), Is.EqualTo("new_camera"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_BlenderAction_DisabledUntilGitIsLoaded()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);

                    Assert.That(editor.OpenInBlenderMenuItem, Is.Not.Null);
                    Assert.That(editor.OpenInBlenderMenuItem.IsEnabled, Is.False);
                    Assert.That(editor.BlenderStatusText, Does.Contain("Open a GIT file"));

                    byte[] data = MinimalGffBytes(GFFContent.GIT, ResourceType.GIT);
                    editor.Load("test.git", "test", ResourceType.GIT, data);

                    Assert.That(editor.OpenInBlenderMenuItem.IsEnabled, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_BlenderAction_UsesLoadedGitPathAndReportsMissingAddon()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    byte[] data = MinimalGffBytes(GFFContent.GIT, ResourceType.GIT);
                    editor.Load("test.git", "test", ResourceType.GIT, data);

                    bool launched = false;
                    string launchedPath = null;
                    editor.SetBlenderServicesForTests(
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

                    Assert.That(editor.TryLaunchBlenderForCurrentGit(), Is.True);
                    Assert.That(launched, Is.True);
                    Assert.That(launchedPath, Is.EqualTo("test.git"));
                    Assert.That(editor.BlenderStatusText, Does.Contain("Launched Blender"));

                    editor.SetBlenderServicesForTests(
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

                    Assert.That(editor.TryLaunchBlenderForCurrentGit(), Is.False);
                    Assert.That(editor.BlenderStatusText, Does.Contain("kotorblender"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        [TestCase("Trigger")]
        [TestCase("Encounter")]
        [TestCase("Camera")]
        public async Task GITEditor_AddAdvancedInstanceType_BuildsEditableGitList(string typeName)
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    object instance = editor.AddNewInstanceForTest(typeName);

                    Assert.That(instance, Is.Not.Null);
                    Assert.That(editor.SelectedInstanceForTest, Is.SameAs(instance));

                    var rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    if (typeName == "Trigger")
                    {
                        Assert.That(rebuilt.Triggers, Has.Count.EqualTo(1));
                        Assert.That(rebuilt.Triggers[0].ResRef.ToString(), Is.EqualTo("new_trigger"));
                        Assert.That(rebuilt.Triggers[0].Geometry, Has.Count.EqualTo(3));
                    }
                    else if (typeName == "Encounter")
                    {
                        Assert.That(rebuilt.Encounters, Has.Count.EqualTo(1));
                        Assert.That(rebuilt.Encounters[0].ResRef.ToString(), Is.EqualTo("new_encounter"));
                        Assert.That(rebuilt.Encounters[0].Geometry, Has.Count.EqualTo(3));
                    }
                    else
                    {
                        Assert.That(rebuilt.Cameras, Has.Count.EqualTo(1));
                        Assert.That(rebuilt.Cameras[0].ResRef.ToString(), Is.EqualTo("new_camera"));
                    }
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public void GITHelpers_CameraTemplateResRef_Roundtrips()
        {
            var git = new GIT();
            git.Cameras.Add(new GITCamera
            {
                CameraId = 42,
                ResRef = new ResRef("cam_intro"),
                Fov = 60f,
                Orientation = new System.Numerics.Vector4(0, 0, 0, 1),
                Position = new System.Numerics.Vector3(1, 2, 3)
            });

            var rebuilt = GITHelpers.ConstructGit(GITHelpers.DismantleGit(git));

            Assert.That(rebuilt.Cameras, Has.Count.EqualTo(1));
            Assert.That(rebuilt.Cameras[0].CameraId, Is.EqualTo(42));
            Assert.That(rebuilt.Cameras[0].ResRef.ToString(), Is.EqualTo("cam_intro"));
            Assert.That(rebuilt.Cameras[0].Fov, Is.EqualTo(60f).Within(0.001f));
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_DetailEdits_BuildIntoGitInstance()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    var placeable = new GITPlaceable
                    {
                        ResRef = new ResRef("old_plc"),
                        Tag = "old_tag",
                        Position = new System.Numerics.Vector3(1, 2, 3),
                        Bearing = 0.25f
                    };
                    editor.AddInstanceForTest(placeable);
                    editor.SelectInstanceForTest(placeable);

                    editor.DetailResRef.Text = "new_plc";
                    editor.DetailTag.Text = "new_tag";
                    editor.DetailPosX.Value = 4.5m;
                    editor.DetailPosY.Value = -6.25m;
                    editor.DetailPosZ.Value = 7.75m;
                    editor.DetailBearing.Value = 1.125m;
                    editor.CommitSelectedInstanceDetailsForTest();

                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(placeable.ResRef.ToString(), Is.EqualTo("new_plc"));
                    Assert.That(placeable.Tag, Is.EqualTo("new_tag"));

                    var rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Placeables, Has.Count.EqualTo(1));
                    var saved = rebuilt.Placeables[0];
                    Assert.That(saved.ResRef.ToString(), Is.EqualTo("new_plc"));
                    Assert.That(saved.Tag, Is.EqualTo("new_tag"));
                    Assert.That(saved.Position.X, Is.EqualTo(4.5f).Within(0.001f));
                    Assert.That(saved.Position.Y, Is.EqualTo(-6.25f).Within(0.001f));
                    Assert.That(saved.Position.Z, Is.EqualTo(7.75f).Within(0.001f));
                    Assert.That(saved.Bearing, Is.EqualTo(1.125f).Within(0.001f));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_DetailResRef_TrimsWhitespaceAndClearsInvalidValue()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    var sound = new GITSound
                    {
                        ResRef = new ResRef("old_sound"),
                        Position = new System.Numerics.Vector3(1, 2, 3)
                    };
                    editor.AddInstanceForTest(sound);
                    editor.SelectInstanceForTest(sound);

                    editor.DetailResRef.Text = " new_sound ";
                    editor.CommitSelectedInstanceDetailsForTest();

                    Assert.That(sound.ResRef.ToString(), Is.EqualTo("new_sound"));
                    var rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Sounds[0].ResRef.ToString(), Is.EqualTo("new_sound"));

                    editor.DetailResRef.Text = "bad*sound";
                    editor.CommitSelectedInstanceDetailsForTest();

                    Assert.That(sound.ResRef.IsBlank(), Is.True);
                    rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Sounds[0].ResRef.IsBlank(), Is.True);
                    Assert.That(OdyToolGIT.ResRefFromEditableText(" more_than_16_chars ").IsBlank(), Is.True);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_MoveSelectedInstance_UpdatesModelAndDirtyState()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    var creature = new GITCreature { ResRef = new ResRef("c_test"), Position = new System.Numerics.Vector3(1, 2, 3) };
                    editor.AddInstanceForTest(creature);
                    editor.SelectInstanceForTest(creature);

                    editor.MoveSelectedInstanceForTest(5, -1);

                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(creature.Position.X, Is.EqualTo(6).Within(0.001));
                    Assert.That(creature.Position.Y, Is.EqualTo(1).Within(0.001));
                    Assert.That(creature.Position.Z, Is.EqualTo(3).Within(0.001));
                    Assert.That(editor.SelectedInstanceForTest, Is.SameAs(creature));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_DuplicateSelectedInstance_ClonesOffsetsSelectsAndBuilds()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    var placeable = new GITPlaceable
                    {
                        ResRef = new ResRef("plc_test"),
                        Tag = "plc_tag",
                        Position = new System.Numerics.Vector3(10, 20, 3),
                        Bearing = 0.75f
                    };
                    editor.AddInstanceForTest(placeable);
                    editor.SelectInstanceForTest(placeable);

                    editor.DuplicateSelectedInstanceForTest();

                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(editor.InstanceCount, Is.EqualTo(2));
                    Assert.That(editor.SelectedInstanceForTest, Is.Not.SameAs(placeable));
                    Assert.That(editor.SelectedInstanceForTest, Is.TypeOf<GITPlaceable>());

                    var clone = (GITPlaceable)editor.SelectedInstanceForTest;
                    Assert.That(clone.ResRef.ToString(), Is.EqualTo("plc_test"));
                    Assert.That(clone.Tag, Is.EqualTo("plc_tag"));
                    Assert.That(clone.Bearing, Is.EqualTo(0.75f).Within(0.001f));
                    Assert.That(clone.Position.X, Is.EqualTo(11f).Within(0.001f));
                    Assert.That(clone.Position.Y, Is.EqualTo(21f).Within(0.001f));
                    Assert.That(clone.Position.Z, Is.EqualTo(3f).Within(0.001f));

                    var rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Placeables, Has.Count.EqualTo(2));
                    Assert.That(rebuilt.Placeables.Select(p => p.ResRef.ToString()).ToArray(), Is.EqualTo(new[] { "plc_test", "plc_test" }));
                    Assert.That(rebuilt.Placeables[1].Position.X, Is.EqualTo(11f).Within(0.001f));
                    Assert.That(rebuilt.Placeables[1].Position.Y, Is.EqualTo(21f).Within(0.001f));
                    Assert.That(rebuilt.Placeables[1].Tag, Is.EqualTo("plc_tag"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_UndoRedo_RevertsAndRestoresDuplicateSelectedInstance()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    var creature = new GITCreature
                    {
                        ResRef = new ResRef("c_test"),
                        Position = new System.Numerics.Vector3(2, 4, 0),
                        Bearing = 0.5f
                    };
                    editor.AddInstanceForTest(creature);
                    editor.SelectInstanceForTest(creature);

                    editor.DuplicateSelectedInstanceForTest();
                    Assert.That(editor.InstanceCount, Is.EqualTo(2));

                    editor.UndoGitEditForTest();
                    Assert.That(editor.InstanceCount, Is.EqualTo(1));
                    Assert.That(editor.SelectedInstanceForTest, Is.Null);

                    editor.RedoGitEditForTest();
                    Assert.That(editor.InstanceCount, Is.EqualTo(2));

                    var rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Creatures, Has.Count.EqualTo(2));
                    Assert.That(rebuilt.Creatures[1].Position.X, Is.EqualTo(3f).Within(0.001f));
                    Assert.That(rebuilt.Creatures[1].Position.Y, Is.EqualTo(5f).Within(0.001f));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task GITEditor_Undo_RevertsDetailEdit()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolGIT(null, null);
                    var sound = new GITSound
                    {
                        ResRef = new ResRef("old_sound"),
                        Tag = "old_tag",
                        Position = new System.Numerics.Vector3(1, 2, 3)
                    };
                    editor.AddInstanceForTest(sound);
                    editor.SelectInstanceForTest(sound);

                    editor.DetailResRef.Text = "new_sound";
                    editor.DetailTag.Text = "new_tag";
                    editor.DetailPosX.Value = 9m;
                    editor.CommitSelectedInstanceDetailsForTest();

                    Assert.That(sound.ResRef.ToString(), Is.EqualTo("new_sound"));

                    editor.UndoGitEditForTest();

                    var rebuilt = GITHelpers.ConstructGit(GFF.FromBytes(editor.Build().Item1));
                    Assert.That(rebuilt.Sounds, Has.Count.EqualTo(1));
                    Assert.That(rebuilt.Sounds[0].ResRef.ToString(), Is.EqualTo("old_sound"));
                    Assert.That(rebuilt.Sounds[0].Tag, Is.EqualTo("old_tag"));
                    Assert.That(rebuilt.Sounds[0].Position.X, Is.EqualTo(1f).Within(0.001f));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(120000)]
        public async Task IFOEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.IFO, ResourceType.IFO);
                    var editor = new OdyToolIFO(null, null);
                    editor.Load("module.ifo", "module", ResourceType.IFO, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(180000)]
        public async Task IFOEditor_UsesStructuredEditableSurface()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolIFO(null, null);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    Assert.That(editor.NameEditForTest, Is.Not.Null);
                    Assert.That(editor.DescEditForTest, Is.Not.Null);
                    Assert.That(editor.TagEditForTest, Is.Not.Null);
                    Assert.That(editor.TagGenerateButtonForTest, Is.Not.Null);
                    Assert.That(editor.EntryResrefEditForTest, Is.Not.Null);
                    Assert.That(editor.EntryXSpinForTest, Is.Not.Null);
                    Assert.That(editor.DawnHourSpinForTest, Is.Not.Null);
                    Assert.That(editor.ScriptFields, Does.ContainKey("on_heartbeat"));
                    Assert.That(editor.ScriptFields, Does.ContainKey("on_enter"));
                    Assert.That(editor.ScriptFields, Does.ContainKey("start_movie"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(180000)]
        public async Task IFOEditor_GenerateTag_UsesModuleNameThenResref()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string generatedFromName = null;
                string generatedFromResref = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolIFO(null, null);
                    editor.NameEditForTest.SetLocString(LocalizedString.FromEnglish("Ebon Hawk Interior"));
                    editor.TagGenerateButtonForTest.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    generatedFromName = editor.TagEditForTest.Text;

                    editor.Load("fallback.ifo", "M12aa", ResourceType.IFO, MinimalGffBytes(GFFContent.IFO, ResourceType.IFO));
                    editor.NameEditForTest.SetLocString(LocalizedString.FromInvalid());
                    editor.TagGenerateButtonForTest.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.Button.ClickEvent));
                    generatedFromResref = editor.TagEditForTest.Text;
                }, CancellationToken.None);

                Assert.That(generatedFromName, Is.EqualTo("ebon_hawk_interior"));
                Assert.That(generatedFromResref, Is.EqualTo("m12aa"));
            }
        }

        [Test, Timeout(180000)]
        public async Task IFOEditor_EntryDirection_DisplaysDegreesAndBuildsRadians()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                decimal displayedDegrees = 0;
                float builtRadians = 0;

                await session.Dispatch(() =>
                {
                    var source = new IFO
                    {
                        EntryDirection = (float)(System.Math.PI / 2.0)
                    };
                    byte[] data = GFFAuto.BytesGff(IFOHelpers.DismantleIfo(source), ResourceType.IFO);
                    var editor = new OdyToolIFO(null, null);
                    editor.Load("module.ifo", "module", ResourceType.IFO, data);

                    displayedDegrees = editor.EntryDirSpinForTest.Value ?? 0;
                    editor.EntryDirSpinForTest.Value = 180;

                    var built = IFOHelpers.ConstructIfo(GFF.FromBytes(editor.Build().Item1));
                    builtRadians = built.EntryDirection;
                }, CancellationToken.None);

                Assert.That(displayedDegrees, Is.EqualTo(90m).Within(0.01m));
                Assert.That(System.Math.Abs(builtRadians), Is.EqualTo((float)System.Math.PI).Within(0.001f));
            }
        }

        [Test, Timeout(180000)]
        public async Task IFOEditor_FieldEdits_BuildIntoIfo()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string name = null;
                string description = null;
                string tag = null;
                string voId = null;
                string hak = null;
                string entryArea = null;
                float entryX = 0;
                float entryY = 0;
                float entryZ = 0;
                float entryDirection = 0;
                int dawnHour = 0;
                int duskHour = 0;
                int timeScale = 0;
                int startMonth = 0;
                int startDay = 0;
                int startHour = 0;
                int startYear = 0;
                int xpScale = 0;
                string onHeartbeat = null;
                string onEnter = null;
                string startMovie = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolIFO(null, null);
                    editor.NameEditForTest.SetLocString(LocalizedString.FromEnglish("Module name"));
                    editor.DescEditForTest.SetLocString(LocalizedString.FromEnglish("Module description"));
                    editor.TagEditForTest.Text = "test_mod_tag";
                    editor.VoIdEditForTest.Text = "test_vo";
                    editor.HakEditForTest.Text = "test_hak";
                    editor.EntryResrefEditForTest.Text = "testarea";
                    editor.EntryXSpinForTest.Value = 1.5m;
                    editor.EntryYSpinForTest.Value = -2.5m;
                    editor.EntryZSpinForTest.Value = 3.25m;
                    editor.EntryDirSpinForTest.Value = 90m;
                    editor.DawnHourSpinForTest.Value = 6;
                    editor.DuskHourSpinForTest.Value = 18;
                    editor.TimeScaleSpinForTest.Value = 30;
                    editor.StartMonthSpinForTest.Value = 3;
                    editor.StartDaySpinForTest.Value = 14;
                    editor.StartHourSpinForTest.Value = 9;
                    editor.StartYearSpinForTest.Value = 3956;
                    editor.XpScaleSpinForTest.Value = 12;
                    editor.ScriptFields["on_heartbeat"].Text = "k_mod_hb";
                    editor.ScriptFields["on_enter"].Text = "k_mod_enter";
                    editor.ScriptFields["start_movie"].Text = "intro_mov";

                    byte[] built = editor.Build().Item1;
                    var ifo = IFOHelpers.ConstructIfo(GFF.FromBytes(built));

                    name = ifo.ModName.GetString(Language.English, Gender.Male);
                    description = ifo.Description.GetString(Language.English, Gender.Male);
                    tag = ifo.Tag;
                    voId = ifo.VoId;
                    hak = ifo.Hak;
                    entryArea = ifo.ResRef.ToString();
                    entryX = ifo.EntryX;
                    entryY = ifo.EntryY;
                    entryZ = ifo.EntryZ;
                    entryDirection = ifo.EntryDirection;
                    dawnHour = ifo.DawnHour;
                    duskHour = ifo.DuskHour;
                    timeScale = ifo.TimeScale;
                    startMonth = ifo.StartMonth;
                    startDay = ifo.StartDay;
                    startHour = ifo.StartHour;
                    startYear = ifo.StartYear;
                    xpScale = ifo.XpScale;
                    onHeartbeat = ifo.OnHeartbeat.ToString();
                    onEnter = ifo.OnClientEnter.ToString();
                    startMovie = ifo.StartMovie.ToString();
                }, CancellationToken.None);

                Assert.That(name, Is.EqualTo("Module name"));
                Assert.That(description, Is.EqualTo("Module description"));
                Assert.That(tag, Is.EqualTo("test_mod_tag"));
                Assert.That(voId, Is.EqualTo("test_vo"));
                Assert.That(hak, Is.EqualTo("test_hak"));
                Assert.That(entryArea, Is.EqualTo("testarea"));
                Assert.That(entryX, Is.EqualTo(1.5f).Within(0.001f));
                Assert.That(entryY, Is.EqualTo(-2.5f).Within(0.001f));
                Assert.That(entryZ, Is.EqualTo(3.25f).Within(0.001f));
                Assert.That(entryDirection, Is.EqualTo((float)(System.Math.PI / 2.0)).Within(0.001f));
                Assert.That(dawnHour, Is.EqualTo(6));
                Assert.That(duskHour, Is.EqualTo(18));
                Assert.That(timeScale, Is.EqualTo(30));
                Assert.That(startMonth, Is.EqualTo(3));
                Assert.That(startDay, Is.EqualTo(14));
                Assert.That(startHour, Is.EqualTo(9));
                Assert.That(startYear, Is.EqualTo(3956));
                Assert.That(xpScale, Is.EqualTo(12));
                Assert.That(onHeartbeat, Is.EqualTo("k_mod_hb"));
                Assert.That(onEnter, Is.EqualTo("k_mod_enter"));
                Assert.That(startMovie, Is.EqualTo("intro_mov"));
            }
        }

        [Test, Timeout(180000)]
        public async Task IFOEditor_ResRefFields_TrimWhitespaceAndClearBlankValues()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new IFO
                    {
                        ResRef = new ResRef("old_area"),
                        EntryArea = new ResRef("old_area"),
                        OnHeartbeat = new ResRef("old_hb"),
                        OnLoad = new ResRef("old_load"),
                        OnClientEnter = new ResRef("old_enter"),
                        OnPlayerRest = new ResRef("old_rest"),
                        StartMovie = new ResRef("old_movie")
                    };
                    byte[] data = GFFAuto.BytesGff(IFOHelpers.DismantleIfo(source), ResourceType.IFO);

                    var editor = new OdyToolIFO(null, null);
                    editor.Load("module.ifo", "module", ResourceType.IFO, data);
                    editor.EntryResrefEditForTest.Text = "  new_area  ";
                    editor.ScriptFields["on_heartbeat"].Text = "  k_hb  ";
                    editor.ScriptFields["on_load"].Text = "   ";
                    editor.ScriptFields["on_enter"].Text = "k_enter  ";
                    editor.ScriptFields["on_player_rest"].Text = "";
                    editor.ScriptFields["start_movie"].Text = "  intro_mov";

                    var rebuilt = IFOHelpers.ConstructIfo(GFF.FromBytes(editor.Build().Item1));

                    Assert.That(rebuilt.ResRef.ToString(), Is.EqualTo("new_area"));
                    Assert.That(rebuilt.EntryArea.ToString(), Is.EqualTo("new_area"));
                    Assert.That(rebuilt.OnHeartbeat.ToString(), Is.EqualTo("k_hb"));
                    Assert.That(rebuilt.OnLoad.ToString(), Is.EqualTo(""));
                    Assert.That(rebuilt.OnClientEnter.ToString(), Is.EqualTo("k_enter"));
                    Assert.That(rebuilt.OnPlayerRest.ToString(), Is.EqualTo(""));
                    Assert.That(rebuilt.StartMovie.ToString(), Is.EqualTo("intro_mov"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(180000)]
        public async Task IFOEditor_FindNext_AdvancesAcrossFieldsAndWrapsLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string[] hits = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolIFO(null, null);
                    editor.TagEditForTest.Text = "match_tag";
                    editor.VoIdEditForTest.Text = "match_vo";
                    editor.HakEditForTest.Text = "other_hak";
                    editor.EntryResrefEditForTest.Text = "match_area";
                    editor.ScriptFields["on_enter"].Text = "match_enter";

                    editor.SetFindQueryForTest("match");
                    var found = new List<string>();
                    for (int i = 0; i < 5; i++)
                    {
                        Assert.That(editor.FindNextForTest(), Is.True);
                        found.Add(editor.LastFindFieldKeyForTest);
                    }

                    hits = found.ToArray();
                }, CancellationToken.None);

                Assert.That(hits, Is.EqualTo(new[]
                {
                    "tag",
                    "vo_id",
                    "entry_resref",
                    "on_enter",
                    "tag"
                }));
            }
        }

        [Test, Timeout(180000)]
        public async Task IFOEditor_FindNext_MatchCaseSkipsCaseMismatch()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                bool lowerCaseMatched = true;
                bool exactCaseMatched = false;
                string exactField = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolIFO(null, null);
                    editor.TagEditForTest.Text = "Match_Tag";

                    editor.SetFindQueryForTest("match", matchCase: true);
                    lowerCaseMatched = editor.FindNextForTest();

                    editor.SetFindQueryForTest("Match", matchCase: true);
                    exactCaseMatched = editor.FindNextForTest();
                    exactField = editor.LastFindFieldKeyForTest;
                }, CancellationToken.None);

                Assert.That(lowerCaseMatched, Is.False);
                Assert.That(exactCaseMatched, Is.True);
                Assert.That(exactField, Is.EqualTo("tag"));
            }
        }

        [Test, Timeout(60000)]
        public void IFOHelpers_DismantleConstruct_PreservesModuleFields()
        {
            var source = new IFO
            {
                ModName = LocalizedString.FromEnglish("Module name"),
                Description = LocalizedString.FromEnglish("Module description"),
                Tag = "test_mod_tag",
                VoId = "test_vo",
                Hak = "test_hak",
                ResRef = new ResRef("testarea"),
                EntryArea = new ResRef("testarea"),
                EntryX = 1.5f,
                EntryY = -2.5f,
                EntryZ = 3.25f,
                EntryDirection = 1.5708f,
                DawnHour = 6,
                DuskHour = 18,
                TimeScale = 30,
                StartMonth = 3,
                StartDay = 14,
                StartHour = 9,
                StartYear = 3956,
                XpScale = 12,
                OnHeartbeat = new ResRef("k_mod_hb"),
                OnClientEnter = new ResRef("k_mod_enter"),
                StartMovie = new ResRef("intro_mov")
            };

            var ifo = IFOHelpers.ConstructIfo(IFOHelpers.DismantleIfo(source));

            Assert.That(ifo.ModName.GetString(Language.English, Gender.Male), Is.EqualTo("Module name"));
            Assert.That(ifo.Description.GetString(Language.English, Gender.Male), Is.EqualTo("Module description"));
            Assert.That(ifo.Tag, Is.EqualTo("test_mod_tag"));
            Assert.That(ifo.VoId, Is.EqualTo("test_vo"));
            Assert.That(ifo.Hak, Is.EqualTo("test_hak"));
            Assert.That(ifo.ResRef.ToString(), Is.EqualTo("testarea"));
            Assert.That(ifo.EntryX, Is.EqualTo(1.5f).Within(0.001f));
            Assert.That(ifo.EntryY, Is.EqualTo(-2.5f).Within(0.001f));
            Assert.That(ifo.EntryZ, Is.EqualTo(3.25f).Within(0.001f));
            Assert.That(ifo.EntryDirection, Is.EqualTo(1.5708f).Within(0.001f));
            Assert.That(ifo.DawnHour, Is.EqualTo(6));
            Assert.That(ifo.DuskHour, Is.EqualTo(18));
            Assert.That(ifo.TimeScale, Is.EqualTo(30));
            Assert.That(ifo.StartMonth, Is.EqualTo(3));
            Assert.That(ifo.StartDay, Is.EqualTo(14));
            Assert.That(ifo.StartHour, Is.EqualTo(9));
            Assert.That(ifo.StartYear, Is.EqualTo(3956));
            Assert.That(ifo.XpScale, Is.EqualTo(12));
            Assert.That(ifo.OnHeartbeat.ToString(), Is.EqualTo("k_mod_hb"));
            Assert.That(ifo.OnClientEnter.ToString(), Is.EqualTo("k_mod_enter"));
            Assert.That(ifo.StartMovie.ToString(), Is.EqualTo("intro_mov"));
        }

        [Test, Timeout(60000)]
        public async Task JRLEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.JRL, ResourceType.JRL);
                    var editor = new OdyToolJRL(null, null);
                    editor.Load("test.jrl", "test", ResourceType.JRL, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(120000)]
        public async Task JRLEditor_FieldEdits_BuildIntoJrl()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string questName = null;
                int planetId = 0;
                int plotIndex = 0;
                JRLQuestPriority priority = JRLQuestPriority.Lowest;
                string tag = null;
                string comment = null;
                string entryText = null;
                int entryId = 0;
                bool entryEnd = false;
                float xpPct = 0;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolJRL(null, null);
                    editor.AddQuestForTest(
                        LocalizedString.FromEnglish("Quest name"),
                        planetId: 4,
                        plotIndex: 7,
                        priority: JRLQuestPriority.High,
                        tag: "quest_tag",
                        comment: "quest comment");
                    editor.AddEntryForTest(
                        questIndex: 0,
                        text: LocalizedString.FromEnglish("Entry text"),
                        entryId: 42,
                        end: true,
                        xpPercentage: 12.5f);

                    byte[] built = editor.Build().Item1;
                    var jrl = JRLHelpers.ConstructJrl(GFF.FromBytes(built));
                    var quest = jrl.Quests[0];
                    var entry = quest.Entries[0];

                    questName = quest.Name.GetString(Language.English, Gender.Male);
                    planetId = quest.PlanetId;
                    plotIndex = quest.PlotIndex;
                    priority = quest.Priority;
                    tag = quest.Tag;
                    comment = quest.Comment;
                    entryText = entry.Text.GetString(Language.English, Gender.Male);
                    entryId = entry.EntryId;
                    entryEnd = entry.End;
                    xpPct = entry.XpPercentage;
                }, CancellationToken.None);

                Assert.That(questName, Is.EqualTo("Quest name"));
                Assert.That(planetId, Is.EqualTo(4));
                Assert.That(plotIndex, Is.EqualTo(7));
                Assert.That(priority, Is.EqualTo(JRLQuestPriority.High));
                Assert.That(tag, Is.EqualTo("quest_tag"));
                Assert.That(comment, Is.EqualTo("quest comment"));
                Assert.That(entryText, Is.EqualTo("Entry text"));
                Assert.That(entryId, Is.EqualTo(42));
                Assert.That(entryEnd, Is.True);
                Assert.That(xpPct, Is.EqualTo(12.5f).Within(0.001f));
            }
        }

        [Test, Timeout(120000)]
        public async Task JRLEditor_LoadedJournalEdits_DoNotRestoreOriginalCategories()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                JRL rebuilt = null;
                string preservedCategoryMetadata = null;
                string preservedEntryMetadata = null;

                await session.Dispatch(() =>
                {
                    var source = new JRL();
                    source.Quests.Add(new JRLQuest
                    {
                        Name = LocalizedString.FromEnglish("Original Quest"),
                        PlanetId = 1,
                        PlotIndex = 2,
                        Priority = JRLQuestPriority.Low,
                        Tag = "original",
                        Comment = "original comment",
                        Entries =
                        {
                            new JRLQuestEntry
                            {
                                Text = LocalizedString.FromEnglish("Original entry"),
                                EntryId = 10,
                                End = false,
                                XpPercentage = 0
                            }
                        }
                    });

                    var originalGff = JRLHelpers.DismantleJrl(source);
                    var originalCategory = originalGff.Root.GetList("Categories").At(0);
                    originalCategory.SetString("XCategory", "keep category metadata");
                    originalCategory.GetList("EntryList").At(0).SetString("XEntry", "keep entry metadata");

                    var editor = new OdyToolJRL(null, null);
                    editor.Load("loaded.jrl", "loaded", ResourceType.JRL, GFFAuto.BytesGff(originalGff, ResourceType.JRL));
                    editor.SelectQuestForTest(0);
                    editor.QuestTagControlForTest.Text = "edited";
                    editor.QuestCommentControlForTest.Text = "edited comment";
                    editor.QuestPlanetIdControlForTest.Value = 12;
                    editor.QuestPlotIndexControlForTest.Value = 34;
                    editor.QuestPriorityControlForTest.SelectedItem = JRLQuestPriority.Highest;

                    editor.SelectEntryForTest(0, 0);
                    editor.EntryIdControlForTest.Value = 99;
                    editor.EntryEndControlForTest.IsChecked = true;
                    editor.EntryXpPctControlForTest.Value = 75;

                    byte[] built = editor.Build().Item1;
                    var builtGff = GFF.FromBytes(built);
                    rebuilt = JRLHelpers.ConstructJrl(builtGff);

                    var builtCategory = builtGff.Root.GetList("Categories").At(0);
                    preservedCategoryMetadata = builtCategory.GetString("XCategory");
                    preservedEntryMetadata = builtCategory.GetList("EntryList").At(0).GetString("XEntry");
                }, CancellationToken.None);

                Assert.That(rebuilt.Quests.Count, Is.EqualTo(1));
                Assert.That(rebuilt.Quests[0].Tag, Is.EqualTo("edited"));
                Assert.That(rebuilt.Quests[0].Comment, Is.EqualTo("edited comment"));
                Assert.That(rebuilt.Quests[0].PlanetId, Is.EqualTo(12));
                Assert.That(rebuilt.Quests[0].PlotIndex, Is.EqualTo(34));
                Assert.That(rebuilt.Quests[0].Priority, Is.EqualTo(JRLQuestPriority.Highest));
                Assert.That(rebuilt.Quests[0].Entries.Count, Is.EqualTo(1));
                Assert.That(rebuilt.Quests[0].Entries[0].EntryId, Is.EqualTo(99));
                Assert.That(rebuilt.Quests[0].Entries[0].End, Is.True);
                Assert.That(rebuilt.Quests[0].Entries[0].XpPercentage, Is.EqualTo(75).Within(0.001f));
                Assert.That(preservedCategoryMetadata, Is.EqualTo("keep category metadata"));
                Assert.That(preservedEntryMetadata, Is.EqualTo("keep entry metadata"));
            }
        }

        [Test, Timeout(60000)]
        public void JRLHelpers_DismantleConstruct_PreservesQuestAndEntryFields()
        {
            var source = new JRL();
            source.Quests.Add(new JRLQuest
            {
                Name = LocalizedString.FromEnglish("Quest name"),
                PlanetId = 4,
                PlotIndex = 7,
                Priority = JRLQuestPriority.High,
                Tag = "quest_tag",
                Comment = "quest comment",
                Entries =
                {
                    new JRLQuestEntry
                    {
                        Text = LocalizedString.FromEnglish("Entry text"),
                        EntryId = 42,
                        End = true,
                        XpPercentage = 12.5f
                    }
                }
            });

            var jrl = JRLHelpers.ConstructJrl(JRLHelpers.DismantleJrl(source));
            var quest = jrl.Quests[0];
            var entry = quest.Entries[0];

            Assert.That(quest.Name.GetString(Language.English, Gender.Male), Is.EqualTo("Quest name"));
            Assert.That(quest.PlanetId, Is.EqualTo(4));
            Assert.That(quest.PlotIndex, Is.EqualTo(7));
            Assert.That(quest.Priority, Is.EqualTo(JRLQuestPriority.High));
            Assert.That(quest.Tag, Is.EqualTo("quest_tag"));
            Assert.That(quest.Comment, Is.EqualTo("quest comment"));
            Assert.That(entry.Text.GetString(Language.English, Gender.Male), Is.EqualTo("Entry text"));
            Assert.That(entry.EntryId, Is.EqualTo(42));
            Assert.That(entry.End, Is.True);
            Assert.That(entry.XpPercentage, Is.EqualTo(12.5f).Within(0.001f));
        }

        [Test, Timeout(60000)]
        public void JRLEditorSettings_PersistHolocronFilterAndJumpOptions()
        {
            var settings = new JRLEditorSettings();
            string originalFilterMode = settings.FilterMode;
            bool originalJumpAutoOpen = settings.JumpAutoOpen;

            try
            {
                settings.FilterMode = JRLEditorSettings.FilterModeAllLevels;
                settings.JumpAutoOpen = false;

                var reloaded = new JRLEditorSettings();

                Assert.That(reloaded.FilterMode, Is.EqualTo(JRLEditorSettings.FilterModeAllLevels));
                Assert.That(reloaded.JumpAutoOpen, Is.False);

                reloaded.FilterMode = "invalid";
                Assert.That(new JRLEditorSettings().FilterMode, Is.EqualTo(JRLEditorSettings.FilterModeSmart));
            }
            finally
            {
                settings.FilterMode = originalFilterMode;
                settings.JumpAutoOpen = originalJumpAutoOpen;
            }
        }

        [Test, Timeout(120000)]
        public async Task JRLEditor_SearchFilterModeMatchesHolocronSettings()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string[] smartMatches = null;
                string[] questOnlyMatches = null;
                string[] allLevelMatches = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolJRL(null, null);
                    editor.AddQuestForTest(
                        LocalizedString.FromEnglish("Quest Alpha"),
                        planetId: 0,
                        plotIndex: 0,
                        priority: JRLQuestPriority.Lowest,
                        tag: "quest_alpha",
                        comment: "");
                    editor.AddEntryForTest(
                        questIndex: 0,
                        text: LocalizedString.FromEnglish("Entry Target"),
                        entryId: 1,
                        end: false,
                        xpPercentage: 0);

                    editor.SettingsForTest.FilterMode = JRLEditorSettings.FilterModeSmart;
                    smartMatches = editor.FindMatchTextsForTest("target");

                    editor.SettingsForTest.FilterMode = JRLEditorSettings.FilterModeQuestOnly;
                    questOnlyMatches = editor.FindMatchTextsForTest("target");

                    editor.SettingsForTest.FilterMode = JRLEditorSettings.FilterModeAllLevels;
                    allLevelMatches = editor.FindMatchTextsForTest("target");
                }, CancellationToken.None);

                Assert.That(smartMatches, Is.EqualTo(new[] { "Quest Alpha" }));
                Assert.That(questOnlyMatches, Is.Empty);
                Assert.That(allLevelMatches, Is.EqualTo(new[] { "[1] Entry Target" }));
            }
        }

        [Test, Timeout(120000)]
        public async Task JRLEditor_FilterBarUsesHolocronSettingsModes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string[] smartVisible = null;
                string[] questOnlyVisible = null;
                string[] allLevelsVisible = null;
                string[] tagVisible = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolJRL(null, null);
                    editor.AddQuestForTest(
                        LocalizedString.FromEnglish("Quest Alpha"),
                        planetId: 0,
                        plotIndex: 0,
                        priority: JRLQuestPriority.Lowest,
                        tag: "quest_alpha_tag",
                        comment: "");
                    editor.AddEntryForTest(
                        questIndex: 0,
                        text: LocalizedString.FromEnglish("Entry Target"),
                        entryId: 12,
                        end: false,
                        xpPercentage: 0);

                    editor.SettingsForTest.FilterMode = JRLEditorSettings.FilterModeSmart;
                    smartVisible = editor.VisibleTreeTextsForTest("target");

                    editor.SettingsForTest.FilterMode = JRLEditorSettings.FilterModeQuestOnly;
                    questOnlyVisible = editor.VisibleTreeTextsForTest("target");

                    editor.SettingsForTest.FilterMode = JRLEditorSettings.FilterModeAllLevels;
                    allLevelsVisible = editor.VisibleTreeTextsForTest("target");

                    editor.SettingsForTest.FilterMode = JRLEditorSettings.FilterModeQuestOnly;
                    tagVisible = editor.VisibleTreeTextsForTest("alpha_tag");
                }, CancellationToken.None);

                Assert.That(smartVisible, Is.EqualTo(new[] { "Quest Alpha", "[12] Entry Target" }));
                Assert.That(questOnlyVisible, Is.Empty);
                Assert.That(allLevelsVisible, Is.EqualTo(new[] { "Quest Alpha", "[12] Entry Target" }));
                Assert.That(tagVisible, Is.EqualTo(new[] { "Quest Alpha", "[12] Entry Target" }));
            }
        }

        [Test, Timeout(120000)]
        public async Task JRLEditor_DuplicateMoveAndSortMatchHolocronManipulation()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                JRL rebuilt = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolJRL(null, null);
                    editor.AddQuestForTest(
                        LocalizedString.FromEnglish("Quest Alpha"),
                        planetId: 0,
                        plotIndex: 0,
                        priority: JRLQuestPriority.Lowest,
                        tag: "alpha",
                        comment: "alpha comment");
                    editor.AddEntryForTest(
                        questIndex: 0,
                        text: LocalizedString.FromEnglish("Entry two"),
                        entryId: 2,
                        end: false,
                        xpPercentage: 0);
                    editor.AddEntryForTest(
                        questIndex: 0,
                        text: LocalizedString.FromEnglish("Entry one"),
                        entryId: 1,
                        end: false,
                        xpPercentage: 0);
                    editor.AddQuestForTest(
                        LocalizedString.FromEnglish("Quest Beta"),
                        planetId: 0,
                        plotIndex: 0,
                        priority: JRLQuestPriority.Lowest,
                        tag: "beta",
                        comment: "");

                    editor.SelectQuestForTest(0);
                    editor.RunDuplicateSelectedForTest();
                    editor.RunMoveSelectedForTest(1);

                    editor.SelectQuestForTest(0);
                    editor.RunSortSelectedQuestEntriesForTest(true);

                    editor.SelectEntryForTest(0, 0);
                    editor.RunDuplicateSelectedForTest();

                    rebuilt = JRLHelpers.ConstructJrl(GFF.FromBytes(editor.Build().Item1));
                }, CancellationToken.None);

                Assert.That(rebuilt.Quests.Count, Is.EqualTo(3));
                Assert.That(rebuilt.Quests[0].Tag, Is.EqualTo("alpha"));
                Assert.That(rebuilt.Quests[1].Tag, Is.EqualTo("beta"));
                Assert.That(rebuilt.Quests[2].Tag, Is.EqualTo("alpha_copy"));
                Assert.That(rebuilt.Quests[0].Entries.Count, Is.EqualTo(3));
                Assert.That(rebuilt.Quests[0].Entries[0].EntryId, Is.EqualTo(1));
                Assert.That(rebuilt.Quests[0].Entries[1].EntryId, Is.EqualTo(2));
                Assert.That(rebuilt.Quests[0].Entries[2].EntryId, Is.EqualTo(2), "Duplicated entry increments the original ID and is inserted after it.");
                Assert.That(rebuilt.Quests[0].Entries[1].Text.GetString(Language.English, Gender.Male), Is.EqualTo("Entry one"));
                Assert.That(rebuilt.Quests[0].Entries[2].Text.GetString(Language.English, Gender.Male), Is.EqualTo("Entry two"));
            }
        }

        [Test, Timeout(120000)]
        public async Task JRLEditor_TreeContextMenuExposesHolocronManipulationActions()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                string[] menuHeaders = null;
                JRL rebuilt = null;

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolJRL(null, null);
                    editor.AddQuestForTest(
                        LocalizedString.FromEnglish("Quest Alpha"),
                        planetId: 0,
                        plotIndex: 0,
                        priority: JRLQuestPriority.Lowest,
                        tag: "alpha",
                        comment: "");
                    editor.SelectQuestForTest(0);

                    var contextMenu = editor.JournalTreeContextMenuForTest;
                    Assert.That(contextMenu, Is.Not.Null);

                    var headers = new System.Collections.Generic.List<string>();
                    Avalonia.Controls.MenuItem duplicateItem = null;
                    foreach (object item in contextMenu.Items)
                    {
                        var menuItem = item as Avalonia.Controls.MenuItem;
                        if (menuItem == null)
                        {
                            continue;
                        }

                        headers.Add(menuItem.Header?.ToString());
                        if (menuItem.Name == "ctxDuplicate")
                        {
                            duplicateItem = menuItem;
                        }
                    }

                    menuHeaders = headers.ToArray();
                    Assert.That(duplicateItem, Is.Not.Null);
                    duplicateItem.RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(Avalonia.Controls.MenuItem.ClickEvent));

                    rebuilt = JRLHelpers.ConstructJrl(GFF.FromBytes(editor.Build().Item1));
                }, CancellationToken.None);

                Assert.That(menuHeaders, Does.Contain("Add Quest"));
                Assert.That(menuHeaders, Does.Contain("Add Entry"));
                Assert.That(menuHeaders, Does.Contain("Duplicate"));
                Assert.That(menuHeaders, Does.Contain("Move Up"));
                Assert.That(menuHeaders, Does.Contain("Move Down"));
                Assert.That(menuHeaders, Does.Contain("Sort Entries by ID Ascending"));
                Assert.That(menuHeaders, Does.Contain("Sort Entries by ID Descending"));
                Assert.That(menuHeaders, Does.Contain("Remove"));
                Assert.That(rebuilt.Quests.Count, Is.EqualTo(2));
                Assert.That(rebuilt.Quests[1].Tag, Is.EqualTo("alpha_copy"));
            }
        }

        [Test, Timeout(120000)]
        public async Task JRLEditor_DeleteKeyRemovesSelectedJournalItemLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolJRL(null, null);
                    editor.AddQuestForTest(
                        LocalizedString.FromEnglish("Quest Alpha"),
                        planetId: 0,
                        plotIndex: 0,
                        priority: JRLQuestPriority.Lowest,
                        tag: "alpha",
                        comment: "");
                    editor.AddEntryForTest(
                        questIndex: 0,
                        text: LocalizedString.FromEnglish("Entry one"),
                        entryId: 1,
                        end: false,
                        xpPercentage: 0);

                    editor.SelectEntryForTest(0, 0);
                    editor.RaiseEvent(CreateKeyEventArgs(Key.Delete));

                    Assert.That(editor.EntryCountForTest(0), Is.EqualTo(0));
                    Assert.That(editor.IsDirty, Is.True);

                    editor.SelectQuestForTest(0);
                    editor.RaiseEvent(CreateKeyEventArgs(Key.Delete));

                    Assert.That(editor.ModelRowCount, Is.EqualTo(0));
                    Assert.That(JRLHelpers.ConstructJrl(GFF.FromBytes(editor.Build().Item1)).Quests, Is.Empty);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task PTHEditor_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalGffBytes(GFFContent.PTH, ResourceType.PTH);
                    var editor = new OdyToolPTH(null, null);
                    editor.Load("test.pth", "test", ResourceType.PTH, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF gff = GFF.FromBytes(built);
                    Assert.That(gff.Root, Is.Not.Null);
                    Assert.That(gff.Root.Count, Is.GreaterThanOrEqualTo(0));
                    Assert.That(editor, Is.Not.Null);
                    Assert.That(result.Item2, Is.Not.Null);
                    Assert.That(built.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task PTHEditor_SelectedCanvasNodes_CanAddAndRemoveEdge()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolPTH(null, null);
                    editor.AddNode(0, 0);
                    editor.AddNode(10, 0);

                    editor.SelectNodeIndicesForTest(0, 1);
                    editor.AddEdgeBetweenSelectedForTest();

                    Assert.That(editor.ConnectionCount, Is.EqualTo(2));
                    Assert.That(editor.Pth().IsConnected(0, 1), Is.True);
                    Assert.That(editor.Pth().IsConnected(1, 0), Is.True);

                    editor.RemoveEdgeBetweenSelectedForTest();

                    Assert.That(editor.ConnectionCount, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task PTHEditor_MoveSelected_RefreshesSelectionAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolPTH(null, null);
                    editor.AddNode(1, 2);
                    editor.SelectNodeIndicesForTest(0);

                    editor.MoveSelected(7, 8);

                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(editor.NodeAt(0).X, Is.EqualTo(7).Within(0.001));
                    Assert.That(editor.NodeAt(0).Y, Is.EqualTo(8).Within(0.001));
                    Assert.That(editor.SelectedNodeIndicesForTest(), Is.EqualTo(new[] { 0 }));
                }, CancellationToken.None);
            }
        }

        private static KeyEventArgs CreateKeyEventArgs(Key key)
        {
            var args = new KeyEventArgs();
            typeof(RoutedEventArgs).GetProperty("RoutedEvent", BindingFlags.Public | BindingFlags.Instance)
                ?.SetValue(args, Avalonia.Input.InputElement.KeyDownEvent, null);
            typeof(KeyEventArgs).GetProperty("Key", BindingFlags.Public | BindingFlags.Instance)
                ?.SetValue(args, key, null);
            return args;
        }
    }
}
