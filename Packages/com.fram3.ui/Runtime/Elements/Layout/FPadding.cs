#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Insets its single child by the given <see cref="FEdgeInsets"/>.
    /// Maps to padding applied directly to the child's containing <c>VisualElement</c>.
    /// </summary>
    public sealed class FPadding : FSingleChildElement
    {
        /// <summary>The amount of padding to apply on each edge.</summary>
        public FEdgeInsets Padding { get; }

        /// <summary>
        /// Creates an <see cref="FPadding"/> element.
        /// </summary>
        /// <param name="padding">The padding insets to apply.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public FPadding(FEdgeInsets padding, FKey? key = null) : base(key)
        {
            Padding = padding;
        }
    }
}