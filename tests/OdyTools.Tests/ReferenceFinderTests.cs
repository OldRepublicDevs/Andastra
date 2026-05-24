using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Tools;
using NUnit.Framework;

namespace OdyTools.Tests
{
    [TestFixture]
    public class ReferenceFinderTests
    {
        [Test]
        public void FindScriptResRefInGffBytes_FindsUtcScriptField()
        {
            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_test_hb");

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            List<string> paths = ReferenceFinder.FindScriptResRefInGffBytes(bytes, "k_test_hb");

            Assert.That(paths, Is.Not.Empty);
            Assert.That(paths, Has.Some.EqualTo("ScriptHeartbeat"));
        }

        [Test]
        public void FindScriptResRefInGffBytes_EmptyNeedleReturnsEmpty()
        {
            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_test_hb");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            Assert.That(ReferenceFinder.FindScriptResRefInGffBytes(bytes, ""), Is.Empty);
            Assert.That(ReferenceFinder.FindScriptResRefInGffBytes(bytes, "   "), Is.Empty);
        }

        [Test]
        public void FindScriptResRefInGffBytes_NoMatchReturnsEmpty()
        {
            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_other");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            Assert.That(ReferenceFinder.FindScriptResRefInGffBytes(bytes, "k_test_hb"), Is.Empty);
        }
    }
}
