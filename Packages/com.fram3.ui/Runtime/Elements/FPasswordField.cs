#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A single-line password input field. Identical in API to <see cref="FTextField"/>
    /// but masks the entered characters. Maps to a UIToolkit <c>TextField</c> with
    /// <c>isPasswordField = true</c>.
    /// </summary>
    public sealed class FPasswordField : FLeafElement
    {
        /// <summary>The current text value of the password field.</summary>
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
        /// Callback invoked whenever the text value changes.
        /// Receives the new string value as its argument.
        /// </summary>
        public Action<string>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FPasswordField"/> element.
        /// </summary>
        /// <param name="value">The current text value.</param>
        /// <param name="onChanged">Callback invoked on every value change.</param>
        /// <param name="placeholder">Optional placeholder text shown when the field is empty.</param>
        /// <param name="readOnly">When true the field is not editable.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FPasswordField(
            string? value = null,
            Action<string>? onChanged = null,
            string? placeholder = null,
            bool readOnly = false,
            FKey? key = null
        ) : base(key)
        {
            Value = value ?? string.Empty;
            OnChanged = onChanged;
            Placeholder = placeholder;
            ReadOnly = readOnly;
        }
    }
}
