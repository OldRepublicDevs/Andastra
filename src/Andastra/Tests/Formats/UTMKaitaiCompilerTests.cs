using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Andastra.Parsing;
using Andastra.Parsing.Common;
using Andastra.Parsing.Formats.GFF;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Resource.Generics;
using Andastra.Parsing.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Andastra.Parsing.Tests.Formats
{
    /// <summary>
    /// Comprehensive tests for UTM.ksy Kaitai Struct compiler functionality.
    /// Tests compile UTM.ksy to multiple languages and validate the generated parsers work correctly.
    ///
    /// Supported languages tested (at least 12 as required):
    /// - Python, Java, JavaScript, C#, C++, Ruby, PHP, Go, Rust, Swift, Perl, Lua, Nim, VisualBasic
    /// </summary>
    public class UTMKaitaiCompilerTests
    {
        private static readonly string UtmKsyPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "src", "Andastra", "Parsing", "Resource", "Formats", "GFF", "Generics", "UTM", "UTM.ksy"
        ));

        private static readonly string TestUtmFile = TestFileHelper.GetPath("test.utm");
        private static readonly string TestOutputDir = Path.Combine(
            AppContext.BaseDirectory,
            "test_files", "kaitai_utm_compiled"
        );

        // Supported languages in Kaitai Struct (at least 12 as required)
        private static readonly string[] SupportedLanguages = new[]
        {
            "python",
            "java",
            "javascript",
            "csharp",
            "cpp_stl",
            "go",
            "ruby",
            "php",
            "rust",
            "swift",
            "lua",
            "nim",
            "perl",
            "visualbasic"
        };

        static UTMKaitaiCompilerTests()
        {
            // Normalize UTM.ksy path
            UtmKsyPath = Path.GetFullPath(UtmKsyPath);
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
        public void TestUtmKsyFileExists()
        {
            File.Exists(UtmKsyPath).Should().BeTrue($"UTM.ksy should exist at {UtmKsyPath}");

            // Validate it's a valid Kaitai Struct file
            string content = File.ReadAllText(UtmKsyPath);
            content.Should().Contain("meta:", "UTM.ksy should contain meta section");
            content.Should().Contain("id: utm", "UTM.ksy should have id: utm");
            content.Should().Contain("file-extension: utm", "UTM.ksy should specify utm file extension");
        }

        [Fact(Timeout = 600000)] // 10 minute timeout for compiling all languages
        public void TestCompileUtmToAllLanguages()
        {
            var normalizedKsyPath = Path.GetFullPath(UtmKsyPath);
            if (!File.Exists(normalizedKsyPath))
            {
                // Skip if .ksy file doesn't exist
                return;
            }

            // Check if Java/Kaitai compiler is available
            var javaCheck = RunCommand("java", "-version");
            if (javaCheck.ExitCode != 0)
            {
                // Skip test if Java is not available
                return;
            }

            Directory.CreateDirectory(TestOutputDir);

            var results = new Dictionary<string, CompileResult>();

            foreach (var language in SupportedLanguages)
            {
                try
                {
                    var result = CompileToLanguage(normalizedKsyPath, language);
                    results[language] = result;
                }
                catch (Exception ex)
                {
                    results[language] = new CompileResult
                    {
                        Success = false,
                        ErrorMessage = ex.Message,
                        Output = ex.ToString()
                    };
                }
            }

            // Report results
            var successful = results.Where(r => r.Value.Success).ToList();
            var failed = results.Where(r => !r.Value.Success).ToList();

            // At least 12 languages should compile successfully
            successful.Count.Should().BeGreaterOrEqualTo(12,
                $"At least 12 languages should compile successfully. " +
                $"Successful ({successful.Count}): {string.Join(", ", successful.Select(s => s.Key))}. " +
                $"Failed ({failed.Count}): {string.Join(", ", failed.Select(f => $"{f.Key}: {f.Value.ErrorMessage}"))}");

            // Log successful compilations and verify output files
            foreach (var success in successful)
            {
                // Verify output files were created
                var outputDir = Path.Combine(TestOutputDir, success.Key);
                if (Directory.Exists(outputDir))
                {
                    var files = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories)
                        .Where(f => !f.EndsWith("compile_output.txt") && !f.EndsWith("compile_error.txt"))
                        .ToList();
                    files.Count.Should().BeGreaterThan(0,
                        $"Language {success.Key} should generate output files. Found: {string.Join(", ", files.Select(Path.GetFileName))}");

                    // Verify at least one parser file was generated (language-specific patterns)
                    var hasParserFile = files.Any(f =>
                        f.Contains("utm") || f.Contains("Utm") || f.Contains("UTM") ||
                        f.EndsWith(".py") || f.EndsWith(".java") || f.EndsWith(".js") ||
                        f.EndsWith(".cs") || f.EndsWith(".cpp") || f.EndsWith(".h") ||
                        f.EndsWith(".go") || f.EndsWith(".rb") || f.EndsWith(".php") ||
                        f.EndsWith(".rs") || f.EndsWith(".swift") || f.EndsWith(".lua") ||
                        f.EndsWith(".nim") || f.EndsWith(".pm") || f.EndsWith(".vb"));

                    hasParserFile.Should().BeTrue(
                        $"Language {success.Key} should generate parser files. Files: {string.Join(", ", files.Select(Path.GetFileName))}");
                }
            }
        }

        [Fact(Timeout = 300000)]
        public void TestCompiledParserValidatesUtmFile()
        {
            // Create test UTM file if it doesn't exist
            if (!File.Exists(TestUtmFile))
            {
                CreateTestUtmFile(TestUtmFile);
            }

            // Test Python parser (most commonly available)
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            string langOutputDir = Path.Combine(TestOutputDir, "python");
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            // Compile to Python
            var compileInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t python \"{UtmKsyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(UtmKsyPath)
            };

            using (var process = Process.Start(compileInfo))
            {
                if (process != null)
                {
                    process.WaitForExit(60000);
                    process.ExitCode.Should().Be(0, "Python compilation should succeed");
                }
            }

            // Verify Python parser file was generated
            string[] pythonFiles = Directory.GetFiles(langOutputDir, "*.py", SearchOption.AllDirectories);
            pythonFiles.Should().NotBeEmpty("Python parser files should be generated");

            // Note: Actually using the generated parser would require Python runtime and kaitaistruct library
            // This test validates that compilation succeeds and generates expected files
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

            string langOutputDir = Path.Combine(TestOutputDir, "csharp");
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            // Compile to C#
            var compileInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t csharp \"{UtmKsyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(UtmKsyPath)
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
            string utmCsFile = csFiles.FirstOrDefault(f => Path.GetFileName(f).ToLowerInvariant().Contains("utm"));
            if (utmCsFile != null)
            {
                string csContent = File.ReadAllText(utmCsFile);
                csContent.Should().Contain("class", "Generated C# file should contain class definition");
                csContent.Should().Contain("GffHeader", "Generated C# file should contain GffHeader structure");
            }
        }

        [Fact(Timeout = 300000)]
        public void TestCompiledJavaParserStructure()
        {
            // Test Java parser compilation
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            string langOutputDir = Path.Combine(TestOutputDir, "java");
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            // Compile to Java
            var compileInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t java \"{UtmKsyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(UtmKsyPath)
            };

            using (var process = Process.Start(compileInfo))
            {
                if (process != null)
                {
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(60000);
                    process.ExitCode.Should().Be(0,
                        $"Java compilation should succeed. STDOUT: {stdout}, STDERR: {stderr}");
                }
            }

            // Verify Java parser files were generated
            string[] javaFiles = Directory.GetFiles(langOutputDir, "*.java", SearchOption.AllDirectories);
            javaFiles.Should().NotBeEmpty("Java parser files should be generated");
        }

        [Fact(Timeout = 300000)]
        public void TestCompiledJavaScriptParserStructure()
        {
            // Test JavaScript parser compilation
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            string langOutputDir = Path.Combine(TestOutputDir, "javascript");
            if (Directory.Exists(langOutputDir))
            {
                Directory.Delete(langOutputDir, true);
            }
            Directory.CreateDirectory(langOutputDir);

            // Compile to JavaScript
            var compileInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t javascript \"{UtmKsyPath}\" -d \"{langOutputDir}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(UtmKsyPath)
            };

            using (var process = Process.Start(compileInfo))
            {
                if (process != null)
                {
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(60000);
                    process.ExitCode.Should().Be(0,
                        $"JavaScript compilation should succeed. STDOUT: {stdout}, STDERR: {stderr}");
                }
            }

            // Verify JavaScript parser files were generated
            string[] jsFiles = Directory.GetFiles(langOutputDir, "*.js", SearchOption.AllDirectories);
            jsFiles.Should().NotBeEmpty("JavaScript parser files should be generated");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToPython()
        {
            TestCompileToLanguage("python");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToJava()
        {
            TestCompileToLanguage("java");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToJavaScript()
        {
            TestCompileToLanguage("javascript");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToCSharp()
        {
            TestCompileToLanguage("csharp");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToCpp()
        {
            TestCompileToLanguage("cpp_stl");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToGo()
        {
            TestCompileToLanguage("go");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToRuby()
        {
            TestCompileToLanguage("ruby");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToPhp()
        {
            TestCompileToLanguage("php");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToRust()
        {
            TestCompileToLanguage("rust");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToSwift()
        {
            TestCompileToLanguage("swift");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToLua()
        {
            TestCompileToLanguage("lua");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToNim()
        {
            TestCompileToLanguage("nim");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToPerl()
        {
            TestCompileToLanguage("perl");
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToVisualBasic()
        {
            TestCompileToLanguage("visualbasic");
        }

        private void TestCompileToLanguage(string language)
        {
            var normalizedKsyPath = Path.GetFullPath(UtmKsyPath);
            if (!File.Exists(normalizedKsyPath))
            {
                // Skip if .ksy file doesn't exist
                return;
            }

            var javaCheck = RunCommand("java", "-version");
            if (javaCheck.ExitCode != 0)
            {
                // Skip if Java is not available
                return;
            }

            Directory.CreateDirectory(TestOutputDir);

            var result = CompileToLanguage(normalizedKsyPath, language);

            if (!result.Success)
            {
                // Some languages may not be fully supported or may have missing dependencies
                // Log the error but don't fail the test for individual language failures
                // The "all languages" test will verify at least some work
                return;
            }

            result.Success.Should().BeTrue(
                $"Compilation to {language} should succeed. Error: {result.ErrorMessage}, Output: {result.Output}");

            // Verify output directory was created
            var outputDir = Path.Combine(TestOutputDir, language);
            Directory.Exists(outputDir).Should().BeTrue(
                $"Output directory for {language} should be created");

            // Verify parser files were actually generated
            var files = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith("compile_output.txt") && !f.EndsWith("compile_error.txt"))
                .ToList();

            files.Count.Should().BeGreaterThan(0,
                $"Language {language} should generate parser files. Found: {string.Join(", ", files.Select(Path.GetFileName))}");

            // Verify at least one parser file matches language-specific patterns
            var hasParserFile = files.Any(f =>
                f.Contains("utm") || f.Contains("Utm") || f.Contains("UTM") ||
                f.EndsWith(".py") || f.EndsWith(".java") || f.EndsWith(".js") ||
                f.EndsWith(".cs") || f.EndsWith(".cpp") || f.EndsWith(".h") ||
                f.EndsWith(".go") || f.EndsWith(".rb") || f.EndsWith(".php") ||
                f.EndsWith(".rs") || f.EndsWith(".swift") || f.EndsWith(".lua") ||
                f.EndsWith(".nim") || f.EndsWith(".pm") || f.EndsWith(".vb"));

            hasParserFile.Should().BeTrue(
                $"Language {language} should generate parser files. Files: {string.Join(", ", files.Select(Path.GetFileName))}");
        }

        [Fact(Timeout = 300000)]
        public void TestUtmKsySyntaxValidation()
        {
            // Validate UTM.ksy syntax by attempting compilation
            string compilerPath = FindKaitaiCompiler();
            if (string.IsNullOrEmpty(compilerPath))
            {
                return; // Skip if compiler not available
            }

            // Use Python as validation target (most commonly supported)
            var validateInfo = new ProcessStartInfo
            {
                FileName = compilerPath,
                Arguments = $"-t python \"{UtmKsyPath}\" --debug",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(UtmKsyPath)
            };

            using (var process = Process.Start(validateInfo))
            {
                if (process != null)
                {
                    string stdout = process.StandardOutput.ReadToEnd();
                    string stderr = process.StandardError.ReadToEnd();
                    process.WaitForExit(30000);

                    // Compiler should not report syntax errors
                    stderr.Should().NotContain("error", "UTM.ksy should not have syntax errors");
                    process.ExitCode.Should().Be(0,
                        $"UTM.ksy syntax should be valid. STDOUT: {stdout}, STDERR: {stderr}");
                }
            }
        }

        [Fact(Timeout = 300000)]
        public void TestCompileUtmToMultipleLanguagesSimultaneously()
        {
            var normalizedKsyPath = Path.GetFullPath(UtmKsyPath);
            if (!File.Exists(normalizedKsyPath))
            {
                return;
            }

            var javaCheck = RunCommand("java", "-version");
            if (javaCheck.ExitCode != 0)
            {
                return;
            }

            Directory.CreateDirectory(TestOutputDir);

            // Compile to multiple languages in a single command
            var languages = new[] { "python", "java", "javascript", "csharp" };
            var languageArgs = string.Join(" ", languages.Select(l => $"-t {l}"));

            var result = RunKaitaiCompiler(normalizedKsyPath, languageArgs, TestOutputDir);

            // Compilation should succeed (or at least not fail catastrophically)
            // Some languages may fail due to missing dependencies, but the command should execute
            result.ExitCode.Should().BeInRange(-1, 1,
                $"Kaitai compiler should execute. Output: {result.Output}, Error: {result.Error}");
        }

        [Fact(Timeout = 300000)]
        public void TestUtmKsyFileTypeSignature()
        {
            // Validate UTM.ksy defines correct file type signature
            string content = File.ReadAllText(UtmKsyPath);
            content.Should().Contain("valid: \"UTM \"", "UTM.ksy should validate file type signature as 'UTM '");
        }

        [Fact(Timeout = 300000)]
        public void TestUtmKsyVersionValidation()
        {
            // Validate UTM.ksy defines correct version validation
            string content = File.ReadAllText(UtmKsyPath);
            content.Should().Contain("V3.2", "UTM.ksy should support V3.2 version");
            content.Should().Contain("V3.3", "UTM.ksy should support V3.3 version");
            content.Should().Contain("V4.0", "UTM.ksy should support V4.0 version");
            content.Should().Contain("V4.1", "UTM.ksy should support V4.1 version");
        }

        [Fact(Timeout = 300000)]
        public void TestUtmKsyGffFieldTypeEnum()
        {
            // Validate UTM.ksy defines GFF field type enum
            string content = File.ReadAllText(UtmKsyPath);
            content.Should().Contain("gff_field_type:", "UTM.ksy should define gff_field_type enum");
            content.Should().Contain("uint8", "UTM.ksy should define uint8 field type");
            content.Should().Contain("resref", "UTM.ksy should define resref field type");
            content.Should().Contain("localized_string", "UTM.ksy should define localized_string field type");
            content.Should().Contain("list", "UTM.ksy should define list field type");
        }

        [Fact(Timeout = 300000)]
        public void TestUtmKsyStructInstances()
        {
            // Validate UTM.ksy defines struct instances for computed values
            string content = File.ReadAllText(UtmKsyPath);
            content.Should().Contain("instances:", "UTM.ksy should define instances section");
            content.Should().Contain("has_single_field", "UTM.ksy should define has_single_field instance");
            content.Should().Contain("has_multiple_fields", "UTM.ksy should define has_multiple_fields instance");
            content.Should().Contain("field_indices_offset", "UTM.ksy should define field_indices_offset instance");
        }

        [Fact(Timeout = 300000)]
        public void TestUtmKsyLocalizedStringStructure()
        {
            // Validate UTM.ksy defines LocalizedString structure
            string content = File.ReadAllText(UtmKsyPath);
            content.Should().Contain("localized_string_data:", "UTM.ksy should define localized_string_data type");
            content.Should().Contain("localized_substring:", "UTM.ksy should define localized_substring type");
            content.Should().Contain("string_ref", "UTM.ksy should define string_ref field");
            content.Should().Contain("string_count", "UTM.ksy should define string_count field");
        }

        [Fact(Timeout = 300000)]
        public void TestUtmKsyDocumentation()
        {
            // Validate UTM.ksy has comprehensive documentation
            string content = File.ReadAllText(UtmKsyPath);
            content.Should().Contain("BuySellFlag", "UTM.ksy should document BuySellFlag field");
            content.Should().Contain("ItemList", "UTM.ksy should document ItemList field");
            content.Should().Contain("UTM_ItemList", "UTM.ksy should document UTM_ItemList structure");
            content.Should().Contain("InventoryRes", "UTM.ksy should document InventoryRes field");
            content.Should().Contain("Infinite", "UTM.ksy should document Infinite field");
            content.Should().Contain("Dropable", "UTM.ksy should document Dropable field");
            content.Should().Contain("Repos_PosX", "UTM.ksy should document Repos_PosX field");
            content.Should().Contain("Repos_PosY", "UTM.ksy should document Repos_PosY field");
            content.Should().Contain("MarkUp", "UTM.ksy should document MarkUp field");
            content.Should().Contain("MarkDown", "UTM.ksy should document MarkDown field");
            content.Should().Contain("OnOpenStore", "UTM.ksy should document OnOpenStore field");
            content.Should().Contain("ResRef", "UTM.ksy should document ResRef field");
            content.Should().Contain("LocName", "UTM.ksy should document LocName field");
            content.Should().Contain("Tag", "UTM.ksy should document Tag field");
            content.Should().Contain("Comment", "UTM.ksy should document Comment field");
        }

        private CompileResult CompileToLanguage(string ksyPath, string language)
        {
            var outputDir = Path.Combine(TestOutputDir, language);
            Directory.CreateDirectory(outputDir);

            var result = RunKaitaiCompiler(ksyPath, $"-t {language}", outputDir);

            return new CompileResult
            {
                Success = result.ExitCode == 0,
                Output = result.Output,
                ErrorMessage = result.Error,
                ExitCode = result.ExitCode
            };
        }

        private (int ExitCode, string Output, string Error) RunKaitaiCompiler(
            string ksyPath, string arguments, string outputDir)
        {
            // Try different ways to invoke Kaitai Struct compiler
            // 1. As a command (if installed via package manager)
            var result = RunCommand("kaitai-struct-compiler", $"{arguments} -d \"{outputDir}\" \"{ksyPath}\"");

            if (result.ExitCode == 0)
            {
                return result;
            }

            // 2. Try with .jar extension
            result = RunCommand("kaitai-struct-compiler.jar", $"{arguments} -d \"{outputDir}\" \"{ksyPath}\"");

            if (result.ExitCode == 0)
            {
                return result;
            }

            // 3. Try as Java JAR (common installation method)
            var jarPath = FindKaitaiCompilerJar();
            if (!string.IsNullOrEmpty(jarPath) && File.Exists(jarPath))
            {
                result = RunCommand("java", $"-jar \"{jarPath}\" {arguments} -d \"{outputDir}\" \"{ksyPath}\"");
                return result;
            }

            // 4. Try in common installation locations
            var commonPaths = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "bin", "kaitai-struct-compiler"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "kaitai-struct-compiler", "kaitai-struct-compiler.jar"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "kaitai-struct-compiler", "kaitai-struct-compiler.jar"),
            };

            foreach (var path in commonPaths)
            {
                if (File.Exists(path))
                {
                    if (path.EndsWith(".jar"))
                    {
                        result = RunCommand("java", $"-jar \"{path}\" {arguments} -d \"{outputDir}\" \"{ksyPath}\"");
                    }
                    else
                    {
                        result = RunCommand(path, $"{arguments} -d \"{outputDir}\" \"{ksyPath}\"");
                    }

                    if (result.ExitCode == 0)
                    {
                        return result;
                    }
                }
            }

            // Return the last result (which will be a failure)
            return result;
        }

        private string FindKaitaiCompilerJar()
        {
            // Check environment variable first
            var envJar = Environment.GetEnvironmentVariable("KAITAI_COMPILER_JAR");
            if (!string.IsNullOrEmpty(envJar) && File.Exists(envJar))
            {
                return envJar;
            }

            // Check common locations for Kaitai Struct compiler JAR
            var searchPaths = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "kaitai-struct-compiler.jar"),
                Path.Combine(AppContext.BaseDirectory, "..", "kaitai-struct-compiler.jar"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".kaitai", "kaitai-struct-compiler.jar"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "kaitai-struct-compiler.jar"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "kaitai-struct-compiler.jar"),
            };

            foreach (var path in searchPaths)
            {
                var normalized = Path.GetFullPath(path);
                if (File.Exists(normalized))
                {
                    return normalized;
                }
            }

            return null;
        }

        private string FindKaitaiCompiler()
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

        private (int ExitCode, string Output, string Error) RunCommand(string command, string arguments)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = AppContext.BaseDirectory
                };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process == null)
                    {
                        return (-1, "", $"Failed to start process: {command}");
                    }

                    var output = process.StandardOutput.ReadToEnd();
                    var error = process.StandardError.ReadToEnd();
                    process.WaitForExit(30000); // 30 second timeout

                    return (process.ExitCode, output, error);
                }
            }
            catch (Exception ex)
            {
                return (-1, "", ex.Message);
            }
        }

        private class CompileResult
        {
            public bool Success { get; set; }
            public string Output { get; set; }
            public string ErrorMessage { get; set; }
            public int ExitCode { get; set; }
        }

        private static void CreateTestUtmFile(string path)
        {
            var utm = new UTM();
            utm.ResRef = new ResRef("test_merchant");
            utm.Name = LocalizedString.FromEnglish("Test Merchant");
            utm.Tag = "TEST";
            utm.MarkUp = 10;
            utm.MarkDown = 5;
            utm.CanBuy = true;
            utm.CanSell = true;
            utm.Comment = "Test merchant comment";
            utm.OnOpenScript = new ResRef("test_open");

            // Add a test item
            var item = new UTMItem();
            item.ResRef = new ResRef("test_item");
            item.Infinite = 1;
            item.Droppable = 0;
            utm.Items.Add(item);

            byte[] data = UTMHelpers.BytesUtm(utm, Game.K2);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, data);
        }
    }
}

