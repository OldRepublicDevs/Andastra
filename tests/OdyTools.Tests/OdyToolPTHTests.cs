using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Headless;
using BioWare.Common;
using BioWare.Resource.Formats.GFF.Generics;
using NUnit.Framework;
using OdyTools.Editors;

namespace OdyTools.Tests
{
    public class OdyToolPTHTests
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

        [Test]
        public async Task OdyToolPTH_New_AllowsSelectingEditingAndConnectingNodes()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolPTH(null, null);
                    editor.New();

                    editor.AddNode(1, 2);
                    editor.AddNode(3, 4);
                    editor.SelectNodeIndicesForTest(0, 1);
                    editor.AddEdgeBetweenSelectedForTest();

                    Assert.That(editor.NodeCount, Is.EqualTo(2));
                    Assert.That(editor.SelectedNodeIndicesForTest(), Is.EqualTo(new[] { 0, 1 }));
                    Assert.That(editor.ConnectionCount, Is.EqualTo(2), "PTH edges should be bidirectional.");

                    editor.SelectNodeIndicesForTest(0);
                    editor.MoveSelected(5, 6);

                    Assert.That(editor.NodeAt(0).X, Is.EqualTo(5).Within(0.001));
                    Assert.That(editor.NodeAt(0).Y, Is.EqualTo(6).Within(0.001));
                    Assert.That(editor.SelectedNodeIndicesForTest(), Is.EqualTo(new[] { 0 }));
                    Assert.That(editor.Build().Item1.Length, Is.GreaterThan(0));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolPTH_CanvasToolModesExposeHolocronAddAndConnectWorkflow()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                bool controlsAvailable = false;
                string initialMode = null;
                string finalMode = null;
                string xamlLoadError = null;
                int nodeCount = -1;
                int connectionCount = -1;
                Vector2 firstNode = default;
                Vector2 secondNode = default;
                int[] connectedSelection = Array.Empty<int>();
                int[] finalSelection = Array.Empty<int>();

                await session.Dispatch(() =>
                {
                    var editor = new OdyToolPTH(null, null);
                    editor.New();

                    controlsAvailable = editor.ToolModeControlsAvailableForTest;
                    initialMode = editor.ToolModeForTest;
                    xamlLoadError = editor.XamlLoadErrorForTest;

                    editor.SetToolModeForTest("AddNode");
                    editor.HandleCanvasToolClickForTest(1, 2);
                    editor.HandleCanvasToolClickForTest(3, 4);
                    nodeCount = editor.NodeCount;
                    firstNode = editor.NodeAt(0);
                    secondNode = editor.NodeAt(1);

                    editor.SetToolModeForTest("Connect");
                    editor.HandleCanvasToolClickForTest(1, 2);
                    editor.HandleCanvasToolClickForTest(3, 4);
                    connectedSelection = editor.SelectedNodeIndicesForTest().ToArray();
                    connectionCount = editor.ConnectionCount;

                    editor.SetToolModeForTest("Select");
                    editor.HandleCanvasToolClickForTest(1, 2);
                    finalMode = editor.ToolModeForTest;
                    finalSelection = editor.SelectedNodeIndicesForTest().ToArray();
                }, CancellationToken.None);

                Assert.That(controlsAvailable, Is.True, xamlLoadError);
                Assert.That(initialMode, Is.EqualTo("Select"));
                Assert.That(nodeCount, Is.EqualTo(2));
                Assert.That(firstNode, Is.EqualTo(new Vector2(1, 2)));
                Assert.That(secondNode, Is.EqualTo(new Vector2(3, 4)));
                Assert.That(connectedSelection, Is.EqualTo(new[] { 0, 1 }));
                Assert.That(connectionCount, Is.EqualTo(2), "Connect mode should create bidirectional PTH edges.");
                Assert.That(finalMode, Is.EqualTo("Select"));
                Assert.That(finalSelection, Is.EqualTo(new[] { 0 }));
            }
        }

        [Test]
        public async Task OdyToolPTH_AddNodeAction_UsesTypedCoordinatesBeforeCanvasMousePosition()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolPTH(null, null);
                    editor.New();

                    editor.SetNodePositionInputsForTest(12.5f, -3.25f);
                    editor.AddNodeFromEditActionForTest();

                    Assert.That(editor.NodeCount, Is.EqualTo(1));
                    Assert.That(editor.NodeAt(0).X, Is.EqualTo(12.5f).Within(0.001));
                    Assert.That(editor.NodeAt(0).Y, Is.EqualTo(-3.25f).Within(0.001));
                    Assert.That(editor.SelectedNodeIndicesForTest(), Is.EqualTo(new[] { 0 }));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolPTH_AddNodeAction_UsesLastCanvasWorldPositionAfterMouseMove()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolPTH(null, null);
                    editor.New();

                    editor.SetNodePositionInputsForTest(1, 1);
                    editor.UpdateMousePosition(8.75f, 9.5f);
                    editor.AddNodeFromEditActionForTest();

                    Assert.That(editor.NodeCount, Is.EqualTo(1));
                    Assert.That(editor.NodeAt(0).X, Is.EqualTo(8.75f).Within(0.001));
                    Assert.That(editor.NodeAt(0).Y, Is.EqualTo(9.5f).Within(0.001));
                    Assert.That(editor.SelectedNodeIndicesForTest(), Is.EqualTo(new[] { 0 }));
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolPTH_Load_RebuildsEditablePathState()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var pth = new PTH();
                    int first = pth.Add(10, 20);
                    int second = pth.Add(30, 40);
                    pth.Connect(first, second);
                    pth.Connect(second, first);

                    byte[] data = PTHAuto.BytesPth(pth);
                    var editor = new OdyToolPTH(null, null);
                    editor.Load("test.pth", "test", ResourceType.PTH, data);

                    Assert.That(editor.NodeCount, Is.EqualTo(2));
                    Assert.That(editor.ConnectionCount, Is.EqualTo(2));
                    Assert.That(editor.NodeAt(0), Is.EqualTo(new Vector2(10, 20)));
                    Assert.That(editor.NodeAt(1), Is.EqualTo(new Vector2(30, 40)));

                    editor.SelectNodeIndicesForTest(0, 1);
                    editor.RemoveEdgeBetweenSelectedForTest();

                    Assert.That(editor.ConnectionCount, Is.EqualTo(0));

                    PTH rebuilt = PTHAuto.ReadPth(editor.Build().Item1);
                    Assert.That(rebuilt.Count, Is.EqualTo(2));
                    Assert.That(rebuilt.GetConnections().Any(), Is.False);
                }, CancellationToken.None);
            }
        }

        [Test]
        public async Task OdyToolPTH_LoadVendorPth_BuildPreservesNodesAndConnections()
        {
            byte[] data = File.ReadAllBytes(VendorTestFile("test.pth"));
            PTH original = PTHAuto.ReadPth(data);

            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolPTH(null, null);
                    editor.Load("test.pth", "test", ResourceType.PTH, data);

                    PTH rebuilt = PTHAuto.ReadPth(editor.Build().Item1);

                    Assert.That(rebuilt.Count, Is.EqualTo(original.Count));
                    for (int i = 0; i < original.Count; i++)
                    {
                        Assert.That(rebuilt.GetPoint(i).X, Is.EqualTo(original.GetPoint(i).X).Within(0.0001f), "X mismatch at node " + i);
                        Assert.That(rebuilt.GetPoint(i).Y, Is.EqualTo(original.GetPoint(i).Y).Within(0.0001f), "Y mismatch at node " + i);
                    }

                    var originalConnections = original.GetConnections()
                        .Select(edge => (edge.SourceIndex, edge.TargetIndex))
                        .OrderBy(edge => edge.SourceIndex)
                        .ThenBy(edge => edge.TargetIndex)
                        .ToArray();
                    var rebuiltConnections = rebuilt.GetConnections()
                        .Select(edge => (edge.SourceIndex, edge.TargetIndex))
                        .OrderBy(edge => edge.SourceIndex)
                        .ThenBy(edge => edge.TargetIndex)
                        .ToArray();

                    Assert.That(rebuiltConnections, Is.EqualTo(originalConnections));
                }, CancellationToken.None);
            }
        }
    }
}
