#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Displays a read-only progress bar. Maps to UIToolkit's native
    /// <c>ProgressBar</c> element.
    /// </summary>
    public sealed class FProgressBar : FLeafElement
    {
        /// <summary>
        /// The current progress value. Should be within [<see cref="Min"/>, <see cref="Max"/>].
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// The minimum value of the range. Defaults to 0.
        /// </summary>
        public float Min { get; }

        /// <summary>
        /// The maximum value of the range. Defaults to 100.
        /// </summary>
        public float Max { get; }

        /// <summary>
        /// An optional text label displayed inside the progress bar.
        /// Null means no label is shown.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        /// Creates an <see cref="FProgressBar"/> element.
        /// </summary>
        /// <param name="value">The current progress value.</param>
        /// <param name="min">The minimum value. Defaults to 0.</param>
        /// <param name="max">The maximum value. Defaults to 100.</param>
        /// <param name="title">An optional label shown inside the bar.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FProgressBar(
            float value,
            float min = 0f,
            float max = 100f,
            string? title = null,
            FKey? key = null
        ) : base(key)
        {
            Value = value;
            Min = min;
            Max = max;
            Title = title;
        }
    }
}