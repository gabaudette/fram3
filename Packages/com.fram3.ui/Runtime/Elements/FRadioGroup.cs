#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A group of mutually exclusive radio button options.
    /// Only one option can be selected at a time.
    /// Maps to a UIToolkit <c>RadioButtonGroup</c>.
    /// </summary>
    public sealed class FRadioGroup : FLeafElement
    {
        /// <summary>The list of option labels displayed as radio buttons.</summary>
        public IReadOnlyList<string> Options { get; }

        /// <summary>
        /// The currently selected option value, or null when nothing is selected.
        /// </summary>
        public string? SelectedValue { get; }

        /// <summary>
        /// Callback invoked whenever the selected option changes.
        /// Receives the newly selected option value as its argument.
        /// </summary>
        public Action<string>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FRadioGroup"/> element.
        /// </summary>
        /// <param name="options">The list of option labels. Must not be null.</param>
        /// <param name="selectedValue">The currently selected option, or null for no selection.</param>
        /// <param name="onChanged">Callback invoked when the selection changes.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
        public FRadioGroup(
            IReadOnlyList<string> options,
            string? selectedValue = null,
            Action<string>? onChanged = null,
            FKey? key = null
        ) : base(key)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            SelectedValue = selectedValue;
            OnChanged = onChanged;
        }
    }
}