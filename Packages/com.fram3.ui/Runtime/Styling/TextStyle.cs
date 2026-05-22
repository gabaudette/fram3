#nullable enable
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
using UnityEngine;
using UnityEngine.TextCore.Text;
#endif

namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes the visual style of a text label, including font size, color, weight, decoration,
    /// alignment, and font asset.
    /// </summary>
    /// <since>2.0.0-beta.1</since>
    /// <status>live</status>
    /// <param name="FontSize">The font size in logical pixels. Null inherits from the parent.</param>
    /// <param name="Color">The text color. Null inherits from the parent.</param>
    /// <param name="Bold">Whether the text is bold.</param>
    /// <param name="Italic">Whether the text is italic.</param>
    /// <param name="Underline">Whether the text is underlined.</param>
    /// <param name="LetterSpacing">Additional spacing between characters in logical pixels.</param>
    /// <param name="LineHeight">Line height as a multiplier of the font size. Null uses the default.</param>
    /// <param name="TextAlign">
    /// Horizontal and vertical text alignment within the label bounds.
    /// Null inherits from the parent. Maps to <c>style.unityTextAlign</c>.
    /// </param>
    /// <param name="FontAsset">
    /// SDF font asset used for rendering. Null inherits from the parent.
    /// Maps to <c>style.unityFontDefinition</c> via <c>FontDefinition.FromSDFFont</c>.
    /// </param>
    /// <param name="ResetPadding">
    /// When true, zeroes out the UIToolkit Label's built-in padding and margin.
    /// Use when the label is inside a flex container that handles its own centering,
    /// to prevent intrinsic Label padding from offsetting the visual result.
    /// </param>
    public sealed record TextStyle(
        float? FontSize = null,
        FrameColor? Color = null,
        bool Bold = false,
        bool Italic = false,
        bool Underline = false,
        float LetterSpacing = 0f,
        float? LineHeight = null,
        bool ResetPadding = false,
#if !FRAM3_PURE_TESTS && !FRAM3_DOC_BUILD
        TextAnchor? TextAlign = null,
        FontAsset? FontAsset = null
#else
        int? TextAlign = null,
        object? FontAsset = null
#endif
    )
    {
        /// <summary>A text style with no overrides, inheriting all values from the parent.</summary>
        public static TextStyle Inherit => new();

        /// <summary>Returns a copy of this style with <see cref="Bold"/> set to <c>true</c>.</summary>
        public TextStyle AsBold() => this with { Bold = true };

        /// <summary>Returns a copy of this style with <see cref="Italic"/> set to <c>true</c>.</summary>
        public TextStyle AsItalic() => this with { Italic = true };

        /// <summary>Returns a copy of this style with <see cref="Underline"/> set to <c>true</c>.</summary>
        public TextStyle AsUnderlined() => this with { Underline = true };

        /// <summary>Returns a copy of this style with the given <see cref="FontSize"/>.</summary>
        /// <param name="size">The font size in logical pixels.</param>
        public TextStyle WithSize(float size) => this with { FontSize = size };

        /// <summary>Returns a copy of this style with the given <see cref="Color"/>.</summary>
        /// <param name="color">The text color.</param>
        public TextStyle WithColor(FrameColor color) => this with { Color = color };

        /// <summary>Returns a copy of this style with the given <see cref="LetterSpacing"/>.</summary>
        /// <param name="spacing">Additional spacing between characters in logical pixels.</param>
        public TextStyle WithLetterSpacing(float spacing) => this with { LetterSpacing = spacing };

        /// <summary>Returns a copy of this style with the given <see cref="LineHeight"/>.</summary>
        /// <param name="lineHeight">Line height as a multiplier of the font size.</param>
        public TextStyle WithLineHeight(float lineHeight) => this with { LineHeight = lineHeight };
    }
}
