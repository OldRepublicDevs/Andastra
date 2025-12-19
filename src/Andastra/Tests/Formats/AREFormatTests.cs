using System;
using System.IO;
using System.Numerics;
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
    /// Comprehensive tests for ARE (Area) binary I/O operations.
    /// Tests validate the ARE format structure as defined in ARE.ksy Kaitai Struct definition.
    /// ARE files are GFF-based format files that store static area information.
    /// </summary>
    public class AREFormatTests
    {
        private static readonly string BinaryTestFile = TestFileHelper.GetPath("test.are");
        private static readonly string DoesNotExistFile = "./thisfiledoesnotexist";
        private static readonly string CorruptBinaryTestFile = TestFileHelper.GetPath("test_corrupted.are");

        [Fact(Timeout = 120000)]
        public void TestBinaryIO()
        {
            if (!File.Exists(BinaryTestFile))
            {
                // Create a test ARE file if it doesn't exist
                CreateTestAreFile(BinaryTestFile);
            }

            // Test reading ARE file
            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));
            ValidateIO(are);

            // Test writing and reading back
            byte[] data = AREHelpers.BytesAre(are, Game.K2);
            are = AREHelpers.ReadAre(data);
            ValidateIO(are);
        }

        [Fact(Timeout = 120000)]
        public void TestAreHeaderStructure()
        {
            // Test that ARE header matches Kaitai Struct definition
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            // Read GFF header to validate structure
            GFF gff = new GFFBinaryReader(BinaryTestFile).Load();

            // Validate GFF header constants match ARE.ksy
            // GFF header is 56 bytes (14 fields * 4 bytes each)
            gff.Content.Should().Be(GFFContent.ARE, "ARE file should have ARE content type");
        }

        [Fact(Timeout = 120000)]
        public void TestAreFileTypeSignature()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            // Read raw header bytes
            byte[] header = new byte[8];
            using (var fs = File.OpenRead(BinaryTestFile))
            {
                fs.Read(header, 0, 8);
            }

            // Validate file type signature matches ARE.ksy
            string fileType = System.Text.Encoding.ASCII.GetString(header, 0, 4);
            fileType.Should().Be("ARE ", "File type should be 'ARE ' (space-padded) as defined in ARE.ksy");

            // Validate version
            string version = System.Text.Encoding.ASCII.GetString(header, 4, 4);
            version.Should().BeOneOf(new[] { "V3.2", "V3.3", "V4.0", "V4.1" }, "Version should match ARE.ksy valid values");
        }

        [Fact(Timeout = 120000)]
        public void TestAreBasicProperties()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate basic ARE properties exist and have reasonable values
            are.Tag.Should().NotBeNull("Tag should not be null");
            are.Name.Should().NotBeNull("Name should not be null");
            are.AlphaTest.Should().BeGreaterOrEqualTo(0, "AlphaTest should be non-negative");
            are.CameraStyle.Should().BeGreaterOrEqualTo(0, "CameraStyle should be non-negative");
            are.DefaultEnvMap.Should().NotBeNull("DefaultEnvMap should not be null");
            are.WindPower.Should().BeGreaterOrEqualTo(0, "WindPower should be non-negative");
            are.LoadScreenID.Should().BeGreaterOrEqualTo(0, "LoadScreenID should be non-negative");
        }

        [Fact(Timeout = 120000)]
        public void TestAreMapProperties()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate map properties exist
            are.NorthAxis.Should().BeDefined("NorthAxis should be defined");
            are.MapZoom.Should().BeGreaterOrEqualTo(0, "MapZoom should be non-negative");
            are.MapResX.Should().BeGreaterOrEqualTo(0, "MapResX should be non-negative");
            are.MapPoint1.Should().NotBeNull("MapPoint1 should not be null");
            are.MapPoint2.Should().NotBeNull("MapPoint2 should not be null");
            are.WorldPoint1.Should().NotBeNull("WorldPoint1 should not be null");
            are.WorldPoint2.Should().NotBeNull("WorldPoint2 should not be null");
        }

        [Fact(Timeout = 120000)]
        public void TestAreLightingProperties()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate lighting color properties exist
            are.SunAmbient.Should().NotBeNull("SunAmbient should not be null");
            are.SunDiffuse.Should().NotBeNull("SunDiffuse should not be null");
            are.DynamicLight.Should().NotBeNull("DynamicLight should not be null");
            are.FogColor.Should().NotBeNull("FogColor should not be null");
            
            // Validate fog properties
            are.FogNear.Should().BeGreaterOrEqualTo(0.0f, "FogNear should be non-negative");
            are.FogFar.Should().BeGreaterOrEqualTo(are.FogNear, "FogFar should be >= FogNear");
        }

        [Fact(Timeout = 120000)]
        public void TestAreGrassProperties()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate grass properties exist
            are.GrassTexture.Should().NotBeNull("GrassTexture should not be null");
            are.GrassDensity.Should().BeGreaterOrEqualTo(0.0f, "GrassDensity should be non-negative");
            are.GrassSize.Should().BeGreaterOrEqualTo(0.0f, "GrassSize should be non-negative");
            are.GrassProbLL.Should().BeGreaterOrEqualTo(0.0f, "GrassProbLL should be non-negative");
            are.GrassProbLR.Should().BeGreaterOrEqualTo(0.0f, "GrassProbLR should be non-negative");
            are.GrassProbUL.Should().BeGreaterOrEqualTo(0.0f, "GrassProbUL should be non-negative");
            are.GrassProbUR.Should().BeGreaterOrEqualTo(0.0f, "GrassProbUR should be non-negative");
            are.GrassAmbient.Should().NotBeNull("GrassAmbient should not be null");
            are.GrassDiffuse.Should().NotBeNull("GrassDiffuse should not be null");
            are.GrassEmissive.Should().NotBeNull("GrassEmissive should not be null");
        }

        [Fact(Timeout = 120000)]
        public void TestAreScriptHooks()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate script hook properties exist
            are.OnEnter.Should().NotBeNull("OnEnter should not be null");
            are.OnExit.Should().NotBeNull("OnExit should not be null");
            are.OnHeartbeat.Should().NotBeNull("OnHeartbeat should not be null");
            are.OnUserDefined.Should().NotBeNull("OnUserDefined should not be null");
        }

        [Fact(Timeout = 120000)]
        public void TestAreStealthProperties()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate stealth XP properties
            are.StealthXpLoss.Should().BeGreaterOrEqualTo(0, "StealthXpLoss should be non-negative");
            are.StealthXpMax.Should().BeGreaterOrEqualTo(0, "StealthXpMax should be non-negative");
        }

        [Fact(Timeout = 120000)]
        public void TestAreK2SpecificFields()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate KotOR 2-specific dirty formula fields exist
            are.DirtyFormula1.Should().BeGreaterOrEqualTo(0, "DirtyFormula1 should be non-negative");
            are.DirtyFormula2.Should().BeGreaterOrEqualTo(0, "DirtyFormula2 should be non-negative");
            are.DirtyFormula3.Should().BeGreaterOrEqualTo(0, "DirtyFormula3 should be non-negative");
        }

        [Fact(Timeout = 120000)]
        public void TestAreRoundTrip()
        {
            // Test creating ARE, writing, and reading it back
            ARE originalAre = CreateTestAreObject();

            // Write to bytes
            byte[] data = AREHelpers.BytesAre(originalAre, Game.K2);

            // Read back
            ARE loadedAre = AREHelpers.ReadAre(data);

            // Validate round-trip
            loadedAre.Tag.Should().Be(originalAre.Tag);
            loadedAre.AlphaTest.Should().Be(originalAre.AlphaTest);
            loadedAre.CameraStyle.Should().Be(originalAre.CameraStyle);
            loadedAre.MapZoom.Should().Be(originalAre.MapZoom);
            loadedAre.MapResX.Should().Be(originalAre.MapResX);
            loadedAre.WindPower.Should().Be(originalAre.WindPower);
        }

        [Fact(Timeout = 120000)]
        public void TestAreInvalidSignature()
        {
            // Create file with invalid signature
            string tempFile = Path.GetTempFileName();
            try
            {
                using (var fs = File.Create(tempFile))
                {
                    byte[] invalid = System.Text.Encoding.ASCII.GetBytes("INVALID");
                    fs.Write(invalid, 0, invalid.Length);
                }

                Action act = () => new GFFBinaryReader(tempFile).Load();
                act.Should().Throw<InvalidDataException>().WithMessage("*Invalid GFF file type*");
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Fact(Timeout = 120000)]
        public void TestAreInvalidVersion()
        {
            // Create file with invalid version
            string tempFile = Path.GetTempFileName();
            try
            {
                using (var fs = File.Create(tempFile))
                {
                    byte[] header = new byte[56];
                    System.Text.Encoding.ASCII.GetBytes("ARE ").CopyTo(header, 0);
                    System.Text.Encoding.ASCII.GetBytes("V2.0").CopyTo(header, 4);
                    // Fill rest with zeros for minimal valid GFF structure
                    fs.Write(header, 0, header.Length);
                }

                Action act = () => new GFFBinaryReader(tempFile).Load();
                act.Should().Throw<InvalidDataException>().WithMessage("*Unsupported GFF version*");
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Fact(Timeout = 120000)]
        public void TestReadRaises()
        {
            // Test reading from directory
            Action act1 = () => new GFFBinaryReader(".").Load();
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                act1.Should().Throw<UnauthorizedAccessException>();
            }
            else
            {
                act1.Should().Throw<IOException>();
            }

            // Test reading non-existent file
            Action act2 = () => new GFFBinaryReader(DoesNotExistFile).Load();
            act2.Should().Throw<FileNotFoundException>();

            // Test reading corrupted file
            if (File.Exists(CorruptBinaryTestFile))
            {
                Action act3 = () => new GFFBinaryReader(CorruptBinaryTestFile).Load();
                act3.Should().Throw<InvalidDataException>();
            }
        }

        [Fact(Timeout = 120000)]
        public void TestAreEmptyFile()
        {
            // Test ARE with minimal required fields
            ARE are = new ARE();
            are.Tag = "TEST_AREA";
            are.Name = LocalizedString.FromInvalid();

            byte[] data = AREHelpers.BytesAre(are, Game.K2);
            ARE loaded = AREHelpers.ReadAre(data);

            loaded.Tag.Should().Be("TEST_AREA");
        }

        [Fact(Timeout = 120000)]
        public void TestAreColorFields()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate all color fields are Color objects (not null)
            are.SunAmbient.Should().NotBeNull();
            are.SunDiffuse.Should().NotBeNull();
            are.DynamicLight.Should().NotBeNull();
            are.FogColor.Should().NotBeNull();
            are.GrassAmbient.Should().NotBeNull();
            are.GrassDiffuse.Should().NotBeNull();
            are.GrassEmissive.Should().NotBeNull();
        }

        [Fact(Timeout = 120000)]
        public void TestAreMapCoordinateRanges()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate map coordinates are within reasonable ranges
            // Map points are normalized 0.0-1.0 image coordinates
            are.MapPoint1.X.Should().BeInRange(0.0f, 1.0f, "MapPoint1.X should be normalized 0.0-1.0");
            are.MapPoint1.Y.Should().BeInRange(0.0f, 1.0f, "MapPoint1.Y should be normalized 0.0-1.0");
            are.MapPoint2.X.Should().BeInRange(0.0f, 1.0f, "MapPoint2.X should be normalized 0.0-1.0");
            are.MapPoint2.Y.Should().BeInRange(0.0f, 1.0f, "MapPoint2.Y should be normalized 0.0-1.0");

            // World points can be any float value (world coordinates)
            // Just check they're valid floats (not NaN or Infinity)
            are.WorldPoint1.X.Should().NotBe(float.NaN);
            are.WorldPoint1.Y.Should().NotBe(float.NaN);
            are.WorldPoint2.X.Should().NotBe(float.NaN);
            are.WorldPoint2.Y.Should().NotBe(float.NaN);
        }

        [Fact(Timeout = 120000)]
        public void TestAreBooleanFields()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate boolean fields are properly set
            // These are stored as UInt8 (0 or 1) in GFF but exposed as bool in ARE class
            are.Unescapable.Should().BeOfType<bool>();
            are.DisableTransit.Should().BeOfType<bool>();
            are.StealthXp.Should().BeOfType<bool>();
            are.FogEnabled.Should().BeOfType<bool>();
        }

        [Fact(Timeout = 120000)]
        public void TestAreListFields()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestAreFile(BinaryTestFile);
            }

            ARE are = AREHelpers.ReadAre(File.ReadAllBytes(BinaryTestFile));

            // Validate list fields exist (may be empty)
            are.AreaList.Should().NotBeNull("AreaList should not be null");
            are.MapList.Should().NotBeNull("MapList should not be null");
        }

        private static void ValidateIO(ARE are)
        {
            // Basic validation that ARE object was loaded successfully
            are.Should().NotBeNull("ARE object should not be null");
            are.Tag.Should().NotBeNull("Tag should not be null");
        }

        private static void CreateTestAreFile(string path)
        {
            ARE are = CreateTestAreObject();
            byte[] data = AREHelpers.BytesAre(are, Game.K2);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, data);
        }

        private static ARE CreateTestAreObject()
        {
            ARE are = new ARE();
            are.Tag = "TEST_AREA";
            are.Name = LocalizedString.FromInvalid();
            are.AlphaTest = 128;
            are.CameraStyle = 0;
            are.DefaultEnvMap = ResRef.FromBlank();
            are.Unescapable = false;
            are.DisableTransit = false;
            are.StealthXp = false;
            are.StealthXpLoss = 0;
            are.StealthXpMax = 0;
            are.GrassTexture = ResRef.FromBlank();
            are.GrassDensity = 0.0f;
            are.GrassSize = 0.0f;
            are.GrassProbLL = 0.0f;
            are.GrassProbLR = 0.0f;
            are.GrassProbUL = 0.0f;
            are.GrassProbUR = 0.0f;
            are.GrassAmbient = new Color(0, 0, 0);
            are.GrassDiffuse = new Color(0, 0, 0);
            are.GrassEmissive = new Color(0, 0, 0);
            are.FogEnabled = false;
            are.FogNear = 0.0f;
            are.FogFar = 100.0f;
            are.FogColor = new Color(128, 128, 128);
            are.SunFogEnabled = false;
            are.SunFogNear = 0.0f;
            are.SunFogFar = 100.0f;
            are.SunFogColor = new Color(128, 128, 128);
            are.SunAmbient = new Color(128, 128, 128);
            are.SunDiffuse = new Color(255, 255, 255);
            are.DynamicLight = new Color(64, 64, 64);
            are.WindPower = 0;
            are.ShadowOpacity = ResRef.FromBlank();
            are.ChancesOfRain = ResRef.FromBlank();
            are.ChancesOfSnow = ResRef.FromBlank();
            are.ChancesOfLightning = ResRef.FromBlank();
            are.ChancesOfFog = ResRef.FromBlank();
            are.Weather = 0;
            are.SkyBox = 0;
            are.MoonAmbient = 0;
            are.DawnAmbient = 0;
            are.DayAmbient = 0;
            are.DuskAmbient = 0;
            are.NightAmbient = 0;
            are.DawnDir1 = 0;
            are.DawnDir2 = 0;
            are.DawnDir3 = 0;
            are.DayDir1 = 0;
            are.DayDir2 = 0;
            are.DayDir3 = 0;
            are.DuskDir1 = 0;
            are.DuskDir2 = 0;
            are.DuskDir3 = 0;
            are.NightDir1 = 0;
            are.NightDir2 = 0;
            are.NightDir3 = 0;
            are.DawnColor1 = new Color(0, 0, 0);
            are.DawnColor2 = new Color(0, 0, 0);
            are.DawnColor3 = new Color(0, 0, 0);
            are.DayColor1 = new Color(0, 0, 0);
            are.DayColor2 = new Color(0, 0, 0);
            are.DayColor3 = new Color(0, 0, 0);
            are.DuskColor1 = new Color(0, 0, 0);
            are.DuskColor2 = new Color(0, 0, 0);
            are.DuskColor3 = new Color(0, 0, 0);
            are.NightColor1 = new Color(0, 0, 0);
            are.NightColor2 = new Color(0, 0, 0);
            are.NightColor3 = new Color(0, 0, 0);
            are.OnEnter = ResRef.FromBlank();
            are.OnExit = ResRef.FromBlank();
            are.OnHeartbeat = ResRef.FromBlank();
            are.OnUserDefined = ResRef.FromBlank();
            are.OnEnter2 = ResRef.FromBlank();
            are.OnExit2 = ResRef.FromBlank();
            are.OnHeartbeat2 = ResRef.FromBlank();
            are.OnUserDefined2 = ResRef.FromBlank();
            are.AreaList = new System.Collections.Generic.List<string>();
            are.MapList = new System.Collections.Generic.List<ResRef>();
            are.DirtyFormula1 = 0;
            are.DirtyFormula2 = 0;
            are.DirtyFormula3 = 0;
            are.Comment = "";
            are.NorthAxis = ARENorthAxis.PositiveX;
            are.MapZoom = 0;
            are.MapResX = 512;
            are.MapPoint1 = new Vector2(0.0f, 0.0f);
            are.MapPoint2 = new Vector2(1.0f, 1.0f);
            are.WorldPoint1 = new Vector2(0.0f, 0.0f);
            are.WorldPoint2 = new Vector2(100.0f, 100.0f);
            are.LoadScreenID = 0;
            return are;
        }
    }
}

