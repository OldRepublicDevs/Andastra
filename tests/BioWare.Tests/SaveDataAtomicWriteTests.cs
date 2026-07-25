using System;
using System.IO;
using System.Linq;
using BioWare.Extract.SaveData;
using NUnit.Framework;

namespace BioWare.Tests
{
    public class SaveDataAtomicWriteTests
    {
        [Test]
        public void WriteBytesAtomic_CreatesNewFile_AndCleansTempArtifacts()
        {
            string tempDir = CreateTempDirectory();
            try
            {
                string targetPath = Path.Combine(tempDir, "globalvars.res");
                byte[] expected = { 0x01, 0x02, 0x03, 0x04 };

                SaveFolderIO.WriteBytesAtomic(targetPath, expected);

                Assert.That(File.Exists(targetPath), Is.True);
                Assert.That(File.ReadAllBytes(targetPath), Is.EqualTo(expected));
                Assert.That(GetTempArtifacts(tempDir, targetPath).Length, Is.EqualTo(0));
            }
            finally
            {
                TryDeleteDirectory(tempDir);
            }
        }

        [Test]
        public void WriteBytesAtomic_OverwritesExistingFile()
        {
            string tempDir = CreateTempDirectory();
            try
            {
                string targetPath = Path.Combine(tempDir, "partytable.res");
                File.WriteAllBytes(targetPath, new byte[] { 0x0A, 0x0B });
                byte[] expected = { 0xAA, 0xBB, 0xCC };

                SaveFolderIO.WriteBytesAtomic(targetPath, expected);

                Assert.That(File.ReadAllBytes(targetPath), Is.EqualTo(expected));
                Assert.That(GetTempArtifacts(tempDir, targetPath).Length, Is.EqualTo(0));
            }
            finally
            {
                TryDeleteDirectory(tempDir);
            }
        }

        [Test]
        [Platform(Include = "Win,Win32")]
        public void WriteBytesAtomic_WhenTargetLocked_ThrowsAndPreservesOriginalFile()
        {
            string tempDir = CreateTempDirectory();
            try
            {
                string targetPath = Path.Combine(tempDir, "savenfo.res");
                byte[] original = { 0x10, 0x20, 0x30 };
                byte[] replacement = { 0x99, 0x88, 0x77 };
                File.WriteAllBytes(targetPath, original);

                using (new FileStream(targetPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    var exception = Assert.Catch(() => SaveFolderIO.WriteBytesAtomic(targetPath, replacement));
                    Assert.That(exception, Is.Not.Null);
                }

                Assert.That(File.ReadAllBytes(targetPath), Is.EqualTo(original));
                Assert.That(GetTempArtifacts(tempDir, targetPath).Length, Is.EqualTo(0));
            }
            finally
            {
                TryDeleteDirectory(tempDir);
            }
        }

        [Test]
        public void SaveInfo_SaveLoad_RoundtripPersistsCoreFields()
        {
            string tempDir = CreateTempDirectory();
            try
            {
                var saveInfo = new SaveInfo(tempDir)
                {
                    AreaName = "danm13",
                    LastModule = "ebo_m12aa",
                    SavegameName = "Atomic Save Test",
                    TimePlayed = 1234,
                    CheatUsed = true,
                    PcName = "Revan"
                };

                saveInfo.Save();

                var loaded = new SaveInfo(tempDir);
                loaded.Load();

                Assert.That(loaded.AreaName, Is.EqualTo("danm13"));
                Assert.That(loaded.LastModule, Is.EqualTo("ebo_m12aa"));
                Assert.That(loaded.SavegameName, Is.EqualTo("Atomic Save Test"));
                Assert.That(loaded.TimePlayed, Is.EqualTo(1234));
                Assert.That(loaded.CheatUsed, Is.True);
                Assert.That(loaded.PcName, Is.EqualTo("Revan"));
            }
            finally
            {
                TryDeleteDirectory(tempDir);
            }
        }

        [Test]
        public void PartyTable_SaveLoad_RoundtripPersistsResourceFields()
        {
            string tempDir = CreateTempDirectory();
            try
            {
                var partyTable = new PartyTable(tempDir)
                {
                    Gold = 1000,
                    XpPool = 5000,
                    ItemComponents = 3,
                    ItemChemicals = 4,
                    CheatUsed = true,
                    SoloMode = true
                };
                partyTable.Members.Add(new PartyMemberEntry
                {
                    Index = -1,
                    IsLeader = true
                });

                partyTable.Save();

                var loaded = new PartyTable(tempDir);
                loaded.Load();

                Assert.That(loaded.Gold, Is.EqualTo(1000));
                Assert.That(loaded.XpPool, Is.EqualTo(5000));
                Assert.That(loaded.ItemComponents, Is.EqualTo(3));
                Assert.That(loaded.ItemChemicals, Is.EqualTo(4));
                Assert.That(loaded.CheatUsed, Is.True);
                Assert.That(loaded.SoloMode, Is.True);
                Assert.That(loaded.Members, Has.Count.EqualTo(1));
                Assert.That(loaded.Members[0].IsLeader, Is.True);
                Assert.That(loaded.Members[0].Index, Is.EqualTo(-1));
            }
            finally
            {
                TryDeleteDirectory(tempDir);
            }
        }

        private static string CreateTempDirectory()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "andastra_savedata_tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            return tempDir;
        }

        private static string[] GetTempArtifacts(string directoryPath, string targetPath)
        {
            string pattern = Path.GetFileName(targetPath) + ".tmp.*";
            return Directory.GetFiles(directoryPath, pattern);
        }

        private static void TryDeleteDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch
            {
            }
        }
    }
}
