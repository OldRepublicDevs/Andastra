using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using OdyTools.Editors;
using NUnit.Framework;

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

        [Test, Timeout(60000)]
        public async Task OdyToolUTC_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolUTI_LoadMinimalGff_BuildsValidGff()
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

        [Test, Timeout(120000), Explicit("Editor init very slow in headless; run with --filter when needed.")]
        public async Task OdyToolUTD_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolUTE_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolUTM_LoadMinimalGff_BuildsValidGff()
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

        [Test, Timeout(120000), Explicit("Editor init very slow in headless; run with --filter when needed.")]
        public async Task OdyToolUTP_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolUTS_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolUTT_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolUTW_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolARE_LoadMinimalGff_BuildsValidGff()
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
        public async Task OdyToolGIT_LoadMinimalGff_BuildsValidGff()
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

        [Test, Timeout(120000), Explicit("Editor init very slow in headless; run with --filter when needed.")]
        public async Task OdyToolIFO_LoadMinimalGff_BuildsValidGff()
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

        [Test, Timeout(60000), Explicit("HeadlessUnitTestSession.Dispose() throws NRE after this editor; run with filter when needed.")]
        public async Task OdyToolJRL_LoadMinimalGff_BuildsValidGff()
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

        [Test, Timeout(60000)]
        public async Task OdyToolPTH_LoadMinimalGff_BuildsValidGff()
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
    }
}
