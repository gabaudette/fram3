namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes a uniform border with a color and width applied to all four sides of an element.
    /// </summary>
    /// <param name="Color">The color of the border.</param>
    /// <param name="Width">The width of the border in logical pixels.</param>
    public sealed record FBorder(FColor Color, float Width)
    {
        /// <summary>A border with zero width and transparent color.</summary>
        public static FBorder None => new FBorder(FColor.Transparent, 0f);
    }
}
