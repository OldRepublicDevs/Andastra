using System;
using System.IO;
using BioWare.Resource;
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
