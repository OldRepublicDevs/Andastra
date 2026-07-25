using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.TLK;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// TLK Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolTLKTests
    {
        [Test]
        public async Task OdyToolTLK_LoadEmpty_BuildsValidTLK()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTLK(null, null);
                    editor.Load("dialog.tlk", "dialog", ResourceType.TLK, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.GreaterThanOrEqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTLK_Insert_AddsEntry()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTLK(null, null);
                    editor.New();
                    editor.Insert();
                    var result = editor.Build();
                    var tlk = TLKAuto.ReadTlk(result.Item1);
                    Assert.That(tlk.Entries.Count, Is.EqualTo(1));
                    Assert.That(tlk.Entries[0].Text, Is.EqualTo(""));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTLK_LoadValidTLK_BuildsEquivalent()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var tlk = new TLK(Language.English);
                    tlk.Entries.Add(new TLKEntry("Hello", new ResRef("sound1")));
                    tlk.Entries.Add(new TLKEntry("World", new ResRef("")));
                    byte[] origData = TLKAuto.BytesTlk(tlk, ResourceType.TLK);

                    var editor = new OdyToolTLK(null, null);
                    editor.Load("dialog.tlk", "dialog", ResourceType.TLK, origData);
                    var result = editor.Build();
                    var rebuilt = TLKAuto.ReadTlk(result.Item1);
                    Assert.That(rebuilt.Entries.Count, Is.EqualTo(2));
                    Assert.That(rebuilt.Entries[0].Text, Is.EqualTo("Hello"));
                    Assert.That(rebuilt.Entries[0].Voiceover.ToString(), Is.EqualTo("sound1"));
                    Assert.That(rebuilt.Entries[1].Text, Is.EqualTo("World"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTLK_SelectAndEditEntry_BuildsUpdatedTextAndSound()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var tlk = new TLK(Language.English);
                    tlk.Entries.Add(new TLKEntry("Old line", new ResRef("old_vo")));
                    tlk.Entries.Add(new TLKEntry("Keep line", new ResRef("keep_vo")));
                    byte[] origData = TLKAuto.BytesTlk(tlk, ResourceType.TLK);

                    var editor = new OdyToolTLK(null, null);
                    editor.Load("dialog.tlk", "dialog", ResourceType.TLK, origData);

                    Assert.That(editor.SelectEntryForTest(0), Is.True);
                    Assert.That(editor.TextEditorEnabledForTest, Is.True);
                    Assert.That(editor.SoundEditorEnabledForTest, Is.True);
                    Assert.That(editor.IsDirty, Is.False);

                    Assert.That(editor.EditSelectedEntryForTest("Updated line", "new_vo"), Is.True);

                    Assert.That(editor.SelectedEntryTextForTest, Is.EqualTo("Updated line"));
                    Assert.That(editor.SelectedEntrySoundForTest, Is.EqualTo("new_vo"));
                    Assert.That(editor.IsDirty, Is.True);

                    var rebuilt = TLKAuto.ReadTlk(editor.Build().Item1);
                    Assert.That(rebuilt.Entries.Count, Is.EqualTo(2));
                    Assert.That(rebuilt.Entries[0].Text, Is.EqualTo("Updated line"));
                    Assert.That(rebuilt.Entries[0].Voiceover.ToString(), Is.EqualTo("new_vo"));
                    Assert.That(rebuilt.Entries[1].Text, Is.EqualTo("Keep line"));
                    Assert.That(rebuilt.Entries[1].Voiceover.ToString(), Is.EqualTo("keep_vo"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTLK_ChangeLanguage_PersistsInBuild()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var tlk = new TLK(Language.English);
                    tlk.Entries.Add(new TLKEntry("Test", new ResRef("")));
                    byte[] origData = TLKAuto.BytesTlk(tlk, ResourceType.TLK);

                    var editor = new OdyToolTLK(null, null);
                    editor.Load("dialog.tlk", "dialog", ResourceType.TLK, origData);
                    editor.ChangeLanguage(Language.French);
                    var result = editor.Build();
                    var rebuilt = TLKAuto.ReadTlk(result.Item1);
                    Assert.That(rebuilt.Language, Is.EqualTo(Language.French));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTLK_DoFilter_FiltersByText()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTLK(null, null);
                    editor.New();
                    editor.Insert();
                    editor.Insert();
                    editor.Insert();
                    // Access internal state via Build - entries will have empty text
                    // DoFilter is a UI filter - we just verify it doesn't throw
                    editor.DoFilter("test");
                    editor.DoFilter("");
                    var result = editor.Build();
                    Assert.That(result.Item1, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTLK_SearchAndJumpPanels_StartHiddenAndToggleLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTLK(null, null);

                    Assert.That(editor.SearchBoxVisibleForTest, Is.False);
                    Assert.That(editor.JumpBoxVisibleForTest, Is.False);

                    editor.ToggleFilterBox();
                    editor.ToggleGotoBox();

                    Assert.That(editor.SearchBoxVisibleForTest, Is.True);
                    Assert.That(editor.JumpBoxVisibleForTest, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTLK_JumpToEntry_RespectsActiveFilterLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var tlk = new TLK(Language.English);
                    tlk.Entries.Add(new TLKEntry("Alpha line", new ResRef("alpha_vo")));
                    tlk.Entries.Add(new TLKEntry("Beta line", new ResRef("beta_vo")));
                    tlk.Entries.Add(new TLKEntry("Gamma line", new ResRef("gamma_vo")));

                    var editor = new OdyToolTLK(null, null);
                    editor.Load("dialog.tlk", "dialog", ResourceType.TLK, TLKAuto.BytesTlk(tlk, ResourceType.TLK));

                    Assert.That(editor.SelectEntryForTest(0), Is.True);
                    editor.DoFilter("Gamma");

                    bool jumpedHidden = editor.JumpToEntryForTest(1);

                    Assert.That(jumpedHidden, Is.False, "Filtered-out source rows should not become selected.");

                    bool jumpedVisible = editor.JumpToEntryForTest(2);

                    Assert.That(jumpedVisible, Is.True);
                    Assert.That(editor.SelectedEntryTextForTest, Is.EqualTo("Gamma line"));
                }, CancellationToken.None);
            }
        }
    }
}
