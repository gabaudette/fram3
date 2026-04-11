#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements
{
    /// <summary>
    /// A scrollable container that wraps a single child. Maps to UIToolkit's
    /// <c>ScrollView</c> native element. The child scrolls vertically by default;
    /// use <see cref="ScrollDirection"/> to change the scroll axis.
    /// </summary>
    public sealed class FScrollView : FSingleChildElement
    {
        /// <summary>
        /// The axis along which the content scrolls.
        /// </summary>
        public FScrollDirection ScrollDirection { get; }

        /// <summary>
        /// Creates an <see cref="FScrollView"/> element.
        /// </summary>
        /// <param name="scrollDirection">
        /// The scroll axis. Defaults to <see cref="FScrollDirection.Vertical"/>.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FScrollView(
            FScrollDirection scrollDirection = FScrollDirection.Vertical,
            FKey? key = null
        ) : base(key)
        {
            ScrollDirection = scrollDirection;
        }
    }

    /// <summary>
    /// Specifies the scrolling axis for <see cref="FScrollView"/>.
    /// </summary>
    public enum FScrollDirection
    {
        /// <summary>Content scrolls along the vertical axis.</summary>
        Vertical,
        /// <summary>Content scrolls along the horizontal axis.</summary>
        Horizontal,
        /// <summary>Content scrolls along both axes.</summary>
        Both
    }
}
