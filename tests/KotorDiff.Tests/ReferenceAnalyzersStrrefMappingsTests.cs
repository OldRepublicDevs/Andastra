using System.Collections.Generic;
using BioWare.TSLPatcher.Mods.TLK;
using KotorDiff.Diff;
using NUnit.Framework;

namespace KotorDiff.Tests
{
    [TestFixture]
    public class ReferenceAnalyzersStrrefMappingsTests
    {
        [Test]
        public void BuildStrrefMappingsFromTlkMod_TwoModifiers_MapsModIndexToTokenId()
        {
            var tlkMod = new ModificationsTLK("append.tlk", false);
            var first = new ModifyTLK(0, false) { ModIndex = 42, Text = "first" };
            var second = new ModifyTLK(1, false) { ModIndex = 99, Text = "second" };
            tlkMod.Modifiers.Add(first);
            tlkMod.Modifiers.Add(second);

            Dictionary<int, int> mappings = ReferenceAnalyzers.BuildStrrefMappingsFromTlkMod(tlkMod);

            Assert.That(mappings.Count, Is.EqualTo(2));
            Assert.That(mappings[42], Is.EqualTo(0));
            Assert.That(mappings[99], Is.EqualTo(1));
        }

        [Test]
        public void BuildStrrefMappingsFromTlkMod_EmptyModifiers_ReturnsEmpty()
        {
            var tlkMod = new ModificationsTLK("append.tlk", false);

            Dictionary<int, int> mappings = ReferenceAnalyzers.BuildStrrefMappingsFromTlkMod(tlkMod);

            Assert.That(mappings, Is.Empty);
        }

        [Test]
        public void BuildStrrefMappingsFromTlkMod_NullMod_ReturnsEmpty()
        {
            Dictionary<int, int> mappings = ReferenceAnalyzers.BuildStrrefMappingsFromTlkMod(null);

            Assert.That(mappings, Is.Empty);
        }
    }
}
