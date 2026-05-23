#nullable enable
namespace Fram3.UI.Styling
{
    /// <summary>
    /// <status>live</status>
    /// Describes the visual decoration of a box, including background color, border, corner radii, and shadow.
    /// Used with container elements such as <c>Container</c>.
    /// </summary>
    /// <since>2.0.0-beta.1</since>
    /// <status>live</status>
    /// <param name="Color">The background fill color. Null means no fill.</param>
    /// <param name="Border">The border drawn around the box. Null means no border.</param>
    /// <param name="BorderRadius">The corner radii of the box. Null means sharp corners.</param>
    /// <param name="Shadow">A drop shadow cast by the box. Null means no shadow.</param>
    public sealed record BoxDecoration(
        FrameColor? Color = null,
        Border? Border = null,
        BorderRadius? BorderRadius = null,
        Shadow? Shadow = null
    )
    {
        /// <summary>A decoration with no fill, no border, no radius, and no shadow.</summary>
        public static BoxDecoration None => new();
    }
}