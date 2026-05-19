#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Arranges a sequence of items in a fixed-column grid.
    /// Rendered as a leaf element: the painter builds the native UIToolkit VE subtree
    /// directly, using explicit flex properties to guarantee equal-width columns.
    /// </summary>
    /// <since>2.0.0-beta.1</since>
    /// <status>live</status>
    public sealed class Grid<T> : LeafElement, IGridElement
    {
        /// <summary>Number of columns per row.</summary>
        public int ColumnCount { get; }

        /// <summary>The data items to render.</summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>Builds the element for a single item.</summary>
        public Func<T, Element> ItemBuilder { get; }

        /// <summary>Horizontal spacing between cells. Defaults to 0.</summary>
        public float ColumnSpacing { get; }

        /// <summary>Vertical spacing between rows. Defaults to 0.</summary>
        public float RowSpacing { get; }

        /// <summary>
        /// Creates a <see cref="Grid{T}"/> element.
        /// </summary>
        /// <param name="columnCount">Number of columns per row. Must be greater than zero.</param>
        /// <param name="items">The data source.</param>
        /// <param name="itemBuilder">Factory that turns one item into an <see cref="Element"/>.</param>
        /// <param name="columnSpacing">Gap between cells within a row.</param>
        /// <param name="rowSpacing">Gap between rows.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Grid(
            int columnCount,
            IReadOnlyList<T> items,
            Func<T, Element> itemBuilder,
            float columnSpacing = 0f,
            float rowSpacing = 0f,
            Key? key = null
        ) : base(key)
        {
            if (columnCount <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(columnCount),
                    "Must be greater than zero."
                );
            }

            ColumnCount = columnCount;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            ItemBuilder = itemBuilder ?? throw new ArgumentNullException(nameof(itemBuilder));
            ColumnSpacing = columnSpacing;
            RowSpacing = rowSpacing;
        }

        int IGridElement.ItemCount => Items.Count;

        Element IGridElement.BuildItemAt(int index) => ItemBuilder(Items[index]);
    }
}