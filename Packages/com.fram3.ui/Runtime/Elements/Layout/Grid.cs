#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Arranges a sequence of items in a fixed-column grid.
    /// Items are distributed into rows of <see cref="ColumnCount"/> cells each.
    /// Each row is a <see cref="Row"/>; the rows are stacked in a <see cref="Column"/>.
    /// </summary>
    public sealed class Grid<T> : StatelessElement
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
            if (columnCount <= 0) throw new ArgumentOutOfRangeException(nameof(columnCount), "Must be greater than zero.");
            ColumnCount = columnCount;
            Items = items ?? throw new ArgumentNullException(nameof(items));
            ItemBuilder = itemBuilder ?? throw new ArgumentNullException(nameof(itemBuilder));
            ColumnSpacing = columnSpacing;
            RowSpacing = rowSpacing;
        }

        /// <inheritdoc/>
        public override Element Build(BuildContext context)
        {
            var rows = new List<Element>();

            for (var i = 0; i < Items.Count; i += ColumnCount)
            {
                if (rows.Count > 0 && RowSpacing > 0f)
                    rows.Add(SizedBox.FromSize(height: RowSpacing));

                var cells = new List<Element>();
                for (var j = 0; j < ColumnCount; j++)
                {
                    if (cells.Count > 0 && ColumnSpacing > 0f)
                        cells.Add(SizedBox.FromSize(width: ColumnSpacing));

                    var index = i + j;
                    if (index < Items.Count)
                        cells.Add(new Expanded { Child = ItemBuilder(Items[index]) });
                    else
                        cells.Add(new Expanded());
                }

                rows.Add(new Row(crossAxisAlignment: CrossAxisAlignment.Start)
                {
                    Children = cells.ToArray()
                });
            }

            return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
            {
                Children = rows.ToArray()
            };
        }
    }
}
