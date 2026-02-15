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
    /// NSS Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolNSSTests
    {
        [Test, Timeout(90000)]
        public async Task OdyToolNSS_LoadEmpty_BuildsEmptyBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolNSS(null, null);
                    editor.Load("test.nss", "test", ResourceType.NSS, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolNSS_LoadAndBuild_PreservesSource()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    string source = "void main() { }";
                    byte[] originalData = Encoding.UTF8.GetBytes(source);

                    var editor = new OdyToolNSS(null, null);
                    editor.Load("test.nss", "test", ResourceType.NSS, originalData);

                    Tuple<byte[], byte[]> buildResult = editor.Build();
                    byte[] builtData = buildResult.Item1;
                    Assert.That(builtData, Is.Not.Null.And.Length.GreaterThan(0));

                    string decoded = Encoding.UTF8.GetString(builtData);
                    Assert.That(decoded, Is.EqualTo(source));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(120000)]
        public async Task OdyToolNSS_New_BuildsEmptyBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolNSS(null, null);
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
