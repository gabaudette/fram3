#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A numeric range slider. Maps to a UIToolkit <c>Slider</c>.
    /// </summary>
    public sealed class FSlider : FLeafElement
    {
        /// <summary>The current value of the slider.</summary>
        public float Value { get; }

        /// <summary>The minimum value of the slider range.</summary>
        public float Min { get; }

        /// <summary>The maximum value of the slider range.</summary>
        public float Max { get; }

        /// <summary>
        /// An optional text label displayed alongside the slider.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever the slider value changes.
        /// Receives the new float value as its argument.
        /// </summary>
        public Action<float>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FSlider"/> element.
        /// </summary>
        /// <param name="value">The current slider value. Clamped to [min, max] by UIToolkit.</param>
        /// <param name="min">The minimum value of the range.</param>
        /// <param name="max">The maximum value of the range.</param>
        /// <param name="onChanged">Callback invoked on every value change.</param>
        /// <param name="label">Optional label text shown beside the slider.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FSlider(
            float value = 0f,
            float min = 0f,
            float max = 1f,
            Action<float>? onChanged = null,
            string? label = null,
            FKey? key = null
        ) : base(key)
        {
            Value = value;
            Min = min;
            Max = max;
            OnChanged = onChanged;
            Label = label;
        }
    }
}