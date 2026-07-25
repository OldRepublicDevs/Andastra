using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Interactivity;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.TPC;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// TPC Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolTPCTests
    {
        [Test]
        public void OdyToolTPC_NewTexture_BuildsValidBytes()
        {
            var tpc = TPC.FromBlank();
            byte[] data = TPCAuto.BytesTpc(tpc, ResourceType.TPC);

            Assert.That(data, Is.Not.Null);
            Assert.That(data.Length, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void OdyToolTPC_DecodeMipmapToBgra_ConvertsRgbaPixelsForPreview()
        {
            var mipmap = new TPCMipmap(1, 1, TPCTextureFormat.RGBA, new byte[] { 10, 20, 30, 40 });

            byte[] bgra = OdyToolTPC.DecodeMipmapToBgra(mipmap);

            Assert.That(bgra, Is.EqualTo(new byte[] { 30, 20, 10, 40 }));
        }

        [Test]
        public void OdyToolTPC_DecodeMipmapToBgra_ConvertsGreyscalePixelsForPreview()
        {
            var mipmap = new TPCMipmap(1, 1, TPCTextureFormat.Greyscale, new byte[] { 77 });

            byte[] bgra = OdyToolTPC.DecodeMipmapToBgra(mipmap);

            Assert.That(bgra, Is.EqualTo(new byte[] { 77, 77, 77, 255 }));
        }

        [Test]
        public void OdyToolTPC_RotateMipmapRight_SwapsDimensionsAndPixels()
        {
            var mipmap = CreateGreyscaleMipmap2By3();

            var rotated = OdyToolTPC.RotateMipmapRight(mipmap);

            Assert.That(rotated.Width, Is.EqualTo(3));
            Assert.That(rotated.Height, Is.EqualTo(2));
            Assert.That(rotated.Data, Is.EqualTo(new byte[] { 5, 3, 1, 6, 4, 2 }));
        }

        [Test]
        public void OdyToolTPC_RotateMipmapLeft_SwapsDimensionsAndPixels()
        {
            var mipmap = CreateGreyscaleMipmap2By3();

            var rotated = OdyToolTPC.RotateMipmapLeft(mipmap);

            Assert.That(rotated.Width, Is.EqualTo(3));
            Assert.That(rotated.Height, Is.EqualTo(2));
            Assert.That(rotated.Data, Is.EqualTo(new byte[] { 2, 4, 6, 1, 3, 5 }));
        }

        [Test]
        public void OdyToolTPC_FlipMipmapHorizontal_ReversesEachRow()
        {
            var mipmap = CreateGreyscaleMipmap2By3();

            var flipped = OdyToolTPC.FlipMipmapHorizontal(mipmap);

            Assert.That(flipped.Width, Is.EqualTo(2));
            Assert.That(flipped.Height, Is.EqualTo(3));
            Assert.That(flipped.Data, Is.EqualTo(new byte[] { 2, 1, 4, 3, 6, 5 }));
        }

        [Test]
        public void OdyToolTPC_FlipMipmapVertical_ReversesRowOrder()
        {
            var mipmap = CreateGreyscaleMipmap2By3();

            var flipped = OdyToolTPC.FlipMipmapVertical(mipmap);

            Assert.That(flipped.Width, Is.EqualTo(2));
            Assert.That(flipped.Height, Is.EqualTo(3));
            Assert.That(flipped.Data, Is.EqualTo(new byte[] { 5, 6, 3, 4, 1, 2 }));
        }

        [Test]
        public async Task OdyToolTPC_CanLoadHolocronDdsExtension()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var tpc = new TPC();
                    tpc.SetSingle(new byte[] { 10, 20, 30, 40 }, TPCTextureFormat.RGBA, 1, 1);
                    byte[] ddsData = TPCAuto.BytesTpc(tpc, ResourceType.DDS);

                    string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".dds");
                    File.WriteAllBytes(path, ddsData);

                    try
                    {
                        var editor = new OdyToolTPC(null, null);

                        Assert.That(editor.CanLoadPath(path), Is.True);
                        Assert.That(editor.TryLoadStartupPath(path), Is.True);
                        Assert.That(editor.TextureWidth, Is.EqualTo(1));
                        Assert.That(editor.TextureHeight, Is.EqualTo(1));
                        Assert.That(editor.TextureFormatName, Is.EqualTo(TPCTextureFormat.BGRA.ToString()));
                        Assert.That(editor.Build().Item1.Length, Is.GreaterThan(0));
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

        [Test]
        public async Task OdyToolTPC_AlphaTestControl_EditsHeaderValueLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                float initialAlpha = -1;
                decimal? initialControlValue = null;
                bool controlAvailable = false;
                bool initiallyDirty = true;
                float editedAlpha = -1;
                bool editedDirty = false;
                byte[] builtData = null;

                await session.Dispatch(() =>
                {
                    var tpc = TPC.FromBlank();
                    byte[] data = TPCAuto.BytesTpc(tpc, ResourceType.TPC);

                    var editor = new OdyToolTPC(null, null);
                    editor.Load("alpha.tpc", "alpha", ResourceType.TPC, data);

                    initialAlpha = editor.AlphaTestValue;
                    controlAvailable = editor.AlphaTestBoxForTests != null;
                    initialControlValue = editor.AlphaTestBoxForTests?.Value;
                    initiallyDirty = editor.IsDirty;

                    editor.AlphaTestBoxForTests.Value = 0.75m;

                    editedAlpha = editor.AlphaTestValue;
                    editedDirty = editor.IsDirty;
                    builtData = editor.Build().Item1;
                }, CancellationToken.None);

                Assert.That(initialAlpha, Is.EqualTo(1f).Within(0.001f));
                Assert.That(controlAvailable, Is.True);
                Assert.That(initialControlValue, Is.EqualTo(1m));
                Assert.That(initiallyDirty, Is.False);
                Assert.That(editedAlpha, Is.EqualTo(0.75f).Within(0.001f));
                Assert.That(editedDirty, Is.True);
                Assert.That(BitConverter.ToSingle(builtData, 4), Is.EqualTo(0.75f).Within(0.001f));
            }
        }

        [TestCase("png", "tpc")]
        [TestCase("jpg", "tpc")]
        [TestCase("bmp", "bmp")]
        [TestCase("tga", "tga")]
        [TestCase("dds", "dds")]
        [TestCase("tpc", "tpc")]
        public void OdyToolTPC_CommonBitmapBuildType_UsesWritableTextureFormat(string inputExtension, string expectedBuildExtension)
        {
            ResourceType inputType = ResourceType.FromExtension(inputExtension);
            ResourceType expectedBuildType = ResourceType.FromExtension(expectedBuildExtension);

            Assert.That(OdyToolTPC.GetBuildResourceType(inputType), Is.EqualTo(expectedBuildType));
        }

        [Test]
        public void OdyToolTPC_SaveAsChoices_ExposeWritableTextureFormats()
        {
            var patterns = OdyToolTPC.CreateSaveFileTypeChoices()
                .SelectMany(choice => choice.Patterns ?? Array.Empty<string>())
                .ToArray();

            Assert.That(patterns, Does.Contain("*.tpc"));
            Assert.That(patterns, Does.Contain("*.tga"));
            Assert.That(patterns, Does.Contain("*.dds"));
            Assert.That(patterns, Does.Contain("*.bmp"));
            Assert.That(patterns, Does.Not.Contain("*.png"));
            Assert.That(patterns, Does.Not.Contain("*.jpg"));
        }

        [Test]
        public async Task OdyToolTPC_CanInspectPltWithoutConvertingOriginalBytes()
        {
            byte[] pltData = CreateMinimalPlt(2, 1, new byte[] { 10, 0, 200, 4 });
            string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + ".plt");
            File.WriteAllBytes(path, pltData);

            try
            {
                using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
                {
                    await session.Dispatch(() =>
                    {
                        var editor = new OdyToolTPC(null, null);

                        Assert.That(editor.CanLoadPath(path), Is.True);
                        Assert.That(editor.TryLoadStartupPath(path), Is.True);
                        Assert.That(editor.TextureWidth, Is.EqualTo(2));
                        Assert.That(editor.TextureHeight, Is.EqualTo(1));
                        Assert.That(editor.TextureFormatName, Is.EqualTo(TPCTextureFormat.RGBA.ToString()));
                        Assert.That(editor.TxiText, Does.Contain("PLT preview"));
                        Assert.That(editor.Build().Item1, Is.EqualTo(pltData));
                    }, CancellationToken.None);
                }
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        [Test]
        public async Task OdyToolTPC_CopyPasteMenu_RoundtripsTextureThroughClipboard()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                bool copied = false;
                bool pasted = false;
                string clipboardText = "";
                int pastedWidth = 0;
                int pastedHeight = 0;
                bool pastedDirty = false;

                await session.Dispatch(async () =>
                {
                    var source = new OdyToolTPC(null, null);
                    source.Show();
                    source.Load("source.dds", "source", ResourceType.DDS, CreateTextureBytes(1, 1, 10, 20, 30, 255));
                    Assert.That(source.TextureWidth, Is.EqualTo(1));
                    Assert.That(source.TextureHeight, Is.EqualTo(1));

                    var copyItem = source.FindControl<MenuItem>("actionCopy");
                    Assert.That(copyItem, Is.Not.Null);
                    Assert.That(copyItem.Command, Is.Not.Null);
                    copied = await source.CopyTextureToClipboardAsync();

                    clipboardText = await source.Clipboard.GetTextAsync();
                    copied = copied && clipboardText != null && clipboardText.StartsWith("data:application/x-odytools-tpc;base64,", StringComparison.Ordinal);

                    var destination = new OdyToolTPC(null, null);
                    destination.Show();
                    destination.Load("destination.dds", "destination", ResourceType.DDS, CreateTextureBytes(1, 1, 200, 210, 220, 255));
                    Assert.That(destination.TextureWidth, Is.EqualTo(1));

                    var pasteItem = destination.FindControl<MenuItem>("actionPaste");
                    Assert.That(pasteItem, Is.Not.Null);
                    Assert.That(pasteItem.Command, Is.Not.Null);
                    pasted = await destination.PasteTextureFromClipboardAsync();

                    pasted = pasted && destination.TextureWidth == 1 && destination.TextureHeight == 1;
                    pastedWidth = destination.TextureWidth;
                    pastedHeight = destination.TextureHeight;
                    pastedDirty = destination.IsDirty;
                    source.Close();
                    destination.Close();
                }, CancellationToken.None);

                Assert.That(copied, Is.True);
                Assert.That(clipboardText, Does.Contain("base64"));
                Assert.That(pasted, Is.True);
                Assert.That(pastedWidth, Is.EqualTo(1));
                Assert.That(pastedHeight, Is.EqualTo(1));
                Assert.That(pastedDirty, Is.True);
            }
        }

        [Test]
        public async Task OdyToolTPC_PasteMenu_IgnoresNonTextureClipboardText()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                int width = 0;
                bool dirty = true;

                await session.Dispatch(async () =>
                {
                    var editor = new OdyToolTPC(null, null);
                    editor.Show();
                    editor.Load("texture.dds", "texture", ResourceType.DDS, CreateTextureBytes(1, 1, 1, 2, 3, 255));
                    Assert.That(editor.TextureWidth, Is.EqualTo(1));
                    await editor.Clipboard.SetTextAsync("plain text");

                    var pasteItem = editor.FindControl<MenuItem>("actionPaste");
                    Assert.That(pasteItem, Is.Not.Null);
                    Assert.That(pasteItem.Command, Is.Not.Null);
                    Assert.That(await editor.PasteTextureFromClipboardAsync(), Is.False);

                    width = editor.TextureWidth;
                    dirty = editor.IsDirty;
                    editor.Close();
                }, CancellationToken.None);

                Assert.That(width, Is.EqualTo(1));
                Assert.That(dirty, Is.False);
            }
        }

        private static byte[] CreateMinimalPlt(int width, int height, byte[] pixelPairs)
        {
            using (var stream = new MemoryStream())
            using (var writer = new System.IO.BinaryWriter(stream))
            {
                writer.Write(new byte[] { (byte)'P', (byte)'L', (byte)'T', (byte)' ' });
                writer.Write(new byte[] { (byte)'V', (byte)'1', (byte)' ', (byte)' ' });
                writer.Write(0u);
                writer.Write(0u);
                writer.Write((uint)width);
                writer.Write((uint)height);
                writer.Write(pixelPairs);
                return stream.ToArray();
            }
        }

        private static TPCMipmap CreateGreyscaleMipmap2By3()
        {
            return new TPCMipmap(2, 3, TPCTextureFormat.Greyscale, new byte[] { 1, 2, 3, 4, 5, 6 });
        }

        private static byte[] CreateTextureBytes(int width, int height, byte r, byte g, byte b, byte a)
        {
            var pixels = new byte[width * height * 4];
            for (int offset = 0; offset < pixels.Length; offset += 4)
            {
                pixels[offset] = r;
                pixels[offset + 1] = g;
                pixels[offset + 2] = b;
                pixels[offset + 3] = a;
            }

            var tpc = new TPC();
            tpc.SetSingle(pixels, TPCTextureFormat.RGBA, width, height);
            return TPCAuto.BytesTpc(tpc, ResourceType.DDS);
        }

    }
}
