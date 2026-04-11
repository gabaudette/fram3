#nullable enable
using System;

namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes the corner radii of a rounded rectangle.
    /// Each corner radius is a non-negative value in logical pixels.
    /// </summary>
    public readonly struct FBorderRadius : IEquatable<FBorderRadius>
    {
        /// <summary>Radius of the top-left corner.</summary>
        public float TopLeft { get; }

        /// <summary>Radius of the top-right corner.</summary>
        public float TopRight { get; }

        /// <summary>Radius of the bottom-right corner.</summary>
        public float BottomRight { get; }

        /// <summary>Radius of the bottom-left corner.</summary>
        public float BottomLeft { get; }

        /// <summary>
        /// Creates a border radius with individual values for each corner.
        /// </summary>
        /// <param name="topLeft">Radius of the top-left corner.</param>
        /// <param name="topRight">Radius of the top-right corner.</param>
        /// <param name="bottomRight">Radius of the bottom-right corner.</param>
        /// <param name="bottomLeft">Radius of the bottom-left corner.</param>
        public FBorderRadius(float topLeft, float topRight, float bottomRight, float bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomRight = bottomRight;
            BottomLeft = bottomLeft;
        }

        /// <summary>Creates a border radius with the same value on all four corners.</summary>
        /// <param name="all">The radius applied to every corner.</param>
        /// <returns>An <see cref="FBorderRadius"/> with equal values on all corners.</returns>
        public static FBorderRadius All(float all) => new(all, all, all, all);

        /// <summary>Creates a border radius with only the top corners rounded.</summary>
        /// <param name="value">The radius applied to the top-left and top-right corners.</param>
        /// <returns>An <see cref="FBorderRadius"/> with only the top corners set.</returns>
        public static FBorderRadius OnlyTop(float value) => new(value, value, 0f, 0f);

        /// <summary>Creates a border radius with only the bottom corners rounded.</summary>
        /// <param name="value">The radius applied to the bottom-left and bottom-right corners.</param>
        /// <returns>An <see cref="FBorderRadius"/> with only the bottom corners set.</returns>
        public static FBorderRadius OnlyBottom(float value) => new(0f, 0f, value, value);

        /// <summary>
        /// Creates a border radius with individual values for each corner, all defaulting to zero.
        /// Use named arguments to set only the corners you need.
        /// </summary>
        /// <param name="topLeft">Radius of the top-left corner.</param>
        /// <param name="topRight">Radius of the top-right corner.</param>
        /// <param name="bottomRight">Radius of the bottom-right corner.</param>
        /// <param name="bottomLeft">Radius of the bottom-left corner.</param>
        /// <returns>An <see cref="FBorderRadius"/> with the specified corner values.</returns>
        public static FBorderRadius Only(
            float topLeft = 0f,
            float topRight = 0f,
            float bottomRight = 0f,
            float bottomLeft = 0f
        ) => new(topLeft, topRight, bottomRight, bottomLeft);

        /// <summary>
        /// Creates a border radius rounding only the left-side corners with one value and
        /// the right-side corners with another.
        /// </summary>
        /// <param name="left">Radius applied to the top-left and bottom-left corners.</param>
        /// <param name="right">Radius applied to the top-right and bottom-right corners.</param>
        /// <returns>An <see cref="FBorderRadius"/> with asymmetric left and right radii.</returns>
        public static FBorderRadius Horizontal(float left = 0f, float right = 0f) =>
            new(left, right, right, left);

        /// <summary>
        /// Creates a border radius rounding only the top corners with one value and
        /// the bottom corners with another.
        /// </summary>
        /// <param name="top">Radius applied to the top-left and top-right corners.</param>
        /// <param name="bottom">Radius applied to the bottom-left and bottom-right corners.</param>
        /// <returns>An <see cref="FBorderRadius"/> with asymmetric top and bottom radii.</returns>
        public static FBorderRadius Vertical(float top = 0f, float bottom = 0f) =>
            new(top, top, bottom, bottom);

        /// <summary>Zero radius on all corners (sharp rectangle).</summary>
        public static FBorderRadius Zero => new(0f, 0f, 0f, 0f);

        /// <inheritdoc />
        public bool Equals(FBorderRadius other) =>
            TopLeft.Equals(other.TopLeft)
            && TopRight.Equals(other.TopRight)
            && BottomRight.Equals(other.BottomRight)
            && BottomLeft.Equals(other.BottomLeft);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is FBorderRadius other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(TopLeft, TopRight, BottomRight, BottomLeft);

        public static bool operator ==(FBorderRadius left, FBorderRadius right) => left.Equals(right);

        public static bool operator !=(FBorderRadius left, FBorderRadius right) => !left.Equals(right);

        /// <inheritdoc />
        public override string ToString() =>
            $"FBorderRadius(" +
            $"topLeft:{TopLeft}, " +
            $"topRight:{TopRight}," +
            $" bottomRight:{BottomRight}, " +
            $"bottomLeft:{BottomLeft}" +
            $")";
    }
}