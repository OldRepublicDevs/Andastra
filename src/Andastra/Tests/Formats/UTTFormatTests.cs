using System;
using System.IO;
using System.Linq;
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
    /// Comprehensive tests for UTT binary I/O operations.
    /// Tests validate the UTT format structure as defined in UTT.ksy Kaitai Struct definition.
    /// UTT files are GFF-based format files with file type signature "UTT ".
    /// </summary>
    public class UTTFormatTests
    {
        private static readonly string BinaryTestFile = TestFileHelper.GetPath("test.utt");
        private static readonly string DoesNotExistFile = "./thisfiledoesnotexist";
        private static readonly string CorruptBinaryTestFile = TestFileHelper.GetPath("test_corrupted.utt");

        [Fact(Timeout = 120000)]
        public void TestBinaryIO()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestUttFile(BinaryTestFile);
            }

            // Test reading UTT file
            UTT utt = UTTAuto.ReadUtt(File.ReadAllBytes(BinaryTestFile));
            ValidateIO(utt);

            // Test writing and reading back
            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            utt = UTTAuto.ReadUtt(data);
            ValidateIO(utt);
        }

        [Fact(Timeout = 120000)]
        public void TestUttGffHeaderStructure()
        {
            // Test that UTT GFF header matches Kaitai Struct definition
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestUttFile(BinaryTestFile);
            }

            // Read raw GFF header bytes (56 bytes)
            byte[] header = new byte[56];
            using (var fs = File.OpenRead(BinaryTestFile))
            {
                fs.Read(header, 0, 56);
            }

            // Validate file type signature matches UTT.ksy
            string fileType = System.Text.Encoding.ASCII.GetString(header, 0, 4);
            fileType.Should().Be("UTT ", "File type should be 'UTT ' (space-padded) as defined in UTT.ksy");

            // Validate version
            string version = System.Text.Encoding.ASCII.GetString(header, 4, 4);
            version.Should().BeOneOf("V3.2", "V3.3", "V4.0", "V4.1", "Version should match UTT.ksy valid values");

            // Validate header structure offsets (all should be non-negative and reasonable)
            uint structArrayOffset = BitConverter.ToUInt32(header, 8);
            uint structCount = BitConverter.ToUInt32(header, 12);
            uint fieldArrayOffset = BitConverter.ToUInt32(header, 16);
            uint fieldCount = BitConverter.ToUInt32(header, 20);
            uint labelArrayOffset = BitConverter.ToUInt32(header, 24);
            uint labelCount = BitConverter.ToUInt32(header, 28);
            uint fieldDataOffset = BitConverter.ToUInt32(header, 32);
            uint fieldDataCount = BitConverter.ToUInt32(header, 36);
            uint fieldIndicesOffset = BitConverter.ToUInt32(header, 40);
            uint fieldIndicesCount = BitConverter.ToUInt32(header, 44);
            uint listIndicesOffset = BitConverter.ToUInt32(header, 48);
            uint listIndicesCount = BitConverter.ToUInt32(header, 52);

            structArrayOffset.Should().BeGreaterOrEqualTo(56, "Struct array offset should be >= 56 (after header)");
            fieldArrayOffset.Should().BeGreaterOrEqualTo(56, "Field array offset should be >= 56 (after header)");
            labelArrayOffset.Should().BeGreaterOrEqualTo(56, "Label array offset should be >= 56 (after header)");
            fieldDataOffset.Should().BeGreaterOrEqualTo(56, "Field data offset should be >= 56 (after header)");
            fieldIndicesOffset.Should().BeGreaterOrEqualTo(56, "Field indices offset should be >= 56 (after header)");
            listIndicesOffset.Should().BeGreaterOrEqualTo(56, "List indices offset should be >= 56 (after header)");

            structCount.Should().BeGreaterOrEqualTo(1, "Struct count should be >= 1 (root struct always present)");
        }

        [Fact(Timeout = 120000)]
        public void TestUttFileTypeSignature()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestUttFile(BinaryTestFile);
            }

            // Read raw header bytes
            byte[] header = new byte[8];
            using (var fs = File.OpenRead(BinaryTestFile))
            {
                fs.Read(header, 0, 8);
            }

            // Validate file type signature matches UTT.ksy
            string fileType = System.Text.Encoding.ASCII.GetString(header, 0, 4);
            fileType.Should().Be("UTT ", "File type should be 'UTT ' (space-padded) as defined in UTT.ksy");

            // Validate version
            string version = System.Text.Encoding.ASCII.GetString(header, 4, 4);
            version.Should().BeOneOf("V3.2", "V3.3", "V4.0", "V4.1", "Version should match UTT.ksy valid values");
        }

        [Fact(Timeout = 120000)]
        public void TestUttInvalidSignature()
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

                Action act = () => UTTAuto.ReadUtt(File.ReadAllBytes(tempFile));
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
        public void TestUttInvalidVersion()
        {
            // Create file with invalid version
            string tempFile = Path.GetTempFileName();
            try
            {
                using (var fs = File.Create(tempFile))
                {
                    byte[] header = new byte[56];
                    System.Text.Encoding.ASCII.GetBytes("UTT ").CopyTo(header, 0);
                    System.Text.Encoding.ASCII.GetBytes("V2.0").CopyTo(header, 4);
                    fs.Write(header, 0, header.Length);
                }

                Action act = () => UTTAuto.ReadUtt(File.ReadAllBytes(tempFile));
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
        public void TestUttRootStructFields()
        {
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestUttFile(BinaryTestFile);
            }

            UTT utt = UTTAuto.ReadUtt(File.ReadAllBytes(BinaryTestFile));

            // Validate root struct fields exist (as per UTT.ksy documentation)
            GFF gff = GFF.FromBytes(File.ReadAllBytes(BinaryTestFile));
            GFFStruct root = gff.Root;

            // Core UTT fields should be present or have defaults
            root.Acquire<ResRef>("TemplateResRef", ResRef.FromBlank()).Should().NotBeNull("TemplateResRef should not be null");
            root.Acquire<string>("Tag", "").Should().NotBeNull("Tag should not be null");
            root.Acquire<LocalizedString>("LocalizedName", LocalizedString.FromInvalid()).Should().NotBeNull("LocalizedName should not be null");
            root.Acquire<string>("KeyName", "").Should().NotBeNull("KeyName should not be null");
            root.Acquire<int>("Type", 0).Should().BeGreaterOrEqualTo(0, "Type should be non-negative");
            
            // TrapFlag should be valid byte (0 or 1)
            byte? trapFlag = root.GetUInt8("TrapFlag");
            if (trapFlag.HasValue)
            {
                trapFlag.Value.Should().BeInRange((byte)0, (byte)1, "TrapFlag should be 0 or 1");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestUttTrapFlag()
        {
            // Test TrapFlag field (0 = no trap, 1 = trap)
            if (!File.Exists(BinaryTestFile))
            {
                CreateTestUttFile(BinaryTestFile);
            }

            var testCases = new[]
            {
                (flag: (byte)0, isTrap: false),
                (flag: (byte)1, isTrap: true),
            };

            foreach (var testCase in testCases)
            {
                var testUtt = new UTT();
                testUtt.IsTrap = testCase.isTrap;

                byte[] data = UTTAuto.BytesUtt(testUtt, Game.K2);
                GFF gff = GFF.FromBytes(data);
                byte trapFlag = gff.Root.GetUInt8("TrapFlag").Value;

                trapFlag.Should().Be(testCase.flag, $"TrapFlag should be {testCase.flag} for IsTrap={testCase.isTrap}");

                // Round-trip test
                UTT loaded = UTTAuto.ReadUtt(data);
                loaded.IsTrap.Should().Be(testCase.isTrap, "IsTrap should match after round-trip");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestUttTriggerType()
        {
            // Test Type field (trigger type identifier)
            var utt = new UTT();
            utt.ResRef = new ResRef("test_trigger");
            utt.Tag = "TESTTRIG";
            utt.TypeId = 7; // Trigger type

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.TypeId.Should().Be(7, "TypeId should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttScriptHooks()
        {
            // Test script hook fields
            var utt = new UTT();
            utt.OnEnterScript = new ResRef("enter_script");
            utt.OnExitScript = new ResRef("exit_script");
            utt.OnClickScript = new ResRef("click_script");
            utt.OnTrapTriggeredScript = new ResRef("trap_script");
            utt.OnHeartbeatScript = new ResRef("heartbeat_script");
            utt.OnUserDefinedScript = new ResRef("user_script");
            utt.OnDisarmScript = new ResRef("disarm_script");

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.OnEnterScript.Should().Be(new ResRef("enter_script"), "OnEnterScript should match");
            loaded.OnExitScript.Should().Be(new ResRef("exit_script"), "OnExitScript should match");
            loaded.OnClickScript.Should().Be(new ResRef("click_script"), "OnClickScript should match");
            loaded.OnTrapTriggeredScript.Should().Be(new ResRef("trap_script"), "OnTrapTriggeredScript should match");
            loaded.OnHeartbeatScript.Should().Be(new ResRef("heartbeat_script"), "OnHeartbeatScript should match");
            loaded.OnUserDefinedScript.Should().Be(new ResRef("user_script"), "OnUserDefinedScript should match");
            loaded.OnDisarmScript.Should().Be(new ResRef("disarm_script"), "OnDisarmScript should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttTrapProperties()
        {
            // Test trap-related properties
            var utt = new UTT();
            utt.IsTrap = true;
            utt.TrapDetectable = true;
            utt.TrapDetectDc = 20;
            utt.TrapDisarmable = true;
            utt.TrapDisarmDc = 25;
            utt.TrapType = 3;
            utt.TrapOnce = true;

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.IsTrap.Should().BeTrue("IsTrap should match");
            loaded.TrapDetectable.Should().BeTrue("TrapDetectable should match");
            loaded.TrapDetectDc.Should().Be(20, "TrapDetectDc should match");
            loaded.TrapDisarmable.Should().BeTrue("TrapDisarmable should match");
            loaded.TrapDisarmDc.Should().Be(25, "TrapDisarmDc should match");
            loaded.TrapType.Should().Be(3, "TrapType should match");
            loaded.TrapOnce.Should().BeTrue("TrapOnce should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttEmptyFile()
        {
            // Test UTT with minimal structure
            var utt = new UTT();
            utt.ResRef = ResRef.FromBlank();
            utt.Name = LocalizedString.FromInvalid();
            utt.Tag = "";
            utt.TypeId = 0;
            utt.IsTrap = false;

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.ResRef.Should().Be(ResRef.FromBlank());
            loaded.Tag.Should().Be("");
            loaded.TypeId.Should().Be(0);
            loaded.IsTrap.Should().BeFalse();
        }

        [Fact(Timeout = 120000)]
        public void TestUttKeyName()
        {
            // Test KeyName field
            var utt = new UTT();
            utt.KeyName = "REQUIRED_KEY";

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.KeyName.Should().Be("REQUIRED_KEY", "KeyName should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttAutoRemoveKey()
        {
            // Test AutoRemoveKey field
            var utt = new UTT();
            utt.AutoRemoveKey = false;

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.AutoRemoveKey.Should().BeFalse("AutoRemoveKey should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttLocalizedStringName()
        {
            // Test LocalizedName field (LocalizedString)
            var utt = new UTT();
            utt.Name = LocalizedString.FromEnglish("English Trigger Name");
            utt.Name.Set(Language.German, Gender.Male, "Deutscher Triggername");

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.Name.Should().NotBeNull("Name should not be null");
            loaded.Name.Get(Language.English, Gender.Male).Should().Be("English Trigger Name", "English name should match");
            loaded.Name.Get(Language.German, Gender.Male).Should().Be("Deutscher Triggername", "German name should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttCommentField()
        {
            // Test Comment field
            var utt = new UTT();
            utt.Comment = "This is a test trigger comment";

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.Comment.Should().Be("This is a test trigger comment", "Comment should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttFactionAndCursor()
        {
            // Test Faction and Cursor fields
            var utt = new UTT();
            utt.FactionId = 5;
            utt.Cursor = 2;
            utt.HighlightHeight = 10.5f;

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.FactionId.Should().Be(5, "FactionId should match");
            loaded.Cursor.Should().Be(2, "Cursor should match");
            loaded.HighlightHeight.Should().BeApproximately(10.5f, 0.01f, "HighlightHeight should match");
        }

        [Fact(Timeout = 120000)]
        public void TestReadRaises()
        {
            // Test reading from directory
            Action act1 = () => UTTAuto.ReadUtt(File.ReadAllBytes("."));
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                act1.Should().Throw<UnauthorizedAccessException>();
            }
            else
            {
                act1.Should().Throw<IOException>();
            }

            // Test reading non-existent file
            Action act2 = () => UTTAuto.ReadUtt(File.ReadAllBytes(DoesNotExistFile));
            act2.Should().Throw<FileNotFoundException>();

            // Test reading corrupted file
            if (File.Exists(CorruptBinaryTestFile))
            {
                Action act3 = () => UTTAuto.ReadUtt(File.ReadAllBytes(CorruptBinaryTestFile));
                act3.Should().Throw<InvalidDataException>();
            }
        }

        [Fact(Timeout = 120000)]
        public void TestUttRoundTripPreservesStructure()
        {
            // Create complex UTT structure
            var utt = new UTT();
            utt.ResRef = new ResRef("complex_trigger");
            utt.Name = LocalizedString.FromEnglish("Complex Trigger");
            utt.Tag = "COMPLEX";
            utt.TypeId = 7;
            utt.Comment = "Complex trigger with all properties";
            utt.IsTrap = true;
            utt.TrapDetectable = true;
            utt.TrapDetectDc = 20;
            utt.TrapDisarmable = true;
            utt.TrapDisarmDc = 25;
            utt.TrapType = 3;
            utt.TrapOnce = true;
            utt.OnEnterScript = new ResRef("complex_enter");
            utt.OnExitScript = new ResRef("complex_exit");
            utt.OnClickScript = new ResRef("complex_click");
            utt.OnTrapTriggeredScript = new ResRef("complex_trap");
            utt.KeyName = "COMPLEX_KEY";
            utt.AutoRemoveKey = true;
            utt.FactionId = 5;
            utt.Cursor = 2;
            utt.HighlightHeight = 15.0f;

            // Round-trip test
            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.ResRef.Should().Be(new ResRef("complex_trigger"));
            loaded.Tag.Should().Be("COMPLEX");
            loaded.TypeId.Should().Be(7);
            loaded.Comment.Should().Be("Complex trigger with all properties");
            loaded.IsTrap.Should().BeTrue();
            loaded.TrapDetectable.Should().BeTrue();
            loaded.TrapDetectDc.Should().Be(20);
            loaded.TrapDisarmable.Should().BeTrue();
            loaded.TrapDisarmDc.Should().Be(25);
            loaded.TrapType.Should().Be(3);
            loaded.TrapOnce.Should().BeTrue();
            loaded.OnEnterScript.Should().Be(new ResRef("complex_enter"));
            loaded.OnExitScript.Should().Be(new ResRef("complex_exit"));
            loaded.OnClickScript.Should().Be(new ResRef("complex_click"));
            loaded.OnTrapTriggeredScript.Should().Be(new ResRef("complex_trap"));
            loaded.KeyName.Should().Be("COMPLEX_KEY");
            loaded.AutoRemoveKey.Should().BeTrue();
            loaded.FactionId.Should().Be(5);
            loaded.Cursor.Should().Be(2);
            loaded.HighlightHeight.Should().BeApproximately(15.0f, 0.01f);
        }

        [Fact(Timeout = 120000)]
        public void TestUttAllFieldsRoundTrip()
        {
            // Test all UTT fields in a comprehensive round-trip
            var utt = new UTT();
            utt.ResRef = new ResRef("all_fields_trigger");
            utt.Name = LocalizedString.FromEnglish("All Fields Trigger");
            utt.Name.Set(Language.French, Gender.Female, "Déclencheur Tous Champs");
            utt.Tag = "ALLFIELDS";
            utt.TypeId = 7;
            utt.Comment = "Trigger with all fields set";
            utt.IsTrap = true;
            utt.TrapDetectable = true;
            utt.TrapDetectDc = 20;
            utt.TrapDisarmable = true;
            utt.TrapDisarmDc = 25;
            utt.TrapType = 3;
            utt.TrapOnce = true;
            utt.OnEnterScript = new ResRef("all_enter");
            utt.OnExitScript = new ResRef("all_exit");
            utt.OnClickScript = new ResRef("all_click");
            utt.OnTrapTriggeredScript = new ResRef("all_trap");
            utt.OnHeartbeatScript = new ResRef("all_heartbeat");
            utt.OnUserDefinedScript = new ResRef("all_user");
            utt.OnDisarmScript = new ResRef("all_disarm");
            utt.KeyName = "ALL_KEY";
            utt.AutoRemoveKey = true;
            utt.FactionId = 5;
            utt.Cursor = 2;
            utt.HighlightHeight = 20.0f;
            utt.PortraitId = 10;
            utt.LoadscreenId = 5;
            utt.PaletteId = 3;

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            // Validate all fields
            loaded.ResRef.Should().Be(utt.ResRef);
            loaded.Name.Get(Language.English, Gender.Male).Should().Be(utt.Name.Get(Language.English, Gender.Male));
            loaded.Name.Get(Language.French, Gender.Female).Should().Be(utt.Name.Get(Language.French, Gender.Female));
            loaded.Tag.Should().Be(utt.Tag);
            loaded.TypeId.Should().Be(utt.TypeId);
            loaded.Comment.Should().Be(utt.Comment);
            loaded.IsTrap.Should().Be(utt.IsTrap);
            loaded.TrapDetectable.Should().Be(utt.TrapDetectable);
            loaded.TrapDetectDc.Should().Be(utt.TrapDetectDc);
            loaded.TrapDisarmable.Should().Be(utt.TrapDisarmable);
            loaded.TrapDisarmDc.Should().Be(utt.TrapDisarmDc);
            loaded.TrapType.Should().Be(utt.TrapType);
            loaded.TrapOnce.Should().Be(utt.TrapOnce);
            loaded.OnEnterScript.Should().Be(utt.OnEnterScript);
            loaded.OnExitScript.Should().Be(utt.OnExitScript);
            loaded.OnClickScript.Should().Be(utt.OnClickScript);
            loaded.OnTrapTriggeredScript.Should().Be(utt.OnTrapTriggeredScript);
            loaded.OnHeartbeatScript.Should().Be(utt.OnHeartbeatScript);
            loaded.OnUserDefinedScript.Should().Be(utt.OnUserDefinedScript);
            loaded.OnDisarmScript.Should().Be(utt.OnDisarmScript);
            loaded.KeyName.Should().Be(utt.KeyName);
            loaded.AutoRemoveKey.Should().Be(utt.AutoRemoveKey);
            loaded.FactionId.Should().Be(utt.FactionId);
            loaded.Cursor.Should().Be(utt.Cursor);
            loaded.HighlightHeight.Should().BeApproximately(utt.HighlightHeight, 0.01f);
        }

        private static void ValidateIO(UTT utt)
        {
            // Basic validation
            utt.Should().NotBeNull();
            utt.ResRef.Should().NotBeNull();
            utt.Name.Should().NotBeNull();
            utt.TypeId.Should().BeGreaterOrEqualTo(0);
        }

        private static void CreateTestUttFile(string path)
        {
            var utt = new UTT();
            utt.ResRef = new ResRef("test_trigger");
            utt.Name = LocalizedString.FromEnglish("Test Trigger");
            utt.Tag = "TEST";
            utt.TypeId = 7;
            utt.IsTrap = false;
            utt.Comment = "Test trigger comment";
            utt.OnEnterScript = new ResRef("test_enter");
            utt.OnExitScript = new ResRef("test_exit");

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, data);
        }
    }
}

