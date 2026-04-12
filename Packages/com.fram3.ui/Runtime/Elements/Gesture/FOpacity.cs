#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Gesture
{
    /// <summary>
    /// Applies a uniform opacity to its child element.
    /// Maps to UIToolkit's <c>opacity</c> style property on a <c>VisualElement</c>.
    /// </summary>
    public sealed class FOpacity : FSingleChildElement
    {
        /// <summary>
        /// The opacity value in the range [0, 1].
        /// 0 is fully transparent, 1 is fully opaque.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Creates an <see cref="FOpacity"/> element.
        /// </summary>
        /// <param name="value">Opacity in the range [0, 1]. Clamped to this range.</param>
        /// <param name="child">The child element to apply opacity to. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="child"/> is null.</exception>
        public FOpacity(
            float value,
            FElement child,
            FKey? key = null
        ) : base(key)
        {
            Value = System.Math.Clamp(value, 0f, 1f);
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }
    }
}