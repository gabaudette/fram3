#nullable enable
namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes the visual style of a text label, including font size, color, weight, and decoration.
    /// </summary>
    /// <param name="FontSize">The font size in logical pixels. Null inherits from the parent.</param>
    /// <param name="Color">The text color. Null inherits from the parent.</param>
    /// <param name="Bold">Whether the text is bold.</param>
    /// <param name="Italic">Whether the text is italic.</param>
    /// <param name="Underline">Whether the text is underlined.</param>
    /// <param name="LetterSpacing">Additional spacing between characters in logical pixels.</param>
    /// <param name="LineHeight">Line height as a multiplier of the font size. Null uses the default.</param>
    public sealed record FTextStyle(
        float? FontSize = null,
        FColor? Color = null,
        bool Bold = false,
        bool Italic = false,
        bool Underline = false,
        float LetterSpacing = 0f,
        float? LineHeight = null
    )
    {
        /// <summary>A text style with no overrides, inheriting all values from the parent.</summary>
        public static FTextStyle Inherit => new FTextStyle();
    }
}
