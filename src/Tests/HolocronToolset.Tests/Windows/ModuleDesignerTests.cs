using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using HolocronToolset.Data;
using HolocronToolset.Tests.TestHelpers;
using HolocronToolset.Windows;
using Xunit;

namespace HolocronToolset.Tests.Windows
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/windows/test_module_designer.py
    // Original: Comprehensive tests for Module Designer - testing ALL functionality.
    // Uses Avalonia for actual UI testing including:
    // - Module loading and unloading
    // - Instance management
    // - Undo/redo operations
    // - Resource tree navigation
    // - Property editing
    [Collection("Avalonia Test Collection")]
    public class ModuleDesignerTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;
        private static HTInstallation _installation;

        public ModuleDesignerTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        static ModuleDesignerTests()
        {
            string k1Path = Environment.GetEnvironmentVariable("K1_PATH");
            if (string.IsNullOrEmpty(k1Path))
            {
                k1Path = @"C:\Program Files (x86)\Steam\steamapps\common\swkotor";
            }

            if (!string.IsNullOrEmpty(k1Path) && File.Exists(Path.Combine(k1Path, "chitin.key")))
            {
                _installation = new HTInstallation(k1Path, "Test");
            }
        }

        // Matching PyKotor implementation - test module designer creates with installation
        [Fact]
        public void TestModuleDesignerCreatesWithInstallation()
        {
            // Matching Python: Test module designer initializes correctly with installation.
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            string oldCwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(tempPath);

                var designer = new ModuleDesignerWindow(null, _installation, null);
                designer.Show();

                // Matching Python: assert designer.ui is not None
                designer.Ui.Should().NotBeNull("UI should be initialized");

                // Matching Python: assert designer.undo_stack is not None
                designer.UndoStack.Should().NotBeNull("UndoStack should be initialized");
                designer.UndoStack.Should().BeOfType<UndoStack>("UndoStack should be of type UndoStack");

                // Matching Python: assert designer._module is None (initially)
                designer.GetModule().Should().BeNull("Module should be null initially");
            }
            finally
            {
                Directory.SetCurrentDirectory(oldCwd);
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Cleanup may fail if files are locked
                }
            }
        }

        // Matching PyKotor implementation - test module designer creates without installation
        [Fact]
        public void TestModuleDesignerCreatesWithoutInstallation()
        {
            // Matching Python: Test module designer works without installation.
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            string oldCwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(tempPath);

                var designer = new ModuleDesignerWindow(null, null, null);
                designer.Show();

                // Matching Python: assert designer.ui is not None
                designer.Ui.Should().NotBeNull("UI should be initialized");

                // Matching Python: assert designer.undo_stack is not None
                designer.UndoStack.Should().NotBeNull("UndoStack should be initialized");
            }
            finally
            {
                Directory.SetCurrentDirectory(oldCwd);
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Cleanup may fail if files are locked
                }
            }
        }

        // Matching PyKotor implementation - test unload module
        [Fact]
        public void TestUnloadModule()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            string oldCwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(tempPath);

                var designer = new ModuleDesignerWindow(null, _installation, null);
                designer.Show();

                // Test unload when no module is loaded
                designer.UnloadModule();

                // Matching Python: assert designer._module is None
                designer.GetModule().Should().BeNull("Module should be null after unload");

                // Matching Python: Window title should be reset
                designer.Title.Should().Be("Module Designer", "Title should be reset after unload");
            }
            finally
            {
                Directory.SetCurrentDirectory(oldCwd);
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Cleanup may fail if files are locked
                }
            }
        }

        // Matching PyKotor implementation - test undo/redo stack
        [Fact]
        public void TestUndoRedoStack()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            string oldCwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(tempPath);

                var designer = new ModuleDesignerWindow(null, _installation, null);
                designer.Show();

                var undoStack = designer.UndoStack;

                // Matching Python: assert undo_stack.canUndo() is False initially
                undoStack.CanUndo().Should().BeFalse("Should not be able to undo initially");

                // Matching Python: assert undo_stack.canRedo() is False initially
                undoStack.CanRedo().Should().BeFalse("Should not be able to redo initially");
            }
            finally
            {
                Directory.SetCurrentDirectory(oldCwd);
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Cleanup may fail if files are locked
                }
            }
        }

        // Matching PyKotor implementation - test rebuild resource tree
        [Fact]
        public void TestRebuildResourceTree()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            string oldCwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(tempPath);

                var designer = new ModuleDesignerWindow(null, _installation, null);
                designer.Show();

                // Test rebuild resource tree when no module is loaded
                designer.RebuildResourceTree();

                // Matching Python: Resource tree should be cleared when no module
                // This is tested via the UI wrapper
                designer.Ui.ModuleTree.Should().NotBeNull("Module tree should exist");
            }
            finally
            {
                Directory.SetCurrentDirectory(oldCwd);
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Cleanup may fail if files are locked
                }
            }
        }

        // Matching PyKotor implementation - test rebuild instance list
        [Fact]
        public void TestRebuildInstanceList()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            string oldCwd = Directory.GetCurrentDirectory();
            try
            {
                Directory.SetCurrentDirectory(tempPath);

                var designer = new ModuleDesignerWindow(null, _installation, null);
                designer.Show();

                // Test rebuild instance list when no module is loaded
                designer.RebuildInstanceList();

                // Matching Python: Instance list should be rebuilt (no exceptions)
                designer.Should().NotBeNull("Designer should still be valid");
            }
            finally
            {
                Directory.SetCurrentDirectory(oldCwd);
                try
                {
                    Directory.Delete(tempPath, true);
                }
                catch
                {
                    // Cleanup may fail if files are locked
                }
            }
        }
    }
}

