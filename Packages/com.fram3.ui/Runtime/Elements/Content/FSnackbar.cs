#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A brief notification bar that appears temporarily at the bottom of the screen.
    /// Supports an optional action button that the user can tap.
    /// Maps to a plain UIToolkit <c>VisualElement</c> styled as a snackbar bar.
    /// </summary>
    public sealed class FSnackbar : FLeafElement
    {
        /// <summary>The message text displayed in the snackbar.</summary>
        public string Message { get; }

        /// <summary>
        /// The label for the optional action button.
        /// Null means no action button is shown.
        /// </summary>
        public string? ActionLabel { get; }

        /// <summary>
        /// Callback invoked when the user taps the action button.
        /// Only relevant when <see cref="ActionLabel"/> is non-null.
        /// </summary>
        public Action? OnAction { get; }

        /// <summary>
        /// How long the snackbar remains visible, in seconds.
        /// Defaults to 4 seconds.
        /// </summary>
        public float Duration { get; }

        /// <summary>
        /// Creates an <see cref="FSnackbar"/> element.
        /// </summary>
        /// <param name="message">The notification text. Must not be null.</param>
        /// <param name="actionLabel">Label for the optional action button.</param>
        /// <param name="onAction">Callback invoked when the action button is tapped.</param>
        /// <param name="duration">Visible duration in seconds. Defaults to 4.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message"/> is null.</exception>
        public FSnackbar(
            string message,
            string? actionLabel = null,
            Action? onAction = null,
            float duration = 4f,
            FKey? key = null
        ) : base(key)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            ActionLabel = actionLabel;
            OnAction = onAction;
            Duration = duration;
        }
    }
}