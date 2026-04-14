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
        /// Creates an <see cref="Container"/> element.
        /// </summary>
        /// <param name="decoration">Optional visual decoration.</param>
        /// <param name="width">Optional explicit width in logical pixels.</param>
        /// <param name="height">Optional explicit height in logical pixels.</param>
        /// <param name="padding">Optional inner padding.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Container(
            BoxDecoration? decoration = null,
            float? width = null,
            float? height = null,
            EdgeInsets? padding = null,
            Key? key = null
        ) : base(key)
        {
            Decoration = decoration;
            Width = width;
            Height = height;
            Padding = padding;
        }

        /// <summary>
        /// Returns the child element when one was set, or an empty list when used
        /// as a styled box without content.
        /// </summary>
        public override IReadOnlyList<Element> GetChildren()
        {
            return HasChild ? base.GetChildren() : Array.Empty<Element>();
        }
    }
}