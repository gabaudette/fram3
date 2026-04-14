#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// A thin horizontal or vertical separator line with no children.
    /// Maps to a UIToolkit <c>VisualElement</c> with a fixed thickness,
    /// a background color, and an optional margin.
    /// </summary>
    public sealed class Divider : LeafElement
    {
        /// <summary>
        /// The axis along which the divider is drawn.
        /// </summary>
        public DividerAxis Axis { get; }

        /// <summary>
        /// The thickness of the divider line in logical pixels. Defaults to 1.
        /// </summary>
        public float Thickness { get; }

        /// <summary>
        /// The color of the divider line. Null uses the inherited foreground color.
        /// </summary>
        public FrameColor? Color { get; }

        /// <summary>
        /// Space added on each side perpendicular to the divider axis. Defaults to 0.
        /// </summary>
        public float Indent { get; }

        /// <summary>
        /// Creates an <see cref="Divider"/> element.
        /// </summary>
        /// <param name="axis">The draw axis. Defaults to <see cref="DividerAxis.Horizontal"/>.</param>
        /// <param name="thickness">Line thickness in logical pixels. Defaults to 1.</param>
        /// <param name="color">Line color. Null means no explicit color is applied.</param>
        /// <param name="indent">Perpendicular indent in logical pixels. Defaults to 0.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Divider(
            DividerAxis axis = DividerAxis.Horizontal,
            float thickness = 1f,
            FrameColor? color = null,
            float indent = 0f,
            Key? key = null
        ) : base(key)
        {
            Axis = axis;
            Thickness = thickness;
            Color = color;
            Indent = indent;
        }
    }

    /// <summary>
    /// Specifies the draw axis for an <see cref="Divider"/>.
    /// </summary>
    public enum DividerAxis
    {
        /// <summary>A horizontal line spanning the full available width.</summary>
        Horizontal,

        /// <summary>A vertical line spanning the full available height.</summary>
        Vertical
    }
}