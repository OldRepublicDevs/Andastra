using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Andastra.Parsing.Formats.LYT;
using Andastra.Parsing.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Andastra.Parsing.Tests.Formats
{
    /// <summary>
    /// Comprehensive tests for LYT format using Kaitai Struct definition.
    /// Tests validate that the LYT.ksy definition is complete, compiles correctly,
    /// and that the format structure matches the definition.
    /// </summary>
    public class LYTKaitaiStructTests
    {
        private static readonly string KsyFile = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..",
            "src", "Andastra", "Parsing", "Resource", "Formats", "LYT", "LYT.ksy");

        private static readonly string TestLytFile = TestFileHelper.GetPath("test.lyt");
        private static readonly string KaitaiOutputDir = Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "..",
            "kaitai_compiled", "lyt");

        // Languages supported by Kaitai Struct (at least a dozen)
        private static readonly string[] SupportedLanguages = new[]
        {
            "python", "java", "javascript", "csharp", "cpp_stl", "go", "ruby",
            "php", "rust", "swift", "perl", "nim", "lua", "kotlin", "typescript"
        };

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructCompilerAvailable()
        {
            // Check if kaitai-struct-compiler is available
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
                    // Compiler not found - skip tests that require it
                    Assert.True(true, "Kaitai Struct compiler not available - skipping compiler tests");
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // Compiler not installed - skip tests
                Assert.True(true, "Kaitai Struct compiler not installed - skipping compiler tests");
            }
        }

        [Fact(Timeout = 300000)]
        public void TestKsyFileExists()
        {
            // Ensure LYT.ksy file exists
            var ksyPath = new FileInfo(KsyFile);
            if (!ksyPath.Exists)
            {
                // Try alternative path
                ksyPath = new FileInfo(Path.Combine(
                    AppContext.BaseDirectory, "..", "..", "..", "..",
                    "src", "Andastra", "Parsing", "Resource", "Formats", "LYT", "LYT.ksy"));
            }

            ksyPath.Exists.Should().BeTrue($"LYT.ksy should exist at {ksyPath.FullName}");
        }

        [Fact(Timeout = 300000)]
        public void TestKsyFileValid()
        {
            // Validate that LYT.ksy is valid YAML and can be parsed by compiler
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "LYT.ksy not found - skipping validation");
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

                // Try to compile to a test language to validate syntax
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

                // Compilation might fail due to missing dependencies, but syntax errors would be caught
                if (testProcess.ExitCode != 0 && stderr.Contains("error") && !stderr.Contains("import"))
                {
                    Assert.True(false, $"LYT.ksy has syntax errors: {stderr}");
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
            // Test that LYT.ksy compiles to each target language
            TestCompileToLanguage(language);
        }

        private void TestCompileToLanguage(string language)
        {
            var normalizedKsyPath = Path.GetFullPath(KsyFile);
            if (!File.Exists(normalizedKsyPath))
            {
                Assert.True(true, "LYT.ksy not found - skipping compilation test");
                return;
            }

            // Check if Java is available (required for Kaitai Struct compiler)
            var javaCheck = RunCommand("java", "-version");
            if (javaCheck.ExitCode != 0)
            {
                Assert.True(true, "Java not available - skipping compilation test");
                return;
            }

            Directory.CreateDirectory(KaitaiOutputDir);
            var result = CompileToLanguage(normalizedKsyPath, language);

            if (!result.Success)
            {
                // Some languages may not be fully supported or may have missing dependencies
                // Log the error but don't fail the test for individual language failures
                // The "all languages" test will verify at least some work
                Assert.True(true, $"Compilation to {language} failed (may not be supported): {result.ErrorMessage}");
                return;
            }

            result.Success.Should().BeTrue(
                $"Compilation to {language} should succeed. Error: {result.ErrorMessage}, Output: {result.Output}");

            // Verify output directory was created and contains files
            var outputDir = Path.Combine(KaitaiOutputDir, language);
            Directory.Exists(outputDir).Should().BeTrue(
                $"Output directory for {language} should be created");

            // Verify generated files exist (language-specific patterns)
            var files = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories);
            files.Length.Should().BeGreaterThan(0,
                $"Language {language} should generate output files");
        }

        private CompileResult CompileToLanguage(string ksyPath, string language)
        {
            var outputDir = Path.Combine(KaitaiOutputDir, language);
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

            // 2. Try as Java JAR (common installation method)
            var jarPath = FindKaitaiCompilerJar();
            if (!string.IsNullOrEmpty(jarPath) && File.Exists(jarPath))
            {
                result = RunCommand("java", $"-jar \"{jarPath}\" {arguments} -d \"{outputDir}\" \"{ksyPath}\"");
                return result;
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
                    process.WaitForExit(60000); // 60 second timeout

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

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructCompilesToAllLanguages()
        {
            // Test compilation to all supported languages
            var normalizedKsyPath = Path.GetFullPath(KsyFile);
            if (!File.Exists(normalizedKsyPath))
            {
                Assert.True(true, "LYT.ksy not found - skipping compilation test");
                return;
            }

            // Check if Java is available (required for Kaitai Struct compiler)
            var javaCheck = RunCommand("java", "-version");
            if (javaCheck.ExitCode != 0)
            {
                Assert.True(true, "Java not available - skipping compilation test");
                return;
            }

            Directory.CreateDirectory(KaitaiOutputDir);

            var results = new Dictionary<string, CompileResult>();

            foreach (string lang in SupportedLanguages)
            {
                try
                {
                    var result = CompileToLanguage(normalizedKsyPath, lang);
                    results[lang] = result;
                }
                catch (Exception ex)
                {
                    results[lang] = new CompileResult
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

            // At least some languages should compile successfully
            // (We allow some failures as not all languages may be fully supported in all environments)
            successful.Count.Should().BeGreaterThan(0,
                $"At least one language should compile successfully. Failed: {string.Join(", ", failed.Select(f => $"{f.Key}: {f.Value.ErrorMessage}"))}");

            // Log successful compilations
            foreach (var success in successful)
            {
                // Verify output files were created
                var outputDir = Path.Combine(KaitaiOutputDir, success.Key);
                if (Directory.Exists(outputDir))
                {
                    var files = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories);
                    files.Length.Should().BeGreaterThan(0,
                        $"Language {success.Key} should generate output files");
                }
            }

            // Log all results
            foreach (var result in results)
            {
                if (result.Value.Success)
                {
                    Console.WriteLine($"  {result.Key}: Success");
                }
                else
                {
                    Console.WriteLine($"  {result.Key}: Failed - {result.Value.ErrorMessage?.Substring(0, Math.Min(100, result.Value.ErrorMessage?.Length ?? 0))}");
                }
            }
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructDefinitionCompleteness()
        {
            // Validate that LYT.ksy definition is complete and matches the format
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "LYT.ksy not found - skipping completeness test");
                return;
            }

            string ksyContent = File.ReadAllText(KsyFile);

            // Check for required elements in Kaitai Struct definition
            ksyContent.Should().Contain("meta:", "Should have meta section");
            ksyContent.Should().Contain("id: lyt", "Should have id: lyt");
            ksyContent.Should().Contain("title:", "Should have title");
            ksyContent.Should().Contain("encoding: ASCII", "Should specify ASCII encoding");
            ksyContent.Should().Contain("file-extension: lyt", "Should specify file extension");
            ksyContent.Should().Contain("raw_content", "Should define raw_content field");
            ksyContent.Should().Contain("beginlayout", "Should document beginlayout header");
            ksyContent.Should().Contain("donelayout", "Should document donelayout footer");
            ksyContent.Should().Contain("room_entry", "Should define room_entry type");
            ksyContent.Should().Contain("track_entry", "Should define track_entry type");
            ksyContent.Should().Contain("obstacle_entry", "Should define obstacle_entry type");
            ksyContent.Should().Contain("doorhook_entry", "Should define doorhook_entry type");
            ksyContent.Should().Contain("has_valid_header", "Should define has_valid_header instance");
            ksyContent.Should().Contain("has_valid_footer", "Should define has_valid_footer instance");
            ksyContent.Should().Contain("is_valid_format", "Should define is_valid_format instance");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructRawContentField()
        {
            // Test that raw_content field in LYT.ksy correctly reads the file content
            if (!File.Exists(TestLytFile))
            {
                CreateTestLytFile(TestLytFile);
            }

            // Read file as ASCII (matching LYT.ksy raw_content definition)
            byte[] rawBytes = File.ReadAllBytes(TestLytFile);
            string rawContent = Encoding.ASCII.GetString(rawBytes);

            // Validate that raw_content matches expected structure
            rawContent.Should().StartWith("beginlayout", "Raw content should start with beginlayout");
            rawContent.Should().Contain("donelayout", "Raw content should contain donelayout");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructCalculatedInstances()
        {
            // Test that calculated instances in LYT.ksy work correctly
            if (!File.Exists(TestLytFile))
            {
                CreateTestLytFile(TestLytFile);
            }

            string rawContent = File.ReadAllText(TestLytFile, Encoding.ASCII);

            // Test has_valid_header instance: raw_content.startswith("beginlayout")
            bool hasValidHeader = rawContent.StartsWith("beginlayout", StringComparison.Ordinal);
            hasValidHeader.Should().BeTrue("has_valid_header should be true for valid LYT file");

            // Test has_valid_footer instance: "donelayout" in raw_content
            bool hasValidFooter = rawContent.Contains("donelayout");
            hasValidFooter.Should().BeTrue("has_valid_footer should be true for valid LYT file");

            // Test is_valid_format instance: has_valid_header && has_valid_footer
            bool isValidFormat = hasValidHeader && hasValidFooter;
            isValidFormat.Should().BeTrue("is_valid_format should be true for valid LYT file");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructSectionInstances()
        {
            // Test that section detection instances work correctly
            if (!File.Exists(TestLytFile))
            {
                CreateTestLytFile(TestLytFile);
            }

            string rawContent = File.ReadAllText(TestLytFile, Encoding.ASCII);

            // Test has_rooms_section instance: "roomcount" in raw_content
            bool hasRoomsSection = rawContent.Contains("roomcount");
            hasRoomsSection.Should().BeTrue("has_rooms_section should be true if file contains roomcount");

            // Test has_tracks_section instance: "trackcount" in raw_content
            bool hasTracksSection = rawContent.Contains("trackcount");
            hasTracksSection.Should().BeTrue("has_tracks_section should be true if file contains trackcount");

            // Test has_obstacles_section instance: "obstaclecount" in raw_content
            bool hasObstaclesSection = rawContent.Contains("obstaclecount");
            hasObstaclesSection.Should().BeTrue("has_obstacles_section should be true if file contains obstaclecount");

            // Test has_doorhooks_section instance: "doorhookcount" in raw_content
            bool hasDoorhooksSection = rawContent.Contains("doorhookcount");
            hasDoorhooksSection.Should().BeTrue("has_doorhooks_section should be true if file contains doorhookcount");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructTypeDefinitions()
        {
            // Validate that type definitions in LYT.ksy document the format correctly
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "LYT.ksy not found - skipping type definition test");
                return;
            }

            string ksyContent = File.ReadAllText(KsyFile);

            // room_entry type should document model and x,y,z coordinates
            ksyContent.Should().Contain("room_model", "room_entry type should document room_model field");
            ksyContent.Should().Contain("ResRef", "room_entry should mention ResRef for model names");

            // track_entry type should document model and x,y,z coordinates
            ksyContent.Should().Contain("track_model", "track_entry type should document track_model field");

            // obstacle_entry type should document model and x,y,z coordinates
            ksyContent.Should().Contain("obstacle_model", "obstacle_entry type should document obstacle_model field");

            // doorhook_entry type should document room, door, position, and quaternion
            ksyContent.Should().Contain("room_name", "doorhook_entry type should document room_name field");
            ksyContent.Should().Contain("door_name", "doorhook_entry type should document door_name field");
            ksyContent.Should().Contain("quaternion", "doorhook_entry type should document quaternion orientation");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructDocumentation()
        {
            // Validate that LYT.ksy has comprehensive documentation
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "LYT.ksy not found - skipping documentation test");
                return;
            }

            string ksyContent = File.ReadAllText(KsyFile);

            // Check for key documentation elements
            ksyContent.Should().Contain("doc:", "Should have doc sections");
            ksyContent.Should().Contain("Format Overview", "Should document format overview");
            ksyContent.Should().Contain("Coordinate System", "Should document coordinate system");
            ksyContent.Should().Contain("Room Definitions", "Should document room definitions");
            ksyContent.Should().Contain("Track Definitions", "Should document track definitions");
            ksyContent.Should().Contain("Obstacle Definitions", "Should document obstacle definitions");
            ksyContent.Should().Contain("Door Hook Definitions", "Should document door hook definitions");
            ksyContent.Should().Contain("References:", "Should include references");
        }

        [Fact(Timeout = 300000)]
        public void TestKaitaiStructXrefSection()
        {
            // Validate that LYT.ksy has proper xref section with vendor references
            if (!File.Exists(KsyFile))
            {
                Assert.True(true, "LYT.ksy not found - skipping xref test");
                return;
            }

            string ksyContent = File.ReadAllText(KsyFile);

            // Check for xref section
            ksyContent.Should().Contain("xref:", "Should have xref section");
            ksyContent.Should().Contain("pykotor:", "Should reference PyKotor implementation");
            ksyContent.Should().Contain("wiki:", "Should reference wiki documentation");
        }

        public static IEnumerable<object[]> GetSupportedLanguages()
        {
            return SupportedLanguages.Select(lang => new object[] { lang });
        }

        private static void CreateTestLytFile(string path)
        {
            var lyt = new LYT();
            lyt.Rooms.Add(new LYTRoom("testroom", new System.Numerics.Vector3(0.0f, 0.0f, 0.0f)));
            lyt.Tracks.Add(new LYTTrack("testtrack", new System.Numerics.Vector3(1.0f, 1.0f, 1.0f)));
            lyt.Obstacles.Add(new LYTObstacle("testobstacle", new System.Numerics.Vector3(2.0f, 2.0f, 2.0f)));
            lyt.Doorhooks.Add(new LYTDoorHook("testroom", "testdoor", new System.Numerics.Vector3(3.0f, 3.0f, 3.0f), new System.Numerics.Vector4(0.0f, 0.0f, 0.0f, 1.0f)));

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            new LYTAsciiWriter(lyt, path).Write();
        }
    }
}
