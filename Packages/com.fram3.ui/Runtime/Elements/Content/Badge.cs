#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// Overlays a small colored pip on the top-right corner of its child element.
    /// When <see cref="Count"/> is provided the pip displays that number as a label,
    /// capped at 99+. When <see cref="Count"/> is null a compact dot is rendered instead.
    /// The pip bleeds outside the child's bounds so it is visible over the corner.
    /// Typically used on inventory slots, ability icons, minimap markers, and avatar thumbnails
    /// to communicate notifications, item quantity, or status.
    /// </summary>
    /// <status>live</status>
    public sealed class Badge : SingleChildElement
    {
        /// <summary>
        /// The numeric value displayed inside the pip.
        /// Null renders a compact dot with no label.
        /// </summary>
        public int? Count { get; }

        /// <summary>
        /// The background color of the pip.
        /// Null defaults to the theme's <see cref="Styling.Theme.ErrorColor"/>.
        /// </summary>
        public FrameColor? Color { get; }

        /// <summary>
        /// Creates a <see cref="Badge"/> element.
        /// </summary>
        /// <param name="child">The element to attach the badge to.</param>
        /// <param name="count">
        /// The count to display inside the pip. Null renders a dot badge.
        /// </param>
        /// <param name="color">
        /// The pip background color. Null uses the theme's error color.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Badge(
            Element child,
            int? count = null,
            FrameColor? color = null,
            Key? key = null
        ) : base(key)
        {
            Child = child;
            Count = count;
            Color = color;
        }
    }
}
