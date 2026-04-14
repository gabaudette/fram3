#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Content
{
    /// <summary>
    /// A scrollable container that wraps a single child. Maps to UIToolkit's
    /// <c>ScrollView</c> native element. The child scrolls vertically by default;
    /// use <see cref="ScrollDirection"/> to change the scroll axis.
    /// </summary>
    public sealed class ScrollView : SingleChildElement
    {
        /// <summary>
        /// The axis along which the content scrolls.
        /// </summary>
        public ScrollDirection ScrollDirection { get; }

        /// <summary>
        /// Creates an <see cref="ScrollView"/> element.
        /// </summary>
        /// <param name="scrollDirection">
        /// The scroll axis. Defaults to <see cref="ScrollDirection.Vertical"/>.
        /// </param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public ScrollView(
            ScrollDirection scrollDirection = ScrollDirection.Vertical,
            Key? key = null
        ) : base(key)
        {
            ScrollDirection = scrollDirection;
        }
    }

    /// <summary>
    /// Specifies the scrolling axis for <see cref="ScrollView"/>.
    /// </summary>
    public enum ScrollDirection
    {
        /// <summary>Content scrolls along the vertical axis.</summary>
        Vertical,
        /// <summary>Content scrolls along the horizontal axis.</summary>
        Horizontal,
        /// <summary>Content scrolls along both axes.</summary>
        Both
    }
}
