using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.SSF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Tools;
using KotorDiff.Diff;
using NUnit.Framework;

namespace KotorDiff.Tests
{
    [TestFixture]
    public class ReferenceAnalyzersInstallationTests
    {
        [Test]
        public void CollectInstallationStrRefResources_FindsOverrideSsf()
        {
            const int targetStrRef = 424242;
            string installRoot = CreateStubInstallRoot();
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(
                Path.Combine(overrideDir, "test_set.ssf"),
                CreateSsfBytesWithStrRef(targetStrRef));

            try
            {
                var installation = new Installation(installRoot);
                HashSet<FileResource> found = ReferenceAnalyzers.CollectInstallationStrRefResources(
                    installation,
                    targetStrRef);

                Assert.That(found, Is.Not.Empty);
                Assert.That(found, Has.Some.Matches<FileResource>(
                    r => string.Equals(r.ResName, "test_set", StringComparison.OrdinalIgnoreCase)
                         && r.ResType == ResourceType.SSF));
            }
            finally
            {
                TryDeleteDirectory(installRoot);
            }
        }

        [Test]
        public void CollectInstallationStrRefResources_WithBuiltCache_FindsOverrideSsf()
        {
            const int targetStrRef = 424242;
            string installRoot = CreateStubInstallRoot();
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(
                Path.Combine(overrideDir, "test_set.ssf"),
                CreateSsfBytesWithStrRef(targetStrRef));

            try
            {
                var installation = new Installation(installRoot);
                StrRefReferenceCache cache = ReferenceCacheHelpers.BuildStrRefReferenceCache(installation);
                HashSet<FileResource> found = ReferenceAnalyzers.CollectInstallationStrRefResources(
                    installation,
                    targetStrRef,
                    cache);

                Assert.That(found, Is.Not.Empty);
                Assert.That(found, Has.Some.Matches<FileResource>(
                    r => string.Equals(r.ResName, "test_set", StringComparison.OrdinalIgnoreCase)
                         && r.ResType == ResourceType.SSF));
            }
            finally
            {
                TryDeleteDirectory(installRoot);
            }
        }

        [Test]
        public void CollectInstallationStrRefResources_NoMatch_ReturnsEmpty()
        {
            string installRoot = CreateStubInstallRoot();
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(
                Path.Combine(overrideDir, "test_set.ssf"),
                CreateSsfBytesWithStrRef(111111));

            try
            {
                var installation = new Installation(installRoot);
                HashSet<FileResource> found = ReferenceAnalyzers.CollectInstallationStrRefResources(
                    installation,
                    999999);

                Assert.That(found, Is.Empty);
            }
            finally
            {
                TryDeleteDirectory(installRoot);
            }
        }

        [Test]
        public void CollectInstallationGffResources_IncludesOverrideUtc()
        {
            string installRoot = CreateStubInstallRoot();
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(
                Path.Combine(overrideDir, "test_npc.utc"),
                CreateUtcBytes());

            try
            {
                var installation = new Installation(installRoot);
                List<FileResource> gffResources = ReferenceAnalyzers.CollectInstallationGffResources(installation);

                Assert.That(gffResources, Is.Not.Empty);
                Assert.That(gffResources, Has.Some.Matches<FileResource>(
                    r => string.Equals(r.ResName, "test_npc", StringComparison.OrdinalIgnoreCase)
                         && r.ResType == ResourceType.UTC));
            }
            finally
            {
                TryDeleteDirectory(installRoot);
            }
        }

        [Test]
        public void CollectInstallationGffResources_NoGff_ReturnsEmpty()
        {
            string installRoot = CreateStubInstallRoot();
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(
                Path.Combine(overrideDir, "test_set.ssf"),
                CreateSsfBytesWithStrRef(424242));

            try
            {
                var installation = new Installation(installRoot);
                List<FileResource> gffResources = ReferenceAnalyzers.CollectInstallationGffResources(installation);

                Assert.That(gffResources, Is.Empty);
            }
            finally
            {
                TryDeleteDirectory(installRoot);
            }
        }

        private static string CreateStubInstallRoot()
        {
            string installRoot = Path.Combine(
                Path.GetTempPath(),
                "kotordiff-install-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);
            return installRoot;
        }

        private static void TryDeleteDirectory(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch
            {
                // Best-effort cleanup.
            }
        }

        private static byte[] CreateSsfBytesWithStrRef(int strRef)
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, strRef);
            return SSFAuto.BytesSsf(ssf);
        }

        private static byte[] CreateUtcBytes()
        {
            var utc = new UTC();
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            return GFFAuto.BytesGff(gff, ResourceType.UTC);
        }
    }
}
