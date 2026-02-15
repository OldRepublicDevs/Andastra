using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.SSF;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// SSF Editor Load/Build roundtrip tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolSSFTests
    {
        [Test, Timeout(180000)]
        public async Task OdyToolSSF_LoadAndBuild_PreservesData()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var ssf = new SSF();
                    ssf.SetData(SSFSound.BATTLE_CRY_1, 100);
                    ssf.SetData(SSFSound.SELECT_1, 200);
                    ssf.SetData(SSFSound.DEAD, 300);
                    byte[] originalData = ssf.ToBytes();

                    var editor = new OdyToolSSF(null, null);
                    editor.Load("test.ssf", "test", ResourceType.SSF, originalData);

                    Tuple<byte[], byte[]> buildResult = editor.Build();
                    byte[] builtData = buildResult.Item1;
                    Assert.That(builtData, Is.Not.Null.And.Length.GreaterThan(0));

                    SSF loaded = SSF.FromBytes(builtData);
                    Assert.That(loaded.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(100));
                    Assert.That(loaded.Get(SSFSound.SELECT_1), Is.EqualTo(200));
                    Assert.That(loaded.Get(SSFSound.DEAD), Is.EqualTo(300));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(120000)]
        public async Task OdyToolSSF_New_BuildsValidSSF()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSSF(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                    SSF loaded = SSF.FromBytes(data);
                    Assert.That(loaded.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(90000)]
        public async Task OdyToolSSF_LoadEmpty_BuildsValidSSF()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSSF(null, null);
                    editor.Load("x.ssf", "x", ResourceType.SSF, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                }, CancellationToken.None);
            }
        }
    }
}
