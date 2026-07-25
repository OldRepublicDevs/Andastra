using System;
using System.IO;
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

        [TestCase(".ogg", "OGG Vorbis")]
        [TestCase(".wma", "Windows Media Audio")]
        [TestCase(".wmv", "Windows Media Video")]
        [TestCase(".xmv", "Xbox Media Video")]
        [TestCase(".flac", "FLAC")]
        [TestCase(".bmu", "BMU (obfuscated MP3)")]
        public async Task OdyToolWAV_CanLoadHolocronAudioExtensions(string extension, string expectedFormat)
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = CreateAudioHeader(extension);
                    string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + extension);
                    File.WriteAllBytes(path, data);

                    try
                    {
                        var editor = new OdyToolWAV(null, null);

                        Assert.That(editor.CanLoadPath(path), Is.True);
                        Assert.That(editor.TryLoadStartupPath(path), Is.True);
                        Assert.That(editor.DetectedFormat, Is.EqualTo(expectedFormat));

                        Tuple<byte[], byte[]> result = editor.Build();
                        Assert.That(result.Item1, Is.EqualTo(data));
                    }
                    finally
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                    }
                }, CancellationToken.None);
            }
        }

        [TestCase(".bmu", nameof(ResourceType.BMU), ".bmu")]
        [TestCase(".wmv", nameof(ResourceType.WMV), ".wmv")]
        [TestCase(".xmv", nameof(ResourceType.XMV), ".xmv")]
        [TestCase(".ogg", nameof(ResourceType.OGG), ".ogg")]
        public async Task OdyToolWAV_TempPlaybackFile_UsesLoadedResourceExtensionForHolocronAudioAliases(
            string extension,
            string resourceTypeName,
            string expectedTempExtension)
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = CreateAudioHeader(extension);
                    ResourceType restype = (ResourceType)typeof(ResourceType).GetField(resourceTypeName).GetValue(null);
                    var editor = new OdyToolWAV(null, null);

                    try
                    {
                        editor.Load("test" + extension, "test", restype, data);

                        Assert.That(editor.TempFile, Is.Not.Null.And.EndsWith(expectedTempExtension));
                        Assert.That(File.Exists(editor.TempFile), Is.True);
                        Assert.That(editor.Build().Item1, Is.EqualTo(data));
                    }
                    finally
                    {
                        editor.CleanupTempFile();
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public void OdyToolWAV_GetPlaybackExtension_FallsBackToMagicBytesWhenResourceTypeIsUnknown()
        {
            byte[] mp3Data = new byte[] { (byte)'I', (byte)'D', (byte)'3', 0, 0 };

            Assert.That(OdyToolWAV.GetPlaybackExtension(mp3Data, null), Is.EqualTo(".mp3"));
        }

        private static byte[] CreateAudioHeader(string extension)
        {
            switch (extension)
            {
                case ".ogg":
                    return new byte[] { (byte)'O', (byte)'g', (byte)'g', (byte)'S', 0, 0, 0, 0 };
                case ".wma":
                    return new byte[] { 0x30, 0x26, 0xB2, 0x75, 0, 0, 0, 0 };
                case ".wmv":
                    return new byte[] { 0x30, 0x26, 0xB2, 0x75, (byte)'W', (byte)'M', (byte)'V', 0 };
                case ".xmv":
                    return new byte[] { (byte)'X', (byte)'M', (byte)'V', 0, 1, 2, 3, 4 };
                case ".flac":
                    return new byte[] { (byte)'f', (byte)'L', (byte)'a', (byte)'C', 0, 0, 0, 0 };
                case ".bmu":
                    return new byte[] { (byte)'B', (byte)'M', (byte)'U', 0, 0xFF, 0xFB, 0, 0 };
                default:
                    return new byte[] { (byte)'R', (byte)'I', (byte)'F', (byte)'F', 0, 0, 0, 0 };
            }
        }
    }
}
