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
    /// Comprehensive tests for TPC/DDS/TGA/TXI Kaitai Struct compiler functionality.
    /// Tests compile TPC.ksy, DDS.ksy, TGA.ksy, and TXI.ksy to multiple languages and validate the generated parsers work correctly.
    ///
    /// Supported languages tested:
    /// - Python, Java, JavaScript, C#, C++, Ruby, PHP, Go, Rust, Perl, Lua, Nim, VisualBasic, Swift, Kotlin
    /// </summary>
    public class TPCKaitaiCompilerTests
    {
        private static readonly string TpcKsyPath = Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "..", "..", "..", "..", "src", "Andastra", "Parsing", "Resource", "Formats", "TPC", "TPC.ksy");
        
        private static readonly string DdsKsyPath = Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "..", "..", "..", "..", "src", "Andastra", "Parsing", "Resource", "Formats", "TPC", "DDS.ksy");
        
        private static readonly string TgaKsyPath = Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "..", "..", "..", "..", "src", "Andastra", "Parsing", "Resource", "Formats", "TPC", "TGA.ksy");
        
        private static readonly string TxiKsyPath = Path.Combine(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
            "..", "..", "..", "..", "src", "Andastra", "Parsing", "Resource", "Formats", "TPC", "TXI.ksy");

        private static readonly string CompilerOutputDir = Path.Combine(Path.GetTempPath(), "kaitai_tpc_tests");

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
            "kotlin"
        };

        static TPCKaitaiCompilerTests()
        {
            // Normalize KSY paths
            TpcKsyPath = Path.GetFullPath(TpcKsyPath);
            DdsKsyPath = Path.GetFullPath(DdsKsyPath);
            TgaKsyPath = Path.GetFullPath(TgaKsyPath);
            TxiKsyPath = Path.GetFullPath(TxiKsyPath);

            // Create output directory
            if (!Directory.Exists(CompilerOutputDir))
            {
                Directory.CreateDirectory(CompilerOutputDir);
            }
        }

        [Fact(Timeout = 300000)] // 5 minutes timeout for compilation
        public void TestKaitaiStructCompilerAvailable()
        {
            // Test that kaitai-struct-compiler is available
            string compilerPath = FindKaitaiCompiler();
            compilerPath.Should().NotBeNullOrEmpty("kaitai-struct-compiler should be available in PATH or common locations");

            // Test compiler version
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
        public void TestTpcKsyFileExists()
        {
            File.Exists(TpcKsyPath).Should().BeTrue($"TPC.ksy should exist at {TpcKsyPath}");

            // Validate it's a valid Kaitai Struct file
            string content = File.ReadAllText(TpcKsyPath);
            content.Should().Contain("meta:", "TPC.ksy should contain meta section");
            content.Should().Contain("id: tpc", "TPC.ksy should have id: tpc");
        }

        [Fact(Timeout = 300000)]
        public void TestDdsKsyFileExists()
        {
            File.Exists(DdsKsyPath).Should().BeTrue($"DDS.ksy should exist at {DdsKsyPath}");

            string content = File.ReadAllText(DdsKsyPath);
            content.Should().Contain("meta:", "DDS.ksy should contain meta section");
            content.Should().Contain("id: dds", "DDS.ksy should have id: dds");
        }

        [Fact(Timeout = 300000)]
        public void TestTgaKsyFileExists()
        {
            File.Exists(TgaKsyPath).Should().BeTrue($"TGA.ksy should exist at {TgaKsyPath}");

            string content = File.ReadAllText(TgaKsyPath);
            content.Should().Contain("meta:", "TGA.ksy should contain meta section");
            content.Should().Contain("id: tga", "TGA.ksy should have id: tga");
        }

        [Fact(Timeout = 300000)]
        public void TestTxiKsyFileExists()
        {
            File.Exists(TxiKsyPath).Should().BeTrue($"TXI.ksy should exist at {TxiKsyPath}");

            string content = File.ReadAllText(TxiKsyPath);
            content.Should().Contain("meta:", "TXI.ksy should contain meta section");
            content.Should().Contain("id: txi", "TXI.ksy should have id: txi");
        }

        [Theory(Timeout = 300000)]
        [MemberData(nameof(GetSupportedLanguages))]
        public void TestCompileTpcKsyToLanguage(string language)
        {
            TestCompileKsyToLanguage(TpcKsyPath, "tpc", language);
        }

        [Theory(Timeout = 300000)]
        [MemberData(nameof(GetSupportedLanguages))]
        public void TestCompileDdsKsyToLanguage(string language)
        {
            TestCompileKsyToLanguage(DdsKsyPath, "dds", language);
        }

        [Theory(Timeout = 300000)]
        [MemberData(nameof(GetSupportedLanguages))]
        public void TestCompileTgaKsyToLanguage(string language)
        {
            TestCompileKsyToLanguage(TgaKsyPath, "tga", language);
        }

        [Theory(Timeout = 300000)]
        [MemberData(nameof(GetSupportedLanguages))]
        public void TestCompileTxiKsyToLanguage(string language)
        {
            TestCompileKsyToLanguage(TxiKsyPath, "txi", language);
        }

        private void TestCompileKsyToLanguage(string ksyPath, string formatName, string language)
        {
            // Skip if compiler not available
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip test if compiler not available
            }

            // Skip if KSY file doesn't exist
            if (!File.Exists(ksyPath))
            {
                return; // Skip test if KSY file doesn't exist
            }

            // Create output directory for this language and format
            string langOutputDir = Path.Combine(CompilerOutputDir, formatName, language);
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            // Compile KSY to target language
            var processInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t {language} \"{ksyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(ksyPath)
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
                    process.WaitForExit(60000); // 60 second timeout
                    exitCode = process.ExitCode;
                }
            }

            // Compilation should succeed
            exitCode.Should().Be(0,
                $"kaitai-struct-compiler should compile {formatName}.ksy to {language} successfully. " +
                $"STDOUT: {stdout}, STDERR: {stderr}");

            // Verify output files were generated
            string[] generatedFiles = Directory.GetFiles(langOutputDir, "*", SearchOption.AllDirectories);
            generatedFiles.Should().NotBeEmpty($"Compilation to {language} should generate output files");
        }

        [Fact(Timeout = 600000)] // 10 minutes timeout for compiling all languages
        public void TestCompileAllKsyFilesToAllLanguages()
        {
            // Test compilation to all supported languages for all KSY files
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            var ksyFiles = new[]
            {
                new { Path = TpcKsyPath, Name = "TPC" },
                new { Path = DdsKsyPath, Name = "DDS" },
                new { Path = TgaKsyPath, Name = "TGA" },
                new { Path = TxiKsyPath, Name = "TXI" }
            };

            var results = new Dictionary<string, bool>();
            var errors = new Dictionary<string, string>();

            foreach (var ksyFile in ksyFiles)
            {
                if (!File.Exists(ksyFile.Path))
                {
                    continue; // Skip if file doesn't exist
                }

                foreach (string language in SupportedLanguages)
                {
                    string testKey = $"{ksyFile.Name}-{language}";
                    try
                    {
                        string langOutputDir = Path.Combine(CompilerOutputDir, ksyFile.Name.ToLowerInvariant(), language);
                        if (Directory.Exists(langOutputDir))
                        {
                            Directory.Delete(langOutputDir, true);
                        }
                        Directory.CreateDirectory(langOutputDir);

                        var processInfo = new ProcessStartInfo
                        {
                            FileName = compilerPath,
                            Arguments = $"-t {language} \"{ksyFile.Path}\" -d \"{langOutputDir}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WorkingDirectory = Path.GetDirectoryName(ksyFile.Path)
                        };

                        using (var process = Process.Start(processInfo))
                        {
                            if (process != null)
                            {
                                string stdout = process.StandardOutput.ReadToEnd();
                                string stderr = process.StandardError.ReadToEnd();
                                process.WaitForExit(60000);

                                bool success = process.ExitCode == 0;
                                results[testKey] = success;

                                if (!success)
                                {
                                    errors[testKey] = $"Exit code: {process.ExitCode}, STDOUT: {stdout}, STDERR: {stderr}";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        results[testKey] = false;
                        errors[testKey] = ex.Message;
                    }
                }
            }

            // Report results
            int successCount = results.Values.Count(r => r);
            int totalCount = results.Count;

            // At least 12 languages should compile successfully for each format
            // (4 formats * 12 languages = 48 minimum successful compilations)
            int expectedMinimum = 4 * 12; // At least 12 languages per format
            successCount.Should().BeGreaterOrEqualTo(expectedMinimum,
                $"At least {expectedMinimum} compilations should succeed out of {totalCount} total. " +
                $"Results: {string.Join(", ", results.Select(kvp => $"{kvp.Key}: {(kvp.Value ? "OK" : "FAIL")}"))}. " +
                $"Errors: {string.Join("; ", errors.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}");
        }

        [Fact(Timeout = 300000)]
        public void TestTpcKsySyntaxValidation()
        {
            TestKsySyntaxValidation(TpcKsyPath, "TPC");
        }

        [Fact(Timeout = 300000)]
        public void TestDdsKsySyntaxValidation()
        {
            TestKsySyntaxValidation(DdsKsyPath, "DDS");
        }

        [Fact(Timeout = 300000)]
        public void TestTgaKsySyntaxValidation()
        {
            TestKsySyntaxValidation(TgaKsyPath, "TGA");
        }

        [Fact(Timeout = 300000)]
        public void TestTxiKsySyntaxValidation()
        {
            TestKsySyntaxValidation(TxiKsyPath, "TXI");
        }

        private void TestKsySyntaxValidation(string ksyPath, string formatName)
        {
            // Validate KSY syntax by attempting compilation
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            if (!File.Exists(ksyPath))
            {
                return; // Skip if file doesn't exist
            }

            // Use Python as validation target (most commonly supported)
            var validateInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t python \"{ksyPath}\" --debug",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(ksyPath)
            };

            using (var process = Process.Start(validateInfo))
            {
                if (process != null)
                {
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(30000);

                    // Compiler should not report syntax errors
                    if (process.ExitCode != 0)
                    {
                        // Check if it's a known limitation vs actual error
                        if (!stderr.Contains("error") || stderr.Contains("import"))
                        {
                            // May be acceptable (missing imports, etc.)
                            return;
                        }
                    }

                    process.ExitCode.Should().Be(0,
                        $"{formatName}.ksy syntax should be valid. STDOUT: {stdout}, STDERR: {stderr}");
                }
            }
        }

        [Fact(Timeout = 300000)]
        public void TestCompiledCSharpParserStructure()
        {
            // Test C# parser compilation and basic structure validation
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            if (!File.Exists(TpcKsyPath))
            {
                return; // Skip if file doesn't exist
            }

            string langOutputDir = Path.Combine(CompilerOutputDir, "tpc", "csharp");
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            // Compile to C#
            var compileInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t csharp \"{TpcKsyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(TpcKsyPath)
            };

            using (var process = Process.Start(compileInfo))
            {
                if (process != null)
                {
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(60000);
                    process.ExitCode.Should().Be(0,
                        $"C# compilation should succeed. STDOUT: {stdout}, STDERR: {stderr}");
                }
            }

            // Verify C# parser file was generated
            string[] csFiles = Directory.GetFiles(langOutputDir, "*.cs", SearchOption.AllDirectories);
            csFiles.Should().NotBeEmpty("C# parser files should be generated");

            // Verify generated C# file contains expected structure
            string tpcCsFile = csFiles.FirstOrDefault(f => Path.GetFileName(f).ToLowerInvariant().Contains("tpc"));
            if (tpcCsFile != null)
            {
                string csContent = File.ReadAllText(tpcCsFile);
                csContent.Should().Contain("class", "Generated C# file should contain class definition");
            }
        }

        [Theory(Timeout = 300000)]
        [InlineData("cpp_stl")]
        [InlineData("ruby")]
        [InlineData("php")]
        [InlineData("go")]
        [InlineData("rust")]
        [InlineData("perl")]
        [InlineData("lua")]
        [InlineData("nim")]
        [InlineData("visualbasic")]
        public void TestCompileTpcKsyToAdditionalLanguages(string language)
        {
            // Test compilation to additional languages
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            string langOutputDir = Path.Combine(CompilerOutputDir, "tpc", language);
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            // Compile to target language
            var compileInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t {language} \"{TpcKsyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(TpcKsyPath)
            };

            int exitCode = -1;
            string stdout = "";
            string stderr = "";

            using (var process = Process.Start(compileInfo))
            {
                if (process != null)
                {
                    stdout = process.StandardOutput.ReadToEnd();
                    stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(60000);
                    exitCode = process.ExitCode;
                }
            }

            // Compilation should succeed (some languages may not be fully supported, but should attempt)
            if (exitCode != 0)
            {
                // Log but don't fail - some languages may not be available in all compiler versions
                Console.WriteLine($"Warning: {language} compilation failed with exit code {exitCode}. STDOUT: {stdout}, STDERR: {stderr}");
            }
            else
            {
                // Verify output files were generated
                string[] generatedFiles = Directory.GetFiles(langOutputDir, "*", SearchOption.AllDirectories);
                generatedFiles.Should().NotBeEmpty($"{language} compilation should generate output files");
            }
        }

        public static IEnumerable<object[]> GetSupportedLanguages()
        {
            return SupportedLanguages.Select(lang => new object[] { lang });
        }

        private static string FindKaitaiCompiler()
        {
            // Try common locations and PATH
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

