#nullable enable
using System;
using Fram3.UI.Styling;

namespace Fram3.UI.Animation
{
    /// <summary>
    /// Provides linear interpolation functions for all Fram3 styling types.
    /// Each method smoothly blends from <paramref name="a"/> to <paramref name="b"/>
    /// at the normalized position <paramref name="t"/> in [0, 1].
    /// Values of <paramref name="t"/> outside [0, 1] are valid and produce extrapolation.
    /// </summary>
    public static class FLerp
    {
        /// <summary>
        /// Linearly interpolates between two <see cref="float"/> values.
        /// </summary>
        /// <param name="a">Start value.</param>
        /// <param name="b">End value.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated value.</returns>
        public static float Float(float a, float b, float t) => a + (b - a) * t;

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="float"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start value.</param>
        /// <param name="b">End value.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated value, or null when both inputs are null.</returns>
        public static float? NullableFloat(float? a, float? b, float t)
        {
            if (a == null && b == null)
            {
                return null;
            }

            var from = a ?? b!.Value;
            var to = b ?? a!.Value;
            return Float(from, to, t);
        }

        /// <summary>
        /// Linearly interpolates each RGBA channel between two <see cref="FColor"/> values.
        /// </summary>
        /// <param name="a">Start color.</param>
        /// <param name="b">End color.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated color.</returns>
        public static FColor Color(FColor a, FColor b, float t) =>
            new FColor(
                Float(a.R, b.R, t),
                Float(a.G, b.G, t),
                Float(a.B, b.B, t),
                Float(a.A, b.A, t)
            );

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="FColor"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start color.</param>
        /// <param name="b">End color.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated color, or null when both inputs are null.</returns>
        public static FColor? NullableColor(FColor? a, FColor? b, float t)
        {
            if (a == null && b == null)
            {
                return null;
            }

            var from = a ?? b!.Value;
            var to = b ?? a!.Value;
            return Color(from, to, t);
        }

        /// <summary>
        /// Linearly interpolates each edge of two <see cref="FEdgeInsets"/> values.
        /// </summary>
        /// <param name="a">Start insets.</param>
        /// <param name="b">End insets.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated insets.</returns>
        public static FEdgeInsets EdgeInsets(FEdgeInsets a, FEdgeInsets b, float t) =>
            new FEdgeInsets(
                Float(a.Top, b.Top, t),
                Float(a.Right, b.Right, t),
                Float(a.Bottom, b.Bottom, t),
                Float(a.Left, b.Left, t)
            );

        /// <summary>
        /// Linearly interpolates each axis of two <see cref="FAlignment"/> values.
        /// </summary>
        /// <param name="a">Start alignment.</param>
        /// <param name="b">End alignment.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated alignment.</returns>
        public static FAlignment Alignment(FAlignment a, FAlignment b, float t) =>
            new FAlignment(Float(a.X, b.X, t), Float(a.Y, b.Y, t));

        /// <summary>
        /// Linearly interpolates each corner radius of two <see cref="FBorderRadius"/> values.
        /// </summary>
        /// <param name="a">Start radius.</param>
        /// <param name="b">End radius.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border radius.</returns>
        public static FBorderRadius BorderRadius(FBorderRadius a, FBorderRadius b, float t) =>
            new FBorderRadius(
                Float(a.TopLeft, b.TopLeft, t),
                Float(a.TopRight, b.TopRight, t),
                Float(a.BottomRight, b.BottomRight, t),
                Float(a.BottomLeft, b.BottomLeft, t)
            );

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="FBorderRadius"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start radius.</param>
        /// <param name="b">End radius.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border radius, or null when both inputs are null.</returns>
        public static FBorderRadius? NullableBorderRadius(FBorderRadius? a, FBorderRadius? b, float t)
        {
            if (a == null && b == null)
            {
                return null;
            }

            var from = a ?? b!.Value;
            var to = b ?? a!.Value;
            return BorderRadius(from, to, t);
        }

        /// <summary>
        /// Linearly interpolates between two <see cref="FBorder"/> values.
        /// </summary>
        /// <param name="a">Start border.</param>
        /// <param name="b">End border.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border.</returns>
        public static FBorder Border(FBorder a, FBorder b, float t) =>
            new FBorder(Color(a.Color, b.Color, t), Float(a.Width, b.Width, t));

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="FBorder"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start border.</param>
        /// <param name="b">End border.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border, or null when both inputs are null.</returns>
        public static FBorder? NullableBorder(FBorder? a, FBorder? b, float t)
        {
            if (a == null && b == null)
            {
                return null;
            }

            var from = a ?? b!;
            var to = b ?? a!;
            return Border(from, to, t);
        }

        /// <summary>
        /// Linearly interpolates between two <see cref="FShadow"/> values.
        /// </summary>
        /// <param name="a">Start shadow.</param>
        /// <param name="b">End shadow.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated shadow.</returns>
        public static FShadow Shadow(FShadow a, FShadow b, float t) =>
            new FShadow(
                Color(a.Color, b.Color, t),
                Float(a.OffsetX, b.OffsetX, t),
                Float(a.OffsetY, b.OffsetY, t),
                Float(a.BlurRadius, b.BlurRadius, t)
            );

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="FShadow"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start shadow.</param>
        /// <param name="b">End shadow.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated shadow, or null when both inputs are null.</returns>
        public static FShadow? NullableShadow(FShadow? a, FShadow? b, float t)
        {
            if (a == null && b == null)
            {
                return null;
            }

            var from = a ?? b!;
            var to = b ?? a!;
            return Shadow(from, to, t);
        }

        /// <summary>
        /// Linearly interpolates between two <see cref="FBoxDecoration"/> values,
        /// blending each component independently.
        /// </summary>
        /// <param name="a">Start decoration.</param>
        /// <param name="b">End decoration.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated box decoration.</returns>
        public static FBoxDecoration BoxDecoration(FBoxDecoration a, FBoxDecoration b, float t) =>
            new FBoxDecoration(
                NullableColor(a.Color, b.Color, t),
                NullableBorder(a.Border, b.Border, t),
                NullableBorderRadius(a.BorderRadius, b.BorderRadius, t),
                NullableShadow(a.Shadow, b.Shadow, t)
            );

        /// <summary>
        /// Linearly interpolates numeric properties of two <see cref="FTextStyle"/> values.
        /// Boolean properties (Bold, Italic, Underline) are taken from <paramref name="b"/>
        /// when <paramref name="t"/> is greater than or equal to 0.5, otherwise from
        /// <paramref name="a"/>.
        /// </summary>
        /// <param name="a">Start text style.</param>
        /// <param name="b">End text style.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated text style.</returns>
        public static FTextStyle TextStyle(FTextStyle a, FTextStyle b, float t)
        {
            var useBSide = t >= 0.5f;
            return new FTextStyle(
                NullableFloat(a.FontSize, b.FontSize, t),
                NullableColor(a.Color, b.Color, t),
                useBSide ? b.Bold : a.Bold,
                useBSide ? b.Italic : a.Italic,
                useBSide ? b.Underline : a.Underline,
                Float(a.LetterSpacing, b.LetterSpacing, t),
                NullableFloat(a.LineHeight, b.LineHeight, t)
            );
        }
    }
}
