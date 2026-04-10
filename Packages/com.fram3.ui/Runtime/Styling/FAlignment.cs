#nullable enable
using System;

namespace Fram3.UI.Styling
{
    /// <summary>
    /// Describes a 2D alignment position within a container.
    /// Values range from -1 (start) to 1 (end) on each axis, with 0 representing the center.
    /// </summary>
    public readonly struct FAlignment : IEquatable<FAlignment>
    {
        /// <summary>
        /// Horizontal alignment in the range [-1, 1].
        /// -1 is left, 0 is center, 1 is right.
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Vertical alignment in the range [-1, 1].
        /// -1 is top, 0 is center, 1 is bottom.
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Creates an alignment with specific horizontal and vertical values.
        /// </summary>
        /// <param name="x">Horizontal position in the range [-1, 1].</param>
        /// <param name="y">Vertical position in the range [-1, 1].</param>
        public FAlignment(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>Top-left corner.</summary>
        public static FAlignment TopLeft => new(-1f, -1f);

        /// <summary>Top-center.</summary>
        public static FAlignment TopCenter => new(0f, -1f);

        /// <summary>Top-right corner.</summary>
        public static FAlignment TopRight => new(1f, -1f);

        /// <summary>Center-left.</summary>
        public static FAlignment CenterLeft => new(-1f, 0f);

        /// <summary>Exact center.</summary>
        public static FAlignment Center => new(0f, 0f);

        /// <summary>Center-right.</summary>
        public static FAlignment CenterRight => new(1f, 0f);

        /// <summary>Bottom-left corner.</summary>
        public static FAlignment BottomLeft => new(-1f, 1f);

        /// <summary>Bottom-center.</summary>
        public static FAlignment BottomCenter => new(0f, 1f);

        /// <summary>Bottom-right corner.</summary>
        public static FAlignment BottomRight => new(1f, 1f);

        /// <inheritdoc />
        public bool Equals(FAlignment other) => X.Equals(other.X) && Y.Equals(other.Y);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is FAlignment other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(X, Y);

        /// <inheritdoc />
        public static bool operator ==(FAlignment left, FAlignment right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(FAlignment left, FAlignment right) => !left.Equals(right);

        /// <inheritdoc />
        public override string ToString() => $"FAlignment(x:{X}, y:{Y})";
    }
}