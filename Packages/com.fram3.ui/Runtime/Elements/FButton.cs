#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A tappable button with a text label. Maps to a UIToolkit <c>Button</c>.
    /// </summary>
    public sealed class FButton : FLeafElement
    {
        /// <summary>The text label displayed on the button.</summary>
        public string Label { get; }

        /// <summary>
        /// The callback invoked when the button is pressed.
        /// Null means the button is present but performs no action.
        /// </summary>
        public Action? OnPressed { get; }

        /// <summary>
        /// Creates an <see cref="FButton"/> element.
        /// </summary>
        /// <param name="label">The text label to display.</param>
        /// <param name="onPressed">The callback invoked on press. Null means no action.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FButton(string? label, Action? onPressed = null, FKey? key = null) : base(key)
        {
            Label = label ?? string.Empty;
            OnPressed = onPressed;
        }
    }
}