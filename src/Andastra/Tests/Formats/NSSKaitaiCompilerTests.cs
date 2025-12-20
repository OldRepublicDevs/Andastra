using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Andastra.Parsing.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Andastra.Parsing.Tests.Formats
{
    /// <summary>
    /// Comprehensive tests for NSS.ksy Kaitai Struct compiler functionality.
    /// Tests compile NSS.ksy to multiple languages and validate the generated parsers work correctly.
    ///
    /// Supported languages tested:
    /// - Python, Java, JavaScript, C#, C++, Ruby, PHP, Go, Rust, Perl, Lua, Nim, VisualBasic, Swift, Kotlin, TypeScript
    /// </summary>
    public class NSSKaitaiCompilerTests
    {
        private static readonly string NssKsyPath = Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "..", "..", "..", "..", "src", "Andastra", "Parsing", "Resource", "Formats", "NSS", "NSS.ksy");

        private static readonly string TestNssFile = TestFileHelper.GetPath("test.nss");
        private static readonly string CompilerOutputDir = Path.Combine(Path.GetTempPath(), "kaitai_nss_tests");

        // Supported Kaitai Struct target languages (at least a dozen)
        private static readonly string[] SupportedLanguages = new[]
        {
            "python",
            "java",
            "javascript",
            "csharp",
            "cpp_stl",
            "ruby",
            "php",
            "go",
            "rust",
            "perl",
            "lua",
            "nim",
            "visualbasic",
            "swift",
            "kotlin",
            "typescript"
        };

        static NSSKaitaiCompilerTests()
        {
            // Normalize NSS.ksy path
            NssKsyPath = Path.GetFullPath(NssKsyPath);

            // Create output directory
            if (!Directory.Exists(CompilerOutputDir))
            {
                Directory.CreateDirectory(CompilerOutputDir);
            }
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructCompilerAvailable()
        {
            string compilerPath = FindKaitaiCompiler();
            compilerPath.Should().NotBeNullOrEmpty("kaitai-struct-compiler should be available in PATH or common locations");

            var processInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = "--version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                if (process != null)
                {
                    process.WaitForExit(10000);
                    process.ExitCode.Should().Be(0, "kaitai-struct-compiler should execute successfully");
                }
            }
        }

        [Fact(Timeout = 300000)]
        public void TestNssKsyFileExists()
        {
            File.Exists(NssKsyPath).Should().BeTrue($"NSS.ksy should exist at {NssKsyPath}");

            string content = File.ReadAllText(NssKsyPath);
            content.Should().Contain("meta:", "NSS.ksy should contain meta section");
            content.Should().Contain("id: nss", "NSS.ksy should have id: nss");
            content.Should().Contain("file-extension:", "NSS.ksy should specify file extensions");
            content.Should().Contain("nss", "NSS.ksy should include nss extension");
        }

        [Theory(Timeout = 300000)]
        [MemberData(nameof(GetSupportedLanguages))]
        public void TestCompileNssKsyToLanguage(string language)
        {
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip test if compiler not available
            }

            string langOutputDir = Path.Combine(CompilerOutputDir, language);
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            var processInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t {language} \"{NssKsyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(NssKsyPath)
            };

            string stdout = "";
            string stderr = "";
            int exitCode = -1;

            using (var process = Process.Start(processInfo))
            {
                if (process != null)
                {
                    stdout = process.StandardOutput.ReadToEnd();
                    stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(60000);
                    exitCode = process.ExitCode;
                }
            }

            exitCode.Should().Be(0,
                $"kaitai-struct-compiler should compile NSS.ksy to {language} successfully. " +
                $"STDOUT: {stdout}, STDERR: {stderr}");

            string[] generatedFiles = Directory.GetFiles(langOutputDir, "*", SearchOption.AllDirectories);
            generatedFiles.Should().NotBeEmpty($"Compilation to {language} should generate output files");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileNssKsyToAllLanguages()
        {
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            var results = new Dictionary<string, bool>();
            var errors = new Dictionary<string, string>();

            foreach (string language in SupportedLanguages)
            {
                try
                {
                    string langOutputDir = Path.Combine(CompilerOutputDir, language);
                    if (Directory.Exists(langOutputDir))
                    {
                        Directory.Delete(langOutputDir, true);
                    }
                    Directory.CreateDirectory(langOutputDir);

                    var processInfo = new ProcessStartInfo
                    {
                        FileName = compilerPath,
                        Arguments = $"-t {language} \"{NssKsyPath}\" -d \"{langOutputDir}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = Path.GetDirectoryName(NssKsyPath)
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        if (process != null)
                        {
                            string stdout = process.StandardOutput.ReadToEnd();
                            string stderr = process.StandardError.ReadToEnd();
                            process.WaitForExit(60000);

                            bool success = process.ExitCode == 0;
                            results[language] = success;

                            if (!success)
                            {
                                errors[language] = $"Exit code: {process.ExitCode}, STDOUT: {stdout}, STDERR: {stderr}";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    results[language] = false;
                    errors[language] = ex.Message;
                }
            }

            int successCount = results.Values.Count(r => r);
            int totalCount = SupportedLanguages.Length;

            // At least 12 languages should compile successfully
            successCount.Should().BeGreaterOrEqualTo(12,
                $"At least 12 languages should compile successfully. " +
                $"Results: {string.Join(", ", results.Select(kvp => $"{kvp.Key}: {(kvp.Value ? "OK" : "FAIL")}"))}. " +
                $"Errors: {string.Join("; ", errors.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}");
        }

        [Fact(Timeout = 300000)]
        public void TestNssKsySyntaxValidation()
        {
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            var validateInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t python \"{NssKsyPath}\" --debug",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(NssKsyPath)
            };

            using (var process = Process.Start(validateInfo))
            {
                if (process != null)
                {
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(30000);

                    stderr.Should().NotContain("error", "NSS.ksy should not have syntax errors");
                    process.ExitCode.Should().Be(0,
                        $"NSS.ksy syntax should be valid. STDOUT: {stdout}, STDERR: {stderr}");
                }
            }
        }

        [Fact(Timeout = 300000)]
        public void TestNssKsyDefinitionCompleteness()
        {
            if (!File.Exists(NssKsyPath))
            {
                return; // Skip if file doesn't exist
            }

            string ksyContent = File.ReadAllText(NssKsyPath);

            // Check for required elements in Kaitai Struct definition
            ksyContent.Should().Contain("meta:", "Should have meta section");
            ksyContent.Should().Contain("id: nss", "Should have id: nss");
            ksyContent.Should().Contain("file-extension:", "Should define file extensions");
            ksyContent.Should().Contain("nss", "Should include nss extension");

            // Check for NSS format structure (text format)
            ksyContent.Should().Contain("source_code", "Should define source_code field");
            ksyContent.Should().Contain("encoding:", "Should specify encoding (UTF-8)");
            ksyContent.Should().Contain("size-eos:", "Should use size-eos for text content");
        }

        [Fact(Timeout = 300000)]
        public void TestNssKsyTextFormatStructure()
        {
            if (!File.Exists(NssKsyPath))
            {
                return; // Skip if file doesn't exist
            }

            string ksyContent = File.ReadAllText(NssKsyPath);

            // Check for text format structure
            ksyContent.Should().Contain("encoding: UTF-8", "Should use UTF-8 encoding");
            ksyContent.Should().Contain("bom", "Should handle optional BOM");
            ksyContent.Should().Contain("source_code", "Should define source_code field");
            ksyContent.Should().Contain("str", "Should use str type for text content");
        }

        public static IEnumerable<object[]> GetSupportedLanguages()
        {
            return SupportedLanguages.Select(lang => new object[] { lang });
        }

        private static string FindKaitaiCompiler()
        {
            string[] possiblePaths = new[]
            {
                "kaitai-struct-compiler",
                "ksc",
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "kaitai-struct-compiler", "kaitai-struct-compiler.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "kaitai-struct-compiler", "kaitai-struct-compiler.exe"),
                "/usr/bin/kaitai-struct-compiler",
                "/usr/local/bin/kaitai-struct-compiler",
                "C:\\Program Files\\kaitai-struct-compiler\\kaitai-struct-compiler.exe"
            };

            foreach (string path in possiblePaths)
            {
                try
                {
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = "--version",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        if (process != null)
                        {
                            process.WaitForExit(5000);
                            if (process.ExitCode == 0)
                            {
                                return path;
                            }
                        }
                    }
                }
                catch
                {
                    // Continue searching
                }
            }

            return null;
        }
    }
}

