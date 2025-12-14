using System;
using System.IO;
using CSharpKOTOR.Installation;
using CSharpKOTOR.Resources;
using FluentAssertions;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Tests.TestHelpers;
using Xunit;

namespace HolocronToolset.NET.Tests.Data
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_htinstallation.py
    // Original: Comprehensive tests for HTInstallation
    [Collection("Avalonia Test Collection")]
    public class HTInstallationTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public HTInstallationTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(Skip = "Requires K1_PATH environment variable and valid installation")]
        public void TestHtGetCache2DA()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_htinstallation.py
            // Original: def test_ht_get_cache_2da(self):
            string k1Path = Environment.GetEnvironmentVariable("K1_PATH");
            if (string.IsNullOrEmpty(k1Path) || !File.Exists(Path.Combine(k1Path, "chitin.key")))
            {
                return; // Skip if K1_PATH not set
            }

            var installation = new HTInstallation(k1Path, "Test");
            var twoda = installation.HtGetCache2DA("appearance");
            twoda.Should().NotBeNull();
        }

        [Fact(Skip = "Requires K1_PATH environment variable and valid installation")]
        public void TestResource()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_htinstallation.py:52-89
            // Original: def test_resource(self):
            string k1Path = Environment.GetEnvironmentVariable("K1_PATH");
            if (string.IsNullOrEmpty(k1Path) || !File.Exists(Path.Combine(k1Path, "chitin.key")))
            {
                return; // Skip if K1_PATH not set
            }

            var installation = new HTInstallation(k1Path, "Test");

            // Test resource lookup
            var result = installation.Installation.Resource("c_bantha", ResourceType.UTC, new[] { SearchLocation.CHITIN });
            result.Should().NotBeNull();
        }
    }
}
