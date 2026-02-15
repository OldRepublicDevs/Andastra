using BioWare.Resource.Formats.SSF;
using NUnit.Framework;

namespace BioWare.Tests
{
    /// <summary>
    /// SSF serialization roundtrip tests. Validates that SSF -> bytes -> SSF preserves data.
    /// </summary>
    public class SSFRoundtripTests
    {
        [Test]
        public void SSF_Roundtrip_PreservesSoundEntries()
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, 100);
            ssf.SetData(SSFSound.SELECT_1, 200);
            ssf.SetData(SSFSound.DEAD, 300);

            byte[] data = ssf.ToBytes();
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));

            SSF loaded = SSF.FromBytes(data);
            Assert.That(loaded.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(100));
            Assert.That(loaded.Get(SSFSound.SELECT_1), Is.EqualTo(200));
            Assert.That(loaded.Get(SSFSound.DEAD), Is.EqualTo(300));
        }

        [Test]
        public void SSF_Roundtrip_DefaultValues()
        {
            var ssf = new SSF();
            byte[] data = ssf.ToBytes();
            SSF loaded = SSF.FromBytes(data);
            Assert.That(loaded, Is.EqualTo(ssf));
        }
    }
}
