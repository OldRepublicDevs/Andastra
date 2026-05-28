using System;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.NCS;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class FindRefsCommandCliTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [Test]
        public void Cli_FindRefs_Script_InOverride_ExitsZero()
        {
            RunCliHitTest("k_cli_sub", "script", CreateInstallWithScriptReference("k_cli_sub"));
        }

        [Test]
        public void Cli_FindRefs_Script_CompiledNcsInOverride_ExitsZero()
        {
            const string targetResRef = "k_cli_ncs_target";
            RunCliHitTest(targetResRef, "script", CreateInstallWithCompiledNcsScriptReference(targetResRef));
        }

        [Test]
        public void Cli_FindRefs_Tag_InOverride_ExitsZero()
        {
            RunCliHitTest("tag_cli_sub", "tag", CreateInstallWithTagReference("tag_cli_sub"));
        }

        [Test]
        public void Cli_FindRefs_Template_InOverride_ExitsZero()
        {
            RunCliHitTest("tpl_cli_sub", "template", CreateInstallWithTemplateReference("tpl_cli_sub"));
        }

        [Test]
        public void Cli_FindRefs_Conversation_InOverride_ExitsZero()
        {
            RunCliHitTest("dlg_cli_sub", "conversation", CreateInstallWithConversationReference("dlg_cli_sub"));
        }

        [Test]
        public void Cli_FindRefs_NoMatch_ExitsNonZero()
        {
            string installRoot = CreateInstallWithScriptReference("k_cli_sub");
            try
            {
                int exitCode = RunKotorCli(
                    "find-refs missing_script --installation \"" + installRoot + "\" --type script --override-only --no-chitin --no-modules",
                    out string stdout,
                    out string stderr);

                Assert.That(exitCode, Is.Not.EqualTo(0), stdout + stderr);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static void RunCliHitTest(string needle, string type, string installRoot)
        {
            try
            {
                int exitCode = RunKotorCli(
                    "find-refs " + needle + " --installation \"" + installRoot + "\" --type " + type + " --override-only --no-chitin --no-modules",
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(needle).IgnoreCase);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static int RunKotorCli(string arguments, out string stdout, out string stderr)
        {
            string cliDll = Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "bin", "Debug", "net9.0", "KotorCLI.dll");
            if (!File.Exists(cliDll))
            {
                var buildPsi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "build \"" + Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "KotorCLI.csproj") + "\" --framework net9.0",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = RepoRoot
                };

                using (Process buildProcess = Process.Start(buildPsi))
                {
                    buildProcess.WaitForExit(120000);
                    Assert.That(buildProcess.ExitCode, Is.EqualTo(0), "KotorCLI build failed before integration test.");
                }
            }

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "exec \"" + cliDll + "\" " + arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = RepoRoot
            };

            using (Process process = Process.Start(psi))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit(120000);
                return process.ExitCode;
            }
        }

        private static string CreateInstallWithScriptReference(string scriptResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-cli-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.OnHeartbeat = new ResRef(scriptResRef);
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithCompiledNcsScriptReference(string scriptResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-ncs-cli-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            NCS ncs = NCSAuto.CompileNss(
                "void main() { ExecuteScript(\"" + scriptResRef + "\", OBJECT_SELF); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);
            File.WriteAllBytes(Path.Combine(overrideDir, "caller_script.ncs"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithTemplateReference(string templateResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-cli-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.ResRef = new ResRef(templateResRef);
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithConversationReference(string conversationResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-cli-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Conversation = new ResRef(conversationResRef);
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithTagReference(string tag)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-cli-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = tag;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

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
