#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Elements.Theme;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Defines a single column in a <see cref="Table{TRow}"/>.
    /// </summary>
    /// <status>live</status>
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
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (columns.Count == 0)
            {
                throw new ArgumentException("Columns must not be empty.", nameof(columns));
            }

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
            private float _resolvedWidth = -1f;

            public override Element Build(BuildContext context)
            {
                var theme = ThemeConsumer.Of(context);
                var columns = Element!.Columns;

                // Width not yet measured — render probe only.
                if (_resolvedWidth <= 0f)
                {
                    return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                    {
                        Children = new Element[]
                        {
                            new WidthProbe(w => SetState(() => _resolvedWidth = w))
                        }
                    };
                }

                var colWidths = ComputeColumnWidths(columns, _resolvedWidth);
                var rows = BuildSortedRows();
                var tableRows = new List<Element>
                {
                    BuildHeaderRow(columns, colWidths, theme),
                    new Divider(
                        axis: DividerAxis.Horizontal,
                        thickness: 1f,
                        color: theme.InputBorderColor
                    )
                };

                for (var i = 0; i < rows.Count; i++)
                {
                    tableRows.Add(
                        BuildDataRow(
                            row: rows[i].Original,
                            sortedIndex: rows[i].SortedIndex,
                            displayIndex: i,
                            columns: columns,
                            colWidths: colWidths,
                            theme
                        )
                    );
                }

                return new Column(crossAxisAlignment: CrossAxisAlignment.Stretch)
                {
                    Children = tableRows.ToArray()
                };
            }

            private static float[] ComputeColumnWidths(IReadOnlyList<TableColumn<TRow>> columns, float totalWidth)
            {
                var widths = new float[columns.Count];
                var fixedTotal = 0f;
                var expandedCount = 0;

                foreach (var column in columns)
                {
                    if (column.Width.HasValue)
                    {
                        fixedTotal += column.Width!.Value;
                    }
                    else
                    {
                        expandedCount++;
                    }
                }

                var expandedWidth = expandedCount > 0
                    ? MathF.Floor((totalWidth - fixedTotal) / expandedCount)
                    : 0f;

                for (var i = 0; i < columns.Count; i++)
                {
                    widths[i] = columns[i].Width ?? expandedWidth;
                }

                return widths;
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
                    var compare = string.Compare(
                        col.CellText(a.Item1),
                        col.CellText(b.Item1),
                        StringComparison.OrdinalIgnoreCase
                    );

                    return ascending ? compare : -compare;
                });

                return result;
            }

            private Element BuildHeaderRow(
                IReadOnlyList<TableColumn<TRow>> columns,
                float[] colWidths,
                Styling.Theme theme
            )
            {
                var cells = new List<Element>();
                var cellPadding = EdgeInsets.Symmetric(
                    vertical: theme.Spacing * 0.75f,
                    horizontal: theme.Spacing
                );

                for (var i = 0; i < columns.Count; i++)
                {
                    var col = columns[i];

                    var sortIndicator = "";
                    if (_sortColumnIndex == i)
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

                    Action? onTap = null;
                    if (col.Sortable)
                    {
                        var columnIndex = i;
                        onTap = () => SetState(() =>
                        {
                            if (_sortColumnIndex == columnIndex)
                            {
                                _sortAscending = !_sortAscending;
                            }
                            else
                            {
                                _sortColumnIndex = columnIndex;
                                _sortAscending = true;
                            }
                        });
                    }

                    cells.Add(
                        new Container(
                            padding: cellPadding,
                            width: colWidths[i],
                            onTap: onTap
                        )
                        {
                            Child = label
                        }
                    );
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
                float[] colWidths,
                Styling.Theme theme
            )
            {
                var isSelected = sortedIndex == _selectedRowIndex;
                var isStripe = Element!.StripedRows && displayIndex % 2 == 1;

                FrameColor backGround;
                if (isSelected)
                {
                    backGround = theme.PrimaryColor.WithAlpha(0.18f);
                }
                else if (isStripe)
                {
                    backGround = theme.TrackColor.WithAlpha(0.4f);
                }
                else
                {
                    backGround = theme.SurfaceColor.WithAlpha(0f);
                }

                var cellPadding = EdgeInsets.Symmetric(
                    vertical: theme.Spacing * 0.75f,
                    horizontal: theme.Spacing
                );

                var cells = new List<Element>();
                for (var i = 0; i < columns.Count; i++)
                {
                    var label = new Text(
                        columns[i].CellText(row),
                        new TextStyle(FontSize: theme.FontSize, Color: theme.PrimaryTextColor)
                    );

                    cells.Add(new Container(padding: cellPadding, width: colWidths[i]) { Child = label });
                }

                Action? onTap = null;
                if (Element.OnRowSelected == null)
                {
                    return new Container(
                        decoration: new BoxDecoration(
                            Color: backGround
                        ),
                        onTap: onTap
                    )
                    {
                        Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                        {
                            Children = cells.ToArray()
                        }
                    };
                }

                var capturedRow = row;
                var capturedSortedIndex = sortedIndex;
                onTap = () => SetState(() =>
                {
                    _selectedRowIndex = _selectedRowIndex == capturedSortedIndex
                        ? -1
                        : capturedSortedIndex;

                    Element.OnRowSelected?.Invoke(capturedRow);
                });

                return new Container(
                    decoration: new BoxDecoration(
                        Color: backGround
                    ),
                    onTap: onTap
                )
                {
                    Child = new Row(crossAxisAlignment: CrossAxisAlignment.Center)
                    {
                        Children = cells.ToArray()
                    }
                };
            }
        }
    }
}