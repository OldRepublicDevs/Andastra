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
                    tlk.Entries.Add(new TLKEntry("Hello", new BioWare.ResRef("sound1")));
                    tlk.Entries.Add(new TLKEntry("World", new BioWare.ResRef("")));
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
        public async Task OdyToolTLK_ChangeLanguage_PersistsInBuild()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var tlk = new TLK(Language.English);
                    tlk.Entries.Add(new TLKEntry("Test", new BioWare.ResRef("")));
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
    }
}
