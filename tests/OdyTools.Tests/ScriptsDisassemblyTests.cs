using BioWare.Common;
using BioWare.Resource.Formats.NCS;
using BioWare.Tools;
using NUnit.Framework;

namespace OdyTools.Tests
{
    [TestFixture]
    public class ScriptsDisassemblyTests
    {
        [Test]
        public void DisassembleNcsBytes_ValidCompiledScript_ReturnsOffsetLines()
        {
            NCS compiled = NCSAuto.CompileNss("void main() { }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(compiled);

            string disassembly = Scripts.DisassembleNcsBytes(bytes);

            Assert.That(disassembly, Does.Contain("; NCS Disassembly"));
            Assert.That(disassembly, Does.Contain("; Instructions:"));
            Assert.That(disassembly, Does.Match(@"\w{8}: "));
        }

        [Test]
        public void DisassembleNcsBytes_EmptyInput_ReturnsEmpty()
        {
            Assert.That(Scripts.DisassembleNcsBytes(null), Is.Empty);
            Assert.That(Scripts.DisassembleNcsBytes(new byte[0]), Is.Empty);
        }

        [Test]
        public void DisassembleNcsBytes_InvalidBytes_ReturnsErrorComment()
        {
            string disassembly = Scripts.DisassembleNcsBytes(new byte[] { 0x00, 0x01, 0x02 });

            Assert.That(disassembly, Does.StartWith("; Disassembly failed:"));
        }
    }
}
