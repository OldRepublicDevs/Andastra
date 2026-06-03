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

        [Test]
        public void Execute_JsonOutput_SsfHit_IncludesStrRefMetadata()
        {
            const int targetStrRef = 77777;
            string installRoot = CreateInstallWithStrRef(targetStrRef);
            var output = new System.IO.StringWriter();
            var originalOut = Console.Out;
            try
            {
                Console.SetOut(output);
                var logger = new StandardLogger(noColor: true);
                int exitCode = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: null,
                    jsonOutput: true,
                    countOnly: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
                string text = output.ToString();
                Assert.That(text, Does.Contain("\"needle\":\"77777\""));
                Assert.That(text, Does.Contain("\"type\":\"strref\""));
                Assert.That(text, Does.Contain("\"count\":1"));
            }
            finally
            {
                Console.SetOut(originalOut);
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_JsonOutput_NoMatch_EmitsEmptyArray()
        {
            string installRoot = CreateInstallWithStrRef(77777);
            var output = new System.IO.StringWriter();
            var originalOut = Console.Out;
            try
            {
                Console.SetOut(output);
                var logger = new StandardLogger(noColor: true);
                int exitCode = FindStrRefCommand.Execute(
                    99999,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: null,
                    jsonOutput: true,
                    countOnly: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(1));
                string text = output.ToString();
                Assert.That(text, Does.Contain("\"count\":0"));
                Assert.That(text, Does.Contain("\"references\":[]"));
            }
            finally
            {
                Console.SetOut(originalOut);
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_CountOnly_SsfHit_PrintsOne()
        {
            const int targetStrRef = 77777;
            string installRoot = CreateInstallWithStrRef(targetStrRef);
            var output = new System.IO.StringWriter();
            var originalOut = Console.Out;
            try
            {
                Console.SetOut(output);
                var logger = new StandardLogger(noColor: true);
                int exitCode = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: null,
                    jsonOutput: false,
                    countOnly: true,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(output.ToString().Trim(), Is.EqualTo("1"));
            }
            finally
            {
                Console.SetOut(originalOut);
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_CacheFile_SecondRunUsesSavedCacheWithoutRescan()
        {
            const int targetStrRef = 77777;
            string installRoot = CreateInstallWithStrRef(targetStrRef);
            string cacheFile = Path.Combine(Path.GetTempPath(), "kotorcli-strref-cache-" + Guid.NewGuid().ToString("N") + ".json");

            try
            {
                var logger = new StandardLogger();
                int firstExit = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: true,
                    ncsStrRefMin: null,
                    jsonOutput: false,
                    countOnly: false,
                    moduleGlobFilters: null,
                    cacheFilePath: cacheFile,
                    rebuildCache: false,
                    logger);

                Assert.That(firstExit, Is.EqualTo(0));
                Assert.That(File.Exists(cacheFile), Is.True);
                DateTime firstWrite = File.GetLastWriteTimeUtc(cacheFile);

                System.Threading.Thread.Sleep(1100);

                int secondExit = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: true,
                    ncsStrRefMin: null,
                    jsonOutput: false,
                    countOnly: false,
                    moduleGlobFilters: null,
                    cacheFilePath: cacheFile,
                    rebuildCache: false,
                    logger);

                Assert.That(secondExit, Is.EqualTo(0));
                Assert.That(File.GetLastWriteTimeUtc(cacheFile), Is.EqualTo(firstWrite));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
                if (File.Exists(cacheFile))
                {
                    File.Delete(cacheFile);
                }
            }
        }

        [Test]
        public void Execute_NcsDeadReturnLocalStrRef_CachePath_ExitsNonZero()
        {
            const int targetStrRef = 424242;
            string installRoot = CreateInstallWithNcsDeadReturnLocalStrRef(targetStrRef);
            string cacheFile = Path.Combine(Path.GetTempPath(), "kotorcli-ncs-deadret-cache-" + Guid.NewGuid().ToString("N") + ".json");
            try
            {
                var logger = new StandardLogger();
                int buildExit = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: null,
                    jsonOutput: false,
                    countOnly: false,
                    moduleGlobFilters: null,
                    cacheFilePath: cacheFile,
                    rebuildCache: true,
                    logger);
                Assert.That(buildExit, Is.EqualTo(1));

                int cachedExit = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: null,
                    jsonOutput: false,
                    countOnly: false,
                    moduleGlobFilters: null,
                    cacheFilePath: cacheFile,
                    rebuildCache: false,
                    logger);
                Assert.That(cachedExit, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
                if (File.Exists(cacheFile))
                {
                    File.Delete(cacheFile);
                }
            }
        }

        [Test]
        public void Execute_NcsEarlyReturnLiveLocalStrRef_CachePath_ExitsZero()
        {
            const int targetStrRef = 424242;
            string installRoot = CreateInstallWithNcsEarlyReturnLiveLocalStrRef(targetStrRef);
            string cacheFile = Path.Combine(Path.GetTempPath(), "kotorcli-ncs-live-cache-" + Guid.NewGuid().ToString("N") + ".json");
            try
            {
                var logger = new StandardLogger();
                int buildExit = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: null,
                    jsonOutput: false,
                    countOnly: false,
                    moduleGlobFilters: null,
                    cacheFilePath: cacheFile,
                    rebuildCache: true,
                    logger);
                Assert.That(buildExit, Is.EqualTo(0));

                int cachedExit = FindStrRefCommand.Execute(
                    targetStrRef,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    noNcs: false,
                    ncsStrRefMin: null,
                    jsonOutput: false,
                    countOnly: false,
                    moduleGlobFilters: null,
                    cacheFilePath: cacheFile,
                    rebuildCache: false,
                    logger);
                Assert.That(cachedExit, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
                if (File.Exists(cacheFile))
                {
                    File.Delete(cacheFile);
                }
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

        private static string CreateInstallWithNcsDeadReturnLocalStrRef(int strref)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-ncs-deadret-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + strref + ";\n    if (1) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_script.ncs"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithNcsEarlyReturnLiveLocalStrRef(int strref)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-ncs-live-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + strref + ";\n    if (0) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
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
