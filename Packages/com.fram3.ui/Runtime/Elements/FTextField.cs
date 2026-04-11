#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A single-line or multi-line text input field. Maps to a UIToolkit <c>TextField</c>.
    /// </summary>
    public sealed class FTextField : FLeafElement
    {
        /// <summary>The current text value displayed in the field.</summary>
        public string Value { get; }

        /// <summary>
        /// Placeholder text shown when the field is empty.
        /// Null means no placeholder is displayed.
        /// </summary>
        public string? Placeholder { get; }

        /// <summary>
        /// Whether the field is read-only. When true the user cannot edit the value.
        /// </summary>
        public bool ReadOnly { get; }

        /// <summary>
        /// Whether the field accepts multiple lines of text.
        /// </summary>
        public bool Multiline { get; }

        /// <summary>
        /// Callback invoked whenever the text value changes.
        /// Receives the new string value as its argument.
        /// </summary>
        public Action<string>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FTextField"/> element.
        /// </summary>
        /// <param name="value">The current text value.</param>
        /// <param name="onChanged">Callback invoked on every value change.</param>
        /// <param name="placeholder">Optional placeholder text shown when the field is empty.</param>
        /// <param name="readOnly">When true the field is not editable.</param>
        /// <param name="multiline">When true the field accepts multiple lines.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FTextField(
            string? value = null,
            Action<string>? onChanged = null,
            string? placeholder = null,
            bool readOnly = false,
            bool multiline = false,
            FKey? key = null
        ) : base(key)
        {
            Value = value ?? string.Empty;
            OnChanged = onChanged;
            Placeholder = placeholder;
            ReadOnly = readOnly;
            Multiline = multiline;
        }
    }
}