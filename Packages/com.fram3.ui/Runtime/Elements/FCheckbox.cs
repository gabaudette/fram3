#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A checkbox control that represents a binary checked/unchecked state.
    /// Semantically distinct from <see cref="FToggle"/>, which is a general
    /// on/off switch. Maps to a UIToolkit <c>Toggle</c>.
    /// </summary>
    public sealed class FCheckbox : FLeafElement
    {
        /// <summary>The current checked state of the checkbox.</summary>
        public bool Value { get; }

        /// <summary>
        /// An optional text label displayed alongside the checkbox.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever the checked state changes.
        /// Receives the new boolean value as its argument.
        /// </summary>
        public Action<bool>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FCheckbox"/> element.
        /// </summary>
        /// <param name="value">The current checked state.</param>
        /// <param name="onChanged">Callback invoked on every state change.</param>
        /// <param name="label">Optional label text shown beside the checkbox.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FCheckbox(
            bool value = false,
            Action<bool>? onChanged = null,
            string? label = null,
            FKey? key = null
        ) : base(key)
        {
            Value = value;
            OnChanged = onChanged;
            Label = label;
        }
    }
}