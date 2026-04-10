#nullable enable
namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes the visual decoration of a box, including background color, border, corner radii, and shadow.
    /// Used with container elements such as <c>FContainer</c>.
    /// </summary>
    /// <param name="Color">The background fill color. Null means no fill.</param>
    /// <param name="Border">The border drawn around the box. Null means no border.</param>
    /// <param name="BorderRadius">The corner radii of the box. Null means sharp corners.</param>
    /// <param name="Shadow">A drop shadow cast by the box. Null means no shadow.</param>
    public sealed record FBoxDecoration(
        FColor? Color = null,
        FBorder? Border = null,
        FBorderRadius? BorderRadius = null,
        FShadow? Shadow = null
    )
    {
        /// <summary>A decoration with no fill, no border, no radius, and no shadow.</summary>
        public static FBoxDecoration None => new();
    }
}