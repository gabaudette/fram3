#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A transparent wrapper that detects pointer gestures on its child subtree.
    /// Maps to a plain UIToolkit <c>VisualElement</c> with registered pointer event callbacks.
    /// </summary>
    public sealed class FGestureDetector : FSingleChildElement
    {
        /// <summary>
        /// Callback invoked when the user taps (clicks) inside this element's bounds.
        /// </summary>
        public Action? OnTap { get; }

        /// <summary>
        /// Callback invoked when the pointer enters this element's bounds.
        /// </summary>
        public Action? OnPointerEnter { get; }

        /// <summary>
        /// Callback invoked when the pointer exits this element's bounds.
        /// </summary>
        public Action? OnPointerExit { get; }

        /// <summary>
        /// Creates an <see cref="FGestureDetector"/> element.
        /// </summary>
        /// <param name="child">The child element whose area is monitored for gestures.</param>
        /// <param name="onTap">Callback invoked on tap/click.</param>
        /// <param name="onPointerEnter">Callback invoked when the pointer enters.</param>
        /// <param name="onPointerExit">Callback invoked when the pointer exits.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FGestureDetector(
            FElement child,
            Action? onTap = null,
            Action? onPointerEnter = null,
            Action? onPointerExit = null,
            FKey? key = null
        ) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
            OnTap = onTap;
            OnPointerEnter = onPointerEnter;
            OnPointerExit = onPointerExit;
        }
    }
}