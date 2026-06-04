#nullable enable
using System;
using System.Collections.Generic;
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// A general-purpose container element that combines decoration, sizing, and
    /// padding in a single node. Maps to a UIToolkit <c>VisualElement</c> with
    /// decoration, size constraints, and padding applied directly to its style.
    /// When used without a child, it renders as a styled box with no content.
    /// </summary>
    /// <status>live</status>
    public sealed class Container : SingleChildElement
    {
        /// <summary>
        /// The visual decoration applied to this container (background color, border,
        /// corner radius, shadow). Null means no decoration.
        /// </summary>
        public BoxDecoration? Decoration { get; }

        /// <summary>
        /// The explicit width in logical pixels. Null means unconstrained on the horizontal axis.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// The explicit height in logical pixels. Null means unconstrained on the vertical axis.
        /// </summary>
        public float? Height { get; }

        /// <summary>
        /// Inner padding applied between the container boundary and its child.
        /// Null means no padding.
        /// </summary>
        public EdgeInsets? Padding { get; }

        /// <summary>
        /// When true, the container centers its child along both axes using
        /// flex alignment. Equivalent to wrapping the child in a <c>Center</c>
        /// but applied directly on this element's native node.
        /// </summary>
        public bool CenterChild { get; }

        /// <summary>
        /// Optional tap callback. When set, the container's native element registers
        /// a pointer-down handler that invokes this action. Use this instead of wrapping
        /// in a <see cref="Fram3.UI.Elements.Gesture.GestureDetector"/> when the tap
        /// handler must not introduce an extra layout node.
        /// </summary>
        public Action? OnTap { get; }

        /// <summary>
        /// Creates an <see cref="Container"/> element.
        /// </summary>
        /// <param name="decoration">Optional visual decoration.</param>
        /// <param name="width">Optional explicit width in logical pixels.</param>
        /// <param name="height">Optional explicit height in logical pixels.</param>
        /// <param name="padding">Optional inner padding.</param>
        /// <param name="centerChild">When true, centers the child along both axes.</param>
        /// <param name="onTap">Optional tap callback registered directly on the native element.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Container(
            BoxDecoration? decoration = null,
            float? width = null,
            float? height = null,
            EdgeInsets? padding = null,
            bool centerChild = false,
            Action? onTap = null,
            Key? key = null
        ) : base(key)
        {
            Decoration = decoration;
            Width = width;
            Height = height;
            Padding = padding;
            CenterChild = centerChild;
            OnTap = onTap;
        }

        /// <summary>
        /// Returns the child element when one was set, or an empty list when used
        /// as a styled box without content.
        /// </summary>
        /// <inheritdoc/>
        public override bool ShouldRebuild(Element oldEl, Element newEl)
        {
            var o = (Container)oldEl;
            var n = (Container)newEl;
            return o.Decoration != n.Decoration
                || o.Width != n.Width
                || o.Height != n.Height
                || o.Padding != n.Padding
                || o.CenterChild != n.CenterChild
                || o.OnTap != n.OnTap;
        }

        /// <inheritdoc/>
        public override IReadOnlyList<Element> GetChildren()
        {
            return HasChild ? base.GetChildren() : Array.Empty<Element>();
        }
    }
}