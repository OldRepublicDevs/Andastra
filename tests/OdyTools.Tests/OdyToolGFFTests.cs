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
    /// GFF Editor Load/Build roundtrip tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolGFFTests
    {
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
    }
}
