#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Layout
{
    [TestFixture]
    internal sealed class GridTests
    {
        // -----------------------------------------------------------------------
        // Constructor validation
        // -----------------------------------------------------------------------

        [Test]
        public void Constructor_ZeroColumnCount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Grid<int>(columnCount: 0, items: new[] { 1 }, itemBuilder: i => new Text(i.ToString())));
        }

        [Test]
        public void Constructor_NegativeColumnCount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Grid<int>(columnCount: -1, items: new[] { 1 }, itemBuilder: i => new Text(i.ToString())));
        }

        [Test]
        public void Constructor_NullItems_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Grid<int>(columnCount: 2, items: null!, itemBuilder: i => new Text(i.ToString())));
        }

        [Test]
        public void Constructor_NullItemBuilder_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Grid<int>(columnCount: 2, items: new[] { 1 }, itemBuilder: null!));
        }

        [Test]
        public void Constructor_StoresProperties()
        {
            var items = new[] { 1, 2, 3 };
            Func<int, Element> builder = i => new Text(i.ToString());
            var key = new ValueKey<string>("grid");

            var grid = new Grid<int>(
                columnCount: 3,
                items: items,
                itemBuilder: builder,
                columnSpacing: 8f,
                rowSpacing: 4f,
                key: key);

            Assert.That(grid.ColumnCount, Is.EqualTo(3));
            Assert.That(grid.Items, Is.SameAs(items));
            Assert.That(grid.ItemBuilder, Is.SameAs(builder));
            Assert.That(grid.ColumnSpacing, Is.EqualTo(8f));
            Assert.That(grid.RowSpacing, Is.EqualTo(4f));
            Assert.That(grid.Key, Is.SameAs(key));
        }

        [Test]
        public void Constructor_DefaultSpacing_IsZero()
        {
            var grid = new Grid<int>(columnCount: 2, items: Array.Empty<int>(), itemBuilder: i => new Text(i.ToString()));

            Assert.That(grid.ColumnSpacing, Is.EqualTo(0f));
            Assert.That(grid.RowSpacing, Is.EqualTo(0f));
        }

        // -----------------------------------------------------------------------
        // Build — structure
        // -----------------------------------------------------------------------

        [Test]
        public void Build_EmptyItems_ReturnsColumn()
        {
            var grid = new Grid<int>(columnCount: 3, items: Array.Empty<int>(), itemBuilder: i => new Text(i.ToString()));
            var result = grid.Build(null!);

            Assert.That(result, Is.InstanceOf<Column>());
        }

        [Test]
        public void Build_EmptyItems_ReturnsEmptyColumn()
        {
            var grid = new Grid<int>(columnCount: 3, items: Array.Empty<int>(), itemBuilder: i => new Text(i.ToString()));
            var column = (Column)grid.Build(null!);

            Assert.That(column.GetChildren(), Is.Empty);
        }

        [Test]
        public void Build_ExactlyOneRow_ProducesOneRow()
        {
            var grid = new Grid<int>(columnCount: 3, items: new[] { 1, 2, 3 }, itemBuilder: i => new Text(i.ToString()));
            var column = (Column)grid.Build(null!);
            var rows = column.GetChildren();

            Assert.That(rows, Has.Count.EqualTo(1));
            Assert.That(rows[0], Is.InstanceOf<Row>());
        }

        [Test]
        public void Build_TwoFullRows_ProducesTwoRows()
        {
            var grid = new Grid<int>(columnCount: 2, items: new[] { 1, 2, 3, 4 }, itemBuilder: i => new Text(i.ToString()));
            var column = (Column)grid.Build(null!);

            Assert.That(column.GetChildren(), Has.Count.EqualTo(2));
        }

        [Test]
        public void Build_PartialLastRow_FilledWithEmptyExpanded()
        {
            // 3 items in a 2-column grid: row 0 = [1,2], row 1 = [3, empty]
            var grid = new Grid<int>(columnCount: 2, items: new[] { 1, 2, 3 }, itemBuilder: i => new Text(i.ToString()));
            var column = (Column)grid.Build(null!);
            var rows = column.GetChildren();

            Assert.That(rows, Has.Count.EqualTo(2));
            var lastRow = (Row)rows[1];
            var cells = lastRow.GetChildren();
            // Two Expanded cells: first wraps a Text, second has no child
            Assert.That(cells, Has.Count.EqualTo(2));
            var lastCell = (Expanded)cells[1];
            Assert.That(lastCell.Child, Is.Null);
        }

        [Test]
        public void Build_WithRowSpacing_InsertsSizedBoxBetweenRows()
        {
            var grid = new Grid<int>(columnCount: 1, items: new[] { 1, 2 }, itemBuilder: i => new Text(i.ToString()), rowSpacing: 8f);
            var column = (Column)grid.Build(null!);
            var children = column.GetChildren();

            // Row, SizedBox, Row
            Assert.That(children, Has.Count.EqualTo(3));
            Assert.That(children[1], Is.InstanceOf<SizedBox>());
        }

        [Test]
        public void Build_WithColumnSpacing_InsertsSizedBoxBetweenCells()
        {
            var grid = new Grid<int>(columnCount: 2, items: new[] { 1, 2 }, itemBuilder: i => new Text(i.ToString()), columnSpacing: 4f);
            var column = (Column)grid.Build(null!);
            var row = (Row)column.GetChildren()[0];
            var cells = row.GetChildren();

            // Expanded, SizedBox, Expanded
            Assert.That(cells, Has.Count.EqualTo(3));
            Assert.That(cells[1], Is.InstanceOf<SizedBox>());
        }

        [Test]
        public void Build_NoSpacing_NoSizedBoxInserted()
        {
            var grid = new Grid<int>(columnCount: 2, items: new[] { 1, 2, 3, 4 }, itemBuilder: i => new Text(i.ToString()));
            var column = (Column)grid.Build(null!);
            var children = column.GetChildren();

            // Two rows, no SizedBox spacers
            Assert.That(children, Has.Count.EqualTo(2));
            foreach (var child in children)
                Assert.That(child, Is.InstanceOf<Row>());
        }

        [Test]
        public void Build_ItemBuilderCalledForEachItem()
        {
            var called = new List<int>();
            var grid = new Grid<int>(
                columnCount: 2,
                items: new[] { 10, 20, 30 },
                itemBuilder: i => { called.Add(i); return new Text(i.ToString()); });

            grid.Build(null!);

            Assert.That(called, Is.EqualTo(new[] { 10, 20, 30 }));
        }
    }
}
