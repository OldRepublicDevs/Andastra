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
            root.Acquire<uint>("Type", 0).Should().BeInRange(0u, 7u, "Type should be 0-7 (trigger type enum)");

            // TrapFlag should be valid byte (0 or 1)
            byte? trapFlag = root.GetUInt8("TrapFlag");
            if (trapFlag.HasValue)
            {
                trapFlag.Value.Should().BeInRange((byte)0, (byte)1, "TrapFlag should be 0 or 1");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestUttTriggerTypeEnum()
        {
            // Test trigger type values (0-7 as per UTT.ksy documentation)
            var testCases = new[]
            {
                (type: 0u, name: "Generic"),
                (type: 1u, name: "Waypoint"),
                (type: 2u, name: "Door"),
                (type: 3u, name: "Placeable"),
                (type: 4u, name: "Store"),
                (type: 5u, name: "Area of Effect"),
                (type: 6u, name: "Encounter"),
                (type: 7u, name: "Trigger"),
            };

            foreach (var testCase in testCases)
            {
                var utt = new UTT();
                utt.TypeId = (int)testCase.type;

                byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
                UTT loaded = UTTAuto.ReadUtt(data);

                loaded.TypeId.Should().Be((int)testCase.type, $"Type should be {testCase.type} ({testCase.name})");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestUttTrapFlag()
        {
            // Test TrapFlag field (0 = no trap, 1 = trap)
            var utt1 = new UTT();
            utt1.IsTrap = false;

            byte[] data1 = UTTAuto.BytesUtt(utt1, Game.K2);
            GFF gff1 = GFF.FromBytes(data1);
            gff1.Root.GetUInt8("TrapFlag").Should().Be(0, "TrapFlag should be 0 when IsTrap is false");

            var utt2 = new UTT();
            utt2.IsTrap = true;

            byte[] data2 = UTTAuto.BytesUtt(utt2, Game.K2);
            GFF gff2 = GFF.FromBytes(data2);
            gff2.Root.GetUInt8("TrapFlag").Should().Be(1, "TrapFlag should be 1 when IsTrap is true");

            // Round-trip test
            UTT loaded1 = UTTAuto.ReadUtt(data1);
            loaded1.IsTrap.Should().BeFalse("IsTrap should be false after round-trip");

            UTT loaded2 = UTTAuto.ReadUtt(data2);
            loaded2.IsTrap.Should().BeTrue("IsTrap should be true after round-trip");
        }

        [Fact(Timeout = 120000)]
        public void TestUttScriptFields()
        {
            // Test script ResRef fields
            var utt = new UTT();
            utt.OnTrapTriggeredScript = new ResRef("trap_script");
            utt.OnClickScript = new ResRef("click_script");
            utt.OnHeartbeatScript = new ResRef("heartbeat_script");
            utt.OnEnterScript = new ResRef("enter_script");
            utt.OnExitScript = new ResRef("exit_script");
            utt.OnUserDefinedScript = new ResRef("user_script");
            utt.OnDisarmScript = new ResRef("disarm_script");

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.OnTrapTriggeredScript.Should().Be(new ResRef("trap_script"), "OnTrapTriggeredScript should match");
            loaded.OnClickScript.Should().Be(new ResRef("click_script"), "OnClickScript should match");
            loaded.OnHeartbeatScript.Should().Be(new ResRef("heartbeat_script"), "OnHeartbeatScript should match");
            loaded.OnEnterScript.Should().Be(new ResRef("enter_script"), "OnEnterScript should match");
            loaded.OnExitScript.Should().Be(new ResRef("exit_script"), "OnExitScript should match");
            loaded.OnUserDefinedScript.Should().Be(new ResRef("user_script"), "OnUserDefinedScript should match");
            loaded.OnDisarmScript.Should().Be(new ResRef("disarm_script"), "OnDisarmScript should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttEmptyFile()
        {
            // Test UTT with minimal structure
            var utt = new UTT();
            utt.ResRef = ResRef.FromBlank();
            utt.Tag = "";
            utt.Name = LocalizedString.FromInvalid();
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
        public void TestUttLocalizedStringName()
        {
            // Test LocalizedName field (LocalizedString)
            var utt = new UTT();
            utt.Name = LocalizedString.FromEnglish("English Trigger Name");
            utt.Name.SetData(Language.German, Gender.Male, "Deutscher Auslösername");

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.Name.Should().NotBeNull("Name should not be null");
            loaded.Name.Get(Language.English, Gender.Male).Should().Be("English Trigger Name", "English name should match");
            loaded.Name.Get(Language.German, Gender.Male).Should().Be("Deutscher Auslösername", "German name should match");
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
        public void TestUttKeyNameField()
        {
            // Test KeyName field
            var utt = new UTT();
            utt.KeyName = "required_key";

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.KeyName.Should().Be("required_key", "KeyName should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttTrapProperties()
        {
            // Test trap-related properties
            var utt = new UTT();
            utt.IsTrap = true;
            utt.TrapDetectable = true;
            utt.TrapDetectDc = 15;
            utt.TrapDisarmable = true;
            utt.TrapDisarmDc = 20;
            utt.TrapType = 1;
            utt.TrapOnce = true;

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            loaded.IsTrap.Should().BeTrue("IsTrap should match");
            loaded.TrapDetectable.Should().BeTrue("TrapDetectable should match");
            loaded.TrapDetectDc.Should().Be(15, "TrapDetectDc should match");
            loaded.TrapDisarmable.Should().BeTrue("TrapDisarmable should match");
            loaded.TrapDisarmDc.Should().Be(20, "TrapDisarmDc should match");
            loaded.TrapType.Should().Be(1, "TrapType should match");
            loaded.TrapOnce.Should().BeTrue("TrapOnce should match");
        }

        [Fact(Timeout = 120000)]
        public void TestUttAllFieldsRoundTrip()
        {
            // Test all UTT fields in a comprehensive round-trip
            var utt = new UTT();
            utt.ResRef = new ResRef("all_fields_trigger");
            utt.Tag = "ALLFIELDS";
            utt.Name = LocalizedString.FromEnglish("All Fields Trigger");
            utt.Name.SetData(Language.French, Gender.Female, "Déclencheur Tous Champs");
            utt.KeyName = "master_key";
            utt.TypeId = 7; // Trigger type
            utt.IsTrap = true;
            utt.TrapDetectable = true;
            utt.TrapDetectDc = 18;
            utt.TrapDisarmable = true;
            utt.TrapDisarmDc = 22;
            utt.TrapType = 2;
            utt.TrapOnce = false;
            utt.AutoRemoveKey = true;
            utt.FactionId = 5;
            utt.Cursor = 3;
            utt.HighlightHeight = 2.5f;
            utt.Comment = "Trigger with all fields set";
            utt.OnTrapTriggeredScript = new ResRef("trap_triggered");
            utt.OnClickScript = new ResRef("on_click");
            utt.OnHeartbeatScript = new ResRef("heartbeat");
            utt.OnEnterScript = new ResRef("on_enter");
            utt.OnExitScript = new ResRef("on_exit");
            utt.OnUserDefinedScript = new ResRef("user_defined");
            utt.OnDisarmScript = new ResRef("disarm");

            byte[] data = UTTAuto.BytesUtt(utt, Game.K2);
            UTT loaded = UTTAuto.ReadUtt(data);

            // Validate all fields
            loaded.ResRef.Should().Be(utt.ResRef);
            loaded.Tag.Should().Be(utt.Tag);
            loaded.Name.Get(Language.English, Gender.Male).Should().Be(utt.Name.Get(Language.English, Gender.Male));
            loaded.Name.Get(Language.French, Gender.Female).Should().Be(utt.Name.Get(Language.French, Gender.Female));
            loaded.KeyName.Should().Be(utt.KeyName);
            loaded.TypeId.Should().Be(utt.TypeId);
            loaded.IsTrap.Should().Be(utt.IsTrap);
            loaded.TrapDetectable.Should().Be(utt.TrapDetectable);
            loaded.TrapDetectDc.Should().Be(utt.TrapDetectDc);
            loaded.TrapDisarmable.Should().Be(utt.TrapDisarmable);
            loaded.TrapDisarmDc.Should().Be(utt.TrapDisarmDc);
            loaded.TrapType.Should().Be(utt.TrapType);
            loaded.TrapOnce.Should().Be(utt.TrapOnce);
            loaded.AutoRemoveKey.Should().Be(utt.AutoRemoveKey);
            loaded.FactionId.Should().Be(utt.FactionId);
            loaded.Cursor.Should().Be(utt.Cursor);
            loaded.HighlightHeight.Should().BeApproximately(utt.HighlightHeight, 0.001f);
            loaded.Comment.Should().Be(utt.Comment);
            loaded.OnTrapTriggeredScript.Should().Be(utt.OnTrapTriggeredScript);
            loaded.OnClickScript.Should().Be(utt.OnClickScript);
            loaded.OnHeartbeatScript.Should().Be(utt.OnHeartbeatScript);
            loaded.OnEnterScript.Should().Be(utt.OnEnterScript);
            loaded.OnExitScript.Should().Be(utt.OnExitScript);
            loaded.OnUserDefinedScript.Should().Be(utt.OnUserDefinedScript);
            loaded.OnDisarmScript.Should().Be(utt.OnDisarmScript);
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

        private static void ValidateIO(UTT utt)
        {
            // Basic validation
            utt.Should().NotBeNull("UTT object should not be null");
            utt.ResRef.Should().NotBeNull("ResRef should not be null");
            utt.Name.Should().NotBeNull("Name should not be null");
            utt.TypeId.Should().BeInRange(0, 7, "TypeId should be 0-7");
        }

        private static void CreateTestUttFile(string path)
        {
            var utt = new UTT();
            utt.ResRef = new ResRef("test_trigger");
            utt.Tag = "TEST";
            utt.Name = LocalizedString.FromEnglish("Test Trigger");
            utt.TypeId = 7; // Trigger type
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
