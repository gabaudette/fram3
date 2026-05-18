#nullable enable
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Elements.Content;
using Fram3.UI.Elements.Layout;
using Fram3.UI.Styling;
using UnityEngine.UIElements;

namespace Fram3.UI.Rendering.Internal
{
    internal static partial class ElementPainter
    {
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

            return container;
        }

        private static void RebuildNativeGrid(IGridElement grid, VisualElement container, Theme theme)
        {
            container.Clear();
            BuildNativeGridRows(grid, container, theme);
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
                        BuildNativeTree(grid.BuildItemAt(index), cell, theme);
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
                    item.style.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
                    if (item.userData == null)
                    {
                        item.userData = new object();
                        item.RegisterCallback<PointerEnterEvent>(_ =>
                            item.style.backgroundColor = ToUnity(theme.TrackColor)
                        );

                        item.RegisterCallback<PointerLeaveEvent>(_ =>
                            item.style.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f)
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

            listView.RegisterCallback<AttachToPanelEvent>(_ =>
                listView.schedule.Execute(() => ApplyScrollbarTheme(listView, theme)).ExecuteLater(1));

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

        private static void PaintListView(IListViewDescriptor listViewDescriptor, ListView listView)
        {
            listView.fixedItemHeight = listViewDescriptor.ItemHeight;
            listView.selectionType = MapSelectionType(listViewDescriptor.SelectionMode);
            if (listView.userData is ListViewDescriptorHolder holder)
            {
                holder.Descriptor = listViewDescriptor;
#if !FRAM3_PURE_TESTS
                if (holder.IndexListCount == listViewDescriptor.ItemCount)
                {
                    return;
                }

                holder.IndexListCount = listViewDescriptor.ItemCount;
                listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
#endif
            }
#if !FRAM3_PURE_TESTS
            else
            {
                listView.itemsSource = BuildIndexList(listViewDescriptor.ItemCount);
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
