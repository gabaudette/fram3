#nullable enable
using Fram3.UI.Core;

namespace Fram3.UI.Elements.Layout
{
    /// <summary>
    /// Wraps a single child and causes it to expand along the main axis of the
    /// parent flex container, proportional to its <see cref="Flex"/> factor.
    /// Maps to UIToolkit's <c>flexGrow</c> style property.
    /// </summary>
    public sealed class Expanded : SingleChildElement
    {
        /// <summary>
        /// The flex factor that controls how much of the remaining space this element
        /// claims relative to other <see cref="Expanded"/> siblings. Defaults to 1.
        /// </summary>
        public float Flex { get; }

        /// <summary>
        /// Creates an <see cref="Expanded"/> element.
        /// </summary>
        /// <param name="flex">The flex grow factor. Must be greater than zero. Defaults to 1.</param>
        /// <param name="key">An optional key for reconciliation identity.</param>
        public Expanded(float flex = 1f, Key? key = null) : base(key)
        {
            Flex = flex;
        }
    }
}