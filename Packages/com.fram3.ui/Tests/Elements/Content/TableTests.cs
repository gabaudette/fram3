#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Core.Internal;
using Fram3.UI.Elements.Content;
using Fram3.UI.Rendering.Internal;
using NUnit.Framework;
using StylingTheme = Fram3.UI.Styling.Theme;
using UnityEngine.UIElements;

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

#if FRAM3_PURE_TESTS
        private static VisualElement BuildNative(Element element)
        {
            var theme = StylingTheme.Default;
            var native = ElementPainter.CreateNative(element, theme);
            ElementPainter.Paint(element, native, theme);
            foreach (var child in element.GetChildren())
            {
                native.Add(BuildNative(child));
            }
            return native;
        }

        private static VisualElement GetHeaderRowNative(Element built)
        {
            var native = BuildNative(built);
            // built = Column > [headerContainer, divider, ...dataRows]
            // headerContainer > Row > [cell0, cell1, cell2, ...]
            var headerContainer = native.Children()[0];
            var row = headerContainer.Children()[0];
            return row;
        }

        private static VisualElement GetDataRowNative(Element built, int rowIndex = 0)
        {
            var native = BuildNative(built);
            // built = Column > [headerContainer, divider, dataRow0, dataRow1, ...]
            var dataRowWrapper = native.Children()[2 + rowIndex];
            // dataRowWrapper is Container (no onRowSelected) or GestureDetector > Container
            // either way first child is the Row
            var possibleRow = dataRowWrapper.Children()[0];
            if (possibleRow.childCount > 0 && possibleRow.Children()[0].childCount == 0)
            {
                // dataRowWrapper was a GestureDetector; its child is the Container, whose child is the Row
                return possibleRow.Children()[0];
            }
            return possibleRow;
        }

        private static string DumpNative(VisualElement n, int depth = 0)
        {
            var indent = new string(' ', depth * 2);
            var s = $"{indent}[{n.GetType().Name}] w={n.style.width} fg={n.style.flexGrow} fs={n.style.flexShrink} children={n.childCount}\n";
            foreach (var c in n.Children())
                s += DumpNative(c, depth + 1);
            return s;
        }

        [Test]
        public void HeaderRow_FixedWidthColumn_HasCorrectWidth()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var row = GetHeaderRowNative(built);
            var cell = row.Children()[1];

            Assert.That(cell.style.width, Is.EqualTo(80f).Within(0.001f));
        }

        [Test]
        public void DataRow_FixedWidthColumn_HasCorrectWidth()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var row = GetDataRowNative(built);
            var cell = row.Children()[1];

            Assert.That(cell.style.width, Is.EqualTo(80f).Within(0.001f));
        }

        [Test]
        public void HeaderRow_FixedWidthColumn_FlexShrinkIsZero()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var row = GetHeaderRowNative(built);
            var cell = row.Children()[1];

            Assert.That(cell.style.flexShrink, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void DataRow_FixedWidthColumn_FlexShrinkIsZero()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var row = GetDataRowNative(built);
            var cell = row.Children()[1];

            Assert.That(cell.style.flexShrink, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void HeaderRow_ExpandedColumn_FlexGrowIsOne()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var row = GetHeaderRowNative(built);
            var cell = row.Children()[0];

            Assert.That(cell.style.flexGrow, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void DataRow_ExpandedColumn_FlexGrowIsOne()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var row = GetDataRowNative(built);
            var cell = row.Children()[0];

            Assert.That(cell.style.flexGrow, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void HeaderAndDataRow_FixedWidthColumn_HaveSameWidth()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var headerRow = GetHeaderRowNative(built);
            var dataRow = GetDataRowNative(built);

            Assert.That(headerRow.Children()[1].style.width,
                Is.EqualTo(dataRow.Children()[1].style.width).Within(0.001f));
        }

        [Test]
        public void HeaderAndDataRow_ExpandedColumn_HaveSameFlexGrow()
        {
            var cols = new[]
            {
                new TableColumn<Item>("Name", r => r.Name),
                new TableColumn<Item>("Value", r => r.Value.ToString(), width: 80f),
                new TableColumn<Item>("Type", r => r.Type)
            };
            var expander = new NodeExpander(new RebuildScheduler());
            var node = expander.Mount(new Table<Item>(cols, ThreeRows()), null);
            var built = ((State<Table<Item>>)node.State!).Build(node.Context);

            var headerRow = GetHeaderRowNative(built);
            var dataRow = GetDataRowNative(built);

            Assert.That(headerRow.Children()[0].style.flexGrow,
                Is.EqualTo(dataRow.Children()[0].style.flexGrow).Within(0.001f));
        }
#endif
    }
}
