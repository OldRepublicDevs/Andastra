using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using Avalonia.Input;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.LIP;
using OdyTools.Editors;
using OdyTools.Utils;
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
        public async Task OdyToolLIP_Constructor_BuildsProgrammaticSurfaceWithoutInstallation()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLIP(null, null);

                    Assert.That(editor.HasProgrammaticEditorSurfaceForTest, Is.True);
                    Assert.That(editor.Build().Item1, Is.Not.Null.And.Length.GreaterThan(0));
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
                        Assert.That(editor.AudioPathBoxTextForTest, Is.EqualTo(wavPath));
                        Assert.That(editor.Duration, Is.EqualTo(3.5f).Within(0.01f));
                        Assert.That((double)(editor.DurationSpinValueForTest ?? 0m), Is.EqualTo(3.5d).Within(0.01d));
                        Assert.That(editor.PlayPreviewButtonEnabledForTest, Is.True);
                        Assert.That(editor.StopPreviewButtonEnabledForTest, Is.True);
                        Assert.That(editor.IsDirty, Is.True);
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

        [Test]
        public async Task OdyToolLIP_New_ClearsLoadedAudioPreviewState()
        {
            string wavPath = CreateTempWav(1.25f);
            try
            {
                using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
                {
                    await session.Dispatch(() =>
                    {
                        var editor = new OdyToolLIP(null, null);
                        editor.LoadAudioFile(wavPath);
                        Assert.That(editor.AudioFilePath, Is.EqualTo(wavPath));

                        editor.New();

                        Assert.That(editor.AudioFilePath, Is.Null);
                        Assert.That(editor.AudioPathBoxTextForTest, Is.EqualTo(string.Empty));
                        Assert.That(editor.PlayPreviewButtonEnabledForTest, Is.False);
                        Assert.That(editor.StopPreviewButtonEnabledForTest, Is.False);
                        Assert.That(editor.PreviewLabelTextForTest, Is.EqualTo("None"));
                        Assert.That(editor.Duration, Is.EqualTo(0.0f));
                        Assert.That(editor.IsDirty, Is.False);
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

        [Test]
        public async Task OdyToolLIP_Load_ClearsLoadedAudioPreviewState()
        {
            string wavPath = CreateTempWav(1.5f);
            try
            {
                using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
                {
                    await session.Dispatch(() =>
                    {
                        var editor = new OdyToolLIP(null, null);
                        editor.LoadAudioFile(wavPath);
                        Assert.That(editor.AudioFilePath, Is.EqualTo(wavPath));

                        var lip = new LIP();
                        lip.Length = 4.0f;
                        byte[] data = LIPAuto.BytesLip(lip);
                        editor.Load("loaded.lip", "loaded", ResourceType.LIP, data);

                        Assert.That(editor.AudioFilePath, Is.Null);
                        Assert.That(editor.AudioPathBoxTextForTest, Is.EqualTo(string.Empty));
                        Assert.That(editor.PlayPreviewButtonEnabledForTest, Is.False);
                        Assert.That(editor.StopPreviewButtonEnabledForTest, Is.False);
                        Assert.That(editor.Duration, Is.EqualTo(4.0f).Within(0.001f));
                        Assert.That(editor.IsDirty, Is.False);
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

        [Test]
        public async Task OdyToolLIP_LoadAudioFile_ClearsUndoHistory()
        {
            string wavPath = CreateTempWav(2.0f);
            try
            {
                using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
                {
                    await session.Dispatch(() =>
                    {
                        var editor = new OdyToolLIP(null, null);
                        editor.New();
                        editor.Duration = 5.0f;
                        editor.AddKeyframe(1.0f, LIPShape.AH);
                        Assert.That(editor.CanUndo, Is.True);

                        editor.LoadAudioFile(wavPath);
                        Assert.That(editor.CanUndo, Is.False);
                        Assert.That(editor.CanRedo, Is.False);
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

        [Test]
        public void OdyToolLIP_GetKeyframeIndexAtTime_UsesLastKeyframeBeforeTime()
        {
            var lip = new LIP();
            lip.Add(1.0f, LIPShape.AH);
            lip.Add(2.5f, LIPShape.OH);

            Assert.That(OdyToolLIP.GetKeyframeIndexAtTime(lip, 0.5f), Is.EqualTo(-1));
            Assert.That(OdyToolLIP.GetKeyframeIndexAtTime(lip, 1.0f), Is.EqualTo(0));
            Assert.That(OdyToolLIP.GetKeyframeIndexAtTime(lip, 1.5f), Is.EqualTo(0));
            Assert.That(OdyToolLIP.GetKeyframeIndexAtTime(lip, 2.5f), Is.EqualTo(1));
            Assert.That(OdyToolLIP.GetKeyframeIndexAtTime(lip, 3.0f), Is.EqualTo(1));
        }

        [Test]
        public void OdyToolLIP_GetShapeAtPlaybackTime_ReturnsDiscreteShape()
        {
            var lip = new LIP();
            lip.Add(1.0f, LIPShape.AH);
            lip.Add(2.5f, LIPShape.OH);

            Assert.That(OdyToolLIP.GetShapeAtPlaybackTime(lip, 1.2f), Is.EqualTo(LIPShape.AH));
            Assert.That(OdyToolLIP.GetShapeAtPlaybackTime(lip, 2.6f), Is.EqualTo(LIPShape.OH));
            Assert.That(OdyToolLIP.GetShapeAtPlaybackTime(lip, 0.1f), Is.Null);
        }

        [Test]
        public async Task OdyToolLIP_ScrubPreview_UpdatesShapeSelectionAndDoesNotDirtyDocument()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var lip = new LIP();
                    lip.Length = 5.0f;
                    lip.Add(1.0f, LIPShape.AH);
                    lip.Add(2.5f, LIPShape.OH);
                    byte[] data = LIPAuto.BytesLip(lip);

                    var editor = new OdyToolLIP(null, null);
                    editor.Load("scrub.lip", "scrub", ResourceType.LIP, data);
                    Assert.That(editor.IsDirty, Is.False);
                    Assert.That(editor.ScrubSliderMaximumForTest, Is.EqualTo(5.0d).Within(0.001d));
                    Assert.That(editor.ScrubSliderEnabledForTest, Is.True);

                    editor.SeekPreviewForTest(2.6f);

                    Assert.That(editor.PreviewLabelTextForTest, Is.EqualTo("OH"));
                    Assert.That(editor.SelectedKeyframeIndexForTest, Is.EqualTo(1));
                    Assert.That(editor.ScrubSliderValueForTest, Is.EqualTo(2.6d).Within(0.001d));
                    Assert.That(editor.ScrubTimeLabelTextForTest, Is.EqualTo("2.600s / 5.000s"));
                    Assert.That(editor.IsDirty, Is.False);

                    editor.SeekPreviewForTest(99.0f);
                    Assert.That(editor.ScrubSliderValueForTest, Is.EqualTo(5.0d).Within(0.001d));
                    Assert.That(editor.ScrubTimeLabelTextForTest, Is.EqualTo("5.000s / 5.000s"));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLIP_New_ResetsScrubPreviewState()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var lip = new LIP();
                    lip.Length = 3.0f;
                    lip.Add(1.0f, LIPShape.AH);

                    var editor = new OdyToolLIP(null, null);
                    editor.Load("scrub.lip", "scrub", ResourceType.LIP, LIPAuto.BytesLip(lip));
                    editor.SeekPreviewForTest(1.0f);

                    editor.New();

                    Assert.That(editor.ScrubSliderValueForTest, Is.EqualTo(0.0d));
                    Assert.That(editor.ScrubSliderMaximumForTest, Is.EqualTo(0.0d));
                    Assert.That(editor.ScrubSliderEnabledForTest, Is.False);
                    Assert.That(editor.ScrubTimeLabelTextForTest, Is.EqualTo("0.000s / 0.000s"));
                    Assert.That(editor.PreviewLabelTextForTest, Is.EqualTo("None"));
                    Assert.That(editor.SelectedKeyframeIndexForTest, Is.EqualTo(-1));
                    Assert.That(editor.IsDirty, Is.False);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLIP_EscapeShortcut_StopsPreviewAndResetsLabel()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLIP(null, null);
                    editor.SetPreviewLabelTextForTest("AH");

                    Assert.That(editor.TryHandlePlaybackShortcutForTest(Key.Escape, KeyModifiers.None), Is.True);
                    Assert.That(editor.PreviewLabelTextForTest, Is.EqualTo("None"));
                    Assert.That(editor.TryHandlePlaybackShortcutForTest(Key.Escape, KeyModifiers.Control), Is.False);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLIP_FindNext_SelectsMatchingKeyframesAndWrapsLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                int firstIndex = -1;
                decimal? firstTime = null;
                LIPShape? firstShape = null;
                int secondIndex = -1;
                int wrappedIndex = -1;

                await session.Dispatch(() =>
                {
                    var lip = new LIP();
                    lip.Length = 4.0f;
                    lip.Add(1.0f, LIPShape.AH);
                    lip.Add(2.5f, LIPShape.OH);
                    byte[] data = LIPAuto.BytesLip(lip);

                    var editor = new OdyToolLIP(null, null);
                    editor.Load("find.lip", "find", ResourceType.LIP, data);
                    editor.SetFindQueryForTest("H");

                    Assert.That(editor.FindNextForTest(), Is.True);
                    firstIndex = editor.SelectedKeyframeIndexForTest;
                    firstTime = editor.TimeSpinValueForTest;
                    firstShape = editor.ShapeComboSelectedShapeForTest;

                    Assert.That(editor.FindNextForTest(), Is.True);
                    secondIndex = editor.SelectedKeyframeIndexForTest;

                    Assert.That(editor.FindNextForTest(), Is.True);
                    wrappedIndex = editor.SelectedKeyframeIndexForTest;
                }, CancellationToken.None);

                Assert.That(firstIndex, Is.EqualTo(0));
                Assert.That(firstTime, Is.EqualTo(1.0m));
                Assert.That(firstShape, Is.EqualTo(LIPShape.AH));
                Assert.That(secondIndex, Is.EqualTo(1));
                Assert.That(wrappedIndex, Is.EqualTo(0));
            }
        }

        [Test]
        public async Task OdyToolLIP_FindNext_MatchCaseSkipsCaseMismatch()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                bool lowerCaseMatched = true;
                bool exactCaseMatched = false;
                int selectedIndex = -1;

                await session.Dispatch(() =>
                {
                    var lip = new LIP();
                    lip.Length = 2.0f;
                    lip.Add(1.0f, LIPShape.AH);

                    var editor = new OdyToolLIP(null, null);
                    editor.Load("find.lip", "find", ResourceType.LIP, LIPAuto.BytesLip(lip));

                    editor.SetFindQueryForTest("ah", matchCase: true);
                    lowerCaseMatched = editor.FindNextForTest();

                    editor.SetFindQueryForTest("AH", matchCase: true);
                    exactCaseMatched = editor.FindNextForTest();
                    selectedIndex = editor.SelectedKeyframeIndexForTest;
                }, CancellationToken.None);

                Assert.That(lowerCaseMatched, Is.False);
                Assert.That(exactCaseMatched, Is.True);
                Assert.That(selectedIndex, Is.EqualTo(0));
            }
        }

        [Test]
        public void LipHeadPreviewHelper_GetMouthStateLabel_ReturnsEmptyForNullShape()
        {
            Assert.That(LipHeadPreviewHelper.GetMouthStateLabel(null), Is.EqualTo(string.Empty));
            Assert.That(LipHeadPreviewHelper.GetMouthStateLabel(LIPShape.AH), Is.EqualTo("Mouth: AH"));
        }

        [Test]
        public void LipHeadPreviewHelper_TryPopulateAppearanceCombo_ReturnsFalseWithoutInstallation()
        {
            var combo = new Avalonia.Controls.ComboBox();
            Assert.That(LipHeadPreviewHelper.TryPopulateAppearanceCombo(null, combo), Is.False);
            Assert.That(combo.Items.Count, Is.EqualTo(0));
        }

        [Test]
        public void LipHeadPreviewHelper_FormatPlaybackOverlay_AppendsMouthHint()
        {
            Assert.That(
                LipHeadPreviewHelper.FormatPlaybackOverlay("Model: head_01", LIPShape.EE),
                Is.EqualTo("Model: head_01 | Mouth: EE"));
            Assert.That(
                LipHeadPreviewHelper.FormatPlaybackOverlay(string.Empty, LIPShape.OH),
                Is.EqualTo("Mouth: OH"));
            Assert.That(
                LipHeadPreviewHelper.FormatPlaybackOverlay("Model: head_01", null),
                Is.EqualTo("Model: head_01"));
        }

        [Test]
        public void LipHeadPreviewHelper_TryLoadHeadModel_ReturnsFalseWithoutInstallation()
        {
            byte[] mdl;
            byte[] mdx;
            string modelName;
            Assert.That(
                LipHeadPreviewHelper.TryLoadHeadModel(null, 0, out mdl, out mdx, out modelName),
                Is.False);
            Assert.That(mdl, Is.Null);
            Assert.That(mdx, Is.Null);
            Assert.That(modelName, Is.Null);
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
