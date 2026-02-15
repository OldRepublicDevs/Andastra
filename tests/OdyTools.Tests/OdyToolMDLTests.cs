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
    /// MDL Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolMDLTests
    {
        [Test]
        public async Task OdyToolMDL_New_BuildsValidOutput()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolMDL(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    byte[] dataExt = result.Item2;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(dataExt, Is.Not.Null);
                }, CancellationToken.None);
            }
        }
    }
}
