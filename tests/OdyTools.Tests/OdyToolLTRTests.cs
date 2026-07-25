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

        [Test]
        public async Task OdyToolLTR_GridEdits_UpdateCorrectProbabilityTables()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLTR(null, null);
                    editor.New();

                    editor.SetQuickModeForTest(0);
                    editor.SelectDoublePreviousForTest("b");
                    editor.EditDoubleRowForTest("c", 0.12f, 0.34f, 0.56f);

                    editor.SelectTriplePreviousForTest("x", "y");
                    editor.EditTripleRowForTest("z", 0.21f, 0.43f, 0.65f);

                    var result = LTRAuto.ReadLtr(editor.Build().Item1);

                    Assert.That(result.GetDoublesStart("b", "c"), Is.EqualTo(0.12f).Within(0.0001f));
                    Assert.That(result.GetDoublesMiddle("b", "c"), Is.EqualTo(0.34f).Within(0.0001f));
                    Assert.That(result.GetDoublesEnd("b", "c"), Is.EqualTo(0.56f).Within(0.0001f));
                    Assert.That(result.GetTriplesStart("x", "y", "z"), Is.EqualTo(0.21f).Within(0.0001f));
                    Assert.That(result.GetTriplesMiddle("x", "y", "z"), Is.EqualTo(0.43f).Within(0.0001f));
                    Assert.That(result.GetTriplesEnd("x", "y", "z"), Is.EqualTo(0.65f).Within(0.0001f));

                    Assert.That(result.GetSinglesStart("c"), Is.EqualTo(0f).Within(0.0001f));
                    Assert.That(result.GetSinglesMiddle("c"), Is.EqualTo(0f).Within(0.0001f));
                    Assert.That(result.GetSinglesEnd("c"), Is.EqualTo(0f).Within(0.0001f));
                    Assert.That(result.GetSinglesStart("z"), Is.EqualTo(0f).Within(0.0001f));
                    Assert.That(result.GetSinglesMiddle("z"), Is.EqualTo(0f).Within(0.0001f));
                    Assert.That(result.GetSinglesEnd("z"), Is.EqualTo(0f).Within(0.0001f));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLTR_QuickEdit_UpdatesSelectedModeAndMarksDirty()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLTR(null, null);
                    try
                    {
                        editor.New();

                        editor.SelectDoublePreviousForTest("b");
                        editor.SetQuickEditForTest(1, "c", 0.12f, 0.34f, 0.56f);
                        editor.ApplyQuickEditForTest();

                        Assert.That(editor.IsDirty, Is.True);
                        var result = LTRAuto.ReadLtr(editor.Build().Item1);
                        Assert.That(result.GetDoublesStart("b", "c"), Is.EqualTo(0.12f).Within(0.0001f));
                        Assert.That(result.GetDoublesMiddle("b", "c"), Is.EqualTo(0.34f).Within(0.0001f));
                        Assert.That(result.GetDoublesEnd("b", "c"), Is.EqualTo(0.56f).Within(0.0001f));
                        Assert.That(editor.StatusTextForTest, Does.Contain("Mode: Doubles"));
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolLTR_DistributionToolsAndGenerator_OperateOnVisibleRows()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolLTR(null, null);
                    try
                    {
                        editor.New();

                        editor.SetQuickModeForTest(0);
                        editor.SetUniformVisibleDistributionForTest();
                        var uniform = LTRAuto.ReadLtr(editor.Build().Item1);
                        float expected = 1f / LTR.NumCharacters;
                        Assert.That(uniform.GetSinglesStart("a"), Is.EqualTo(expected).Within(0.0001f));
                        Assert.That(uniform.GetSinglesMiddle("z"), Is.EqualTo(expected).Within(0.0001f));
                        Assert.That(uniform.GetSinglesEnd("-"), Is.EqualTo(expected).Within(0.0001f));

                        editor.SelectSingleRowForTest("a");
                        Assert.That(editor.ContextTextForTest, Does.Contain("a"));

                        editor.GenerateNameSamplesForTest(5);
                        Assert.That(editor.GeneratedNamesForTest, Is.Empty);
                        Assert.That(editor.StatusTextForTest, Does.Contain("Name generation failed"));

                        var valid = CreateGeneratableLtr();
                        editor.Load("valid.ltr", "valid", ResourceType.LTR, LTRAuto.BytesLtr(valid));
                        editor.GenerateNameSamplesForTest(5);
                        Assert.That(editor.GeneratedNamesForTest, Has.Count.EqualTo(5));
                        Assert.That(editor.GeneratedNamesForTest, Is.All.Not.Null);

                        editor.ClearGeneratedNamesForTest();
                        Assert.That(editor.GeneratedNamesForTest, Is.Empty);
                    }
                    finally
                    {
                        editor.Close();
                    }
                }, CancellationToken.None);
            }
        }

        private static LTR CreateGeneratableLtr()
        {
            var ltr = new LTR();
            ltr.SetSinglesStart("a", 1f);
            foreach (char c in LTR.CharacterSet)
            {
                string previous = c.ToString();
                ltr.SetDoublesStart(previous, "a", 1f);
                foreach (char c2 in LTR.CharacterSet)
                {
                    string previous2 = c2.ToString();
                    ltr.SetTriplesStart(previous, previous2, "a", 1f);
                    ltr.SetTriplesMiddle(previous, previous2, "a", 1f);
                    ltr.SetTriplesEnd(previous, previous2, "a", 1f);
                }
            }

            return ltr;
        }
    }
}
