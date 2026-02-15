using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// WAV Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolWAVTests
    {
        [Test]
        public async Task OdyToolWAV_LoadEmpty_BuildsEmptyBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolWAV(null, null);
                    editor.Load("test.wav", "test", ResourceType.WAV, new byte[0]);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolWAV_LoadAndBuild_PreservesData()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] original = new byte[] { 0x52, 0x49, 0x46, 0x46 }; // "RIFF" header start
                    var editor = new OdyToolWAV(null, null);
                    editor.Load("test.wav", "test", ResourceType.WAV, original);
                    Tuple<byte[], byte[]> buildResult = editor.Build();
                    byte[] built = buildResult.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    Assert.That(built, Is.EqualTo(original));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolWAV_New_BuildsEmptyBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolWAV(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }
    }
}
