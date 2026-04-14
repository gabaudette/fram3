#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Input
{
    /// <summary>
    /// An integer number input field. Maps to UIToolkit's <c>IntegerField</c>.
    /// </summary>
    public sealed class IntField : LeafElement
    {
        /// <summary>The current integer value.</summary>
        public int Value { get; }

        /// <summary>
        /// An optional text label displayed alongside the field.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever the value changes.
        /// Receives the new integer value as its argument.
        /// </summary>
        public Action<int>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="IntField"/> element.
        /// </summary>
        /// <param name="value">The current integer value.</param>
        /// <param name="onChanged">Callback invoked on every value change.</param>
        /// <param name="label">Optional label text shown beside the field.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public IntField(
            int value = 0,
            Action<int>? onChanged = null,
            string? label = null,
            Key? key = null
        ) : base(key)
        {
            Value = value;
            OnChanged = onChanged;
            Label = label;
        }
    }
}