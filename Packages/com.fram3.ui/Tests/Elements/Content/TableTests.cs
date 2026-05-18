#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using NUnit.Framework;

namespace Fram3.UI.Tests.Elements.Content
{
    [TestFixture]
    internal sealed class TableTests
    {
        private sealed class Item
        {
            public string Name { get; }
            public string Type { get; }
            public int Value { get; }

            public Item(string name, string type, int value)
            {
                Name = name;
                Type = type;
                Value = value;
            }
        }

        private static TableColumn<Item> NameCol(bool sortable = true)
            => new TableColumn<Item>("Name", r => r.Name, sortable: sortable);

        private static TableColumn<Item> TypeCol()
            => new TableColumn<Item>("Type", r => r.Type);

        private static TableColumn<Item> ValueCol(float? width = null)
            => new TableColumn<Item>("Value", r => r.Value.ToString(), width: width);

        private static IReadOnlyList<TableColumn<Item>> TwoCols()
            => new[] { NameCol(), TypeCol() };

        private static IReadOnlyList<TableColumn<Item>> ThreeCols()
            => new[] { NameCol(), TypeCol(), ValueCol() };

        private static IReadOnlyList<Item> ThreeRows()
            => new[]
            {
                new Item("Sword", "Weapon", 150),
                new Item("Shield", "Armor", 200),
                new Item("Potion", "Consumable", 50)
            };

        private static IReadOnlyList<Item> EmptyRows()
            => Array.Empty<Item>();

        // --- TableColumn constructor ---

        [Test]
        public void Column_Constructor_StoresHeader()
        {
            var col = NameCol();
            Assert.That(col.Header, Is.EqualTo("Name"));
        }

        [Test]
        public void Column_Constructor_NullHeader_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new TableColumn<Item>(null!, r => r.Name));
        }

        [Test]
        public void Column_Constructor_NullCellText_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new TableColumn<Item>("Name", null!));
        }

        [Test]
        public void Column_Constructor_StoresCellText()
        {
            Func<Item, string> fn = r => r.Name;
            var col = new TableColumn<Item>("Name", fn);
            Assert.That(col.CellText, Is.SameAs(fn));
        }

        [Test]
        public void Column_Constructor_DefaultWidth_IsNull()
        {
            Assert.That(NameCol().Width, Is.Null);
        }

        [Test]
        public void Column_Constructor_StoresWidth()
        {
            var col = new TableColumn<Item>("Name", r => r.Name, width: 120f);
            Assert.That(col.Width, Is.EqualTo(120f));
        }

        [Test]
        public void Column_Constructor_DefaultSortable_IsTrue()
        {
            Assert.That(NameCol().Sortable, Is.True);
        }

        [Test]
        public void Column_Constructor_SortableFalse_Stores()
        {
            Assert.That(NameCol(sortable: false).Sortable, Is.False);
        }

        [Test]
        public void Column_CellText_InvokesCorrectly()
        {
            var col = NameCol();
            var item = new Item("Sword", "Weapon", 150);
            Assert.That(col.CellText(item), Is.EqualTo("Sword"));
        }

        // --- Table constructor ---

        [Test]
        public void Constructor_StoresColumns()
        {
            var cols = TwoCols();
            var table = new Table<Item>(cols, ThreeRows());
            Assert.That(table.Columns, Is.SameAs(cols));
        }

        [Test]
        public void Constructor_StoresRows()
        {
            var rows = ThreeRows();
            var table = new Table<Item>(TwoCols(), rows);
            Assert.That(table.Rows, Is.SameAs(rows));
        }

        [Test]
        public void Constructor_NullColumns_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Table<Item>(null!, ThreeRows()));
        }

        [Test]
        public void Constructor_EmptyColumns_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                new Table<Item>(Array.Empty<TableColumn<Item>>(), ThreeRows()));
        }

        [Test]
        public void Constructor_NullRows_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new Table<Item>(TwoCols(), null!));
        }

        [Test]
        public void Constructor_DefaultOnRowSelected_IsNull()
        {
            var table = new Table<Item>(TwoCols(), ThreeRows());
            Assert.That(table.OnRowSelected, Is.Null);
        }

        [Test]
        public void Constructor_StoresOnRowSelected()
        {
            Action<Item> cb = _ => { };
            var table = new Table<Item>(TwoCols(), ThreeRows(), onRowSelected: cb);
            Assert.That(table.OnRowSelected, Is.SameAs(cb));
        }

        [Test]
        public void Constructor_DefaultStripedRows_IsTrue()
        {
            var table = new Table<Item>(TwoCols(), ThreeRows());
            Assert.That(table.StripedRows, Is.True);
        }

        [Test]
        public void Constructor_StripedRowsFalse_Stores()
        {
            var table = new Table<Item>(TwoCols(), ThreeRows(), stripedRows: false);
            Assert.That(table.StripedRows, Is.False);
        }

        // --- GetChildren ---

        [Test]
        public void GetChildren_IsEmpty()
        {
            Assert.That(new Table<Item>(TwoCols(), ThreeRows()).GetChildren(), Is.Empty);
        }

        // --- CreateState ---

        [Test]
        public void CreateState_ReturnsNonNull()
        {
            Assert.That(new Table<Item>(TwoCols(), ThreeRows()).CreateState(), Is.Not.Null);
        }

        [Test]
        public void CreateState_ReturnsDifferentInstances()
        {
            var table = new Table<Item>(TwoCols(), ThreeRows());
            Assert.That(table.CreateState(), Is.Not.SameAs(table.CreateState()));
        }

        // --- Mount / Build ---

        [Test]
        public void Mount_EmptyRows_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Table<Item>(TwoCols(), EmptyRows()), null));
        }

        [Test]
        public void Build_EmptyRows_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(TwoCols(), EmptyRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_ThreeRows_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Table<Item>(ThreeCols(), ThreeRows()), null));
        }

        [Test]
        public void Build_ThreeRows_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(ThreeCols(), ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithOnRowSelected_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() =>
                expander.Mount(new Table<Item>(TwoCols(), ThreeRows(), onRowSelected: _ => { }), null));
        }

        [Test]
        public void Build_WithOnRowSelected_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(
                new Table<Item>(TwoCols(), ThreeRows(), onRowSelected: _ => { }), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_StripedRowsFalse_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() =>
                expander.Mount(new Table<Item>(TwoCols(), ThreeRows(), stripedRows: false), null));
        }

        [Test]
        public void Build_StripedRowsFalse_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(
                new Table<Item>(TwoCols(), ThreeRows(), stripedRows: false), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithFixedWidthColumn_DoesNotThrow()
        {
            var cols = new[] { NameCol(), ValueCol(width: 80f) };
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Table<Item>(cols, ThreeRows()), null));
        }

        [Test]
        public void Build_WithFixedWidthColumn_ReturnsNonNull()
        {
            var cols = new[] { NameCol(), ValueCol(width: 80f) };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_WithNonSortableColumn_DoesNotThrow()
        {
            var cols = new[] { NameCol(sortable: false), TypeCol() };
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() => expander.Mount(new Table<Item>(cols, ThreeRows()), null));
        }

        [Test]
        public void Build_WithNonSortableColumn_ReturnsNonNull()
        {
            var cols = new[] { NameCol(sortable: false), TypeCol() };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        [Test]
        public void Mount_SingleColumn_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            Assert.DoesNotThrow(() =>
                expander.Mount(new Table<Item>(new[] { NameCol() }, ThreeRows()), null));
        }

        [Test]
        public void Build_SingleRow_ReturnsNonNull()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(
                new Table<Item>(TwoCols(), new[] { new Item("X", "Y", 1) }), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);
            Assert.That(built, Is.Not.Null);
        }

        // --- DidUpdateElement / Rebuild ---

        [Test]
        public void DidUpdateElement_ChangeRows_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(TwoCols(), ThreeRows()), null);
            expander.UpdateElement(node, new Table<Item>(TwoCols(), EmptyRows()));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_ChangeColumns_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(TwoCols(), ThreeRows()), null);
            expander.UpdateElement(node, new Table<Item>(ThreeCols(), ThreeRows()));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_AddOnRowSelected_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(TwoCols(), ThreeRows()), null);
            expander.UpdateElement(node, new Table<Item>(TwoCols(), ThreeRows(), onRowSelected: _ => { }));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        [Test]
        public void DidUpdateElement_ToggleStriped_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(TwoCols(), ThreeRows()), null);
            expander.UpdateElement(node, new Table<Item>(TwoCols(), ThreeRows(), stripedRows: false));
            Assert.DoesNotThrow(() => expander.Rebuild(node));
        }

        // --- Unmount ---

        [Test]
        public void Unmount_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(TwoCols(), ThreeRows()), null);
            Assert.DoesNotThrow(() => expander.Unmount(node));
        }

        [Test]
        public void Unmount_WithOnRowSelected_DoesNotThrow()
        {
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(
                new Table<Item>(TwoCols(), ThreeRows(), onRowSelected: _ => { }), null);
            Assert.DoesNotThrow(() => expander.Unmount(node));
        }
    }
}
