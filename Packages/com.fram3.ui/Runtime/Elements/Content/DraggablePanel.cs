#nullable enable
using System;
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A floating panel that the user can drag freely within the root container.
    /// Attaches directly to the root like <see cref="Fram3.UI.Elements.Gesture.Modal"/>.
    /// Drag is handled entirely in the painter via pointer events on the title bar;
    /// position and size updates mutate native styles directly without triggering a
    /// framework rebuild.
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
        /// Initial width of the panel in logical pixels.
        /// When null the panel sizes to its content width.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// Initial height of the panel in logical pixels.
        /// When null the panel sizes to its content height.
        /// </summary>
        public float? Height { get; }

        /// <summary>
        /// Optional callback invoked when the user presses the close button.
        /// When null no close button is rendered.
        /// </summary>
        public Action? OnClose { get; }

        /// <summary>
        /// When true a resize grip is rendered in the bottom-right corner and the
        /// user can drag it to resize the panel.
        /// </summary>
        public bool Resizable { get; }

        /// <summary>Minimum width in logical pixels when resizable. Defaults to 120.</summary>
        public float MinWidth { get; }

        /// <summary>Maximum width in logical pixels when resizable. Defaults to float.MaxValue (unbounded).</summary>
        public float MaxWidth { get; }

        /// <summary>Minimum height in logical pixels when resizable. Defaults to 80.</summary>
        public float MinHeight { get; }

        /// <summary>Maximum height in logical pixels when resizable. Defaults to float.MaxValue (unbounded).</summary>
        public float MaxHeight { get; }

        /// <summary>
        /// Creates a <see cref="DraggablePanel"/>.
        /// </summary>
        /// <param name="child">The content displayed inside the panel. Must not be null.</param>
        /// <param name="initialX">Initial left position in logical pixels.</param>
        /// <param name="initialY">Initial top position in logical pixels.</param>
        /// <param name="title">Optional title shown in the drag handle bar.</param>
        /// <param name="width">Optional initial width. Null means size-to-content.</param>
        /// <param name="height">Optional initial height. Null means size-to-content.</param>
        /// <param name="onClose">Optional close button callback.</param>
        /// <param name="resizable">Whether a resize grip is shown. Defaults to false.</param>
        /// <param name="minWidth">Minimum width when resizable. Defaults to 120.</param>
        /// <param name="maxWidth">Maximum width when resizable. Defaults to unbounded.</param>
        /// <param name="minHeight">Minimum height when resizable. Defaults to 80.</param>
        /// <param name="maxHeight">Maximum height when resizable. Defaults to unbounded.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public DraggablePanel(
            Element child,
            float initialX = 100f,
            float initialY = 100f,
            string? title = null,
            float? width = null,
            float? height = null,
            Action? onClose = null,
            bool resizable = false,
            float minWidth = 120f,
            float maxWidth = float.MaxValue,
            float minHeight = 80f,
            float maxHeight = float.MaxValue,
            Key? key = null
        ) : base(key)
        {
            Child = child ?? throw new ArgumentNullException(nameof(child));
            Title = title;
            InitialX = initialX;
            InitialY = initialY;
            Width = width;
            Height = height;
            OnClose = onClose;
            Resizable = resizable;
            MinWidth = minWidth;
            MaxWidth = maxWidth;
            MinHeight = minHeight;
            MaxHeight = maxHeight;
        }
    }
}