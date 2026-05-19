#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A floating panel that the user can drag freely within the root container.
    /// Attaches directly to the root like <see cref="Fram3.UI.Elements.Gesture.Modal"/>.
    /// Drag is handled entirely in the painter via pointer events on the title bar;
    /// position updates mutate native styles directly without triggering a framework rebuild.
    /// </summary>
    public sealed class DraggablePanel : SingleChildElement, IRootAttachedElement
    {
        /// <summary>Optional title displayed in the drag handle bar.</summary>
        public string? Title { get; }

        /// <summary>Initial horizontal position in logical pixels from the left of the root container.</summary>
        public float InitialX { get; }

        /// <summary>Initial vertical position in logical pixels from the top of the root container.</summary>
        public float InitialY { get; }

        /// <summary>
        /// Fixed width of the panel in logical pixels.
        /// When null the panel sizes to its content.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// Optional callback invoked when the user presses the close button.
        /// When null no close button is rendered.
        /// </summary>
        public Action? OnClose { get; }

        /// <summary>
        /// Creates a <see cref="DraggablePanel"/>.
        /// </summary>
        /// <param name="child">The content displayed inside the panel. Must not be null.</param>
        /// <param name="initialX">Initial left position in logical pixels.</param>
        /// <param name="initialY">Initial top position in logical pixels.</param>
        /// <param name="title">Optional title shown in the drag handle bar.</param>
        /// <param name="width">Optional fixed width. Null means size-to-content.</param>
        /// <param name="onClose">Optional close button callback.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public DraggablePanel(
            Element child,
            float initialX = 100f,
            float initialY = 100f,
            string? title = null,
            float? width = null,
            Action? onClose = null,
            Key? key = null
        ) : base(key)
        {
            if (child == null) throw new ArgumentNullException(nameof(child));
            Child = child;
            Title = title;
            InitialX = initialX;
            InitialY = initialY;
            Width = width;
            OnClose = onClose;
        }
    }
}
