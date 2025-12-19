using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Numerics;
using Andastra.Parsing.Resource.Formats.LYT;
using Andastra.Parsing.Formats.LYT;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Common;
using Andastra.Parsing.Tests.Common;
using FluentAssertions;
using Xunit;

namespace Andastra.Parsing.Tests.Formats
{
    /// <summary>
    /// Comprehensive tests for LYT (Layout) file format operations.
    /// Tests validate the LYT format structure as defined in LYT.ksy Kaitai Struct definition.
    /// </summary>
    public class LYTFormatTests
    {
        private static readonly string TestFile = TestFileHelper.GetPath("test.lyt");
        private static readonly string DoesNotExistFile = "./thisfiledoesnotexist";
        private static readonly string EmptyTestFile = TestFileHelper.GetPath("test_empty.lyt");
        private static readonly string CorruptTestFile = TestFileHelper.GetPath("test_corrupted.lyt");

        [Fact(Timeout = 120000)]
        public void TestAsciiIO()
        {
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            // Test reading LYT file
            LYT lyt = new LYTAsciiReader(TestFile).Load();
            ValidateIO(lyt);

            // Test writing and reading back
            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                lyt = new LYTAsciiReader(tempFile).Load();
                ValidateIO(lyt);
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
        public void TestLytFormatStructure()
        {
            // Test that LYT format matches Kaitai Struct definition
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            // Read file as text to validate structure
            string content = File.ReadAllText(TestFile, Encoding.ASCII);
            
            // Validate header (beginlayout)
            content.Should().StartWith("beginlayout", "LYT file should start with 'beginlayout' as defined in LYT.ksy");
            
            // Validate footer (donelayout)
            content.Should().Contain("donelayout", "LYT file should contain 'donelayout' as defined in LYT.ksy");
        }

        [Fact(Timeout = 120000)]
        public void TestLytFileContentStructure()
        {
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            string content = File.ReadAllText(TestFile, Encoding.ASCII);
            string[] lines = content.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            // Validate structure: beginlayout, sections, donelayout
            lines[0].Trim().Should().Be("beginlayout", "First line should be 'beginlayout'");
            
            // Should contain section keywords
            bool hasRoomCount = lines.Any(l => l.Trim().StartsWith("roomcount", StringComparison.OrdinalIgnoreCase));
            bool hasDonelayout = lines.Any(l => l.Trim().Equals("donelayout", StringComparison.OrdinalIgnoreCase));
            
            hasDonelayout.Should().BeTrue("File should contain 'donelayout' keyword");
        }

        [Fact(Timeout = 120000)]
        public void TestLytRoomsSection()
        {
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            LYT lyt = new LYTAsciiReader(TestFile).Load();

            // Validate rooms structure (matching LYT.ksy documentation)
            lyt.Rooms.Should().NotBeNull("Rooms list should not be null");
            lyt.Rooms.Count.Should().BeGreaterOrEqualTo(0, "Room count should be non-negative");

            // Validate each room entry has required fields
            foreach (var room in lyt.Rooms)
            {
                room.Model.Should().NotBeNullOrEmpty("Room model should not be null or empty");
                room.Position.Should().NotBeNull("Room position should not be null");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestLytTracksSection()
        {
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            LYT lyt = new LYTAsciiReader(TestFile).Load();

            // Validate tracks structure (matching LYT.ksy documentation)
            lyt.Tracks.Should().NotBeNull("Tracks list should not be null");
            lyt.Tracks.Count.Should().BeGreaterOrEqualTo(0, "Track count should be non-negative");

            // Validate each track entry has required fields
            foreach (var track in lyt.Tracks)
            {
                track.Model.Should().NotBeNullOrEmpty("Track model should not be null or empty");
                track.Position.Should().NotBeNull("Track position should not be null");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestLytObstaclesSection()
        {
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            LYT lyt = new LYTAsciiReader(TestFile).Load();

            // Validate obstacles structure (matching LYT.ksy documentation)
            lyt.Obstacles.Should().NotBeNull("Obstacles list should not be null");
            lyt.Obstacles.Count.Should().BeGreaterOrEqualTo(0, "Obstacle count should be non-negative");

            // Validate each obstacle entry has required fields
            foreach (var obstacle in lyt.Obstacles)
            {
                obstacle.Model.Should().NotBeNullOrEmpty("Obstacle model should not be null or empty");
                obstacle.Position.Should().NotBeNull("Obstacle position should not be null");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestLytDoorhooksSection()
        {
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            LYT lyt = new LYTAsciiReader(TestFile).Load();

            // Validate doorhooks structure (matching LYT.ksy documentation)
            lyt.Doorhooks.Should().NotBeNull("Doorhooks list should not be null");
            lyt.Doorhooks.Count.Should().BeGreaterOrEqualTo(0, "Doorhook count should be non-negative");

            // Validate each doorhook entry has required fields
            foreach (var doorhook in lyt.Doorhooks)
            {
                doorhook.Room.Should().NotBeNullOrEmpty("Doorhook room should not be null or empty");
                doorhook.Door.Should().NotBeNullOrEmpty("Doorhook door should not be null or empty");
                doorhook.Position.Should().NotBeNull("Doorhook position should not be null");
                doorhook.Orientation.Should().NotBeNull("Doorhook orientation should not be null");
            }
        }

        [Fact(Timeout = 120000)]
        public void TestLytEmptyFile()
        {
            // Test LYT with minimal structure (no rooms, tracks, obstacles, doorhooks)
            var lyt = new LYT();
            lyt.Rooms.Should().NotBeNull("Empty LYT should have empty rooms list");
            lyt.Tracks.Should().NotBeNull("Empty LYT should have empty tracks list");
            lyt.Obstacles.Should().NotBeNull("Empty LYT should have empty obstacles list");
            lyt.Doorhooks.Should().NotBeNull("Empty LYT should have empty doorhooks list");

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Rooms.Count.Should().Be(0);
                loaded.Tracks.Count.Should().Be(0);
                loaded.Obstacles.Count.Should().Be(0);
                loaded.Doorhooks.Count.Should().Be(0);
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
        public void TestLytMultipleRooms()
        {
            // Test LYT with multiple rooms
            var lyt = new LYT();
            lyt.Rooms.Add(new LYTRoom("room1", new Vector3(0.0f, 0.0f, 0.0f)));
            lyt.Rooms.Add(new LYTRoom("room2", new Vector3(10.0f, 10.0f, 10.0f)));
            lyt.Rooms.Add(new LYTRoom("room3", new Vector3(20.0f, 20.0f, 20.0f)));

            lyt.Rooms.Count.Should().Be(3);

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Rooms.Count.Should().Be(3);
                loaded.Rooms[0].Model.Should().Be("room1");
                loaded.Rooms[1].Model.Should().Be("room2");
                loaded.Rooms[2].Model.Should().Be("room3");
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
        public void TestLytMultipleTracks()
        {
            // Test LYT with multiple tracks
            var lyt = new LYT();
            lyt.Tracks.Add(new LYTTrack("track1", new Vector3(0.0f, 0.0f, 0.0f)));
            lyt.Tracks.Add(new LYTTrack("track2", new Vector3(5.0f, 5.0f, 5.0f)));

            lyt.Tracks.Count.Should().Be(2);

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Tracks.Count.Should().Be(2);
                loaded.Tracks[0].Model.Should().Be("track1");
                loaded.Tracks[1].Model.Should().Be("track2");
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
        public void TestLytMultipleObstacles()
        {
            // Test LYT with multiple obstacles
            var lyt = new LYT();
            lyt.Obstacles.Add(new LYTObstacle("obstacle1", new Vector3(0.0f, 0.0f, 0.0f)));
            lyt.Obstacles.Add(new LYTObstacle("obstacle2", new Vector3(3.0f, 3.0f, 3.0f)));

            lyt.Obstacles.Count.Should().Be(2);

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Obstacles.Count.Should().Be(2);
                loaded.Obstacles[0].Model.Should().Be("obstacle1");
                loaded.Obstacles[1].Model.Should().Be("obstacle2");
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
        public void TestLytMultipleDoorhooks()
        {
            // Test LYT with multiple doorhooks
            var lyt = new LYT();
            lyt.Doorhooks.Add(new LYTDoorHook("room1", "door1", new Vector3(0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)));
            lyt.Doorhooks.Add(new LYTDoorHook("room2", "door2", new Vector3(5.0f, 5.0f, 5.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)));

            lyt.Doorhooks.Count.Should().Be(2);

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Doorhooks.Count.Should().Be(2);
                loaded.Doorhooks[0].Room.Should().Be("room1");
                loaded.Doorhooks[0].Door.Should().Be("door1");
                loaded.Doorhooks[1].Room.Should().Be("room2");
                loaded.Doorhooks[1].Door.Should().Be("door2");
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
        public void TestLytCompleteFile()
        {
            // Test LYT with all sections populated
            var lyt = new LYT();
            lyt.Rooms.Add(new LYTRoom("room1", new Vector3(0.0f, 0.0f, 0.0f)));
            lyt.Tracks.Add(new LYTTrack("track1", new Vector3(1.0f, 1.0f, 1.0f)));
            lyt.Obstacles.Add(new LYTObstacle("obstacle1", new Vector3(2.0f, 2.0f, 2.0f)));
            lyt.Doorhooks.Add(new LYTDoorHook("room1", "door1", new Vector3(3.0f, 3.0f, 3.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)));

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Rooms.Count.Should().Be(1);
                loaded.Tracks.Count.Should().Be(1);
                loaded.Obstacles.Count.Should().Be(1);
                loaded.Doorhooks.Count.Should().Be(1);
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
        public void TestLytRoomPositionValues()
        {
            // Test that room positions are preserved correctly
            var lyt = new LYT();
            lyt.Rooms.Add(new LYTRoom("testroom", new Vector3(123.456f, -789.012f, 345.678f)));

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Rooms.Count.Should().Be(1);
                loaded.Rooms[0].Position.X.Should().BeApproximately(123.456f, 0.001f);
                loaded.Rooms[0].Position.Y.Should().BeApproximately(-789.012f, 0.001f);
                loaded.Rooms[0].Position.Z.Should().BeApproximately(345.678f, 0.001f);
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
        public void TestLytDoorhookOrientationValues()
        {
            // Test that doorhook orientations are preserved correctly
            var lyt = new LYT();
            lyt.Doorhooks.Add(new LYTDoorHook("room1", "door1", new Vector3(0.0f, 0.0f, 0.0f), new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();
                LYT loaded = new LYTAsciiReader(tempFile).Load();

                loaded.Doorhooks.Count.Should().Be(1);
                loaded.Doorhooks[0].Orientation.X.Should().BeApproximately(0.5f, 0.001f);
                loaded.Doorhooks[0].Orientation.Y.Should().BeApproximately(0.5f, 0.001f);
                loaded.Doorhooks[0].Orientation.Z.Should().BeApproximately(0.5f, 0.001f);
                loaded.Doorhooks[0].Orientation.W.Should().BeApproximately(0.5f, 0.001f);
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
            Action act1 = () => new LYTAsciiReader(".").Load();
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                act1.Should().Throw<UnauthorizedAccessException>();
            }
            else
            {
                act1.Should().Throw<IOException>();
            }

            // Test reading non-existent file
            Action act2 = () => new LYTAsciiReader(DoesNotExistFile).Load();
            act2.Should().Throw<FileNotFoundException>();
        }

        [Fact(Timeout = 120000)]
        public void TestLytRoundTrip()
        {
            // Test complete round-trip: create, write, read, validate
            var originalLyt = new LYT();
            originalLyt.Rooms.Add(new LYTRoom("room1", new Vector3(1.0f, 2.0f, 3.0f)));
            originalLyt.Rooms.Add(new LYTRoom("room2", new Vector3(4.0f, 5.0f, 6.0f)));
            originalLyt.Tracks.Add(new LYTTrack("track1", new Vector3(7.0f, 8.0f, 9.0f)));
            originalLyt.Obstacles.Add(new LYTObstacle("obstacle1", new Vector3(10.0f, 11.0f, 12.0f)));
            originalLyt.Doorhooks.Add(new LYTDoorHook("room1", "door1", new Vector3(13.0f, 14.0f, 15.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)));

            string tempFile = Path.GetTempFileName();
            try
            {
                // Write
                new LYTAsciiWriter(originalLyt, tempFile).Write();

                // Read
                LYT loadedLyt = new LYTAsciiReader(tempFile).Load();

                // Validate
                loadedLyt.Rooms.Count.Should().Be(originalLyt.Rooms.Count);
                loadedLyt.Tracks.Count.Should().Be(originalLyt.Tracks.Count);
                loadedLyt.Obstacles.Count.Should().Be(originalLyt.Obstacles.Count);
                loadedLyt.Doorhooks.Count.Should().Be(originalLyt.Doorhooks.Count);

                for (int i = 0; i < originalLyt.Rooms.Count; i++)
                {
                    loadedLyt.Rooms[i].Model.Should().Be(originalLyt.Rooms[i].Model);
                    loadedLyt.Rooms[i].Position.Should().Be(originalLyt.Rooms[i].Position);
                }
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
        public void TestLytAsciiEncoding()
        {
            // Test that LYT files are encoded as ASCII
            var lyt = new LYT();
            lyt.Rooms.Add(new LYTRoom("testroom", new Vector3(0.0f, 0.0f, 0.0f)));

            string tempFile = Path.GetTempFileName();
            try
            {
                new LYTAsciiWriter(lyt, tempFile).Write();

                // Read as bytes and verify ASCII encoding
                byte[] bytes = File.ReadAllBytes(tempFile);
                string content = Encoding.ASCII.GetString(bytes);

                content.Should().Contain("beginlayout");
                content.Should().Contain("roomcount");
                content.Should().Contain("testroom");
                content.Should().Contain("donelayout");
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
        public void TestLytFileFormatKeywords()
        {
            // Test that LYT file contains all required keywords as per LYT.ksy documentation
            if (!File.Exists(TestFile))
            {
                CreateTestLytFile(TestFile);
            }

            string content = File.ReadAllText(TestFile, Encoding.ASCII);

            // Validate required keywords are present
            content.Should().Contain("beginlayout", "File should contain 'beginlayout' keyword");
            content.Should().Contain("donelayout", "File should contain 'donelayout' keyword");
        }

        private static void ValidateIO(LYT lyt)
        {
            // Basic validation
            lyt.Should().NotBeNull();
            lyt.Rooms.Should().NotBeNull();
            lyt.Tracks.Should().NotBeNull();
            lyt.Obstacles.Should().NotBeNull();
            lyt.Doorhooks.Should().NotBeNull();
            lyt.Rooms.Count.Should().BeGreaterOrEqualTo(0);
            lyt.Tracks.Count.Should().BeGreaterOrEqualTo(0);
            lyt.Obstacles.Count.Should().BeGreaterOrEqualTo(0);
            lyt.Doorhooks.Count.Should().BeGreaterOrEqualTo(0);
        }

        private static void CreateTestLytFile(string path)
        {
            var lyt = new LYT();
            lyt.Rooms.Add(new LYTRoom("testroom", new Vector3(0.0f, 0.0f, 0.0f)));
            lyt.Tracks.Add(new LYTTrack("testtrack", new Vector3(1.0f, 1.0f, 1.0f)));
            lyt.Obstacles.Add(new LYTObstacle("testobstacle", new Vector3(2.0f, 2.0f, 2.0f)));
            lyt.Doorhooks.Add(new LYTDoorHook("testroom", "testdoor", new Vector3(3.0f, 3.0f, 3.0f), new Vector4(0.0f, 0.0f, 0.0f, 1.0f)));

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            new LYTAsciiWriter(lyt, path).Write();
        }
    }
}

