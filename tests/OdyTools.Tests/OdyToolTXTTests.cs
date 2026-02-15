using System;
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
    }
}
