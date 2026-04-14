#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Input
{
    /// <summary>
    /// A floating-point number input field. Maps to UIToolkit's <c>FloatField</c>.
    /// </summary>
    public sealed class FloatField : LeafElement
    {
        /// <summary>The current float value.</summary>
        public float Value { get; }

        /// <summary>
        /// An optional text label displayed alongside the field.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever the value changes.
        /// Receives the new float value as its argument.
        /// </summary>
        public Action<float>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FloatField"/> element.
        /// </summary>
        /// <param name="value">The current float value.</param>
        /// <param name="onChanged">Callback invoked on every value change.</param>
        /// <param name="label">Optional label text shown beside the field.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FloatField(
            float value = 0f,
            Action<float>? onChanged = null,
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