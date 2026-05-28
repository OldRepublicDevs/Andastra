using System;
using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource.Formats.NCS;
using BioWare.Tools;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class NcsConstStringScannerTests
    {
        [Test]
        public void ExtractConstsInstructions_CompiledExecuteScript_FindsResRefLiteral()
        {
            const string targetResRef = "k_target_hb";
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ExecuteScript(\"" + targetResRef + "\", OBJECT_SELF); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstStringScanner.ConstsInstruction> instructions =
                NcsConstStringScanner.ExtractConstsInstructions(bytes);

            Assert.That(
                instructions,
                Has.Some.Matches<NcsConstStringScanner.ConstsInstruction>(i => i.Value == targetResRef));
            Assert.That(
                instructions,
                Has.Some.Matches<NcsConstStringScanner.ConstsInstruction>(i => i.StringByteOffset >= 0));
        }

        [Test]
        public void ExtractConstsInstructions_NullOrEmptyInput_ReturnsEmpty()
        {
            Assert.That(NcsConstStringScanner.ExtractConstsInstructions(null), Is.Empty);
            Assert.That(NcsConstStringScanner.ExtractConstsInstructions(new byte[0]), Is.Empty);
        }

        [Test]
        public void ExtractConstsInstructions_InvalidHeader_ReturnsEmpty()
        {
            Assert.That(
                NcsConstStringScanner.ExtractConstsInstructions(new byte[] { 0x00, 0x01, 0x02 }),
                Is.Empty);
        }

        [Test]
        public void ExtractConstsOffsetsForValue_FindsExactMatch()
        {
            const string targetResRef = "k_target_hb";
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ExecuteScript(\"" + targetResRef + "\", OBJECT_SELF); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<int> offsets = NcsConstStringScanner.ExtractConstsOffsetsForValue(bytes, targetResRef);

            Assert.That(offsets, Is.Not.Empty);
            Assert.That(NcsConstStringScanner.ExtractConstsOffsetsForValue(bytes, "missing"), Is.Empty);
        }

        [Test]
        public void FindScriptResRefInNcsBytes_CompiledExecuteScript_UsesNcsBytecodePath()
        {
            const string targetResRef = "k_target_hb";
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ExecuteScript(\"" + targetResRef + "\", OBJECT_SELF); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<string> paths = ReferenceFinder.FindScriptResRefInNcsBytes(bytes, targetResRef);

            Assert.That(paths, Is.Not.Empty);
            Assert.That(paths, Has.All.StartsWith("(NCS bytecode) offset_"));
        }
    }
}
