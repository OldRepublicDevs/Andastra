using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using OdyTools.Editors.DLG;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// DLG Editor Load/Build tests. Uses minimal valid GFF DLG data. Uses Avalonia headless session.
    /// </summary>
    public class OdyToolDLGTests
    {
        [Test]
        public async Task OdyToolDLG_LoadMinimalGff_BuildsValidGff()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var gff = new GFF(GFFContent.DLG);
                    byte[] data = GFFAuto.BytesGff(gff, ResourceType.DLG);

                    var editor = new OdyToolDLG(null, null);
                    editor.Load("test.dlg", "test", ResourceType.DLG, data);

                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    GFF loaded = GFF.FromBytes(built);
                    Assert.That(loaded.Root, Is.Not.Null);
                }, CancellationToken.None);
            }
        }
    }
}
