#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Non-generic view of an <see cref="ListView{T}"/> used by the painter to access
    /// item data and builder logic without knowing the concrete item type.
    /// </summary>
    internal interface IListViewDescriptor
    {
        int ItemCount { get; }
        float ItemHeight { get; }
        ListSelectionMode SelectionMode { get; }
        Element BuildItemAt(int index);
        Action<IReadOnlyList<object?>>? OnSelectionChangedUntyped { get; }
    }

    /// <summary>
    /// A virtualized scrollable list that renders one item per data entry using a builder
    /// function. Maps to a UIToolkit <c>ListView</c>. Item elements are stateless -- no
    /// full reconciliation is performed per item; the painter is called directly in
    /// <c>makeItem</c> and <c>bindItem</c>.
    /// </summary>
    /// <typeparam name="T">The type of data item displayed in each row.</typeparam>
    public sealed class ListView<T> : LeafElement, IListViewDescriptor
    {
        /// <summary>The ordered list of data items to display.</summary>
        public IReadOnlyList<T> Items { get; }

        /// <summary>
        /// A function that maps a single data item to an <see cref="Element"/> describing
        /// its visual representation.
        /// </summary>
        public Func<T, Element> ItemBuilder { get; }

        /// <summary>The fixed height in pixels of each row. Defaults to 32.</summary>
        public float ItemHeight { get; }

        /// <summary>Controls how many items can be selected at once.</summary>
        public ListSelectionMode SelectionMode { get; }

        /// <summary>
        /// Callback invoked whenever the selection changes.
        /// Receives the currently selected items as its argument.
        /// </summary>
        public Action<IReadOnlyList<T>>? OnSelectionChanged { get; }

        /// <summary>
        /// Creates an <see cref="ListView{T}"/> element.
        /// </summary>
        /// <param name="items">The data items to display. Must not be null.</param>
        /// <param name="itemBuilder">
        /// A function that produces an <see cref="Element"/> for each item. Must not be null.
        /// </param>
        /// <param name="itemHeight">The fixed row height in pixels. Defaults to 32.</param>
        /// <param name="selectionMode">The selection behavior. Defaults to <see cref="ListSelectionMode.None"/>.</param>
        /// <param name="onSelectionChanged">Optional callback invoked when the selection changes.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="items"/> or <paramref name="itemBuilder"/> is null.
        /// </exception>
        public ListView(
            IReadOnlyList<T> items,
            Func<T, Element> itemBuilder,
            float itemHeight = 32f,
            ListSelectionMode selectionMode = ListSelectionMode.None,
            Action<IReadOnlyList<T>>? onSelectionChanged = null,
            Key? key = null
        ) : base(key)
        {
            Items = items ?? throw new ArgumentNullException(nameof(items));
            ItemBuilder = itemBuilder ?? throw new ArgumentNullException(nameof(itemBuilder));
            ItemHeight = itemHeight;
            SelectionMode = selectionMode;
            OnSelectionChanged = onSelectionChanged;
        }

        int IListViewDescriptor.ItemCount => Items.Count;

        Element IListViewDescriptor.BuildItemAt(int index) => ItemBuilder(Items[index]);

        Action<IReadOnlyList<object?>>? IListViewDescriptor.OnSelectionChangedUntyped =>
            OnSelectionChanged == null
                ? null
                : items =>
                {
                    var typed = new List<T>(items.Count);
                    foreach (var item in items)
                    {
                        if (item is T t)
                        {
                            typed.Add(t);
                        }
                    }

                    OnSelectionChanged(typed);
                };
    }
}