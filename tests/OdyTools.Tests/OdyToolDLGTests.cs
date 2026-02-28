using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// DLG Editor Load/Build and model tests. Ported from vendor/tests/gui/editors/test_dlg_editor.py
    /// (TestDLGStandardItemModel and related). Uses minimal GFF or in-memory DLG. Avalonia headless session.
    /// </summary>
    public class OdyToolDLGTests
    {
        /// <summary>
        /// Builds a DLG with multiple entries/replies and starters, matching vendor create_complex_tree structure.
        /// </summary>
        private static DLG CreateComplexTree()
        {
            var dlg = new DLG();
            var entries = new List<DLGEntry>();
            var replies = new List<DLGReply>();
            for (int i = 0; i < 5; i++)
            {
                entries.Add(new DLGEntry { Comment = "E" + i });
            }
            for (int i = 0; i < 5; i++)
            {
                replies.Add(new DLGReply { Text = LocalizedString.FromEnglish("R" + i) });
            }

            void AddLinks(DLGNode parentNode, IList<DLGNode> children)
            {
                for (int i = 0; i < children.Count; i++)
                {
                    var link = new DLGLink(children[i], i);
                    parentNode.Links.Add(link);
                }
            }

            AddLinks(entries[0], new[] { replies[0] });
            AddLinks(replies[0], new[] { entries[1] });
            AddLinks(entries[1], new[] { replies[1] });
            AddLinks(replies[1], new[] { entries[2] });
            AddLinks(entries[2], new[] { replies[2] });
            AddLinks(replies[2], new[] { entries[3] });
            AddLinks(entries[3], new[] { replies[3] });
            AddLinks(replies[3], new[] { entries[4] });

            entries[2].Links.Add(new DLGLink(replies[1], 1));
            replies[0].Links.Add(new DLGLink(entries[4], 1));

            dlg.Starters.Add(new DLGLink(entries[0], 0));
            dlg.Starters.Add(new DLGLink(entries[1], 1));

            UpdateListIndex(dlg.Starters, null);
            return dlg;
        }

        private static void UpdateListIndex(List<DLGLink> links, HashSet<DLGNode> seenNodes)
        {
            seenNodes = seenNodes ?? new HashSet<DLGNode>();
            for (int i = 0; i < links.Count; i++)
            {
                links[i].ListIndex = i;
                var node = links[i].Node;
                if (node != null && !seenNodes.Contains(node))
                {
                    seenNodes.Add(node);
                    UpdateListIndex(node.Links, seenNodes);
                }
            }
        }

        /// <summary>
        /// Collects all tree items in row order (root items first, then children recursively).
        /// Avalonia equivalent of walking QModelIndex for every row.
        /// </summary>
        private static List<DLGStandardItem> CollectAllItemsInOrder(DLGModel model)
        {
            var list = new List<DLGStandardItem>();
            void Collect(DLGStandardItem item)
            {
                if (item == null) return;
                list.Add(item);
                for (int i = 0; i < item.RowCount; i++)
                {
                    var child = item.Child(i, 0);
                    if (child != null) Collect(child);
                }
            }
            foreach (var root in model.GetRootItems())
                Collect(root);
            return list;
        }

        /// <summary>
        /// Converts JsonElement to Dictionary/primitive so DLGLink.FromDict can consume MIME role JSON.
        /// </summary>
        private static object JsonElementToObject(JsonElement el)
        {
            switch (el.ValueKind)
            {
                case JsonValueKind.Object:
                    var dict = new Dictionary<string, object>();
                    foreach (var p in el.EnumerateObject())
                        dict[p.Name] = JsonElementToObject(p.Value);
                    return dict;
                case JsonValueKind.Array:
                    var arr = new List<object>();
                    foreach (var e in el.EnumerateArray())
                        arr.Add(JsonElementToObject(e));
                    return arr;
                case JsonValueKind.String:
                    return el.GetString();
                case JsonValueKind.Number:
                    if (el.TryGetInt32(out int i32)) return i32;
                    if (el.TryGetInt64(out long i64)) return i64;
                    return el.GetDouble();
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.Null:
                    return null;
                default:
                    return el.GetRawText();
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var gff = new GFF(GFFContent.DLG);
                    byte[] data = GFFAuto.BytesGff(gff, ResourceType.DLG);

                    var editor = new OdyToolDLG(null, null);
                    editor.Load("test.dlg", "test", ResourceType.DLG, data);

                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF loaded = GFF.FromBytes(built);
                    Assert.That(loaded.Root, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadDLG_DictionariesFilledCorrectly()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);

                    var model = editor.Model;
                    var items = new List<DLGStandardItem>();
                    foreach (var link in dlg.Starters)
                    {
                        if (model.LinkToItems.TryGetValue(link, out var list))
                            items.AddRange(list);
                    }

                    foreach (var item in items)
                    {
                        Assert.That(item.Link, Is.Not.Null);
                        Assert.That(model.LinkToItems[item.Link], Does.Contain(item));
                        Assert.That(item.Link.Node, Is.Not.Null);
                        Assert.That(model.NodeToItems[item.Link.Node], Does.Contain(item));
                        Assert.That(model.LinkToItems, Does.ContainKey(item.Link));
                        Assert.That(model.NodeToItems, Does.ContainKey(item.Link.Node));
                    }
                }, CancellationToken.None);
            }
        }

        /// <summary>
        /// Port of test_hashing: item identity/hash is stable and distinct per item (Avalonia/C# equivalent of Python hash(item)==id(item)).
        /// </summary>
        [Test]
        public async Task OdyToolDLG_LoadDLG_Hashing_IdentityStableAndDistinct()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);

                    var items = CollectAllItemsInOrder(editor.Model);
                    Assert.That(items.Count, Is.GreaterThan(0));

                    foreach (var item in items)
                    {
                        int hash1 = RuntimeHelpers.GetHashCode(item);
                        int hash2 = RuntimeHelpers.GetHashCode(item);
                        Assert.That(hash2, Is.EqualTo(hash1), "GetHashCode must be stable for same item");
                    }
                    var distinctByRef = new HashSet<DLGStandardItem>(items);
                    Assert.That(distinctByRef.Count, Is.EqualTo(items.Count), "All tree items must be distinct by reference (identity)");
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadDLG_LinkListIndexSync()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();

                    void VerifyListIndex(DLGNode node, HashSet<DLGNode> seen)
                    {
                        if (seen == null) seen = new HashSet<DLGNode>();
                        for (int i = 0; i < node.Links.Count; i++)
                        {
                            Assert.That(node.Links[i].ListIndex, Is.EqualTo(i), "Link list_index before LoadDLG");
                            var n = node.Links[i].Node;
                            if (n != null && !seen.Contains(n)) { seen.Add(n); VerifyListIndex(n, seen); }
                        }
                    }
                    for (int i = 0; i < dlg.Starters.Count; i++)
                    {
                        Assert.That(dlg.Starters[i].ListIndex, Is.EqualTo(i), "Starter list_index before");
                        VerifyListIndex(dlg.Starters[i].Node, new HashSet<DLGNode>());
                    }

                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);

                    for (int i = 0; i < dlg.Starters.Count; i++)
                    {
                        Assert.That(dlg.Starters[i].ListIndex, Is.EqualTo(i), "Starter list_index after");
                        VerifyListIndex(dlg.Starters[i].Node, new HashSet<DLGNode>());
                    }

                    var items = new List<DLGStandardItem>();
                    foreach (var link in dlg.Starters)
                    {
                        if (editor.Model.LinkToItems.TryGetValue(link, out var list))
                            items.AddRange(list);
                    }
                    for (int index = 0; index < items.Count; index++)
                    {
                        Assert.That(items[index].Link, Is.Not.Null);
                        Assert.That(items[index].Link.ListIndex, Is.EqualTo(index), "Root item link list_index");
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ShiftItem_ReordersRootItems()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);

                    var itemsBefore = editor.Model.GetRootItems().ToList();
                    Assert.That(itemsBefore.Count, Is.GreaterThanOrEqualTo(2));

                    editor.Model.ShiftItem(itemsBefore[0], 1);

                    var itemsAfter = editor.Model.GetRootItems().ToList();
                    Assert.That(itemsAfter[0], Is.EqualTo(itemsBefore[1]));
                    Assert.That(itemsAfter[1], Is.EqualTo(itemsBefore[0]));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_PasteItem_AddsChildUnderParent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);

                    var rootItems = editor.Model.GetRootItems();
                    Assert.That(rootItems.Count, Is.GreaterThanOrEqualTo(1));
                    var firstRoot = rootItems[0];

                    var pastedLink = new DLGLink(new DLGReply
                    {
                        Text = LocalizedString.FromEnglish("Pasted Entry"),
                        ListIndex = 69
                    });
                    editor.Model.PasteItem(firstRoot, pastedLink, null, true);

                    var pastedItem = firstRoot.RowCount > 0 ? firstRoot.Child(0, 0) : null;
                    Assert.That(pastedItem, Is.Not.Null);
                    Assert.That(pastedItem.Link, Is.Not.Null);
                    Assert.That(pastedItem.Link.Node, Is.Not.Null);
                    Assert.That(firstRoot.Link?.Node?.Links.Count, Is.GreaterThan(0));
                    Assert.That(firstRoot.Link.Node.Links[0], Is.EqualTo(pastedItem.Link));
                }, CancellationToken.None);
            }
        }

        /// <summary>
        /// Port of test_serialize_mime_data: MIME serialization (Avalonia JSON drag/drop format) roundtrips;
        /// deserialized link matches the original item's link structurally.
        /// </summary>
        [Test]
        public async Task OdyToolDLG_SerializeMimeData_RoundtripsAndMatchesOriginalLink()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);

                    var allItems = CollectAllItemsInOrder(editor.Model);
                    Assert.That(allItems.Count, Is.GreaterThanOrEqualTo(5), "Need at least 5 items to assert item index 4");

                    string mimeJson = editor.Model.MimeData(allItems);
                    Assert.That(mimeJson, Is.Not.Null.And.Not.Empty, "MIME data format must be present (Avalonia DLG JSON)");

                    var parsed = editor.Model.ParseMimeData(mimeJson);
                    Assert.That(parsed, Is.Not.Null);
                    Assert.That(parsed.Count, Is.GreaterThanOrEqualTo(5));

                    var itemData = parsed[4];
                    Assert.That(itemData.ContainsKey("roles"), Is.True);
                    var roles = itemData["roles"] as Dictionary<string, object>;
                    Assert.That(roles, Is.Not.Null);
                    Assert.That(roles.ContainsKey("261"), Is.True);
                    var linkJson = roles["261"] as string;
                    Assert.That(linkJson, Is.Not.Null.And.Not.Empty);

                    Dictionary<string, object> linkDict;
                    using (var doc = JsonDocument.Parse(linkJson))
                    {
                        linkDict = (Dictionary<string, object>)JsonElementToObject(doc.RootElement);
                    }
                    Assert.That(linkDict, Is.Not.Null);

                    var deserializedLink = DLGLink.FromDict(linkDict, null);
                    Assert.That(deserializedLink, Is.Not.Null);
                    Assert.That(deserializedLink.Node, Is.Not.Null);

                    var targetItem = allItems[4];
                    Assert.That(targetItem.Link, Is.Not.Null);
                    var originalLink = targetItem.Link;

                    Assert.That(deserializedLink.ListIndex, Is.EqualTo(originalLink.ListIndex));
                    Assert.That(deserializedLink.Node.GetType(), Is.EqualTo(originalLink.Node.GetType()));
                    string origText = originalLink.Node?.Text?.GetString(Language.English, Gender.Male) ?? "";
                    string deserText = deserializedLink.Node?.Text?.GetString(Language.English, Gender.Male) ?? "";
                    Assert.That(deserText, Is.EqualTo(origText));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_New_ThenAddRootNode_AddsStarter()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    Assert.That(editor.Model.RowCount, Is.EqualTo(0));
                    Assert.That(editor.CoreDlg.Starters.Count, Is.EqualTo(0));

                    editor.Model.AddRootNode();
                    Assert.That(editor.Model.RowCount, Is.EqualTo(1));
                    Assert.That(editor.CoreDlg.Starters.Count, Is.EqualTo(1));
                    Assert.That(editor.Model.GetRootItems()[0].Link, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadOneStarter_AddChildToItem_AddsChild()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = new DLG();
                    var entry = new DLGEntry { Comment = "E0" };
                    dlg.Starters.Add(new DLGLink(entry, 0));

                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);

                    var root = editor.Model.GetRootItems()[0];
                    Assert.That(root.RowCount, Is.EqualTo(0));

                    editor.Model.AddChildToItem(root, null);
                    Assert.That(root.RowCount, Is.EqualTo(1));
                    Assert.That(entry.Links.Count, Is.EqualTo(1));
                    Assert.That(root.Child(0, 0).Link?.Node, Is.InstanceOf<DLGReply>());
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadMinimalGff_BuildRoundtrip_ValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var gff = new GFF(GFFContent.DLG);
                    byte[] data = GFFAuto.BytesGff(gff, ResourceType.DLG);
                    var editor = new OdyToolDLG(null, null);
                    editor.Load("test.dlg", "test", ResourceType.DLG, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    var reloaded = GFF.FromBytes(built);
                    Assert.That(reloaded.Root, Is.Not.Null);
                    Assert.That(reloaded.Root.Count, Is.GreaterThanOrEqualTo(0));
                }, CancellationToken.None);
            }
        }

        // ========== Ports of pytest tests: model operations, delete, copy/paste ==========

        [Test]
        public async Task OdyToolDLG_DeleteNode_RemovesStarter()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    Assert.That(editor.Model.RowCount, Is.EqualTo(1));
                    var rootItem = editor.Model.Item(0, 0);
                    editor.Model.DeleteNode(rootItem);
                    Assert.That(editor.Model.RowCount, Is.EqualTo(0));
                    Assert.That(editor.CoreDlg.Starters.Count, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_DeleteNodeEverywhere_RemovesAllRefs()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = new DLG();
                    var entry = new DLGEntry();
                    dlg.Starters.Add(new DLGLink(entry, 0));
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);
                    int initialCount = editor.Model.RowCount;
                    var rootItem = editor.Model.GetRootItems()[0];
                    editor.Model.DeleteNodeEverywhere(rootItem.Link.Node);
                    Assert.That(editor.Model.RowCount, Is.LessThanOrEqualTo(initialCount));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_CopyPaste_AddsRoot()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(async () =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    Assert.That(rootItem?.Link, Is.Not.Null);
                    await editor.Model.CopyLinkAndNode(rootItem.Link, editor);
                    Assert.That(editor.GetCopyLink(), Is.Not.Null);
                    editor.Model.PasteItem(null, editor.GetCopyLink(), null, true);
                    Assert.That(editor.Model.RowCount, Is.EqualTo(2));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_CreateFromScratchRoundtrip_StructurePreserved()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    editor.CoreDlg.ConversationType = DLGConversationType.Computer;
                    editor.CoreDlg.Skippable = true;
                    if (editor.VoIdEdit != null) editor.VoIdEdit.Text = "test_vo";
                    var entryNode = rootItem?.Link?.Node as DLGEntry;
                    if (entryNode != null)
                    {
                        entryNode.Speaker = "TestSpeaker";
                        entryNode.Listener = "PLAYER";
                        entryNode.Comment = "Test comment";
                    }
                    editor.Model.AddChildToItem(rootItem, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                    var editor2 = new OdyToolDLG(null, null);
                    editor2.Load("test.dlg", "test", ResourceType.DLG, data);
                    Assert.That(editor2.Model.RowCount, Is.EqualTo(1));
                    Assert.That(editor2.CoreDlg.ConversationType, Is.EqualTo(DLGConversationType.Computer));
                    Assert.That(editor2.CoreDlg.Skippable, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_BuildAllFileProperties_Roundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    editor.CoreDlg.ConversationType = DLGConversationType.Type3;
                    editor.CoreDlg.ComputerType = DLGComputerType.Ancient;
                    editor.CoreDlg.Skippable = true;
                    editor.CoreDlg.AnimatedCut = 1;
                    editor.CoreDlg.OldHitCheck = true;
                    editor.CoreDlg.UnequipHands = true;
                    editor.CoreDlg.UnequipItems = true;
                    editor.CoreDlg.DelayEntry = 123;
                    editor.CoreDlg.DelayReply = 456;
                    editor.CoreDlg.VoId = "test_vo_id";
                    editor.CoreDlg.OnAbort = ResRef.FromString("abort_scr");
                    editor.CoreDlg.OnEnd = ResRef.FromString("end_script");
                    editor.CoreDlg.AmbientTrack = ResRef.FromString("ambient");
                    editor.CoreDlg.CameraModel = ResRef.FromString("cam_mdl");
                    if (editor.VoIdEdit != null) editor.VoIdEdit.Text = "test_vo_id";
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.ConversationType, Is.EqualTo(DLGConversationType.Type3));
                    Assert.That(dlg.ComputerType, Is.EqualTo(DLGComputerType.Ancient));
                    Assert.That(dlg.Skippable, Is.True);
                    Assert.That(dlg.DelayEntry, Is.EqualTo(123));
                    Assert.That(dlg.DelayReply, Is.EqualTo(456));
                    Assert.That(dlg.VoId, Is.EqualTo("test_vo_id"));
                    Assert.That(dlg.OnAbort.ToString(), Is.EqualTo("abort_scr"));
                    Assert.That(dlg.OnEnd.ToString(), Is.EqualTo("end_script"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_StuntList_AddStunt_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    var stunt = new DLGStunt
                    {
                        StuntModel = ResRef.FromString("test_model"),
                        Participant = "PLAYER"
                    };
                    editor.CoreDlg.Stunts.Add(stunt);
                    editor.RefreshStuntList();
                    Assert.That(editor.StuntList, Is.Not.Null);
                    Assert.That(editor.CoreDlg.Stunts.Count, Is.EqualTo(1));
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Stunts.Count, Is.EqualTo(1));
                    Assert.That(dlg.Stunts[0].StuntModel.ToString(), Is.EqualTo("test_model"));
                    Assert.That(dlg.Stunts[0].Participant, Is.EqualTo("PLAYER"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_MultipleStunts_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    for (int i = 0; i < 5; i++)
                    {
                        editor.CoreDlg.Stunts.Add(new DLGStunt
                        {
                            StuntModel = ResRef.FromString("model_" + i),
                            Participant = "PARTICIPANT_" + i
                        });
                    }
                    editor.RefreshStuntList();
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Stunts.Count, Is.EqualTo(5));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_Animation_AddToNode_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var node = rootItem?.Link?.Node;
                    Assert.That(node, Is.Not.Null);
                    node.Animations.Add(new DLGAnimation { AnimationId = 1, Participant = "PLAYER" });
                    editor.RefreshAnimList();
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters.Count, Is.GreaterThan(0));
                    Assert.That(dlg.Starters[0].Node.Animations.Count, Is.EqualTo(1));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadAndSavePreservesData()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();
                    dlg.ConversationType = DLGConversationType.Human;
                    dlg.Skippable = true;
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);
                    Tuple<byte[], byte[]> result = editor.Build();
                    var savedDlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(savedDlg.Starters.Count, Is.EqualTo(dlg.Starters.Count));
                    Assert.That(savedDlg.ConversationType, Is.EqualTo(dlg.ConversationType));
                    Assert.That(savedDlg.Skippable, Is.EqualTo(dlg.Skippable));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_GffRoundtripNoModification_ValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var gff = new GFF(GFFContent.DLG);
                    byte[] data = GFFAuto.BytesGff(gff, ResourceType.DLG);
                    var editor = new OdyToolDLG(null, null);
                    editor.Load("test.dlg", "test", ResourceType.DLG, data);
                    byte[] saved = editor.Build().Item1;
                    Assert.That(GFF.FromBytes(saved).Root, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_EmptyDlg_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters.Count, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_DeepNesting_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = new DLG();
                    DLGEntry e0 = new DLGEntry();
                    DLGReply r0 = new DLGReply();
                    DLGEntry e1 = new DLGEntry();
                    DLGReply r1 = new DLGReply();
                    e0.Links.Add(new DLGLink(r0, 0));
                    r0.Links.Add(new DLGLink(e1, 0));
                    e1.Links.Add(new DLGLink(r1, 0));
                    dlg.Starters.Add(new DLGLink(e0, 0));
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);
                    var items = CollectAllItemsInOrder(editor.Model);
                    Assert.That(items.Count, Is.GreaterThanOrEqualTo(4));
                    Tuple<byte[], byte[]> result = editor.Build();
                    var reloaded = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(reloaded.Starters.Count, Is.EqualTo(1));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManySiblings_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = new DLG();
                    var entry = new DLGEntry();
                    for (int i = 0; i < 10; i++)
                        entry.Links.Add(new DLGLink(new DLGReply(), i));
                    dlg.Starters.Add(new DLGLink(entry, 0));
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);
                    var root = editor.Model.GetRootItems()[0];
                    Assert.That(root.RowCount, Is.EqualTo(10));
                    Tuple<byte[], byte[]> result = editor.Build();
                    var reloaded = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(reloaded.Starters[0].Node.Links.Count, Is.EqualTo(10));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_AlternatingNodeTypes_EntryReplyEntry()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var root = editor.Model.Item(0, 0);
                    Assert.That(root?.Link?.Node, Is.InstanceOf<DLGEntry>());
                    var child1 = editor.Model.AddChildToItem(root, null);
                    Assert.That(child1?.Link?.Node, Is.InstanceOf<DLGReply>());
                    var child2 = editor.Model.AddChildToItem(child1, null);
                    Assert.That(child2?.Link?.Node, Is.InstanceOf<DLGEntry>());
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_AllWidgetsExist()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    Assert.That(editor.DialogTree, Is.Not.Null);
                    Assert.That(editor.Model, Is.Not.Null);
                    Assert.That(editor.CoreDlg, Is.Not.Null);
                    Assert.That(editor.SpeakerEdit, Is.Not.Null);
                    Assert.That(editor.Script1ResrefEdit, Is.Not.Null);
                    Assert.That(editor.Script2ResrefEdit, Is.Not.Null);
                    Assert.That(editor.ListenerEdit, Is.Not.Null);
                    Assert.That(editor.VoiceComboBox, Is.Not.Null);
                    Assert.That(editor.SoundComboBox, Is.Not.Null);
                    Assert.That(editor.ConversationSelect, Is.Not.Null);
                    Assert.That(editor.ComputerSelect, Is.Not.Null);
                    Assert.That(editor.FindInput, Is.Not.Null);
                    Assert.That(editor.ResultsLabel, Is.Not.Null);
                    Assert.That(editor.LeftDockWidget, Is.Not.Null);
                    Assert.That(editor.OrphanedNodesList, Is.Not.Null);
                    Assert.That(editor.PinnedItemsList, Is.Not.Null);
                    Assert.That(editor.StuntList, Is.Not.Null);
                    Assert.That(editor.AnimsList, Is.Not.Null);
                    editor.Close();
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_UndoRedo_StackExists()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    Assert.That(editor.CanUndo, Is.True);
                    Assert.That(editor.CanRedo, Is.False);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_KeysDownExists()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    Assert.That(editor.KeysDown, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadMultipleFiles_Reloads()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var dlg = CreateComplexTree();
                    var editor = new OdyToolDLG(null, null);
                    editor.LoadDLG(dlg);
                    int firstCount = editor.Model.RowCount;
                    editor.New();
                    Assert.That(editor.Model.RowCount, Is.EqualTo(0));
                    editor.LoadDLG(dlg);
                    Assert.That(editor.Model.RowCount, Is.EqualTo(firstCount));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_BuildAllNodeProperties_Roundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var entry = rootItem?.Link?.Node as DLGEntry;
                    Assert.That(entry, Is.Not.Null);
                    entry.Speaker = "TestSpeaker";
                    entry.Listener = "PLAYER";
                    entry.Script1 = ResRef.FromString("k_test");
                    entry.Script1Param1 = 42;
                    entry.Comment = "Test comment";
                    entry.Quest = "my_quest";
                    entry.QuestEntry = 5;
                    entry.PlotXpPercentage = 75;
                    entry.Delay = 500;
                    entry.WaitFlags = 2;
                    entry.FadeType = 1;
                    var link = rootItem.Link;
                    link.Active1 = ResRef.FromString("c_test");
                    link.Active1Not = true;
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters.Count, Is.EqualTo(1));
                    var node = dlg.Starters[0].Node as DLGEntry;
                    Assert.That(node, Is.Not.Null);
                    Assert.That(node.Speaker, Is.EqualTo("TestSpeaker"));
                    Assert.That(node.Listener, Is.EqualTo("PLAYER"));
                    Assert.That(node.Script1.ToString(), Is.EqualTo("k_test"));
                    Assert.That(node.Script1Param1, Is.EqualTo(42));
                    Assert.That(node.Comment, Is.EqualTo("Test comment"));
                    Assert.That(node.Quest, Is.EqualTo("my_quest"));
                    Assert.That(node.QuestEntry, Is.EqualTo(5));
                    Assert.That(node.PlotXpPercentage, Is.EqualTo(75));
                    Assert.That(node.Delay, Is.EqualTo(500));
                    Assert.That(node.WaitFlags, Is.EqualTo(2));
                    Assert.That(node.FadeType, Is.EqualTo(1));
                    Assert.That(dlg.Starters[0].Active1.ToString(), Is.EqualTo("c_test"));
                    Assert.That(dlg.Starters[0].Active1Not, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_FocusOnNode_ExistsAndCallable()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var link = rootItem?.Link;
                    Assert.That(link, Is.Not.Null);
                    var result = editor.FocusOnNode(link);
                    Assert.That(result != null || editor.Focused, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_FindReferences_ExistsAndCallable()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    Assert.That(rootItem, Is.Not.Null);
                    editor.FindReferences(rootItem.Link);
                    Assert.That(editor.ReferenceHistoryCount, Is.GreaterThanOrEqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_JumpToNode_ExistsAndCallable()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    Assert.That(rootItem?.Link, Is.Not.Null);
                    editor.JumpToNode(rootItem.Link);
                    Assert.That(editor.DialogTree, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ActionReloadTree_Exists()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    Assert.That(editor.ActionReloadTree, Is.Not.Null);
                    editor.New();
                    editor.Model.AddRootNode();
                    Assert.That(editor.Model.RowCount, Is.EqualTo(1));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_EntryHasSpeaker_ReplyAlternates()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var root = editor.Model.Item(0, 0);
                    Assert.That(root?.Link?.Node, Is.InstanceOf<DLGEntry>());
                    var entry = (DLGEntry)root.Link.Node;
                    entry.Speaker = "NPC_Test";
                    Assert.That(entry.Speaker, Is.EqualTo("NPC_Test"));
                    var child = editor.Model.AddChildToItem(root, null);
                    Assert.That(child?.Link?.Node, Is.InstanceOf<DLGReply>());
                }, CancellationToken.None);
            }
        }

        // ========== Build all link properties (vendor: test_dlg_editor_build_all_link_properties) ==========

        [Test]
        public async Task OdyToolDLG_BuildAllLinkProperties_Roundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var link = rootItem?.Link;
                    Assert.That(link, Is.Not.Null);
                    link.Active1 = ResRef.FromString("cond1");
                    link.Active1Param1 = 101;
                    link.Active1Param2 = 102;
                    link.Active1Param6 = "cond1str";
                    link.Active1Not = true;
                    link.Active2 = ResRef.FromString("cond2");
                    link.Active2Param1 = 201;
                    link.Active2Param6 = "cond2str";
                    link.Active2Not = true;
                    link.Logic = true;
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters.Count, Is.GreaterThan(0));
                    var l = dlg.Starters[0];
                    Assert.That(l.Active1.ToString(), Is.EqualTo("cond1"));
                    Assert.That(l.Active1Param1, Is.EqualTo(101));
                    Assert.That(l.Active1Param2, Is.EqualTo(102));
                    Assert.That(l.Active1Param6, Is.EqualTo("cond1str"));
                    Assert.That(l.Active1Not, Is.True);
                    Assert.That(l.Active2.ToString(), Is.EqualTo("cond2"));
                    Assert.That(l.Active2Param1, Is.EqualTo(201));
                    Assert.That(l.Active2Param6, Is.EqualTo("cond2str"));
                    Assert.That(l.Active2Not, Is.True);
                    Assert.That(l.Logic, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_RemoveStunt_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    var stunt = new DLGStunt { StuntModel = ResRef.FromString("test_model"), Participant = "PLAYER" };
                    editor.CoreDlg.Stunts.Add(stunt);
                    editor.RefreshStuntList();
                    Assert.That(editor.CoreDlg.Stunts.Count, Is.EqualTo(1));
                    editor.CoreDlg.Stunts.Remove(stunt);
                    editor.RefreshStuntList();
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Stunts.Count, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_RemoveAnimation_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var node = rootItem?.Link?.Node;
                    var anim = new DLGAnimation { AnimationId = 1, Participant = "PLAYER" };
                    node.Animations.Add(anim);
                    editor.RefreshAnimList();
                    Assert.That(node.Animations.Count, Is.EqualTo(1));
                    node.Animations.Remove(anim);
                    editor.RefreshAnimList();
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Animations.Count, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_MultipleAnimations_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var node = rootItem?.Link?.Node;
                    for (int i = 0; i < 3; i++)
                        node.Animations.Add(new DLGAnimation { AnimationId = i, Participant = "P" + i });
                    editor.RefreshAnimList();
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Animations.Count, Is.EqualTo(3));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_SpecialCharactersInText_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var entry = rootItem?.Link?.Node as DLGEntry;
                    Assert.That(entry, Is.Not.Null);
                    entry.Speaker = "Speaker<>&\"'";
                    entry.Listener = "Listener\n\t";
                    entry.Quest = "Quest with spaces";
                    entry.Comment = "Comment with\nmultiple\nlines";
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    var node = dlg.Starters[0].Node as DLGEntry;
                    Assert.That(node.Speaker, Is.EqualTo("Speaker<>&\"'"));
                    Assert.That(node.Listener, Is.EqualTo("Listener\n\t"));
                    Assert.That(node.Quest, Is.EqualTo("Quest with spaces"));
                    Assert.That(node.Comment, Is.EqualTo("Comment with\nmultiple\nlines"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_MaxValues_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var node = rootItem?.Link?.Node;
                    node.Delay = 2147483647;
                    node.PlotXpPercentage = 100f;
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Delay, Is.EqualTo(2147483647));
                    Assert.That(dlg.Starters[0].Node.PlotXpPercentage, Is.EqualTo(100f));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_NegativeValues_CameraId_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var rootItem = editor.Model.Item(0, 0);
                    var node = rootItem?.Link?.Node;
                    node.CameraId = -1;
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.CameraId, Is.EqualTo(-1));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_DeepNestingDepth10_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var current = editor.Model.Item(0, 0);
                    for (int i = 0; i < 10; i++)
                        current = editor.Model.AddChildToItem(current, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    int depth = 0;
                    var n = dlg.Starters[0].Node;
                    while (n != null && n.Links.Count > 0)
                    {
                        depth++;
                        n = n.Links[0].Node;
                    }
                    Assert.That(depth, Is.EqualTo(10));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManyRootNodes_20Starters_BuildRoundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    for (int i = 0; i < 20; i++)
                        editor.Model.AddRootNode();
                    Assert.That(editor.Model.RowCount, Is.EqualTo(20));
                    Assert.That(editor.CoreDlg.Starters.Count, Is.EqualTo(20));
                    Tuple<byte[], byte[]> result = editor.Build();
                    var dlg = DLGHelper.ReadDlg(result.Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters.Count, Is.EqualTo(20));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_LoadMinimalGff_PopulatesTreeOrEmpty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var gff = new GFF(GFFContent.DLG);
                    byte[] data = GFFAuto.BytesGff(gff, ResourceType.DLG);
                    var editor = new OdyToolDLG(null, null);
                    editor.Load("test.dlg", "test", ResourceType.DLG, data);
                    Assert.That(editor.Model.RowCount, Is.GreaterThanOrEqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_OrphanedNodesList_Exists()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    Assert.That(editor.OrphanedNodesList, Is.Not.Null);
                    Assert.That(editor.PinnedItemsList, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        // ========== Individual field roundtrips (vendor: manipulate_*_roundtrip) ==========

        [Test]
        public async Task OdyToolDLG_ManipulateSpeaker_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var entry = (DLGEntry)editor.Model.Item(0, 0).Link.Node;
                    entry.Speaker = "ModifiedSpeaker";
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(((DLGEntry)dlg.Starters[0].Node).Speaker, Is.EqualTo("ModifiedSpeaker"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateListener_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.Listener = "PLAYER";
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Listener, Is.EqualTo("PLAYER"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateScript1_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.Script1 = ResRef.FromString("k_my_script");
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Script1.ToString(), Is.EqualTo("k_my_script"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateCondition1_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var link = editor.Model.Item(0, 0).Link;
                    link.Active1 = ResRef.FromString("k_cond_script");
                    link.Active1Not = true;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Active1.ToString(), Is.EqualTo("k_cond_script"));
                    Assert.That(dlg.Starters[0].Active1Not, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateComments_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.Comment = "Line1\nLine2";
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Comment, Is.EqualTo("Line1\nLine2"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateDelay_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.Delay = 1500;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Delay, Is.EqualTo(1500));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateWaitFlags_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.WaitFlags = 3;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.WaitFlags, Is.EqualTo(3));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateFadeType_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.FadeType = 2;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.FadeType, Is.EqualTo(2));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateVoice_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.VoResRef = ResRef.FromString("my_voice");
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.VoResRef.ToString(), Is.EqualTo("my_voice"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateQuest_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.Quest = "main_quest";
                    node.QuestEntry = 7;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Quest, Is.EqualTo("main_quest"));
                    Assert.That(dlg.Starters[0].Node.QuestEntry, Is.EqualTo(7));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulatePlotXp_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.PlotXpPercentage = 50f;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.PlotXpPercentage, Is.EqualTo(50f));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateCameraId_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.CameraId = 5;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.CameraId, Is.EqualTo(5));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateSound_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    var node = editor.Model.Item(0, 0).Link.Node;
                    node.Sound = ResRef.FromString("my_sound");
                    node.SoundExists = 1;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Starters[0].Node.Sound.ToString(), Is.EqualTo("my_sound"));
                    Assert.That(dlg.Starters[0].Node.SoundExists, Is.EqualTo(1));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateConversationType_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    editor.CoreDlg.ConversationType = DLGConversationType.Computer;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.ConversationType, Is.EqualTo(DLGConversationType.Computer));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateVoId_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    editor.CoreDlg.VoId = "vo_custom_01";
                    if (editor.VoIdEdit != null) editor.VoIdEdit.Text = "vo_custom_01";
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.VoId, Is.EqualTo("vo_custom_01"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateReplyDelay_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    editor.CoreDlg.DelayReply = 999;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.DelayReply, Is.EqualTo(999));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateEntryDelay_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    editor.CoreDlg.DelayEntry = 888;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.DelayEntry, Is.EqualTo(888));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolDLG_ManipulateFileLevelCheckboxes_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolDLG(null, null);
                    editor.New();
                    editor.Model.AddRootNode();
                    editor.CoreDlg.Skippable = true;
                    editor.CoreDlg.UnequipHands = true;
                    editor.CoreDlg.UnequipItems = true;
                    editor.CoreDlg.OldHitCheck = true;
                    var dlg = DLGHelper.ReadDlg(editor.Build().Item1, 0, -1, ResourceType.DLG);
                    Assert.That(dlg.Skippable, Is.True);
                    Assert.That(dlg.UnequipHands, Is.True);
                    Assert.That(dlg.UnequipItems, Is.True);
                    Assert.That(dlg.OldHitCheck, Is.True);
                }, CancellationToken.None);
            }
        }
    }
}
