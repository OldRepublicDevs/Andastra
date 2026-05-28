using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.NCS;
using BioWare.Resource.Formats.SSF;
using BioWare.Tools;
using NUnit.Framework;
using OdyTools.Data;
using OdyTools.Utils;

namespace OdyTools.Tests
{
    [TestFixture]
    public class StrRefReferenceHelperTests
    {
        [Test]
        public void CollectStrRefReferences_OverrideOnly_FindsSsfHit()
        {
            string installRoot = CreateInstallWithStrRef(88888);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false
                };

                List<ReferenceSearchResult> results = StrRefReferenceHelper.CollectStrRefReferences(
                    88888,
                    installation,
                    options);

                Assert.That(results, Is.Not.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectStrRefReferences_NoOverride_SkipsOverrideSsf()
        {
            string installRoot = CreateInstallWithStrRef(88888);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = false,
                    SearchChitin = false,
                    SearchModules = false
                };

                List<ReferenceSearchResult> results = StrRefReferenceHelper.CollectStrRefReferences(
                    88888,
                    installation,
                    options);

                Assert.That(results, Is.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void CollectStrRefReferences_NoNcsScan_SkipsNcsHit()
        {
            const int targetStrRef = 424242;
            string installRoot = CreateInstallWithNcsStrRef(targetStrRef);
            try
            {
                var installation = new OdyInstallation(installRoot, "Test");
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false,
                    IncludeNcsStrRefScan = false
                };

                List<ReferenceSearchResult> results = StrRefReferenceHelper.CollectStrRefReferences(
                    targetStrRef,
                    installation,
                    options);

                Assert.That(results, Is.Empty);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithStrRef(int strref)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-strref-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, strref);
            byte[] bytes = SSFAuto.BytesSsf(ssf);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_set.ssf"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithNcsStrRef(int strref)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "odytools-ncs-strref-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + strref + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_script.ncs"), bytes);

            return installRoot;
        }

        private static void DeleteDirectorySafe(string path)
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
    }
}
