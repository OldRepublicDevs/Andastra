using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.NCS;
using BioWare.Resource.Formats.SSF;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class FindStrRefCommandTests
    {
        [Test]
        public void Execute_StrRefInOverrideSsf_ExitsZero()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(77777, installRoot, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NoMatch_ExitsNonZero()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(99999, installRoot, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NegativeStrRef_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = FindStrRefCommand.Execute(-1, Path.GetTempPath(), logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void Execute_NoOverride_SkipsOverrideSsf_ExitsNonZero()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    77777,
                    installRoot,
                    overrideOnly: false,
                    noOverride: true,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_OverrideOnly_FindsOverrideSsf_ExitsZero()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    77777,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_OverrideOnly_FindsOverrideSsf()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    77777,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NoOverride_SkipsOverrideSsf()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    77777,
                    installRoot,
                    overrideOnly: false,
                    noOverride: true,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NcsOnly_FindsWithoutNoNcsFlag()
        {
            string installRoot = CreateInstallWithNcsStrRef(424242);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    424242,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NcsOnly_SkipsWithNoNcsFlag()
        {
            string installRoot = CreateInstallWithNcsStrRef(424242);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    424242,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NoNcs_StillFindsSsfHit()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    77777,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_SlowPathSmallConsti_FoundWithHighMinThreshold()
        {
            const int smallStrRef = 50;
            string installRoot = CreateInstallWithNcsStrRef(smallStrRef);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    smallStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: 100,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NcsStrRefMin_Negative_ReturnsError()
        {
            string installRoot = CreateInstallWithNcsStrRef(424242);
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindStrRefCommand.Execute(
                    424242,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: -1,
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithStrRef(int strref)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-strref-" + Guid.NewGuid().ToString("N"));
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
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-ncs-strref-" + Guid.NewGuid().ToString("N"));
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
