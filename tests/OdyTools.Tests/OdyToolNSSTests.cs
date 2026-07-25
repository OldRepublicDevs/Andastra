using System;
using System.Text;
using Avalonia.Headless.NUnit;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// NSS Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    [NonParallelizable]
    public class OdyToolNSSTests
    {
        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolNSS_LoadEmpty_BuildsEmptyBytes()
        {
            var editor = new OdyToolNSS(null, null);
            editor.Load("test.nss", "test", ResourceType.NSS, null);
            Assert.That(editor.IsDirty, Is.False);
            Tuple<byte[], byte[]> result = editor.Build();
            byte[] data = result.Item1;
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Length, Is.EqualTo(0));
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolNSS_LoadAndBuild_PreservesSource()
        {
            string source = "void main() { }";
            byte[] originalData = Encoding.UTF8.GetBytes(source);

            var editor = new OdyToolNSS(null, null);
            editor.Load("test.nss", "test", ResourceType.NSS, originalData);
            Assert.That(editor.IsDirty, Is.False);

            Tuple<byte[], byte[]> buildResult = editor.Build();
            byte[] builtData = buildResult.Item1;
            Assert.That(builtData, Is.Not.Null.And.Length.GreaterThan(0));

            string decoded = Encoding.UTF8.GetString(builtData);
            Assert.That(decoded, Is.EqualTo(source));
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolNSS_EditSource_BuildsUpdatedSourceAndMarksDirty()
        {
            var editor = new OdyToolNSS(null, null);
            editor.Load("test.nss", "test", ResourceType.NSS, Encoding.UTF8.GetBytes("void main() { }"));
            Assert.That(editor.IsDirty, Is.False);

            editor.SetSourceTextForTest("void main()\n{\n    int nTest = 1;\n}");

            Assert.That(editor.IsDirty, Is.True);
            Assert.That(editor.SourceTextForTest, Does.Contain("nTest"));
            string decoded = Encoding.UTF8.GetString(editor.Build().Item1);
            Assert.That(decoded, Is.EqualTo("void main()\n{\n    int nTest = 1;\n}"));
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolNSS_AnalyzeCode_ReportsUnmatchedBracesInProblemsPanel()
        {
            var editor = new OdyToolNSS(null, null);
            editor.Load("broken.nss", "broken", ResourceType.NSS, Encoding.UTF8.GetBytes("void main()\n{\n    int nTest = 1;\n"));

            editor.AnalyzeCodeForTest();

            Assert.That(editor.DiagnosticsForTest, Has.Count.EqualTo(1));
            Assert.That(editor.DiagnosticsForTest[0].IsError, Is.True);
            Assert.That(editor.DiagnosticsForTest[0].Message, Does.Contain("Unmatched '{'"));
            Assert.That(editor.OutputTextForTest, Does.Contain("Analysis: 1 diagnostic"));
        }

        [Test, Timeout(120000)]
        [AvaloniaTest]
        public void OdyToolNSS_New_BuildsEmptyBytes()
        {
            var editor = new OdyToolNSS(null, null);
            editor.New();
            Assert.That(editor.IsDirty, Is.False);
            Tuple<byte[], byte[]> result = editor.Build();
            byte[] data = result.Item1;
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Length, Is.EqualTo(0));
        }
    }
}
