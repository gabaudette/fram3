#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A box with a fixed <see cref="Width"/> and/or <see cref="Height"/>.
    /// When used with a child, it constrains the child to those dimensions.
    /// When used without a child, it acts as a fixed-size empty space.
    /// </summary>
    public sealed class FSizedBox : FSingleChildElement
    {
        /// <summary>
        /// The explicit width in logical pixels. Null means unconstrained on the horizontal axis.
        /// </summary>
        public float? Width { get; }

        /// <summary>
        /// The explicit height in logical pixels. Null means unconstrained on the vertical axis.
        /// </summary>
        public float? Height { get; }

        /// <summary>
        /// When true, the element grows to fill all available space on both axes
        /// (equivalent to <c>flex-grow: 1</c>). <see cref="Width"/> and <see cref="Height"/>
        /// are ignored when this is true.
        /// </summary>
        public bool IsExpand { get; }

        private FSizedBox(float? width, float? height, bool isExpand, FKey? key) : base(key)
        {
            Width = width;
            Height = height;
            IsExpand = isExpand;
        }

        /// <summary>
        /// Creates an <see cref="FSizedBox"/> with explicit width and/or height.
        /// </summary>
        /// <param name="width">Width in logical pixels. Null means unconstrained.</param>
        /// <param name="height">Height in logical pixels. Null means unconstrained.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public static FSizedBox FromSize(float? width = null, float? height = null, FKey? key = null)
            => new(width, height, false, key);

        /// <summary>
        /// Creates a square <see cref="FSizedBox"/> with equal width and height.
        /// </summary>
        /// <param name="size">The width and height in logical pixels.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public static FSizedBox Square(float size, FKey? key = null)
            => new(size, size, false, key);

        /// <summary>
        /// Creates an <see cref="FSizedBox"/> that grows to fill all available space
        /// on both axes (equivalent to <c>flex-grow: 1</c>).
        /// </summary>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public static FSizedBox Expand(FKey? key = null)
            => new(null, null, true, key);
    }
}