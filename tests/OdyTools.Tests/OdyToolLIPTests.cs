using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.LIP;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// LIP Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolLIPTests
    {
        [Test]
        public async Task OdyToolLIP_New_BuildsValidLIP()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLIP(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                    LIP loaded = LIPAuto.ReadLip(data);
                    Assert.That(loaded, Is.Not.Null);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLIP_AddKeyframe_BuildRoundTripsKeyframes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLIP(null, null);
                    editor.New();
                    editor.Duration = 5.0f;
                    editor.AddKeyframe(1.0f, LIPShape.AH);
                    editor.AddKeyframe(2.5f, LIPShape.OH);
                    Tuple<byte[], byte[]> result = editor.Build();
                    LIP loaded = LIPAuto.ReadLip(result.Item1);
                    Assert.That(loaded.Frames.Count, Is.EqualTo(2));
                    Assert.That(loaded.Length, Is.EqualTo(5.0f).Within(0.001f));
                    Assert.That(loaded.Frames[0].Shape, Is.EqualTo(LIPShape.AH));
                    Assert.That(loaded.Frames[1].Shape, Is.EqualTo(LIPShape.OH));
                }, CancellationToken.None);
            }
        }
    }
}
