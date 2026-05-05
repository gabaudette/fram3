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
        [Test]
        public void Constructor_ZeroColumnCount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Grid<int>(columnCount: 0, items: new[] { 1 }, itemBuilder: i => new Text(i.ToString()))
            );
        }

        [Test]
        public void Constructor_NegativeColumnCount_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Grid<int>(columnCount: -1, items: new[] { 1 }, itemBuilder: i => new Text(i.ToString()))
            );
        }

        [Test]
        public void Constructor_NullItems_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Grid<int>(columnCount: 2, items: null!, itemBuilder: i => new Text(i.ToString()))
            );
        }

        [Test]
        public void Constructor_NullItemBuilder_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Grid<int>(columnCount: 2, items: new[] { 1 }, itemBuilder: null!)
            );
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
                key: key
            );

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
            var grid = new Grid<int>(columnCount: 2, items: Array.Empty<int>(),
                itemBuilder: i => new Text(i.ToString())
            );

            Assert.That(grid.ColumnSpacing, Is.EqualTo(0f));
            Assert.That(grid.RowSpacing, Is.EqualTo(0f));
        }

        // -----------------------------------------------------------------------
        // LeafElement — no framework children
        // -----------------------------------------------------------------------

        [Test]
        public void GetChildren_ReturnsEmpty()
        {
            var grid = new Grid<int>(columnCount: 3, items: new[] { 1, 2, 3 },
                itemBuilder: i => new Text(i.ToString())
            );
            
            Assert.That(grid.GetChildren(), Is.Empty);
        }

        // -----------------------------------------------------------------------
        // IGridElement — ItemCount and BuildItemAt
        // -----------------------------------------------------------------------

        [Test]
        public void IGridElement_ItemCount_MatchesItemsCount()
        {
            var grid = new Grid<int>(columnCount: 2, items: new[] { 10, 20, 30 },
                itemBuilder: i => new Text(i.ToString())
            );

            IGridElement descriptor = grid;

            Assert.That(descriptor.ItemCount, Is.EqualTo(3));
        }

        [Test]
        public void IGridElement_BuildItemAt_InvokesItemBuilder()
        {
            var built = new List<int>();
            var grid = new Grid<int>(
                columnCount: 2,
                items: new[] { 10, 20, 30 },
                itemBuilder: i =>
                {
                    built.Add(i);
                    return new Text(i.ToString());
                }
            );

            IGridElement descriptor = grid;
            descriptor.BuildItemAt(0);
            descriptor.BuildItemAt(2);

            Assert.That(built, Is.EqualTo(new[] { 10, 30 }));
        }

        [Test]
        public void IGridElement_ColumnCountAndSpacing_Exposed()
        {
            var grid = new Grid<int>(
                columnCount: 4,
                items: new[] { 1 },
                itemBuilder: i => new Text(i.ToString()),
                columnSpacing: 8f,
                rowSpacing: 4f
            );

            IGridElement descriptor = grid;

            Assert.That(descriptor.ColumnCount, Is.EqualTo(4));
            Assert.That(descriptor.ColumnSpacing, Is.EqualTo(8f));
            Assert.That(descriptor.RowSpacing, Is.EqualTo(4f));
        }
    }
}