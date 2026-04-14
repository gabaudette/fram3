#nullable enable
using System;
using Fram3.UI.Core;
using Fram3.UI.Styling;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Applies an outer margin to its single child by the given <see cref="EdgeInsets"/>.
    /// Maps to margin applied directly to the child's containing <c>VisualElement</c>.
    /// </summary>
    public sealed class Margin : SingleChildElement
    {
        /// <summary>The amount of margin to apply on each edge.</summary>
        public EdgeInsets Insets { get; }

        /// <summary>
        /// Creates an <see cref="Margin"/> element.
        /// </summary>
        /// <param name="margin">The margin insets to apply.</param>
        /// <param name="child">The child element to apply margin to. Must not be null.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="child"/> is null.</exception>
        public Margin(EdgeInsets margin, Element child, Key? key = null) : base(key)
        {
            Insets = margin;
            Child = child ?? throw new ArgumentNullException(nameof(child));
        }
    }
}