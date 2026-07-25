using System;
using BioWare.Resource.Formats.LTR;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class LTRGenerationTests
    {
        [Test]
        public void Generate_WithEmptyDistribution_FailsInsteadOfLoopingForever()
        {
            var ltr = new LTR();

            Assert.Throws<InvalidOperationException>(() => ltr.Generate(seed: 1234));
        }
    }
}
