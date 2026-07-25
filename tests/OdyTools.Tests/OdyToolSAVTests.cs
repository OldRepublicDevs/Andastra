using System;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Extract.SaveData;
using BioWare.Resource;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// Save game Editor Load/Build tests. Uses Avalonia headless session so UI is not required.
    /// Save editor returns empty bytes from Build() (folder-based).
    /// </summary>
    public class OdyToolSAVTests
    {
        [Test]
        public async Task OdyToolSAV_New_BuildsEmptyBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSAV(null, null);
                    editor.New();
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolSAV_Constructor_BuildsProgrammaticSurfaceWithoutInstallation()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSAV(null, null);

                    Assert.That(editor.HasProgrammaticEditorSurfaceForTest, Is.True);
                    Assert.That(editor.Build().Item1, Is.Not.Null.And.Length.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public void OdyToolSAV_MinimalSaveFolder_LoadsEditableSaveData()
        {
            using (var tempDir = new TempDirectory())
            {
                CreateMinimalSaveFolder(tempDir.Path);

                var saveFolder = new SaveFolderEntry(tempDir.Path);
                saveFolder.Load();

                Assert.That(saveFolder.SaveInfo, Is.Not.Null);
                Assert.That(saveFolder.PartyTable, Is.Not.Null);
                Assert.That(saveFolder.GlobalVars, Is.Not.Null);
                Assert.That(saveFolder.NestedCapsule, Is.Not.Null);
                Assert.That(saveFolder.SaveInfo.SavegameName, Is.EqualTo("Test Save"));
                Assert.That(saveFolder.SaveInfo.AreaName, Is.EqualTo("Test Area"));
                Assert.That(saveFolder.SaveInfo.LastModule, Is.EqualTo("test_module"));
                Assert.That(saveFolder.SaveInfo.TimePlayed, Is.EqualTo(3600));
                Assert.That(saveFolder.SaveInfo.CheatUsed, Is.False);
                Assert.That(saveFolder.PartyTable.Gold, Is.EqualTo(1000));
                Assert.That(saveFolder.PartyTable.XpPool, Is.EqualTo(5000));
                Assert.That(saveFolder.PartyTable.ItemComponents, Is.EqualTo(3));
                Assert.That(saveFolder.PartyTable.ItemChemicals, Is.EqualTo(4));
                Assert.That(saveFolder.GlobalVars.GetBool("TEST_BOOL"), Is.True);
                Assert.That(saveFolder.GlobalVars.GetNumber("TEST_NUM"), Is.EqualTo(42));
                Assert.That(saveFolder.GlobalVars.GetString("TEST_STR"), Is.EqualTo("test string"));
            }
        }

        [Test]
        public async Task OdyToolSAV_LoadThenBuild_ReturnsEmptyBytes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSAV(null, null);
                    editor.Load("save", "save", ResourceType.SAV, null);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] data = result.Item1;
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Length, Is.EqualTo(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolSAV_SaveInfoEdits_UpdateModelAndDirtyState()
        {
            using (var tempDir = new TempDirectory())
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSAV(null, null);
                    var saveInfo = new SaveInfo(tempDir.Path)
                    {
                        SavegameName = "Original Save",
                        AreaName = "Original Area",
                        LastModule = "oldmod",
                        TimePlayed = 10,
                        CheatUsed = false
                    };

                    editor.SetSaveInfoForTesting(saveInfo);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.SaveNameEditForTest.Text = "Edited Save";
                    editor.AreaNameEditForTest.Text = "Edited Area";
                    editor.LastModuleEditForTest.Text = "newmod";
                    editor.TimePlayedSpinForTest.Value = 42;
                    editor.CheatUsedCheckForTest.IsChecked = true;

                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(saveInfo.SavegameName, Is.EqualTo("Edited Save"));
                    Assert.That(saveInfo.AreaName, Is.EqualTo("Edited Area"));
                    Assert.That(saveInfo.LastModule, Is.EqualTo("newmod"));
                    Assert.That(saveInfo.TimePlayed, Is.EqualTo(42));
                    Assert.That(saveInfo.CheatUsed, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolSAV_PartyTableEdits_UpdateModelAndDirtyState()
        {
            using (var tempDir = new TempDirectory())
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSAV(null, null);
                    var partyTable = new PartyTable(tempDir.Path)
                    {
                        Gold = 100,
                        XpPool = 200,
                        ItemComponents = 3,
                        ItemChemicals = 4,
                        CheatUsed = false,
                        SoloMode = false
                    };

                    editor.SetPartyTableForTesting(partyTable);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.PartyGoldSpinForTest.Value = 500;
                    editor.PartyXpPoolSpinForTest.Value = 600;
                    editor.PartyComponentsSpinForTest.Value = 7;
                    editor.PartyChemicalsSpinForTest.Value = 8;
                    editor.PartyCheatUsedCheckForTest.IsChecked = true;
                    editor.PartySoloModeCheckForTest.IsChecked = true;

                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(partyTable.Gold, Is.EqualTo(500));
                    Assert.That(partyTable.XpPool, Is.EqualTo(600));
                    Assert.That(partyTable.ItemComponents, Is.EqualTo(7));
                    Assert.That(partyTable.ItemChemicals, Is.EqualTo(8));
                    Assert.That(partyTable.CheatUsed, Is.True);
                    Assert.That(partyTable.SoloMode, Is.True);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolSAV_GlobalVarEdits_UpdateModelAndDirtyState()
        {
            using (var tempDir = new TempDirectory())
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSAV(null, null);
                    var globals = new GlobalVars(tempDir.Path);
                    globals.SetBool("EXISTING_BOOL", true);
                    globals.SetNumber("EXISTING_NUM", 5);
                    globals.SetString("EXISTING_STR", "old");
                    globals.SetLocation("EXISTING_LOC", new Vector4(1, 2, 3, 4));

                    editor.SetGlobalVarsForTesting(globals);

                    Assert.That(editor.IsDirty, Is.False);

                    editor.AddGlobalBoolForTest("ADDED_BOOL", false);
                    editor.AddGlobalNumberForTest("ADDED_NUM", 255);
                    editor.AddGlobalStringForTest("ADDED_STR", "value");
                    editor.AddGlobalLocationForTest("ADDED_LOC", new Vector4(5, 6, 7, 8));

                    Assert.That(editor.IsDirty, Is.True);
                    Assert.That(globals.GetBool("ADDED_BOOL"), Is.False);
                    Assert.That(globals.GetNumber("ADDED_NUM"), Is.EqualTo(255));
                    Assert.That(globals.GetString("ADDED_STR"), Is.EqualTo("value"));
                    Assert.That(globals.GetLocation("ADDED_LOC"), Is.EqualTo(new Vector4(5, 6, 7, 8)));

                    editor.RemoveGlobalBoolAtForTest(0);
                    editor.RemoveGlobalNumberAtForTest(0);
                    editor.RemoveGlobalStringAtForTest(0);
                    editor.RemoveGlobalLocationAtForTest(0);

                    Assert.That(globals.GetBool("EXISTING_BOOL"), Is.Null);
                    Assert.That(globals.GetNumber("EXISTING_NUM"), Is.Null);
                    Assert.That(globals.GetString("EXISTING_STR"), Is.Null);
                    Assert.That(globals.GetLocation("EXISTING_LOC"), Is.Null);
                    Assert.That(globals.GetBool("ADDED_BOOL"), Is.False);
                    Assert.That(globals.GetNumber("ADDED_NUM"), Is.EqualTo(255));
                    Assert.That(globals.GetString("ADDED_STR"), Is.EqualTo("value"));
                    Assert.That(globals.GetLocation("ADDED_LOC"), Is.EqualTo(new Vector4(5, 6, 7, 8)));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolSAV_GlobalVarAdd_UsesUniqueDefaultNames()
        {
            using (var tempDir = new TempDirectory())
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolSAV(null, null);
                    var globals = new GlobalVars(tempDir.Path);
                    globals.SetBool("NEW_VAR", true);

                    editor.SetGlobalVarsForTesting(globals);

                    string firstAdded = editor.AddDefaultGlobalBoolForTest();
                    string secondAdded = editor.AddDefaultGlobalBoolForTest();

                    Assert.That(firstAdded, Is.EqualTo("NEW_VAR_1"));
                    Assert.That(secondAdded, Is.EqualTo("NEW_VAR_2"));
                    Assert.That(globals.GetBool("NEW_VAR_1"), Is.False);
                    Assert.That(globals.GetBool("NEW_VAR_2"), Is.False);
                }, CancellationToken.None);
            }
        }

        private static void CreateMinimalSaveFolder(string folderPath)
        {
            var saveFolder = new SaveFolderEntry(folderPath);
            saveFolder.SaveInfo.SavegameName = "Test Save";
            saveFolder.SaveInfo.PcName = "TestPlayer";
            saveFolder.SaveInfo.AreaName = "Test Area";
            saveFolder.SaveInfo.LastModule = "test_module";
            saveFolder.SaveInfo.TimePlayed = 3600;
            saveFolder.SaveInfo.CheatUsed = false;

            saveFolder.PartyTable.Members.Add(new PartyMemberEntry
            {
                Index = -1,
                IsLeader = true
            });
            saveFolder.PartyTable.Gold = 1000;
            saveFolder.PartyTable.XpPool = 5000;
            saveFolder.PartyTable.ItemComponents = 3;
            saveFolder.PartyTable.ItemChemicals = 4;
            saveFolder.PartyTable.CheatUsed = false;
            saveFolder.PartyTable.SoloMode = false;

            saveFolder.GlobalVars.SetBool("TEST_BOOL", true);
            saveFolder.GlobalVars.SetNumber("TEST_NUM", 42);
            saveFolder.GlobalVars.SetString("TEST_STR", "test string");

            saveFolder.Save();
        }

        private sealed class TempDirectory : IDisposable
        {
            public TempDirectory()
            {
                Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "odytools-sav-test-" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(Path);
            }

            public string Path { get; }

            public void Dispose()
            {
                try { Directory.Delete(Path, true); }
                catch { }
            }
        }
    }
}
