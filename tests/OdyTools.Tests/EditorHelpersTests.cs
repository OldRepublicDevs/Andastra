using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using BioWare.Common;
using System;
using NUnit.Framework;
using OdyTools.Editors;

namespace OdyTools.Tests
{
    public class EditorHelpersTests
    {
        [Test]
        [AvaloniaTest]
        public void FindControlSafe_FindsNamedLogicalChildBeforeWindowIsShown()
        {
            var target = new TextBlock { Name = "targetLabel", Text = "Ready" };
            var window = new Window
            {
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Name = "otherLabel" },
                        target
                    }
                }
            };

            Assert.That(EditorHelpers.FindControlSafe<TextBlock>(window, "targetLabel"), Is.SameAs(target));
        }

        [TestCase("SAVEGAME.sav/module.sav/resource.git", true)]
        [TestCase("saves/000001 - Test/SAVEGAME.sav/module.git", true)]
        [TestCase("saves/000001/SAVEGAME.sav/module1.sav/module2.sav/resource.git", true)]
        [TestCase("savegame.sav/module.git", true)]
        [TestCase("SaveGame.SAV/module.git", true)]
        [TestCase("modules/test_area/resource.are", false)]
        [TestCase("sample.sav", false)]
        public void IsSaveGameResourcePath_MatchesHolocronVirtualSavePaths(string path, bool expected)
        {
            Assert.That(Editor.IsSaveGameResourcePath(path), Is.EqualTo(expected));
        }

        [Test]
        [AvaloniaTest]
        public void Load_SetsSaveGameResourceFlagFromVirtualSavePath()
        {
            var editor = new MinimalEditor();

            editor.Load("SAVEGAME.sav/module.sav/resource.git", "resource", ResourceType.GIT, new byte[] { 1, 2, 3 });

            Assert.That(editor.IsSaveGameResource, Is.True);

            editor.Load("modules/test_area/resource.git", "resource", ResourceType.GIT, new byte[] { 1, 2, 3 });

            Assert.That(editor.IsSaveGameResource, Is.False);
        }

        private sealed class MinimalEditor : Editor
        {
            public MinimalEditor()
                : base(null, "MinimalEditor", "none", new[] { ResourceType.GIT }, new[] { ResourceType.GIT }, null)
            {
            }

            public override Tuple<byte[], byte[]> Build()
            {
                return Tuple.Create(Array.Empty<byte>(), Array.Empty<byte>());
            }
        }
    }
}
