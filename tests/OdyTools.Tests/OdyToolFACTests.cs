using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using NUnit.Framework;
using OdyTools.Editors;

namespace OdyTools.Tests
{
    public class OdyToolFACTests
    {
        private static byte[] MinimalFacBytes()
        {
            var fac = new FAC();
            fac.Factions.Add(new FACFaction { Name = "Player", IsGlobal = true });
            fac.Factions.Add(new FACFaction { Name = "Hostile", IsGlobal = true });
            fac.Reputations.Add(new FACReputation { FactionId1 = 0, FactionId2 = 1, Reputation = 50 });
            return FACHelpers.BytesFac(fac, ResourceType.FAC);
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_LoadMinimalFac_BuildsValidFac()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalFacBytes();
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    FAC roundtrip = FACHelpers.ReadFac(built);
                    Assert.That(roundtrip.Factions.Count, Is.EqualTo(2));
                    Assert.That(roundtrip.Factions[0].Name, Is.EqualTo("Player"));
                    Assert.That(roundtrip.Reputations.Count, Is.EqualTo(1));
                    Assert.That(roundtrip.Reputations[0].Reputation, Is.EqualTo(50));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_ModifyFactionName_Roundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalFacBytes();
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, data);
                    editor.Fac.Factions[0].Name = "Renamed";
                    Tuple<byte[], byte[]> result = editor.Build();
                    FAC roundtrip = FACHelpers.ReadFac(result.Item1);
                    Assert.That(roundtrip.Factions[0].Name, Is.EqualTo("Renamed"));
                }, CancellationToken.None);
            }
        }
    }
}
