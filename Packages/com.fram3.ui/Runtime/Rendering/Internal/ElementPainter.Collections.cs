#nullable enable
using System.Collections.Generic;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering.Internal
{
    internal static partial class ElementPainter
    {
        private sealed class GridState
        {
            public int ColumnCount;
            public float ColumnSpacing;
            public float RowSpacing;
            public int ItemCount;
        }

        private static VisualElement CreateGrid(IGridElement grid, Theme theme)
        {
            var container = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    alignSelf = Align.Stretch,
                    flexGrow = 1f,
                    flexShrink = 1f
                }
            };

            BuildNativeGridRows(grid, container, theme);
            container.userData = new GridState
            {
                ColumnCount = grid.ColumnCount,
                ColumnSpacing = grid.ColumnSpacing,
                RowSpacing = grid.RowSpacing,
                ItemCount = grid.ItemCount
            };

            return container;
        }

        private static void PatchNativeGrid(IGridElement grid, VisualElement container, Theme theme)
        {
            if (container.userData is not GridState state)
            {
                container.Clear();

                BuildNativeGridRows(grid, container, theme);

                container.userData = new GridState
                {
                    ColumnCount = grid.ColumnCount,
                    ColumnSpacing = grid.ColumnSpacing,
                    RowSpacing = grid.RowSpacing,
                    ItemCount = grid.ItemCount
                };

                return;
            }

            var structureChanged =
                grid.ColumnCount != state.ColumnCount ||
                !Mathf.Approximately(grid.ColumnSpacing, state.ColumnSpacing) ||
                !Mathf.Approximately(grid.RowSpacing, state.RowSpacing);

            if (structureChanged)
            {
                container.Clear();

                BuildNativeGridRows(grid, container, theme);

                state.ColumnCount = grid.ColumnCount;
                state.ColumnSpacing = grid.ColumnSpacing;
                state.RowSpacing = grid.RowSpacing;
                state.ItemCount = grid.ItemCount;

                return;
            }

            var oldItemCount = state.ItemCount;
            var newItemCount = grid.ItemCount;
            var columnCount = grid.ColumnCount;
            var hasRowSpacing = grid.RowSpacing > 0f;

            // Each row occupies one native child slot. When rowSpacing > 0, a spacer
            // precedes each row after the first, so each additional row costs 2 slots.
            // Slot layout (rowSpacing > 0): [row0, spacer, row1, spacer, row2, ...]
            // Slot layout (rowSpacing == 0): [row0, row1, row2, ...]
            var oldRowCount = oldItemCount == 0 ? 0 : (oldItemCount + columnCount - 1) / columnCount;
            var newRowCount = newItemCount == 0 ? 0 : (newItemCount + columnCount - 1) / columnCount;

            // Remove excess trailing rows (and their preceding spacers).
            while (oldRowCount > newRowCount)
            {
                var lastRow = container[container.childCount - 1];

                container.Remove(lastRow);

                if (hasRowSpacing && oldRowCount > 1)
                {
                    var spacer = container[container.childCount - 1];
                    container.Remove(spacer);
                }

                oldRowCount--;
            }

            // Repaint existing cells in-place.
            var repaintRowCount = System.Math.Min(oldRowCount, newRowCount);
            for (var repaintRow = 0; repaintRow < repaintRowCount; repaintRow++)
            {
                var rowSlot = hasRowSpacing ? repaintRow * 2 : repaintRow;
                var row = container[rowSlot];
                for (var cellSlot = 0; cellSlot < columnCount; cellSlot++)
                {
                    // With columnSpacing each column after the first is preceded by a spacer.
                    var actualCellSlot = grid.ColumnSpacing > 0f ? cellSlot * 2 : cellSlot;
                    if (actualCellSlot >= row.childCount)
                    {
                        break;
                    }

                    var cellVisualElement = row[actualCellSlot];
                    var index = repaintRow * columnCount + cellSlot;

                    cellVisualElement.Clear();

                    if (index < newItemCount)
                    {
                        BuildNativeTree(grid.BuildItemAt(index), cellVisualElement, theme);
                    }
                }
            }

            // Append new trailing rows.
            while (oldRowCount < newRowCount)
            {
                if (hasRowSpacing && oldRowCount > 0)
                {
                    container.Add(new VisualElement { style = { height = grid.RowSpacing } });
                }

                var row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        alignSelf = Align.Stretch,
                        alignItems = Align.Stretch
                    }
                };

                for (var j = 0; j < columnCount; j++)
                {
                    if (j > 0 && grid.ColumnSpacing > 0f)
                    {
                        row.Add(new VisualElement { style = { width = grid.ColumnSpacing } });
                    }

                    var cell = new VisualElement
                    {
                        style =
                        {
                            flexGrow = 1f,
                            flexShrink = 1f,
                            flexBasis = new StyleLength(new Length(0, LengthUnit.Percent)),
                            alignSelf = Align.Stretch
                        }
                    };

                    var index = oldRowCount * columnCount + j;
                    if (index < newItemCount)
                    {
                        BuildNativeTree(grid.BuildItemAt(index), cell, theme);
                    }

                    row.Add(cell);
                }

                container.Add(row);
                oldRowCount++;
            }

            state.ItemCount = newItemCount;
        }

        private static void BuildNativeGridRows(IGridElement grid, VisualElement container, Theme theme)
        {
            var columnCount = grid.ColumnCount;
            var itemCount = grid.ItemCount;
            var rowSpacing = grid.RowSpacing;
            var columnSpacing = grid.ColumnSpacing;
            var rowIndex = 0;

            for (var i = 0; i < itemCount; i += columnCount)
            {
                if (rowIndex > 0 && rowSpacing > 0f)
                {
                    var spacer = new VisualElement
                    {
                        style =
                        {
                            height = rowSpacing
                        }
                    };

                    container.Add(spacer);
                }

                var row = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        alignSelf = Align.Stretch,
                        alignItems = Align.Stretch
                    }
                };

                for (var j = 0; j < columnCount; j++)
                {
                    if (j > 0 && columnSpacing > 0f)
                    {
                        var spacer = new VisualElement
                        {
                            style =
                            {
                                width = columnSpacing
                            }
                        };

                        row.Add(spacer);
                    }

                    var cell = new VisualElement
                    {
                        style =
                        {
                            flexGrow = 1f,
                            flexShrink = 1f,
                            flexBasis = new StyleLength(new Length(0, LengthUnit.Percent)),
                            alignSelf = Align.Stretch
                        }
                    };

                    var index = i + j;
                    if (index < itemCount)
                    {
                        BuildNativeTree(
                            element: grid.BuildItemAt(index),
                            parent: cell,
                            theme
                        );
                    }

                    row.Add(cell);
                }

                container.Add(row);
                rowIndex++;
            }
        }

        private sealed class ListViewDescriptorHolder
        {
            public IListViewDescriptor? Descriptor;
            public int IndexListCount = -1;
        }

        private static ListView CreateListView(IListViewDescriptor listViewDescriptor, Theme theme)
        {
            var holder = new ListViewDescriptorHolder { Descriptor = listViewDescriptor };

            var listView = new ListView
            {
                fixedItemHeight = listViewDescriptor.ItemHeight,
                selectionType = MapSelectionType(listViewDescriptor.SelectionMode),
                makeItem = () =>
                {
                    var container = new VisualElement
                    {
                        style =
                        {
                            flexGrow = 1f,
                            alignSelf = Align.Stretch
                        }
                    };

                    return container;
                },
                bindItem = (item, index) =>
                {
                    item.style.backgroundColor = new Color(0f, 0f, 0f, 0f);
                    if (item.userData == null)
                    {
                        item.userData = new object();
                        
                        item.RegisterCallback<PointerEnterEvent>(_ =>
                            item.style.backgroundColor = ToUnity(theme.TrackColor)
                        );

                        item.RegisterCallback<PointerLeaveEvent>(_ =>
                            item.style.backgroundColor = new Color(0f, 0f, 0f, 0f)
                        );
                    }

                    item.Clear();

                    var childElement = holder.Descriptor!.BuildItemAt(index);

                    BuildNativeTree(childElement, item, theme);
                },
                userData = holder
            };
#if !FRAM3_PURE_TESTS
            listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
#endif

            listView.style.flexGrow = 1f;
            listView.style.flexShrink = 1f;

            listView.RegisterCallback<AttachToPanelEvent>(_ => listView.schedule.Execute(() =>
                    ApplyScrollbarTheme(listView, theme)).ExecuteLater(1)
            );

            if (listViewDescriptor.OnSelectionChangedUntyped == null)
            {
                return listView;
            }

            {
                var callback = listViewDescriptor.OnSelectionChangedUntyped;
                listView.selectionChanged += items =>
                {
                    var list = new List<object?>();
                    foreach (var item in items)
                    {
                        list.Add(item);
                    }

                    callback(list);
                };
            }

            return listView;
        }

        private static void PaintListView(IListViewDescriptor listViewDescriptor, ListView listView, Theme theme)
        {
            listView.fixedItemHeight = listViewDescriptor.ItemHeight;
            listView.selectionType = MapSelectionType(listViewDescriptor.SelectionMode);

            if (listView.userData is ListViewDescriptorHolder holder)
            {
                holder.Descriptor = listViewDescriptor;
#if !FRAM3_PURE_TESTS
                if (holder.IndexListCount != listViewDescriptor.ItemCount)
                {
                    holder.IndexListCount = listViewDescriptor.ItemCount;
                    listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
                }

                if (listView.panel != null)
                {
                    listView.schedule.Execute(() => ApplyScrollbarTheme(listView, theme)).ExecuteLater(1);
                }
#endif
            }
#if !FRAM3_PURE_TESTS
            else
            {
                listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
                if (listView.panel != null)
                {
                    listView.schedule.Execute(() => ApplyScrollbarTheme(listView, theme)).ExecuteLater(1);
                }
            }
#endif
        }

        private static List<int> BuildIndexList(int count)
        {
            var list = new List<int>(count);
            for (var i = 0; i < count; i++)
            {
                list.Add(i);
            }

            return list;
        }

        private static SelectionType MapSelectionType(ListSelectionMode mode)
        {
            return mode switch
            {
                ListSelectionMode.Single => SelectionType.Single,
                ListSelectionMode.Multiple => SelectionType.Multiple,
                _ => SelectionType.None
            };
        }
    }
}