#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Input
{
    /// <summary>
    /// A boolean toggle (checkbox). Maps to a UIToolkit <c>Toggle</c>.
    /// </summary>
    public sealed class FrameToggle : LeafElement
    {
        /// <summary>The current checked state of the toggle.</summary>
        public bool Value { get; }

        /// <summary>
        /// An optional text label displayed alongside the toggle.
        /// Null means no label is shown.
        /// </summary>
        public string? Label { get; }

        /// <summary>
        /// Callback invoked whenever the toggle state changes.
        /// Receives the new boolean value as its argument.
        /// </summary>
        public Action<bool>? OnChanged { get; }

        /// <summary>
        /// Creates an <see cref="FrameToggle"/> element.
        /// </summary>
        /// <param name="value">The current checked state.</param>
        /// <param name="onChanged">Callback invoked on every state change.</param>
        /// <param name="label">Optional label text shown beside the toggle.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FrameToggle(
            bool value = false,
            Action<bool>? onChanged = null,
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