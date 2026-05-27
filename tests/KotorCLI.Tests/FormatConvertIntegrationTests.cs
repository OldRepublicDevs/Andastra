using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.SSF;
using BioWare.Resource.Formats.TwoDA;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class FormatConvertIntegrationTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

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

                using (var buildProcess = Process.Start(buildPsi))
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

            using (var process = Process.Start(psi))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit(120000);
                return process.ExitCode;
            }
        }

        [Test]
        public void Gff2Json_MinimalGff_WritesJsonFile()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-gff2json-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string gffPath = Path.Combine(tempDir, "sample.gff");
            string jsonPath = Path.Combine(tempDir, "sample.json");

            try
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "test");
                File.WriteAllBytes(gffPath, GFFAuto.BytesGff(gff, ResourceType.GFF));

                int exitCode = RunKotorCli("gff2json \"" + gffPath + "\" --output \"" + jsonPath + "\"", out _, out string stderr);

                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(jsonPath), Is.True);
                Assert.That(new FileInfo(jsonPath).Length, Is.GreaterThan(0));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void Json2Gff_AfterGff2Json_WritesGffFile()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-json2gff-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string gffPath = Path.Combine(tempDir, "sample.gff");
            string jsonPath = Path.Combine(tempDir, "sample.json");
            string roundTripPath = Path.Combine(tempDir, "roundtrip.gff");

            try
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "test");
                File.WriteAllBytes(gffPath, GFFAuto.BytesGff(gff, ResourceType.GFF));

                int jsonExit = RunKotorCli("gff2json \"" + gffPath + "\" --output \"" + jsonPath + "\"", out _, out string jsonErr);
                Assert.That(jsonExit, Is.EqualTo(0), jsonErr);
                Assert.That(File.Exists(jsonPath), Is.True);

                int gffExit = RunKotorCli("json2gff \"" + jsonPath + "\" --output \"" + roundTripPath + "\"", out _, out string gffErr);
                Assert.That(gffExit, Is.EqualTo(0), gffErr);
                Assert.That(File.Exists(roundTripPath), Is.True);
                Assert.That(new FileInfo(roundTripPath).Length, Is.GreaterThan(0));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void Json2Gff_AfterGff2Json_PreservesLabelField()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-json2gff-label-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string gffPath = Path.Combine(tempDir, "sample.gff");
            string jsonPath = Path.Combine(tempDir, "sample.json");
            string roundTripPath = Path.Combine(tempDir, "roundtrip.gff");

            try
            {
                const string label = "integration-label";
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", label);
                File.WriteAllBytes(gffPath, GFFAuto.BytesGff(gff, ResourceType.GFF));

                int jsonExit = RunKotorCli("gff2json \"" + gffPath + "\" --output \"" + jsonPath + "\"", out _, out string jsonErr);
                Assert.That(jsonExit, Is.EqualTo(0), jsonErr);

                int gffExit = RunKotorCli("json2gff \"" + jsonPath + "\" --output \"" + roundTripPath + "\"", out _, out string gffErr);
                Assert.That(gffExit, Is.EqualTo(0), gffErr);

                byte[] roundTripBytes = File.ReadAllBytes(roundTripPath);
                GFF roundTrip = GFFAuto.ReadGff(roundTripBytes, 0, roundTripBytes.Length, ResourceType.GFF);
                Assert.That(roundTrip.Root.GetString("Label"), Is.EqualTo(label));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void Json2Gff_MissingInput_ExitsNonZero()
        {
            int exitCode = RunKotorCli("json2gff \"" + Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".json") + "\"", out _, out _);
            Assert.That(exitCode, Is.Not.EqualTo(0));
        }

        [Test]
        public void TwoDa2Csv_MinimalTwoDA_WritesCsvFile()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-2da2csv-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string twoDaPath = Path.Combine(tempDir, "sample.2da");
            string csvPath = Path.Combine(tempDir, "sample.csv");

            try
            {
                WriteSampleTwoDA(twoDaPath);

                int exitCode = RunKotorCli("2da2csv \"" + twoDaPath + "\" --output \"" + csvPath + "\"", out _, out string stderr);

                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(csvPath), Is.True);
                Assert.That(new FileInfo(csvPath).Length, Is.GreaterThan(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Csv22Da_AfterTwoDa2Csv_PreservesRowLabel()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-csv22da-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string twoDaPath = Path.Combine(tempDir, "sample.2da");
            string csvPath = Path.Combine(tempDir, "sample.csv");
            string roundTripPath = Path.Combine(tempDir, "roundtrip.2da");
            const string rowLabel = "integration-row";

            try
            {
                WriteSampleTwoDA(twoDaPath, rowLabel);

                int csvExit = RunKotorCli("2da2csv \"" + twoDaPath + "\" --output \"" + csvPath + "\"", out _, out string csvErr);
                Assert.That(csvExit, Is.EqualTo(0), csvErr);
                Assert.That(File.Exists(csvPath), Is.True);

                int twoDaExit = RunKotorCli("csv22da \"" + csvPath + "\" --output \"" + roundTripPath + "\"", out _, out string twoDaErr);
                Assert.That(twoDaExit, Is.EqualTo(0), twoDaErr);
                Assert.That(File.Exists(roundTripPath), Is.True);

                byte[] roundTripBytes = File.ReadAllBytes(roundTripPath);
                TwoDA roundTrip = TwoDAAuto.Read2DA(roundTripBytes, 0, roundTripBytes.Length, ResourceType.TwoDA);
                Assert.That(roundTrip.GetLabel(0), Is.EqualTo(rowLabel));
                Assert.That(roundTrip.GetCellString(0, "label"), Is.EqualTo("value"));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Ssf2Xml_MinimalSsf_WritesXmlFile()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-ssf2xml-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string ssfPath = Path.Combine(tempDir, "sample.ssf");
            string xmlPath = Path.Combine(tempDir, "sample.ssf.xml");

            try
            {
                WriteSampleSsf(ssfPath, 424242);

                int exitCode = RunKotorCli("ssf2xml \"" + ssfPath + "\" --output \"" + xmlPath + "\"", out _, out string stderr);

                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(xmlPath), Is.True);
                Assert.That(new FileInfo(xmlPath).Length, Is.GreaterThan(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Xml2Ssf_AfterSsf2Xml_PreservesBattleCryStrRef()
        {
            const int targetStrRef = 424242;
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-xml2ssf-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string ssfPath = Path.Combine(tempDir, "sample.ssf");
            string xmlPath = Path.Combine(tempDir, "sample.ssf.xml");
            string roundTripPath = Path.Combine(tempDir, "roundtrip.ssf");

            try
            {
                WriteSampleSsf(ssfPath, targetStrRef);

                int xmlExit = RunKotorCli("ssf2xml \"" + ssfPath + "\" --output \"" + xmlPath + "\"", out _, out string xmlErr);
                Assert.That(xmlExit, Is.EqualTo(0), xmlErr);
                Assert.That(File.Exists(xmlPath), Is.True);

                int ssfExit = RunKotorCli("xml2ssf \"" + xmlPath + "\" --output \"" + roundTripPath + "\"", out _, out string ssfErr);
                Assert.That(ssfExit, Is.EqualTo(0), ssfErr);
                Assert.That(File.Exists(roundTripPath), Is.True);

                byte[] roundTripBytes = File.ReadAllBytes(roundTripPath);
                SSF roundTrip = SSFAuto.ReadSsf(roundTripBytes, 0, roundTripBytes.Length, ResourceType.SSF);
                Assert.That(roundTrip.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(targetStrRef));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        private static void WriteSampleTwoDA(string path, string rowLabel = "integration-row")
        {
            var twoDA = new TwoDA(new List<string> { "label" });
            twoDA.AddRow(rowLabel, new Dictionary<string, object> { { "label", "value" } });
            TwoDAAuto.Write2DA(twoDA, path, ResourceType.TwoDA);
        }

        private static void WriteSampleSsf(string path, int battleCryStrRef)
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, battleCryStrRef);
            File.WriteAllBytes(path, SSFAuto.BytesSsf(ssf));
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
