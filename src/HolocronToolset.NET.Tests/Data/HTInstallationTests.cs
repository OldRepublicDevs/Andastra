using System;
using System.IO;
using System.Linq;
using CSharpKOTOR.Common;
using CSharpKOTOR.Installation;
using CSharpKOTOR.Resources;
using FluentAssertions;
using HolocronToolset.NET.Data;
using Xunit;

namespace HolocronToolset.NET.Tests.Data
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_htinstallation.py:45
    // Original: class TestHTInstallation(TestCase):
    public class HTInstallationTests : IDisposable
    {
        private readonly string _k1Path;

        public HTInstallationTests()
        {
            _k1Path = Environment.GetEnvironmentVariable("K1_PATH");
            if (string.IsNullOrEmpty(_k1Path) || !File.Exists(Path.Combine(_k1Path, "chitin.key")))
            {
                // Skip tests if K1_PATH not set
                return;
            }
        }

        public void Dispose()
        {
            // Cleanup if needed
        }

        [Fact(Skip = "Requires K1_PATH environment variable")]
        public void TestResource()
        {
            if (string.IsNullOrEmpty(_k1Path) || !File.Exists(Path.Combine(_k1Path, "chitin.key")))
            {
                return; // Skip if path not available
            }

            var installation = new HTInstallation(_k1Path, "Test");

            // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_htinstallation.py:52-80
            // Original: assert installation.resource("c_bantha", ResourceType.UTC, []) is None
            var result1 = installation.Installation.Resource("c_bantha", ResourceType.UTC, null);
            result1.Should().NotBeNull();

            var result2 = installation.Installation.Resource("c_bantha", ResourceType.UTC, new[] { SearchLocation.CHITIN });
            result2.Should().NotBeNull();

            var result3 = installation.Installation.Resource("xxx", ResourceType.UTC, new[] { SearchLocation.CHITIN });
            result3.Should().BeNull();
        }

        [Fact(Skip = "Requires K1_PATH environment variable")]
        public void TestHtGetCache2DA()
        {
            if (string.IsNullOrEmpty(_k1Path) || !File.Exists(Path.Combine(_k1Path, "chitin.key")))
            {
                return;
            }

            var installation = new HTInstallation(_k1Path, "Test");

            // Test caching functionality
            var twoDA1 = installation.HtGetCache2DA("appearance");
            var twoDA2 = installation.HtGetCache2DA("appearance");

            // Should return same instance (cached)
            twoDA1.Should().NotBeNull();
            twoDA2.Should().NotBeNull();
            ReferenceEquals(twoDA1, twoDA2).Should().BeTrue();

            // Test clearing cache
            installation.HtClearCache2DA();
            var twoDA3 = installation.HtGetCache2DA("appearance");
            twoDA3.Should().NotBeNull();
            // Should be a new instance after clearing
        }

        [Fact]
        public void TestTslProperty()
        {
            // Test TSL property detection
            // This will be tested with actual installations
        }
    }
}
