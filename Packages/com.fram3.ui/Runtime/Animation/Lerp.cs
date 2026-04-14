#nullable enable
using System;
using Fram3.UI.Styling;

namespace Fram3.UI.Animation
{
    /// <summary>
    /// Provides linear interpolation functions for all Fram3 styling types.
    /// Each method smoothly blends between a start and end value at a normalized
    /// position <c>t</c> in [0, 1]. Values of <c>t</c> outside [0, 1] are valid
    /// and produce extrapolation.
    /// </summary>
    public static class Lerp
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
        /// Linearly interpolates each RGBA channel between two <see cref="FrameColor"/> values.
        /// </summary>
        /// <param name="a">Start color.</param>
        /// <param name="b">End color.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated color.</returns>
        public static FrameColor Color(FrameColor a, FrameColor b, float t) =>
            new FrameColor(
                Float(a.R, b.R, t),
                Float(a.G, b.G, t),
                Float(a.B, b.B, t),
                Float(a.A, b.A, t)
            );

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="FrameColor"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start color.</param>
        /// <param name="b">End color.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated color, or null when both inputs are null.</returns>
        public static FrameColor? NullableColor(FrameColor? a, FrameColor? b, float t)
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
        /// Linearly interpolates each edge of two <see cref="EdgeInsets"/> values.
        /// </summary>
        /// <param name="a">Start insets.</param>
        /// <param name="b">End insets.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated insets.</returns>
        public static EdgeInsets EdgeInsets(EdgeInsets a, EdgeInsets b, float t) =>
            new EdgeInsets(
                Float(a.Top, b.Top, t),
                Float(a.Right, b.Right, t),
                Float(a.Bottom, b.Bottom, t),
                Float(a.Left, b.Left, t)
            );

        /// <summary>
        /// Linearly interpolates each axis of two <see cref="Alignment"/> values.
        /// </summary>
        /// <param name="a">Start alignment.</param>
        /// <param name="b">End alignment.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated alignment.</returns>
        public static Alignment Alignment(Alignment a, Alignment b, float t) =>
            new Alignment(Float(a.X, b.X, t), Float(a.Y, b.Y, t));

        /// <summary>
        /// Linearly interpolates each corner radius of two <see cref="BorderRadius"/> values.
        /// </summary>
        /// <param name="a">Start radius.</param>
        /// <param name="b">End radius.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border radius.</returns>
        public static BorderRadius BorderRadius(BorderRadius a, BorderRadius b, float t) =>
            new BorderRadius(
                Float(a.TopLeft, b.TopLeft, t),
                Float(a.TopRight, b.TopRight, t),
                Float(a.BottomRight, b.BottomRight, t),
                Float(a.BottomLeft, b.BottomLeft, t)
            );

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="BorderRadius"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start radius.</param>
        /// <param name="b">End radius.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border radius, or null when both inputs are null.</returns>
        public static BorderRadius? NullableBorderRadius(BorderRadius? a, BorderRadius? b, float t)
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
        /// Linearly interpolates between two <see cref="Border"/> values.
        /// </summary>
        /// <param name="a">Start border.</param>
        /// <param name="b">End border.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border.</returns>
        public static Border Border(Border a, Border b, float t) =>
            new Border(Color(a.Color, b.Color, t), Float(a.Width, b.Width, t));

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="Border"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start border.</param>
        /// <param name="b">End border.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated border, or null when both inputs are null.</returns>
        public static Border? NullableBorder(Border? a, Border? b, float t)
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
        /// Linearly interpolates between two <see cref="Shadow"/> values.
        /// </summary>
        /// <param name="a">Start shadow.</param>
        /// <param name="b">End shadow.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated shadow.</returns>
        public static Shadow Shadow(Shadow a, Shadow b, float t) =>
            new Shadow(
                Color(a.Color, b.Color, t),
                Float(a.OffsetX, b.OffsetX, t),
                Float(a.OffsetY, b.OffsetY, t),
                Float(a.BlurRadius, b.BlurRadius, t)
            );

        /// <summary>
        /// Linearly interpolates between two nullable <see cref="Shadow"/> values.
        /// Returns null when both are null. Snaps to the non-null side if one is null.
        /// </summary>
        /// <param name="a">Start shadow.</param>
        /// <param name="b">End shadow.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated shadow, or null when both inputs are null.</returns>
        public static Shadow? NullableShadow(Shadow? a, Shadow? b, float t)
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
        /// Linearly interpolates between two <see cref="BoxDecoration"/> values,
        /// blending each component independently.
        /// </summary>
        /// <param name="a">Start decoration.</param>
        /// <param name="b">End decoration.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated box decoration.</returns>
        public static BoxDecoration BoxDecoration(BoxDecoration a, BoxDecoration b, float t) =>
            new BoxDecoration(
                NullableColor(a.Color, b.Color, t),
                NullableBorder(a.Border, b.Border, t),
                NullableBorderRadius(a.BorderRadius, b.BorderRadius, t),
                NullableShadow(a.Shadow, b.Shadow, t)
            );

        /// <summary>
        /// Linearly interpolates numeric properties of two <see cref="TextStyle"/> values.
        /// Boolean properties (Bold, Italic, Underline) are taken from <paramref name="b"/>
        /// when <paramref name="t"/> is greater than or equal to 0.5, otherwise from
        /// <paramref name="a"/>.
        /// </summary>
        /// <param name="a">Start text style.</param>
        /// <param name="b">End text style.</param>
        /// <param name="t">Normalized position in [0, 1].</param>
        /// <returns>The interpolated text style.</returns>
        public static TextStyle TextStyle(TextStyle a, TextStyle b, float t)
        {
            var useBSide = t >= 0.5f;
            return new TextStyle(
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