using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.LTR;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// LTR Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolLTRTests
    {
        [Test]
        public async Task OdyToolLTR_LoadEmpty_BuildsValidLtr()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLTR(null, null);
                    editor.Load("test.ltr", "test", ResourceType.LTR, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                    LTR loaded = LTRAuto.ReadLtr(data);
                    Assert.That(loaded, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLTR_New_BuildsValidLtr()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLTR(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLTR_LoadAndBuild_PreservesProbabilities()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var source = new LTR();
                    source.SetSinglesStart("a", 0.77f);
                    source.SetSinglesMiddle("z", 0.33f);
                    source.SetSinglesEnd("-", 0.11f);
                    source.SetDoublesStart("a", "b", 0.52f);
                    source.SetDoublesMiddle("c", "'", 0.19f);
                    source.SetDoublesEnd("d", "e", 0.91f);
                    source.SetTriplesStart("a", "b", "c", 0.43f);
                    source.SetTriplesMiddle("x", "y", "z", 0.27f);
                    source.SetTriplesEnd("'", "-", "a", 0.66f);

                    byte[] input = LTRAuto.BytesLtr(source);
                    var editor = new OdyToolLTR(null, null);
                    editor.Load("test.ltr", "test", ResourceType.LTR, input);
                    byte[] output = editor.Build().Item1;
                    var result = LTRAuto.ReadLtr(output);

                    Assert.That(result.GetSinglesStart("a"), Is.EqualTo(0.77f).Within(0.0001f));
                    Assert.That(result.GetSinglesMiddle("z"), Is.EqualTo(0.33f).Within(0.0001f));
                    Assert.That(result.GetSinglesEnd("-"), Is.EqualTo(0.11f).Within(0.0001f));
                    Assert.That(result.GetDoublesStart("a", "b"), Is.EqualTo(0.52f).Within(0.0001f));
                    Assert.That(result.GetDoublesMiddle("c", "'"), Is.EqualTo(0.19f).Within(0.0001f));
                    Assert.That(result.GetDoublesEnd("d", "e"), Is.EqualTo(0.91f).Within(0.0001f));
                    Assert.That(result.GetTriplesStart("a", "b", "c"), Is.EqualTo(0.43f).Within(0.0001f));
                    Assert.That(result.GetTriplesMiddle("x", "y", "z"), Is.EqualTo(0.27f).Within(0.0001f));
                    Assert.That(result.GetTriplesEnd("'", "-", "a"), Is.EqualTo(0.66f).Within(0.0001f));
                }, CancellationToken.None);
            }
        }
    }
}
