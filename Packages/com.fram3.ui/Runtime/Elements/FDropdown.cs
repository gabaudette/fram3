#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A dropdown selection control. Maps to a UIToolkit <c>DropdownField</c>.
    /// </summary>
    public sealed class FDropdown : FLeafElement
    {
        /// <summary>The list of string options shown in the dropdown.</summary>
        public IReadOnlyList<string> Options { get; }

        /// <summary>
        /// The index of the currently selected option.
        /// -1 means nothing is selected.
        /// </summary>
        public int SelectedIndex { get; }

        /// <summary>
        /// An optional text label displayed alongside the dropdown.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever the selection changes.
        /// Receives the new zero-based index as its argument.
        /// </summary>
        public Action<int>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FDropdown"/> element.
        /// </summary>
        /// <param name="options">The list of options to display. Must not be null.</param>
        /// <param name="selectedIndex">The index of the currently selected option. -1 means no selection.</param>
        /// <param name="onChanged">Callback invoked on every selection change.</param>
        /// <param name="label">Optional label text shown beside the dropdown.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        public FDropdown(
            IReadOnlyList<string> options,
            int selectedIndex = -1,
            Action<int>? onChanged = null,
            string? label = null,
            FKey? key = null) : base(key)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            SelectedIndex = selectedIndex;
            OnChanged = onChanged;
            Label = label;
        }
    }
}
