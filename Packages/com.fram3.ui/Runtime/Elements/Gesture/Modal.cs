#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Gesture
{
    /// <summary>
    /// An overlay element that displays its child on top of a full-screen backdrop.
    /// Visibility is controlled by the caller mounting or unmounting this element.
    /// Maps to a UIToolkit <c>VisualElement</c> with absolute positioning.
    /// </summary>
    public sealed class Modal : SingleChildElement
    {
        /// <summary>
        /// Callback invoked when the user requests dismissal of the modal,
        /// either by tapping the backdrop (when <see cref="BarrierDismissible"/> is true)
        /// or through any other dismiss gesture.
        /// </summary>
        public Action? OnDismiss { get; }

        /// <summary>
        /// When true, tapping the backdrop dismisses the modal by invoking
        /// <see cref="OnDismiss"/>. Defaults to true.
        /// </summary>
        public bool BarrierDismissible { get; }

        /// <summary>
        /// Creates an <see cref="Modal"/> element.
        /// </summary>
        /// <param name="child">The content element displayed inside the modal. Must not be null.</param>
        /// <param name="onDismiss">Callback invoked when dismissal is requested.</param>
        /// <param name="barrierDismissible">
        /// Whether tapping the backdrop triggers <paramref name="onDismiss"/>.
        /// Defaults to true.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="child"/> is null.</exception>
        public Modal(
            Element child,
            Action? onDismiss = null,
            bool barrierDismissible = true,
            Key? key = null
        ) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
            OnDismiss = onDismiss;
            BarrierDismissible = barrierDismissible;
        }
    }
}