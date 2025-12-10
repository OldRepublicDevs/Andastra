using NUnit.Framework;
using CSharpKOTOR.Resources;

namespace CSharpKOTOR.Tests
{
    [TestFixture]
    public class ResourceTypeTests
    {
        [Test]
        public void TestResourceTypeFromExtension()
        {
            // Test common resource type extensions
            ResourceType gitType = ResourceType.FromExtension(".git");
            Assert.IsNotNull(gitType);
            Assert.AreEqual("GIT", gitType.Name);

            ResourceType tlkType = ResourceType.FromExtension(".tlk");
            Assert.IsNotNull(tlkType);
            Assert.AreEqual("TLK", tlkType.Name);

            ResourceType erfType = ResourceType.FromExtension(".erf");
            Assert.IsNotNull(erfType);
            Assert.AreEqual("ERF", erfType.Name);

            ResourceType rimType = ResourceType.FromExtension(".rim");
            Assert.IsNotNull(rimType);
            Assert.AreEqual("RIM", rimType.Name);

            ResourceType ncsType = ResourceType.FromExtension(".ncs");
            Assert.IsNotNull(ncsType);
            Assert.AreEqual("NCS", ncsType.Name);

            ResourceType gffType = ResourceType.FromExtension(".utc");
            Assert.IsNotNull(gffType);
            Assert.AreEqual("GFF", gffType.Name);

            // Test case insensitivity
            ResourceType upperCase = ResourceType.FromExtension(".GIT");
            Assert.AreEqual(gitType, upperCase);
        }

        [Test]
        public void TestResourceTypeFromId()
        {
            // Test getting resource types by ID
            ResourceType gitType = ResourceType.FromId(2015); // GIT type ID
            Assert.IsNotNull(gitType);
            Assert.AreEqual("GIT", gitType.Name);

            ResourceType tlkType = ResourceType.FromId(2018); // TLK type ID
            Assert.IsNotNull(tlkType);
            Assert.AreEqual("TLK", tlkType.Name);
        }

        [Test]
        public void TestResourceTypeProperties()
        {
            ResourceType gitType = ResourceType.FromExtension(".git");
            Assert.IsNotNull(gitType);

            Assert.AreEqual(2015, gitType.TypeId);
            Assert.AreEqual(".git", gitType.Extension);
            Assert.AreEqual("Dynamic Area", gitType.Contents);
            Assert.AreEqual("Area", gitType.Category);
        }

        [Test]
        public void TestInvalidResourceType()
        {
            // Test invalid extension
            ResourceType invalidType = ResourceType.FromExtension(".invalid");
            Assert.IsNull(invalidType);

            // Test invalid ID
            ResourceType invalidId = ResourceType.FromId(-1);
            Assert.IsNull(invalidId);

            // Test invalid large ID
            ResourceType invalidLargeId = ResourceType.FromId(99999);
            Assert.IsNull(invalidLargeId);
        }
    }
}
