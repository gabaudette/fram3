#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Gesture;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Defines a single column in a <see cref="Table{TRow}"/>.
    /// </summary>
    /// <typeparam name="TRow">The row data type.</typeparam>
    public sealed class TableColumn<TRow>
    {
        /// <summary>The label displayed in the column header.</summary>
        public string Header { get; }

        /// <summary>
        /// Extracts the display string for a given row value.
        /// Used to render cell text and to compare values when sorting.
        /// </summary>
        public Func<TRow, string> CellText { get; }

        /// <summary>
        /// Optional fixed width in logical pixels. Null means the column shares
        /// available width equally with other unsized columns.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// Whether this column supports sorting. Defaults to true.
        /// </summary>
        public bool Sortable { get; }

        /// <summary>
        /// Creates a <see cref="TableColumn{TRow}"/>.
        /// </summary>
        /// <param name="header">The column header label. Must not be null.</param>
        /// <param name="cellText">A function that extracts display text from a row. Must not be null.</param>
        /// <param name="width">Optional fixed width in logical pixels.</param>
        /// <param name="sortable">Whether the column header can be clicked to sort. Defaults to true.</param>
        public TableColumn(
            string header,
            Func<TRow, string> cellText,
            float? width = null,
            bool sortable = true
        )
        {
            Header = header ?? throw new ArgumentNullException(nameof(header));
            CellText = cellText ?? throw new ArgumentNullException(nameof(cellText));
            Width = width;
            Sortable = sortable;
        }
    }

    /// <summary>
    /// A columnar data table with optional row selection and column sorting.
    /// Columns are defined via <see cref="TableColumn{TRow}"/>. Rows are provided
    /// as an <see cref="IReadOnlyList{T}"/>. Clicking a sortable column header
    /// cycles ascending/descending sort on that column. Clicking a row fires
    /// <see cref="OnRowSelected"/> with the original row value.
    /// </summary>
    /// <typeparam name="TRow">The row data type.</typeparam>
    public sealed class Table<TRow> : StatefulElement
    {
        /// <summary>The column definitions. Must not be null or empty.</summary>
        public IReadOnlyList<TableColumn<TRow>> Columns { get; }

        /// <summary>The data rows. Must not be null.</summary>
        public IReadOnlyList<TRow> Rows { get; }

        /// <summary>
        /// Called when the user taps a row. Receives the original row value.
        /// Null means row taps are not interactive.
        /// </summary>
        public Action<TRow>? OnRowSelected { get; }

        /// <summary>
        /// Whether alternating row background shading is applied. Defaults to true.
        /// </summary>
        public bool StripedRows { get; }

        /// <summary>
        /// Creates a <see cref="Table{TRow}"/>.
        /// </summary>
        /// <param name="columns">Column definitions. Must not be null or empty.</param>
        /// <param name="rows">Data rows. Must not be null.</param>
        /// <param name="onRowSelected">Optional callback invoked when a row is tapped.</param>
        /// <param name="stripedRows">Whether alternating rows are shaded. Defaults to true.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="columns"/> or <paramref name="rows"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="columns"/> is empty.</exception>
        public Table(
            IReadOnlyList<TableColumn<TRow>> columns,
            IReadOnlyList<TRow> rows,
            Action<TRow>? onRowSelected = null,
            bool stripedRows = true,
            Key? key = null
        ) : base(key)
        {
            if (columns == null) throw new ArgumentNullException(nameof(columns));
            if (columns.Count == 0) throw new ArgumentException("Columns must not be empty.", nameof(columns));
            Columns = columns;
            Rows = rows ?? throw new ArgumentNullException(nameof(rows));
            OnRowSelected = onRowSelected;
            StripedRows = stripedRows;
        }

        /// <inheritdoc/>
        public override Core.State CreateState() => new TableState();

        private sealed class TableState : State<Table<TRow>>
        {
            private int _sortColumnIndex = -1;
            private bool _sortAscending = true;
            private int _selectedRowIndex = -1;

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var columns = Element!.Columns;
                var rows = BuildSortedRows();

                var tableRows = new List<Element>();

                // Header row.
                tableRows.Add(BuildHeaderRow(columns, theme));

                // Divider beneath header.
                tableRows.Add(new Divider(
                    axis: DividerAxis.Horizontal,
                    thickness: 1f,
                    color: theme.InputBorderColor
                ));

                // Data rows.
                for (var i = 0; i < rows.Count; i++)
                {
                    tableRows.Add(BuildDataRow(rows[i].Original, rows[i].SortedIndex, i, columns, theme));
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = tableRows.ToArray()
                };
            }

            private List<(TRow Original, int SortedIndex)> BuildSortedRows()
            {
                var rows = Element!.Rows;
                var result = new List<(TRow, int)>(rows.Count);
                for (var i = 0; i < rows.Count; i++)
                {
                    result.Add((rows[i], i));
                }

                if (_sortColumnIndex < 0 || _sortColumnIndex >= Element.Columns.Count)
                {
                    return result;
                }

                var col = Element.Columns[_sortColumnIndex];
                var ascending = _sortAscending;
                result.Sort((a, b) =>
                {
                    var cmp = string.Compare(col.CellText(a.Item1), col.CellText(b.Item1),
                        StringComparison.OrdinalIgnoreCase);
                    return ascending ? cmp : -cmp;
                });

                return result;
            }

            private Element BuildHeaderRow(IReadOnlyList<TableColumn<TRow>> columns, Styling.Theme theme)
            {
                var cells = new List<Element>();
                var cellPadding = EdgeInsets.Symmetric(
                    vertical: theme.Spacing * 0.75f,
                    horizontal: theme.Spacing
                );

                for (var i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];
                    var colIndex = i;

                    var sortIndicator = "";
                    if (_sortColumnIndex == colIndex)
                    {
                        sortIndicator = _sortAscending ? " ^" : " v";
                    }

                    var label = new Text(
                        col.Header + sortIndicator,
                        new TextStyle(
                            FontSize: theme.FontSize,
                            Color: col.Sortable ? theme.PrimaryColor : theme.PrimaryTextColor,
                            Bold: true
                        )
                    );

                    if (col.Width.HasValue)
                    {
                        Element cellContent = col.Sortable
                            ? new GestureDetector(
                                child: label,
                                onTap: () => SetState(() =>
                                {
                                    if (_sortColumnIndex == colIndex)
                                        _sortAscending = !_sortAscending;
                                    else
                                    {
                                        _sortColumnIndex = colIndex;
                                        _sortAscending = true;
                                    }
                                })
                            )
                            : label;
                        cells.Add(new Container(padding: cellPadding, width: col.Width) { Child = cellContent });
                    }
                    else
                    {
                        if (col.Sortable)
                        {
                            cells.Add(new Expanded(padding: cellPadding)
                            {
                                Child = new GestureDetector(
                                    child: label,
                                    onTap: () => SetState(() =>
                                    {
                                        if (_sortColumnIndex == colIndex)
                                            _sortAscending = !_sortAscending;
                                        else
                                        {
                                            _sortColumnIndex = colIndex;
                                            _sortAscending = true;
                                        }
                                    })
                                )
                            });
                        }
                        else
                        {
                            cells.Add(new Expanded(padding: cellPadding) { Child = label });
                        }
                    }
                }

                return new Container(
                    decoration: new BoxDecoration(Color: theme.SurfaceColor)
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = cells.ToArray()
                    }
                };
            }

            private Element BuildDataRow(
                TRow row,
                int sortedIndex,
                int displayIndex,
                IReadOnlyList<TableColumn<TRow>> columns,
                Styling.Theme theme
            )
            {
                var isSelected = sortedIndex == _selectedRowIndex;
                var isStripe = Element!.StripedRows && displayIndex % 2 == 1;

                FrameColor bg;
                if (isSelected)
                {
                    bg = theme.PrimaryColor.WithAlpha(0.18f);
                }
                else if (isStripe)
                {
                    bg = theme.TrackColor.WithAlpha(0.4f);
                }
                else
                {
                    bg = theme.SurfaceColor.WithAlpha(0f);
                }

                var cellPadding = EdgeInsets.Symmetric(
                    vertical: theme.Spacing * 0.75f,
                    horizontal: theme.Spacing
                );

                var cells = new List<Element>();
                for (var i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];
                    var label = new Text(
                        col.CellText(row),
                        new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)
                    );

                    if (col.Width.HasValue)
                    {
                        cells.Add(new Container(padding: cellPadding, width: col.Width) { Child = label });
                    }
                    else
                    {
                        cells.Add(new Expanded(padding: cellPadding) { Child = label });
                    }
                }

                var rowContent = new Container(
                    decoration: new BoxDecoration(Color: bg)
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = cells.ToArray()
                    }
                };

                if (Element.OnRowSelected == null)
                {
                    return rowContent;
                }

                var capturedRow = row;
                var capturedSortedIndex = sortedIndex;
                return new GestureDetector(
                    child: rowContent,
                    onTap: () => SetState(() =>
                    {
                        _selectedRowIndex = _selectedRowIndex == capturedSortedIndex
                            ? -1
                            : capturedSortedIndex;
                        Element.OnRowSelected?.Invoke(capturedRow);
                    })
                );
            }
        }
    }
}
