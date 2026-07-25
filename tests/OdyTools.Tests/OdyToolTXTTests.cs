using System;
using System.IO;
using System.Text;
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
    /// TXT Editor Load/Build roundtrip tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolTXTTests
    {
        [Test]
        public async Task OdyToolTXT_New_BuildsEmptyBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTXT(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTXT_LoadAndBuild_PreservesText()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string text = "Hello world\nLine two";
                    byte[] originalData = Encoding.UTF8.GetBytes(text);

                    var editor = new OdyToolTXT(null, null);
                    editor.Load("test.txt", "test", ResourceType.TXT, originalData);

                    Tuple<byte[], byte[]> buildResult = editor.Build();
                    byte[] builtData = buildResult.Item1;
                    Assert.That(builtData, Is.Not.Null.And.Length.GreaterThan(0));

                    string decoded = Encoding.UTF8.GetString(builtData);
                    Assert.That(decoded.Replace("\r\n", "\n").Replace("\r", "\n"), Is.EqualTo(text.Replace("\r\n", "\n")));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTXT_LoadEmpty_BuildsEmpty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTXT(null, null);
                    editor.Load("x.txt", "x", ResourceType.TXT, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTXT_CanLoadHolocronTextFallbackExtensions()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    foreach (string extension in new[] { ".cfg", ".log", ".2da_bak" })
                    {
                        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N") + extension);
                        File.WriteAllText(path, "plain text fallback", Encoding.UTF8);

                        try
                        {
                            var editor = new OdyToolTXT(null, null);

                            Assert.That(editor.CanLoadPath(path), Is.True);
                            Assert.That(editor.TryLoadStartupPath(path), Is.True);

                            Tuple<byte[], byte[]> result = editor.Build();
                            string decoded = Encoding.UTF8.GetString(result.Item1);
                            Assert.That(decoded, Does.Contain("plain text fallback"));
                        }
                        finally
                        {
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                        }
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTXT_FindAndReplace_RespectsCaseWholeWordAndDoesNotLoopOnPartialMatches()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTXT(null, null);
                    try
                    {
                        editor.New();
                        editor.SetTextForTest("cat scatter Cat catapult cat");

                        editor.ConfigureFindForTest("cat", matchCase: true, wholeWord: true);
                        Assert.That(editor.FindNextForTest(), Is.True);
                        Assert.That(editor.SelectionStartForTest, Is.EqualTo(0));
                        Assert.That(editor.SelectionEndForTest, Is.EqualTo(3));

                        Assert.That(editor.FindNextForTest(), Is.True);
                        Assert.That(editor.SelectionStartForTest, Is.EqualTo(25));
                        Assert.That(editor.SelectionEndForTest, Is.EqualTo(28));

                        editor.ConfigureFindForTest("cat", replace: "dog", matchCase: true, wholeWord: true);
                        editor.ReplaceAllForTest();
                        Assert.That(editor.TextForTest, Is.EqualTo("dog scatter Cat catapult dog"));

                        editor.ConfigureFindForTest("cat", matchCase: true, wholeWord: true);
                        editor.SetSelectionForTest(0, 0);
                        Assert.That(editor.FindNextForTest(), Is.False, "Whole-word search must not loop forever on scatter/catapult partial matches.");
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolTXT_ViewControls_UpdateWrapZoomStatusAndBuildOutput()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolTXT(null, null);
                    try
                    {
                        editor.Load("test.txt", "test", ResourceType.TXT, Encoding.UTF8.GetBytes("alpha\nbeta\ngamma"));
                        editor.SetSelectionForTest(8, 8);

                        Assert.That(editor.StatusLineColumnForTest, Is.EqualTo("Ln 2, Col 3"));
                        Assert.That(editor.StatusCharactersForTest, Does.Contain("16"));
                        Assert.That(editor.StatusLinesForTest, Does.Contain("3"));

                        editor.ToggleWordWrap();
                        Assert.That(editor.WordWrapForTest, Is.True);

                        double initialFontSize = editor.FontSizeForTest;
                        editor.ZoomInForTest();
                        Assert.That(editor.FontSizeForTest, Is.GreaterThan(initialFontSize));
                        Assert.That(editor.ZoomLabelForTest, Is.Not.Empty);
                        editor.ZoomResetForTest();
                        Assert.That(editor.FontSizeForTest, Is.EqualTo(initialFontSize).Within(0.001));

                        editor.ToggleStatusBarForTest();
                        Assert.That(editor.StatusBarVisibleForTest, Is.False);

                        string decoded = Encoding.UTF8.GetString(editor.Build().Item1).Replace("\r\n", "\n").Replace("\r", "\n");
                        Assert.That(decoded, Is.EqualTo("alpha\nbeta\ngamma"));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }
    }
}
