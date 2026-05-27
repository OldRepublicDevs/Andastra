using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.KEY;
using KotorCLI.Commands;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ExtractCommandTests
    {
        [Test]
        public void ResolveBifIndex_MatchesFilenameCaseInsensitive()
        {
            var key = new KEY();
            key.BifEntries.Add(new BifEntry { Filename = "data/models.bif" });
            key.BifEntries.Add(new BifEntry { Filename = "data/textures_01.bif" });

            int? index = ExtractCommand.ResolveBifIndex(key, "/game/data/models.bif");

            Assert.That(index, Is.EqualTo(0));
        }

        [Test]
        public void BuildBifResourceLookup_FiltersByBifIndex()
        {
            var key = new KEY();
            key.KeyEntries.Add(new KeyEntry
            {
                ResRef = new ResRef("npc001"),
                ResType = ResourceType.UTC,
                ResourceId = (uint)((1 << 20) | 3)
            });
            key.KeyEntries.Add(new KeyEntry
            {
                ResRef = new ResRef("npc002"),
                ResType = ResourceType.UTC,
                ResourceId = (uint)((0 << 20) | 3)
            });

            Dictionary<int, KeyEntry> lookup = ExtractCommand.BuildBifResourceLookup(key, 0);

            Assert.That(lookup.Count, Is.EqualTo(1));
            Assert.That(lookup[3].ResRef.ToString(), Is.EqualTo("npc002"));
        }
    }
}
