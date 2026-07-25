using System.Linq;
using NUnit.Framework;
using BioWare.Resource.Formats.GFF.Generics;

namespace BioWare.Tests
{
    public class PTHRoundtripTests
    {
        [Test]
        public void PTH_Roundtrip_PreservesBidirectionalConnections()
        {
            var pth = new PTH();
            int first = pth.Add(10, 20);
            int second = pth.Add(30, 40);
            pth.Connect(first, second);
            pth.Connect(second, first);

            byte[] data = PTHAuto.BytesPth(pth);
            PTH loaded = PTHAuto.ReadPth(data);

            Assert.That(loaded.Count, Is.EqualTo(2));
            Assert.That(loaded.GetConnections().Count, Is.EqualTo(2));
            Assert.That(loaded.GetConnections().Any(edge => edge.SourceIndex == first && edge.TargetIndex == second), Is.True);
            Assert.That(loaded.GetConnections().Any(edge => edge.SourceIndex == second && edge.TargetIndex == first), Is.True);
        }
    }
}
