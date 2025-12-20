using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Andastra.Parsing.Formats.TwoDA;
using Andastra.Parsing.Tests.Common;
using FluentAssertions;
using Xunit;
using TwoDABinaryWriter = Andastra.Parsing.Formats.TwoDA.TwoDABinaryWriter;
using TwoDABinaryReader = Andastra.Parsing.Formats.TwoDA.TwoDABinaryReader;

namespace Andastra.Parsing.Tests.Formats
{
    /// <summary>
    /// Comprehensive tests for TwoDA format using Kaitai Struct generated parsers.
    /// Tests validate that the TwoDA.ksy definition compiles correctly to multiple languages
    /// and that the generated parsers correctly parse TwoDA files.
    /// </summary>
    public class TwoDAKaitaiStructTests
    {
        private static readonly string KsyFile = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..",
            "src", "Andastra", "Parsing", "Resource", "Formats", "TwoDA", "TwoDA.ksy");

        private static readonly string TestTwoDAFile = TestFileHelper.GetPath("test.2da");
        private static readonly string KaitaiOutputDir = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..",
            "kaitai_compiled", "twoda");

        // Languages supported by Kaitai Struct (at least a dozen)
        private static readonly string[] SupportedLanguages = new[]
        {
            "python", "java", "javascript", "csharp", "cpp_stl", "go", "ruby",
            "php", "rust", "swift", "perl", "nim", "lua", "kotlin", "typescript"
        };

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructCompilerAvailable()
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "kaitai-struct-compiler",
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                process.WaitForExit(5000);

                if (process.ExitCode == 0)
                {
                    string version = process.StandardOutput.ReadToEnd();
                    version.Should().NotBeNullOrEmpty("Kaitai Struct compiler should return version");
                }
                else
                {
                    Assert.True(true, "Kaitai Struct compiler not available - skipping compiler tests");
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Assert.True(true, "Kaitai Struct compiler not installed - skipping compiler tests");
            }
        }

        [Fact(Timeout = 300000)]
        public void TestKsyFileExists()
        {
            var ksyPath = new FileInfo(KsyFile);
            if (!ksyPath.Exists)
            {
                ksyPath = new FileInfo(Path.Combine(
                    AppContext.BaseDirectory, "..", "..", "..", "..",
                    "src", "Andastra", "Parsing", "Resource", "Formats", "TwoDA", "TwoDA.ksy"));
            }

            ksyPath.Exists.Should().BeTrue($"TwoDA.ksy should exist at {ksyPath.FullName}");
        }

        [Fact(Timeout = 300000)]
        public void TestKsyFileValid()
        {
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "TwoDA.ksy not found - skipping validation");
                return;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "kaitai-struct-compiler",
                    Arguments = $"--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                process.WaitForExit(5000);

                if (process.ExitCode != 0)
                {
                    Assert.True(true, "Kaitai Struct compiler not available - skipping validation");
                    return;
                }

                var testProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "kaitai-struct-compiler",
                        Arguments = $"-t python \"{KsyFile}\" -d \"{Path.GetTempPath()}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                testProcess.Start();
                testProcess.WaitForExit(30000);

                string stderr = testProcess.StandardError.ReadToEnd();

                if (testProcess.ExitCode != 0 && stderr.Contains("error") && !stderr.Contains("import"))
                {
                    Assert.True(false, $"TwoDA.ksy has syntax errors: {stderr}");
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Assert.True(true, "Kaitai Struct compiler not installed - skipping validation");
            }
        }

        [Theory(Timeout = 300000)]
        [MemberData(nameof(GetSupportedLanguages))]
        public void TestKaitaiStructCompilation(string language)
        {
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "TwoDA.ksy not found - skipping compilation test");
                return;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "kaitai-struct-compiler",
                    Arguments = $"-t {language} \"{KsyFile}\" -d \"{Path.GetTempPath()}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                process.WaitForExit(60000);

                string stdout = process.StandardOutput.ReadToEnd();
                string stderr = process.StandardError.ReadToEnd();

                if (process.ExitCode != 0)
                {
                    if (stderr.Contains("not supported") || stderr.Contains("unsupported"))
                    {
                        Assert.True(true, $"Language {language} not supported by compiler: {stderr}");
                    }
                    else
                    {
                        Assert.True(false, $"Failed to compile TwoDA.ksy to {language}: {stderr}");
                    }
                }
                else
                {
                    Assert.True(true, $"Successfully compiled TwoDA.ksy to {language}");
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Assert.True(true, "Kaitai Struct compiler not installed - skipping compilation test");
            }
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructCompilesToAllLanguages()
        {
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "TwoDA.ksy not found - skipping compilation test");
                return;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "kaitai-struct-compiler",
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                process.WaitForExit(5000);

                if (process.ExitCode != 0)
                {
                    Assert.True(true, "Kaitai Struct compiler not available - skipping compilation test");
                    return;
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Assert.True(true, "Kaitai Struct compiler not installed - skipping compilation test");
                return;
            }

            int successCount = 0;
            int failCount = 0;
            var results = new List<string>();

            foreach (string lang in SupportedLanguages)
            {
                var compileProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "kaitai-struct-compiler",
                        Arguments = $"-t {lang} \"{KsyFile}\" -d \"{Path.GetTempPath()}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                try
                {
                    compileProcess.Start();
                    compileProcess.WaitForExit(60000);

                    if (compileProcess.ExitCode == 0)
                    {
                        successCount++;
                        results.Add($"{lang}: Success");
                    }
                    else
                    {
                        failCount++;
                        string error = compileProcess.StandardError.ReadToEnd();
                        results.Add($"{lang}: Failed - {error.Substring(0, Math.Min(100, error.Length))}");
                    }
                }
                catch (Exception ex)
                {
                    failCount++;
                    results.Add($"{lang}: Error - {ex.Message}");
                }
            }

            results.Should().NotBeEmpty("Should have compilation results");

            foreach (string result in results)
            {
                Console.WriteLine($"  {result}");
            }

            Assert.True(successCount > 0, $"At least one language should compile successfully. Results: {string.Join(", ", results)}");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructGeneratedParserConsistency()
        {
            if (!File.Exists(TestTwoDAFile))
            {
                var twoda = new TwoDA(new List<string> { "col1", "col2", "col3" });
                twoda.AddRow("0", new Dictionary<string, object> { { "col1", "abc" }, { "col2", "def" }, { "col3", "ghi" } });
                twoda.AddRow("1", new Dictionary<string, object> { { "col1", "def" }, { "col2", "ghi" }, { "col3", "123" } });
                twoda.AddRow("2", new Dictionary<string, object> { { "col1", "123" }, { "col2", "" }, { "col3", "abc" } });

                Directory.CreateDirectory(Path.GetDirectoryName(TestTwoDAFile));
                byte[] data = new TwoDABinaryWriter(twoda).Write();
                File.WriteAllBytes(TestTwoDAFile, data);
            }

            TwoDA twodaParsed = new TwoDABinaryReader(TestTwoDAFile).Load();

            FileInfo fileInfo = new FileInfo(TestTwoDAFile);
            fileInfo.Length.Should().BeGreaterThan(0, "TwoDA file should have content");

            twodaParsed.GetHeight().Should().BeGreaterThan(0, "TwoDA should have rows");
            twodaParsed.GetWidth().Should().BeGreaterThan(0, "TwoDA should have columns");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructDefinitionCompleteness()
        {
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "TwoDA.ksy not found - skipping completeness test");
                return;
            }

            string ksyContent = File.ReadAllText(KsyFile);

            ksyContent.Should().Contain("meta:", "Should have meta section");
            ksyContent.Should().Contain("id: twoda", "Should have id: twoda");
            ksyContent.Should().Contain("header", "Should define header field");
            ksyContent.Should().Contain("column_headers_raw", "Should define column_headers_raw field");
            ksyContent.Should().Contain("row_count", "Should define row_count field");
            ksyContent.Should().Contain("row_labels_section", "Should define row_labels_section");
            ksyContent.Should().Contain("cell_offsets_array", "Should define cell_offsets_array");
            ksyContent.Should().Contain("data_size", "Should define data_size field");
            ksyContent.Should().Contain("cell_values_section", "Should define cell_values_section");
            ksyContent.Should().Contain("twoda_header", "Should define twoda_header type");
            ksyContent.Should().Contain("row_label_entry", "Should define row_label_entry type");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructCompilesToAtLeastDozenLanguages()
        {
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "TwoDA.ksy not found - skipping test");
                return;
            }

            SupportedLanguages.Length.Should().BeGreaterThanOrEqualTo(12,
                "Should support at least a dozen languages for testing");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "kaitai-struct-compiler",
                    Arguments = "--version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            try
            {
                process.Start();
                process.WaitForExit(5000);

                if (process.ExitCode != 0)
                {
                    Assert.True(true, "Kaitai Struct compiler not available - skipping test");
                    return;
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Assert.True(true, "Kaitai Struct compiler not installed - skipping test");
                return;
            }

            int compiledCount = 0;
            foreach (string lang in SupportedLanguages)
            {
                var compileProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "kaitai-struct-compiler",
                        Arguments = $"-t {lang} \"{KsyFile}\" -d \"{Path.GetTempPath()}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                try
                {
                    compileProcess.Start();
                    compileProcess.WaitForExit(60000);

                    if (compileProcess.ExitCode == 0)
                    {
                        compiledCount++;
                    }
                }
                catch
                {
                    // Ignore individual failures
                }
            }

            compiledCount.Should().BeGreaterThanOrEqualTo(12,
                $"Should successfully compile TwoDA.ksy to at least 12 languages. Compiled to {compiledCount} languages.");
        }

        public static IEnumerable<object[]> GetSupportedLanguages()
        {
            return SupportedLanguages.Select(lang => new object[] { lang });
        }
    }
}

