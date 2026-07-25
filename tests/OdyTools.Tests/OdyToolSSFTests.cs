using System;
using System.IO;
using Avalonia.Headless.NUnit;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.SSF;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// SSF Editor Load/Build roundtrip tests. Uses Avalonia headless session so UI is not required.
    /// </summary>
    public class OdyToolSSFTests
    {
        private static string VendorTestFile(string relativePath)
        {
            var directory = new DirectoryInfo(TestContext.CurrentContext.TestDirectory);
            while (directory != null)
            {
                string candidate = Path.Combine(directory.FullName, "vendor", "tests", "test_files", relativePath);
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }

            Assert.Fail("Could not locate vendor test file: " + relativePath);
            return null;
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void OdyToolSSF_LoadAndBuild_PreservesData()
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, 100);
            ssf.SetData(SSFSound.SELECT_1, 200);
            ssf.SetData(SSFSound.DEAD, 300);
            byte[] originalData = ssf.ToBytes();

            var editor = new OdyToolSSF(null, null);
            try
            {
                editor.Load("test.ssf", "test", ResourceType.SSF, originalData);

                Tuple<byte[], byte[]> buildResult = editor.Build();
                byte[] builtData = buildResult.Item1;
                Assert.That(builtData, Is.Not.Null.And.Length.GreaterThan(0));

                SSF loaded = SSF.FromBytes(builtData);
                Assert.That(loaded.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(100));
                Assert.That(loaded.Get(SSFSound.SELECT_1), Is.EqualTo(200));
                Assert.That(loaded.Get(SSFSound.DEAD), Is.EqualTo(300));
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void OdyToolSSF_LoadXmlAndBuild_PreservesData()
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, 100);
            ssf.SetData(SSFSound.SELECT_1, 200);
            ssf.SetData(SSFSound.DEAD, 300);
            byte[] xmlData = SSFAuto.BytesSsf(ssf, ResourceType.SSF_XML);

            var editor = new OdyToolSSF(null, null);
            try
            {
                editor.Load("test.ssf.xml", "test", ResourceType.SSF_XML, xmlData);

                SSF built = SSFAuto.ReadSsf(editor.Build().Item1, fileFormat: ResourceType.SSF);
                Assert.That(built.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(100));
                Assert.That(built.Get(SSFSound.SELECT_1), Is.EqualTo(200));
                Assert.That(built.Get(SSFSound.DEAD), Is.EqualTo(300));
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(180000)]
        [AvaloniaTest]
        public void OdyToolSSF_LoadVendorSoundset_BuildPreservesAllSoundSlots()
        {
            byte[] originalData = File.ReadAllBytes(VendorTestFile("n_ithorian.ssf"));
            SSF original = SSFAuto.ReadSsf(originalData, 0, originalData.Length, ResourceType.SSF);
            var editor = new OdyToolSSF(null, null);
            try
            {
                editor.Load("n_ithorian.ssf", "n_ithorian", ResourceType.SSF, originalData);

                SSF built = SSFAuto.ReadSsf(editor.Build().Item1, fileFormat: ResourceType.SSF);
                Assert.That(built, Is.EqualTo(original));

                foreach (SSFSound sound in Enum.GetValues(typeof(SSFSound)))
                {
                    Assert.That(editor.StrrefSpinForTest(sound).Value, Is.EqualTo(original.Get(sound)), sound.ToString());
                }

                editor.Load("n_ithorian.ssf", "n_ithorian", ResourceType.SSF, editor.Build().Item1);

                foreach (SSFSound sound in Enum.GetValues(typeof(SSFSound)))
                {
                    Assert.That(editor.StrrefSpinForTest(sound).Value, Is.EqualTo(original.Get(sound)), sound.ToString());
                }
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(120000)]
        [AvaloniaTest]
        public void OdyToolSSF_New_BuildsValidSSF()
        {
            var editor = new OdyToolSSF(null, null);
            try
            {
                editor.New();
                Tuple<byte[], byte[]> result = editor.Build();
                byte[] data = result.Item1;
                Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
                SSF loaded = SSF.FromBytes(data);
                Assert.That(loaded.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(0));
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolSSF_LoadEmpty_BuildsValidSSF()
        {
            var editor = new OdyToolSSF(null, null);
            try
            {
                editor.Load("x.ssf", "x", ResourceType.SSF, null);
                Tuple<byte[], byte[]> result = editor.Build();
                byte[] data = result.Item1;
                Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolSSF_StructuredRowsExposePlayAndLocateActions()
        {
            var editor = new OdyToolSSF(null, null);
            try
            {
                Assert.That(editor.HasStructuredEditorSurface, Is.True);

                var strrefSpin = editor.StrrefSpinForTest(SSFSound.BATTLE_CRY_1);
                var soundEdit = editor.SoundResRefTextForTest(SSFSound.BATTLE_CRY_1);
                var previewEdit = editor.PreviewTextForTest(SSFSound.BATTLE_CRY_1);
                var playButton = editor.PlayButtonForTest(SSFSound.BATTLE_CRY_1);
                var locateButton = editor.LocateButtonForTest(SSFSound.BATTLE_CRY_1);
                var locateOrder = editor.SoundLocateOrderForTest;

                Assert.That(strrefSpin, Is.Not.Null);
                Assert.That(soundEdit, Is.Not.Null);
                Assert.That(previewEdit, Is.Not.Null);
                Assert.That(playButton, Is.Not.Null);
                Assert.That(locateButton, Is.Not.Null);
                Assert.That(playButton.IsEnabled, Is.False);
                Assert.That(locateButton.IsEnabled, Is.False);

                strrefSpin.Value = 1234;
                Assert.That(soundEdit.Text, Is.EqualTo(""));
                Assert.That(previewEdit.Text, Is.EqualTo(""));
                Assert.That(locateButton.Content?.ToString(), Does.Contain("Locate"));
                Assert.That(locateOrder, Is.EqualTo(new[]
                {
                    BioWare.Extract.SearchLocation.MUSIC,
                    BioWare.Extract.SearchLocation.VOICE,
                    BioWare.Extract.SearchLocation.SOUND,
                    BioWare.Extract.SearchLocation.OVERRIDE,
                    BioWare.Extract.SearchLocation.MODULES,
                    BioWare.Extract.SearchLocation.RIMS,
                    BioWare.Extract.SearchLocation.CHITIN
                }));

                Tuple<byte[], byte[]> buildResult = editor.Build();
                SSF built = SSF.FromBytes(buildResult.Item1);
                Assert.That(built.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(1234));
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolSSF_AllStructuredRows_EditBuildAndReload()
        {
            var editor = new OdyToolSSF(null, null);
            try
            {
                var sounds = (SSFSound[])Enum.GetValues(typeof(SSFSound));
                Assert.That(editor.HasStructuredEditorSurface, Is.True);
                Assert.That(editor.StatusTextForTest, Does.Contain(sounds.Length + " sounds"));
                Assert.That(editor.SoundCountTextForTest, Does.Contain(sounds.Length + " sounds"));

                for (int i = 0; i < sounds.Length; i++)
                {
                    editor.StrrefSpinForTest(sounds[i]).Value = 1000 + i;
                }

                SSF built = SSF.FromBytes(editor.Build().Item1);
                for (int i = 0; i < sounds.Length; i++)
                {
                    Assert.That(built.Get(sounds[i]), Is.EqualTo(1000 + i), sounds[i].ToString());
                }

                editor.Load("all.ssf", "all", ResourceType.SSF, editor.Build().Item1);
                for (int i = 0; i < sounds.Length; i++)
                {
                    Assert.That(editor.StrrefSpinForTest(sounds[i]).Value, Is.EqualTo(1000 + i), sounds[i].ToString());
                }
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolSSF_UndoRedo_RestoresPreviousStrrefState()
        {
            var editor = new OdyToolSSF(null, null);
            try
            {
                var ssf = new SSF();
                ssf.SetData(SSFSound.BATTLE_CRY_1, 111);
                editor.Load("undo.ssf", "undo", ResourceType.SSF, ssf.ToBytes());

                var spin = editor.StrrefSpinForTest(SSFSound.BATTLE_CRY_1);
                spin.Value = 222;

                SSF edited = SSF.FromBytes(editor.Build().Item1);
                Assert.That(edited.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(222));

                editor.UndoForTest();
                SSF undone = SSF.FromBytes(editor.Build().Item1);
                Assert.That(undone.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(111));
                Assert.That(spin.Value, Is.EqualTo(111));

                editor.RedoForTest();
                SSF redone = SSF.FromBytes(editor.Build().Item1);
                Assert.That(redone.Get(SSFSound.BATTLE_CRY_1), Is.EqualTo(222));
                Assert.That(spin.Value, Is.EqualTo(222));
            }
            finally
            {
                editor.Close();
            }
        }

        [Test, Timeout(90000)]
        [AvaloniaTest]
        public void OdyToolSSF_FindStrref_SelectsMatchingRowAndPreview()
        {
            var editor = new OdyToolSSF(null, null);
            try
            {
                editor.New();
                editor.StrrefSpinForTest(SSFSound.SELECT_2).Value = 3210;

                Assert.That(editor.FindStrrefReferencesMenuAvailableForTest, Is.True);

                editor.FindStrrefForTest(3210);

                Assert.That(editor.SelectedSoundForTest, Is.EqualTo(SSFSound.SELECT_2));
                Assert.That(editor.StatusTextForTest, Does.Contain("28 sounds"));
            }
            finally
            {
                editor.Close();
            }
        }
    }
}
