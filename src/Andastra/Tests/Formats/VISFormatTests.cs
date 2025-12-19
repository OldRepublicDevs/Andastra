using System;
using System.IO;
using System.Linq;
using System.Text;
using Andastra.Parsing.Formats.VIS;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Common;
using Andastra.Parsing.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Andastra.Parsing.Tests.Formats
{
    /// <summary>
    /// Comprehensive tests for VIS (Visibility) file format operations.
    /// Tests validate the VIS format structure as defined in VIS.ksy Kaitai Struct definition.
    /// 
    /// VIS files are ASCII text files that define room visibility relationships for occlusion culling.
    /// Format: Parent lines contain "ROOM_NAME CHILD_COUNT", followed by CHILD_COUNT indented child lines.
    /// </summary>
    public class VISFormatTests
    {
        private static readonly string TestFile = TestFileHelper.GetPath("test.vis");
        private static readonly string CorruptTestFile = TestFileHelper.GetPath("test_corrupted.vis");
        private static readonly string DoesNotExistFile = "./thisfiledoesnotexist";

        [Fact(Timeout = 120000)]
        public void TestVisAsciiIO()
        {
            if (!File.Exists(TestFile))
            {
                CreateTestVisFile(TestFile);
            }

            // Test reading VIS file
            VIS vis = new VISAsciiReader(TestFile).Load();
            ValidateIO(vis);

            // Test writing and reading back
            string tempFile = Path.GetTempFileName();
            try
            {
                new VISAsciiWriter(vis, tempFile).Write();
                VIS loaded = new VISAsciiReader(tempFile).Load();
                ValidateIO(loaded);
                
                // Verify round-trip equality
                vis.Should().BeEquivalentTo(loaded, "VIS files should be equal after round-trip");
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
        public void TestVisFileStructure()
        {
            // Test that VIS file structure matches format specification
            if (!File.Exists(TestFile))
            {
                CreateTestVisFile(TestFile);
            }

            VIS vis = new VISAsciiReader(TestFile).Load();

            // Validate that VIS has rooms
            vis.AllRooms().Should().NotBeEmpty("VIS file should contain at least one room");

            // Validate that each room has visibility relationships
            foreach (var room in vis.AllRooms())
            {
                var visibleRooms = vis.GetVisibleRooms(room);
                visibleRooms.Should().NotBeNull($"Room '{room}' should have visibility data");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestVisParentChildStructure()
        {
            // Test parent-child room structure
            if (!File.Exists(TestFile))
            {
                CreateTestVisFile(TestFile);
            }

            VIS vis = new VISAsciiReader(TestFile).Load();

            // Read raw file content to validate structure
            string content = File.ReadAllText(TestFile, Encoding.ASCII);
            string[] lines = content.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            int lineIndex = 0;
            while (lineIndex < lines.Length)
            {
                string line = lines[lineIndex].Trim();
                
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                {
                    lineIndex++;
                    continue;
                }

                // Skip version headers
                string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length >= 2 && tokens[1].StartsWith("V", StringComparison.Ordinal))
                {
                    lineIndex++;
                    continue;
                }

                // This should be a parent line
                if (tokens.Length >= 2 && int.TryParse(tokens[1], out int childCount))
                {
                    string parentRoom = tokens[0];
                    
                    // Validate parent room exists in VIS
                    vis.RoomExists(parentRoom).Should().BeTrue($"Parent room '{parentRoom}' should exist in VIS");

                    // Validate child count matches
                    var visibleRooms = vis.GetVisibleRooms(parentRoom);
                    visibleRooms.Should().NotBeNull();
                    visibleRooms.Count.Should().Be(childCount, $"Parent room '{parentRoom}' should have {childCount} visible rooms");

                    // Skip to next parent line (after child lines)
                    lineIndex += childCount + 1;
                }
                else
                {
                    lineIndex++;
                }
            }
        }

        [Fact(Timeout = 120000)]
        public void TestVisChildLineIndentation()
        {
            // Test that child lines are properly indented (2 spaces minimum)
            if (!File.Exists(TestFile))
            {
                CreateTestVisFile(TestFile);
            }

            string content = File.ReadAllText(TestFile, Encoding.ASCII);
            string[] lines = content.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            int lineIndex = 0;
            while (lineIndex < lines.Length)
            {
                string line = lines[lineIndex];
                string trimmed = line.Trim();
                
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(trimmed))
                {
                    lineIndex++;
                    continue;
                }

                string[] tokens = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                // Skip version headers
                if (tokens.Length >= 2 && tokens[1].StartsWith("V", StringComparison.Ordinal))
                {
                    lineIndex++;
                    continue;
                }

                // Check if this is a parent line
                if (tokens.Length >= 2 && int.TryParse(tokens[1], out int childCount))
                {
                    // Parent line should not be indented
                    line.Should().NotStartWith("  ", "Parent lines should not be indented");

                    // Read child lines
                    for (int i = 0; i < childCount; i++)
                    {
                        lineIndex++;
                        if (lineIndex >= lines.Length)
                        {
                            break;
                        }

                        string childLine = lines[lineIndex];
                        string childTrimmed = childLine.TrimStart();
                        
                        // Child line should be indented (at least 2 spaces, but parser handles variable indentation)
                        if (!string.IsNullOrWhiteSpace(childTrimmed))
                        {
                            // The original line should have leading whitespace (indentation)
                            childLine.Should().NotBe(childTrimmed, "Child lines should be indented");
                        }
                    }
                }

                lineIndex++;
            }
        }

        [Fact(Timeout = 120000)]
        public void TestVisEmptyLinesIgnored()
        {
            // Test that empty lines are ignored during parsing
            string visContent = @"room_01 2
  room_02

  room_03
room_02 1

  room_01";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                VIS vis = new VISAsciiReader(tempFile).Load();

                // Should parse correctly despite empty lines
                vis.RoomExists("room_01").Should().BeTrue();
                vis.RoomExists("room_02").Should().BeTrue();
                vis.RoomExists("room_03").Should().BeTrue();

                var visibleFrom01 = vis.GetVisibleRooms("room_01");
                visibleFrom01.Should().NotBeNull();
                visibleFrom01.Count.Should().Be(2);
                visibleFrom01.Should().Contain("room_02");
                visibleFrom01.Should().Contain("room_03");
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
        public void TestVisVersionHeaderSkipped()
        {
            // Test that version headers (e.g., "room V3.28") are skipped
            string visContent = @"room V3.28
room_01 2
  room_02
  room_03";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                VIS vis = new VISAsciiReader(tempFile).Load();

                // Version header should be skipped, room_01 should be parsed
                vis.RoomExists("room_01").Should().BeTrue();
                vis.RoomExists("room").Should().BeFalse("Version header room should not be added");

                var visibleFrom01 = vis.GetVisibleRooms("room_01");
                visibleFrom01.Should().NotBeNull();
                visibleFrom01.Count.Should().Be(2);
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
        public void TestVisCaseInsensitive()
        {
            // Test that room names are case-insensitive
            string visContent = @"ROOM_01 2
  ROOM_02
  room_03
room_02 1
  Room_01";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                VIS vis = new VISAsciiReader(tempFile).Load();

                // All variations should refer to the same room (lowercased)
                vis.RoomExists("room_01").Should().BeTrue();
                vis.RoomExists("ROOM_01").Should().BeTrue();
                vis.RoomExists("Room_01").Should().BeTrue();

                // Visibility should work regardless of case
                var visibleFrom01 = vis.GetVisibleRooms("room_01");
                visibleFrom01.Should().NotBeNull();
                visibleFrom01.Should().Contain("room_02");
                visibleFrom01.Should().Contain("room_03");
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
        public void TestVisEmptyFile()
        {
            // Test VIS with no rooms
            var vis = new VIS();
            vis.AllRooms().Should().BeEmpty("Empty VIS should have no rooms");

            string tempFile = Path.GetTempFileName();
            try
            {
                new VISAsciiWriter(vis, tempFile).Write();
                VIS loaded = new VISAsciiReader(tempFile).Load();

                loaded.AllRooms().Should().BeEmpty("Empty VIS file should load with no rooms");
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
        public void TestVisSingleRoom()
        {
            // Test VIS with single room (no visibility relationships)
            var vis = new VIS();
            vis.AddRoom("room_01");

            vis.RoomExists("room_01").Should().BeTrue();
            vis.AllRooms().Count.Should().Be(1);

            var visibleRooms = vis.GetVisibleRooms("room_01");
            visibleRooms.Should().NotBeNull();
            visibleRooms.Count.Should().Be(0, "Room with no visibility relationships should have empty set");
        }

        [Fact(Timeout = 120000)]
        public void TestVisMultipleRooms()
        {
            // Test VIS with multiple rooms and relationships
            var vis = new VIS();
            vis.AddRoom("room_01");
            vis.AddRoom("room_02");
            vis.AddRoom("room_03");
            vis.AddRoom("room_04");

            vis.SetVisible("room_01", "room_02", true);
            vis.SetVisible("room_01", "room_03", true);
            vis.SetVisible("room_02", "room_01", true);
            vis.SetVisible("room_03", "room_04", true);

            vis.AllRooms().Count.Should().Be(4);

            var visibleFrom01 = vis.GetVisibleRooms("room_01");
            visibleFrom01.Should().NotBeNull();
            visibleFrom01.Count.Should().Be(2);
            visibleFrom01.Should().Contain("room_02");
            visibleFrom01.Should().Contain("room_03");

            var visibleFrom02 = vis.GetVisibleRooms("room_02");
            visibleFrom02.Should().NotBeNull();
            visibleFrom02.Count.Should().Be(1);
            visibleFrom02.Should().Contain("room_01");
        }

        [Fact(Timeout = 120000)]
        public void TestVisSelfReference()
        {
            // Test that a room can list itself as visible (rare but valid)
            string visContent = @"room_01 1
  room_01";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                VIS vis = new VISAsciiReader(tempFile).Load();

                vis.RoomExists("room_01").Should().BeTrue();

                var visibleRooms = vis.GetVisibleRooms("room_01");
                visibleRooms.Should().NotBeNull();
                visibleRooms.Should().Contain("room_01", "Room can list itself as visible");
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
        public void TestVisAsymmetricVisibility()
        {
            // Test that visibility is not necessarily symmetric
            string visContent = @"room_01 1
  room_02
room_02 0";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                VIS vis = new VISAsciiReader(tempFile).Load();

                // room_01 can see room_02, but room_02 cannot see room_01
                var visibleFrom01 = vis.GetVisibleRooms("room_01");
                visibleFrom01.Should().NotBeNull();
                visibleFrom01.Should().Contain("room_02");

                var visibleFrom02 = vis.GetVisibleRooms("room_02");
                visibleFrom02.Should().NotBeNull();
                visibleFrom02.Should().NotContain("room_01", "Visibility is not necessarily symmetric");
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
        public void TestVisInvalidChildCount()
        {
            // Test that invalid child count raises exception
            string visContent = @"room_01 invalid";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                Action act = () => new VISAsciiReader(tempFile).Load();
                act.Should().Throw<ArgumentException>()
                    .WithMessage("*expected room count*");
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
        public void TestVisMissingChildLines()
        {
            // Test that missing child lines raise exception
            string visContent = @"room_01 3
  room_02";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                Action act = () => new VISAsciiReader(tempFile).Load();
                act.Should().Throw<ArgumentException>()
                    .WithMessage("*expected 3 child rooms*");
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
        public void TestVisReadRaises()
        {
            // Test reading from directory
            Action act1 = () => new VISAsciiReader(".").Load();
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                act1.Should().Throw<UnauthorizedAccessException>();
            }
            else
            {
                act1.Should().Throw<IOException>();
            }

            // Test reading non-existent file
            Action act2 = () => new VISAsciiReader(DoesNotExistFile).Load();
            act2.Should().Throw<FileNotFoundException>();

            // Test reading corrupted file (if exists)
            if (File.Exists(CorruptTestFile))
            {
                Action act3 = () => new VISAsciiReader(CorruptTestFile).Load();
                // May throw various exceptions depending on corruption type
                act3.Should().Throw<Exception>();
            }
        }

        [Fact(Timeout = 120000)]
        public void TestVisByteArrayIO()
        {
            // Test reading from byte array
            if (!File.Exists(TestFile))
            {
                CreateTestVisFile(TestFile);
            }

            byte[] data = File.ReadAllBytes(TestFile);
            VIS vis = new VISAsciiReader(data).Load();

            ValidateIO(vis);
        }

        [Fact(Timeout = 120000)]
        public void TestVisStreamIO()
        {
            // Test reading from stream
            if (!File.Exists(TestFile))
            {
                CreateTestVisFile(TestFile);
            }

            using (var stream = File.OpenRead(TestFile))
            {
                VIS vis = new VISAsciiReader(stream).Load();
                ValidateIO(vis);
            }
        }

        [Fact(Timeout = 120000)]
        public void TestVisWriteToFile()
        {
            // Test writing VIS to file
            var vis = new VIS();
            vis.AddRoom("room_01");
            vis.AddRoom("room_02");
            vis.AddRoom("room_03");
            vis.SetVisible("room_01", "room_02", true);
            vis.SetVisible("room_01", "room_03", true);

            string tempFile = Path.GetTempFileName();
            try
            {
                new VISAsciiWriter(vis, tempFile).Write();

                // Verify file was created and is readable
                File.Exists(tempFile).Should().BeTrue();
                File.ReadAllText(tempFile, Encoding.ASCII).Should().NotBeEmpty();

                // Verify file can be read back
                VIS loaded = new VISAsciiReader(tempFile).Load();
                loaded.Should().BeEquivalentTo(vis);
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
        public void TestVisWriteToStream()
        {
            // Test writing VIS to stream
            var vis = new VIS();
            vis.AddRoom("room_01");
            vis.AddRoom("room_02");
            vis.SetVisible("room_01", "room_02", true);

            using (var stream = new MemoryStream())
            {
                new VISAsciiWriter(vis, stream).Write();

                stream.Position = 0;
                byte[] data = stream.ToArray();
                data.Should().NotBeEmpty();

                // Verify data can be read back
                VIS loaded = new VISAsciiReader(data).Load();
                loaded.Should().BeEquivalentTo(vis);
            }
        }

        [Fact(Timeout = 120000)]
        public void TestVisVariableIndentation()
        {
            // Test that variable indentation (more than 2 spaces) is handled
            // The parser should trim leading whitespace from child lines
            string visContent = @"room_01 3
  room_02
     room_03
  room_04";

            string tempFile = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFile, visContent, Encoding.ASCII);

                VIS vis = new VISAsciiReader(tempFile).Load();

                vis.RoomExists("room_01").Should().BeTrue();
                vis.RoomExists("room_02").Should().BeTrue();
                vis.RoomExists("room_03").Should().BeTrue();
                vis.RoomExists("room_04").Should().BeTrue();

                var visibleFrom01 = vis.GetVisibleRooms("room_01");
                visibleFrom01.Should().NotBeNull();
                visibleFrom01.Count.Should().Be(3);
                visibleFrom01.Should().Contain("room_02");
                visibleFrom01.Should().Contain("room_03");
                visibleFrom01.Should().Contain("room_04");
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
        public void TestVisLargeFile()
        {
            // Test VIS file with many rooms
            var vis = new VIS();
            const int roomCount = 100;

            // Add rooms
            for (int i = 0; i < roomCount; i++)
            {
                vis.AddRoom($"room_{i:D3}");
            }

            // Create visibility relationships (each room sees next 5 rooms)
            for (int i = 0; i < roomCount; i++)
            {
                for (int j = 1; j <= 5 && (i + j) < roomCount; j++)
                {
                    vis.SetVisible($"room_{i:D3}", $"room_{i + j:D3}", true);
                }
            }

            vis.AllRooms().Count.Should().Be(roomCount);

            // Verify some relationships
            var visibleFrom00 = vis.GetVisibleRooms("room_000");
            visibleFrom00.Should().NotBeNull();
            visibleFrom00.Count.Should().Be(5);

            string tempFile = Path.GetTempFileName();
            try
            {
                new VISAsciiWriter(vis, tempFile).Write();
                VIS loaded = new VISAsciiReader(tempFile).Load();

                loaded.AllRooms().Count.Should().Be(roomCount);
                loaded.Should().BeEquivalentTo(vis);
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
        public void TestVisSetAllVisible()
        {
            // Test SetAllVisible method
            var vis = new VIS();
            vis.AddRoom("room_01");
            vis.AddRoom("room_02");
            vis.AddRoom("room_03");

            vis.SetAllVisible();

            // Each room should see all other rooms
            foreach (var room in vis.AllRooms())
            {
                var visibleRooms = vis.GetVisibleRooms(room);
                visibleRooms.Should().NotBeNull();
                visibleRooms.Count.Should().Be(2, $"Room '{room}' should see all other rooms");
                visibleRooms.Should().NotContain(room, "Room should not see itself");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestVisRemoveRoom()
        {
            // Test removing a room
            var vis = new VIS();
            vis.AddRoom("room_01");
            vis.AddRoom("room_02");
            vis.AddRoom("room_03");
            vis.SetVisible("room_01", "room_02", true);
            vis.SetVisible("room_01", "room_03", true);
            vis.SetVisible("room_02", "room_03", true);

            vis.RemoveRoom("room_02");

            vis.RoomExists("room_02").Should().BeFalse("Removed room should not exist");
            vis.AllRooms().Count.Should().Be(2);

            // room_01 should no longer see room_02
            var visibleFrom01 = vis.GetVisibleRooms("room_01");
            visibleFrom01.Should().NotBeNull();
            visibleFrom01.Should().NotContain("room_02");
            visibleFrom01.Should().Contain("room_03");
        }

        [Fact(Timeout = 120000)]
        public void TestVisRenameRoom()
        {
            // Test renaming a room
            var vis = new VIS();
            vis.AddRoom("room_01");
            vis.AddRoom("room_02");
            vis.SetVisible("room_01", "room_02", true);

            vis.RenameRoom("room_01", "room_renamed");

            vis.RoomExists("room_01").Should().BeFalse("Old room name should not exist");
            vis.RoomExists("room_renamed").Should().BeTrue("New room name should exist");

            var visibleFromRenamed = vis.GetVisibleRooms("room_renamed");
            visibleFromRenamed.Should().NotBeNull();
            visibleFromRenamed.Should().Contain("room_02");
        }

        [Fact(Timeout = 120000)]
        public void TestVisGetVisibleRoomsNull()
        {
            // Test GetVisibleRooms with non-existent room
            var vis = new VIS();
            vis.AddRoom("room_01");

            var visibleRooms = vis.GetVisibleRooms("room_nonexistent");
            visibleRooms.Should().BeNull("Non-existent room should return null");
        }

        [Fact(Timeout = 120000)]
        public void TestVisSetVisibleThrowsForNonExistentRooms()
        {
            // Test that SetVisible throws for non-existent rooms
            var vis = new VIS();
            vis.AddRoom("room_01");

            Action act = () => vis.SetVisible("room_01", "room_nonexistent", true);
            act.Should().Throw<ArgumentException>()
                .WithMessage("*does not exist*");
        }

        [Fact(Timeout = 120000)]
        public void TestVisGetVisibleThrowsForNonExistentRooms()
        {
            // Test that GetVisible throws for non-existent rooms
            var vis = new VIS();
            vis.AddRoom("room_01");

            Action act = () => vis.GetVisible("room_01", "room_nonexistent");
            act.Should().Throw<ArgumentException>()
                .WithMessage("*does not exist*");
        }

        [Fact(Timeout = 120000)]
        public void TestVisEquality()
        {
            // Test VIS equality comparison
            var vis1 = new VIS();
            vis1.AddRoom("room_01");
            vis1.AddRoom("room_02");
            vis1.SetVisible("room_01", "room_02", true);

            var vis2 = new VIS();
            vis2.AddRoom("room_01");
            vis2.AddRoom("room_02");
            vis2.SetVisible("room_01", "room_02", true);

            vis1.Should().BeEquivalentTo(vis2, "VIS objects with same data should be equal");

            // Test inequality
            vis2.SetVisible("room_02", "room_01", true);
            vis1.Should().NotBeEquivalentTo(vis2, "VIS objects with different data should not be equal");
        }

        private static void ValidateIO(VIS vis)
        {
            // Basic validation
            vis.Should().NotBeNull("VIS object should not be null");
            vis.AllRooms().Should().NotBeNull("AllRooms should not be null");
        }

        private static void CreateTestVisFile(string path)
        {
            var vis = new VIS();
            vis.AddRoom("room_01");
            vis.AddRoom("room_02");
            vis.AddRoom("room_03");
            vis.AddRoom("room_04");

            vis.SetVisible("room_01", "room_02", true);
            vis.SetVisible("room_01", "room_03", true);
            vis.SetVisible("room_01", "room_04", true);
            vis.SetVisible("room_02", "room_01", true);
            vis.SetVisible("room_03", "room_04", true);
            vis.SetVisible("room_04", "room_03", true);
            vis.SetVisible("room_04", "room_01", true);

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            new VISAsciiWriter(vis, path).Write();
        }
    }
}

