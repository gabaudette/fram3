#nullable enable
using System;

namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes insets along the four edges of a rectangle, used for padding and margins.
    /// </summary>
    public readonly struct FEdgeInsets : IEquatable<FEdgeInsets>
    {
        /// <summary>Inset from the top edge.</summary>
        public float Top { get; }

        /// <summary>Inset from the right edge.</summary>
        public float Right { get; }

        /// <summary>Inset from the bottom edge.</summary>
        public float Bottom { get; }

        /// <summary>Inset from the left edge.</summary>
        public float Left { get; }

        /// <summary>
        /// Creates insets with individual values for each edge.
        /// </summary>
        /// <param name="top">Inset from the top edge.</param>
        /// <param name="right">Inset from the right edge.</param>
        /// <param name="bottom">Inset from the bottom edge.</param>
        /// <param name="left">Inset from the left edge.</param>
        public FEdgeInsets(float top, float right, float bottom, float left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        /// <summary>
        /// Creates insets with the same value on all four edges.
        /// </summary>
        /// <param name="all">The inset value applied to every edge.</param>
        /// <returns>An <see cref="FEdgeInsets"/> with equal values on all edges.</returns>
        public static FEdgeInsets All(float all) => new(all, all, all, all);

        /// <summary>
        /// Creates insets with equal top/bottom and equal left/right values.
        /// </summary>
        /// <param name="vertical">Inset applied to the top and bottom edges.</param>
        /// <param name="horizontal">Inset applied to the left and right edges.</param>
        /// <returns>An <see cref="FEdgeInsets"/> with symmetric vertical and horizontal values.</returns>
        public static FEdgeInsets Symmetric(float vertical, float horizontal) =>
            new(vertical, horizontal, vertical, horizontal);

        /// <summary>
        /// Creates insets with only the top edge set; all other edges are zero.
        /// </summary>
        /// <param name="value">The top inset value.</param>
        public static FEdgeInsets OnlyTop(float value) => new(value, 0f, 0f, 0f);

        /// <summary>
        /// Creates insets with only the right edge set; all other edges are zero.
        /// </summary>
        /// <param name="value">The right inset value.</param>
        public static FEdgeInsets OnlyRight(float value) => new(0f, value, 0f, 0f);

        /// <summary>
        /// Creates insets with only the bottom edge set; all other edges are zero.
        /// </summary>
        /// <param name="value">The bottom inset value.</param>
        public static FEdgeInsets OnlyBottom(float value) => new(0f, 0f, value, 0f);

        /// <summary>
        /// Creates insets with only the left edge set; all other edges are zero.
        /// </summary>
        /// <param name="value">The left inset value.</param>
        public static FEdgeInsets OnlyLeft(float value) => new(0f, 0f, 0f, value);

        /// <summary>Zero insets on all edges.</summary>
        public static FEdgeInsets Zero => new(0f, 0f, 0f, 0f);

        /// <summary>The total horizontal inset (left + right).</summary>
        public float Horizontal => Left + Right;

        /// <summary>The total vertical inset (top + bottom).</summary>
        public float Vertical => Top + Bottom;

        /// <inheritdoc />
        public bool Equals(FEdgeInsets other) =>
            Top.Equals(other.Top) && Right.Equals(other.Right) &&
            Bottom.Equals(other.Bottom) && Left.Equals(other.Left);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is FEdgeInsets other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Top, Right, Bottom, Left);

        /// <inheritdoc />
        public static bool operator ==(FEdgeInsets left, FEdgeInsets right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(FEdgeInsets left, FEdgeInsets right) => !left.Equals(right);

        /// <inheritdoc />
        public override string ToString() =>
            $"FEdgeInsets(top:{Top}, right:{Right}, bottom:{Bottom}, left:{Left})";
    }
}