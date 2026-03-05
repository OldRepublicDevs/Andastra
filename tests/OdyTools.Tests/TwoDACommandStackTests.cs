using System.Collections.Generic;
using System.Collections.ObjectModel;
using OdyTools.Editors.TwoDACommands;
using NUnit.Framework;

namespace OdyTools.Tests
{
    public class TwoDACommandStackTests
    {
        [Test]
        public void TwoDACommandStack_DuplicateColumn_UndoRedo_Works()
        {
            var rows = new ObservableCollection<ObservableCollection<string>>
            {
                new ObservableCollection<string> { "0", "A", "B" },
                new ObservableCollection<string> { "1", "C", "D" }
            };
            var headers = new List<string> { "c1", "c2" };
            var stack = new TwoDACommandStack();

            var command = new DuplicateColumnCommand(rows, headers, 0);
            stack.Execute(command);

            Assert.That(headers.Count, Is.EqualTo(3));
            Assert.That(headers[1], Is.EqualTo("c1"));
            Assert.That(rows[0][2], Is.EqualTo("A"));
            Assert.That(rows[1][2], Is.EqualTo("C"));

            stack.Undo();
            Assert.That(headers.Count, Is.EqualTo(2));
            Assert.That(rows[0].Count, Is.EqualTo(3));
            Assert.That(rows[1].Count, Is.EqualTo(3));

            stack.Redo();
            Assert.That(headers.Count, Is.EqualTo(3));
            Assert.That(rows[0][2], Is.EqualTo("A"));
            Assert.That(rows[1][2], Is.EqualTo("C"));
        }

        [Test]
        public void TwoDACommandStack_MultiLevelSort_UndoRestoresOrder()
        {
            var rows = new ObservableCollection<ObservableCollection<string>>
            {
                new ObservableCollection<string> { "0", "b", "2" },
                new ObservableCollection<string> { "1", "a", "2" },
                new ObservableCollection<string> { "2", "a", "1" }
            };
            var stack = new TwoDACommandStack();

            var command = new MultiLevelSortCommand(rows, new List<(int columnIndex, bool ascending)>
            {
                (0, true),
                (1, true)
            });
            stack.Execute(command);

            Assert.That(rows[0][1], Is.EqualTo("a"));
            Assert.That(rows[0][2], Is.EqualTo("1"));
            Assert.That(rows[1][1], Is.EqualTo("a"));
            Assert.That(rows[1][2], Is.EqualTo("2"));
            Assert.That(rows[2][1], Is.EqualTo("b"));

            stack.Undo();
            Assert.That(rows[0][1], Is.EqualTo("b"));
            Assert.That(rows[1][1], Is.EqualTo("a"));
            Assert.That(rows[2][1], Is.EqualTo("a"));
        }

        [Test]
        public void TwoDACommandStack_RemoveDuplicateRows_UndoRestoresRows()
        {
            var rows = new ObservableCollection<ObservableCollection<string>>
            {
                new ObservableCollection<string> { "0", "A", "X" },
                new ObservableCollection<string> { "0", "A", "X" },
                new ObservableCollection<string> { "2", "B", "Y" }
            };
            var stack = new TwoDACommandStack();

            stack.Execute(new RemoveDuplicateRowsCommand(rows));
            Assert.That(rows.Count, Is.EqualTo(2));

            stack.Undo();
            Assert.That(rows.Count, Is.EqualTo(3));
            Assert.That(rows[1][1], Is.EqualTo("A"));
            Assert.That(rows[1][2], Is.EqualTo("X"));
        }
    }
}
