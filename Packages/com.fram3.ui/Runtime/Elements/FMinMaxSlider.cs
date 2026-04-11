#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A dual-handle range slider for selecting a minimum and maximum value within a fixed range.
    /// Maps to UIToolkit's <c>MinMaxSlider</c>.
    /// </summary>
    public sealed class FMinMaxSlider : FLeafElement
    {
        /// <summary>The current low (minimum) handle value.</summary>
        public float MinValue { get; }

        /// <summary>The current high (maximum) handle value.</summary>
        public float MaxValue { get; }

        /// <summary>The minimum allowed value for the low handle.</summary>
        public float LowLimit { get; }

        /// <summary>The maximum allowed value for the high handle.</summary>
        public float HighLimit { get; }

        /// <summary>
        /// An optional text label displayed alongside the slider.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever either handle changes.
        /// Receives the new low and high values as arguments.
        /// </summary>
        public Action<float, float>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FMinMaxSlider"/> element.
        /// </summary>
        /// <param name="minValue">The current low handle value.</param>
        /// <param name="maxValue">The current high handle value.</param>
        /// <param name="lowLimit">The minimum allowed value for the low handle.</param>
        /// <param name="highLimit">The maximum allowed value for the high handle.</param>
        /// <param name="onChanged">Callback invoked when either handle changes.</param>
        /// <param name="label">Optional label text shown beside the slider.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FMinMaxSlider(
            float minValue = 0f,
            float maxValue = 1f,
            float lowLimit = 0f,
            float highLimit = 1f,
            Action<float, float>? onChanged = null,
            string? label = null,
            FKey? key = null
        ) : base(key)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            LowLimit = lowLimit;
            HighLimit = highLimit;
            OnChanged = onChanged;
            Label = label;
        }
    }
}