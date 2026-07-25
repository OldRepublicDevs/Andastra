using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Input;
using Avalonia.Interactivity;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using NUnit.Framework;
using OdyTools.Editors;

namespace OdyTools.Tests
{
    public class OdyToolFACTests
    {
        private static byte[] MinimalFacBytes()
        {
            var fac = new FAC();
            fac.Factions.Add(new FACFaction { Name = "Player", IsGlobal = true });
            fac.Factions.Add(new FACFaction { Name = "Hostile", IsGlobal = true });
            fac.Reputations.Add(new FACReputation { FactionId1 = 0, FactionId2 = 1, Reputation = 50 });
            return FACHelpers.BytesFac(fac, ResourceType.FAC);
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_LoadMinimalFac_BuildsValidFac()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalFacBytes();
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, data);
                    Tuple<byte[], byte[]> result = editor.Build();
                    byte[] built = result.Item1;
                    Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
                    FAC roundtrip = FACHelpers.ReadFac(built);
                    Assert.That(roundtrip.Factions.Count, Is.EqualTo(2));
                    Assert.That(roundtrip.Factions[0].Name, Is.EqualTo("Player"));
                    Assert.That(roundtrip.Reputations.Count, Is.EqualTo(1));
                    Assert.That(roundtrip.Reputations[0].Reputation, Is.EqualTo(50));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_ModifyFactionName_Roundtrips()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalFacBytes();
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, data);
                    editor.Fac.Factions[0].Name = "Renamed";
                    Tuple<byte[], byte[]> result = editor.Build();
                    FAC roundtrip = FACHelpers.ReadFac(result.Item1);
                    Assert.That(roundtrip.Factions[0].Name, Is.EqualTo("Renamed"));
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_RemoveFaction_ReindexesReputations()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    byte[] data = MinimalFacBytes();
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, data);
                    Assert.That(editor.Fac.Factions.Count, Is.EqualTo(2));
                    Assert.That(editor.Fac.Reputations.Count, Is.EqualTo(1));

                    int removedIndex = 0;
                    editor.Fac.Factions.RemoveAt(removedIndex);
                    editor.Fac.Reputations.RemoveAll(r => r.FactionId1 == removedIndex || r.FactionId2 == removedIndex);
                    foreach (FACReputation rep in editor.Fac.Reputations)
                    {
                        if (rep.FactionId1 > removedIndex)
                        {
                            rep.FactionId1--;
                        }
                        if (rep.FactionId2 > removedIndex)
                        {
                            rep.FactionId2--;
                        }
                    }

                    Tuple<byte[], byte[]> result = editor.Build();
                    FAC roundtrip = FACHelpers.ReadFac(result.Item1);
                    Assert.That(roundtrip.Factions.Count, Is.EqualTo(1));
                    Assert.That(roundtrip.Factions[0].Name, Is.EqualTo("Hostile"));
                    Assert.That(roundtrip.Reputations, Is.Empty);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_ContextMenusExposeHolocronAddRemoveActions()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, MinimalFacBytes());

                    Assert.That(editor.FactionListContextMenuForTest, Is.Not.Null);
                    Assert.That(editor.ReputationListContextMenuForTest, Is.Not.Null);
                    Assert.That(MenuHeaders(editor.FactionListContextMenuForTest), Is.EqualTo(new[] { "Remove Faction", "Add Faction" }));
                    Assert.That(MenuHeaders(editor.ReputationListContextMenuForTest), Is.EqualTo(new[] { "Remove Reputation", "Add Reputation" }));

                    editor.FactionListForTest.SelectedIndex = 0;
                    var removeFaction = FindMenuItem(editor.FactionListContextMenuForTest, "ctxRemoveFaction");
                    removeFaction.RaiseEvent(new RoutedEventArgs(MenuItem.ClickEvent));

                    Assert.That(editor.Fac.Factions.Count, Is.EqualTo(1));
                    Assert.That(editor.Fac.Reputations, Is.Empty);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_DeleteKeyRemovesSelectedReputation()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, MinimalFacBytes());
                    editor.ReputationListForTest.SelectedIndex = 0;

                    editor.ReputationListForTest.RaiseEvent(CreateKeyEventArgs(Key.Delete));

                    Assert.That(editor.Fac.Reputations, Is.Empty);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_DeleteKeyRemovesSelectedFactionAndRelatedReputationsLikeHolocron()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, MinimalFacBytes());
                    editor.FactionListForTest.SelectedIndex = 0;

                    editor.FactionListForTest.RaiseEvent(CreateKeyEventArgs(Key.Delete));

                    Assert.That(editor.Fac.Factions.Count, Is.EqualTo(1));
                    Assert.That(editor.Fac.Factions[0].Name, Is.EqualTo("Hostile"));
                    Assert.That(editor.Fac.Reputations, Is.Empty);
                    Assert.That(FACHelpers.ReadFac(editor.Build().Item1).Reputations, Is.Empty);
                }, CancellationToken.None);
            }
        }

        [Test, Timeout(60000)]
        public async Task FACEditor_ReputationEndpointEdits_Roundtrip()
        {
            using (var session = HeadlessUnitTestSession.StartNew(typeof(TestApp)))
            {
                await session.Dispatch(() =>
                {
                    var fac = new FAC();
                    fac.Factions.Add(new FACFaction { Name = "Player", IsGlobal = true });
                    fac.Factions.Add(new FACFaction { Name = "Hostile", IsGlobal = true });
                    fac.Factions.Add(new FACFaction { Name = "Neutral", IsGlobal = true });
                    fac.Reputations.Add(new FACReputation { FactionId1 = 0, FactionId2 = 1, Reputation = 50 });

                    byte[] data = FACHelpers.BytesFac(fac, ResourceType.FAC);
                    var editor = new OdyToolFAC(null, null);
                    editor.Load("repute.fac", "repute", ResourceType.FAC, data);

                    Assert.That(editor.HasStructuredEditorSurface, Is.True);
                    editor.ReputationListForTest.SelectedIndex = 0;
                    editor.ReputationFactionId1SpinForTest.Value = 2;
                    editor.ReputationFactionId2SpinForTest.Value = 0;
                    editor.ReputationValueSpinForTest.Value = 15;

                    Tuple<byte[], byte[]> result = editor.Build();
                    FAC roundtrip = FACHelpers.ReadFac(result.Item1);

                    Assert.That(roundtrip.Reputations.Count, Is.EqualTo(1));
                    Assert.That(roundtrip.Reputations[0].FactionId1, Is.EqualTo(2));
                    Assert.That(roundtrip.Reputations[0].FactionId2, Is.EqualTo(0));
                    Assert.That(roundtrip.Reputations[0].Reputation, Is.EqualTo(15));
                }, CancellationToken.None);
            }
        }

        private static string[] MenuHeaders(ContextMenu menu)
        {
            var headers = new System.Collections.Generic.List<string>();
            foreach (object item in menu.Items)
            {
                if (item is MenuItem menuItem)
                {
                    headers.Add(menuItem.Header?.ToString());
                }
            }
            return headers.ToArray();
        }

        private static MenuItem FindMenuItem(ContextMenu menu, string name)
        {
            foreach (object item in menu.Items)
            {
                if (item is MenuItem menuItem && menuItem.Name == name)
                {
                    return menuItem;
                }
            }
            return null;
        }

        private static KeyEventArgs CreateKeyEventArgs(Key key)
        {
            var args = new KeyEventArgs();
            typeof(RoutedEventArgs).GetProperty("RoutedEvent", BindingFlags.Public | BindingFlags.Instance)
                ?.SetValue(args, Avalonia.Input.InputElement.KeyDownEvent, null);
            typeof(KeyEventArgs).GetProperty("Key", BindingFlags.Public | BindingFlags.Instance)
                ?.SetValue(args, key, null);
            return args;
        }
    }
}
