using System;
using System.IO;
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

        [Test]
        public async Task OdyToolLIP_LoadAudioFile_SetsDurationFromWav()
        {
            string wavPath = CreateTempWav(3.5f);
            try
            {
                using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
                {
                    await session.Dispatch(() =>
                    {
                        var editor = new OdyToolLIP(null, null);
                        editor.LoadAudioFile(wavPath);
                        Assert.That(editor.AudioFilePath, Is.EqualTo(wavPath));
                        Assert.That(editor.Duration, Is.EqualTo(3.5f).Within(0.01f));
                    }, CancellationToken.None);
                }
            }
            finally
            {
                if (File.Exists(wavPath))
                {
                    File.Delete(wavPath);
                }
            }
        }

        private static string CreateTempWav(float durationSeconds, int sampleRate = 44100)
        {
            int blockAlign = 2;
            int frameCount = (int)(durationSeconds * sampleRate);
            int dataSize = frameCount * blockAlign;
            byte[] wavBytes;
            using (var ms = new MemoryStream())
            {
                using (var bw = new System.IO.BinaryWriter(ms))
                {
                    bw.Write(new byte[] { 0x52, 0x49, 0x46, 0x46 });
                    bw.Write((uint)(4 + 8 + 16 + 8 + dataSize));
                    bw.Write(new byte[] { 0x57, 0x41, 0x56, 0x45 });
                    bw.Write(new byte[] { 0x66, 0x6D, 0x74, 0x20 });
                    bw.Write((uint)16);
                    bw.Write((ushort)1);
                    bw.Write((ushort)1);
                    bw.Write((uint)sampleRate);
                    bw.Write((uint)(sampleRate * blockAlign));
                    bw.Write((ushort)blockAlign);
                    bw.Write((ushort)16);
                    bw.Write(new byte[] { 0x64, 0x61, 0x74, 0x61 });
                    bw.Write((uint)dataSize);
                    bw.Write(new byte[dataSize]);
                }
                wavBytes = ms.ToArray();
            }

            string path = Path.Combine(Path.GetTempPath(), "odylip_" + Guid.NewGuid().ToString("N") + ".wav");
            File.WriteAllBytes(path, wavBytes);
            return path;
        }
    }
}
