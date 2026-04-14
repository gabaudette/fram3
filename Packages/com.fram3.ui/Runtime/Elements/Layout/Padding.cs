#nullable enable
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Insets its single child by the given <see cref="EdgeInsets"/>.
    /// Maps to padding applied directly to the child's containing <c>VisualElement</c>.
    /// </summary>
    public sealed class Padding : SingleChildElement
    {
        /// <summary>The amount of padding to apply on each edge.</summary>
        public EdgeInsets Insets { get; }

        /// <summary>
        /// Creates an <see cref="Padding"/> element.
        /// </summary>
        /// <param name="padding">The padding insets to apply.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Padding(EdgeInsets padding, Key? key = null) : base(key)
        {
            Insets = padding;
        }
    }
}