#nullable enable
namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes a drop shadow cast by an element.
    /// </summary>
    /// <param name="Color">The color of the shadow.</param>
    /// <param name="OffsetX">Horizontal offset of the shadow in logical pixels. Positive values move the shadow right.</param>
    /// <param name="OffsetY">Vertical offset of the shadow in logical pixels. Positive values move the shadow down.</param>
    /// <param name="BlurRadius">The blur radius in logical pixels. A value of 0 produces a sharp shadow.</param>
    public sealed record Shadow(FrameColor Color, float OffsetX, float OffsetY, float BlurRadius)
    {
        /// <summary>Creates a shadow with no offset and no blur.</summary>
        /// <param name="color">The shadow color.</param>
        /// <param name="blur">The blur radius.</param>
        /// <returns>A centered <see cref="Shadow"/> with the given color and blur.</returns>
        public static Shadow Ambient(FrameColor color, float blur) => new(color, 0f, 0f, blur);
    }
}