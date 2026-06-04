#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Gesture
{
    /// <summary>
    /// A transparent wrapper that detects pointer gestures on its child subtree.
    /// Maps to a plain UIToolkit <c>VisualElement</c> with registered pointer event callbacks.
    /// </summary>
    /// <status>live</status>
    public sealed class GestureDetector : SingleChildElement
    {
        /// <summary>
        /// Callback invoked when the user taps (clicks) inside this element's bounds.
        /// </summary>
        public Action? OnTap { get; }

        /// <summary>
        /// Callback invoked when the user double-taps (double-clicks) inside this element's bounds.
        /// </summary>
        public Action? OnDoubleTap { get; }

        /// <summary>
        /// Callback invoked when the pointer is held down for 500 ms without releasing.
        /// </summary>
        public Action? OnLongPress { get; }

        /// <summary>
        /// Callback invoked when the pointer enters this element's bounds.
        /// </summary>
        public Action? OnPointerEnter { get; }

        /// <summary>
        /// Callback invoked when the pointer exits this element's bounds.
        /// </summary>
        public Action? OnPointerExit { get; }

        /// <summary>
        /// Callback invoked when the user right-clicks (secondary button down) inside this
        /// element's bounds. Receives the pointer position in root-container logical pixels
        /// as <c>(x, y)</c>. Use this to open a <see cref="Fram3.UI.Elements.Content.ContextMenu"/>
        /// at the reported coordinates.
        /// </summary>
        public Action<float, float>? OnSecondaryTap { get; }

        /// <summary>
        /// Creates a <see cref="GestureDetector"/> element.
        /// </summary>
        /// <param name="child">The child element whose area is monitored for gestures.</param>
        /// <param name="onTap">Callback invoked on a single tap/click.</param>
        /// <param name="onDoubleTap">Callback invoked on a double-tap/double-click.</param>
        /// <param name="onLongPress">Callback invoked when the pointer is held for 500 ms.</param>
        /// <param name="onPointerEnter">Callback invoked when the pointer enters.</param>
        /// <param name="onPointerExit">Callback invoked when the pointer exits.</param>
        /// <param name="onSecondaryTap">Callback invoked on right-click; receives root-space (x, y).</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public GestureDetector(
            Element child,
            Action? onTap = null,
            Action? onDoubleTap = null,
            Action? onLongPress = null,
            Action? onPointerEnter = null,
            Action? onPointerExit = null,
            Action<float, float>? onSecondaryTap = null,
            Key? key = null
        ) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
            OnTap = onTap;
            OnDoubleTap = onDoubleTap;
            OnLongPress = onLongPress;
            OnPointerEnter = onPointerEnter;
            OnPointerExit = onPointerExit;
            OnSecondaryTap = onSecondaryTap;
        }

        /// <inheritdoc/>
        public override bool ShouldRebuild(Element oldEl, Element newEl)
        {
            var o = (GestureDetector)oldEl;
            var n = (GestureDetector)newEl;
            return o.OnTap != n.OnTap
                || o.OnDoubleTap != n.OnDoubleTap
                || o.OnLongPress != n.OnLongPress
                || o.OnPointerEnter != n.OnPointerEnter
                || o.OnPointerExit != n.OnPointerExit
                || o.OnSecondaryTap != n.OnSecondaryTap;
        }
    }
}