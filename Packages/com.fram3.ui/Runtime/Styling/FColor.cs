#nullable enable
using System;
using System.Globalization;

namespace Fram3.UI.Styling
{
    /// <summary>
    /// An immutable RGBA color with each channel normalized to the range [0, 1].
    /// </summary>
    public readonly struct FColor : IEquatable<FColor>
    {
        /// <summary>Red channel in the range [0, 1].</summary>
        public float R { get; }

        /// <summary>Green channel in the range [0, 1].</summary>
        public float G { get; }

        /// <summary>Blue channel in the range [0, 1].</summary>
        public float B { get; }

        /// <summary>Alpha channel in the range [0, 1]. 0 is fully transparent, 1 is fully opaque.</summary>
        public float A { get; }

        /// <summary>
        /// Creates a color from normalized RGBA channel values.
        /// </summary>
        /// <param name="r">Red channel in the range [0, 1].</param>
        /// <param name="g">Green channel in the range [0, 1].</param>
        /// <param name="b">Blue channel in the range [0, 1].</param>
        /// <param name="a">Alpha channel in the range [0, 1]. Defaults to 1 (fully opaque).</param>
        public FColor(float r, float g, float b, float a = 1f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <summary>
        /// Creates a color from 8-bit per channel RGBA values in the range [0, 255].
        /// </summary>
        /// <param name="r">Red channel in the range [0, 255].</param>
        /// <param name="g">Green channel in the range [0, 255].</param>
        /// <param name="b">Blue channel in the range [0, 255].</param>
        /// <param name="a">Alpha channel in the range [0, 255]. Defaults to 255 (fully opaque).</param>
        /// <returns>A new <see cref="FColor"/> with normalized channel values.</returns>
        public static FColor FromRgba255(byte r, byte g, byte b, byte a = 255)
        {
            return new FColor(r / 255f, g / 255f, b / 255f, a / 255f);
        }

        /// <summary>
        /// Creates a color from a hex string in the format <c>#RRGGBB</c> or <c>#RRGGBBAA</c>.
        /// The leading <c>#</c> is optional.
        /// </summary>
        /// <param name="hex">The hex color string.</param>
        /// <returns>A new <see cref="FColor"/> parsed from the hex string.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="hex"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="hex"/> is not a valid 6- or 8-digit hex color string.
        /// </exception>
        public static FColor FromHex(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException(nameof(hex));
            }

            var cleaned = hex.TrimStart('#');
            if (cleaned.Length != 6 && cleaned.Length != 8)
            {
                throw new ArgumentException(
                    $"Hex color string must be 6 or 8 digits (RRGGBB or RRGGBBAA), got '{hex}'.",
                    nameof(hex)
                );
            }

            if (!uint.TryParse(cleaned, NumberStyles.HexNumber, null, out var value))
            {
                throw new ArgumentException(
                    $"Hex color string contains invalid characters: '{hex}'.",
                    nameof(hex)
                );
            }

            if (cleaned.Length == 6)
            {
                var r = (byte)((value >> 16) & 0xFF);
                var g = (byte)((value >> 8) & 0xFF);
                var b = (byte)(value & 0xFF);
                return FromRgba255(r, g, b);
            }
            else
            {
                var r = (byte)((value >> 24) & 0xFF);
                var g = (byte)((value >> 16) & 0xFF);
                var b = (byte)((value >> 8) & 0xFF);
                var a = (byte)(value & 0xFF);
                return FromRgba255(r, g, b, a);
            }
        }

        /// <summary>
        /// Returns a copy of this color with the alpha channel replaced by <paramref name="a"/>.
        /// </summary>
        /// <param name="a">The new alpha value in the range [0, 1].</param>
        /// <returns>A new <see cref="FColor"/> with the updated alpha channel.</returns>
        public FColor WithAlpha(float a) => new(R, G, B, a);

        /// <summary>Fully transparent black.</summary>
        public static FColor Transparent => new(0f, 0f, 0f, 0f);

        /// <summary>Opaque white.</summary>
        public static FColor White => new(1f, 1f, 1f);

        /// <summary>Opaque black.</summary>
        public static FColor Black => new(0f, 0f, 0f);

        /// <summary>Opaque red.</summary>
        public static FColor Red => new(1f, 0f, 0f);

        /// <summary>Opaque green.</summary>
        public static FColor Green => new(0f, 1f, 0f);

        /// <summary>Opaque blue.</summary>
        public static FColor Blue => new(0f, 0f, 1f);

        /// <inheritdoc />
        public bool Equals(FColor other) =>
            R.Equals(other.R) && G.Equals(other.G) && B.Equals(other.B) && A.Equals(other.A);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is FColor other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(R, G, B, A);

        /// <inheritdoc />
        public static bool operator ==(FColor left, FColor right) => left.Equals(right);

        /// <inheritdoc />
        public static bool operator !=(FColor left, FColor right) => !left.Equals(right);

        /// <inheritdoc />
        public override string ToString() => $"FColor(r:{R:F3}, g:{G:F3}, b:{B:F3}, a:{A:F3})";
    }
}